using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class FindT
    {
        public static int Threshold(int[,] img)
        {
            //找閾值
            int maxColor = 0;
            int minColor = 255;
            int[] histogram = new int[256];
            int height = img.GetLength(0);
            int width = img.GetLength(1);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    histogram[img[y, x]]++;
                    if (img[y, x] > maxColor) maxColor = img[y, x];
                    if (img[y, x] < minColor) minColor = img[y, x];
                }
            }
            int T = (maxColor - minColor) / 2;

            int PixelC1 = 0, PixelS1 = 0;
            int PixelC2 = 0, PixelS2 = 0;
            int PixelA1, PixelA2;
            for (int i = 0; i <= T; i++)
            {
                PixelS1 += histogram[i] * i;
                PixelC1 += histogram[i];
            }
            PixelA1 = PixelS1 / PixelC1;
            for (int i = T + 1; i < 256; i++)
            {
                PixelS2 += histogram[i] * i;
                PixelC2 += histogram[i];
            }
            PixelA2 = PixelS2 / PixelC2;
            return (PixelA2 + PixelA1) / 2;
        }
    }
}
