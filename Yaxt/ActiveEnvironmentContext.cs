/**
 * Copyright 2009-2015 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 */

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
