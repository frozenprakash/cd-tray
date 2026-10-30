using System;

namespace CD_Tray
{
    internal enum TrayAction
    {
        OpenThenClose,
        Open,
        Close,
    }

    internal sealed class Options
    {
        public TrayAction Action { get; private set; } = TrayAction.OpenThenClose;
        public int Seconds { get; private set; } = 1;
        public char? Drive { get; private set; }
        public bool ShowHelp { get; private set; }

        public static Options Parse(string[] args)
        {
            var options = new Options();
            foreach (string raw in args)
            {
                string arg = raw.Trim();
                string lower = arg.ToLowerInvariant();

                if (lower == "-h" || lower == "--help" || lower == "/?" || lower == "/h" || lower == "help")
                    options.ShowHelp = true;
                else if (lower == "o" || lower == "open")
                    options.Action = TrayAction.Open;
                else if (lower == "c" || lower == "close")
                    options.Action = TrayAction.Close;
                else if (int.TryParse(arg, out int seconds) && seconds > 0)
                {
                    options.Action = TrayAction.OpenThenClose;
                    options.Seconds = seconds;
                }
                else if (arg.Length >= 1 && arg.Length <= 2 && char.IsLetter(arg[0]) && (arg.Length == 1 || arg[1] == ':'))
                    options.Drive = char.ToUpperInvariant(arg[0]);
                else
                    throw new ArgumentException("Unrecognised argument: " + raw);
            }
            return options;
        }
    }
}
