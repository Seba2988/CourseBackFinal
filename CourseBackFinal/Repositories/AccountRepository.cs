using CourseBackFinal.Models;
using CourseBackFinal.Data;
using Microsoft.AspNetCore.Identity;
using CourseBackFinal.Helpers;
using System.Linq;

namespace CourseBackFinal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> SignUp(SignupModel signupModel, bool isProfessor)
        {
            string roleName = isProfessor ? "Professor" : "Student";
            var user = new AppUser()
            {
                FirstName = signupModel.FirstName,
                LastName = signupModel.LastName,
                Email = signupModel.Email,
                UserName = signupModel.Email,
            };
            if (!isProfessor)
            {
                user.Address = signupModel.Address;
                user.DateOfBirth = (DateTime)signupModel.DateOfBirth;
            }
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if (result.Succeeded) return await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }

        public async Task<string?> Login(SigninModel signinModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signinModel.Email, signinModel.Password, false, false);
            if (!result.Succeeded) return null;
            var user = await _userManager.FindByEmailAsync(signinModel.Email);
            return await LoginHelper.NewToken(user, _configuration, _userManager);
        }
        public async Task<IdentityResult> UpdateUser(bool isProfessor, string userName, UpdateUserModel updateUserModel)
        {
            if (updateUserModel == null || (
                updateUserModel.Email == null &&
                updateUserModel.Password == null &&
                updateUserModel.FirstName == null &&
                updateUserModel.LastName == null &&
                updateUserModel.Address == null &&
                updateUserModel.DateOfBirth == null
                ))
            {
                return IdentityResult.Failed(new IdentityError() { Description = "There are no fields to update for this user"});
            }
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                if (updateUserModel.FirstName != null)
                {
                    user.FirstName = updateUserModel.FirstName;
                }
                if (updateUserModel.LastName != null)
                {
                    user.LastName = updateUserModel.LastName;
                }
                if (updateUserModel.Email != null)
                {
                    user.Email = updateUserModel.Email;
                }
                if (!isProfessor)
                {
                    if (updateUserModel.DateOfBirth != null)
                    {
                        user.DateOfBirth = updateUserModel.DateOfBirth;
                    }
                    if (updateUserModel.Address != null)
                    {
                        user.Address = updateUserModel.Address;
                    }
                }
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded && !string.IsNullOrEmpty(updateUserModel.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    result = await _userManager.AddPasswordAsync(user, updateUserModel.Password);
                }
                return result;
            }
            return IdentityResult.Failed();
        }



        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null) return await _userManager.DeleteAsync(user);
            return IdentityResult.Failed();
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersByRoleName(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if(users.Count()==0) return null;
            IEnumerable<UserDTO> usersToReturn = Array.Empty<UserDTO>();
            foreach(var user in users)
            {
                var userToAppend =  new UserDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email
                };
                if (user.Classes != null) userToAppend.Classes = user.Classes;
                if (user.Courses != null) userToAppend.Courses = user.Courses;
                if (user.Address != null) userToAppend.Address = user.Address;
                if (user.DateOfBirth != null) userToAppend.DateOfBirth = user.DateOfBirth;
                usersToReturn = usersToReturn.Concat(new UserDTO[] { userToAppend });
            }
            return usersToReturn;
        }
    }
}
