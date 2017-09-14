using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Asdrubals {
    class Food {

        public double X { get; set; }
        public double Y { get; set; }

        public bool Consumed { get; set;} =  false;

        private double Width = 25;
        private double Height = 25;

        private Image img;

        public Food() {
            this.img = getImg();
            this.X = Constants.RandomSeed.Next(0, (int)Constants.BoundX - (int)Width);
            this.Y = Constants.RandomSeed.Next(0, (int)Constants.BoundY - (int)Height);

        }

        public Rectangle Draw() {
            return new Rectangle() {
                Fill = new ImageBrush(img.Source),
                Width = this.Width, Height = this.Height,
                Margin = new System.Windows.Thickness(X, Y, 0, 0)
            };
            //return r;
        }

        private Image getImg() {
            Image finalImage = new Image();
            finalImage.Width = Width;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit(); //pack://application:,,,/AssemblyName;component/Resources/logo.png 
            logo.UriSource = new Uri(@"C:\Users\Felipe\Source\Repos\asdrubals\Asdrubals\Asdrubals\Resources\food.png");
            logo.EndInit();
            finalImage.Source = logo;
            return finalImage;
        }
        public Rect getRect() {
            return new Rect(this.X, this.Y, this.Width, this.Height);
        }
    }
}
