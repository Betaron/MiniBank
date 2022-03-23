namespace Minibank.Web.Controllers.MoneyTransferHistoryUnits.Dto
{
    public class CreateHistoryUnitDto
    {
        public double Amount { get; set; }
        public string Currency { get; set; }
        public string FromAccountId { get; set; }
        public string ToAccountId { get; set; }
    }
}
