using System;

namespace PDD.Models
{
    internal class MenuItem
    {
        internal int Id;
        internal Type Type;
        internal string Text;
        internal string Image;

        public MenuItem(int id, Type type, string text, string image)
        {
            Id = id;
            Type = type;
            Text = text;
            Image = image;
        }
    }
}
