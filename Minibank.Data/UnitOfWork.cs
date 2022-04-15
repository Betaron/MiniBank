using Minibank.Core;

namespace Minibank.Data
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly MinibankContext _context;
        
        public UnitOfWork(MinibankContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
