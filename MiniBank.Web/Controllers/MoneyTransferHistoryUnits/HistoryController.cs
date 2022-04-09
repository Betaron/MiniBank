using Microsoft.AspNetCore.Mvc;
using Minibank.Core.Domains.MoneyTransferHistoryUnits;
using Minibank.Core.Domains.MoneyTransferHistoryUnits.Services;
using Minibank.Web.Controllers.MoneyTransferHistoryUnits.Dto;

namespace Minibank.Web.Controllers.MoneyTransferHistoryUnits
{
    [ApiController]
    [Route("history")]
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
        public async Task<HistoryUnitDto> GetById(string id, CancellationToken cancellationToken)
        {
            var model = await _historyService.GetByIdAsync(id, cancellationToken);

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
        public async Task<IEnumerable<HistoryUnitDto>> GetAll(CancellationToken cancellationToken)
        {
            var models = await _historyService.GetAllAsync(cancellationToken);

            return models.Select(it => new HistoryUnitDto
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
        public async Task Create(CreateHistoryUnitDto model, CancellationToken cancellationToken)
        {
            await _historyService.CreateAsync(new MoneyTransferHistoryUnit
            {
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            }, cancellationToken);
        }

        /// <summary>
        /// Searches money transfer history unit 
        /// and changes based on the passed
        /// </summary>
        /// <param name="model">Money transfer history unit to be changed</param>
        [HttpPut("{id}")]
        public async Task Update(string id, UpdateHistoryUnitDto model, CancellationToken cancellationToken)
        {
            await _historyService.UpdateAsync(new MoneyTransferHistoryUnit
            {
                Id = id,
                Amount = model.Amount,
                Currency = model.Currency,
                FromAccountId = model.FromAccountId,
                ToAccountId = model.ToAccountId
            }, cancellationToken);
        }

        /// <summary>
        /// Deletes a money transfer history unit by id
        /// </summary>
        /// <param name="id">Money transfer history unit identification number</param>
        [HttpDelete("{id}")]
        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            await _historyService.DeleteAsync(id, cancellationToken);
        }
    }
}
