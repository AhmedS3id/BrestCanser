namespace BrestCanser.Api.Settings;

public static class FileSettings
{
	public const int MaxFileSizeInMB = 10;
	public const long MaxFileSizeInBytes = MaxFileSizeInMB * 1024 * 1024;
	public static readonly string[] AllowedSignatures =
	[
		"FF-D8",  // JPEG , JPG
        "89-50",  // PNG
        "52-49",  // WebP
    ];
}