using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Data.Context;
using Api.Domain.Entities;
using Api.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {

        protected readonly MyContext _context;
        private DbSet<T> _dataset;

        public BaseRepository(MyContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();    
        }    

        //Verifica se ID é existente na base de dados.
        public async Task<bool> ExistAsync (Guid id) {
            return await _dataset.AnyAsync(p => p.ID.Equals (id));
        }
        
        public async Task<bool> DeleteAsync(Guid id)
        {
            try {
                
              var result = await _dataset.SingleOrDefaultAsync(p => p.ID.Equals(id));

                if (result == null ) {
                    return false;
                }

               _dataset.Remove(result);

               await _context.SaveChangesAsync(); 

               return true; 

            } catch (Exception ex ) {
                throw ex;
            }


        }

        public async Task<T> InsertAsync(T item)
        {
            try 
            {
                if (item.ID == Guid.Empty ){
                   item.ID = Guid.NewGuid();  
                }

                item.CreateAt = DateTime.UtcNow;

                _dataset.Add(item);

                await _context.SaveChangesAsync();


            } catch (Exception ex) {
                throw ex;
            }         

            return item;
        }

        public async Task<T> SelectAsync(Guid id)
        {
            try {

                return await _dataset.SingleOrDefaultAsync (p => p.ID.Equals(id));

            } catch (Exception ex) {
                throw ex;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
           try {
               return await _dataset.ToListAsync();
           } catch (Exception ex) {
               throw ex;
           }
        }

        public async Task<T> UpdateAsync(T item)
        {
            try {

                // Localiza o Registro existente no BD
                var result = await _dataset.SingleOrDefaultAsync(p => p.ID.Equals(item.ID));

                if (result == null ) {
                    return null;
                }

                item.UpdateAt = DateTime.UtcNow;

                item.CreateAt = result.CreateAt;

                _context.Entry(result).CurrentValues.SetValues(item);

                await _context.SaveChangesAsync();



            } catch (Exception ex) {
                throw ex;
            }


            return item;
        }
    }
}