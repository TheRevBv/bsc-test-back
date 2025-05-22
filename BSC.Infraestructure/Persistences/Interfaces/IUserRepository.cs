using BSC.Domain.Entities;

namespace BSC.Infrastructure.Persistences.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<Usuario> UsuarioByCorreo(string email);
    }
}