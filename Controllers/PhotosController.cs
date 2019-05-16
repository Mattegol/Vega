using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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
        private readonly IHostingEnvironment _host;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly PhotoSettings _photoSettings;

        private readonly IPhotoService _photoService;

        public PhotosController(IHostingEnvironment host, 
            IVehicleRepository repository, 
            IUnitOfWork unitOfWork, 
            IMapper mapper,
            IOptionsSnapshot<PhotoSettings> options,
            IPhotoService photoService)
        {
            _photoSettings = options.Value;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _repository = repository;
            _host = host;
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhotos(int vehicleId)
        {
            var photos = await _unitOfWork.Photos.GetPhotos(vehicleId);

            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await _repository.GetVehicle(vehicleId);
            if (vehicle == null) return NotFound();
            if (file == null) return BadRequest("Null file.");
            if (file.Length == 0) return BadRequest("Empty file.");
            if (file.Length > _photoSettings.MaxBytes) return BadRequest("Max file size exceeded.");
            if (!_photoSettings.IsSupported(file.FileName)) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(_host.WebRootPath, "uploads");
            var photo= await _photoService.UploadPhoto(vehicle, file, uploadsFolderPath);

            return Ok(_mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}