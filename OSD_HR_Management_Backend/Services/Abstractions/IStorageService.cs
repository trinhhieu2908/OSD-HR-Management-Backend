using Microsoft.AspNetCore.Mvc;
using OSD_HR_Management_Backend.Services.Models;

namespace OSD_HR_Management_Backend.Services.Abstractions;

public interface IStorageService
{
    Task<string> UploadFileAsync(S3ObjectUpload obj);
}
