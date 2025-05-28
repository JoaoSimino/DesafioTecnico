namespace DesafioTecnico1.Model
{
    //public record Cliente(Guid Id, string Email, string Telefone, DateTime DataCadastro); para dtos
    public class Cliente 
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    }
}
