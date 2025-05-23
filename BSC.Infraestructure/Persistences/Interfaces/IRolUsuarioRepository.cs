using BSC.Domain.Entities;

namespace BSC.Infrastructure.Persistences.Interfaces;

public interface IRolUsuarioRepository
{
    Task<IEnumerable<RolUsuario>> GetByUsuarioIdAsync(int usuarioId);
    Task AddRangeAsync(IEnumerable<RolUsuario> entidades);
    void RemoveRange(IEnumerable<RolUsuario> entidades);
}
