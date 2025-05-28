using MediatR;

namespace DesafioTecnico1.Event;

public class PedidoCanceladoEvent : INotification
{
    public Guid PedidoId { get; }

    public PedidoCanceladoEvent(Guid pedidoId)
    {
        PedidoId = pedidoId;
    }
}
