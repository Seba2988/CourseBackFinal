using CourseBackFinal.Models;
using CourseBackFinal.Data;
using Microsoft.AspNetCore.Identity;
using CourseBackFinal.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using CourseBackFinal.DTO;

namespace CourseBackFinal.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly CourseManagementContext _context;

        public AccountRepository(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IConfiguration configuration,
            CourseManagementContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _context = context;
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
                return IdentityResult.Failed(new IdentityError() { Description = "There are no fields to update for this user" });
            }
            var user = await _userManager.FindByEmailAsync(userName);
            if (user != null)
            {
                bool isChanged = false;
                if (updateUserModel.FirstName != null && updateUserModel.FirstName != user.FirstName)
                {
                    user.FirstName = updateUserModel.FirstName;
                    isChanged = true;
                }
                if (updateUserModel.LastName != null && updateUserModel.LastName != user.LastName)
                {
                    user.LastName = updateUserModel.LastName;
                    isChanged = true;
                }
                if (updateUserModel.Email != null && updateUserModel.Email != user.Email)
                {
                    user.Email = updateUserModel.Email;
                    user.UserName = updateUserModel.Email;
                    isChanged = true;
                }
                if (!isProfessor)
                {
                    if (updateUserModel.DateOfBirth != null && updateUserModel.DateOfBirth != user.DateOfBirth)
                    {
                        user.DateOfBirth = updateUserModel.DateOfBirth;
                        isChanged = true;
                    }
                    if (updateUserModel.Address != null && updateUserModel.Address != user.Address)
                    {
                        user.Address = updateUserModel.Address;
                        isChanged = true;
                    }
                }
                if (!isChanged && updateUserModel.Password == null)
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Description = "Nothing has changed for this user"
                    });
                }
                IdentityResult result = await _userManager.UpdateAsync(user);
                if (result.Succeeded && !string.IsNullOrEmpty(updateUserModel.Password))
                {
                    await _userManager.RemovePasswordAsync(user);
                    result = await _userManager.AddPasswordAsync(user, updateUserModel.Password);
                }
                return result;
            }
            return IdentityResult.Failed(new IdentityError()
            {
                Description = "The user is not found"
            });
        }

        public async Task<IdentityResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                var absences = await _context.Absences.Where(a => a.Student == user).ToListAsync();
                _context.Absences.RemoveRange(absences);
                await _context.SaveChangesAsync();
                return await _userManager.DeleteAsync(user);
            }
            return IdentityResult.Failed();
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersByRoleName(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            if (users.Count() == 0) return null;
            IEnumerable<UserDTO> usersToReturn = Array.Empty<UserDTO>();
            foreach (var user in users)
            {
                UserDTO userToAppend = await UserDTOMaker(user.Id);
                usersToReturn = usersToReturn.Concat(new UserDTO[] { userToAppend });
            }
            return usersToReturn;
        }

        public async Task<UserDTO> GetUserById(string userId)
        {
            var user = await UserDTOMaker(userId);
            return user;
        }

        private async Task<UserDTO> UserDTOMaker(string userId)
        {
            var userToReturn = await _context.Users
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    DateOfBirth = u.DateOfBirth,
                    Address = u.Address,
                    Courses = (IList<CourseInStudentDTO>)u.Courses
                    .Select(c => new CourseInStudentDTO
                    {
                        Id = c.Id,
                        Name = c.Name,
                        StartingDate = c.StartingDate,
                        EndingDate = c.EndingDate,
                        ProfessorId = c.ProfessorId
                    }),
                    Absences = (IList<AbsenceDTO>)u.Absences
                    .Select(a => new AbsenceDTO
                    {
                        Id = a.Id,
                        IsPresent = a.IsPresent,
                        ReasonOfAbsence = a.ReasonOfAbsence,
                        Class = new ClassDTO
                        {
                            Date = a.Class.Date,
                            Id = a.Class.Id
                        },
                        StudentId = u.Id
                    })

                }).SingleOrDefaultAsync(u => u.Id == userId);
            return userToReturn;
        }
    }
}
