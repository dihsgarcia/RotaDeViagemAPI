namespace RotaDeViagemAPI.Models
{
    public class RotaDeViagem
    {
        public int Id { get; set; }
        public string Origem { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public int ValorViagemInt { get; set; }
    }

    public class RotaDeViagemResponse
    {
        public string Rota { get; set; } = string.Empty;
        public int ValorViagemInt { get; set; }     
    }
}
