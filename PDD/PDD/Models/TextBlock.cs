using System.Collections.Generic;

namespace PDD.Models
{
    class TextBlock
    {
        internal string Text;
        internal List<string> TextList;

        public TextBlock(string text, List<string> textList)
        {
            Text = text;
            TextList = textList;
        }
    }
}
