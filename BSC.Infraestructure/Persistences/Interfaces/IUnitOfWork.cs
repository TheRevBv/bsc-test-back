using BSC.Domain.Entities;
using System.Data;

namespace BSC.Infrastructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaración o matricula de nuestra interfaces a nivel de repository
        IUsuarioRepository Usuario { get; }
        IGenericRepository<Producto> Producto { get; }
        IGenericRepository<Pedido> Pedido { get; }
        IGenericRepository<Rol> Rol { get; }
        public IRolUsuarioRepository RolUsuario { get; }


        void SaveChanges();
        Task SaveChangesAsync();
        IDbTransaction BeginTransaction();
    }
}