using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualStudio.Shell;
using System.ComponentModel.Composition;



namespace aharisu.IMEModeAdornment
{
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [ProvideOptionPageAttribute(typeof(OptionsPageGeneral), "IME Mode Adornment", "General", 101, 106, true)]
    [ProvideProfileAttribute(typeof(OptionsPageGeneral), "IME Mode Adornment", "Options", 101, 106, true, DescriptionResourceID = 101)]
    [InstalledProductRegistration("IMEModeAdornment", "IMEModeAdornment", "1.0")]
    [Guid(GuidStrings.GuidPackage)]
    public sealed class IMEOptionsPagePackage : Package
    {
    }

}