using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class RichTextBoxFormatHelper
    {
        // --- Xử lý sự kiện SelectionChanged để đồng bộ ngược ---
        private static void UpdateAttachedProperty(RichTextBox rtb, DependencyProperty prop, object value)
        {
            // Xóa handler tạm thời để tránh vòng lặp vô tận (Set property -> Trigger change -> Set property)
            rtb.SelectionChanged -= Rtb_SelectionChanged;
            rtb.SetCurrentValue(prop, value);
            rtb.SelectionChanged += Rtb_SelectionChanged;
        }

        private static void Rtb_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                // Đồng bộ FontSize
                var fontSize = rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty);
                if (fontSize != DependencyProperty.UnsetValue)
                    UpdateAttachedProperty(rtb, FontSizeProperty, (double)fontSize);

                // Đồng bộ Foreground
                var foreground = rtb.Selection.GetPropertyValue(TextElement.ForegroundProperty);
                if (foreground != DependencyProperty.UnsetValue && foreground is Brush brush)
                    UpdateAttachedProperty(rtb, ForegroundProperty, brush);

                // Đồng bộ Alignment (Lấy của đoạn văn bản hiện tại)
                var alignment = rtb.Selection.GetPropertyValue(Block.TextAlignmentProperty);
                if (alignment != DependencyProperty.UnsetValue)
                    UpdateAttachedProperty(rtb, AlignProperty, (TextAlignment)alignment);
            }
        }

        private static void AttachSelectionHandler(RichTextBox rtb)
        {
            // Đảm bảo chỉ gán handler 1 lần
            rtb.SelectionChanged -= Rtb_SelectionChanged;
            rtb.SelectionChanged += Rtb_SelectionChanged;
        }
        // --------------------------------------------------------

        #region FontSize
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.RegisterAttached(
                "FontSize",
                typeof(double),
                typeof(RichTextBoxFormatHelper),
                new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFontSizeChanged));

        public static double GetFontSize(DependencyObject obj) => (double)obj.GetValue(FontSizeProperty);
        public static void SetFontSize(DependencyObject obj, double value) => obj.SetValue(FontSizeProperty, value);

        private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                AttachSelectionHandler(rtb); // Đảm bảo đã hook sự kiện
                if (rtb.Selection.GetPropertyValue(TextElement.FontSizeProperty) is double current && Math.Abs(current - (double)e.NewValue) > 0.1)
                {
                    rtb.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, e.NewValue);
                }
            }
        }
        #endregion

        #region Foreground
        public static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.RegisterAttached(
                "Foreground",
                typeof(Brush),
                typeof(RichTextBoxFormatHelper),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnForegroundChanged));

        public static Brush GetForeground(DependencyObject obj) => (Brush)obj.GetValue(ForegroundProperty);
        public static void SetForeground(DependencyObject obj, Brush value) => obj.SetValue(ForegroundProperty, value);

        private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                AttachSelectionHandler(rtb);
                rtb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, e.NewValue);
            }
        }
        #endregion

        #region Align
        public static readonly DependencyProperty AlignProperty =
            DependencyProperty.RegisterAttached(
                "Align",
                typeof(TextAlignment),
                typeof(RichTextBoxFormatHelper),
                new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAlignChanged));

        public static TextAlignment GetAlign(DependencyObject obj) => (TextAlignment)obj.GetValue(AlignProperty);
        public static void SetAlign(DependencyObject obj, TextAlignment value) => obj.SetValue(AlignProperty, value);

        private static void OnAlignChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                AttachSelectionHandler(rtb);
                // WPF RichTextBox hỗ trợ ApplyPropertyValue cho Block.TextAlignment trực tiếp lên Selection
                // Không cần loop thủ công qua các paragraph
                rtb.Selection.ApplyPropertyValue(Block.TextAlignmentProperty, e.NewValue);
            }
        }
        #endregion
    }


    //public static class RichTextBoxFormatHelper
    //{
    //    #region FontSize
    //    public static readonly DependencyProperty FontSizeProperty =
    //        DependencyProperty.RegisterAttached(
    //            "FontSize",
    //            typeof(double),
    //            typeof(RichTextBoxFormatHelper),
    //            new FrameworkPropertyMetadata(12.0, FrameworkPropertyMetadataOptions.Inherits, OnFontSizeChanged));

    //    public static double GetFontSize(DependencyObject obj) => (double)obj.GetValue(FontSizeProperty);
    //    public static void SetFontSize(DependencyObject obj, double value) => obj.SetValue(FontSizeProperty, value);

    //    private static void OnFontSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is RichTextBox rtb)
    //        {
    //            rtb.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, (double)e.NewValue);
    //        }
    //    }
    //    #endregion

    //    #region Foreground
    //    public static readonly DependencyProperty ForegroundProperty =
    //        DependencyProperty.RegisterAttached(
    //            "Foreground",
    //            typeof(Brush),
    //            typeof(RichTextBoxFormatHelper),
    //            new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.Inherits, OnForegroundChanged));

    //    public static Brush GetForeground(DependencyObject obj) => (Brush)obj.GetValue(ForegroundProperty);
    //    public static void SetForeground(DependencyObject obj, Brush value) => obj.SetValue(ForegroundProperty, value);

    //    private static void OnForegroundChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is RichTextBox rtb && e.NewValue is Brush brush)
    //        {
    //            rtb.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
    //        }
    //    }
    //    #endregion

    //    #region Align
    //    public static readonly DependencyProperty AlignProperty =
    //        DependencyProperty.RegisterAttached(
    //            "Align",
    //            typeof(TextAlignment),
    //            typeof(RichTextBoxFormatHelper),
    //            new FrameworkPropertyMetadata(TextAlignment.Left, FrameworkPropertyMetadataOptions.Inherits, OnAlignChanged));

    //    public static TextAlignment GetAlign(DependencyObject obj) => (TextAlignment)obj.GetValue(AlignProperty);
    //    public static void SetAlign(DependencyObject obj, TextAlignment value) => obj.SetValue(AlignProperty, value);

    //    private static void OnAlignChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is RichTextBox rtb && e.NewValue is TextAlignment alignment)
    //        {
    //            var start = rtb.Selection.Start;
    //            var end = rtb.Selection.End;

    //            // Move through all paragraphs in the selection range
    //            var pointer = start;
    //            while (pointer != null && pointer.CompareTo(end) <= 0)
    //            {
    //                var paragraph = pointer.Paragraph;
    //                if (paragraph != null)
    //                {
    //                    paragraph.TextAlignment = alignment;
    //                    // Jump to the end of the paragraph to avoid looping infinitely
    //                    pointer = paragraph.ContentEnd.GetNextInsertionPosition(LogicalDirection.Forward);
    //                }
    //                else
    //                {
    //                    pointer = pointer.GetNextInsertionPosition(LogicalDirection.Forward);
    //                }
    //            }
    //        }
    //        //if (d is RichTextBox rtb && e.NewValue is TextAlignment alignment)
    //        //{
    //        //    var paragraph = rtb.Selection.Start.Paragraph;
    //        //    if (paragraph != null)
    //        //    {
    //        //        paragraph.TextAlignment = alignment;
    //        //    }
    //        //}
    //    }
    //    #endregion
    //}
}
