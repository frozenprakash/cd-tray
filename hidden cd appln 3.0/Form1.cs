using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Runtime.InteropServices;
using System.IO;

namespace CD_Tray
{
    public partial class Form1 : Form
    {
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(string cmd, StringBuilder bfr, Int32 bfrsize, IntPtr ptr);

        public Form1()
        {
            InitializeComponent();
        }

        string[] args = Environment.GetCommandLineArgs();
        
        private void Form1_Load(object sender, EventArgs e)
        {
            //if first args is string
            string cmd = null, cmd1 = null;
            if (args.Count() > 1)
                cmd = cmd1 = args[1];
            
            //if first args is number
            int secs = 0;
            bool issecs = int.TryParse(cmd, out secs);
            if (issecs == true)
                secs = Convert.ToInt32(Convert.ToString(args[1]));
            else
                secs = 0;
            
            //getting the first CDRom drive
            DriveInfo[] drives = DriveInfo.GetDrives();
            string z = "";
            char f = '.';
            foreach (DriveInfo drive in drives)
            {
                if (Convert.ToString(drive.DriveType).CompareTo("CDRom") == 0)
                {
                    z = drive.Name;
                    break;
                }
            }
            
            foreach (char c in z)
            {
                f = c;
                break;
            }

            if (f != '.')
            {
                mciSendString("open " + f + ": type CDAudio alias driveE", null, 0, IntPtr.Zero);

                if (cmd == null || secs!=0)
                {
                    mciSendString("set driveE door open", null, 0, IntPtr.Zero);
                    if (secs == 0)
                        secs = 1;
                    Thread.Sleep(secs*1000);
                    mciSendString("set driveE door closed", null, 0, IntPtr.Zero);
                }
 
                else if (cmd1 == "o")
                    mciSendString("set driveE door open", null, 0, IntPtr.Zero);

                else if (cmd1 == "c")
                    mciSendString("set driveE door closed", null, 0, IntPtr.Zero);
            }
            else
                MessageBox.Show("No Active CD/DVD/BR drive found", "Error");
            Close();
        }
    }
}
