namespace Intersect.Editor.Forms.Editors
{
    partial class FrmMusicPlayer
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.btnPlayStop = new DarkUI.Controls.DarkButton();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnClose = new DarkUI.Controls.DarkButton();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlTop
            //
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.pnlTop.Controls.Add(this.btnPlayStop);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.pnlTop.Size = new System.Drawing.Size(420, 42);
            this.pnlTop.TabIndex = 0;
            //
            // btnPlayStop
            //
            this.btnPlayStop.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPlayStop.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnPlayStop.Location = new System.Drawing.Point(8, 6);
            this.btnPlayStop.Name = "btnPlayStop";
            this.btnPlayStop.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.btnPlayStop.Size = new System.Drawing.Size(90, 30);
            this.btnPlayStop.TabIndex = 0;
            this.btnPlayStop.Text = "▶  Play";
            this.btnPlayStop.Click += new System.EventHandler(this.btnPlayStop_Click);
            //
            // lstFiles
            //
            this.lstFiles.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom
                | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.lstFiles.BackColor = System.Drawing.Color.FromArgb(60, 63, 65);
            this.lstFiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFiles.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(0, 42);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(420, 348);
            this.lstFiles.TabIndex = 1;
            this.lstFiles.SelectedIndexChanged += new System.EventHandler(this.lstFiles_SelectedIndexChanged);
            //
            // btnClose
            //
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            this.btnClose.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnClose.Location = new System.Drawing.Point(320, 398);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            //
            // FrmMusicPlayer
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(45, 45, 48);
            this.ClientSize = new System.Drawing.Size(420, 440);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "FrmMusicPlayer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Music Player (BGM)";
            this.Load += new System.EventHandler(this.FrmMusicPlayer_Load);
            this.pnlTop.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private DarkUI.Controls.DarkButton btnPlayStop;
        private System.Windows.Forms.ListBox lstFiles;
        private DarkUI.Controls.DarkButton btnClose;
    }
}
