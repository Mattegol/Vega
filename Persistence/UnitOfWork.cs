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

        public IPhotoRepository Photos { get; set; }
        
        public UnitOfWork(VegaDbContext context)
        {
            Vehicles = new VehicleRepository(context);
            Photos = new PhotoRepository(context);
            _context = context;
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}