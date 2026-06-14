using Microsoft.AspNetCore.Mvc;
using WebApplicationMVCv2.Data;

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
            ViewBag.TotalReceitas = lancamentos.Where(l => l.Tipo == "Despesa").Sum(l => l.Valor);
            ViewBag.Saldo = ViewBag.TotalReceitas - ViewBag.TotalDespesas;

            return View(lancamentos);
        }
    }
}
