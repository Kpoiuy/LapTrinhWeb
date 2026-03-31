using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;

namespace BookAPI.Services;

public static class ImageSeedWriter
{
    public static void EnsureSeedImages(string imageRoot)
    {
        Directory.CreateDirectory(imageRoot);

        var configs = new[]
        {
            ("seed-book-1.jpg", new Rgba32(24, 119, 242), true),
            ("seed-book-2.jpg", new Rgba32(16, 185, 129), true),
            ("seed-book-3.png", new Rgba32(244, 114, 182), false),
            ("seed-book-4.png", new Rgba32(249, 115, 22), false),
            ("seed-book-5.jpg", new Rgba32(99, 102, 241), true),
            ("seed-book-6.png", new Rgba32(234, 179, 8), false)
        };

        foreach (var (fileName, baseColor, isJpg) in configs)
        {
            var path = Path.Combine(imageRoot, fileName);
            CreateCover(path, baseColor, isJpg);
        }
    }

    private static void CreateCover(string path, Rgba32 baseColor, bool asJpg)
    {
        using var image = new Image<Rgba32>(300, 420, baseColor);
        image.Mutate(ctx =>
        {
            ctx.Fill(new Rgba32((byte)Math.Min(baseColor.R + 30, 255), (byte)Math.Min(baseColor.G + 30, 255), (byte)Math.Min(baseColor.B + 30, 255)),
                new Rectangle(20, 20, 260, 90));
            ctx.Fill(new Rgba32((byte)Math.Max(baseColor.R - 25, 0), (byte)Math.Max(baseColor.G - 25, 0), (byte)Math.Max(baseColor.B - 25, 0)),
                new Rectangle(20, 130, 260, 260));
        });

        if (asJpg)
        {
            image.Save(path, new JpegEncoder { Quality = 90 });
        }
        else
        {
            image.Save(path, new PngEncoder());
        }
    }
}
