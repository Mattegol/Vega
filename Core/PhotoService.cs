using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using vega.Core.Models;

namespace vega.Core
{
    public class PhotoService : IPhotoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPhotoStorage _photoStorage;
        public PhotoService(IUnitOfWork unitOfWork, IPhotoStorage photoStorage)
        {
            _photoStorage = photoStorage;
            _unitOfWork = unitOfWork;

        }
        public async Task<Photo> UploadPhoto(Vehicle vehicle, IFormFile file, string uploadsFolderPath)
        {
            var fileName = await _photoStorage.StorePhoto(uploadsFolderPath, file);

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await _unitOfWork.CompleteAsync();

            return photo;
        }
    }
}