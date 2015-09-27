namespace PDD.Models
{
    internal class Text
    {
        public string TextString;
        public bool IsHighlighted;

        public Text(string textString, bool isHighlighted)
        {
            TextString = textString;
            IsHighlighted = isHighlighted;
        }
    }
}
