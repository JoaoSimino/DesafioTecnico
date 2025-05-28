using static DesafioTecnico1.Model.StatusPedido;

namespace DesafioTecnico1.Model
{
    //public record Pedido(Guid Id, Guid ClientId, DateTime DataPedido, StatusPedidoEnum Status, List<Produto> Itens); para dtos
    public class Pedido 
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public DateTime DataPedido { get; set; }
        public StatusPedidoEnum Status { get; set; }
        public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();

        public Cliente Cliente { get; set; } = null!;

    }
}
