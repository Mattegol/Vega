using System.Threading.Tasks;
using vega.Core;
using vega.Core.Repositories;
using vega.Persistence.Repositories;

namespace vega.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VegaDbContext _context;

        public IVehicleRepository Vehicles { get; private set; }
        
        public UnitOfWork(VegaDbContext context)
        {
            Vehicles = new VehicleRepository(context);
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}