using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Models;
using vega.Models.Repositories;

namespace vega.Persistence.Repositories
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(VegaDbContext context) 
            : base(context)
        {
        }

        public VegaDbContext VegaDbContext
        {
            get { return _context as VegaDbContext; }
        } 

        public Vehicle GetVehicleWithFeatures(int id)
        {
            return VegaDbContext.Vehicles.Include(v => v.Features).SingleOrDefault(v => v.Id == id);
        }
    }
}