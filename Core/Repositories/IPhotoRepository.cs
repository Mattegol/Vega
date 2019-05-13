using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Core.Models;

namespace vega.Core.Repositories
{
    public interface IPhotoRepository : IRepository<Photo>
    {
         Task<IEnumerable<Photo>> GetPhotos(int vehicleId);
    }
}