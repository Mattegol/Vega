using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core.Models;
using vega.Core.Repositories;

namespace vega.Persistence.Repositories
{
    public class PhotoRepository: Repository<Photo>, IPhotoRepository
    {
        public PhotoRepository(VegaDbContext context) 
            : base(context)
        {
        }

        public VegaDbContext VegaDbContext
        {
            get { return _context as VegaDbContext; }
        } 

        public async Task<IEnumerable<Photo>> GetPhotos(int vehicleId)
        {
            return await VegaDbContext.Photos
                .Where(p => p.VehicleId == vehicleId)
                .ToListAsync();
        }
    }
}