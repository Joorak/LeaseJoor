

using Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Services
{

    public class RoleService : IRoleService
    {
        public RoleService(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager
            //, IMapper mapper
            )
        {
            this.UserManager = userManager;
            this.RoleManager = roleManager;
            //this.Mapper = mapper;
        }


        private UserManager<AppUser> UserManager { get; }


        private RoleManager<AppRole> RoleManager { get; }


        //private IMapper Mapper { get; }


        public async Task<List<string>> CheckUserRolesAsync(User user)
        {
            var roles = await this.UserManager.GetRolesAsync(user.ToAppUser());
            return roles.ToList();
        }


        public RoleResponse? GetDefaultRole()
        {
            var role = this.RoleManager.Roles
                .TagWithCallSite(nameof(this.GetDefaultRole))
                .Where(x => x.Name == StringRoleResources.Default &&
                    x.NormalizedName == StringRoleResources.DefaultNormalized)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            RoleResponse roleResponse = new() { Id = role!.Id, Name = role.Name , NormalizedName = role.NormalizedName  };
            return roleResponse;
        }


        public RoleResponse? GetUserRole()
        {
            var role = this.RoleManager.Roles
                .TagWith(nameof(this.GetUserRole))
                .Where(x => x.Name == StringRoleResources.User &&
                    x.NormalizedName == StringRoleResources.UserNormalized)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            RoleResponse roleResponse = new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName };
            return roleResponse;
        }


        public RoleResponse? GetAdminRole()
        {
            var role = this.RoleManager.Roles
                .TagWith(nameof(this.GetAdminRole))
                .Where(x => x.Name == StringRoleResources.Admin &&
                    x.NormalizedName == StringRoleResources.AdminNormalized)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            RoleResponse roleResponse = new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName };
            return roleResponse;
        }


        public async Task<RequestResponse> SetUserRoleAsync(User user, string role)
        {
            var roles = await this.CheckUserRolesAsync(user);
            if (roles.Count == 0)
            {
                await this.UserManager.AddToRoleAsync(user.ToAppUser(), role);
                var roleData = await this.RoleManager.FindByNameAsync(role);
                return RequestResponse.Success();
            }
            else if (roles.Count > 0)
            {
                await this.UserManager.RemoveFromRoleAsync(user.ToAppUser(), roles[0]);
                await this.UserManager.AddToRoleAsync(user.ToAppUser(), role);
                var roleData = await this.RoleManager.FindByNameAsync(role);
                return RequestResponse.Success();
            }

            throw new Exception("The user has already a role");
        }


        public async Task<List<RoleResponse>> GetRolesAsync()
        {
            var result = this.RoleManager.Roles
                .TagWith(nameof(this.GetRolesAsync))
                .Where(x => x.Name != StringRoleResources.Admin &&
                    x.NormalizedName != StringRoleResources.AdminNormalized)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .ToList();
            
            var roles = new List<RoleResponse>();
            foreach (var role in result)
                roles.Add(new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName });
            return roles;
        }


        public List<RoleResponse> GetRolesForAdmin()
        {
            var result = this.RoleManager.Roles
                .TagWith(nameof(this.GetRolesForAdmin))
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .ToList();

            var roles = new List<RoleResponse>();
            foreach (var role in result)
                roles.Add(new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName });
            return roles;
        }


        public async Task<RoleResponse?> GetRoleByIdAsync(int id)
        {
            var role = this.RoleManager.Roles
                .TagWith(nameof(this.GetRoleByIdAsync))
                .Where(x => x.Id == id)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            RoleResponse roleResponse = new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName };
            return roleResponse;
        }


        public RoleResponse? GetRoleByNormalizedName(string normalizedName)
        {
            var role = this.RoleManager.Roles
                .TagWith(nameof(this.GetRoleByNormalizedName))
                .Where(x => x.NormalizedName == normalizedName)
                //.ProjectTo<RoleResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            RoleResponse roleResponse = new() { Id = role!.Id, Name = role.Name, NormalizedName = role.NormalizedName };
            return roleResponse;
        }


        public async Task<RequestResponse> CreateRoleAsync(CreateRoleRequest request)
        {
            var role = await this.RoleManager.FindByNameAsync(request.Name!);
            if (role != null)
            {
                throw new Exception("The role was already created");
            }

            await this.RoleManager.CreateAsync(new Role
            {
                Name = request.Name,
                NormalizedName = request.Name!.ToUpper(),
            }.ToAppRole());

            var roleData = await this.RoleManager.FindByNameAsync(request.Name);
            return RequestResponse.Success();
        }

        public async Task<RequestResponse> UpdateRoleAsync(UpdateRoleRequest request)
        {
            var existsRole = await this.RoleManager.FindByNameAsync(request.Name!);
            if (existsRole != null)
            {
                throw new Exception("The new role already exists");
            }

            var role = await this.RoleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                throw new Exception("The role was not created");
            }

            role.Name = request.Name;
            role.NormalizedName = request.Name!.ToUpper();

            await this.RoleManager.UpdateAsync(role);
            return RequestResponse.Success();
        }

        public async Task<RequestResponse> DeleteRoleAsync(int roleId)
        {
            var role = await this.RoleManager.FindByIdAsync(roleId.ToString());
            if (role == null)
            {
                throw new Exception("The role was not found");
            }

            await this.RoleManager.DeleteAsync(role);
            return RequestResponse.Success();
        }

        public async Task<Role?> FindRoleByIdAsync(int roleId)
        {
            var result = await this.RoleManager.FindByIdAsync(roleId.ToString());
            return result.ToDomainRole();
        }

        public async Task<Role?> FindRoleByNameAsync(string name)
        {
            var result = await this.RoleManager.FindByNameAsync(name);
            return result.ToDomainRole();
        }
    }
}
