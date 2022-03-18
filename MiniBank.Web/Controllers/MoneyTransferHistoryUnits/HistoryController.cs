using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Services;
using Minibank.Web.Controllers.MoneyTransferHistoryUnits.Dto;

namespace Minibank.Web.Controllers.MoneyTransferHistoryUnits
{
    [ApiController]
    [Route("[controller]")]
    public class HistoryController
    {
        private readonly IMoneyTransferHistoryUnitService _historyService;

        public HistoryController(IMoneyTransferHistoryUnitService historyService)
        {
            _historyService = historyService;
        }

        /// <summary>
        /// Searches for a money transfer history unit by his id
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        /// <returns>Found money transfer history unit</returns>
        [HttpGet("{id}")]
        public HistoryUnitDto GetById(string id)
        {
            var model = _historyService.GetById(id);

            return new HistoryUnitDto
            {
                Id = model.Id,
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            };
        }

        /// <summary>
        /// Show full transactions history
        /// </summary>
        /// <returns>All money transfer history unit</returns>
        [HttpGet]
        public IEnumerable<HistoryUnitDto> GetAll()
        {
            return _historyService.GetAll()
                .Select(it => new HistoryUnitDto
                {
                    Id = it.Id,
                    Amount = it.Amount,
                    Currency = it.Currency,
                    FromAccountId = it.FromAccountId,
                    ToAccountId = it.ToAccountId
                });
        }

        /// <summary>
        /// Adds a new money transfer history unit
        /// </summary>
        /// <param name="model">Template money transfer history unit</param>
        [HttpPost]
        public void Create(NewHistoryUnitDto model)
        {
            _historyService.Create(new MoneyTransferHistoryUnit
            {
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }

        /// <summary>
        /// Searches money transfer history unit 
        /// and changes based on the passed
        /// </summary>
        /// <param name="model">Money transfer history unit to be changed</param>
        [HttpPut("{id}")]
        public void Update(string id, NewHistoryUnitDto model)
        {
            _historyService.Update(new MoneyTransferHistoryUnit
            {
                Id = id,
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            });
        }

        /// <summary>
        /// Deletes a money transfer history unit by id
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _historyService.Delete(id);
        }
    }
}
