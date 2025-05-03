namespace zaraga.logger;

public partial class PlatformPathService
{
    partial void GetPlatformDownloadsPath(ref string downloadsPath)
    {
        downloadsPath = Windows.Storage.KnownFolders.DocumentsLibrary.Path;
    }
}