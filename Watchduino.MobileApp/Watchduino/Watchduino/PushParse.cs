using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messier16.Forms.Plugin
{
    public class PushParse
    {
        public PushParse()
        {

        }

        public void ShowMessage(string text)
        {
            NotImplementedInReferenceAssembly();
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the Xam.Plugins.Settings NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
