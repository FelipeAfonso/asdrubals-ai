using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asdrubals {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        List<Key> pressed_keys = new List<Key>();

        Asdrubal asd;
        Asdrubal asd2;
        List<Food> foods = new List<Food>();

        public MainWindow() {
            InitializeComponent();

        }

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);

            Constants.BoundX = this.MainCanvas.ActualWidth;
            Constants.BoundY = this.MainCanvas.ActualHeight;

            asd = new Asdrubal(5, 2);
            asd2 = new Asdrubal(5, 2);


            new Thread(() => {
                while (true) {
                    try {
                        Dispatcher.Invoke(() => {
                            MainCanvas.Children.Clear();
                            if (asd.Alive || asd2.Alive) {

                                if(pressed_keys.Count > 0) {
                                    foreach(Key k in pressed_keys) {
                                        switch (k) {
                                            case Key.Right:
                                                asd.Move(0);
                                                break;
                                            case Key.Down:
                                                asd.Move(1);
                                                break;
                                            case Key.Left:
                                                asd.Move(2);
                                                break;
                                            case Key.Up:
                                                asd.Move(3);
                                                break;
                                            case Key.D:
                                                asd2.Move(0);
                                                break;
                                            case Key.S:
                                                asd2.Move(1);
                                                break;
                                            case Key.A:
                                                asd2.Move(2);
                                                break;
                                            case Key.W:
                                                asd2.Move(3);
                                                break;
                                            default: break;
                                        }
                                    }
                                }

                                foreach (Food f in foods) {
                                    if (asd.getRect().IntersectsWith(f.getRect()) && !f.Consumed) {
                                        asd.Consume();
                                        f.Consumed = true;
                                    }
                                    if (asd2.getRect().IntersectsWith(f.getRect()) && !f.Consumed) {
                                        asd2.Consume();
                                        f.Consumed = true;
                                    }
                                }
                                MainCanvas.Children.Add(asd.Draw());
                                MainCanvas.Children.Add(asd2.Draw());

                            }
                            foreach (var f in foods) {
                                if (!f.Consumed)
                                    MainCanvas.Children.Add(f.Draw());
                            }
                            var s = "";
                            foreach(Key k in pressed_keys) {
                                s += k.ToString();
                            }
                            label.Content = (asd.Alive) ? asd.Hunger.ToString() : "";
                            label2.Content = (asd2.Alive) ? asd2.Hunger.ToString() : "";

                        });
                        Thread.Sleep(1000 / 60);
                    } catch { }
                }
            }).Start();

            new Thread(() => {
                while (true) {
                    Dispatcher.Invoke(() => {
                        for (int i = 0; i < 10; i++) {
                            foods.Add(new Food());
                        }
                    });
                    Thread.Sleep(10000);
                }
            }).Start();

        }

        private void MainCanvas_KeyDown(object sender, KeyEventArgs e) {
            if (!pressed_keys.Contains(e.Key))
                pressed_keys.Add(e.Key);
            e.Handled = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e) {
            pressed_keys.Remove(e.Key);
            e.Handled = true;
        }
    }
}
