using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace NSE.WebApp.MVC.Models
{
    public class EnderecoViewModel
    {
        [Required]
        [DisplayName("Número")]
        public string Numero { get; set; }

        [Required]
        [DisplayName("CEP")]
        public string Cep { get; set; }

        [Required]
        public string Logradouro { get; set; }

        [Required]
        public string Bairro { get; set; }

        [Required]
        public string Cidade { get; set; }

        [Required]
        public string Estado { get; set; }

        public string Complemento { get; set; }

        public override string ToString()
        {
            return $"{Logradouro}, {Numero} {Complemento} - {Bairro} - {Cidade} - {Estado}";
        }
    }
}