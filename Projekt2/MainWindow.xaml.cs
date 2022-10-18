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
    /// 

    public class MyPixel
    {
        public int R, G, B;

    }

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Bitmap bitmap;

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // string values;

            int tmp1 = 0, tmp2=0;
            string line;

            string Type;

            int WidthSize = 0, HeightSize = 0, ColourSize = 0;
            int i = 0, j = 0;

            int R = -2, G = -2, B = -2;

           // PixelFormat pf = PixelFormats.Bgr24;

            BitmapImage bitmapImage = new();
            //bitmapImage.DecodePixelWidth = 300;
         //   bitmapImage.DecodePixelHeight=200;
           // bitmapImage.pixe


            //Bitmap bitmap = new(3, 2);
            //bitmap.SetResolution(300, 200);

            int rawStride;// = (width * pf.BitsPerPixel + 7) / 8;
           // byte[] rawImage = new byte[rawStride * height];

            OpenFileDialog dialog = new()
            {
                Filter = "PPM File | *.ppm"
            };

            bool? result = dialog.ShowDialog();

            if (result is not true) return;
            string fileName = dialog.FileName;

            int CurrentX=0,CurrentY=0;

            FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read);

            using (StreamReader reader = new(fs, Encoding.ASCII))
            {
                while (!reader.EndOfStream)
                {
                    line = reader.ReadLine();

                    if (!line.Contains('#') && !String.IsNullOrWhiteSpace(line))
                    {
                        if (line.Contains("P3"))
                        {
                            Type = "P3";
                        }
                        else
                        if (WidthSize == 0)
                        {
                            if (line.Contains(' '))
                            {
                                string[] splited = line.Split(' ');
                                WidthSize = Convert.ToInt32(splited[0]);
                                HeightSize = Convert.ToInt32(splited[1]);
                                bitmap = new(WidthSize, HeightSize);
                            }
                            else
                            {
                                WidthSize = Convert.ToInt32(line);
                            }
                        }
                        else
                        if (HeightSize == 0)
                        {
                            HeightSize = Convert.ToInt32(line);
                            bitmap = new(WidthSize, HeightSize);
                        }
                        else
                        if (ColourSize == 0)
                        {
                            ColourSize = Convert.ToInt32(line);
                        }
                        else
                        {
                            string[] splited = line.Split(' ');

                            splited = splited.Where(val => val != "").ToArray();
                            if (splited.Contains("\t"))
                            {
                                splited = line.Split("\t");
                                splited = splited.Where(val => val != "").ToArray();
                            }



                            if (splited.Count() == 1)
                            {
                                if (R < 0)
                                {
                                    R = Convert.ToInt32(splited[0]);
                                }
                                else
                                if (R >= 0 && G < 0)
                                {
                                    G = Convert.ToInt32(splited[0]);
                                }
                                else
                                if (R >= 0 && G >= 0 && B < 0)
                                {
                                    B = Convert.ToInt32(splited[0]);
                                }

                                if (R >= 0 && G >= 0 && B >= 0)
                                {
                                    if(ColourSize == 255)
                                    {
                                        bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R, G, B));
                                    }
                                    else if(ColourSize == 65535)
                                    {
                                        bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R>>8, G>>8, B>>8));
                                    }



                                    CurrentX++;

                                    if (CurrentX >= WidthSize)
                                    {
                                        CurrentX = 0;
                                        CurrentY++;
                                    }

                                    R = G = B = -2;
                                }
                            }
                            else
                            {
                                if (splited.Count() == 3)
                                {
                                    if (R < 0)
                                    {
                                        R = Convert.ToInt32(splited[0]);
                                    }

                                    if (G < 0)
                                    {
                                        G = Convert.ToInt32(splited[1]);
                                    }

                                    if (B < 0)
                                    {
                                        B = Convert.ToInt32(splited[2]);
                                    }

                                    if (R >= 0 && G >= 0 && B >= 0)
                                    {
                                        if (ColourSize == 255)
                                        {
                                            bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R, G, B));
                                        }
                                        else if (ColourSize == 65535)
                                        {
                                            bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R>>8, G>>8, B>>8));
                                        }

                                        CurrentX++;

                                        if (CurrentX >= WidthSize)
                                        {
                                            CurrentX = 0;
                                            CurrentY++;
                                        }

                                        R = G = B = -2;
                                    }
                                }
                                else if (splited.Count() > 3)
                                {
                                    if (splited.Count() % 3 ==0)
                                    {
                                        int pom1 = 0;
                                        for (int am1 = 0; am1 < splited.Count(); am1++)
                                        {
                                            if (pom1==0)
                                            {
                                                R = Convert.ToInt32(splited[am1]);
                                            }
                                            else if (pom1==1)
                                            {
                                                G = Convert.ToInt32(splited[am1]);
                                            }
                                            else if (pom1==2)
                                            {
                                                B = Convert.ToInt32(splited[am1]);
                                            }
                                            pom1++;
                                            if (pom1 == 3)
                                            {
                                                pom1 =0;
                                            }

                                            if (R >= 0 && G >= 0 && B >= 0)
                                            {
                                                if (ColourSize == 255)
                                                {
                                                    bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R, G, B));
                                                }
                                                else if (ColourSize == 65535)
                                                {
                                                    bitmap.SetPixel(CurrentX, CurrentY, Color.FromArgb(R>>8, G>>8, B>>8));
                                                }

                                                CurrentX++;

                                                if (CurrentX >= WidthSize)
                                                {
                                                    CurrentX = 0;
                                                    CurrentY++;
                                                }

                                                R = G = B = -2;

                                            }
                                        }

                                    }
                                }


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
            transform.ScaleX *= 1.5;
            transform.ScaleY *= 1.5;
        }

        private void ZoomOutButtonClick(object sender, MouseButtonEventArgs e)
        {
            var transform = (ScaleTransform)MyImage.RenderTransform;
            transform.ScaleX /= 1.1;
            transform.ScaleY /= 1.1;
        }
    }
}
