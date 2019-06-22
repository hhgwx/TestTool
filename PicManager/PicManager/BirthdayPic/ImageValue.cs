using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PicManager.BirthdayPic
{
    class ImageValue
    {
        public string imagePath
        {
            get; set;
        }

        public string imageName
        {
            get; set;
        }

        public BitmapFrame bitmapFrame
        {
            get; set;
        }
		public BitmapFrame bitmapDeleteFrame
		{
			get; set;
		}
	}
}
