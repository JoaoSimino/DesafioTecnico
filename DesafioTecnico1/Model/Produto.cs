namespace DesafioTecnico1.Model
{
    //public record Produto(Guid Id, string Nome, string Descricao, decimal Preco, int Estoque); para dtos
    public class Produto 
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }

    }
}
