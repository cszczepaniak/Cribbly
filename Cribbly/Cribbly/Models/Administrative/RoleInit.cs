using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Cribbly.Models
{
    public class RoleInit
    {
        RoleManager<IdentityRole> _rolemanager;

        public async Task CreateRoles()
        {
            if (!await _rolemanager.RoleExistsAsync("Admin"))
            {
                var role = new IdentityRole("Admin");
                await _rolemanager.CreateAsync(role);
            }

            if (!await _rolemanager.RoleExistsAsync("User"))
            {
                var role = new IdentityRole("User");
                await _rolemanager.CreateAsync(role);
            }
        }
        public RoleInit(RoleManager<IdentityRole> roleManager)
        {
            _rolemanager = roleManager;
        }

    }
}
