using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureMerge
{
    class Merge2
    {
        public int colorType = 4; //RGB + 灰色
        public int colorArr = 52; // 255/5 + 1

        public ImageID_KIND2 MergePic(String imagePath)
        {
            ImageID_KIND2 imageID1;
            imageID1.Path = imagePath;

            GetImagekey(imagePath, out imageID1.rgbCount);

            return imageID1;
        }

        public Bitmap Resize(String picOld)
        {
            Image img = Image.FromFile(picOld);

//            Bitmap imgOutput = new Bitmap(img, 256, 256);
            Bitmap imgOutput = new Bitmap(img, 32, 32);

            img.Dispose();
            return imgOutput;// (Bitmap)Image.FromFile(picNew);
        }

        public void GetImagekey(String picPath, out int[,] rgbCount)
        {
            Bitmap bitMap = Resize(picPath);
            //RGB点个数统计
            rgbCount = new int[colorType, colorArr];
            for (int i =0;i< colorType; i++)
            {
                for (int j=0;j< colorArr; j++)
                {
                    rgbCount[i,j] = 0;
                }
            }

            //单个点的RGB值
            Color pixelColor;

            for (int x = 0; x < bitMap.Width; x++)
            {
                for (int y = 0; y < bitMap.Height; y++)
                {
                    pixelColor = bitMap.GetPixel(x, y);
                    if (pixelColor.R > pixelColor.G && pixelColor.R > pixelColor.B)
                    {
                        rgbCount[0, pixelColor.R/5]++;
                    }
                    else if (pixelColor.G > pixelColor.R && pixelColor.G > pixelColor.B)
                    {
                        rgbCount[1, pixelColor.G / 5]++;
                    }
                    else if (pixelColor.B > pixelColor.R && pixelColor.B > pixelColor.G)
                    {
                        rgbCount[2, pixelColor.B / 5]++;
                    } else
                    {
                        rgbCount[3, pixelColor.B / 5]++;
                    }
                }
            }
        }

        public Double GetSameLevel(ImageID_KIND2 ImageID1, ImageID_KIND2 ImageID2)
        {
            Double sameLevel = 0.0;
            int diffPoint = 0;
            int allPoint = 0;

            for (int i = 0; i < colorType; i++)
            {
                for (int j = 0; j < colorArr; j++)
                {
                    diffPoint = diffPoint + Math.Abs(ImageID1.rgbCount[i, j] - ImageID2.rgbCount[i, j]);
                    allPoint = allPoint + ImageID1.rgbCount[i, j];
                }
            }

            sameLevel = 1 - (double)diffPoint /(2*allPoint);

//            Console.WriteLine("path1:" + ImageID1.Path + "/path2" + ImageID2.Path);
//            Console.WriteLine("diffPoint:"+ diffPoint + "/allPoint" +2* allPoint);
//            Console.WriteLine("sameLevel:" + sameLevel);

            return sameLevel;

        }
    }

    public struct ImageID_KIND2
    {
        public string Path;
        public int[,] rgbCount;
    }
}
