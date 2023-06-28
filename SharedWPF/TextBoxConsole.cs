using System.Text;
using System.Windows;
using System.IO;
using System.Windows.Controls;

namespace WPFLib
{
    public class TextBoxConsole : TextWriter
    { 
        private readonly TextBox tb;
       

        public TextBoxConsole(TextBox tb, bool scrollToEnd = true)
        {
            this.tb = tb;
            this.ScrollToEnd = scrollToEnd;
        }
        public bool ScrollToEnd { get; set; }
        public void Reset()
        { 
        tb.Clear(); 
        }

        public override void Write(char value)
        {
            tb.AppendText(value.ToString());
            if(ScrollToEnd) tb.ScrollToEnd();

        }

        public override void Write(string? value)
        {
            if (value != null)
            {
                tb.AppendText(value.ToString());
                if (ScrollToEnd) tb.ScrollToEnd();
            }
        }

        public override Encoding Encoding
        {
            get { return Encoding.ASCII; }
        }
       
      
    }
}
