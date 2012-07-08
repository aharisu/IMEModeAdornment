using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Editor;
using System;
using System.Windows.Input;
using Microsoft.VisualStudio.Text.Formatting;
using System.Windows;
using Microsoft.VisualStudio.Text;
using System.ComponentModel.Composition;

namespace aharisu.IMEModeAdornment
{
    class IMEModeAdornment
    {
        private const double LeftMarkerWidth = 20;

        private OptionsPageGeneral _optionsPageGeneral;

        private bool _hasFocus = false;
        private bool _enableIME = false;

        private Image _image;
        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;

        public IMEModeAdornment(IWpfTextView view, OptionsPageGeneral options)
        {
            _optionsPageGeneral = options;
            _view = view;

            InputMethod.Current.ImeState = InputMethodState.DoNotCare;
            _enableIME = InputMethod.Current.ImeState == InputMethodState.On;
            InputMethod.Current.StateChanged += new InputMethodStateChangedEventHandler(Current_StateChanged);

            _adornmentLayer = view.GetAdornmentLayer("IMEModeAdornment");

            _view.Caret.PositionChanged += HighlightLine;
            _view.LayoutChanged += HighlightLine;
            _view.Selection.SelectionChanged += HighlightLine;

            _view.GotAggregateFocus += new EventHandler(_view_GotAggregateFocus);
            _view.LostAggregateFocus += new EventHandler(_view_LostAggregateFocus);
        }

        void Current_StateChanged(object sender, InputMethodStateChangedEventArgs e)
        {
            if (e.IsImeStateChanged)
            {
                _enableIME = InputMethod.Current.ImeState == InputMethodState.On;
                _image = null;
                HighlightLine(null, null);
            }
        }

        void _view_LostAggregateFocus(object sender, EventArgs e)
        {
            _hasFocus = false;
            _image = null;
            HighlightLine(null, null);
        }

        void _view_GotAggregateFocus(object sender, EventArgs e)
        {
            _hasFocus = true;
            _image = null;
            HighlightLine(null, null);
        }

        private void HighlightLine(object sender, EventArgs e)
        {
            ITextViewLine containingTextViewLine = _view.Caret.ContainingTextViewLine;
            if (containingTextViewLine != null)
            {
                if (containingTextViewLine.VisibilityState == VisibilityState.Hidden ||
                    containingTextViewLine.VisibilityState == VisibilityState.Unattached ||
                    !_view.Selection.IsEmpty ||
                    !_hasFocus)
                {
                    _image = null;
                    _adornmentLayer.RemoveAllAdornments();
                    return;
                }
                else
                {
                    if (_image == null ||
                        containingTextViewLine.Height != _image.Source.Height ||
                        _view.ViewportWidth + 2 != _image.Source.Width)
                    {
                        _adornmentLayer.RemoveAllAdornments();

                        System.Drawing.Color color;
                        if (_enableIME)
                        {
                            color = _optionsPageGeneral.EnableIMEColor;
                        }
                        else
                        {
                            color = _optionsPageGeneral.DisableIMEColor;
                        }
                        Brush backgroundBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.G));
                        backgroundBrush.Freeze();

                        Geometry rectangleGeometry = new RectangleGeometry(new Rect(new Size(
                            _optionsPageGeneral.Style == AdornmentStyle.Line ? _view.ViewportWidth + 1 : LeftMarkerWidth, //width
                            containingTextViewLine.Height - 1)));
                        GeometryDrawing geometryDrawing = new GeometryDrawing(backgroundBrush, new Pen(backgroundBrush, 1), rectangleGeometry);
                        geometryDrawing.Freeze();
                        DrawingImage drawingImage = new DrawingImage(geometryDrawing);
                        drawingImage.Freeze();
                        _image = new Image();
                        _image.Source = drawingImage;
                        _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, _image, null);
                    }

                    Canvas.SetLeft(_image, _optionsPageGeneral.Style == AdornmentStyle.Line ? _view.ViewportLeft + 1 : 1);
                    Canvas.SetTop(_image, containingTextViewLine.Top);
                    return;
                }
            }
        }

    }
}