using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace LoL_Offline
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
            this.RegionBox.SelectedIndex = 0;
        }

        private void blockIp(string Ip)
        {
            // Create a firewall rule to block the given ip
            var createRule =
                new ProcessStartInfo("c:\\windows\\system32\\netsh.exe",
                $"advfirewall firewall add rule name = \"N3RLoLOffline\" dir =out remoteip = {Ip} protocol = TCP action = block")
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    Verb = "runas"
                };

            try
            {
                Process.Start(createRule);
            }
            catch (Win32Exception e) {
                MessageBox.Show("Failed to create firewall rules!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var blockedRegion = getSelectedRegionIp();

            if (blockedRegion != "")
                blockIp(blockedRegion);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Remove any existing firewall rules created by this application
            var psi =
                new ProcessStartInfo("c:\\windows\\system32\\netsh.exe",
                "advfirewall firewall delete rule name = \"N3RLoLOffline\"")
                {
                    UseShellExecute = true,
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden
                };
            try
            {
                Process.Start(psi);
            }
            catch (Win32Exception) {
                MessageBox.Show("Failed to delete firewall rules!");
            }
        }

        private string getSelectedRegionIp()
        {
            switch (RegionBox.SelectedItem)
            {
                case "EUW": return "185.40.64.69";
                case "NA": return "192.64.174.69";
                case "EUNE": return "185.40.64.110";
                case "LAS": return "66.151.33.28";
                case "LAN": return "66.151.33.24";
                case "OCE": return "192.64.169.20";
                case "BR": return "66.151.33.20";
                case "TUR": return "185.40.64.105";
                case "RUS": return "185.40.64.99";
                default: return "";
            }
        }

        private void RegionBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.button1.Enabled = getSelectedRegionIp() != "";
        }
    }
} 
