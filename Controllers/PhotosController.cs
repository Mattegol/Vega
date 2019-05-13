using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vega.Controllers.Resources;
using vega.Core;
using vega.Core.Models;
using vega.Core.Repositories;

namespace vega.Controllers
{
    // /api/vehicles/1/photos
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly int MAX_BYTES = 10 * 1024 * 1024;
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png" };

        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PhotosController(IHostingEnvironment host, 
            IVehicleRepository repository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _host = host;
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId);
            if (vehicle == null) return NotFound();
            if (file == null) return BadRequest("Null file.");
            if (file.Length == 0) return BadRequest("Empty file.");
            if (file.Length > MAX_BYTES) return BadRequest("Max file size exceeded.");
            if (!ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(file.FileName))) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}