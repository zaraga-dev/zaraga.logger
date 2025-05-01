using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace zaraga.logger;

public class Manager
{
	/// <summary>
	/// Singleton
	/// </summary>
	private static Manager? _instance;

	public static Manager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new Manager();
			}

			return _instance;
		}
	}

	private readonly string _defaultLogPath;
	private string _customLogPath = "";
	private int _daysToRecord = 10;

	private string UsedLogPath => string.IsNullOrWhiteSpace(_customLogPath) ? _defaultLogPath : _customLogPath;


	/// <summary>
	/// constructor
	/// </summary>
	public Manager()
	{
		_defaultLogPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "app.log");
	}

	/// <summary>
	/// Initialize class, create log file if dont exist
	/// </summary>
	public void Init(string filePath = "", int daysToRecord = 10)
	{
		_customLogPath = filePath;
		_daysToRecord  = daysToRecord;
	}

	/// <summary>
	/// Write a record in the log file
	/// </summary>
	public void Write(string message)
	{
		try
		{
#if DEBUG
			System.Diagnostics.Debug.WriteLine(message);
#endif
			lock (UsedLogPath)
			{
				using (StreamWriter writer = File.AppendText(UsedLogPath))
				{
					writer.WriteLine("{0} - {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), message);
					writer.Close();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
#if DEBUG
			System.Diagnostics.Debug.WriteLine(ex);
#endif
		}
	}

	/// <summary>
	/// Write a record in the log file using all data from exception
	/// </summary>
	public void Write(Exception? exception)
	{
		try
		{
#if DEBUG
			System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(exception?.Message));
			System.Diagnostics.Debug.WriteLine(JsonConvert.SerializeObject(exception?.InnerException?.Message));
#endif
			lock (UsedLogPath)
			{
				using (StreamWriter writer = File.AppendText(UsedLogPath))
				{
					writer.WriteLine("Exception message:");
					writer.WriteLine("{0} - {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), exception?.Message);
					writer.WriteLine("InnerException message:");
					writer.WriteLine("{0} - {1}", DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"), exception?.InnerException?.Message);
					writer.Close();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
#if DEBUG
			System.Diagnostics.Debug.WriteLine(ex);
#endif
		}
	}

	/// <summary>
	/// Delete log records where date is before _daysToRecord value
	/// </summary>
	public void DeleteOldRecords()
	{
		try
		{
			if (File.Exists(UsedLogPath))
			{
				//If file size is greater than 10 mb delete it
				byte[] bytes = GetLogBytes();
				if (bytes.Length > 10485760)
				{
					File.Delete(UsedLogPath);
					return;
				}

				string[] lines                  = File.ReadAllLines(UsedLogPath);
				string[] lineParts              = lines[0].Split('-');
				string   firstlogDatetimeString = lineParts[0].Trim();
				DateTime firstlogDatetime       = DateTime.Parse(firstlogDatetimeString);
				if (firstlogDatetime.Date < DateTime.Now.AddDays(_daysToRecord * -1).Date)
				{
					string dayToDelete = firstlogDatetime.Date.ToString("d");
					int    totalLines  = lines.Count(x => x.StartsWith(dayToDelete));
					File.WriteAllLines(UsedLogPath, lines.Skip(totalLines + 1));
				}
			}
		}
		catch (Exception ex)
		{
			File.Delete(UsedLogPath);
			Console.WriteLine(ex);
#if DEBUG
			System.Diagnostics.Debug.WriteLine(ex);
#endif
		}
	}

	public byte[] GetLogBytes()
	{
		return File.ReadAllBytes(_customLogPath);
	}

	/// <summary>
	/// Creates a copy of log file in public Download folder
	/// </summary>
	public void ExportLogFile()
	{
		try
		{
			string downloadsPath = PlatformPathService.Instance.GetDownloadsPath();

#if ANDROID
			if (OperatingSystem.IsAndroidVersionAtLeast(29))
			{
				File.Copy(UsedLogPath, downloadsPath, overwrite: true);
			}
			else
			{
				//TODO: use required permissions to major android versions
			}
#endif
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex);
#if DEBUG
			System.Diagnostics.Debug.WriteLine(ex);
#endif
		}
	}
}