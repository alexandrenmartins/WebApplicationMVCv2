namespace WebApplicationMVCv2.Models;

public class Lancamento
{
    public int Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public string Tipo { get; set; }
    public string Categoria { get; set; }
    public DateTime Data { get; set; }
}
