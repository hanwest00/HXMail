using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace HXMail.App_Code
{
    //截取屏幕
    public class ClipScreen
    {
        private static Point startPoint, endPoint, point; //起始的选择点,结束的选择点,截屏矩形的左上点
        private static Graphics g, tmpG;
        private static Bitmap screenImage, tmpScreenImage;
        private static Form CatchScreen;
        private static Color color = Color.FromArgb(100, Color.Black); //遮罩透明度(0 - 255),颜色
        private static Color innerColor = Color.FromArgb(120, Color.Pink); //选区透明度(0 - 255),颜色
        public delegate void OnClipComplete(Image retImage);
        /// <summary>
        /// 截屏完成时发生事件
        /// </summary>
        public static event OnClipComplete ClipCompleteEvent; 
        public static int screenWidth = Screen.AllScreens[0].Bounds.Width;
        public static int screenHeight = Screen.AllScreens[0].Bounds.Height;

        public static int clipWidth //选区宽度
        {
            get
            {
                if (startPoint.IsEmpty || endPoint.IsEmpty)
                    return 0;
                return System.Math.Abs(startPoint.X - endPoint.X);
            }
        }

        public static int clipHeight //选区高度
        {
            get
            {
                if (startPoint.IsEmpty || endPoint.IsEmpty)
                    return 0;
                return System.Math.Abs(startPoint.Y - endPoint.Y);
            }

        }

        /// <summary>
        /// 截取屏幕
        /// </summary>
        public static void DoClipScreen()
        {
            CatchScreen = new Form();
            CatchScreen.FormBorderStyle = FormBorderStyle.None;
            CatchScreen.BackColor = System.Drawing.Color.White;
            CatchScreen.TransparencyKey = System.Drawing.Color.White;
            CatchScreen.Width = Screen.AllScreens[0].Bounds.Width;
            CatchScreen.Height = Screen.AllScreens[0].Bounds.Height;
            CatchScreen.StartPosition = FormStartPosition.CenterScreen;
            screenImage = new Bitmap(screenWidth, screenHeight);
            g = Graphics.FromImage(screenImage);
            g.CopyFromScreen(new System.Drawing.Point(0, 0), new System.Drawing.Point(0, 0), new System.Drawing.Size(screenWidth, screenHeight));
            CatchScreen.BackgroundImage = screenImage;

            CatchScreen.MouseClick += new MouseEventHandler(delegate(object obj, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                        startPoint = new Point(0, 0);
                        endPoint = new Point(0, 0);
                        point = new Point(0, 0);
                        CatchScreen.BackgroundImage = screenImage;
                        return;
                }
                if (!startPoint.IsEmpty && !endPoint.IsEmpty)
                    return;
                if (startPoint.IsEmpty)
                {
                    point = startPoint = e.Location;
                    tmpScreenImage = screenImage.Clone() as Bitmap;
                    tmpG = Graphics.FromImage(tmpScreenImage);
                    tmpG.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    tmpG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    tmpG.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, color, color), 0, 0, screenWidth, screenHeight);
                    tmpG.FillEllipse(System.Drawing.Brushes.HotPink, startPoint.X - 2, startPoint.Y - 2, 5, 5);
                    tmpG.DrawString(string.Format("{0},{1}", startPoint.X, startPoint.Y), SystemFonts.DefaultFont, System.Drawing.Brushes.HotPink, startPoint.X - 50, startPoint.Y - 20);
                    CatchScreen.BackgroundImage = tmpScreenImage.Clone() as Image;
                    return;
                }
                endPoint = e.Location;

                //判断选区左上的点坐标
                if ((endPoint.X > startPoint.X && endPoint.Y < startPoint.Y) || (endPoint.X < startPoint.X && endPoint.Y > startPoint.Y))
                    point = new Point(startPoint.X < endPoint.X ? startPoint.X : endPoint.X, startPoint.Y < endPoint.Y ? startPoint.Y : endPoint.Y);
                else if ((startPoint.X == endPoint.X) && (startPoint.Y == endPoint.Y))
                {
                    startPoint = new Point(0, 0);
                    endPoint = new Point(0, 0);
                    point = new Point(0, 0);
                    CatchScreen.BackgroundImage = screenImage;
                    return;
                }
                else
                    point = startPoint.X < endPoint.X ? startPoint : endPoint;

                tmpG.FillEllipse(System.Drawing.Brushes.HotPink, endPoint.X - 2, endPoint.Y - 2, 5, 5);
                tmpG.DrawString(string.Format("{0},{1}", endPoint.X, endPoint.Y), SystemFonts.DefaultFont, System.Drawing.Brushes.HotPink, endPoint.X + 50, endPoint.Y + 20);

                //选区的border
                tmpG.DrawRectangle(System.Drawing.Pens.Pink, point.X, point.Y, clipWidth, clipHeight);

                tmpG.FillRectangle(new System.Drawing.Drawing2D.HatchBrush(System.Drawing.Drawing2D.HatchStyle.DiagonalCross, innerColor, innerColor), point.X, point.Y, clipWidth, clipHeight);

                CatchScreen.BackgroundImage = tmpScreenImage;
            });

            CatchScreen.MouseDoubleClick += new MouseEventHandler(delegate(object obj, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    startPoint = new Point(0, 0);
                    endPoint = new Point(0, 0);
                    point = new Point(0, 0);
                    CatchScreen.Close();
                    return;
                }
                if (startPoint.IsEmpty || endPoint.IsEmpty)
                    return;
                tmpScreenImage = new Bitmap(clipWidth, clipHeight);
                tmpG = Graphics.FromImage(tmpScreenImage);
                tmpG.DrawImage(screenImage, new System.Drawing.Rectangle(0, 0, clipWidth, clipHeight), new System.Drawing.Rectangle(point.X, point.Y, clipWidth, clipHeight), System.Drawing.GraphicsUnit.Pixel);
                ClipCompleteEvent(tmpScreenImage);
                CatchScreen.Close();
            });

            CatchScreen.KeyPress += new KeyPressEventHandler(delegate(object obj,KeyPressEventArgs e)
            {
                if (e.KeyChar == 27)
                {
                    startPoint = new Point(0, 0);
                    endPoint = new Point(0, 0);
                    point = new Point(0, 0);
                    CatchScreen.Close();
                }
            });

            CatchScreen.FormClosing += new FormClosingEventHandler(delegate(object obj, FormClosingEventArgs e)
            {
                if (tmpScreenImage != null)
                    tmpScreenImage.Dispose();
                if (tmpG != null)
                    tmpG.Dispose();
                if (screenImage != null)
                    screenImage.Dispose();
                if (g != null)
                    g.Dispose();
            });

            CatchScreen.Show();
            CatchScreen.Activate();
        }
    }
}
