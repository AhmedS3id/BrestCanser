using BrestCanser.Api.Clients.MLModel.Contracts.Images;
using BrestCanser.Api.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.Extensions.Options;

namespace BrestCanser.Api.Services;

public class ImageService(IOptions<CloudinarySettings> cloudinarySettings) : IImageService
{
	private readonly Cloudinary _cloudinary = new(new Account(
		cloudinarySettings.Value.CloudName,
		cloudinarySettings.Value.ApiKey,
		cloudinarySettings.Value.ApiSecret
	));

	public async Task<UploadImageResult> UploadAsync(IFormFile file, string folder, CancellationToken cancellationToken = default)
	{
		await using var stream = file.OpenReadStream();

		var uploadParams = new ImageUploadParams
		{
			File = new FileDescription(file.FileName, stream),
			Folder = folder,
			UseFilename = true
		};

		var uploadResult = await _cloudinary.UploadAsync(uploadParams, cancellationToken);

		var imageUrl = uploadResult.SecureUrl.ToString();


		return new UploadImageResult(imageUrl, uploadResult.PublicId);
	}

	public async Task DeleteAsync(string publicId)
	{
		var deleteParams = new DeletionParams(publicId);
		await _cloudinary.DestroyAsync(deleteParams);
	}

}
