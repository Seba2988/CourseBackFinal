using CourseBackFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseBackFinal.Repositories
{
    public interface IAccountRepository
    {
        Task<IdentityResult> SignUp(SignupModel signupModel, bool isProfessor);
        Task<string> Login(SigninModel signinModel);
        Task Logout();
        //Task<IdentityResult> UpdateUser(bool isProfessor, string userId, UpdateUserModel? updateUserModel);
        Task<IdentityResult> UpdateUser(bool isProfessor, string userName, UpdateUserModel? updateUserModel);
        Task<IdentityResult> DeleteUser(string userId);

        Task<IEnumerable<UserDTO>> GetAllUsersByRoleName(string roleName);
    }
}
