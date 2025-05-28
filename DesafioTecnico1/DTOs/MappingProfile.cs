using AutoMapper;
using DesafioTecnico1.DTOsç;
using DesafioTecnico1.Model;
using Microsoft.Extensions.Hosting;

namespace DesafioTecnico1.DTOs
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<ClienteDto, Cliente>();
            CreateMap<ProdutoDto, Produto>();
            CreateMap<PedidoDtoCadastro, Pedido>();
            CreateMap<ItemPedidoDto,ItemPedido>();
            CreateMap<PedidoAlterarStatusDto, Pedido>();
        }
    }
}
