using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections;
using System.Windows.Media.Imaging;

namespace GrainGrowth_byAnn
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
            public class data
            {
                public int i;
                public int j;
                public data(int _i, int _j)
                {
                    i = _i;
                    j = _j;
                }
            }
            public class MyClassSpecialComparer : IEqualityComparer<data>
            {
                public bool Equals(data x, data y)
                {
                    return x.i == y.i && x.j == y.j;
                }
                public int GetHashCode(data x)
                {
                    return x.i.GetHashCode() + x.j.GetHashCode();
                }
            }

            static int width = 800;
            static int height = 800;

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(0.000000001);
            timer.Tick += Timer_Tick;
            Button_animation.Background = Brushes.Turquoise;
            ButtonStart.Background = Brushes.PaleVioletRed;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            width = Int32.Parse(textBox.Text);
            height = Int32.Parse(textBoxheight.Text);
        }

        Rectangle[,] field = new Rectangle[width, height];
        DispatcherTimer timer = new DispatcherTimer();

        Dictionary<data, SolidColorBrush> colors;
        Dictionary<data, int> cellID;

        //.......................................
        static int cell_size = 60;

        //button Start/Clear
        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            colors = new Dictionary<data, SolidColorBrush>(new MyClassSpecialComparer());
            cellID = new Dictionary<data, int>(new MyClassSpecialComparer());

            List<int> iList = new List<int>();

       

            int cells = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    cellID.Add(new data(i, j), cells);  // dodaje wszystkie kolejne id komórek board'a do słownika
                    cells++;
                }
            }

            Random rand = new Random();
            if (radioButtonRandom.IsChecked == true)     // ****random1
            {
                int liveCells = Int32.Parse(textBoxRandom.Text);  // liczba zarodkow random 
       
            int count = 0;
      
                while (count < liveCells)
                {
                    int number = rand.Next(0, width * height);  // losuje randomowe numery komórek

                    if (!iList.Contains(number))
                    {
                        iList.Add(number);   // dodadaje numery wylosowanych randomowo komórek do listy iList
                        count++;
                    }
                }
            }


            if (radioButtonEvenly.IsChecked == true)    //evenly-rownomiernie
            {
                int liveCells = Int32.Parse(textBoxRow.Text);
                int liveCells2 = Int32.Parse(textBoxColumn.Text);


                //int l = (int)Math.Sqrt(liveCells);
                //int a = height / (2 * l);
                 //int b = width / (2 * l);
                 int a = height / (liveCells+2);
                 int b = width / (liveCells2+2);
                 int a2 = height / (liveCells);
                 int b2 = width / (liveCells2);


                for (int i = a; i < height; i = i +a2)
                {
                    for (int j = b; j < width; j = j +b2)
                    {
                        iList.Add(cellID[new data(i, j)]);
                    }
                }
            }


            if(radioButtonRandomR.IsChecked ==true)
            {
                int r = Int32.Parse(textBoxR.Text);
                int liveCells = Int32.Parse(textBoxLcell.Text);

                Dictionary<data, int> busyCells = new Dictionary<data, int>(new MyClassSpecialComparer());
               // int r = Int32.Parse(textBox2.Text);
                Random generator = new Random();
                int a = generator.Next(0, height);
                int b = generator.Next(0, width);

                busyCells.Add(new data(a, b), 1);
                int rownanie = 0;
                int[,] tabR = new int[liveCells, 2];

                for (int i = 0; i < liveCells; i++)
                {
                    tabR[i, 0] = -1;    //tyle ile jest żywych komórek tyle będzie środków promieni
                    tabR[i, 1] = -1;
                }

                int counter = 0;
                int ile_max_wylosowalo = 0;

                while (counter < liveCells)  // dopoki nie wylosuje tyle środkow promeni ile zarodkow/zywaych komorek
                {
                    if (!busyCells.ContainsKey(new data(a, b)))  // jezeli dana koorka-promien nie jest zajeta
                    {
                        tabR[counter, 0] = a; // a -szerokość 
                        tabR[counter, 1] = b; // b -wysokosc
                        iList.Add(cellID[new data(a, b)]);   // tutaj do ilisty dodaje promieni i poźniej je maluje
                        counter++;
                    }  /*
                        for (int i = 0; i < height; i++)
                        {
                            for (int j = 0; j < width; j++)
                            {
                                rownanie = ((int)Math.Pow(i - a, 2) + (int)Math.Pow(j - b, 2));
                            }
                        }
                    }
                    else
                    {
                        /////////
                    }*/

                    bool pomm = false;
                    int counterpom = 0;
                    while (true)//losuje dopoki nie znajde nowego promeinia
                    {
                        a = generator.Next(height);
                        b = generator.Next(width);
                        for (int i = 0; i < liveCells; i++)
                        {
                            rownanie = ((int)Math.Pow(a - tabR[i, 0], 2) + (int)Math.Pow(b - tabR[i, 1], 2));
                            //(x-a)^2-(y-b)^2<r^2
                            if (rownanie <= (int)Math.Pow(r, 2))
                            {
                                pomm = true; //losowane zarodki znajduja sie w obrebie kola! blad, nalezy wylosowac nowe punkty
                            }
                        }
                        if (pomm == false)//jesli znaleziono punkt przerwanie petli
                        {
                            ile_max_wylosowalo++;
                            break;
                        }
                        if (counterpom > 1000000) // gdy nie mozna znalezc punktow spelniajacych zalozenia
                        {
                            break;
                        }
                        pomm = false;
                        counterpom++;
                    }
                    if (counterpom > 1000000) //Przerywamy dzialanie poniewaz Za duzy R lub za duzo komorek
                    {
                        label5.Background = Brushes.Red;
                        label5.Content = "Za duzy promien \n lub za duzo komorek";
                        label6.Content = "MAX:" + ile_max_wylosowalo;
                        break;
                    }
                }

            }



            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle r = new Rectangle();
                   // r.Width = board.ActualWidth / width - 0.5;
                    //r.Height = board.ActualHeight / height - 0.5;

                    r.Width = (board.ActualWidth / cell_size) - 0.5;
                    r.Height = (board.ActualWidth / cell_size) - 0.5;

                    r.Fill = Brushes.HotPink;

                    if (iList.Contains(cellID[new data(i, j)]))  // jeżeli iLista zawiera dany numer/id komórki - wylosowanej randomowo to jn.
                    {
                        int R = rand.Next(0, 255);
                        int G = rand.Next(0, 255);
                        int B = rand.Next(0, 255);
                        var brush = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
                        r.Fill = brush;  // maluje na nowy kolor
                    } 
                    else
                    {
                        r.Fill = Brushes.HotPink;
                    }

                    board.Children.Add(r);
                    //Canvas.SetLeft(r, j * board.ActualWidth / width - 1.0);   // ***
                    //Canvas.SetTop(r, i * board.ActualHeight / height - 1.0);

                    Canvas.SetLeft(r, j * (board.ActualWidth / cell_size)); // ustal położenie następnego małego kwadracika
                    Canvas.SetTop(r, i * (board.ActualHeight / cell_size));

                    r.MouseDown += R_MouseDown;


                    if (radioButtonManual.IsChecked == true)     // ****manual
                    {
                        r.MouseDown += R_MouseDown;

                    }

                    field[i, j] = r;  // do globalnej tablicy kwadrartów przypisuje te kolejne kwadraty kliknięte lub nie, żeby później  móc sprawdzić sąsiedztwo von Neum

                    colors.Add(new data(i, j), (SolidColorBrush)r.Fill); // random - dodaj nowy kolor do słownika kolorów 
                }
            }



        }


        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Random rand = new Random();
            int R = rand.Next(0, 255);
            int G = rand.Next(0, 255);
            int B = rand.Next(0, 255);
            var brush = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
            ((Rectangle)sender).Fill = brush;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SolidColorBrush[,] neighbors = new SolidColorBrush[height, width];

            if (checkBoxNeum.IsChecked == true)
            { //von Neumann neighborhood

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Dictionary<SolidColorBrush, int> dict = new Dictionary<SolidColorBrush, int>();
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
                        if (colors[new data(up, j)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(up, j)]))
                            {
                                dict[colors[new data(up, j)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(up, j)], 1);
                            }
                        }

                        if (colors[new data(i, left)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(i, left)]))
                            {
                                dict[colors[new data(i, left)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(i, left)], 1);
                            }
                        }

                        if (colors[new data(i, right)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(i, right)]))
                            {
                                dict[colors[new data(i, right)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(i, right)], 1);
                            }
                        }

                        if (colors[new data(down, j)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(down, j)]))
                            {
                                dict[colors[new data(down, j)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(down, j)], 1);
                            }
                        }
                        KeyValuePair<SolidColorBrush, int> max = new KeyValuePair<SolidColorBrush, int>();
                        foreach (var kvp in dict)
                        {
                            if (kvp.Value > max.Value)
                                max = kvp;
                        }
                        if (max.Value == 0)
                        {
                            neighbors[i, j] = Brushes.HotPink;
                        }
                        else
                        {
                            neighbors[i, j] = max.Key;
                        }
                    }///////////////////////////////
                }
            }


            // zmiana koloru na ten którego jest najwięcej
            int start;
            int end1;
            int end2;
            if (checkBox_periodic.IsChecked == true)   // periodic
            {
                start = 0;
                end1 = height;
                end2 = width;
            }
            else
            {
                start = 1;
                end1 = height - 1;
                end2 = width - 1;
            }
            for (int i = start; i < end1; i++)
            {
                for (int j = start; j < end2; j++)
                {
                    if (checkBox_periodic.IsChecked != true)
                    {
                        field[0, j].Fill = Brushes.White;
                        field[height - 1, j].Fill = Brushes.White;
                        field[i, 0].Fill = Brushes.White;
                        field[i, width - 1].Fill = Brushes.White;

                        field[0, 0].Fill = Brushes.White;
                        field[height - 1, 0].Fill = Brushes.White;
                        field[0, width - 1].Fill = Brushes.White;
                        field[height - 1, width - 1].Fill = Brushes.White;
                    }

                    if (field[i, j].Fill == Brushes.HotPink)
                        field[i, j].Fill = neighbors[i, j];
                    colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;
                }
            }



        }


        // Start animation
        private void Button_animation_Click(object sender, RoutedEventArgs e)
        {

            if (timer.IsEnabled)
            {
                timer.Stop();
                Button_animation.Content = "Start animation";
                Button_animation.Background = Brushes.Turquoise;
        
            }
            else
            {
                timer.Start();
                Button_animation.Content = "Stop animation";
                Button_animation.Background = Brushes.Red;
            }


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
