namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Searches for a user by his id
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>Found user</returns>
        Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <returns>All users from repository</returns>
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new user to the repository. Copies an argument
        /// </summary>
        /// <param name="user">Template user</param>
        Task CreateAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Searches the repository for a user by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="user">User to be changed</param>
        Task UpdateAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User identification number</param>
        Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}
