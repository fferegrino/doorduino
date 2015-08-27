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
        public App()
        {
            CrossParse cp = new CrossParse();
            cp.SuscribeAsync();
            cp.InitializeClient("SrsOx5oaNWLszFoOVjTAJY3XK56ZcfZsjQxYGTWK", "71OE1UfHgIYkLar1A2vJPt7HEzwlH6natzLGmHa2");
            var btn = new Button() { Text = "Push me" };
            var lbl = new Label
            {
                XAlign = TextAlignment.Center,
                Text = "Welcome to Xamarin Forms!"
            };
            cp.OnPushReceived += (dic) => { lbl.Text = dic["alert"].ToString(); };
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
            // Handle when your app starts
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
