using Messier16.Forms.Plugin.Xparse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Watchduino
{
    public class App : Application
    {
        public App()
        {
            var btn = new Button() { Text = "Suscribir" };
            var lbl = new Label
            {
                XAlign = TextAlignment.Center,
                Text = "Welcome to Xamarin Forms!"
            };
            btn.Clicked += async (sender, args) =>
            {
                await CrossParse.SuscribeAsync();
                lbl.Text = "Listo";
                CrossParse.AddParsePushNotificationReceivedListener();
                CrossParse.OnPushReceived += (s, a) =>
                {
                    lbl.Text = a.Payload["alert"].ToString();
                };
            };
            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children = {
                        lbl,
                        btn
                    }
                }
            };
        }

        protected override void OnStart()
        {
            //CrossParse.InitializeClient(ParseKeys.AppId, ParseKeys.NetKey);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
