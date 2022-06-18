using System.IO;
using System.Net.Mime;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Chat_app_client.Connect
{
    public static class Image_to_byte
    {
        public static byte[] Img_to_byte_convert(BitmapImage img ){
            ImageSourceConverter _imageConverter = new ImageSourceConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(img, typeof(byte[]));
            return xByte;
        }
    }
}

