using BrestCanser.Api.Clients.MLModel.Contracts.Images;

namespace BrestCanser.Api.Services;

public interface IImageService
{
	Task<UploadImageResult> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default);
	Task DeleteAsync(string publicId);
}
