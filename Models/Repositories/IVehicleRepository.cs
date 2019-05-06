namespace vega.Models.Repositories
{
    public interface IVehicleRepository : IRepository<Vehicle>
    {
         Vehicle GetVehicleWithFeatures(int id);
    }
}