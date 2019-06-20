﻿using System;
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
{    // Rekrystalizacja OKSY
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
            timer.Interval = TimeSpan.FromSeconds(0.00000000000000001);
            timer.Tick += Timer_Tick;

            timer2.Interval = TimeSpan.FromSeconds(0.00000000000000000000000000000000000001);
            timer2.Tick += Timer_Tick2;

            timer3.Interval = TimeSpan.FromSeconds(0.00000000000000001);
           // timer3.Tick += Timer_Tick3;

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
        DispatcherTimer timer2 = new DispatcherTimer(); // do MonteCarlo
        DispatcherTimer timer3 = new DispatcherTimer();

        Dictionary<data, SolidColorBrush> colors;
        Dictionary<data, int> cellID;


        static int cell_size = 80;
        double A = 86710969050178.5;
        double B = 9.41268203527779;
        double end_time = 0.2;
        double criticalDensity = 4215840142323.42;
   
        bool[,,] isRecrystallized;
        double[,,] density;
        double criticalDensityCell;

        Dictionary<data, SolidColorBrush> colors2Rec;
        List<double> summaryDensity = new List<double>();

        private double calculateDensity(double t)
        {
            double ro = 0;
            ro = A / B + (1 - A / B) * Math.Exp(-B * t);
            return ro;
        }


        private bool checkIfRecrys(int i, int j)
        {
            bool[] nei = getNeighboursRecr(isRecrystallized, i, j);

            bool checkRecr = false;
            //to change for checking neighbours recrystalization
            for (int ii = 0; ii < nei.Length; ii++)
            {
                if (nei[ii] == true) checkRecr = true;    // jeżeli któryś z sąsiadów zrekrystalizował zwróć true
            }
            if (checkRecr == false) return false;


            double[] neiD = getNeighboursDens(density, i, j);
            for(int ii = 0; ii < neiD.Length; ii++)
            {
                if (density[i, j, 0] < neiD[ii]) return false;
            }

            return true;
        }


        private double[] getNeighboursDens(double[,,] tab, int i, int j)
        {
            double[] neighb;

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
            neighb = new double[4];
            neighb[0] = tab[up, j, 0];
            neighb[1] = tab[i, left, 0];
            neighb[2] = tab[i, right, 0];
            neighb[3] = tab[down, j, 0];


            return neighb;
        }


        private bool[] getNeighboursRecr(bool[,,] tab, int i, int j )
        {
            bool[] neighb;

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

            neighb = new bool[4];
            neighb[0] = tab[up, j, 0];
            neighb[1] = tab[i, left, 0];
            neighb[2] = tab[i,right, 0];
            neighb[3] = tab[down,j, 0];

            return neighb;
        }


        private int[] getNeighbours(int[,,] tab, int i, int j)
        {
            int[] neighb;

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
            neighb = new int[4];
            neighb[0] = tab[up, j, 0];
            neighb[1] = tab[i, left, 0];
            neighb[2] = tab[i, right, 0];
            neighb[3] = tab[down, j, 0];

            return neighb;
        }


         // button recrystalization
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            colors2Rec = new Dictionary<data, SolidColorBrush>(new MyClassSpecialComparer());
            isRecrystallized = new bool[height, width, 2];
            density = new double[height, width, 2];

            Random rand = new Random();

            //.............................sprawdz granice 
            int amount = 0;
            List<int> edgeList = new List<int>();

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {

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
                    if (colors[new data(up, left)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(up, j)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(up, right)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(i, left)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(i, right)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(down, left)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(down, j)] != colors[new data(i, j)]) { amount++; }
                    if (colors[new data(down, right)] != colors[new data(i, j)]) { amount++; }


                    if (amount > 0)
                    {
                        edgeList.Add(cellID[new data(i, j)]);
                    }
                    amount = 0;
             
                }

            }//.................sprawdz granice koniec


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        isRecrystallized[i, j, k] = false;
                        density[i, j, k] = 0;
                    }
                }
            }

            int steps = 0;
            int quantityOfGrains = width * height;
            criticalDensityCell = criticalDensity / quantityOfGrains;

            //  Calculating summaryDensity and writing to file
            string createText = "";
            //string path = @"D:\file_results.csv";
            string path = "file_results.csv";

            for (double t=0.0; t <= end_time; t+=0.001)
            {
                steps++;
                double densityy = calculateDensity(t);
                summaryDensity.Add(densityy);
                createText += t + ";" + densityy + Environment.NewLine;//gestos dla każdego kroku czasowego, potem jeszcze trzeba podzielić przez ilos komorek/ziarn
            }
            System.IO.File.WriteAllText(path, createText);
       
            for (int t = 1; t < steps; t++)
            {
                 //  zapisuje wyniki z porzedniego kroku czasowego
                for (int i = 0; i < height; i++)
                { 
                    for (int j = 0; j < width; j++)
                    {
                        isRecrystallized[i, j, 0] = isRecrystallized[i, j, 1];
                        density[i, j, 0] = density[i, j, 1];
                    }
                }
                // nowy krok czasowy
                double deltaDensity = summaryDensity[t] - summaryDensity[t - 1];
                double densityPart1= 0.3 * deltaDensity;
                double densityPart2 = deltaDensity - densityPart1;
                double densityforAll = densityPart1 / quantityOfGrains;


                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        density[i, j, 1] = density[i, j, 0] + densityforAll;
                    }
                }

                //rozdaje drugą porcje dyslokacji losowym komórkom
                double additionalDensity = densityPart2 / (0.01);
                for (int i = 0; i < 100; i++)
                {
                    int ii = rand.Next(height);
                    int jj = rand.Next(width);

                    int random = rand.Next(11);
                    bool check = false;
                    if ( (edgeList.Contains(cellID[new data(ii, jj)])) && random < 8)
                    {
                        density[ii, jj, 1] += additionalDensity;
                        check = true;
                      
                    }
                    else if (random < 2)
                    {
                        density[ii, jj, 1] += additionalDensity;
                        check = true;
                        
                    }
                    if (check == false) i--;
                }

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if ((edgeList.Contains(cellID[new data(i, j)])) && density[i, j, 0] > criticalDensityCell && isRecrystallized[i, j, 0] == false)
                        {
                            //  nowe ziarno
                            density[i, j, 1] = 0;
                            isRecrystallized[i, j, 1] = true;
                            int R = rand.Next(0, 255);
                            int G = rand.Next(0);
                            int B = rand.Next(0);
                            var brush = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
                            colors2Rec.Add(new data(i, j), brush);
                            // colors[new data(i, j)] = colors2Rec[new data(i, j)];
                            field[i, j].Fill = brush;
                        }
                        else if (checkIfRecrys(i, j) && isRecrystallized[i, j,0] == false)   
                        {
                        //  rekrystalizacja   
                            density[i, j,1] = 0;
                            isRecrystallized[i, j,1] = true;
                        }

                    }
                }

                /*
                //  Visualizing changes
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {

                        if (isRecrystallized[i, j, 0])
                        {
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
                            if(isRecrystallized[up, j, 0]== true)
                            {
                                field[i, j].Fill = colors[new data(up, j)];
                                colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;

                            }
                            else if (isRecrystallized[i, left, 0] == true)
                            {
                                field[i, j].Fill = colors[new data(i, left)];
                                colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;
                            }
                            else if (isRecrystallized[i, right, 0] == true)
                            {
                                field[i, j].Fill = colors[new data(i , right)];
                                colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;
                            }
                            else if (isRecrystallized[down, j, 0] == true)
                            {
                                field[i, j].Fill = colors[new data(down, j)];
                                colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;
                            }
                        }

                    }
                } */
            }

        }

 
        //button   
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

                while (counter < liveCells)  // dopoki nie wylosuje tyle środkow promeni ile zarodkow/zywaych komorek
                {
                    if (!busyCells.ContainsKey(new data(a, b)))  // jezeli dana koorka-promien nie jest zajeta
                    {
                        tabR[counter, 0] = a; // a -szerokość 
                        tabR[counter, 1] = b; // b -wysokosc
                        iList.Add(cellID[new data(a, b)]);
                        counter++;

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
                    }

                    bool pomm = false;
                    int counterpom = 0;
                    while (true)//losuje dop
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
                        label5.Content = "Za duzy promien lub za duzo komorek";
                        break;
                    }
                }

            }


            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle r = new Rectangle();
                    //r.Width = board.ActualWidth / width - 0.5;
                    //r.Height = board.ActualHeight / height - 0.5;

                    r.Width = (board.ActualWidth / cell_size) - 0.5;
                    r.Height = (board.ActualWidth / cell_size) - 0.5;

                    r.Fill = Brushes.HotPink;

                    if (iList.Contains(cellID[new data(i, j)]))  // jeżeli iLista zawiera dany numer/id komórki - wylosowanej randomowo to jn.
                    {
                        int R = rand.Next(0);
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
            int R = rand.Next(0);
            int G = rand.Next(0, 255);
            int B = rand.Next(0, 255);
            var brush = new SolidColorBrush(Color.FromRgb((byte)R, (byte)G, (byte)B));
            ((Rectangle)sender).Fill = brush;
        }


        int[,] tabE = new int[height, width];
        private void Timer_Tick2(object sender, EventArgs e)
        {

            if (checkBoxEnergy.IsChecked == false)
            { 
                Random rand = new Random();

            Dictionary<data, int> busyCells = new Dictionary<data, int>(new MyClassSpecialComparer());

            int i = rand.Next(0, height);
            int j = rand.Next(0, width);
            int energy1 = 0;
            int energy2 = 0;
            int deltaE = 0;
                //label5.Content = i + " " + j;
                if (!busyCells.ContainsKey(new data(i, j)))
                {

                    busyCells.Add(new data(i, j), 1);
                    //liczymy energie E1
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
                    if (colors[new data(up, left)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(up, j)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(up, right)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(i, left)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(i, right)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(down, left)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(down, j)] != colors[new data(i, j)]) { energy1++; }
                    if (colors[new data(down, right)] != colors[new data(i, j)]) { energy1++; }


                    int ni = 0;
                    int nj = 0;
                    int x = rand.Next(0,8);
                    if(x == 0)
                    {
                        ni = up; 
                        nj = left;
                    }
                    else if (x == 1)
                    {
                        ni = up;
                        nj = j;
                    }
                    else if(x == 2)
                    {
                        ni = up;
                        nj = right;
                    }
                    else if(x == 3)
                    {
                        ni = i;
                        nj = left;
                    }
                    else if(x == 4)
                    {
                        ni = i;
                        nj = right;
                    }
                    else if(x == 5)
                    {
                        ni = down;
                        nj = left;
                    }
                    else if(x == 6)
                    {
                        ni = down;
                        nj = j;
                    }
                    else if(x == 7)
                    {
                        ni = down;
                        nj = right;
                    }

                    SolidColorBrush newColor = colors[new data(ni, nj)];

                    if (colors[new data(up, left)] != newColor) { energy2++; }
                    if (colors[new data(up, j)] != newColor) { energy2++; }
                    if (colors[new data(up, right)] != newColor) { energy2++; }
                    if (colors[new data(i, left)] != newColor) { energy2++; }
                    if (colors[new data(i, right)] != newColor) { energy2++; }
                    if (colors[new data(down, left)] != newColor) { energy2++; }
                    if (colors[new data(down, j)] != newColor) { energy2++; }
                    if (colors[new data(down, right)] != newColor) { energy2++; }


                    deltaE = energy2 - energy1;

                    //if (energy2 <= energy1)
                    if (deltaE <= 0)
                    {
                        field[i, j].Fill = newColor;
                        colors[new data(i, j)] = (SolidColorBrush)field[i, j].Fill;
                        //colors.Add(new data(i, j), (SolidColorBrush)r.Fill);

                    }
                    else
                    {
                        Random random1 = new Random();
                        double kT = 6;
                        double value_random = random1.NextDouble();
                        double p = Math.Exp(-deltaE / kT);

                        if (value_random < p)
                        {
                            field[i, j].Fill = newColor;
                        }

                    }

                }
            
              
            }
            /*
            else
            {
                   //energy
            }*/

        }

        private void butttonEnergy_Click(object sender, RoutedEventArgs e)
        {

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {

                        if (tabE[i, j] == 0)
                        {
                            field[i, j].Fill = Brushes.Blue;

                        }
                        if (tabE[i, j] <0)
                        {
                        field[i, j].Fill = Brushes.Green;
                        }

                    }
                }
         
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


            if (checkBoxMoore.IsChecked == true)  // Moor neighborhood
            {
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

                        if (colors[new data(up, left)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(up, left)]))
                            {
                                dict[colors[new data(up, left)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(up, left)], 1);
                            }
                        }

                        if (colors[new data(down, left)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(down, left)]))
                            {
                                dict[colors[new data(down, left)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(down, left)], 1);
                            }
                        }

                        if (colors[new data(up, right)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(up, right)]))
                            {
                                dict[colors[new data(up, right)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(up, right)], 1);
                            }
                        }

                        if (colors[new data(down, right)] != Brushes.HotPink)
                        {
                            if (dict.ContainsKey(colors[new data(down, right)]))
                            {
                                dict[colors[new data(down, right)]]++;
                            }
                            else
                            {
                                dict.Add(colors[new data(down, right)], 1);
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
                    } //////////////////
                }
            }


            if (checkBoxPentagonal.IsChecked == true)
            {

                Random generator = new Random();

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                      
                       int a = generator.Next(2);
                      //  Console.WriteLine(a);
                        label5.Content = a;

                        if (a == 0)
                       {

                            //pentagonal left neighborhood
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
                            if (colors[new data(up, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, left)]))
                                {
                                    dict[colors[new data(up, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, left)], 1);
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
                            if (colors[new data(down, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, left)]))
                                {
                                    dict[colors[new data(down, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, left)], 1);
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

                        }
                        else
                        {   
                            //pentagonalright neighborhood
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
                            if (colors[new data(up, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, right)]))
                                {
                                    dict[colors[new data(up, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, right)], 1);
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
                            if (colors[new data(down, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, right)]))
                                {
                                    dict[colors[new data(down, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, right)], 1);
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


                      }//koniec if-else 0,1
                        


                    }/////////////////
                }
            }

            // ok left

            if (checkBoxHeksagonal.IsChecked == true)
            { //hexagonal neighborhood

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Random side = new Random();
                        int number = side.Next(0, 2);

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

                            if (colors[new data(up, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, right)]))
                                {
                                    dict[colors[new data(up, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, right)], 1);
                                }
                            }
                            if (colors[new data(down, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, left)]))
                                {
                                    dict[colors[new data(down, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, left)], 1);
                                }
                            }
                        //}
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

                    }
                }
            }//////////


          //  right
            if (checkBoxHexRight.IsChecked == true)
            { //hexagonal  right neighborhood

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

                            if (colors[new data(up, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, left)]))
                                {
                                    dict[colors[new data(up, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, left)], 1);
                                }
                            }

                            if (colors[new data(down, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, right)]))
                                {
                                    dict[colors[new data(down, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, right)], 1);
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

                    }
                }
            }//////////

       
            if (checkBoxHexRand.IsChecked == true)
            { //hexagonal  random neighborhood

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Random side = new Random();
                        int number = side.Next(0, 2);

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
                        if (number == 0)
                        {
                            if (colors[new data(up, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, left)]))
                                {
                                    dict[colors[new data(up, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, left)], 1);
                                }
                            }

                            if (colors[new data(down, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, right)]))
                                {
                                    dict[colors[new data(down, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, right)], 1);
                                }
                            }
                        }
                        else
                        {
                            if (colors[new data(up, right)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(up, right)]))
                                {
                                    dict[colors[new data(up, right)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(up, right)], 1);
                                }
                            }
                            if (colors[new data(down, left)] != Brushes.HotPink)
                            {
                                if (dict.ContainsKey(colors[new data(down, left)]))
                                {
                                    dict[colors[new data(down, left)]]++;
                                }
                                else
                                {
                                    dict.Add(colors[new data(down, left)], 1);
                                }
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

                    }
                }
            }//////////



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
                Button_animation.Content = "Grain growth";
                Button_animation.Background = Brushes.LightGreen;
        
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

        private void buttonMonteCarlo_Click_1(object sender, RoutedEventArgs e)
        {
            if (timer2.IsEnabled)
            {
                timer2.Stop();
                buttonMonteCarlo.Content = "Monte Carlo";
                buttonMonteCarlo.Background = Brushes.Turquoise;
            }
            else
            {
                timer2.Start();
                buttonMonteCarlo.Content = "Stop animation";
                buttonMonteCarlo.Background = Brushes.Red;
            }
        }

    }




}