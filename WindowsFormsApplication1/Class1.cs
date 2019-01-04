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
    class ImageProcessF:Form1
    {
        public static void Grayscale(Bitmap img)
        {
            //灰階化
            Color pixel;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    pixel = img.GetPixel(x, y);
                    int R = pixel.R;
                    int G = pixel.G;
                    int B = pixel.B;
                    int A = pixel.A;

                    int Gray = (R + G + B) / 3;
                    processpix[y, x] = Gray;
                    //img.SetPixel(x, y, Color.FromArgb(A, Gray, Gray, Gray));
                }
            }
        }

        public static void Sharpen(int[,] img)
        {
            //銳化
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
            int laplace = 0;
            int count = 0;
            int height = img.GetLength(0);
            int width = img.GetLength(1);

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
                            laplace += Laplacian[count] * img[a, b];
                            count++;
                        }
                    }
                    if (laplace > 255) laplace = 255;
                    if (laplace < 0) laplace = 0;
                    //laplacian[y, x] = laplace;
                    img[y, x] = laplace;
                    //img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, laplace, laplace, laplace));
                }
            }
        }

        public static void Bilevel(int[,] img)
        {
            //二值化
            int T = FindT.Threshold(img);
            int pix;
            int height = img.GetLength(0);
            int width = img.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (img[y, x] < T) pix = 0;
                    else pix = 255;
                    processpix[y, x] = pix;
                    //img.SetPixel(x, y, Color.FromArgb(img.GetPixel(x, y).A, pixel, pixel, pixel));
                }
            }
        }
    }
}
