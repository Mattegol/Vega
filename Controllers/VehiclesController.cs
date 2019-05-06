using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers
{
  [Route("/api/vehicles")]
  public class VehiclesController : Controller
  {
    private readonly IMapper _mapper;
    private readonly VegaDbContext _context;

    public VehiclesController(IMapper mapper, VegaDbContext context)
    {
      _context = context;
      _mapper = mapper;
    }

    [HttpPost]
    public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
    {
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var vehicle = _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
        vehicle.LastUpdate = DateTime.Now;

        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync();

        var result = _mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

        return Ok(result);
    }

    [HttpPut("{id}")] 
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
    {
        if (!ModelState.IsValid)
          return BadRequest(ModelState);

        var vehicle = await _context.Vehicles.Include(v => v.Features).SingleOrDefaultAsync(v => v.Id == id);

        if(vehicle == null)
        {
            return NotFound();
        }

        _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
        vehicle.LastUpdate = DateTime.Now;

        await _context.SaveChangesAsync();

        var result = _mapper.Map<Vehicle, SaveVehicleResource>(vehicle);

        return Ok(result);
    }

    [HttpDelete("{id}")] 
    public async Task<IActionResult> DeleteVehicle(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);

        if(vehicle == null)
        {
            return NotFound();
        }
       
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync();

        return Ok(id);
    }

    [HttpGet("{id}")] 
    public async Task<IActionResult> GetVehicle(int id)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.Features)
              .ThenInclude(vf => vf.Feature)
            .Include(v => v.Model)
              .ThenInclude(m => m.Make)
            .SingleOrDefaultAsync(v => v.Id == id);

        if(vehicle == null)
        {
            return NotFound();
        }

        var vehicleResource = _mapper.Map<Vehicle, VehicleResource>(vehicle);

        return Ok(vehicleResource);
    }

  }
}