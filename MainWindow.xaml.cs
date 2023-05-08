using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

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

        private void PrzyciskOdWszystkiego_Click(object sender, RoutedEventArgs e)
        { // Generowanie losowej macierzy
            MacierzDataGrid.Items.Clear();
            int pkt = int.Parse(Rozmiar.Text);
            List<string[]> list = new List<string[]>();
            int sz = int.Parse(Szansa.Text);

            int[,] M = new int[pkt, pkt];
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
            // Define the columns
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
            List<int> Lista = new List<int>();


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
    }
}
