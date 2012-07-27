using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SchemaTool.Forms;

namespace SchemaTool
{

    /// <summary>
    /// Tools that interact with the environment
    /// </summary>
    public interface IEnvironmentChild
    {
        ActiveEnvironmentContext Context { get; set; }
    }

    /// <summary>
    /// Active environment context
    /// </summary>
    public class ActiveEnvironmentContext
    {

        /// <summary>
        /// Fires when the status of the environment has changed
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Active object has changed
        /// </summary>
        public event EventHandler ActiveObjectChanged;

        private object activeObject;
        /// <summary>
        /// Gets or sets the active object
        /// </summary>
        public object ActiveObject
        {
            get { return activeObject; }
            set
            {
                activeObject = value;
                if (ActiveObjectChanged != null) ActiveObjectChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Requests a status change
        /// </summary>
        public void RequestStatusChange(object sender, StatusChangedEventArgs e)
        {
            if (StatusChanged != null)
                StatusChanged(sender, e);
        }

    }
}
