using SalesWebMvc.Models;
using SalesWebMvc.Data;
using NuGet.Protocol.Plugins;
using SalesWebMvc.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.ToListAsync();
        }

        public async Task InsertAsync(Seller obj)
        {
            var department = await _context.Department.FirstOrDefaultAsync(d => d.Id == obj.DepartmentId);
            if (department == null)
            {
                throw new NotFoundException("Department not found");
            }

            obj.Department = department;
            _context.Add(obj);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FindByIdAsync(int id)
        {
            var seller = await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(d => d.Id == id);

            if (seller == null)
            {
                throw new NotFoundException($"Seller with id {id} was not found");
            }

            return seller;
        }

        public async Task RemoveAsync(int id)
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id);

                if (obj != null)
                {
                    _context.Seller.Remove(obj);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Cannot delete this seller because he/she has sales");
            }
        }


        public async Task UpdateAsync(Seller obj)
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if (!hasAny)
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                await _context.SaveChangesAsync();
            }
            catch (DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
