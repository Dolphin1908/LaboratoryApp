using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class RichTextBoxBindingHelper
    {
        // Thay vì bind FlowDocument, ta bind chuỗi XAML (dạng string)
        // Điều này giúp dễ dàng lưu xuống Database và tuân thủ MVVM hơn.
        public static readonly DependencyProperty DocumentXamlProperty =
            DependencyProperty.RegisterAttached(
                "DocumentXaml",
                typeof(string),
                typeof(RichTextBoxBindingHelper),
                new FrameworkPropertyMetadata(
                    null,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnDocumentXamlChanged));

        public static string GetDocumentXaml(DependencyObject obj) => (string)obj.GetValue(DocumentXamlProperty);
        public static void SetDocumentXaml(DependencyObject obj, string value) => obj.SetValue(DocumentXamlProperty, value);

        private static void OnDocumentXamlChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is RichTextBox rtb)
            {
                // Đảm bảo luôn gỡ bỏ handler cũ trước khi xử lý để tránh memory leak hoặc double event
                rtb.TextChanged -= Rtb_TextChanged;

                string currentXaml = string.Empty;
                try
                {
                    // Lấy nội dung hiện tại an toàn
                    if (rtb.Document != null)
                        currentXaml = XamlWriter.Save(rtb.Document);
                }
                catch { }

                string newXaml = (string)e.NewValue;

                // Case 1: XAML rỗng hoặc null -> Clear RichTextBox
                if (string.IsNullOrEmpty(newXaml))
                {
                    if (rtb.Document != null)
                    {
                        rtb.Document.Blocks.Clear();
                    }
                    else
                    {
                        rtb.Document = new FlowDocument();
                    }
                }
                // Case 2: XAML có nội dung -> Load vào
                else if (!AreXamlStringsEqual(currentXaml, newXaml))
                {
                    try
                    {
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(newXaml)))
                        {
                            var doc = (FlowDocument)XamlReader.Load(stream);

                            // Set Document
                            rtb.Document = doc;
                        }
                    }
                    catch
                    {
                        rtb.Document = new FlowDocument();
                    }
                }

                // QUAN TRỌNG: Đăng ký lại sự kiện TextChanged để lắng nghe người dùng nhập liệu
                // Việc này bây giờ sẽ luôn chạy vì OnDocumentXamlChanged đã được kích hoạt
                rtb.TextChanged += Rtb_TextChanged;
            }

            //if (d is RichTextBox rtb)
            //{
            //    // Tránh lặp vô tận: Nếu XAML mới giống hệt nội dung đang có thì không load lại
            //    string currentXaml = XamlWriter.Save(rtb.Document);
            //    string newXaml = (string)e.NewValue;

            //    if (string.IsNullOrEmpty(newXaml))
            //    {
            //        rtb.Document.Blocks.Clear();
            //        return;
            //    }

            //    // Chỉ load lại nếu thực sự khác biệt (tránh reset con trỏ chuột khi đang gõ)
            //    if (!AreXamlStringsEqual(currentXaml, newXaml))
            //    {
            //        try
            //        {
            //            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(newXaml)))
            //            {
            //                var doc = (FlowDocument)XamlReader.Load(stream);

            //                // Ngắt event handler trước khi gán để tránh trigger ngược lại ViewModel
            //                rtb.TextChanged -= Rtb_TextChanged;
            //                rtb.Document = doc;
            //                rtb.TextChanged += Rtb_TextChanged;
            //            }
            //        }
            //        catch
            //        {
            //            rtb.Document = new FlowDocument();
            //        }
            //    }

            //    // Đảm bảo event TextChanged luôn được đăng ký
            //    rtb.TextChanged -= Rtb_TextChanged;
            //    rtb.TextChanged += Rtb_TextChanged;
            //}
        }

        private static void Rtb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                // Tạm thời gỡ bỏ listener để tránh việc SetDocumentXaml kích hoạt lại OnDocumentXamlChanged gây vòng lặp (dù đã có check string equal nhưng cẩn thận vẫn hơn)
                rtb.TextChanged -= Rtb_TextChanged;

                try
                {
                    string xaml = XamlWriter.Save(rtb.Document);
                    SetDocumentXaml(rtb, xaml);
                }
                finally
                {
                    // Gắn lại listener
                    rtb.TextChanged += Rtb_TextChanged;
                }
            }

            //if (sender is RichTextBox rtb)
            //{
            //    // Khi người dùng gõ, cập nhật ngược lại XAML string vào ViewModel
            //    string xaml = XamlWriter.Save(rtb.Document);
            //    SetDocumentXaml(rtb, xaml);
            //}
        }

        // Hàm so sánh chuỗi tương đối để tránh reload không cần thiết
        private static bool AreXamlStringsEqual(string xaml1, string xaml2)
        {
            if (string.IsNullOrEmpty(xaml1) && string.IsNullOrEmpty(xaml2)) return true;
            if (string.IsNullOrEmpty(xaml1) || string.IsNullOrEmpty(xaml2)) return false;
            return string.Equals(xaml1, xaml2, StringComparison.Ordinal);

            //// So sánh đơn giản, có thể cải thiện bằng cách so sánh Hash nếu chuỗi quá dài
            //return string.Equals(xaml1, xaml2, StringComparison.Ordinal);
        }
    }

    //public static class RichTextBoxBindingHelper
    //{
    //    /// <summary>
    //    /// Gán FlowDocument cho RichTextBox để hỗ trợ binding hai chiều
    //    /// </summary>
    //    public static readonly DependencyProperty BindableDocumentProperty =
    //        DependencyProperty.RegisterAttached(
    //            "BindableDocument",
    //            typeof(FlowDocument),
    //            typeof(RichTextBoxBindingHelper),
    //            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableDocumentChanged));

    //    public static void SetBindableDocument(DependencyObject element, FlowDocument value) => element.SetValue(BindableDocumentProperty, value);
    //    public static FlowDocument GetBindableDocument(DependencyObject element) => (FlowDocument)element.GetValue(BindableDocumentProperty);

    //    /// <summary>
    //    /// Thay đổi FlowDocument trong RichTextBox
    //    /// </summary>
    //    /// <param name="d"></param>
    //    /// <param name="e"></param>
    //    private static void OnBindableDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    //    {
    //        if (d is RichTextBox rtb)
    //        {
    //            rtb.TextChanged -= RichTextBox_TextChanged;

    //            rtb.Document = e.NewValue as FlowDocument ?? new FlowDocument();

    //            rtb.TextChanged += RichTextBox_TextChanged;
    //        }
    //    }

    //    private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
    //    {
    //        if (sender is RichTextBox rtb)
    //        {
    //            // Update source property with current document instance
    //            SetBindableDocument(rtb, rtb.Document);
    //        }
    //    }
    //}
}
