﻿using System;
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
        private bool move1 = true;

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
            cardTypes = new CardType[20];

            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                int current = random.Next(signs.Count);
                button.Content = signs[current];
                signs.RemoveAt(current);
                button.Background = Brushes.White;
                button.Foreground = Brushes.Red;
                button.IsEnabled = true;
            });
        }
        /// <summary>
        /// Resetowanie planszy
        /// </summary>
        private void RefreshPlay()
        {
            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);

                if (cardTypes[column + row * 5] == CardType.click)
                {
                    cardTypes[column + row * 5] = CardType.free;
                    button.Background = Brushes.White;
                    button.Foreground = button.Background;
                }
            });
            
        }

        /// <summary>
        /// Sprawdzanie czy zostala odkryta para takich samych znakow
        /// </summary>
        private void CheckHit()
        {
            //lista z kliknietymi buttonami (dlugosc listy 2 elementy)
            List<Button> hittedbtn = new List<Button>();
            //lista z indeksami kliknietcyh buttonow w CardTypes (dlugosc listy 2 elementy)
            List<int> cards = new List<int>();

            //Wyszukiwanie kliknietych buttonow
            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);

                if (cardTypes[column + row * 5] == CardType.click)
                {
                    hittedbtn.Add(button);
                    cards.Add(column + row * 5);
                }
            });

            //Sprawdzanie czy wyswietlone karty sa takie same 
            if(hittedbtn.Count == 2)
            {            
                if (hittedbtn[0].Content.ToString() == hittedbtn[1].Content.ToString() && cards[0]!=cards[1])
                {                
                    cardTypes[cards[0]] = CardType.hit;
                    cardTypes[cards[1]] = CardType.hit;
                    hittedbtn[0].IsEnabled = false;
                    hittedbtn[1].IsEnabled = false;
                }

                
            }
            //Odswiezenie widoku planszy
           
            

        }

        private string GameStatus()
        {
            //ilosc trafionych elementow
            int hittedCount = 0;

            for (int i = 0; i < cardTypes.Length; i++)
            {
                if(cardTypes[i]==CardType.hit)
                    hittedCount++;
            }
            Console.WriteLine(hittedCount);
            Console.WriteLine(cardTypes.Length);
            if (hittedCount == cardTypes.Length)
                return "Koniec gry!";
            else
                return "0";
             
        }
        

        private void Gamebtn(object sender, RoutedEventArgs e)
        {
            RandomInit();

        }

        private void Cardbtn(object sender, RoutedEventArgs e)
        {
            //Wyswietlenia tylko 2 elementow 
            if (move1)
            {
                CheckHit();
                RefreshPlay();
            }
            GameStatus();



            move1 ^= true;

            Button bt1 = (Button)sender;

            //Wspolzedne kliknietego button
            var column = Grid.GetColumn(bt1);
            var row = Grid.GetRow(bt1);

            cardTypes[column + row * 5] = CardType.click;

            bt1.Background = Brushes.Gray;
            

        }
    }
}
