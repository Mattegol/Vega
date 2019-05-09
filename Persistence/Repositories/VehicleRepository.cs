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

        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var query = VegaDbContext.Vehicles
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .AsQueryable();

            if(queryObj.MakeId.HasValue)
            {
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId);
            }

            if(queryObj.ModelId.HasValue)
            {
                query = query.Where(v => v.ModelId == queryObj.ModelId);
            }

            if (queryObj.SortBy == "make") {
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);
            }

            if (queryObj.SortBy == "model") {
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);
            }

            if (queryObj.SortBy == "contactName") {
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);
            }

            if (queryObj.SortBy == "id") {
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);
            }

            return await query.ToListAsync();
                
        }
    }
}