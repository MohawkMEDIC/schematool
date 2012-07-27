using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchemaTool
{
    public class SelectionChangedEventArgs : EventArgs
    {
        public object SelectedObject { get; set; }
    }
}
