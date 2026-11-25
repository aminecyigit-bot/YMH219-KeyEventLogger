using System;
using System.Windows.Forms;

namespace YMH219_KeyEventLogger
{
    public class KeyLogEntry
    {
        public DateTime Timestamp { get; }
        public Keys KeyCode { get; }
        public string KeyDisplay { get; }
        public string Modifiers { get; }

        public KeyLogEntry(Keys keyCode, Keys modifiers)
        {
            Timestamp = DateTime.Now;
            KeyCode = keyCode;
            KeyDisplay = FormatKeyName(keyCode);
            Modifiers = modifiers == Keys.None ? "-" : modifiers.ToString();
        }

        private string FormatKeyName(Keys key)
        {
            return key switch
            {
                Keys.Space => "SPACE (Boşluk)",
                Keys.Return => "ENTER",
                Keys.Back => "BACKSPACE",
                Keys.Escape => "ESC",
                Keys.Tab => "TAB",
                Keys.Capital => "CAPS LOCK",
                Keys.ShiftKey or Keys.LShiftKey or Keys.RShiftKey => "SHIFT",
                Keys.ControlKey or Keys.LControlKey or Keys.RControlKey => "CTRL",
                Keys.Menu or Keys.LMenu or Keys.RMenu => "ALT",
                _ => key.ToString()
            };
        }

        public override string ToString()
        {
            return $"[{Timestamp:dd.MM.yyyy HH:mm:ss.fff}]  Tuş: {KeyDisplay,-18} | Kombinasyon: {Modifiers}";
        }
    }
}