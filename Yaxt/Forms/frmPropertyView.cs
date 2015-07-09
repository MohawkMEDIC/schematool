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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SchemaTool.Forms
{
    public partial class frmPropertyView : DockContent, IEnvironmentChild
    {
        public frmPropertyView()
        {
            InitializeComponent();
            this.DockAreas = DockAreas.DockBottom | DockAreas.DockLeft | DockAreas.DockRight | DockAreas.DockTop | DockAreas.Float;
        }

        private void pgMain_SelectedObjectsChanged(object sender, EventArgs e)
        {

        }

        #region IEnvironmentChild Members

        private ActiveEnvironmentContext context;

        /// <summary>
        /// Gets or sets the active context
        /// </summary>
        public ActiveEnvironmentContext Context
        {
            get { return context; }
            set
            {
                context = value;
                context.ActiveObjectChanged += new EventHandler(delegate(object sender, EventArgs e)
                    {
                        if (context.ActiveObject != null)
                            GenerateLabelContent(context.ActiveObject);
                        else
                            lblObjectType.Text = "";

                        pgMain.SelectedObject = context.ActiveObject;
                    });
            }
        }

        private void GenerateLabelContent(object p)
        {
            object[] desc = p.GetType().GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (desc.Length == 0)
                lblObjectType.Text = lblObjectType.Text = p.GetType().Name;
            else
                lblObjectType.Text = (desc[0] as DescriptionAttribute).Description;
        }

        #endregion
    }
}
