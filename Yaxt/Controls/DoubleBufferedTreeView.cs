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
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Everest.Workshop.SchemaTool
{
    /// <summary>
    /// Inspired by http://www.codeproject.com/KB/list/double-buffered-tree-view.aspx
    /// </summary>
    public class DoubleBufferedTreeView : TreeView
    {
        const int WM_PRINTCLIENT = 0x0318;
        const int PRF_CLIENT = 0x00000004;
        const int TV_FIRST = 0x1100;
        const int TVM_SETBKCOLOR = TV_FIRST + 29;
        const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
        const int TVS_EX_DOUBLEBUFFER = 0x0004;

        // Import the SendMessage command
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public DoubleBufferedTreeView()
        {
            // Enabled double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            // This only works on vista systems
            if (Environment.OSVersion.Version.Major < 6)
                SetStyle(ControlStyles.UserPaint, true);
        }

        private void UpdateExtendedStyles()
        {
            int Style = 0;

            if (DoubleBuffered)
                Style |= TVS_EX_DOUBLEBUFFER;

            if (Style != 0)
                SendMessage(Handle, TVM_SETEXTENDEDSTYLE, (IntPtr)TVS_EX_DOUBLEBUFFER, (IntPtr)Style);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UpdateExtendedStyles();
            if (!(Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor == 1)))
                SendMessage(Handle, TVM_SETBKCOLOR, IntPtr.Zero, (IntPtr)ColorTranslator.ToWin32(BackColor));
        }

        
        protected override void OnPaint(PaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint))
            {
                Message msg = new Message();
                msg.HWnd = Handle;
                msg.Msg = WM_PRINTCLIENT;
                msg.WParam = e.Graphics.GetHdc();
                msg.LParam = (IntPtr)PRF_CLIENT;
                DefWndProc(ref msg);
                e.Graphics.ReleaseHdc(msg.WParam);
            }
            base.OnPaint(e);
        }
    }
}
