using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core.Repositories;

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

        public async Task<Vehicle> GetVehicle(int id)
        {
            return await VegaDbContext.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetVehicles(Filter filter)
        {
            var query = VegaDbContext.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .AsQueryable();

            if(filter.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == filter.MakeId);
            }

            return await query.ToListAsync();
                
        }
    }
}