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
    public class DepartmentRepository :GenericRepository<Department> ,IDepartmentRepository
    {
        private  readonly MVCAppContext _dbcontext;

        public DepartmentRepository(MVCAppContext dbcontext):base(dbcontext)
        {
            _dbcontext = dbcontext;
        }

    }
}
