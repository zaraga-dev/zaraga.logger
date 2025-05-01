using System.IO;

namespace zaraga.logger;

public partial class PlatformPathService
{
	partial void GetDownloadsPath(ref string path)
	{
		path = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
	}
}