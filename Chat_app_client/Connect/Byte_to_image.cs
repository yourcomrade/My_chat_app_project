using System;
using System.IO;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

//The code I consult from internet
//Link: https://social.msdn.microsoft.com/Forums/silverlight/en-US/a11de4f1-010f-4b36-9845-6f60783e61a6/bind-image-source-with-image-byte-array?forum=silverlightcontrols
//Link: https://stackoverflow.com/questions/686461/how-do-i-bind-a-byte-array-to-an-image-in-wpf-with-a-value-converter
namespace Chat_app_client.Connect
{
    public class Byte_to_image:IValueConverter
    {
        private static BitmapImage ConvertBytetoImage(byte[] byteArray)
        {
            BitmapImage img = new BitmapImage();
            using (var ms = new MemoryStream(byteArray))
            {
                img.BeginInit();
                img.CacheOption = BitmapCacheOption.OnLoad;
                img.StreamSource = ms;
                img.EndInit();
                img.Freeze();
            }
            return img;

        }

        public  object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var imageByteArray = value as byte[];
            if (imageByteArray == null) return null;
            return ConvertBytetoImage(imageByteArray);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}

