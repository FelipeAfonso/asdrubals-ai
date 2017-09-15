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

        List<Asdrubal> player_list = new List<Asdrubal>();
        List<Player> connected_players = new List<Player>();


        List<Food> foods = new List<Food>();

        public MainWindow() {
            InitializeComponent();

        }

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);

            Constants.BoundX = this.MainMenu.ActualWidth;
            Constants.BoundY = this.MainMenu.ActualHeight;

            new Thread(() => {

                TCPServer.start();
                while (true) {
                    if (TCPServer.accept()) {
                        var cl = TCPServer.clients.Last();
                        connected_players.Add(new Player() { Name = "Player " + TCPServer.clients.Count, Client = cl });
                        Dispatcher.Invoke(() => {
                            PlayerList.Items.Add(new Label() {
                                Content = "Player " + TCPServer.clients.Count
                            });
                        });
                    }
                }
            }).Start();

            new Thread(() => {
                while (true) {
                    try {
                        foreach (var c in TCPServer.clients) {
                            if (c.Connected) {
                                var player = connected_players.First(n => n.Client == c);
                                string s = TCPServer.receive(c);
                                Console.WriteLine($"Mensagem ({player.Name}): {s}" );
                                TCPServer.send("check", c);
                                foreach (char i in s) {
                                    int m = Int32.Parse(i.ToString());
                                    if (m < 4)
                                        if (!player.movement.Contains(m)) player.movement.Add(m);
                                        else
                                            switch (m) {
                                                case 4:
                                                    player.movement.Remove(0);
                                                    break;
                                                case 5:
                                                    player.movement.Remove(1);
                                                    break;
                                                case 6:
                                                    player.movement.Remove(2);
                                                    break;
                                                case 7:
                                                    player.movement.Remove(3);
                                                    break;
                                            }
                                }
                            } else {
                                var player = connected_players.First(p => p.Client == c);
                                TCPServer.closeClient(c);
                                connected_players.Remove(player);
                                Dispatcher.Invoke(() => {
                                    foreach (Label l in PlayerList.Items) {
                                        if ((string)l.Content == player.Name) PlayerList.Items.Remove(l);
                                    }
                                });
                            }
                        }
                    } catch { }
                }
            }).Start();
        }

        private void start_game(int players) {
            Thread.Sleep(100);
            foreach (Player p in connected_players) {
                p.asdrubal = new Asdrubal(5, 2, p.Name);
                player_list.Add(p.asdrubal);
            }
            Dispatcher.Invoke(() => {
                new Thread(() => {
                    while (true) {
                        try {
                            Dispatcher.Invoke(() => {
                                MainCanvas.Children.Clear();

                                foreach (var p in connected_players) {
                                    foreach (byte i in p.movement) {
                                        p.asdrubal.Move(i);
                                    }
                                }

                                foreach (Food f in foods) {
                                    foreach (Asdrubal a in player_list) {
                                        if (a.getRect().IntersectsWith(f.getRect()) && !f.Consumed) {
                                            a.Consume();
                                            f.Consumed = true;
                                        }
                                    }
                                }

                                foreach (Asdrubal a in player_list) {
                                    MainCanvas.Children.Add(a.Draw());
                                }

                                foreach (Food f in foods) {
                                    if (!f.Consumed)
                                        MainCanvas.Children.Add(f.Draw());
                                    else foods.Remove(f);
                                }
                                //var s = "";
                                //foreach (Key k in pressed_keys) {
                                //    s += k.ToString();
                                //}
                                //label.Content = (asd.Alive) ? asd.Hunger.ToString() : "";
                                //label2.Content = (asd2.Alive) ? asd2.Hunger.ToString() : "";

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



            });
        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            MainCanvas.Visibility = Visibility.Visible;
            MainMenu.Visibility = Visibility.Collapsed;

            start_game(2);
        }
    }
}
