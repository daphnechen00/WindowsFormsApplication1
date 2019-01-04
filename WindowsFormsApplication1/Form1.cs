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
        static string filepath = "C:\\Users\\user\\Pictures\\Finger.jpg";
        static Bitmap bmp = new Bitmap(filepath);
        static int width = bmp.Width;
        static int height = bmp.Height;
        //邊緣位置
        static int[,] sobel = new int[height, width];
        //處理中影像二維
        static int[,] processpix = new int[height, width];

        private void Form1_Load(object sender, EventArgs e)
        {
            //read image
            Bitmap processbmp = new Bitmap(bmp);
            pictureBox1.Image = Image.FromFile(filepath);
            Grayscale(processbmp);
            Sobel(processbmp);
            //Sharpen(bmp);
            SetEdge(processbmp);
            Bilevel(processbmp);
            
            //load image in picturebox
            pictureBox2.Image = processbmp;
            ;

            //write image
            bmp.Save("C:\\Users\\user\\Documents\\Visual Studio 2015\\Projects\\WindowsFormsApplication1\\WindowsFormsApplication1\\Output.jpg");
        }

        public void Grayscale(Bitmap img)
        {
            //灰階化
            Color pixel;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    pixel = img.GetPixel(x, y);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    int A = pixel.A;

                    int Gray = (R + G + B) / 3;
                    img.SetPixel(x, y, Color.FromArgb(A, Gray, Gray, Gray));
                }
            }
            
        }

        public void Sobel(Bitmap img)
        {
            //邊緣偵測
            int[] maskx = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] masky = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };
            
            int sobelx = 0;
            int sobely = 0;
            int count = 0;
            int result = 0;
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    count = 0;
                    sobelx = 0;
                    sobely = 0;
                    for (int a = x - 1; a <= x + 1; a++)
                    {
                        for (int b = y - 1; b <= y + 1; b++)
                        {
                            sobelx += maskx[count] * img.GetPixel(a, b).R;
                            sobely += masky[count] * img.GetPixel(a, b).R;
                            count++;
                        }
                    }
                    result = (int)System.Math.Sqrt(sobelx * sobelx + sobely * sobely);
                    //result = System.Math.Abs(sobelx) + System.Math.Abs(sobely);
                    if (result > 255) result = 255;
                    sobel[y, x] = result;
                }
            }
            WritePixel(sobel,img);
        }

        public void Sharpen(Bitmap img)
        {
            //銳化
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            int laplace = 0;
            int count = 0;
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    count = 0;
                    laplace = 0;
                    for (int a = y - 1; a <= y + 1; a++)
                    {
                        for (int b = x - 1; b <= x + 1; b++)
                        {
                            laplace += Laplacian[count] * img.GetPixel(x, y).R;
                            count++;
                        }
                    }
                    if (laplace > 255) laplace = 255;
                    if (laplace < 0) laplace = 0;
                    //laplacian[y, x] = laplace;
                    img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, laplace, laplace, laplace));
                }
            }
        }

        public int Threshold(Bitmap img)
        {
            //找閾值
            int maxColor = 0;
            int minColor = 255;
            int[] histogram = new int[256];
            for(int y = 0; y < height; y++)
            {
                for(int x = 0; x < width; x++)
                {
                    histogram[img.GetPixel(x, y).R]++;
                    if (img.GetPixel(x, y).R > maxColor) maxColor = img.GetPixel(x, y).R;
                    if (img.GetPixel(x, y).R < minColor) minColor = img.GetPixel(x, y).R;
                }
            }
            int T= (maxColor - minColor) / 2;

            int PixelC1=0, PixelS1=0;
            int PixelC2=0, PixelS2=0;
            int PixelA1, PixelA2;
            for (int i = 0; i <= T; i++)
            {
                PixelS1 += histogram[i] * i;
                PixelC1 += histogram[i];
            }
            PixelA1 = PixelS1 / PixelC1;
            for (int i = T+1; i < 256; i++)
            {
                PixelS2 += histogram[i] * i;
                PixelC2 += histogram[i];
            }
            PixelA2 = PixelS2 / PixelC2;
            return (PixelA2 + PixelA1) / 2;
        }

        public void Bilevel(Bitmap img)
        {
            //二值化
            int T = Threshold(img);
            int pixel;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (img.GetPixel(x, y).R < T) pixel = 0;
                    else pixel = 255;
                    img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, pixel, pixel, pixel));
                }
            }
        }

        public void SetEdge(Bitmap img)
        {
            int sobelT = Threshold(img);
            int pixel;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (isEdge(x, y, sobelT,img))
                    {
                        pixel = 0;
                        img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, pixel, pixel, pixel));
                    }
                }
            }
        }
        public Boolean isEdge(int x , int y, int T, Bitmap img)
        {
            //邊緣強化
            if (sobel[y,x]> T) return true;
            else return false;
        }

        public Bitmap WritePixel(int[,] image, Bitmap img)
        {
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
