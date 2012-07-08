using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Utilities;
using Microsoft.VisualStudio.Shell;

namespace aharisu.IMEModeAdornment
{
    [Export(typeof(IWpfTextViewCreationListener))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    internal sealed class IMEModeAdornmentAdornmentFactory : IWpfTextViewCreationListener
    {
        /// <summary>
        /// Defines the adornment layer for the scarlet adornment. This layer is ordered 
        /// after the selection layer in the Z-order
        /// </summary>
        [Export(typeof(AdornmentLayerDefinition))]
        [Name("IMEModeAdornment")]
        [Order(Before = PredefinedAdornmentLayers.Caret)]
        [TextViewRole(PredefinedTextViewRoles.Document)]
        public AdornmentLayerDefinition editorAdornmentLayer = null;

        public void TextViewCreated(IWpfTextView textView)
        {
            new IMEModeAdornment(textView);
        }
    }
}
