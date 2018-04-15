using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Map.Framework;

namespace WpfApp_ImageSource
{
    public class ImageFactory
    {
        private readonly int _width;
        private readonly int _height;

        public ImageFactory(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public BitmapSource Create(IRoute routes, ICar car)
        {
            WriteableBitmap wbitmap = new WriteableBitmap(
                _width, _height, 96, 96, PixelFormats.Bgra32, null);
            byte[,,] pixels = new byte[_height, _width, 4];

            // Clear to black.

            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        pixels[row, col, i] = 0;
                    }

                    pixels[row, col, 3] = 255;
                }
            }

            //for (int i = 0; i < 10; i++)
            //{
            //    for (int j = 0; j < 10; j++)
            //    {
            //        pixels[i, 0, 2] = byte.MaxValue;
            //    }
            //}

            // Show car position
            //pixels[(int) car.StartLat, (int) car.StartLon, 0] = byte.MaxValue;
            pixels[(int)car.StartLon, (int)car.StartLat, 0] = byte.MaxValue;

            // Blue 0, Green 1, Red 2
            foreach (var doc in routes.Docs)
            {
                //pixels[(int) doc.Lat, (int) doc.Lon, 2] = byte.MaxValue;
                pixels[(int)doc.Lon, (int)doc.Lat, 2] = byte.MaxValue;
            }

            // Copy the data into a one-dimensional array.
            byte[] pixels1D = new byte[_width * _height * 4];
            int index = 0;


            for (int row = 0; row < _height; row++)
            {
                for (int col = 0; col < _width; col++)
                {
                    for (int i = 0; i < 4; i++)
                        pixels1D[index++] = pixels[row, col, i];
                }
            }

            // Update writeable bitmap with the colorArray to the image.
            Int32Rect rect = new Int32Rect(0, 0, _width, _height);
            int stride = 4 * _width;
            wbitmap.WritePixels(rect, pixels1D, stride, 0);

            return wbitmap;
        }
    }
}
