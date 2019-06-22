using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PicManager
{
    static class Const
    {
        //取得缩略图 直接赋值给Image的Source属性
        public static BitmapFrame getThumbImage(string path, int width, int heigth)
        {
			try
			{
				Image originalImage = Image.FromFile(path);

				using (System.Drawing.Image thumbImage = originalImage.GetThumbnailImage(width, heigth, () => { return true; }, IntPtr.Zero))
				{
					MemoryStream ms = new MemoryStream();
					thumbImage.Save(ms, ImageFormat.Png);

					BitmapFrame bitmapFrame = BitmapFrame.Create(ms);

					originalImage.Dispose();
					return bitmapFrame;
				}
			}
			catch (Exception)
			{
				Image fromImage = Image.FromFile("D:\\imageEdit\\流光字.jpg");
//				MemoryStream stream = new MemoryStream();
//				fromImage.Save(stream, ImageFormat.Png);
//				string base64 = Convert.ToBase64String(stream.GetBuffer());

				//base64 图片
				string base64 = "iVBORw0KGgoAAAANSUhEUgAAAF0AAABcCAIAAACDTy4xAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAC5SURBVHhe7dCBAAAADASh+Uv/BEK4ELpFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX68V6sV6sF+vFerFerBfrxXqxXqwX2R6GQeWKWfcTQwAAAABJRU5ErkJggg==";
				byte[] bytesImg = Convert.FromBase64String(base64);
				MemoryStream memStreamImg = new MemoryStream(bytesImg);
				return BitmapFrame.Create(memStreamImg);
			}
        }

		public static BitmapFrame getThumbDeleteImage(string path, int width, int heigth)
		{
			//base64 图片
			string base64 = "iVBORw0KGgoAAAANSUhEUgAAABQAAAAUCAIAAAAC64paAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAATSURBVDhPYxgFo2AUjIIhCxgYAATEAAHN+RzrAAAAAElFTkSuQmCCAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA==";
			byte[] bytesImg = Convert.FromBase64String(base64);
			MemoryStream memStreamImg = new MemoryStream(bytesImg);
			return BitmapFrame.Create(memStreamImg);
		}
	}
}
