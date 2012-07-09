using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.ComponentModel;

namespace aharisu.IMEModeAdornment
{
    static class Settings
    {
        private static readonly string SettingsFileName;
        static Settings()
        {
            SettingsFileName = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "settings.xml");
        }

        public static Color EnableIMEColor = Color.FromArgb(80, 200, 75, 200);
        public static Color DisableIMEColor = Color.FromArgb(80, 150, 150, 150);
        public static  AdornmentStyle Style = AdornmentStyle.Line;

        public static void LoadSettingsFromStorage()
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(SettingsFileName);

                XmlElement settings = doc.DocumentElement;

               TypeConverter colorConverter =  TypeDescriptor.GetConverter(typeof(Color));
                EnableIMEColor = (Color)colorConverter.ConvertFromInvariantString(settings.GetAttribute("EnableIMEColor"));
                DisableIMEColor = (Color)colorConverter.ConvertFromInvariantString(settings.GetAttribute("DisableIMEColor"));

                TypeConverter enumConverter = TypeDescriptor.GetConverter(typeof(AdornmentStyle));
                Style = (AdornmentStyle)enumConverter.ConvertFromInvariantString(settings.GetAttribute("Style"));
            }
            catch (System.IO.IOException) { }
        }

        public static void SaveSettingsToStorage()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement settings = doc.CreateElement("settings");
            doc.AppendChild(settings);

           TypeConverter colorConverter =  TypeDescriptor.GetConverter(typeof(Color));
           settings.SetAttribute("EnableIMEColor", colorConverter.ConvertToInvariantString(EnableIMEColor));
           settings.SetAttribute("DisableIMEColor", colorConverter.ConvertToInvariantString(DisableIMEColor));

            TypeConverter enumConverter = TypeDescriptor.GetConverter(typeof(AdornmentStyle));
           settings.SetAttribute("Style", enumConverter.ConvertToInvariantString(Style));

            doc.Save(SettingsFileName);
        }

    }
}
