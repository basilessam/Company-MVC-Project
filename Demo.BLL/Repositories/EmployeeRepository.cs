using Demo.BLL.Interfaces;
using Demo.DAL.Contexts;
using Demo.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        

        public EmployeeRepository(MVCAppContext dbcontext):base(dbcontext)
        {
           
        }

        public IQueryable<Employee> GetEmployeesByAddress(string address)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Employee> GetEmployeesByName(string name)
        =>_dbcontext.Employees.Where(E=>E.Name.ToLower().Contains(name.ToLower()));
        
    }
}
