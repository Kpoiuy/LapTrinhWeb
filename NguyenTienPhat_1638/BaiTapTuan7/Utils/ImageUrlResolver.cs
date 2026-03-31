namespace BaiTapTuan7.Utils;

public static class ImageUrlResolver
{
    public const string DefaultImagePath = "/images/course-default.svg";

    public static string Resolve(string? imagePath)
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return DefaultImagePath;
        }

        var trimmed = imagePath.Trim();
        if (Uri.TryCreate(trimmed, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps || uri.Scheme == "data"))
        {
            return trimmed;
        }

        var normalized = trimmed.Replace('\\', '/');
        if (normalized.StartsWith("~/", StringComparison.Ordinal))
        {
            normalized = normalized[1..];
        }
        else if (!normalized.StartsWith("/", StringComparison.Ordinal))
        {
            normalized = "/" + normalized.TrimStart('/');
        }

        return string.IsNullOrWhiteSpace(normalized) ? DefaultImagePath : normalized;
    }
}
