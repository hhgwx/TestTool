using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureMerge
{
    class Merge
    {
        public ImageID MergePic(String imagePath)
        { 
            ImageID imageID1;
            imageID1.Path = imagePath;

            //RGB点个数统计
            int[] rgbCount1 = new int[3] { 0, 0, 0 };
            //RGB点颜色值合计
            long[] rbgSum1 = new long[3] { 0, 0, 0 };
            GetImagekey(imagePath, out imageID1.rgbCount, out imageID1.rbgSum);

            return imageID1;


/*
            String pic1 = @"D:\mergeImg\1.jpg";
            String pic2 = @"D:\mergeImg\5.jpg";

            //RGB点个数统计
            int[] rgbCount1 = new int[3] { 0, 0, 0 };
            //RGB点颜色值合计
            long[] rbgSum1 = new long[3] { 0, 0, 0 };
            RGBCount(pic1, out rgbCount1, out rbgSum1);

            //RGB点个数统计
            int[] rgbCount2 = new int[3] { 0, 0, 0 };
            //RGB点颜色值合计
            long[] rbgSum2 = new long[3] { 0, 0, 0 };
            RGBCount(pic2, out rgbCount2, out rbgSum2);

            Double sameLevel1 = 0.0;
            sameLevel1 = 1-(double)(Math.Abs(rgbCount1[0] - rgbCount2[0]) + Math.Abs(rgbCount1[1] - rgbCount2[1]) + Math.Abs(rgbCount1[2] - rgbCount2[2])) / (rgbCount1[0] + rgbCount1[1] + rgbCount1[2] + rgbCount2[0] + rgbCount2[1] + rgbCount2[2]);

//            Console.WriteLine("sameLevel: " + sameLevel1);

            Double sameLevel2 = 0.0;
            sameLevel2 = 1 - (double)(Math.Abs(rbgSum1[0] - rbgSum2[0]) + Math.Abs(rbgSum1[1] - rbgSum2[1]) + Math.Abs(rbgSum1[2] - rbgSum2[2])) / (rbgSum1[0] + rbgSum1[1] + rbgSum1[2] + rbgSum2[0] + rbgSum2[1] + rbgSum2[2]);

//            Console.WriteLine("sameLevel2: " + sameLevel2);

            Console.WriteLine("sameLevel: " + (sameLevel1+sameLevel2)/2);
            */
        }

        public Bitmap Resize(String picOld)
        {
            Image img = Image.FromFile(picOld);

//            Bitmap imgOutput = new Bitmap(img, 256, 256);
            Bitmap imgOutput = new Bitmap(img, 32, 32);

//                        imgOutput.Save(picOld + "_bak.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
//                        imgOutput.Dispose();

            img.Dispose();
            return imgOutput;// (Bitmap)Image.FromFile(picNew);
        }

        public void GetImagekey(String picPath, out int[] rgbCount, out long[] rbgSum)
        {
            Bitmap bitMap = Resize(picPath);
            //RGB点个数统计
            rgbCount = new int[3] { 0, 0, 0 };
            //RGB点颜色值合计
            rbgSum = new long[3] { 0, 0, 0 };

            //单个点的RGB值
            Color pixelColor;


            for (int x = 0; x < bitMap.Width; x++)
            {
                for (int y = 0; y < bitMap.Height; y++)
                {
                    pixelColor = bitMap.GetPixel(x, y);
                    //Color newColor = Color.FromArgb(pixelColor.R, 0, 0);
                    //bitMap.SetPixel(x, y, newColor);

                    if (pixelColor.R >= pixelColor.G && pixelColor.R >= pixelColor.B)
                    {
                        rgbCount[0]++;
                        rbgSum[0] = rbgSum[0] + pixelColor.R;
                    }
                    else if (pixelColor.G >= pixelColor.R && pixelColor.G >= pixelColor.B)
                    {
                        rgbCount[1]++;
                        rbgSum[1] = rbgSum[1] + pixelColor.G;
                    }
                    else if (pixelColor.B >= pixelColor.R && pixelColor.B >= pixelColor.G)
                    {
                        rgbCount[2]++;
                        rbgSum[2] = rbgSum[2] + pixelColor.B;
                    }
                }
            }

            for (int i = 0; i < 3; i++)
            {
                rgbCount[i] = rgbCount[i] == 0 ? 1 : rgbCount[i];
            }
        }

        public Double GetSameLevel(ImageID ImageID1, ImageID ImageID2)
        {
            Double sameLevel1 = 0.0;
            sameLevel1 = 1 - (double)(Math.Abs(ImageID1.rgbCount[0] - ImageID2.rgbCount[0]) + Math.Abs(ImageID1.rgbCount[1] - ImageID2.rgbCount[1]) + Math.Abs(ImageID1.rgbCount[2] - ImageID2.rgbCount[2])) / (ImageID1.rgbCount[0] + ImageID1.rgbCount[1] + ImageID1.rgbCount[2] + ImageID2.rgbCount[0] + ImageID2.rgbCount[1] + ImageID2.rgbCount[2]);

            Double sameLevel2 = 0.0;
            sameLevel2 = 1 - (double)(Math.Abs(ImageID1.rbgSum[0] - ImageID2.rbgSum[0]) + Math.Abs(ImageID1.rbgSum[1] - ImageID2.rbgSum[1]) + Math.Abs(ImageID1.rbgSum[2] - ImageID2.rbgSum[2])) / (ImageID1.rbgSum[0] + ImageID1.rbgSum[1] + ImageID1.rbgSum[2] + ImageID2.rbgSum[0] + ImageID2.rbgSum[1] + ImageID2.rbgSum[2]);

            return (sameLevel1 + sameLevel2) / 2;

        }
    }

    public struct ImageID
    {
        public string Path;
        public int[] rgbCount;
        public long[] rbgSum;
    }
}
