using System.IO;

namespace zaraga.logger;

public partial class PlatformPathService
{
    partial void GetPlatformDownloadsPath(ref string downloadsPath)
    {
        downloadsPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory?.AbsolutePath ?? "", Android.OS.Environment.DirectoryDownloads ?? "Downloads");
    }
}