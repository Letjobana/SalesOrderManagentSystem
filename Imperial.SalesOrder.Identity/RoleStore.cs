using Imperial.SalesOrder.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.Identity
{
    class RoleStore<TRole> : IRoleStore<TRole, int> where TRole : IdentityRole
    {
        private Repository repo;

        //Create a new RoleStore with the Repository using the Default Connection String
        public RoleStore()
        {
            repo = new Repository();
        }

        //Create a new RoleStore with the specified Repository
        public RoleStore(Repository repository)
        {
            repo = repository;
        }

        public Task CreateAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(TRole role)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByIdAsync(int roleId)
        {
            throw new NotImplementedException();
        }

        public Task<TRole> FindByNameAsync(string roleName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(TRole role)
        {
            throw new NotImplementedException();
        }
    }
}
