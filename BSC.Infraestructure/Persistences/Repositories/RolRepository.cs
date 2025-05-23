using Microsoft.EntityFrameworkCore;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;

namespace BSC.Infrastructure.Persistences.Repositories
{
    public class RolUsuarioRepository(BSCContext context) : IRolUsuarioRepository
    {
        private readonly BSCContext _context = context;

        public async Task<IEnumerable<RolUsuario>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.RolesUsuario
                .Where(r => r.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task AddRangeAsync(IEnumerable<RolUsuario> entidades)
        {
            await _context.RolesUsuario.AddRangeAsync(entidades);
        }

        public void RemoveRange(IEnumerable<RolUsuario> entidades)
        {
            _context.RolesUsuario.RemoveRange(entidades);
        }
    }

}