using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
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


namespace Grafy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        static int pkt = 0;
        List<int> Lista = new List<int>();
        List<string[]> list = new List<string[]>();
        int[,] M;
        private void PrzyciskOdWszystkiego_Click(object sender, RoutedEventArgs e)
        { // Generowanie losowej macierzy zaleznej od ilosci pkt i szansy
            MacierzDataGrid.ItemsSource = null;
            MacierzDataGrid.Items.Clear();
            MacierzDataGrid.Columns.Clear();
            list.Clear();
            Lista.Clear();
            pkt = int.Parse(Rozmiar.Text);
            M = new int[pkt, pkt];
            int sz = int.Parse(Szansa.Text);


            Random generator = new Random();
            for (int i = 0; i < pkt; i++)
            {
                string[] m = new string[pkt];
                for (int j = 0; j < pkt; j++)
                {
                    if (j == i)
                    {
                        M[i, j] = 0;
                    }
                    else if (j > i)
                    {
                        if (sz > generator.Next(101))
                        {
                            M[i, j] = 1;
                            M[j, i] = 1;
                        }
                        else
                        {
                            M[i, j] = 0;
                            M[j, i] = 0;
                        }
                    }
                    m[j] = M[i, j].ToString();
                }
                list.Add(m);
                MacierzDataGrid.Items.Add(list[i]);
            }
            // Definicja kolumn w datagridzie

            for (int i = 0; i < pkt; i++)
            {
                var col = new DataGridTextColumn();
                col.Header = $"{i}";
                col.Binding = new Binding($"[{i}]");
                MacierzDataGrid.Columns.Add(col);
            }




            //suma wierszy
            int[] suma = new int[pkt];

            for (int i = 0; i < pkt; i++)
            {
                suma[i] = 0;
                for (int j = 0; j < pkt; j++)
                {
                    suma[i] += M[i, j];
                }

            }



            foreach (int i in suma)
            {
                Lista.Add(i);
            }

            Lista.Sort();

            for (int i = pkt; i > 0; i--)
            {
                if (i == pkt)
                {
                    Posortowane.Text = ("{" + Lista[i - 1] + ",");
                }
                if (i == 1)
                {
                    Posortowane.Text += (Lista[i - 1] + "}");
                }
                else
                {
                    Posortowane.Text += (Lista[i - 1] + ",");
                }

            }

            //sąsiadowanie
            Sasiadowanie.Items.Clear();
            string[] sasiady = new string[pkt];
            for (int i = 0; i < pkt; i++)
            {

                for (int j = 0; j < pkt; j++)
                {

                    if (M[i, j] == 1)
                    {
                        sasiady[i] += (j + " ");
                    }
                }
                Sasiadowanie.Items.Add("punkt " + i + " sąsiaduje z : " + sasiady[i]);

            }

            //Suma krawędzi w grafie : 
            int sumaPolaczen = 0;
            foreach (var item in suma)
            {
                sumaPolaczen += item;
            }
            SumaKrawedzi.Text = (sumaPolaczen).ToString();

            //Gestość grafu

            GestoscGrafu.Text = (2*Double.Parse(SumaKrawedzi.Text) / pkt * (pkt - 1)).ToString();
        }

        private void Zapisz_Click(object sender, RoutedEventArgs e)
        {
            string projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string filePath = System.IO.Path.Combine(projectDirectory, "Save.txt");
            StreamWriter sw = new StreamWriter(filePath);
            sw.WriteLine("Macierz : ");


            //Wpisanie macierzy do pliku TXT
            foreach (string[] tablica in list)
            {
                foreach (string item in tablica)
                {
                    sw.Write(item + " ");
                }
                sw.WriteLine();
            }
            sw.WriteLine();
            //Wpisanie sąsiadowania do macierzy 

            sw.WriteLine("Sąsiadowanie");
            foreach (var item in Sasiadowanie.Items)
            {
                sw.WriteLine(item.ToString());
            }

            //Wpisanie szeregu 
            sw.WriteLine();
            sw.WriteLine("Posortowany :");
            sw.WriteLine(Posortowane.Text);
            //Wypisanie sumy połączń w grafie
            sw.WriteLine("Suma połączeń w grafie:" + SumaKrawedzi.Text);

            sw.WriteLine();
            sw.WriteLine("Gęstość grafu to :" + GestoscGrafu);
            sw.Close();
        }
        //Rysowanie ma byc w nowym oknie



        //BFS
        struct dane
        {
            public int odległość;
            public int poprzednik;
        };
        static dane[] BFS(int[,] macierz, int start)
        {
            dane[] tab = new dane[macierz.GetLength(0)];
            for (int i = 0; i < macierz.GetLength(0); i++)
            {
                tab[i].odległość = int.MaxValue;
                tab[i].poprzednik = -1;
            }
            tab[start].odległość = 0;
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            while (q.Count != 0)
            {
                int u = q.Dequeue();
                for (int i = 0; i < macierz.GetLength(0); i++)
                {
                    if (macierz[u, i] > 0 && tab[i].odległość == int.MaxValue)
                    {
                        tab[i].odległość = tab[u].odległość + 1;
                        tab[i].poprzednik = u;
                        q.Enqueue(i);
                    }
                }
            }
            return tab;
        }

        static void wypiszdane(int i, dane d, ListBox blist)
        {

            blist.Items.Add("{0}\t" + i);
            if (d.odległość == int.MaxValue)
            {
                blist.Items.Add("nieosiągalny");
            }
            else
            {
                blist.Items.Add($"{0}\t" + d.odległość);
                if (d.poprzednik == -1)
                    blist.Items.Add("brak");
                else blist.Items.Add("{0}" + d.poprzednik); ;
            }

        }

        private void BFSbutton_Click(object sender, RoutedEventArgs e)
        {
            dane[] tab = BFS(M, 0);
            for (int i = 0; i < pkt; i++)
            {

                wypiszdane(i, tab[i], BFSlistBox);
            }
        }

        //Metodka rysująca

        public void Rysuj(Canvas canvas)
        {

            canvas.Children.Clear();
            int nodeCount = pkt;
            int nodeSize = 20; // rozmiar wierzchołka
            double centerX = nodeCount * nodeSize / 2;
            double centerY = nodeCount * nodeSize / 2;
            double radius = nodeCount * nodeSize / 2 - nodeSize / 2; // promień okręgu
            for (int i = 0; i < nodeCount; i++)
            {
                // obliczenie pozycji wierzchołka na okręgu
                double angle = 2 * Math.PI / nodeCount * i;
                double x = centerX + radius * Math.Cos(angle);
                double y = centerY + radius * Math.Sin(angle);

                // rysowanie wierzchołka
                Ellipse node = new Ellipse()
                {
                    Width = nodeSize,
                    Height = nodeSize,
                    Fill = Brushes.Red
                };
                Canvas.SetLeft(node, x - nodeSize / 2);
                Canvas.SetTop(node, y - nodeSize / 2);
                canvas.Children.Add(node);
                //tu ma byc dodawanie numeru wierzcholka
 
                // rysowanie krawędzi
                for (int j = 0; j < nodeCount; j++)
                {
                    if (M[i, j] == 1)
                    {
                        // obliczenie pozycji wierzchołka docelowego na okręgu
                        double angle2 = 2 * Math.PI / nodeCount * j;
                        double x2 = centerX + radius * Math.Cos(angle2);
                        double y2 = centerY + radius * Math.Sin(angle2);

                        Line edge = new Line()
                        {
                            X1 = x,
                            Y1 = y,
                            X2 = x2,
                            Y2 = y2,
                            Stroke = Brushes.Black,
                            StrokeThickness = 2
                        };
                        canvas.Children.Add(edge);
                    }
                }
                FormattedText number = new FormattedText(
i.ToString(),
System.Globalization.CultureInfo.CurrentCulture,
FlowDirection.LeftToRight,
new Typeface("Green"),
12,
Brushes.White,
VisualTreeHelper.GetDpi(node).PixelsPerDip
);
                TextBlock textBlock = new TextBlock()
                {
                    Text = i.ToString(),
                    Foreground = Brushes.Blue,
                    FontSize = 16
                };
                Canvas.SetLeft(textBlock, x - nodeSize / 2 + 4);
                Canvas.SetTop(textBlock, y - nodeSize / 2 + 4);
                canvas.Children.Add(textBlock);
            }

        }

        private void Rysujbutton_Click(object sender, RoutedEventArgs e)
        {
            
            Rysuj(CanvasRysuj);
        }
    }
}
