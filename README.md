
# zaraga.logger

Manager for a .txt file to use as log recorder in .Net MAUI apps :)




## Features

- Singleton Instance
- Use a default directory or can use a directory specified in the initalization
- Export a .txt file in the downloads directory
- Cross platform


## Installation

Install using .Net CLI

```bash
  dotnet add package zaraga.logger --version 1.2.0
```

Install using Package Manager
```bash
NuGet\Install-Package zaraga.logger -Version 1.2.0
```
## Usage/Examples
- As default the logger is stored in the local application data folder and use 10 days to store records, the records with more than 11 days to the past where going to be deleted. 

- The Manager class can be used as static property to usin with App.StaticLogManagerInstance
```c#
public partial class App : Application
{
	public static zaraga.logger.Manager? StaticLogManagerInstance;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		StaticLogManagerInstance  = zaraga.logger.Manager.Instance;
	}
}
```

```c#
private void AddLog()
{
    App.StaticLogManagerInstance?.Write("Message to store");
}
```

- Or if you prefered you can use instance directly in other classes
```c#
private void AddLog()
{
    zaraga.logger.Manager.Instance?.Write("Message to store");
}
```

- In the Init method you can specify a custom file path and the days you want to store in the log file
```c#
using System.IO;
using System;
using Microsoft.Maui.ApplicationModel;

public partial class App : Application
{
	public static zaraga.logger.Manager? Console;

	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
		Console  = zaraga.logger.Manager.Instance;

		string pkgName= Platform.CurrentActivity?.PackageName ?? "MyAppName";
        string logfilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{pkgName}.txt");
        Console.Init(filePath: logfilePath, daysToRecord: 3);
	}
}
```


## Authors

- [@Agarz4](https://github.com/Agarz4)
