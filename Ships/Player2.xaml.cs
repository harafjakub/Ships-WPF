using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static Ships.MainWindow;

namespace Ships{
    public partial class Player2 : Window{
        public Player2(){
            InitializeComponent();
            CreateButtons();
            Loaded += MyWindow_Loaded;
        }
        
        private void MyWindow_Loaded(object sender, RoutedEventArgs e){
            foreach (var button in gridMain.Children.OfType<Button>().ToList()){
                if (MainWindow.player2ships.Contains(Convert.ToString(button.Tag)))
                    ((Game)gridMain.DataContext).FieldP2[Convert.ToInt32(Convert.ToString(button.Tag).Substring(1))] = 3;
                if (MainWindow.player1shipsE.Contains(Convert.ToString(button.Tag)))
                    button.Content = ".";
            }
        }
        int numOfShips = 20;
        List<String> hitBoxes = new List<String>();

        private void Button_Click(object sender, RoutedEventArgs e){
            if (MainWindow.turn == 2){
                Button btn = (Button)sender;
                SolidColorBrush ChosenColor;
                if (btn.Content != null){
                    ((Game)gridMain.DataContext).FieldP1[Convert.ToInt32(Convert.ToString(btn.Tag).Substring(1))] = 1;
                    ChosenColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#990000"); // hit color
                }
                else{
                    ((Game)gridMain.DataContext).FieldP1[Convert.ToInt32(Convert.ToString(btn.Tag).Substring(1))] = 2;
                    ChosenColor = (SolidColorBrush)new BrushConverter().ConvertFrom("#003366"); // miss color
                }
                btn.Background = ChosenColor;
                if (!hitBoxes.Contains(Convert.ToString(btn.Tag)) && btn.Content != null){
                    numOfShips--;
                    hitBoxes.Add(Convert.ToString(btn.Tag));
                }
                if (numOfShips == 0){
                    MainWindow.automaticClose = true;
                    MessageBox.Show("Player 2 Wins!");
                    Restart();
                }
                MainWindow.turn = 1;
                TurnLabel.Content = "PLAYER 1 TURN";
                ((Game)this.DataContext).Player1content = "YOUR TURN";
            }
        }
        private void CreateButtons(){
            // PLAYER BUTTONS 
            YesNoToBooleanConverter converter = new YesNoToBooleanConverter();
            for (int i = 2; i < 12; i++){
                for (int j = 1; j < 11; j++){
                    Button button = new Button();{
                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);
                    };
                    button.Tag = ("P" + Convert.ToString(j - 1) + Convert.ToString(i - 2));
                    button.Style = (Style)FindResource("PlayerButton");
                    Binding binding = new Binding();
                    binding.Converter = converter;
                    binding.Mode = BindingMode.TwoWay;
                    binding.Path = new PropertyPath("FieldP2[" + Convert.ToString(j - 1) + Convert.ToString(i - 2) + "]");
                    button.SetBinding(Button.BackgroundProperty, binding);
                    button.Click += new RoutedEventHandler(Button_Click);
                    this.gridMain.Children.Add(button);
                }
            }
            // ENEMY BUTTONS
            for (int i = 2; i < 12; i++){
                for (int j = 13; j < 23; j++){
                    Button button = new Button();{
                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);
                    };
                    button.Tag = ("E" + Convert.ToString(j - 13) + Convert.ToString(i - 2));
                    button.Style = (Style)FindResource("EnemyButton");
                    button.Click += new RoutedEventHandler(Button_Click);
                    this.gridMain.Children.Add(button);
                }
            }
        }
        void Restart(){
            var response = MessageBox.Show("Do want to restart?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
            if (response == MessageBoxResult.No){ 
                Application.Current.Shutdown();
            }
            else{
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e){
            if (MainWindow.automaticClose == false){
                MainWindow.automaticClose = true;
                var response = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (response == MessageBoxResult.No){
                    e.Cancel = true;
                }
                else{
                    Application.Current.Shutdown();
                }
            }
        }
    }
}
