using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Л.Р._2._2
{
    public partial class frmChild : Form
    {
        public bool Is_Saved { get; set; }
        public Color Color { get; set; }
        public int brushWidth { get; set; }
        public string Path { get; set; }
        int old_X, old_Y;
        public Bitmap bmp;
        static Point[] arr;
        static Point beg = new Point(-1, -1), end;
        static Pen pen;
        static Rectangle rect;
        static Point[] starPoints = new Point[10];
        static bool is_changed;
        static bool scale;
        bool is_file_changed;
        

        public frmChild()
        {
            InitializeComponent();
        }
        

        private void pdDraw_MouseMove(object sender, MouseEventArgs e)
        {
            
            frmMain.tools Tool = ((frmMain)ParentForm).GetTool();

            if (e.Button == MouseButtons.Left)
            {
                Image bg = pdDraw.Image;
                Graphics g = Graphics.FromImage(bmp);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //if (bg != null) g.DrawImageUnscaled(bg, new Point(0, 0));

                switch (((frmMain)ParentForm).GetTool())
                {
                    case frmMain.tools.brush:
                        Array.Resize(ref arr, arr.Length + 1);
                        arr[arr.Length - 1] = new Point(e.X, e.Y);
                        if (arr.Length > 2)
                            g.DrawCurve(pen, arr);
                        is_changed = true;
                        break;
                    case frmMain.tools.ellipse:
                        beg = new Point(Math.Min(old_X, e.X), Math.Min(old_Y, e.Y));
                        int width = Math.Abs(old_X - e.X);
                        int height = Math.Abs(old_Y - e.Y);
                        rect = new Rectangle(beg, new Size(width, height));
                        is_changed = true;
                        
                        pdDraw.Refresh();
                        break;
                    case frmMain.tools.star:
                        Point middle = new Point(Math.Abs(e.X / 2 + old_X / 2), Math.Abs(e.Y / 2 + old_Y / 2));     //Задать центр звезды
                        int r = Math.Abs(middle.X - e.X);                                   //Радиус круга
                        double aoutR = -Math.PI / 2;                                                  //Начальный угол для внешних точек
                        double ainR = -Math.PI / 2 + 2 * Math.PI / 10;                                                   //Начальный угол для внутренних точек
                        for (int i = 0; i < 9; i++)
                        {
                            starPoints[i++] = new Point(middle.X + (int)(r * Math.Cos(aoutR)), middle.Y + (int)(r * Math.Sin(aoutR)));
                            starPoints[i] = new Point(middle.X + (int)(r / 3 * Math.Cos(ainR)), middle.Y + (int)(r / 3 * Math.Sin(ainR)));
                            aoutR += 2 * Math.PI / 5; ainR += 2 * Math.PI / 5;
                        }
                        is_changed = true;
                        
                        break;
                    case frmMain.tools.eraser:
                        Pen ePen = new Pen(Color.White, pen.Width);
                        Array.Resize(ref arr, arr.Length + 1);
                        arr[arr.Length - 1] = new Point(e.X, e.Y);
                        if (arr.Length > 2)
                            g.DrawCurve(ePen, arr);
                        is_changed = true;
                       
                        break;
                    case frmMain.tools.line:
                        end = e.Location;
                        is_changed = true;
                        
                        pdDraw.Refresh();
                        break;
                }
                pdDraw.Image = bmp;
            }
        }

    

        private void frmChild_Activated(object sender, EventArgs e)
        {
            var frmMain = (frmMain)ParentForm;
            pen = new Pen(frmMain.GetColor(), frmMain.GetBrushWidth());
            rect = new Rectangle();
            starPoints = new Point[10];
            end = new Point();
            //var temptool = (this.ParentForm as frmMain).Tool;
            //(this.ParentForm as frmMain).Tool = frmMain.tools.brush;
            ////Graphics g = pdDraw.CreateGraphics();
            ////g.Clear(Color.Wheat);
            //pdDraw.BackgroundImage = bmp;

        }
        private void frmChild_Load(object sender, EventArgs e)
        {
            is_file_changed = false;
            var frmMain = (frmMain)ParentForm;
            //pdDraw.SizeMode = PictureBoxSizeMode.Normal;
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            bmp = new Bitmap(pdDraw.Width, pdDraw.Height);
            //background = new Bitmap(pdDraw.Width, pdDraw.Height);
            pdDraw.Image = bmp;
            pdDraw.BackgroundImage = bmp;
            pen = new Pen(frmMain.GetColor(), frmMain.GetBrushWidth());
            pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void pdDraw_MouseDown(object sender, MouseEventArgs e)
        {

            is_file_changed = true;

            var frmMain = (frmMain)ParentForm;
            pen = new Pen(frmMain.GetColor(), frmMain.GetBrushWidth());
            old_X = e.X;
            old_Y = e.Y;
            if (((frmMain)ParentForm).GetTool() == frmMain.tools.brush || ((frmMain)ParentForm).GetTool() == frmMain.tools.eraser)
                arr = new Point[0];
            pdDraw.BackgroundImage = bmp;
        }

        private void pdDraw_Paint(object sender, PaintEventArgs e)
        {
            if (is_changed)
            {
                //is_file_changed = true;
                switch (((frmMain)ParentForm).GetTool())
                {
                    case frmMain.tools.ellipse:
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        e.Graphics.DrawEllipse(pen, rect);
                        e.Graphics.Save();
                        
                        break;
                    case frmMain.tools.star:
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                        for (int i = 0; i < 9; i++)
                            e.Graphics.DrawLine(pen, starPoints[i], starPoints[i + 1]);
                        e.Graphics.DrawLine(pen, starPoints[9], starPoints[0]);
                        e.Graphics.Save();
                        
                        break;
                    case frmMain.tools.line:
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                        e.Graphics.DrawLine(pen, new Point(old_X, old_Y), end);
                        e.Graphics.Save();
                        
                        break;
                }
            }
            //e.Graphics.Dispose();
        }

        private void pdDraw_MouseUp(object sender, MouseEventArgs e)
        {
            if (is_changed)
            {

                switch (((frmMain)ParentForm).GetTool())
                {
                    case frmMain.tools.ellipse:
                        Graphics g = Graphics.FromImage(bmp);
                        g.DrawEllipse(pen, rect);
                        break;
                    case frmMain.tools.star:
                        g = Graphics.FromImage(bmp);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                        for (int i = 0; i < 9; i++)
                            g.DrawLine(pen, starPoints[i], starPoints[i + 1]);
                        g.DrawLine(pen, starPoints[9], starPoints[0]);
                        g.Save();
                        break;
                    case frmMain.tools.line:
                        g = Graphics.FromImage(bmp);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                        g.DrawLine(pen, new Point(old_X, old_Y), end);
                        g.Save();
                        break;
                }
                pdDraw.Invalidate();
                pdDraw.Refresh();

                rect = new Rectangle();
                starPoints = new Point[10];
                end = new Point();
            }
            is_changed = false;
        }

        private void frmChild_MouseUp(object sender, MouseEventArgs e)
        {
            //Graphics g = Graphics.FromImage(pdDraw.Image);
            //background = (Bitmap)pdDraw.Image;
        }
        public void Save(string _path)
        {
            try
            {
                bmp.Save(Path);
            }catch(Exception)
            {
                MessageBox.Show("Попробуйте сохранить файл под другим именем");
            }
        }

        public void SaveAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Сохранение изображения";
            sfd.Filter = "Bitmap (*.bmp)|*bmp| JPEG (*.jpeg)|*jpeg| Все файлы (*.*)|(*.*)";
            var diagResult = sfd.ShowDialog();
            switch (diagResult)
            {
                case DialogResult.OK:
                    Is_Saved = true;
                    bmp.Save(sfd.FileName+".bmp");
                    Path = sfd.FileName+".bmp";
                    break;
                
            }
        }


        public void OpenFile(string path)
        {
            is_file_changed = false;
            arr = new Point[0];

            var frmMain = (frmMain)ParentForm;
            //pdDraw.SizeMode = PictureBoxSizeMode.Normal;
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            Text += " "+frmMain.childCount++;

            bmp = new Bitmap(pdDraw.Width, pdDraw.Height);
            //background = new Bitmap(pdDraw.Width, pdDraw.Height);
            pdDraw.Image = bmp;
            pdDraw.BackgroundImage = bmp;
            pen = new Pen(frmMain.GetColor(), frmMain.GetBrushWidth());
            pen.DashCap = System.Drawing.Drawing2D.DashCap.Round;
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;


            bmp = (Bitmap)Image.FromFile(path);
            is_changed = false;
            scale = true;
            pdDraw.Width = bmp.Width;
            pdDraw.Height = bmp.Height;
            Width = bmp.Width;
            Height = bmp.Height;
            Is_Saved = true;
            Path = path;
            pdDraw.Image = bmp;
            pdDraw.BackgroundImage = bmp;
            pdDraw.Invalidate();
            pdDraw.Refresh();
            scale = false;
            Is_Saved = false;
        }


        #region

        public void ZoomIn()
        {
            try
            {
                scale = true;
                rect = new Rectangle();
                starPoints = new Point[10];
                end = new Point();
                old_X = 0; old_Y = 0;
                arr = new Point[0];

                var old = bmp.Clone();

                Width = bmp.Width * 2;
                Height = bmp.Height * 2;


                bmp = new Bitmap(bmp.Width * 2, bmp.Height * 2);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(old as Image, new Rectangle(0, 0, bmp.Width, bmp.Height));
                pdDraw.Image = bmp;
                pdDraw.BackgroundImage = bmp;
                scale = false;

            }
            catch (Exception)
            {

            }
        }

        public void ZoomOut()
        {
            try
            {
                scale = true;
                rect = new Rectangle();
                starPoints = new Point[10];
                end = new Point();
                old_X = 0; old_Y = 0;
                arr = new Point[0];

                var old = bmp.Clone();

                Width = bmp.Width / 2;
                Height = bmp.Height / 2;
                
                bmp = new Bitmap(bmp.Width / 2, bmp.Height / 2);
                Graphics g = Graphics.FromImage(bmp);
                g.DrawImage(old as Image, new Rectangle(0,0,bmp.Width, bmp.Height));
                pdDraw.Image = bmp;
                pdDraw.BackgroundImage = bmp;
                scale = false;
                
            }
            catch (Exception)
            {

            }

        }
        public void Function1()
        {
            is_file_changed = true;
            rect = new Rectangle();
            starPoints = new Point[10];
            end = new Point();
            old_X = 0; old_Y = 0;

            if (pdDraw.Image == null) return;
            Bitmap tempbtmp = new Bitmap(pdDraw.Image);
            int i, j;
            int DispX = 1, DispY = 1;
            int red, green, blue;
            string name = this.Text;

            for (i = 0; i < tempbtmp.Height - 2; i++)
            {
                for (j = 0; j < tempbtmp.Width - 2; j++)
                {
                    Color pixel1 = tempbtmp.GetPixel(j, i),
                          pixel2 = tempbtmp.GetPixel(j + DispX, i + DispY);

                    red = Math.Min(Math.Abs(pixel1.R - pixel2.R) + 128, 255);

                    green = Math.Min(Math.Abs(pixel1.G - pixel2.G) + 128, 255);

                    blue = Math.Min(Math.Abs(pixel1.B - pixel2.B) + 128, 255);

                    bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));
                }
                if (i % 10 == 0)
                {
                    is_changed = true;
                    pdDraw.Invalidate();
                    pdDraw.Refresh();
                    this.Text = Math.Truncate(100 * i / (pdDraw.Image.Height - 2.0)).ToString() + "%";
                }
            }
            is_changed = true;
            pdDraw.Refresh();
            this.Text = name;
            is_changed = false;
        }

        public void Function2()
        {
            is_file_changed = true;
            rect = new Rectangle();
            starPoints = new Point[10];
            end = new Point();
            old_X = 0; old_Y = 0;

            if (pdDraw.Image == null) return;
            Bitmap tempbtmp = new Bitmap(pdDraw.Image);
            int DY = 1, DX = 1;
            int i, j;
            int red, green, blue;
            string name = this.Text;

            for (i = DX; i < tempbtmp.Height - DX - 1; i++)
            {
                for (j = DY; j < tempbtmp.Width - DY - 1; j++)
                {
                    red = (int)(tempbtmp.GetPixel(j, i).R + 0.5 * tempbtmp.GetPixel(j, i).R
                        - tempbtmp.GetPixel(j - DX, i - DY).R);
                    green = (int)(tempbtmp.GetPixel(j, i).G + 0.7 * tempbtmp.GetPixel(j, i).G
                        - tempbtmp.GetPixel(j - DX, i - DY).G);
                    blue = (int)(tempbtmp.GetPixel(j, i).B + 0.5 * tempbtmp.GetPixel(j, i).B
                        - tempbtmp.GetPixel(j - DX, i - DY).B);

                    red = Math.Min(Math.Max(red, 0), 255);
                    green = Math.Min(Math.Max(green, 0), 255);
                    blue = Math.Min(Math.Max(blue, 0), 255);

                    bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));
                }
                if (i % 10 == 0)
                {
                    is_changed = true;
                    pdDraw.Invalidate();
                    pdDraw.Refresh();
                    this.Text = Math.Truncate(100 * i / (pdDraw.Image.Height - 2.0)).ToString() + "%";

                }
            }

            is_changed = false;
        }

        public void Function3()
        {
            is_file_changed = true;
            rect = new Rectangle();
            starPoints = new Point[10];
            end = new Point();
            old_X = 0; old_Y = 0;

            if (pdDraw.Image == null) return;
            Bitmap tempbtmp = new Bitmap(pdDraw.Image);
            int DY = 1, DX = 1;
            int i, j;
            int red, green, blue;
            string name = this.Text;

            for (i = DX; i < tempbtmp.Height - DX - 1; i++)
            {
                for (j = DY; j < tempbtmp.Width - DY - 1; j++)
                {
                    red = (int)(tempbtmp.GetPixel(j - 1, i - 1).R + tempbtmp.GetPixel(j - 1, i).R
                        + tempbtmp.GetPixel(j - 1, i + 1).R + tempbtmp.GetPixel(j, i - 1).R
                        + tempbtmp.GetPixel(j, i).R + tempbtmp.GetPixel(j, i + 1).R
                        + tempbtmp.GetPixel(j + 1, i - 1).R + tempbtmp.GetPixel(j + 1, i).R
                        + tempbtmp.GetPixel(j + 1, i + 1).R)/9;

                    green = (int)(tempbtmp.GetPixel(j - 1, i - 1).G + tempbtmp.GetPixel(j - 1, i).G
                        + tempbtmp.GetPixel(j - 1, i + 1).G + tempbtmp.GetPixel(j, i - 1).G
                        + tempbtmp.GetPixel(j, i).G + tempbtmp.GetPixel(j, i + 1).G
                        + tempbtmp.GetPixel(j + 1, i - 1).G + tempbtmp.GetPixel(j + 1, i).G
                        + tempbtmp.GetPixel(j + 1, i + 1).G)/9;

                    blue = (int)(tempbtmp.GetPixel(j - 1, i - 1).B + tempbtmp.GetPixel(j - 1, i).B
                        + tempbtmp.GetPixel(j - 1, i + 1).B + tempbtmp.GetPixel(j, i - 1).B
                        + tempbtmp.GetPixel(j, i).B + tempbtmp.GetPixel(j, i + 1).B
                        + tempbtmp.GetPixel(j + 1, i - 1).B + tempbtmp.GetPixel(j + 1, i).B
                        + tempbtmp.GetPixel(j + 1, i + 1).B)/9;

                    red = Math.Min(Math.Max(red, 0), 255);
                    green = Math.Min(Math.Max(green, 0), 255);
                    blue = Math.Min(Math.Max(blue, 0), 255);

                    bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));
                }
                if (i % 10 == 0)
                {
                    is_changed = true;
                    pdDraw.Invalidate();
                    pdDraw.Refresh();
                    Text = Math.Truncate(100 * i / (pdDraw.Image.Height - 2.0)).ToString() + "%";
                }
            }
            is_changed = true;
            pdDraw.Refresh();
            Text = name;
            is_changed = false;
        }

        public void Function4()
        {
            is_file_changed = true;
            Width = pdDraw.Image.Width;
            Height = pdDraw.Image.Height;

            pdDraw.Width = pdDraw.Image.Width;
            pdDraw.Height = pdDraw.Image.Height;
            //pdDraw.BackgroundImage = bmp;
            is_changed = true;
            pdDraw.Refresh();
            is_changed = false;

        }

        public void Function5()
        {
            is_file_changed = true;
            ////pdDraw.MinimumSize = new Size(pdDraw.Image.Width / 2, pdDraw.Image.Width / 2);
            //Width /= 2;
            //Height /= 2;
            int w = pdDraw.Image.Width;
            int h = pdDraw.Image.Height;

            Width /= 2;
            Height /= 2;
            pdDraw.Width = w / 2;
            pdDraw.Height = h / 2;
            //pdDraw.MinimumSize = bmp.Size;
            is_changed = true;
            pdDraw.Refresh();
            is_changed = false;
        }

        public void Function6()
        {
            is_file_changed = true;
            //pdDraw.MinimumSize = new Size(pdDraw.Image.Width * 2, pdDraw.Image.Width * 2);
            //Width *= 2;
            //Height *= 2;

            Width *= 2;
            Height *= 2;
            pdDraw.Width = pdDraw.Image.Width * 2;
            pdDraw.Height = pdDraw.Image.Height * 2;

            is_changed = true;
            pdDraw.Refresh();
            is_changed = false;
        }

        public void Function7()
        {
            is_file_changed = true;
            rect = new Rectangle();
            starPoints = new Point[10];
            end = new Point();
            old_X = 0; old_Y = 0;

            Random Rnd = new Random();
            bmp = new Bitmap(pdDraw.Image);
            pdDraw.Image = bmp;
            Bitmap tempbmp = new Bitmap(pdDraw.Image);
            int DX = 1, DY = 1;
            int red, green, blue;

            for (int i = 3; i < tempbmp.Height - 3; i++)
            {
                for (int j = 3; j < tempbmp.Width - 3; j++)
                {
                
                    DX = (int)(Rnd.NextDouble() * 4 - 2);
                    DY = (int)(Rnd.NextDouble() * 4 - 2);

                    red = tempbmp.GetPixel(j + DX, i + DY).R;
                    green = tempbmp.GetPixel(j + DX, i + DY).G;
                    blue = tempbmp.GetPixel(j + DX, i + DY).B;
                    bmp.SetPixel(j, i, Color.FromArgb(red, green, blue));
                }

                if (i % 10 == 0)
                {
                    is_changed = true;
                    pdDraw.Invalidate();
                    pdDraw.Refresh();
                    this.Text = Math.Truncate(100 * i / (pdDraw.Image.Height - 2.0)).ToString() + "%";
                }
            }
            is_changed = false;
        }

        public void Function8()
        {
            is_file_changed = true;
            pdDraw.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            Width = pdDraw.Height * pdDraw.Image.Width / pdDraw.Image.Height;
            pdDraw.Width = pdDraw.Height * pdDraw.Image.Width / pdDraw.Image.Height;

            //Width = pdDraw.Width;
            //Height = pdDraw.Height;

            //is_changed = true;
            pdDraw.Refresh();
            //is_changed = false;

        }

        public void Function9()
        {
            is_file_changed = true;
            pdDraw.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            Width = pdDraw.Height * pdDraw.Image.Width / pdDraw.Image.Height;
            pdDraw.Width = pdDraw.Height * pdDraw.Image.Width / pdDraw.Image.Height;

            //Width = pdDraw.Width;
            //Height = pdDraw.Height;

            //is_changed = true;
            pdDraw.Refresh();
            //is_changed = false;

        }

        private void frmChild_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (is_file_changed)
            {
                try
                {
                    if (Path == null)
                    {

                        DialogResult result = MessageBox.Show("Сохранить изображение в форме " + Text + " перед закрытием?", Text, MessageBoxButtons.YesNoCancel);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                SaveFileDialog sfd = new SaveFileDialog();
                                sfd.Title = "Сохранение изображения";
                                sfd.Filter = "Bitmap (*.bmp)|*bmp| JPEG (*.jpeg)|*jpeg| Все файлы (*.*)|(*.*)";
                                var diagResult = sfd.ShowDialog();
                                switch (diagResult)
                                {
                                    case DialogResult.OK: bmp.Save(sfd.FileName + ".bmp"); break;
                                    case DialogResult.Cancel: e.Cancel = true; break;
                                }
                                break;
                            case DialogResult.No:
                                break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }
                        

                    }
                    else if (!Is_Saved)
                    {
                        DialogResult result = MessageBox.Show("Сохранить изображение в форме " + Text + " перед закрытием?", Text, MessageBoxButtons.YesNoCancel);
                        switch (result)
                        {
                            case DialogResult.Yes:
                                //bmp.Save(Path.Replace(".bmp", "1.bmp"));
                                bmp.Save(Path);
                                break;
                            case DialogResult.No: break;
                            case DialogResult.Cancel:
                                e.Cancel = true;
                                break;
                        }

                    }
                    else //bmp.Save(Path.Replace(".bmp", "1.bmp"));
                        bmp.Save(Path);
                    is_file_changed = false;
                }
                catch (Exception k)
                {
                    MessageBox.Show(k.Message);
                }
            }
        }



        private void pdDraw_SizeChanged(object sender, EventArgs e)
        {
            if (!scale)
            {
                Bitmap new_bmp = new Bitmap(pdDraw.Width, pdDraw.Height);
                Graphics g = Graphics.FromImage(new_bmp);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = new_bmp;
                pdDraw.Image = bmp;
                pdDraw.BackgroundImage = bmp;
                pdDraw.Invalidate();
                pdDraw.Refresh();
            }

            //Size = new Size(pdDraw.Width + 16, pdDraw.Height + 38);
            //Bitmap new_bmp = new Bitmap(pdDraw.Width, pdDraw.Height);
            //Graphics g = Graphics.FromImage(new_bmp);
            //g.DrawImage(bmp, new Point(0,0));
            //bmp = new_bmp;
            //pdDraw.Image = bmp;

            //pdDraw.MinimumSize = pdDraw.Size;
           
            //FormBorderStyle = FormBorderStyle.FixedSingle;
            //pdDraw.Invalidate();
            //pdDraw.Refresh();
            //pdDraw.MinimumSize = bmp.Size;
            //FormBorderStyle = FormBorderStyle.Sizable;
            //pdDraw.MinimumSize = pdDraw.Size;

        }

        private void pdDraw_Click(object sender, EventArgs e)
        {

        }

        public void Function10()
        {
            is_file_changed = true;
            pdDraw.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
            is_changed = true;
            pdDraw.Refresh();
            is_changed = false;

        }

        public void Function11()
        {
            is_file_changed = true;
            pdDraw.Image.RotateFlip(RotateFlipType.RotateNoneFlipY);
            is_changed = true;
            pdDraw.Refresh();
            is_changed = false;

        }
        #endregion
    }


}

