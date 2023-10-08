using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using System.ComponentModel;
using System.Windows;
using System.Drawing;
using System.Drawing.Imaging;

namespace TextToImageConvertion
{
    public class TextToImage : CodeActivity
    {
        [Category("Input")]
        [RequiredArgument]
        public InArgument<string> ImageText { get; set; }
        [Category("Input")]
        [DefaultValue(12)]
        public InArgument<int> FontSize { get; set; }
        [Category("Input")]
        public InArgument<string> FontStyle { get; set; }
        [Category("Input")]
        public InArgument<Color> BackgroungColor { get; set; }
        [Category("Input")]
        public InArgument<Color> FontColor { get; set; }
        [Category("Input")]
        public InArgument<string> ImageFilePath { get; set; }
        [Category("OutPut")]
        public InArgument<Exception> ExceptionMessage { get; set; }
        protected override void Execute(CodeActivityContext context)
        {
            context.SetValue(ExceptionMessage, new Exception());
            try
            {
                string imageText = ImageText.Get(context);
                int fontSize = FontSize.Get(context);
                string fontStyle = FontStyle.Get(context);
                Color bgColor = BackgroungColor.Get(context);
                Color ftColor = FontColor.Get(context);
                string filePath= ImageFilePath.Get(context);
                if(filePath == null)
                    filePath=Environment.CurrentDirectory + "/" + imageText.Substring(0,1)+".png";
                if(bgColor == null) 
                    bgColor=Color.White;
                if(ftColor == null)
                    ftColor=Color.Black;
                if(fontStyle == null)
                    fontStyle=FontStyles.Normal.ToString();
                if (fontSize == 0)
                    fontSize=12;

                Bitmap bitmap = ConvertTextToImage(imageText, fontStyle, fontSize, bgColor, ftColor, fontSize * imageText.Length, fontSize * 2); ;
                bitmap.Save(filePath, ImageFormat.Png);

            }
            catch(Exception e)
            {
                context.SetValue(ExceptionMessage, e);
            }   
        }
        public Bitmap ConvertTextToImage(string txt, string fontname, int fontsize, Color bgcolor, Color fcolor, int width, int Height)
        {
            Bitmap bmp = new Bitmap(width, Height);
            using (Graphics graphics = Graphics.FromImage(bmp))
            {

                Font font = new Font(fontname, fontsize);
                graphics.FillRectangle(new SolidBrush(bgcolor), 0, 0, bmp.Width, bmp.Height);
                graphics.DrawString(txt, font, new SolidBrush(fcolor), 0, 0);
                graphics.Flush();
                font.Dispose();
                graphics.Dispose();

            }
            return bmp;
        }
    }
}
