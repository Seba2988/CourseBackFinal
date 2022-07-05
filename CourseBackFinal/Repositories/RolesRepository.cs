using Microsoft.AspNetCore.Identity;
namespace CourseBackFinal.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private RoleManager<IdentityRole> _roleManager;
        public RolesRepository(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<string?> CreateRole(string roleName)
        {
            IdentityRole role = new()
            {
                Name = roleName
            };
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return role.Id;
            }
            else return null;
        }
    }
}
