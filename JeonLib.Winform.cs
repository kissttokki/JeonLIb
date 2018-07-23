using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;


namespace JeonLib.Winform
{
    public static class Extention
    {
        public static void ShowMe(this object a)
        {
            System.Windows.Forms.MessageBox.Show(a.ToString());
        }
    }
}
