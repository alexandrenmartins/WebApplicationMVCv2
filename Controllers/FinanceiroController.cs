using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplicationMVCv2.Data;
using WebApplicationMVCv2.Models;

namespace WebApplicationMVCv2.Controllers
{
    public class FinanceiroController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly AppPostgresContext _postgresContext;

        private const string SessionKeyDb = "postgres";

        private AppDbContext ActiveContext =>
            HttpContext.Session.GetString(SessionKeyDb) == "sqlserver"
                ? _dbContext
                : null!;

        private AppPostgresContext ActivePostgresContext =>
            HttpContext.Session.GetString(SessionKeyDb) == "sqlserver"
                ? null!
                : _postgresContext;

        public FinanceiroController(AppDbContext dbContext, AppPostgresContext postgresContext)
        {
            _dbContext = dbContext;
            _postgresContext = postgresContext;
        }

        private List<Lancamento> GetLancamentos()
        {
            if (HttpContext.Session.GetString(SessionKeyDb) == "sqlserver")
                return _dbContext.Lancamentos.ToList();
            return _postgresContext.Lancamentos.ToList();
        }

        private void SaveLancamento(Lancamento lancamento)
        {
            if (HttpContext.Session.GetString(SessionKeyDb) == "sqlserver")
                _dbContext.Lancamentos.Add(lancamento);
            else
                _postgresContext.Lancamentos.Add(lancamento);
        }

        private void RemoveLancamento(Lancamento lancamento)
        {
            if (HttpContext.Session.GetString(SessionKeyDb) == "sqlserver")
                _dbContext.Lancamentos.Remove(lancamento);
            else
                _postgresContext.Lancamentos.Remove(lancamento);
        }

        private Lancamento? FindLancamento(int id)
        {
            if (HttpContext.Session.GetString(SessionKeyDb) == "sqlserver")
                return _dbContext.Lancamentos.FirstOrDefault(l => l.Id == id);
            return _postgresContext.Lancamentos.FirstOrDefault(l => l.Id == id);
        }

        private void SaveChanges()
        {
            if (HttpContext.Session.GetString(SessionKeyDb) == "sqlserver")
                _dbContext.SaveChanges();
            else
                _postgresContext.SaveChanges();
        }

        private bool TestarConexao(DbContext context)
        {
            try
            {
                return context.Database.CanConnect();
            }
            catch
            {
                return false;
            }
        }

        public IActionResult ToggleDatabase(string db)
        {
            var context = db == "sqlserver" ? (DbContext)_dbContext : _postgresContext;

            if (!TestarConexao(context))
            {
                TempData["ErroBanco"] = "Banco de dados indisponível. Mantendo conexão atual.";
                return RedirectToAction("Index");
            }

            HttpContext.Session.SetString(SessionKeyDb, db);
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            List<Lancamento> lancamentos;

            try
            {
                lancamentos = GetLancamentos();
            }
            catch
            {
                TempData["ErroBanco"] = "Não foi possível conectar ao banco de dados selecionado.";
                lancamentos = new List<Lancamento>();
            }

            ViewBag.TotalReceitas = lancamentos.Where(l => l.Tipo == "Receita").Sum(l => l.Valor);
            ViewBag.TotalDespesas = lancamentos.Where(l => l.Tipo == "Despesa").Sum(l => l.Valor);
            ViewBag.Saldo = ViewBag.TotalReceitas - ViewBag.TotalDespesas;
            ViewBag.SelectedDb = HttpContext.Session.GetString(SessionKeyDb) ?? "postgres";

            return View(lancamentos);
        }

        public IActionResult CriarLancamentoView()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CriarLancamentoView(Lancamento lancamento)
        {
            if (ModelState.IsValid)
            {
                SaveLancamento(lancamento);
                SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lancamento);
        }

        public IActionResult DeletarLancamentoView(int id)
        {
            var lancamento = FindLancamento(id);
            if (lancamento == null)
                return NotFound();

            return View(lancamento);
        }

        [HttpPost]
        public IActionResult ConfirmaDeletarLancamento(int id)
        {
            var lancamento = FindLancamento(id);
            if (lancamento == null)
                return NotFound();
            RemoveLancamento(lancamento);
            SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
