using Microsoft.Win32;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using static System.Net.Mime.MediaTypeNames;
using Color = System.Drawing.Color;

namespace Projekt2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // string values;

            int tmp1 = 0, tmp2=0;
            string line;

            string Type;

            int WidthSize = 0, HeightSize = 0, ColourSize = 0;
            int i = 0, j = 0;

           // PixelFormat pf = PixelFormats.Bgr24;

            BitmapImage bitmapImage = new();
            bitmapImage.DecodePixelWidth = 3;
            bitmapImage.DecodePixelHeight=2;
           // bitmapImage.pixe


            Bitmap bitmap = new(3, 2);
            bitmap.SetResolution(3, 2);

            int rawStride;// = (width * pf.BitsPerPixel + 7) / 8;
           // byte[] rawImage = new byte[rawStride * height];

            OpenFileDialog dialog = new()
            {
                Filter = "PPM File | *.ppm"
            };

            bool? result = dialog.ShowDialog();

            if (result is not true) return;
            string fileName = dialog.FileName;

            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);

            using (StreamReader reader = new(fs, Encoding.ASCII))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (!line.Contains('#'))
                    {
                        if(line != "")
                        {
                            if (line.Contains("P3"))
                            {
                                Type = "P3";
                            }
                            else if (WidthSize == 0)
                            {
                                string[] splited = line.Replace("  ", "").Split(' ');
                                if (splited.Count()>1)
                                {
                                    WidthSize = Convert.ToInt32(splited[0]);
                                    HeightSize = Convert.ToInt32(splited[1]);
                                }
                                else
                                {
                                    WidthSize = Convert.ToInt32(splited[0]);
                                }


                                // bitmapImage.DecodePixelHeight = HeightSize;
                                //  bitmapImage.DecodePixelWidth = WidthSize;

                            }
                            else if (HeightSize == 0)
                            {
                                string[] splited = line.Replace("  ", "").Split(' ');


                                HeightSize = Convert.ToInt32(splited[0]);


                            }
                            else if (ColourSize == 0)
                            {
                                ColourSize = Convert.ToInt32(line);

                            }
                            else
                            {
                                string[] splited = line.Replace("  ", "").Split(' ');
                                tmp1 = 0;

                                if (splited.Count()>3)
                                {
                                    for (int z = 0; z < splited.Count()/3; z++)
                                    {
                                        if (z == 0)
                                        {
                                            bitmap.SetPixel(z, tmp2, Color.FromArgb(255, Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]), Convert.ToInt32(splited[2])));
                                        }
                                        else if (z==1)
                                        {
                                            bitmap.SetPixel(z, tmp2, Color.FromArgb(255, Convert.ToInt32(splited[3]), Convert.ToInt32(splited[4]), Convert.ToInt32(splited[5])));
                                        }
                                        else if (z==2)
                                        {
                                            bitmap.SetPixel(z, tmp2, Color.FromArgb(255, Convert.ToInt32(splited[6]), Convert.ToInt32(splited[7]), Convert.ToInt32(splited[8])));
                                        }
                                        // bitmap.SetPixel(z, tmp2, Color.FromArgb(255, Convert.ToInt32(splited[z]), Convert.ToInt32(splited[z]), Convert.ToInt32(splited[z])));
                                        tmp1++;
                                    }
                                }
                                else
                                {
                                    bitmap.SetPixel(tmp1, tmp2, Color.FromArgb(255, Convert.ToInt32(splited[0]), Convert.ToInt32(splited[1]), Convert.ToInt32(splited[2])));
                                }

                                tmp2++;
                            }
                        }
                        
                    }
                }
            }
            bitmapImage = ToBitmapImage(bitmap);
            MyImage.Source = bitmapImage;

        }

        public BitmapImage ToBitmapImage(Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        private void ZoomInButtonClick(object sender, MouseButtonEventArgs e)
        {
            var transform = (ScaleTransform)MyImage.RenderTransform;
            transform.ScaleX *= 5;
            transform.ScaleY *= 5;
        }

        private void ZoomOutButtonClick(object sender, MouseButtonEventArgs e)
        {
            var transform = (ScaleTransform)MyImage.RenderTransform;
            transform.ScaleX /= 1.1;
            transform.ScaleY /= 1.1;
        }
    }
}
