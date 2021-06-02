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
using System.Drawing;
namespace GrafGry
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        string[] Tokeny;
        int[] Tokenylepsze;
        Graf graf = new Graf(new Wierzcholek(0, 0, 0));
        
        public MainWindow()
        {
            InitializeComponent();
            Tokeny = ListOfTokens.Text.Split(',');
            Tokenylepsze = new int[Tokeny.Length];
            for(int i = 0; i < Tokeny.Length; i++)
            {
                Tokenylepsze[i] = Convert.ToInt32(Tokeny[i]);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Tokeny = ListOfTokens.Text.Split(','); 
            for (int i = 0; i < Tokeny.Length; i++)
            {
                Tokenylepsze[i] = Convert.ToInt32(Tokeny[i]);
            }
            int Max = Convert.ToInt32(Cos.Text);
            Graf graf = new Graf(new Wierzcholek(0,0,0));
            for (int i = 0; i < graf.Wierzcholki.Count; i++)
            {
                graf.StworzWierzcholki(graf.Wierzcholki[i], Tokeny, Max);
            }
        }
        static int log2(int n)
        {
            return (n == 1) ? 0 : 1 + log2(n / 2);
        }
        public class Wierzcholek
        {
            public int wynik { get; set; }
            public int Linka { get; set; }
            public int koniec { get; set; }
            public int Deepth = 0;
            public string Gracz = "Protagonista";
            public List<Wierzcholek> Childrens = new List<Wierzcholek>();
            public Wierzcholek(int wartosc, int krawedz, int Glebokosc)
            {
                wynik = wartosc;
                Linka = krawedz;
                Deepth = Glebokosc;
            }
        }
        public class Graf
        {
            public List<Wierzcholek> Wierzcholki = new List<Wierzcholek>();
            public int Deepth = 0;
            public Graf(Wierzcholek korzeń)
            {
                Wierzcholki.Add(korzeń);
            }
            public void StworzWierzcholki(Wierzcholek wierzcholek, string[] tokeny, int Max)
            {
                if (wierzcholek.wynik < Max)
                {
                    for (int i = 0; i < tokeny.Length; i++)
                    {
                        Wierzcholek Node = new Wierzcholek(wierzcholek.wynik + Convert.ToInt32(tokeny[i]),Convert.ToInt32(tokeny[i]),wierzcholek.Deepth+1);
                        Wierzcholki.Add(Node);
                        wierzcholek.Childrens.Add(Node);
                        //if (Node.wynik > 21)
                        //{
                        //    Node.koniec = -1;
                        //}
                        //else if (Node.wynik == 21)
                        //{
                        //    Node.koniec = 0;
                        //}
                        //else
                        //{
                        //    Node.wynik = 1;
                        //}
                    }
                }
            }
            public int Koniec()
            {
                int OstatniaGlebokosc = Wierzcholki[Wierzcholki.Count-1].Deepth;
                return OstatniaGlebokosc;
            } 
            public int MiniMax(int depth, int NodeIndex, bool isMax, int[] scores, int h)
            {
                if(depth == h) //koniec reached
                {
                    return scores[NodeIndex];
                }
                if (isMax)
                {
                    return Math.Max(MiniMax(depth + 1, NodeIndex * 2, false, scores, h), MiniMax(depth + 1, NodeIndex * 2 + 1, false, scores, h));
                }
                else
                {
                    return Math.Min(MiniMax(depth + 1, NodeIndex * 2, true, scores, h),            MiniMax(depth + 1, NodeIndex * 2 + 1, true, scores, h));
                }
            }
        }
        public class Krawedz
        {
            int wartosc { get; set; }
            public Krawedz(int wartosc)
            {
                this.wartosc = wartosc;
            }
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int wartosc = Convert.ToInt32(AktualnaWartosc.Content);
            if (Convert.ToInt32(AktualnaWartosc.Content) < 21)
            {
                wartosc = wartosc + Convert.ToInt32(NewToken.Text);
                AktualnaWartosc.Content = wartosc;
                if(wartosc == Convert.ToInt32(Cos.Text))
                {
                    EndGame.Visibility = Visibility.Visible;
                    EndGame.Content = "REMIS";
                }
                else if(wartosc > Convert.ToInt32(Cos.Text))
                {
                    EndGame.Visibility = Visibility.Visible;
                    EndGame.Content = "Porażka";
                }
            }
            if(Convert.ToInt32(AktualnaWartosc.Content) < Convert.ToInt32(Cos.Text))
            {
                //RUCH OPONENTA
                Random random = new Random();
                Oponent.Content = Tokeny[random.Next(Tokeny.Length)];
                wartosc = wartosc + Convert.ToInt32(Oponent.Content);
                AktualnaWartosc.Content = wartosc;
                if(Convert.ToInt32(AktualnaWartosc.Content) > Convert.ToInt32(Cos.Text))
                {
                    EndGame.Visibility = Visibility.Visible;
                    EndGame.Content = "Wygrana";
                }
                else if(Convert.ToInt32(AktualnaWartosc.Content) == Convert.ToInt32(Cos.Text))
                {
                    EndGame.Visibility = Visibility.Visible;
                    EndGame.Content = "Remis";
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AktualnaWartosc.Content = 0;
            Oponent.Content = 0;
            EndGame.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            int n = Tokenylepsze.Length;
            int h = log2(n);
            int res = graf.MiniMax(0,0, (bool)Gracz.IsChecked, Tokenylepsze,h);
            MinMax.Content = res;
        }
    }
}
