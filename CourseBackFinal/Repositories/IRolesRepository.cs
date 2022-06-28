namespace CourseBackFinal.Repositories
{
    public interface IRolesRepository
    {
        Task<string> CreateRole(string roleName);
    }
}
