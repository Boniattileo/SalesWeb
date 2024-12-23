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

        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }

        public void Insert(Seller obj)
        {
            obj.Department = _context.Department.FirstOrDefault(d => d.Id == obj.DepartmentId);
            _context.Add(obj);
            _context.SaveChanges();
        }

        public Seller FindById(int id)
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(d => d.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id);

            if (obj != null)
            {
                _context.Seller.Remove(obj);
                _context.SaveChanges();
            }
        }

        public void Update(Seller obj)
        {
            if (!_context.Seller.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id not found");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbConcurrencyException e) 
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
