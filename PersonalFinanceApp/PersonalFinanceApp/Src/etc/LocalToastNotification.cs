using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Notifications;

namespace PersonalFinanceApp.Src.etc;
public class LocalToastNotification {
    private string _tile;
    public string Tile {
        get => _tile;
        set => _tile = value;
    }
    private string _content;
    public string Content {
        get => _content;
        set => _content = value;
    }

    private string _imageUrl;
    public string ImageUrl {
        get => _imageUrl;
        set => _imageUrl = value;
    }
    public LocalToastNotification() { }

    // Constructor for setting up initial values
    public LocalToastNotification(string tile, string content, string imageUrl = null) {
        _tile = tile;
        _content = content;
        _imageUrl = imageUrl;
    }

    public void ShowNotification() {
        var builder = new ToastContentBuilder()
            .AddArgument("meetingId", 9813)
            .AddText(Tile, hintMaxLines: 1)
            .AddText(Content);
        // Add image if URL is provided
        if (!string.IsNullOrEmpty(ImageUrl)) {
            builder.AddInlineImage(new Uri(ImageUrl));
        }

        // Handle toast activation to navigate when clicked
        builder.AddToastActivationInfo("action=launch", ToastActivationType.Foreground);
        //alarm
        var alarm = new ToastContentBuilder()
            .AddText("Notification text")
            .SetToastScenario(ToastScenario.Alarm)
            .AddButton(new ToastButton()
            .SetContent("Dismiss")
            .AddArgument("action", "dismiss"));
        //reminder
        var reminder = new ToastContentBuilder()
            .AddText("Notification text.")
            .SetToastScenario(ToastScenario.Reminder);



        // Show the toast notification

        //builder.Show();
        alarm.Show();
        //reminder.Show();
    }

}
