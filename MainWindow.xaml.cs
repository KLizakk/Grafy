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
        
        private void PrzyciskOdWszystkiego_Click(object sender, RoutedEventArgs e)
        { // Generowanie losowej macierzy zaleznej od ilosci pkt i szansy
            MacierzDataGrid.ItemsSource = null;
            MacierzDataGrid.Items.Clear();
            MacierzDataGrid.Columns.Clear();
            list.Clear();
            Lista.Clear();
            pkt = int.Parse(Rozmiar.Text);
            int[,] M = new int[pkt, pkt];
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

            sw.Close();
        }
        //Rysowanie ma byc w nowym oknie
        

    }
}
