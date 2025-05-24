using Microsoft.EntityFrameworkCore;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;

namespace BSC.Infrastructure.Persistences.Repositories
{
    public class UsuarioRepository(BSCContext context) : GenericRepository<Usuario>(context), IUsuarioRepository
    {
        private readonly BSCContext _context = context;

        public async Task<Usuario> UsuarioByCorreo(string email)
        {
            var user = await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Correo!.Equals(email));
            return user!;
        }

        public new async Task<Usuario> GetByIdAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.RolesUsuario)
                    .ThenInclude(ru => ru.Rol)
                .FirstOrDefaultAsync(u => u.Id == id && u.Estado == (int)StateTypes.Active);

            return usuario!;
        }
    }
}