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

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardType[] cardTypes;
        private string move1;
        private string move2;
        //lista znakow, ktore beda na kartach
        private List<char> signs;

        public MainWindow()
        {
            InitializeComponent();
            NewGame();
        }

        //Tworzenie nowej gry
        private void NewGame()
        {
            //Tablica z czystymi kartami
            cardTypes = new CardType[20];

            for (int i = 0; i < cardTypes.Length; i++)
                cardTypes[i] = CardType.free;

            //Domyslny kolor kart
            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                button.Background = Brushes.Gray;
                button.Content = string.Empty;
                button.FontSize = 70;
            });
        }

        //Przypisywanie znakow do losowych kart
        private void RandomInit()
        {
            var random = new Random();
            signs = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };

            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                int current = random.Next(signs.Count);
                button.Content = signs[current];
                signs.RemoveAt(current);
                button.Background = Brushes.White;
                button.Foreground = button.Background;
            });
        }

        private void Gamebtn(object sender, RoutedEventArgs e)
        {
            RandomInit();

        }

        private void Cardbtn(object sender, RoutedEventArgs e)
        {

        }
    }
}
