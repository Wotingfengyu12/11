using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;

namespace CQC.ConTest
{
    public partial class Loading : WaitForm
    {
        public Loading()
        {
            InitializeComponent();
            this.progressPanel1.AutoHeight = true;
        }

        #region Overrides

        public override void SetCaption(string caption)
        {
            base.SetCaption(caption);
            this.progressPanel1.Caption = caption;
        }
        public override void SetDescription(string description)
        {
            base.SetDescription(description);
            this.progressPanel1.Description = description;
        }
        public override void ProcessCommand(Enum cmd, object arg)
        {
            base.ProcessCommand(cmd, arg);
        }

        #endregion

        public enum WaitFormCommand
        {
        }
    }

    class CustomOverlayPainter : OverlayWindowPainterBase
    {
        // Defines the string’s font.
        public readonly Font drawFont;
        string text;

        public CustomOverlayPainter()
        {
            drawFont = new Font("Tahoma", 10);
        }

        public CustomOverlayPainter(string str)
        {
            drawFont = new Font("Tahoma", 10);
            text = str;
        }

        protected override void Draw(OverlayWindowCustomDrawContext context)
        {
            //The Handled event parameter should be set to true. 
            //to disable the default drawing algorithm. 
            context.Handled = true;
            //Provides access to the drawing surface. 
            GraphicsCache cache = context.DrawArgs.Cache;
            //Adjust the TextRenderingHint option
            //to improve the image quality.
            cache.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //Overlapped control bounds. 
            Rectangle bounds = context.DrawArgs.Bounds;
            //Draws the default background. 
            context.DrawBackground();
            //Specify the string that will be drawn on the Overlay Form instead of the wait indicator.
            String drawString = text;
            //Get the system's black brush.
            Brush drawBrush = Brushes.Black;
            //Calculate the size of the message string.
            SizeF textSize = cache.CalcTextSize(drawString, drawFont);
            //A point that specifies the upper-left corner of the rectangle where the string will be drawn.
            PointF drawPoint = new PointF(
                bounds.Left + bounds.Width / 2 - textSize.Width / 2,
                bounds.Top + bounds.Height / 2 - textSize.Height / 2
                );
            //Draw the string on the screen.
            cache.Graphics.DrawString(drawString, drawFont, drawBrush, drawPoint);
        }
    }
    //...
}