using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesafioTecnico1.Migrations
{
    /// <inheritdoc />
    public partial class CriandorelaçãoentreClienteePedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Produto_Pedido_PedidoId",
                table: "Produto");

            migrationBuilder.DropIndex(
                name: "IX_Produto_PedidoId",
                table: "Produto");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "Produto");

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoId",
                table: "ItemPedido",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pedido_ClientId",
                table: "Pedido",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemPedido_PedidoId",
                table: "ItemPedido",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedido_Cliente_ClientId",
                table: "Pedido",
                column: "ClientId",
                principalTable: "Cliente",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItemPedido_Pedido_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedido_Cliente_ClientId",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_Pedido_ClientId",
                table: "Pedido");

            migrationBuilder.DropIndex(
                name: "IX_ItemPedido_PedidoId",
                table: "ItemPedido");

            migrationBuilder.DropColumn(
                name: "PedidoId",
                table: "ItemPedido");

            migrationBuilder.AddColumn<Guid>(
                name: "PedidoId",
                table: "Produto",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Produto_PedidoId",
                table: "Produto",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Produto_Pedido_PedidoId",
                table: "Produto",
                column: "PedidoId",
                principalTable: "Pedido",
                principalColumn: "Id");
        }
    }
}
