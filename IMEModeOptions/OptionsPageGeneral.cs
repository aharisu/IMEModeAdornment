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
        private static readonly string SettingsFileName;
        static OptionsPageGeneral()
        {
            SettingsFileName = Path.Combine(
                Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "settings.xml");
        }

        private AdornmentStyle _style = AdornmentStyle.Line;

        private Color _enableIMEColor = Color.FromArgb(80, 200, 75, 200);
        private Color _disableIMEColor = Color.FromArgb(80, 150, 150, 150);

        private EventHandler _settingsChanged;
        [Browsable(false)]
        public event EventHandler SettingsChanged
        {
            add { _settingsChanged += value; }
            remove { _settingsChanged -= value; }
        }

        [Category("Style Options")]
        [Description("IME Adornment Style")]
        public AdornmentStyle Style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }

        [Category("Style Options")]
        [Description("IME Enable Andronment Color")]
        public Color EnableIMEColor
        {
            get
            {
                return _enableIMEColor;
            }
            set
            {
                _enableIMEColor = value;
            }
        }

        [Category("Style Options")]
        [Description("IME Disable Andronment Color")]
        public Color DisableIMEColor
        {
            get
            {
                return _disableIMEColor;
            }
            set
            {
                _disableIMEColor = value;
            }
        }

        public override void LoadSettingsFromStorage()
        {
            try
            {
                object automationObject = this.AutomationObject;
                XmlDocument doc = new XmlDocument();
                doc.Load(SettingsFileName);

                XmlElement settings = doc.DocumentElement;
                if (settings != null)
                {
                    Attribute[] visible = new Attribute[] { DesignerSerializationVisibilityAttribute.Visible };
                    foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(automationObject, visible))
                    {
                        TypeConverter converter = prop.Converter;
                        if (!converter.CanConvertTo(typeof(string)) || !converter.CanConvertFrom(typeof(string)))
                        {
                            continue;
                        }

                        string value = settings.GetAttribute(prop.Name);
                        if (value != null && value != "")
                        {
                            prop.SetValue(automationObject,
                                converter.ConvertFromInvariantString(value));
                        }
                    }
                }
            }
            catch (System.IO.IOException) { }
        }

        public override void SaveSettingsToStorage()
        {
            object automationObject = this.AutomationObject;

            XmlDocument doc = new XmlDocument();
            XmlElement settings = doc.CreateElement("settings");
            doc.AppendChild(settings);

            Attribute[] visible = new Attribute[] { DesignerSerializationVisibilityAttribute.Visible };
            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(automationObject, visible))
            {
                TypeConverter converter = prop.Converter;
                if (!converter.CanConvertTo(typeof(string)) || !converter.CanConvertFrom(typeof(string)))
                {
                    continue;
                }

                settings.SetAttribute(prop.Name,
                    converter.ConvertToInvariantString(prop.GetValue(automationObject)));
            }

            doc.Save(SettingsFileName);
        }

    }
}
