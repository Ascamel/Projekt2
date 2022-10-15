using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            string values = "";
            string line = "";

            string Type;

            int SizeX, SizeY, ColourSize;

            PixelFormat pf = PixelFormats.Bgr24;

            int width = 200;
            int height = 200;
            int rawStride = (width * pf.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * height];

            BitmapSource MyBitMap;//= BitmapSource.Create()

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

                 values = reader.ReadToEnd();
                
                
            }
           // Trace.WriteLine(values);


            foreach(char c in values)
            {
                if (c == 'P')
                {
                    if (values[values.IndexOf(c)+1].Equals('3'))
                    {
                        Type = "P3";
                    }
                    else
                    {
                        Type = "P6";
                    }
                }
            }

            MyBitMap = BitmapSource.Create(6, 6, 1, 1, pf, null, rawImage, rawStride);
            MyImage.Source = MyBitMap;
        }

        public void AddImage()
        {
           // MyImage.Sou
        }

        public string ImageParser(string ToParse)
        {
            string result = "sad";
            foreach(char c in ToParse)
            {
                Trace.WriteLine(c);
            }

            return result;
        }
    }
}
