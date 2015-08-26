using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Messier16.Forms.Plugin
{
    public class PushParse
    {
        public PushParse()
        {

        }

        public void ShowMessage(string text)
        {
            MessageBox.Show(string.Empty, text, MessageBoxButton.OK);
        }
        
    }
}
