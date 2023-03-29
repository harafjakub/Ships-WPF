using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Ships{
    public partial class MainWindow : Window{
        public static int turn = 1;
        Player1 window1 = new Player1();
        Player2 window2 = new Player2();
        public static List<Button> chosenButtons = new List<Button>();
        public static List<String> player1ships = new List<String>();
        public static List<String> player2ships = new List<String>();
        public static List<String> player1shipsE = new List<String>();
        public static List<String> player2shipsE = new List<String>();
        public static bool automaticClose = false;
        int howManyShips = 20;
        int player = 1;
        int lastfield1;
        int lastfield2;
        int[] occupiedFields = new int[144];
        string whyNotFieldMessage;

        public MainWindow(){
            InitializeComponent();
            int[] array1 = new int[100];
            int[] array2 = new int[100];
            for(int i=0; i<100; i++){
                array1[i] = 0;
                array2[i] = 0;
            }
            Game game = new Game(array1, array2);
            this.DataContext = game;
            window1.DataContext = game;
            window2.DataContext = game;
            CreateButtons();
            ExtendArray();
        }

        private void Button_Click(object sender, RoutedEventArgs e){
            Button btn = (Button)sender;
            if (CheckIfShipFits(btn)){
                occupiedFields[ChangeButtonTagToCustomInt(btn)] = 1;
                if (howManyShips > 1){
                    howManyShips--;
                    btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040"); // ship color
                    btn.Content = ".";
                    chosenButtons.Add(btn);
                }
                else{
                    btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#404040"); // ship color
                    btn.Content = ".";
                    chosenButtons.Add(btn);
                    if (player == 1){
                        PlayerLabel.Content = "PLAYER 2 - LOCATE YOUR SHIPS";
                        MessageBox.Show("Now Player 2 need to locate his ships");
                        ExtendArray();
                        howManyShips = 20;
                        WhichShip.Content = "SINGLE-MASTED SHIP";
                        NumOfSingleShip.Content = "5";
                        NumOfTwoShip.Content = "3";
                        NumOfThreeShip.Content = "2";
                        NumOfFourShip.Content = "1";
                        player = 2;
                        foreach (Button b1 in chosenButtons){
                            player1ships.Add(Convert.ToString(b1.Tag));
                            b1.Content = "";
                            b1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#004c99"); // sea color
                        }
                        chosenButtons.Clear();
                    }
                    else{
                        foreach (Button b1 in chosenButtons){
                            player2ships.Add(Convert.ToString(b1.Tag));
                            b1.Content = "";
                            b1.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#004c99"); // sea color
                        }
                        MessageBox.Show("Get ready!");
                        CreateAdditionalLists();
                        automaticClose = true;
                        this.Close();
                        window1.Show();
                        window2.Show();
                    }
                }
                CheckShips();
            }
            else{
                MessageBox.Show(whyNotFieldMessage);
            }
        }

        bool CheckIfShipFits(Button btn){
            int temp = ChangeButtonTagToCustomInt(btn);;
            if (occupiedFields[temp] == 0){
                // for ONE-MASTED SHIP
                if (howManyShips > 16){
                    return true;
                }
                // for TWO-MASTED SHIP
                else if (howManyShips == 16 || howManyShips == 14 || howManyShips == 12){
                    if (occupiedFields[temp + 1] == 0 || occupiedFields[temp - 1] == 0 || occupiedFields[temp + 12] == 0 || occupiedFields[temp - 12] == 0){
                        lastfield1 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "Not enough space to place the ship";
                }
                else if (howManyShips == 15 || howManyShips == 13 || howManyShips == 11){
                    if (temp == lastfield1 + 1 || temp == lastfield1 - 1 || temp == lastfield1 + 12 || temp == lastfield1 - 12){
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field";
                }
                // for THREE-MASTED SHIP
                else if (howManyShips == 10 || howManyShips == 7){
                    if ((occupiedFields[temp + 1] == 0 && occupiedFields[temp + 2] == 0) || (occupiedFields[temp - 1] == 0 && occupiedFields[temp - 2] == 0) || (occupiedFields[temp + 12] == 0 && occupiedFields[temp + 24] == 0) || (occupiedFields[temp - 12] == 0 && occupiedFields[temp - 24] == 0)){
                        lastfield1 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "Not enough space to place the ship";
                }
                else if (howManyShips == 9 || howManyShips == 6){
                    if ((temp == lastfield1 + 1 && occupiedFields[temp + 1] == 0) || (temp == lastfield1 - 1 && occupiedFields[temp - 1] == 0) || (temp == lastfield1 + 12 && occupiedFields[temp + 12] == 0) || (temp == lastfield1 - 12 && occupiedFields[temp - 12] == 0)){
                        lastfield2 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field with 2 free fields in the same direction";
                }
                else if (howManyShips == 8 || howManyShips == 5){
                    int fieldDiff = lastfield2 - lastfield1;
                    if (temp == lastfield2 + fieldDiff){
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field with good direction";
                }
                // for FOUR-MASTED SHIP
                else if (howManyShips == 4){
                    if ((occupiedFields[temp + 1] == 0 && occupiedFields[temp + 2] == 0 && occupiedFields[temp + 3] == 0) || (occupiedFields[temp - 1] == 0 && occupiedFields[temp - 2] == 0 && occupiedFields[temp - 3] == 0) || (occupiedFields[temp + 12] == 0 && occupiedFields[temp + 24] == 0 && occupiedFields[temp + 36] == 0) || (occupiedFields[temp - 12] == 0 && occupiedFields[temp - 24] == 0 && occupiedFields[temp - 36] == 0)){
                        lastfield1 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "Not enough space to place the ship";
                }
                else if (howManyShips == 3){
                    if ((temp == lastfield1 + 1 && occupiedFields[temp + 1] == 0 && occupiedFields[temp + 2] == 0) || (temp == lastfield1 - 1 && occupiedFields[temp - 1] == 0 && occupiedFields[temp - 2] == 0) || (temp == lastfield1 + 12 && occupiedFields[temp + 12] == 0 && occupiedFields[temp + 24] == 0) || (temp == lastfield1 - 12 && occupiedFields[temp - 12] == 0 && occupiedFields[temp - 24] == 0)){
                        lastfield2 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field with 3 free fields in the same direction";
                }
                else if (howManyShips == 2){
                    int fieldDiff = lastfield2 - lastfield1;
                    if (temp == lastfield2 + fieldDiff){
                        lastfield1 = lastfield2;
                        lastfield2 = temp;
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field with 2 free fields in the same direction";
                }
                else if (howManyShips == 1){
                    int fieldDiff = lastfield2 - lastfield1;
                    if (temp == lastfield2 + fieldDiff){
                        return true;
                    }
                    whyNotFieldMessage = "You must select an adjacent field with good direction";
                }
                return false;
            }
            else{
                whyNotFieldMessage = "This field is taken, please select a different one";
                return false;
            }
        }

        void CheckShips(){
            if (howManyShips > 16){
                NumOfSingleShip.Content = Convert.ToString(Convert.ToInt32(NumOfSingleShip.Content) - 1);
            }
            else if (howManyShips == 16){
                NumOfSingleShip.Content = Convert.ToString(Convert.ToInt32(NumOfSingleShip.Content) - 1);
                WhichShip.Content = "TWO-MASTED SHIP";
            }
            else if (howManyShips == 14 || howManyShips == 12){
                NumOfTwoShip.Content = Convert.ToString(Convert.ToInt32(NumOfTwoShip.Content) - 1);
            }
            else if (howManyShips == 10){
                NumOfTwoShip.Content = Convert.ToString(Convert.ToInt32(NumOfTwoShip.Content) - 1);
                WhichShip.Content = "THREE-MASTED SHIP";
            }
            else if (howManyShips == 7){
                NumOfThreeShip.Content = Convert.ToString(Convert.ToInt32(NumOfThreeShip.Content) - 1);
            }
            else if (howManyShips == 4){
                NumOfThreeShip.Content = Convert.ToString(Convert.ToInt32(NumOfThreeShip.Content) - 1);
                WhichShip.Content = "FOUR-MASTED SHIP";
            }
        }

        void ExtendArray(){
            for (int i = 0; i <= 143; i++){
                occupiedFields[i] = 0;
            }
            for (int i = 0; i <= 11; i++){
                occupiedFields[i] = 1;
                occupiedFields[i + 132] = 1;
                occupiedFields[i * 12] = 1;
                occupiedFields[(i * 12) + 11] = 1;
            }
        }
        int ChangeButtonTagToCustomInt(Button button){
            int j = 0;
            int temp = 13;
            for (int i = 0; i < 100; i++){
                if (j == 10){
                    temp += 2;
                    j = 0;
                }
                if (Convert.ToInt32(Convert.ToString(button.Tag).Substring(1)) == i){
                    return (i + temp);
                }
                j++;
            }
            return 0;
        }

        void CreateAdditionalLists(){
            foreach (String buttonTag in player1ships){
                player1shipsE.Add("E" + buttonTag.Substring(1));
            }
            foreach (String buttonTag in player2ships){
                player2shipsE.Add("E" + buttonTag.Substring(1));
            }
        }

        private void CreateButtons(){
            for (int i = 2; i < 12; i++){
                for (int j = 1; j < 11; j++){
                    Button button = new Button();{
                        Grid.SetRow(button, i);
                        Grid.SetColumn(button, j);
                    };
                    button.Tag = ("P" + Convert.ToString(j - 1) + Convert.ToString(i - 2));
                    button.Style = (Style)FindResource("PlayerButton");
                    button.Click += new RoutedEventHandler(Button_Click);
                    this.gridMain.Children.Add(button);
                }
            }
        }
        public class YesNoToBooleanConverter : IValueConverter{
            public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
                switch (value){
                    case 0:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#004c99"); // sea color
                    case 1:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#990000"); // hit color
                    case 2:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#003366"); // miss color
                    case 3:
                        return (SolidColorBrush)new BrushConverter().ConvertFrom("#404040"); // ship color
                }
                return false;
            }

            public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture){
                if (value is Colors){
                }
                return 0;
            }
        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e){
            if (automaticClose == false){
                MainWindow.automaticClose = true;
                var response = MessageBox.Show("Do you really want to exit?", "Exiting...", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (response == MessageBoxResult.No){
                    e.Cancel = true;
                }
                else{
                    Application.Current.Shutdown();
                }
            }
            else{
                MainWindow.automaticClose = false;
            }
        }
    }
}
