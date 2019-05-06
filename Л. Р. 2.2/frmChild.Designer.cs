namespace Л.Р._2._2
{
    partial class frmChild
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            this.pdDraw = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pdDraw)).BeginInit();
            this.SuspendLayout();
            // 
            // pdDraw
            // 
            this.pdDraw.BackColor = System.Drawing.SystemColors.Window;
            this.pdDraw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pdDraw.Location = new System.Drawing.Point(0, 0);
            this.pdDraw.Name = "pdDraw";
            this.pdDraw.Size = new System.Drawing.Size(691, 500);
            this.pdDraw.TabIndex = 0;
            this.pdDraw.TabStop = false;
            this.pdDraw.SizeChanged += new System.EventHandler(this.pdDraw_SizeChanged);
            this.pdDraw.Paint += new System.Windows.Forms.PaintEventHandler(this.pdDraw_Paint);
            this.pdDraw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pdDraw_MouseDown);
            this.pdDraw.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pdDraw_MouseMove);
            this.pdDraw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pdDraw_MouseUp);
            // 
            // frmChild
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(691, 500);
            this.Controls.Add(this.pdDraw);
            this.Name = "frmChild";
            this.Text = "Дочерняя форма";
            this.Activated += new System.EventHandler(this.frmChild_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmChild_FormClosing);
            this.Load += new System.EventHandler(this.frmChild_Load);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.frmChild_MouseUp);
            ((System.ComponentModel.ISupportInitialize)(this.pdDraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox pdDraw;
    }
}