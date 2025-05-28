namespace DesafioTecnico1.Model
{
    public class ItemPedido
    {
        public Guid Id { get; set; }
        public Guid ProdutoId { get; set; }
        public Guid PedidoId { get; set; }
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }

        public Produto Produto { get; set; } = null!;
        public Pedido Pedido { get; set; } = null!;

    }
}
