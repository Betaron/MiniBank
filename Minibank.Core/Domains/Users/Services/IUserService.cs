namespace Minibank.Core.Domains.Users.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Searches for a user by his id
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>Found user</returns>
        User GetById(string id);

        /// <returns>All users from repository</returns>
        IEnumerable<User> GetAll();

        /// <summary>
        /// Adds a new user to the repository. Copies an argument
        /// </summary>
        /// <param name="user">Template user</param>
        void Create(User user);

        /// <summary>
        /// Searches the repository for a user by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="user">User to be changed</param>
        void Update(User user);

        /// <summary>
        /// Deletes a user by id
        /// </summary>
        /// <param name="id">User identification number</param>
        void Delete(string id);
    }
}
