using DAL.Data;
using DAL.Entities;
using DAL.Repositories.AbstractRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.ConcreteRepositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity, new()
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _entities;
        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }
        public async Task<T> AddAsync(T entity)
        {   
            
            await _entities.AddAsync(entity);             
            await _context.SaveChangesAsync();

            return entity;

        }

        public async Task DeleteAsync(int id)
        {
            var deletedItem = await  _entities.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedItem == null)
            {
                throw new KeyNotFoundException("Böyle bir içerik bulunamadı");
            }
            else
            {
                _entities.Remove(deletedItem);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _entities.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
           return await _entities.FindAsync(id);
        }
        public async Task UpdateAsync(int id, T entity)
        {

            var updatedItem = await _entities.FindAsync(id);

            if(updatedItem != null)
            {
                entity.UpdatedDate = DateTime.Now;
                _entities.Update(updatedItem);
                await _context.SaveChangesAsync();                
            }
            else
            {
                throw new KeyNotFoundException($"Key {id} does not exists.");
            }
        }
        public async Task<IEnumerable<T>> WhereAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Where(predicate).ToListAsync();
        }
    }
}
