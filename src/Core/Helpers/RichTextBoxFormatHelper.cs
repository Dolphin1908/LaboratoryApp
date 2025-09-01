using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class RichTextBoxFormatHelper
    {
        #region FontSize
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(
                "FontSize",
                typeof(double),
                typeof(RichTextBoxFormatHelper),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.Inherits, OnFontSizeChanged));

        public static double GetFontSize(DependencyObject obj) => (double)obj.GetValue(FontSizeProperty);
        public static void SetFontSize(DependencyObject obj, double value) => obj.SetValue(FontSizeProperty, value);

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                rtb.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, (double)e.NewValue);
            }
        }
        #endregion

        #region Foreground
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached(
                "Foreground",
                typeof(Brush),
                typeof(RichTextBoxFormatHelper),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits, OnForegroundChanged));

        public static Brush GetForeground(DependencyObject obj) => (Brush)obj.GetValue(ForegroundProperty);
        public static void SetForeground(DependencyObject obj, Brush value) => obj.SetValue(ForegroundProperty, value);

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb && e.NewValue is Brush brush)
            {
                rtb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
        }
        #endregion

    }
}
