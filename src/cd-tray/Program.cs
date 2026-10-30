using System;
using System.Windows.Forms;

namespace CD_Tray
{
    internal static class Program
    {
        private const string Usage =
            "CD Tray - open or close the optical drive tray.\n\n" +
            "Usage: tray [command] [drive:]\n\n" +
            "  (none)    Open the tray, wait 1 second, then close it\n" +
            "  <n>       Open the tray, wait n seconds, then close it\n" +
            "  o         Open the tray\n" +
            "  c         Close the tray\n" +
            "  -h, /?    Show this help\n\n" +
            "The optional drive letter (e.g. E:) selects a drive; by default the\n" +
            "first optical drive is used.";

        [STAThread]
        private static int Main(string[] args)
        {
            try
            {
                var options = Options.Parse(args);
                if (options.ShowHelp)
                {
                    MessageBox.Show(Usage, "CD Tray", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return 0;
                }

                char? drive = options.Drive ?? TrayController.FindFirstOpticalDrive();
                if (drive == null)
                {
                    MessageBox.Show("No CD/DVD/BD drive found.", "CD Tray", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return 2;
                }

                using (var tray = new TrayController(drive.Value))
                {
                    switch (options.Action)
                    {
                        case TrayAction.Open:
                            tray.Open();
                            break;
                        case TrayAction.Close:
                            tray.Close();
                            break;
                        default:
                            tray.Open();
                            System.Threading.Thread.Sleep(options.Seconds * 1000);
                            tray.Close();
                            break;
                    }
                }
                return 0;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message + "\n\n" + Usage, "CD Tray", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "CD Tray", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 3;
            }
        }
    }
}
