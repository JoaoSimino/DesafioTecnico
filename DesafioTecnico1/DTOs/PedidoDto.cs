using DesafioTecnico1.Model;
using static DesafioTecnico1.Model.StatusPedido;

namespace DesafioTecnico1.DTOs;

public record PedidoDtoCadastro(Guid ClientId, DateTime DataPedido, IEnumerable<Produto> Itens);
