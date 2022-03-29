using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        MoneyTransferHistoryUnit GetById(string id);

        /// <summary>
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <returns>All money transfer history unit</returns>
        IEnumerable<MoneyTransferHistoryUnit> GetAll();

        /// <summary>
        /// Adds a new money transfer history unit. Copies an argument
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="unit">Template money transfer history unit</param>
        void Create(MoneyTransferHistoryUnit unit);

        /// <summary>
        /// Searches for a money transfer history unit by id
        /// and changes based on the passed
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="unit">Money transfer history unit to be changed</param>
        void Update(MoneyTransferHistoryUnit unit);

        /// <summary>
        /// Deletes a money transfer history unit by id
        /// <br/>
        /// <i>(You need to implement only the basic logic of working with the repository)</i>
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        void Delete(string id);
    }
}
