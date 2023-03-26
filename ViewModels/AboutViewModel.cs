using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using System.Windows.Input;

namespace Notes.ViewModels;

internal class AboutViewModel
{
    public string Title => AppInfo.Name;
    public string Version => AppInfo.VersionString;
    public string MoreInfoUrl => "https://aka.ms/maui";
    public string Message => "This app is written in XAML and C# with .NET MAUI.";
    public ICommand ShowMoreInfoCommand { get; }
    public ICommand ShowNotificationCommand { get; }


    public AboutViewModel()
    {
        ShowMoreInfoCommand = new AsyncRelayCommand(ShowMoreInfo);
        ShowNotificationCommand = new AsyncRelayCommand(Notification_Test);
        LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
    }

    private void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
    {
        if(e.IsDismissed) {
            Shell.Current.DisplayAlert("Notes","You dismissed me !","cancel");
        }
        else if(e.IsTapped) {
            Shell.Current.DisplayAlert("Notes",$"Thank You! {DateTime.Now}", "ok");
        }

    }

    async Task ShowMoreInfo() =>
        await Launcher.Default.OpenAsync(MoreInfoUrl);

    async Task Notification_Test()
    {
        var request = new NotificationRequest
        {
            NotificationId = 1999,
            Title = "Notes",
            Subtitle = "Your Notes",
            Description = "Please check your notes",
            BadgeNumber = 1234,
            CategoryType = NotificationCategoryType.Reminder,
            Schedule = new NotificationRequestSchedule
            {
                NotifyTime = DateTime.Now.AddSeconds(5),
                NotifyRepeatInterval = TimeSpan.FromDays(1)
            }
        };

        await LocalNotificationCenter.Current.Show(request);

    }

}