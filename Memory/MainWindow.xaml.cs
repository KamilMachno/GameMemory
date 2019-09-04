using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Memory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardType[] cardTypes;
        private bool move1 = true;

        //Aktualny czas na stoperze
        string currentTime = string.Empty;

        //lista znakow, ktore beda na kartach
        private List<char> signs;

        /// <summary>
        /// Start aplikacji
        /// </summary>
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

        /// <summary>
        /// Przypisywanie znakow do losowych kart
        /// </summary>
        private void RandomInit()
        {
            var random = new Random();
            signs = new List<char> { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j' };
            cardTypes = new CardType[20];
            move1 = true;
            currentTime = string.Empty;

            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                int current = random.Next(signs.Count);
                button.Content = signs[current];
                signs.RemoveAt(current);
                button.Background = Brushes.White;
                button.Foreground = Brushes.White;
                button.IsEnabled = true;
            });
        }

        /// <summary>
        /// Odsiwezanie planszy z zachowaniem odkrytych par
        /// </summary>
        private void RefreshPlay()
        {
           
            Board_grid.Children.Cast<Button>().ToList().ForEach(button =>
            {
                var column = Grid.GetColumn(button);
                var row = Grid.GetRow(button);

                if (cardTypes[column + row * 5] == CardType.click)
                {
                    cardTypes[column + row * 5] = CardType.signed;           
                    button.Background = Brushes.White;
                    button.Foreground = Brushes.White;
                }
            });


        }

        /// <summary>
        /// Sprawdzanie czy zostala odkryta para
        /// </summary>
        private void CheckHit()
        {
            //lista z kliknietymi buttonami (dlugosc listy 2 elementy)
            List<Button> hittedbtn = new List<Button>(2);
            //lista z indeksami kliknietcyh buttonow w CardTypes (dlugosc listy 2 elementy)
            List<int> cards = new List<int>(2);

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

            //Przy pierwszym ruchu w nowej grze hittedbtn.Count = 0
            //Przy przypadku dwukrotnego klikniecia w taka sama karte hittedbtn.Count = 1
            if (hittedbtn.Count == 2)
            {
                if ((hittedbtn[0].Content.ToString() == hittedbtn[1].Content.ToString()) && (cards[0] != cards[1]))
                {
                    //zmiana typu karty na trafiona
                    cardTypes[cards[0]] = CardType.hit;
                    cardTypes[cards[1]] = CardType.hit;
                    //zablokowanie klikania kart
                    hittedbtn[0].IsEnabled = false;
                    hittedbtn[1].IsEnabled = false;
                    
                }
                
                    

            }
           

        }

        private void GameStatus()
        {
            //ilosc trafionych elementow
            int hittedCount = 2; //zalozenie, ze na poczatku trafiono pare, aby licznik prawidlowo funkcjionowal

            for (int i = 0; i < cardTypes.Length; i++)
            {
                if(cardTypes[i]==CardType.hit)
                    hittedCount++;
            }
            
            //Dialog YES/ NO po zakonczonej grze  
            if (hittedCount == cardTypes.Length)
            {
                string box_msg = "Koniec gry! Twój wynik: "+stopWatch()+". Rozpocząć nową grę?";

                string box_title = "Koniec gry";

                var selectedOption = MessageBox.Show(box_msg, box_title, MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (selectedOption == MessageBoxResult.Yes)
                {
                    //Rozpoczecie gry od nowa
                    NewGame();
                }
                    
                else
                    //Zamkniecie dialogu wraz z apliakcja
                    this.Close();
            }
        }

        /// <summary>
        /// Obsluga stopera 
        /// </summary>
        DispatcherTimer dispatcherTimer;
        Stopwatch stopWt;
   

        public void Timer()
        {
            dispatcherTimer = new DispatcherTimer();
            stopWt = new Stopwatch();
            dispatcherTimer.Tick += new EventHandler(countingTime);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
         
            startWatch();
        }

        void countingTime(object sender, EventArgs e)
        {
            if (stopWt.IsRunning)
            {
                TimeSpan ts = stopWt.Elapsed;
                currentTime = String.Format("{0:00}:{1:00}:{2:00}",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);

               TimerLbl.Content = currentTime;
            }
        }

        //Rozpoczecie liczenie
        public void startWatch()
        {
            stopWt.Start();
            dispatcherTimer.Start();
        }

        //Zatrzymanie liczenia
        public string stopWatch()
        {
            if (stopWt.IsRunning)
            {
                stopWt.Stop();
                return currentTime;
            }
            else
                return "Error";
           

        }


        /// <summary>
        /// Przypisanie znakow do kart / nowa gra
        /// </summary>
        private void Gamebtn(object sender, RoutedEventArgs e)
        {
            RandomInit();
            Timer();
        }

        private void Cardbtn(object sender, RoutedEventArgs e)
        {
            

            //Sprawdzanie trafienia i odsiwezanie planszy co 2 ruchy
            //Przy pierwszym ruchu w nowej grze warunek spełniony
            if (move1)
            {
                
                CheckHit();
                RefreshPlay();
         
            }
            //Co drugi ruch sprawdzane czy jest wygrana
            else
            {
                GameStatus();               
            }
            move1 ^= true;

           

            //Klikniety button
            Button bt1 = (Button)sender;

            //Wspolzedne kliknietego button
            var column = Grid.GetColumn(bt1);

            var row = Grid.GetRow(bt1);

            cardTypes[column + row * 5] = CardType.click;

            bt1.Background = Brushes.Gray;

           

        }
    }
}
