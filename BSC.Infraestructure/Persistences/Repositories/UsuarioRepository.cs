using Microsoft.EntityFrameworkCore;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;

namespace BSC.Infrastructure.Persistences.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        private readonly BSCContext _context;
        public UsuarioRepository(BSCContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Usuario> UsuarioByCorreo(string email)
        {
            var user = await _context.Usuarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Correo!.Equals(email));
            return user!;
        }
    }
}