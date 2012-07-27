using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeifenLuo.WinFormsUI.Docking;

namespace SchemaTool
{

    /// <summary>
    /// Status changed event args
    /// </summary>
    public class StatusChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The text of the status message
        /// </summary>
        public String Text { get; set; }
        /// <summary>
        /// The progress of the current action
        /// </summary>
        public Int32 Progress { get; set; }
    }
}
