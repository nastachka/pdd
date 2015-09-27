using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SQLite;

namespace PDD.Models
{
    internal class Rule
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int GroupId { get; set; }
        public string Text { get; set; }

        public TextBlock GetTextBlock()
        {
            return new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 10, 10, 10),
                TextWrapping = TextWrapping.Wrap,
                FontSize = 18,
                Text = Text,
            };
        }
    }
}
