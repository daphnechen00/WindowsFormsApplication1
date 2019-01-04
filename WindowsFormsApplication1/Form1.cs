using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //設定基本值
        static string filepath = "Finger.jpg";
        public static Bitmap bmp = new Bitmap(filepath);
        static int Wid = bmp.Width;
        static int Hei = bmp.Height;

        //邊緣位置
        public static int[,] sobel = new int[Hei, Wid];

        //處理中影像二維(較快)
        public static int[,] processpix = new int[Hei, Wid];

        private void Form1_Load(object sender, EventArgs e)
        {
            //read image
            Bitmap processbmp = new Bitmap(bmp);
            pictureBox1.Image = Image.FromFile(filepath);
            ImageProcessF.Grayscale(processbmp);
            EdgeDetection.Sobel(processpix);
            //ImageProcessF.Sharpen(processpix);
            EdgeDetection.SetEdge(processpix);
            ImageProcessF.Bilevel(processpix);
            WritePixel(processpix, processbmp);
            //load image in picturebox
            pictureBox2.Image = processbmp;

            //write image
            processbmp.Save("Output.jpg");
        }
        
        public Bitmap WritePixel(int[,] image, Bitmap img)
        {
            int height = image.GetLength(0);
            int width = image.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, image[y,x], image[y, x], image[y, x]));
                }
            }
            return img;
        }
    }
}
