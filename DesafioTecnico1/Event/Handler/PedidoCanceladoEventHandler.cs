using DesafioTecnico1.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DesafioTecnico1.Event.Handler;

public class PedidoCanceladoEventHandler : INotificationHandler<PedidoCanceladoEvent>
{
    private readonly DesafioTecnicoContext _db;

    public PedidoCanceladoEventHandler(DesafioTecnicoContext db)
    {
        _db = db;
    }

    public async Task Handle(PedidoCanceladoEvent notification, CancellationToken cancellationToken)
    {
        var pedido = await _db.Pedido
            .Include(p => p.Itens)
            .FirstOrDefaultAsync(p => p.Id == notification.PedidoId);

        if (pedido == null) return;

        foreach (var item in pedido.Itens)
        {
            var produto = await _db.Produto.FirstOrDefaultAsync(prod => prod.Id == item.ProdutoId);
            if (produto != null)
            {
                produto.Estoque += item.Quantidade;
            }
            item.Quantidade = 0;
        }

        await _db.SaveChangesAsync();
    }
}

