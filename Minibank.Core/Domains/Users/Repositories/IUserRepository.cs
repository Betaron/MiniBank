namespace Minibank.Core.Domains.Users.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Searches for a user by his id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>Found user</returns>
        Task<User> GetByIdAsync(string id, CancellationToken cancellationToken);

        ///<summary>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All users</returns>
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new user. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="user">Template user</param>
        Task CreateAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Searches for a user by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="user">User to be changed</param>
        Task UpdateAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a user by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">User identification number</param>
        Task DeleteAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// Looking for a specific user
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>True if user exists</returns>
        Task<bool> UserExistsAsync(string id, CancellationToken cancellationToken);
    }
}
