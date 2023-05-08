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
        {
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
                    else
                    {
                        
                        if (sz > generator.Next(101))
                        {
                            M[i, j] = 1;
                            M[j, i] = M[i, j];
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
            }
            MacierzDataGrid.AutoGenerateColumns = false;
            // Define the columns
            for (int i = 0; i < pkt; i++)
            {
                var col = new DataGridTextColumn();
                col.Header = $"{i}";
                col.Binding = new Binding($"[{i}]");
                MacierzDataGrid.Columns.Add(col);
            }

            // Set the ItemsSource
            MacierzDataGrid.ItemsSource = list;
        }
    }
}
