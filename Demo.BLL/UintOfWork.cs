﻿using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL
{
    public class UintOfWork : IUintOfWork
    {
        private readonly MVCAppContext _dbcontext;

        public IEmployeeRepository EmployeeRepository { get; set ; }
        public IDepartmentRepository DepartmentRepository { get ; set ; }
        public UintOfWork(MVCAppContext dbcontext)
        {
            EmployeeRepository = new EmployeeRepository(dbcontext);
            DepartmentRepository=new DepartmentRepository(dbcontext);
            _dbcontext = dbcontext;
        }

        public async Task<int> Complete()
        {
            return await _dbcontext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbcontext.Dispose();
        }

    
    }
}
