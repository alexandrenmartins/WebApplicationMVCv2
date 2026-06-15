using Microsoft.AspNetCore.Mvc;
using WebApplicationMVCv2.Data;
using WebApplicationMVCv2.Models;

namespace WebApplicationMVCv2.Controllers
{
    public class FinanceiroController : Controller
    {
        private readonly AppDbContext _dbContext;
        public FinanceiroController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var lancamentos = _dbContext.Lancamentos.ToList();

            ViewBag.TotalReceitas = lancamentos.Where(l => l.Tipo == "Receita").Sum(l => l.Valor);
            ViewBag.TotalDespesas = lancamentos.Where(l => l.Tipo == "Despesa").Sum(l => l.Valor);
            ViewBag.Saldo = ViewBag.TotalReceitas - ViewBag.TotalDespesas;

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
                _dbContext.Lancamentos.Add(lancamento);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lancamento);
        }

        public IActionResult DeletarLancamentoView(int id)
        {
            //var lancamento = _dbContext.Lancamentos.Find(id);
            var lancamento = _dbContext.Lancamentos.FirstOrDefault(l => l.Id == id);
            if (lancamento == null)
                return NotFound();

            return View(lancamento);
        }

        [HttpPost]
        public IActionResult ConfirmaDeletarLancamento(int id)
        {
            var lancamento = _dbContext.Lancamentos.FirstOrDefault(l => l.Id == id);
            if (lancamento == null)
                return NotFound();
            _dbContext.Lancamentos.Remove(lancamento);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
