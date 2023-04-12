using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interfaces.IRepository.IGenericRepository;
using Core.Models;

namespace Infrastructure.Interfaces.IUnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Train> Trains { get; }
        public Task<int> Save();
    }
}