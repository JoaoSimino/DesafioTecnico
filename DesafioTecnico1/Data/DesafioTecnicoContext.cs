using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DesafioTecnico1.Model;

namespace DesafioTecnico1.Data
{
    public class DesafioTecnicoContext : DbContext
    {
        public DesafioTecnicoContext(DbContextOptions<DesafioTecnicoContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedido>()
                .HasOne(p => p.Cliente)
                .WithMany(c => c.Pedidos)
                .HasForeignKey(p => p.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany()  // Produto não tem coleção de ItemPedido.
                .HasForeignKey(ip => ip.ProdutoId)
                .IsRequired();  // reforça que é obrigatório

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(ip => ip.PedidoId)
                .IsRequired();
        }

        public DbSet<DesafioTecnico1.Model.Cliente> Cliente { get; set; } = default!;
        public DbSet<DesafioTecnico1.Model.Pedido> Pedido { get; set; } = default!;
        public DbSet<DesafioTecnico1.Model.Produto> Produto { get; set; } = default!;
        public DbSet<DesafioTecnico1.Model.ItemPedido> ItemPedido { get; set; } = default!;
    }
}
