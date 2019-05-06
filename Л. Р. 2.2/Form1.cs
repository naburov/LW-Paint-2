using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Л.Р._2._2
{
    public partial class frmMain : Form

    {
        public enum tools { brush, ellipse, rect, star, line, loupe, eraser };
        public tools Tool;
        public Color Color;
        public int brushWidth;
        public int childCount { get; set; }

        public int GetBrushWidth() => brushWidth;
        public Color GetColor() => Color;
        public tools GetTool() => Tool;
        public frmMain()

        {
            InitializeComponent();
        }

        private void tsmiCreate_Click(object sender, EventArgs e)
        {
            frmChild frm = new frmChild();
            frm.MdiParent = this;
            frm.Text += " "+childCount++.ToString();
            frm.Show();
        }



        private void tsmiSaveAS_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.SaveAs();
        private void btnEraser_Click(object sender, EventArgs e) => Tool = tools.eraser;
        private void btnEllipse_Click(object sender, EventArgs e) => Tool = tools.ellipse;
        private void btnLine_Click(object sender, EventArgs e) => Tool = tools.line;
        private void btnBrush_Click(object sender, EventArgs e) => Tool = tools.brush;
        private void btnStar_Click(object sender, EventArgs e) => Tool = tools.star;
        private void frmMain_Paint(object sender, PaintEventArgs e)
        {

        }
        private void каскадомToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);
        private void слеваНаправоToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileVertical);
        private void сверхуВнизToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);
        private void упорядочитьЗначкиToolStripMenuItem_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);
        private void эффект1ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function1();
        private void эффект2ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function2();
        private void эффект3ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function3();
        private void эффект4ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function4();
        private void эффект5ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function5();
        private void эффект6ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function6();
        private void эффект7ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function7();
        private void эффект8ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function8();
        private void эффект9ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function9();
        private void эффект10ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function10();
        private void эффект11ToolStripMenuItem_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.Function11();

        private void btnColor_Click(object sender, EventArgs e)
        {
            if (cd.ShowDialog() == DialogResult.OK)
                Color = cd.Color;
            btnColor.BackColor = Color;
        }

        private void TrackBar1_Scroll(object sender, EventArgs e) => brushWidth = trbarChangeWidth.Value;
        private void btnZoomIn_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.ZoomIn();
        private void btnZoomOut_Click(object sender, EventArgs e) => (ActiveMdiChild as frmChild)?.ZoomOut();

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnColor.BackColor = Color.Black;
            trbarChangeWidth.Value = 5;
            frmChild frm = new frmChild();
            frm.MdiParent = this;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Открытие изображения";
            ofd.Filter = "Bitmap (*.bmp)|*bmp| JPEG (*.jpeg)|*jpeg| Все файлы (*.*)|(*.*)";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                frm.Show();
                frm.OpenFile(ofd.FileName);
                //frm.bmp = (Bitmap)Image.FromFile(ofd.FileName);
                //frm.pdDraw.Invalidate();
                //frm.pdDraw.Refresh();
            }
            else frm.Dispose();

        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChild frm = ActiveMdiChild as frmChild;
            if (frm != null)
            {
                if (frm.Is_Saved)
                    this.Close();
                DialogResult tr = MessageBox.Show("Сохранить ли изображение в окне"+frm.Text+"?", Text, MessageBoxButtons.YesNoCancel);
                if (tr == DialogResult.OK)
                    сохранитьКакToolStripMenuItem_Click(sender, e);
            }
            else this.Close();
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChild frm = ActiveMdiChild as frmChild;
            if (frm == null)
                return;

            if (frm.Path != null)
                frm.Save(frm.Path);
            else
                tsmiSaveAS_Click(sender, e);

            try
            {
                frm.pdDraw.Image.Save(frm.Path);
            }
            catch (Exception)
            {
                if (!frm.Is_Saved) tsmiSaveAS_Click( sender,  e);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            childCount = 0;
            btnColor.BackColor = Color.Black;
            trbarChangeWidth.Value = 5;
            brushWidth = 5;
            Color = Color.Black;
            Tool = tools.brush;
        }

        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
