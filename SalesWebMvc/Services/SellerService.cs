﻿using SalesWebMvc.Models;
using SalesWebMvc.Data;
using NuGet.Protocol.Plugins;
using SalesWebMvc.Models.ViewModels;

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
    }
}
