
using System;
using System.Threading.Tasks;
using Core.Interfaces.IRepository.IGenericRepository;
using Core.Models;
using Infrastructure.ApplicationContext;
using Infrastructure.Interfaces.IUnitOfWork;
using Infrastructure.Repository.GenericRepository;

namespace OnlineShopWebAPIs.BusinessLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly BookingSystemApplicationContext _context;

        public IGenericRepository<Train> Trains { get; }

        public UnitOfWork(BookingSystemApplicationContext context)
        {
            _context = context;

             Trains = new GenericRepository<Train>(context); 

        }


        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);    
        }

        public async Task<int> Save()
        {
            return await _context.SaveChangesAsync();
        }
    }
}

