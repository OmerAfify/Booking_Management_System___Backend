﻿using System;
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
        public IGenericRepository<Route> Routes { get; }
        public IGenericRepository<Schedule> Schedules { get; }
        public Task<int> Save();
    }
}