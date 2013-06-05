using System;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;

namespace Juick.Client.UI {
    public static class RichTextBlockBehavior {
        static readonly Regex UrlRegex = new Regex(@"http(s)?://([\w+?\.\w+])+([a-zA-Z0-9\~\!\@\#\$\%\^\&amp;\*\(\)_\-\=\+\\\/\?\.\:\;\'\,]*)?", RegexOptions.IgnoreCase);

        public static readonly DependencyProperty TextContentProperty =
            DependencyProperty.Register("TextContent", typeof(string), typeof(RichTextBlockBehavior), new PropertyMetadata(null, OnTextContent));

        public static Binding GetTextContent(DependencyObject obj) {
            return (Binding)obj.GetValue(TextContentProperty);
        }

        public static void SetTextContent(DependencyObject obj, Binding value) {
            obj.SetValue(TextContentProperty, value);
        }

        public static void OnTextContent(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var richText = (RichTextBlock)d;
            var textContent = (string)e.NewValue;

            richText.Blocks.Clear();

            if(string.IsNullOrEmpty(textContent)) {
                return;
            }
            var paragraph = new Paragraph();
            richText.Blocks.Add(paragraph);

            var matches = UrlRegex.Matches(textContent);
            if(matches.Count == 0) {
                paragraph.Inlines.Add(new Run { Text = textContent });
                return;
            }
            int index = 0;
            foreach(Match match in matches) {
                Uri uri = null;
                Uri.TryCreate(match.Value, UriKind.Absolute, out uri);
                if(match.Index > 0) {
                    var length = match.Index - index;
                    if(length > 0) {
                        paragraph.Inlines.Add(new Run { Text = textContent.Substring(index, length) });
                    }
                }

                var underline = new Underline();
                underline.Inlines.Add(new Run { Text = uri.Host });
                var linkContent = new TextBlock();
                linkContent.Inlines.Add(underline);
                var hyperlink = new HyperlinkButton {
                    NavigateUri = uri,
                    Content = linkContent,
                    Style = (Style)Application.Current.Resources["TextButtonStyle"],
                    Margin = new Thickness(0, 0, 0, -4)
                };
                paragraph.Inlines.Add(
                    new InlineUIContainer {
                        Child = hyperlink
                    });
                index = match.Index + match.Length;
            }
            if(index < textContent.Length - 1) {
                var lastRunText = textContent.Substring(index);
                if(lastRunText.Length > 0) {
                    paragraph.Inlines.Add(new Run { Text = lastRunText });
                }
            }
        }
    }
}
