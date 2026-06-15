using System.ComponentModel.DataAnnotations;

namespace WebApplicationMVCv2.Models;

public class Lancamento
{
    public int Id { get; set; }
    [Required(ErrorMessage = "A descrição é obrigatória.")]
    public string Descricao { get; set; } = string.Empty;
    [Required(ErrorMessage = "O valor é obrigatório.")]
    public decimal Valor { get; set; }
    [Required]
    public string Tipo { get; set; }
    [Required(ErrorMessage = "A categoria é obrigatória.")]
    public string Categoria { get; set; }
    [Required(ErrorMessage = "A data é obrigatória.")]
    public DateTime Data { get; set; }
}
