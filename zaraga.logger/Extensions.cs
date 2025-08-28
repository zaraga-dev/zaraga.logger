using Microsoft.Maui.Hosting;
using Microsoft.Maui.LifecycleEvents;
using System;

namespace zaraga.logger.extensions;

public static class Extensions
{
    private static Manager _loggerInstance;

    public static MauiAppBuilder AddZaragaLogger(this MauiAppBuilder builder, string filePath = "", int daysToRecord = 10)
    {
        _loggerInstance = Manager.Instance;
        _loggerInstance.Init(filePath, daysToRecord);

        builder.ConfigureLifecycleEvents(events =>
        {
#if ANDROID
            events.AddAndroid(android => android
                       .OnActivityResult((activity, requestCode, resultCode, data) => LogEvent(nameof(AndroidLifecycle.OnActivityResult), requestCode.ToString()))
                       .OnStart((activity) => LogEvent(nameof(AndroidLifecycle.OnStart)))
                       .OnCreate((activity, bundle) => LogEvent(nameof(AndroidLifecycle.OnCreate)))
                       .OnBackPressed((activity) => LogEvent(nameof(AndroidLifecycle.OnBackPressed)) && false)
                       .OnStop((activity) => LogEvent(nameof(AndroidLifecycle.OnStop))));
#elif IOS || MACCATALYST
                    events.AddiOS(ios => ios
                        .OnActivated((app) => LogEvent(nameof(iOSLifecycle.OnActivated)))
                        .OnResignActivation((app) => LogEvent(nameof(iOSLifecycle.OnResignActivation)))
                        .DidEnterBackground((app) => LogEvent(nameof(iOSLifecycle.DidEnterBackground)))
                        .WillTerminate((app) => LogEvent(nameof(iOSLifecycle.WillTerminate))));
#elif WINDOWS
                    events.AddWindows(windows => windows
                           .OnActivated((window, args) => LogEvent(nameof(WindowsLifecycle.OnActivated)))
                           .OnClosed((window, args) => LogEvent(nameof(WindowsLifecycle.OnClosed)))
                           .OnLaunched((window, args) => LogEvent(nameof(WindowsLifecycle.OnLaunched)))
                           .OnLaunching((window, args) => LogEvent(nameof(WindowsLifecycle.OnLaunching)))
                           .OnVisibilityChanged((window, args) => LogEvent(nameof(WindowsLifecycle.OnVisibilityChanged)))
                           .OnPlatformMessage((window, args) =>
                           {
                               if (args.MessageId == Convert.ToUInt32("031A", 16))
                               {
                                   // System theme has changed
                               }
                           }));
#endif
        });

        return builder;
    }

    static bool LogEvent(string eventName, string type = null)
    {
        _loggerInstance.Write($"App: {eventName}{(type == null ? string.Empty : $" ({type})")}");
        return true;
    }

}