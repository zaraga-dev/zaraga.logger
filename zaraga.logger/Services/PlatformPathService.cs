namespace zaraga.logger;

public partial class PlatformPathService
{
	private static PlatformPathService? _instance;

	public static PlatformPathService Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PlatformPathService();
			}

			return _instance;
		}
	}

	public string GetDownloadsPath()
	{
		string path = "";
		GetDownloadsPath(ref path);
		return path;
	}

	partial void GetDownloadsPath(ref string downloadsPath);
}