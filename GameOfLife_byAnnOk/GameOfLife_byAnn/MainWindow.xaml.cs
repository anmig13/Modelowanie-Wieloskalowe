using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Threading;


namespace GameOfLife_byAnn
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static int width = 700; 
        static int height = 700;
        static int cell_size = 60;

        public MainWindow()
        {
            InitializeComponent();
            label.Content = " GAME OF LIFE";
            timer.Interval = TimeSpan.FromSeconds(0.2);
            timer.Tick += Timer_Tick;  // krok czasowy który obługuje animacje
            buttonStartStop.Background = Brushes.Turquoise;
            buttonStart.Background = Brushes.PaleVioletRed; // animation button
        }

        Rectangle[,] field = new Rectangle[width, height];
        DispatcherTimer timer = new DispatcherTimer();

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            width = Int32.Parse(textBoxw.Text);
            height = Int32.Parse(textBox.Text);
        }

        //Button START
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //.............RANDOMpocz1
            System.Collections.Generic.List<int> iList = new System.Collections.Generic.List<int>();

            if (random_button.IsChecked == true)
            {

                int liveCells = Int32.Parse(textBox1.Text);
                Random rand = new Random();
                int count = 0;
                while (count < liveCells)
                {
                    int number = rand.Next(0, width * height);
                    if (!iList.Contains(number))
                    {
                        iList.Add(number);
                        count++;
                    }
                }
            }

            int cells = 0;

            //.............RANDOMkoniec1

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle r = new Rectangle();
                    // r.Width = (board.ActualWidth / width)- 2.0;          // board-canvas window
                    // r.Height = (board.ActualHeight / height)- 2.0;
                    r.Width = (board.ActualWidth/ cell_size) - 0.5;       
                    r.Height =(board.ActualWidth / cell_size )- 0.5;
                   // r.Width = cell_size;
                   // r.Height = cell_size;


                    r.Fill = Brushes.HotPink;

                    if (random_button.IsChecked == true)
                    {
                        //.............RANDOMpocz2
                        if (iList.Contains(cells))
                        {
                            r.Fill = Brushes.Black;
                        }
                        else
                        {
                            r.Fill = Brushes.HotPink;
                        }
                        //.............RANDOMkoniec2

                        r.MouseDown += R_MouseDown;
                    }

                    board.Children.Add(r);

                    //Canvas.SetLeft(r, j * board.ActualWidth / width); // ustal położenie następnego małego kwadracika
                    // Canvas.SetTop(r, i * board.ActualHeight / height );  // - 2.0);
                    Canvas.SetLeft(r, j * (board.ActualWidth / cell_size)); // ustal położenie następnego małego kwadracika
                    Canvas.SetTop(r, i * (board.ActualHeight / cell_size));  // - 2.0);


                    if (manual_button.IsChecked == true)  // manual definition
                    {
                        // textBox1.Clear();
                        r.MouseDown += R_MouseDown;  // do każdego kwadratu dodaje zdarzenie albo kwadrat będzie niebieski-martwy , albo zaznaczam jako żywy-czarny
                    }



                    if (oscilator_button.IsChecked == true)   // oscilator
                    {
                        //textBox1.Clear();
                        if ((i == height / 2 && j == width / 2) || (i == (height / 2) + 1 && j == width / 2) || (i == (height / 2) - 1 && j == width / 2))
                        {
                            r.Fill = Brushes.Black;
                        }

                        r.MouseDown += R_MouseDown;

                    }


                    if (stable_button.IsChecked == true)   // stable
                    {
                        //  textBox1.Clear();
                        if ((i == (height / 2) - 2 && j == width / 2) || (i == (height / 2) - 2 && j == (width / 2) - 1) || (i == (height / 2) && j == width / 2) || (i == (height / 2) && j == (width / 2) - 1))
                        {
                            r.Fill = Brushes.Black;
                        }
                        if ((i == (height / 2) - 1 && j == (width / 2) - 2) || (i == (height / 2) - 1 && j == (width / 2) + 1))
                        {
                            r.Fill = Brushes.Black;
                        }

                        r.MouseDown += R_MouseDown;

                    }


                    if (glider_button.IsChecked == true)   // stable
                    {
                        //textBox1.Clear();
                        if ((i == (height / 2) - 2 && j == width / 2) || (i == (height / 2) - 2 && j == (width / 2) - 1) || (i == (height / 2) - 1 && j == (width / 2) - 2) || (i == (height / 2) - 1 && j == (width / 2) - 1))
                        {
                            r.Fill = Brushes.Black;
                        }
                        if (i == (height / 2) && j == (width / 2))
                        {
                            r.Fill = Brushes.Black;
                        }

                        r.MouseDown += R_MouseDown;

                    }


                    field[i, j] = r;  // do globalnej tablicy kwadrartów przypisuje te kolejne kwadraty kliknięte lub nie, żeby później  móc sprawdzić sąsiedztwo moore'a
                    cells++; //random                
                }
            }


        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ((Rectangle)sender).Fill = (((Rectangle)sender).Fill == Brushes.HotPink) ? Brushes.Black : Brushes.HotPink;
        }

        //button start/ stop animation

        private void buttonStartStop_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                buttonStartStop.Content = "Start animation";
                buttonStartStop.Background = Brushes.Turquoise;
            }
            else
            {
                timer.Start();
                buttonStartStop.Content = "Stop animation";
                buttonStartStop.Background = Brushes.Red;
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int[,] neighbors = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {

                    int count = 0;
                    int up = i - 1;
                    if (up < 0)
                    {
                        up = height - 1;
                    }
                    int down = i + 1;
                    if (down >= height)
                    {
                        down = 0;
                    }
                    int left = j - 1;
                    if (left < 0)
                    {
                        left = width - 1;
                    }
                    int right = j + 1;
                    if (right >= width)
                    {
                        right = 0;
                    }
                    if (field[up, left].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[up, j].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[up, right].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[i, left].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[i, right].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[down, left].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[down, j].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    if (field[down, right].Fill == Brushes.Black)
                    {
                        count++;
                    }
                    neighbors[i, j] = count;  // zliczam sąsiadów danej komórki/pola
                }
            }


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (neighbors[i, j] < 2 || neighbors[i, j] > 3)         // reguły przejścia
                    {
                        field[i, j].Fill = Brushes.HotPink;
                    }
                    else if (neighbors[i, j] == 3)
                    {
                        field[i, j].Fill = Brushes.Black;
                    }
                }
            }



        }

        private void stable_button_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }

        private void buttonClearClear_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    field[i, j].Fill = Brushes.White;
                }

            }
        }
    }
}