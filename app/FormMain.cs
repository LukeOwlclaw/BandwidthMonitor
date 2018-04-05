using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Echevil;
using System.Runtime.InteropServices;

namespace Network_Monitor_Sample
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class FormMain : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label LabelDownload;
        private System.Windows.Forms.Label LabelUpload;
        private System.Windows.Forms.Label LableDownloadValue;
        private System.Windows.Forms.Label LabelUploadValue;
        private System.Windows.Forms.ListBox ListAdapters;
        private System.Windows.Forms.Timer TimerCounter;
        private System.ComponentModel.IContainer components;


        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,
                         int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]


        public static extern bool ReleaseCapture();
        public FormMain()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.Left = 1733;
            this.Top = 941;
            LabelDownload_Click(null, null);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.ListAdapters = new System.Windows.Forms.ListBox();
            this.LabelDownload = new System.Windows.Forms.Label();
            this.LabelUpload = new System.Windows.Forms.Label();
            this.LableDownloadValue = new System.Windows.Forms.Label();
            this.LabelUploadValue = new System.Windows.Forms.Label();
            this.TimerCounter = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // ListAdapters
            // 
            this.ListAdapters.Location = new System.Drawing.Point(153, 12);
            this.ListAdapters.Name = "ListAdapters";
            this.ListAdapters.Size = new System.Drawing.Size(208, 82);
            this.ListAdapters.TabIndex = 0;
            this.ListAdapters.SelectedIndexChanged += new System.EventHandler(this.ListAdapters_SelectedIndexChanged);
            // 
            // LabelDownload
            // 
            this.LabelDownload.Location = new System.Drawing.Point(0, -1);
            this.LabelDownload.Name = "LabelDownload";
            this.LabelDownload.Size = new System.Drawing.Size(41, 23);
            this.LabelDownload.TabIndex = 1;
            this.LabelDownload.Text = "Down:";
            this.LabelDownload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelDownload.Click += new System.EventHandler(this.LabelDownload_Click);
            // 
            // LabelUpload
            // 
            this.LabelUpload.Location = new System.Drawing.Point(0, 22);
            this.LabelUpload.Name = "LabelUpload";
            this.LabelUpload.Size = new System.Drawing.Size(41, 23);
            this.LabelUpload.TabIndex = 2;
            this.LabelUpload.Text = "Up:";
            this.LabelUpload.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.LabelUpload.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // LableDownloadValue
            // 
            this.LableDownloadValue.Location = new System.Drawing.Point(47, -1);
            this.LableDownloadValue.Name = "LableDownloadValue";
            this.LableDownloadValue.Size = new System.Drawing.Size(100, 23);
            this.LableDownloadValue.TabIndex = 3;
            this.LableDownloadValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LableDownloadValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // LabelUploadValue
            // 
            this.LabelUploadValue.Location = new System.Drawing.Point(47, 22);
            this.LabelUploadValue.Name = "LabelUploadValue";
            this.LabelUploadValue.Size = new System.Drawing.Size(100, 23);
            this.LabelUploadValue.TabIndex = 4;
            this.LabelUploadValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LabelUploadValue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // TimerCounter
            // 
            this.TimerCounter.Interval = 1000;
            this.TimerCounter.Tick += new System.EventHandler(this.TimerCounter_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(370, 104);
            this.Controls.Add(this.LabelUploadValue);
            this.Controls.Add(this.LableDownloadValue);
            this.Controls.Add(this.LabelUpload);
            this.Controls.Add(this.LabelDownload);
            this.Controls.Add(this.ListAdapters);
            this.Name = "FormMain";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Network Monitor";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.ResumeLayout(false);

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.Run(new FormMain());
        }

        private NetworkAdapter[] adapters;
        private NetworkMonitor monitor;

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            monitor = new NetworkMonitor();
            this.adapters = monitor.Adapters;

            if (adapters.Length == 0)
            {
                this.ListAdapters.Enabled = false;
                MessageBox.Show("No network adapters found on this computer.");
                return;
            }

            this.ListAdapters.Items.AddRange(this.adapters);

            object mainAdapter = null; ;
            foreach (var item in this.ListAdapters.Items)
            {
                var adapter = item as NetworkAdapter;
                if (adapter.Name.Contains("Ethernet"))
                {
                    mainAdapter = item;
                    //Ethernet has precedence. break here if found.
                    break;
                }
                else if (adapter.Name.Contains("Wireless"))
                {
                    mainAdapter = item;
                }
            }
            if (mainAdapter != null)
            {
                this.ListAdapters.SelectedItem = mainAdapter;
            }

        }

        private void ListAdapters_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            monitor.StopMonitoring();
            monitor.StartMonitoring(adapters[this.ListAdapters.SelectedIndex]);
            this.TimerCounter.Start();
        }

        private void TimerCounter_Tick(object sender, System.EventArgs e)
        {
            NetworkAdapter adapter = this.adapters[this.ListAdapters.SelectedIndex];
            this.LableDownloadValue.Text = String.Format("{0:n} kB/s", adapter.DownloadSpeedKbps);
            this.LabelUploadValue.Text = String.Format("{0:n} kB/s", adapter.UploadSpeedKbps);
        }

        private void LabelDownload_Click(object sender, EventArgs e)
        {
            if (!this.TopMost)
            {
                this.Width = 120;
                this.Height = 85;
                this.TopMost = true;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                this.ControlBox = false;
                this.ShowInTaskbar = false;
            }
            else
            {
                this.Width = 386;
                this.Height = 143;
                this.TopMost = false;
                this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
                this.ControlBox = true;
                this.ShowInTaskbar = true;
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                //HIDE WINDOW FROM ALT-TAB-LISTING:
                CreateParams cp = base.CreateParams;
                // turn on WS_EX_TOOLWINDOW style bit
                cp.ExStyle |= 0x80;
                return cp;
            }
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
    }
}
