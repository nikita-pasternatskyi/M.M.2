using System.Text;

namespace MykroFramework.Debugging
{
    public class DebugTextBlock
    {
        private StringBuilder _text = new StringBuilder();
        private string _previousText;

        public void AddText(string text, string color = null, bool bold = false, bool addLine = false)
        {
            if(color == null)
                color = DebugColors.COLOR_WHITE;
            if (bold)
                _text.Append(DebugTextArguments.BOLD_OPEN);
            _text.Append(DebugTextArguments.COLOR_OPEN);
            _text.Append(color);
            _text.Append(">");
            _text.Append(text);
            _text.Append(DebugTextArguments.COLOR_CLOSE); 
            if (bold)
                _text.Append(DebugTextArguments.BOLD_CLOSE);
            if (addLine)
                _text.AppendLine();
        }

        public void AddLine() => _text.AppendLine();

        public string DrawText()
        {
            var text = _text.ToString();
            if (string.IsNullOrEmpty(text))
                text = _previousText;
            _previousText = text;
            _text.Clear();
            return _previousText;
        }
    }
}
