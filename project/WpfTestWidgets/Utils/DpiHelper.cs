using System.Windows;
using System.Windows.Media;

namespace WpfTestWidgets.Utils
{
    class DpiHelper
    {
        public static Point GetDpi(Visual visual)
        {
			var source = PresentationSource.FromVisual(visual);
			return new Point(
                        96.0 * source.CompositionTarget.TransformToDevice.M11,
						96.0 * source.CompositionTarget.TransformToDevice.M22);
		}
        public static Point UpdateScreenPositionWidthCurrentDPI(Point point,Visual visual)
        {
			var dpi = GetDpi(visual);
			var currentDPI = new Point(96.0 / dpi.X, 96.0 / dpi.Y);
			return new Point(
						point.X * currentDPI.X, 
						point.Y * currentDPI.Y);
		}
	}
}
