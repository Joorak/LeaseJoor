

using Domain.Entities.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{

    public class UserService : IUserService
    {
        
        public UserService(
            UserManager<AppUser> userManager,
            IRoleService roleService,
            IUserClaimsPrincipalFactory<AppUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            this.UserManager = userManager;
            this.RoleService = roleService;
            this.UserClaimsPrincipalFactory = userClaimsPrincipalFactory;
            this.AuthorizationService = authorizationService;
        }


        private UserManager<AppUser> UserManager { get; }


        private IRoleService RoleService { get; }


        //private IMapper Mapper { get; }


        private IUserClaimsPrincipalFactory<AppUser> UserClaimsPrincipalFactory { get; }


        private IAuthorizationService AuthorizationService { get; }


        public async Task<RequestResponse> CreateUserAsync(CreateAccountRequest command)
        {
            var existUser = this.UserManager.Users.SingleOrDefault(u => u.UserName == command.UserName && u.IsActive == true);
            if (existUser != null)
            {
                throw new Exception("The user with the unique identifier already exists");
            }

            var newUser = new User
            {
                UserName = command.FirstName + "@" + command.LastName,
                Email = command.UserName,
                FirstName = command.FirstName,
                LastName = command.LastName,
                IsActive = true,
            };

            var result = await this.UserManager.CreateAsync(newUser.ToAppUser());
            if (command.Role!.Length > 0)
            {
                await this.UserManager.AddToRoleAsync(newUser.ToAppUser(), command.Role);
            }

            var user = await this.UserManager.FindByIdAsync(command.UserName!);
            return RequestResponse.Success();
        }


        public async Task<RequestResponse> DeleteUserAsync(int userId)
        {
            var user = this.UserManager.Users.SingleOrDefault(u => u.Id == userId && u.IsActive == true);
            if (user == null)
            {
                throw new Exception("The user doesn't exist");
            }

            user.IsActive = false;

            var result = await this.UserManager.UpdateAsync(user);
            return RequestResponse.Success();
        }


        public async Task<User?> FindUserByEmailAsync(string email)
        {
            var result = await this.UserManager.FindByEmailAsync(email);
            return result.ToDomainUser();
        }


        public async Task<User?> FindUserByIdAsync(int userId)
        {
            var result = await this.UserManager.FindByIdAsync(userId.ToString());
            return result.ToDomainUser();
        }


        public async Task<UserResponse?> GetUserByIdAsync(int userId)
        {
            var user = this.UserManager.Users
                .TagWithCallSite(nameof(this.GetUserByIdAsync))
                .Where(x => x.Id == userId && x.IsActive == true)
                //.ProjectTo<UserResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();

            UserResponse userResponse = new() { Id = user!.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email , IsActive = user.IsActive };
            return userResponse;
        }

        public UserResponse? GetUserByEmail(string email)
        {
            var user = this.UserManager.Users
                .TagWith(nameof(this.GetUserByEmail))
                .Where(x => x.Email == email.ToLower() && x.IsActive == true)
                //.ProjectTo<UserResponse>(this.Mapper.ConfigurationProvider)
                .FirstOrDefault();
            UserResponse userResponse = new() { Id = user!.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, IsActive = user.IsActive };
            return userResponse;
        }

        public async Task<List<string>> GetUserRoleAsync(User user)
        {
            var userRoles = await this.UserManager.GetRolesAsync(user.ToAppUser());
            return userRoles.ToList();
        }

        public async Task<RequestResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var existUser = this.UserManager.Users.SingleOrDefault(u => u.Id == request.Id && u.IsActive == true);
            if (existUser == null)
            {
                throw new Exception("The user does not exists");
            }

            existUser.UserName = request.FirstName + "@" + request.LastName;
            existUser.FirstName = request.FirstName;
            existUser.LastName = request.LastName;

            var result = await this.UserManager.UpdateAsync(existUser);

            //if (request.Role != null)
            //{
            //    var role = await this.RoleService.FindRoleByNameAsync(request.Role);
            //    await this.AssignUserToRoleAsync(new AssignUserToRoleRequest { UserId = existUser.Id, RoleId = role!.Id });
            //}

            return RequestResponse.Success();
        }

        public async Task<RequestResponse> ActivateUserAsync(ActivateUserRequest request)
        {
            var existUser = this.UserManager.Users.SingleOrDefault(u => u.Id == request.UserId);
            if (existUser == null)
            {
                throw new Exception("The user does not exists");
            }

            existUser.IsActive = true;

            var result = await this.UserManager.UpdateAsync(existUser);
            return RequestResponse.Success();
        }

        public async Task<RequestResponse> UpdateUserEmailAsync(UpdateUserEmailRequest request)
        {
            var existUser = this.UserManager.Users.SingleOrDefault(u => u.Id == request.UserId &&
                u.Email == request.Email && u.IsActive == true);
            if (existUser == null)
            {
                throw new Exception("The user does not exists");
            }

            var userWithNewEmail = await this.UserManager.FindByEmailAsync(request.Email!);
            if (userWithNewEmail != null)
            {
                throw new Exception("The user with the new email value has found in the database");
            }

            existUser.Email = request.Email;

            var result = await this.UserManager.UpdateAsync(existUser);
            return RequestResponse.Success();
        }

        public async Task<List<UserResponse>> GetUsersAsync()
        {
            var result = this.UserManager.Users
                .TagWith(nameof(this.GetUsersAsync))
                .Where(u => u.IsActive == true)
                //.ProjectTo<UserResponse>(this.Mapper.ConfigurationProvider)
                .ToList();
            var users = new List<UserResponse>();
            foreach (var user in result)
                users.Add(new() { Id = user!.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, IsActive = user.IsActive });
            return users;
        }

        public async Task<List<UserResponse>> GetUsersInactiveAsync()
        {
            var result = this.UserManager.Users
                .TagWith(nameof(this.GetUsersInactiveAsync))
                .Where(u => u.IsActive == false)
                //.ProjectTo<UserResponse>(this.Mapper.ConfigurationProvider)
                .ToList();
            var users = new List<UserResponse>();
            foreach (var user in result)
                users.Add(new() { Id = user!.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email, IsActive = user.IsActive });
            return users;
        }


        public async Task<RequestResponse> AssignUserToRoleAsync(AssignUserToRoleRequest command)
        {
            var user = await this.UserManager.FindByIdAsync(command.UserId.ToString());
            var userRole = await this.UserManager.GetRolesAsync(user!);
            await this.UserManager.RemoveFromRoleAsync(user!, userRole[0]);

            var role = await this.RoleService.FindRoleByIdAsync(command.RoleId);
            await this.UserManager.AddToRoleAsync(user!, role!.Name!);

            return RequestResponse.Success();
        }


        public async Task<bool> IsInRoleAsync(int userId, string role)
        {
            var user = this.UserManager.Users.SingleOrDefault(u => u.Id == userId);

            return user != null && await this.UserManager.IsInRoleAsync(user, role);
        }


        public async Task<bool> AuthorizeAsync(int userId, string policyName)
        {
            var user = this.UserManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return false;
            }

            var principal = await this.UserClaimsPrincipalFactory.CreateAsync(user);

            var result = await this.AuthorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }
    }
}
