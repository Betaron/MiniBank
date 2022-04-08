namespace Minibank.Core.Domains.MoneyTransferHistoryUnits.Services
{
    public interface IMoneyTransferHistoryUnitService
    {
        /// <summary>
        /// Searches for a money transfer history unit by his id
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        /// <returns>Found money transfer history unit</returns>
        Task<MoneyTransferHistoryUnit> GetByIdAsync(string id);

        /// <returns>All money transfer history unit from repository</returns>
        Task<IEnumerable<MoneyTransferHistoryUnit>> GetAllAsync();

        /// <summary>
        /// Adds a new money transfer history unit to the repository. Copies an argument
        /// </summary>
        /// <param name="unit">Template money transfer history unit</param>
        Task CreateAsync(MoneyTransferHistoryUnit unit);

        /// <summary>
        /// Searches the repository for a money transfer history unit by id
        /// and changes based on the passed
        /// </summary>
        /// <param name="unit">Money transfer history unit to be changed</param>
        Task UpdateAsync(MoneyTransferHistoryUnit unit);

        /// <summary>
        /// Deletes a money transfer history unit by id
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        Task DeleteAsync(string id);
    }
}
