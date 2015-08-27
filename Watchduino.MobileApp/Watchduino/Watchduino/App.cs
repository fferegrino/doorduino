using Messier16.Forms.Plugin;
using Messier16.Forms.Plugin.Xparse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Watchduino
{
    public class App : Application
    {
        public CrossParse Parse { get; private set; }
        public App()
        {
            Parse = new CrossParse();
            Parse.InitializeClient(ParseKeys.AppId,
                ParseKeys.NetKey);
            Parse.SuscribeAsync();
            var btn = new Button() { Text = "Push me" };
            var lbl = new Label
            {
                XAlign = TextAlignment.Center,
                Text = "Welcome to Xamarin Forms!"
            };
            Parse.OnPushReceived += (dic) => { lbl.Text = dic["alert"].ToString(); };
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
