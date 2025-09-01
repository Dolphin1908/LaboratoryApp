using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LaboratoryApp.src.Core.Helpers
{
    public static class RichTextBoxBindingHelper
    {
        /// <summary>
        /// Gán FlowDocument cho RichTextBox để hỗ trợ binding hai chiều
        /// </summary>
        public static readonly DependencyProperty BindableDocumentProperty =
            DependencyProperty.RegisterAttached(
                "BindableDocument",
                typeof(FlowDocument),
                typeof(RichTextBoxBindingHelper),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnBindableDocumentChanged));

        public static void SetBindableDocument(DependencyObject element, FlowDocument value) => element.SetValue(BindableDocumentProperty, value);
        public static FlowDocument GetBindableDocument(DependencyObject element) => (FlowDocument)element.GetValue(BindableDocumentProperty);

        /// <summary>
        /// Thay đổi FlowDocument trong RichTextBox
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnBindableDocumentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is RichTextBox rtb)
            {
                rtb.TextChanged -= RichTextBox_TextChanged;

                rtb.Document = e.NewValue as FlowDocument ?? new FlowDocument();

                rtb.TextChanged += RichTextBox_TextChanged;
            }
        }

        private static void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is RichTextBox rtb)
            {
                // Update source property with current document instance
                SetBindableDocument(rtb, rtb.Document);
            }
        }
    }
}
