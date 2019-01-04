using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class EdgeDetection: Form1
    {
        public static void Sobel(int[,] img)
        {
            //邊緣偵測
            int[] maskx = { -1, 0, 1, -2, 0, 2, -1, 0, 1 };
            int[] masky = { 1, 2, 1, 0, 0, 0, -1, -2, -1 };

            int sobelx = 0, sobely = 0;
            int count = 0, result = 0;
            int height = img.GetLength(0);
            int width = img.GetLength(1);
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
                            sobelx += maskx[count] * img[b, a];
                            sobely += masky[count] * img[b, a];
                            count++;
                        }
                    }
                    result = (int)System.Math.Sqrt(sobelx * sobelx + sobely * sobely);
                    //result = System.Math.Abs(sobelx) + System.Math.Abs(sobely);
                    if (result > 255) result = 255;
                    sobel[y, x] = result;
                }
            }
            //WritePixel(sobel,img);
        }

        public static void SetEdge(int[,] img)
        {
            int sobelT = FindT.Threshold(img);
            int pix;
            int height = img.GetLength(0);
            int width = img.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (isEdge(x, y, sobelT - 80))//讓邊緣更明顯
                    {
                        pix = 0;
                        processpix[y, x] = pix;
                    }
                }
            }
        }
        public static Boolean isEdge(int x, int y, int T)
        {
            //邊緣強化
            if (sobel[y, x] > T) return true;
            else return false;
        }
    }
}
