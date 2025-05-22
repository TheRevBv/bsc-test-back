using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using BSC.Domain.Entities;
using BSC.Infrastructure.Persistences.Contexts;
using BSC.Infrastructure.Persistences.Interfaces;
using System.Data;

namespace BSC.Infrastructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BSCContext _context;
        private readonly IConfiguration _configuration;
        public IUsuarioRepository _usuario = null!;
        public IGenericRepository<Producto> _producto = null!;

        public UnitOfWork(BSCContext context, IConfiguration configuration)
        {
            _context = context;
            // Set the command timeout to 30 seconds
            _context.Database.SetCommandTimeout(30);
            _configuration = configuration;

        }

        public IUsuarioRepository Usuario => _usuario ?? new UsuarioRepository(_context);
        public IGenericRepository<Producto> Producto => _producto ?? new GenericRepository<Producto>(_context);


        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}