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
        User GetById(string id);

        ///<summary>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All users</returns>
        IEnumerable<User> GetAll();

        /// <summary>
        /// Adds a new user. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="user">Template user</param>
        void Create(User user);

        /// <summary>
        /// Searches for a user by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="user">User to be changed</param>
        void Update(User user);

        /// <summary>
        /// Deletes a user by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">User identification number</param>
        void Delete(string id);

        /// <summary>
        /// Looks for at least one existing account associated with the user
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">User identification number</param>
        /// <returns>True if account exists</returns>
        bool HasBankAccounts(string id);
    }
}
