using Amazon.S3;
using Amazon.S3.Model;
using OSD_HR_Management_Backend.Services.Abstractions;
using OSD_HR_Management_Backend.Services.Models;

namespace OSD_HR_Management_Backend.Services.Implementations;

public class StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;

    public StorageService(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<string> UploadFileAsync(S3ObjectUpload obj)
    {
        var path = string.IsNullOrEmpty(obj.Prefix) ? obj.File.FileName : $"{obj.Prefix?.TrimEnd('/')}/{obj.File.FileName}";
        var bucketExists = await _s3Client.DoesS3BucketExistAsync(obj.BucketName);
        if (!bucketExists) return $"Bucket {obj.BucketName} does not exist.";
        var request = new PutObjectRequest()
        {
            BucketName = obj.BucketName,
            Key = path,
            InputStream = obj.File.OpenReadStream()
        };
        request.Metadata.Add("Content-Type", obj.File.ContentType);
        await _s3Client.PutObjectAsync(request);
        return $"https://{obj.BucketName}.s3.ap-southeast-1.amazonaws.com/{path}";
    }
}
