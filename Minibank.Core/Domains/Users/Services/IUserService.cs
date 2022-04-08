namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Searches for a user by his id
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>Found user</returns>
        Task<User> GetByIdAsync(string id);

        /// <returns>All users from repository</returns>
        Task<IEnumerable<User>> GetAllAsync();

        /// <summary>
        /// Adds a new user to the repository. Copies an argument
        /// </summary>
        /// <param name="user">Template user</param>
        Task CreateAsync(User user);

        /// <summary>
        /// Searches the repository for a user by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="user">User to be changed</param>
        Task UpdateAsync(User user);

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User identification number</param>
        Task DeleteAsync(string id);
    }
}
