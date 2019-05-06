using System.Threading.Tasks;

namespace vega.Models.Repositories
{
    public interface IUnitOfWork
    {
         Task CompleteAsync();
    }
}