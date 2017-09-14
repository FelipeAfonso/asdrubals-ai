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
    class Asdrubal {
        private double x = 200;
        public double X {
            get { return x; }
            set {
                if (value > Constants.BoundX - this.Width)
                    x = Constants.BoundX - this.Width;
                else if (value <= 0)
                    x = 0;
                else
                    x = value;
            }
        }
        private double y = 200;
        public double Y {
            get { return y; }
            set {
                if (value > Constants.BoundY - this.Height)
                    y = Constants.BoundY - this.Height;
                else if (value <= 0)
                    y = 0;
                else
                    y = value;
            }
        }

        public int Hunger = 600;
        public bool Alive = true;

        public double Width = 32;
        public double Height = 35;

        public double velocityX = 0;
        public double velocityY = 0;

        public Brain brain;

        private double velocityIncrease;
        private double velocityDecay;
        private Image img;

        public Asdrubal(double velocityIncrease, double velocityDecay) {
            this.img = getImg();
            this.velocityIncrease = velocityIncrease;
            this.velocityDecay = velocityDecay;
            this.brain = new Brain();
        }

        public Rectangle Draw() {
            if (Alive) {

                if (Hunger <= 0) {
                    Alive = false;
                }

                this.CalculatePosition();

                Hunger--;

                var r = new Rectangle() {
                    Fill = new ImageBrush(img.Source),
                    Width = this.Width, Height = this.Height,
                    Margin = new System.Windows.Thickness(X, Y, 0, 0)
                };

                r.Tag = "Asdrubal";
                return r;
            } else return null;
        }

        private Rectangle getRectangle() {
            return new Rectangle() {
                Fill = new ImageBrush(img.Source),
                Width = this.Width,
                Height = this.Height,
                Margin = new System.Windows.Thickness(X, Y, 0, 0)
            };
        }

        public Rect getRect() {
            return new Rect(this.X, this.Y, this.Width, this.Height);
        }

        private void CalculatePosition() {

            velocityX = ((velocityX < 0.05 && velocityX >=0) || velocityX > -0.05 && velocityX <= 0) ? 0 : velocityX;
            velocityY = ((velocityY < 0.05 && velocityY >=0) || velocityY > -0.05 && velocityY <= 0) ? 0 : velocityY;

            //velocity decay
            this.velocityX = (velocityX > 0 && velocityX != 0) ?
                velocityX - (velocityX / velocityDecay) :
                velocityX + (-velocityX / velocityDecay);
            this.velocityY = (velocityY > 0 && velocityY != 0) ?
                velocityY - (velocityY / velocityDecay) :
                velocityY + (-velocityY / velocityDecay);


            this.X += (velocityX > 5) ? 5 : (velocityX < -5) ? -5 : velocityX;
            this.Y += (velocityY > 5) ? 5 : (velocityY < -5) ? -5 : velocityY;
        }
        
        // 0 = R, 1 = D, 2 = L, 3 = U
        public void Move(byte direction) {
            switch (direction) {
                case 0:
                    velocityX = (velocityX >= 0) ? velocityX + velocityIncrease : 0;
                    break;
                case 1:
                    velocityY = (velocityY >= 0) ? velocityY + velocityIncrease : 0;
                    break;
                case 2:
                    velocityX = (velocityX <= 0) ? velocityX - velocityIncrease: 0;
                    break;
                case 3:
                    velocityY = (velocityY <= 0) ? velocityY - velocityIncrease : 0;
                    break;
                default:
                    throw new InvalidOperationException("Invalid direction");
            }
        }

        private Image getImg() {
            Image finalImage = new Image();
            finalImage.Width = 80;
            BitmapImage logo = new BitmapImage();
            logo.BeginInit(); //pack://application:,,,/AssemblyName;component/Resources/logo.png 
            logo.UriSource = new Uri(@"C:\Users\Felipe\Source\Repos\asdrubals\Asdrubals\Asdrubals\Resources\asdrubal.png");
            logo.EndInit();
            finalImage.Source = logo;
            return finalImage;
        }

        public void Consume() {
            this.Hunger = (Hunger>480)?600:Hunger + 120; 

        }

    }
}
