namespace BrestCanser.Api.Clients.MLModel.Contracts.Images;

public record UploadImageResult(
	string ImageUrl,
	string PublicId
);
