using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Gregstoll;
using PDD.Utility;
using SQLite;

namespace PDD.Models
{
    internal class Picture
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int GroupId { get; set; }
        public string Folder { get; set; }
        public string Source { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Image GetImage()
        {
            return new Image
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Width = Width,
                Height = Height,
                Margin = new Thickness(10, 10, 10, 10),
                Source = new BitmapImage(new Uri("ms-appx:/Assets/" + Folder + "/" + Source)),
            };
        }

        public static void AddImageItems(StackPanel stackPanel, List<Picture> images)
        {
            if (images == null || images.Count < 1)
            {
                return;
            }

            var wrapPanel = new UniversalWrapPanel
            {
                Orientation = Orientation.Horizontal,
                Width = Window.Current.Bounds.Width - 10
            };

            foreach (Picture imageItem in images)
            {
                wrapPanel.Children.Add(imageItem.GetImage());
            }
            stackPanel.Children.Add(wrapPanel);
        }

        public static List<Picture> GetPicturesByGroupId(int id)
        {
            return ReadDataHelper.Pictures.Where(i => i.GroupId == id).ToList();
        }
    }
}
