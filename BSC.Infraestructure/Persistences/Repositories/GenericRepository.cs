using Microsoft.EntityFrameworkCore;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;
using BSC.Utilities.Static;
using System.Linq.Expressions;

namespace BSC.Infrastructure.Persistences.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly BSCContext _context;
        private readonly DbSet<T> _entity;

        public GenericRepository(BSCContext context)
        {
            _context = context;
            _entity = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            var getAll = await _entity
                .Where(x => x.Estado != (int)StateTypes.Inactive)
                .AsNoTracking()
                .ToListAsync();

            return getAll;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var getById = await _entity!.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));

            return getById!;
        }

        public async Task<bool> RegisterAsync(T entity)
        {
            entity.UsuarioAltaId = 1;
            entity.FechaAlta = DateTime.Now;

            await _context.AddAsync(entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<bool> EditAsync(T entity)
        {
            entity.UsuarioModId = 1;
            entity.FechaMod = DateTime.Now;

            _context.Update(entity);

            _context.Entry(entity).Property(x => x.UsuarioAltaId).IsModified = false;
            _context.Entry(entity).Property(x => x.FechaAlta).IsModified = false;

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<bool> RemoveAsync(int id)
        {
            T entity = await GetByIdAsync(id);

            entity!.UsuarioModId = 1;
            entity.FechaMod = DateTime.Now;
            entity.Estado = 0;

            _context.Update(entity);

            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public IQueryable<T> GetEntityQuery(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = _entity;

            if (filter != null) query = query.Where(filter);

            return query;
        }

        public IQueryable<T> GetAllQueryable()
        {
            var getAllQuery = GetEntityQuery();
            return getAllQuery;
        }

        public async Task<IEnumerable<T>> GetSelectAsync()
        {
            var getAll = await _entity
                .Where(x => x.Estado
                    .Equals((int)StateTypes.Active))
                .AsNoTracking()
                .ToListAsync();

            return getAll;
        }
    }
}