namespace Minibank.Core.Domains.MoneyTransferHistoryUnits.Repositories
{
    public interface IMoneyTransferHistoryUnitRepository
    {
        /// <summary>
        /// Searches for a money transfer history unit by his id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        /// <returns>Found money transfer history unit</returns>
        Task<MoneyTransferHistoryUnit> GetByIdAsync(string id, CancellationToken cancellationToken);

        /// <summary>
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All money transfer history unit</returns>
        Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Adds a new money transfer history unit. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="unit">Template money transfer history unit</param>
        Task CreateAsync(MoneyTransferHistoryUnit unit, CancellationToken cancellationToken);

        /// <summary>
        /// Searches for a money transfer history unit by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="unit">Money transfer history unit to be changed</param>
        Task UpdateAsync(MoneyTransferHistoryUnit unit, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a money transfer history unit by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        Task DeleteAsync(string id, CancellationToken cancellationToken);
    }
}
