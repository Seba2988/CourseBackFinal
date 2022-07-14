using CourseBackFinal.DTO;
using CourseBackFinal.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;

namespace CourseBackFinal.Repositories
{
    public interface IAccountRepository
    {
        Task<ResponseObject> SignUp(SignupModel signupModel, bool isProfessor);
        Task<ResponseObject> Login(SigninModel signinModel);
        Task Logout();
        //Task<IdentityResult> UpdateUser(bool isProfessor, string userId, UpdateUserModel? updateUserModel);
        Task<ResponseObject> UpdateUser(bool isProfessor, string userName, UpdateUserModel? updateUserModel);
        Task<ResponseObject> DeleteUser(string userId);

        Task<ResponseObject> GetAllUsersByRoleName(string roleName);
        Task<ResponseObject> GetUserById(string userId);
    }
}
