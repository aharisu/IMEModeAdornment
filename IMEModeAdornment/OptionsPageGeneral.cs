/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Runtime.InteropServices;
using MSVSIP = Microsoft.VisualStudio.Shell;
using EnvDTE;
using System.ComponentModel.Composition;
using System.Xml;
using System.IO;
using System.Drawing;

namespace aharisu.IMEModeAdornment
{
    public enum AdornmentStyle
    {
        Line,
        LeftMarker,
    }

    [Guid(GuidStrings.GuidPageGeneral)]
    [ComVisible(true)]
    public class OptionsPageGeneral : MSVSIP.DialogPage
    {

        [Category("Style Options")]
        [Description("IME Adornment Style")]
        public AdornmentStyle Style
        {
            get
            {
                return Settings.Style;
            }
            set
            {
                Settings.Style = value;
            }
        }

        [Category("Style Options")]
        [Description("IME Enable Andronment Color")]
        public Color EnableIMEColor
        {
            get
            {
                return Settings.EnableIMEColor;
            }
            set
            {
                Settings.EnableIMEColor = value;
            }
        }

        [Category("Style Options")]
        [Description("IME Disable Andronment Color")]
        public Color DisableIMEColor
        {
            get
            {
                return Settings.DisableIMEColor;
            }
            set
            {
                Settings.DisableIMEColor = value;
            }
        }

    }
}
