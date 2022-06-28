using CourseBackFinal.Models;
using Microsoft.AspNetCore.Identity;
using CourseBackFinal.Helpers;

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
            if(!isProfessor)
            {
                user.Address = signupModel.Address;
                user.DateOfBirth = (DateTime)signupModel.DateOfBirth;
            }
            var result = await _userManager.CreateAsync(user, signupModel.Password);
            if(result.Succeeded) return await _userManager.AddToRoleAsync(user, roleName);
            return IdentityResult.Failed();
        }

        public async Task<string?> Login(SigninModel signinModel)
        {
            var result = await _signInManager.PasswordSignInAsync(signinModel.Email, signinModel.Password, false, false);
            if (!result.Succeeded) return null;
            return LoginHelper.NewToken(signinModel, _configuration);
        }

        public async Task<IdentityResult> UpdateUser(bool isProfessor, string userId, UpdateUserModel updateUserModel)
        {
            if(updateUserModel == null)
            {
                return IdentityResult.Failed();
            }
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null)
            {
                if(updateUserModel.FirstName != null)
                {
                    user.FirstName = updateUserModel.FirstName;
                }
                if(updateUserModel.LastName != null)
                {
                    user.LastName = updateUserModel.LastName;
                }
                if(updateUserModel.Email != null)
                {
                    user.Email = updateUserModel.Email;
                }
                if(!isProfessor)
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
                if(result.Succeeded && !string.IsNullOrEmpty(updateUserModel.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    result = await _userManager.AddPasswordAsync(user, updateUserModel.Password);
                }
                if(result.Succeeded)
                {
                    return result;
                }
                return IdentityResult.Failed();
            }
            return IdentityResult.Failed();
        }

        public async Task DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user != null) await _userManager.DeleteAsync(user);
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
