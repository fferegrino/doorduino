using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Parse;
using Android.Support.V4.App;

namespace Watchduino.Droid
{
    [Application(Name = "watchduino.droid.ParseApplication")]
    class ParseApplication : Application
    {
        public ParseApplication(IntPtr handle, JniHandleOwnership ownerShip)
            : base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();

            ParseClient.Initialize(ParseKeys.AppId, ParseKeys.NetKey);
            ParsePush.ParsePushNotificationReceived += ParsePush_ParsePushNotificationReceived;
        }

        void ParsePush_ParsePushNotificationReceived(object sender, ParsePushNotificationEventArgs e)
        {

            var intent = new Intent(Context, typeof(MainActivity));
            PendingIntent pendingIntent = PendingIntent.GetActivity(Context, 0, intent, PendingIntentFlags.OneShot);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(Context)
                .SetContentTitle("Doorduino")
                .SetAutoCancel(true)
                .SetContentText(e.Payload["alert"].ToString())
                .SetPriority((int)NotificationPriority.High)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetCategory(Notification.CategoryAlarm)
                .SetContentIntent(pendingIntent)
                .SetDefaults((int)(NotificationDefaults.Sound | NotificationDefaults.Vibrate))
                .SetSmallIcon(Android.Resource.Drawable.IcMenuSend);

            var notification = builder.Build();
            var notificationManager = (NotificationManager)Context.GetSystemService(Context.NotificationService);
            notificationManager.Notify(0, notification);
        }
    }
}