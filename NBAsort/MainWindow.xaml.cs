using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
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
using System.Windows.Xps.Serialization;
using static System.Formats.Asn1.AsnWriter;

namespace NBAsort {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    #region refer
    //rosetta code.org
    //sort to find and display these 10 awards
    /*
     *Prius              - most points with smallest game time   DONE
     *Gas Guzzler        - least points with most game time      DONE
     *Foul Target        - highest freethrow% with lowest shot%  DONE
     *Overacheiver       - which players are above average       DONE
     *UnderAcheiever     - which players below average           DONE
     *On the fence       - most average overall                  DONE
     *Bang for your buck - rookie with most playtime             DONE
     *Gordon gecko       - highest scoring region (ne,nw,se,sw)  DONE
     *Charlie brown      - worst player in each category         DONE
     *Tiger Uppercut     - best in each category                 DONE
     */
    //file path
    //"C:\\Users\\MCA\\Downloads\\NBA_DATA.csv"
    //per-minute ratings math 
    #endregion

    public partial class MainWindow : Window {

        struct player {
            public string name;
            public string team;
            public bool   rookie;
            public double rating;
            public double gamesPlayed;
            public double minutesPerGame;
            public double pointsPerGame;
            public double rebounds;
            public double assistsPerGame;
            public double shotPercentage;
            public double freethrowPercentage;
            public double pointsPerMin;
            public double freesPerShot;
        }

        //global player array
        player[] playerData;

        public MainWindow() {
            InitializeComponent();
            //get size based on file
            int records = CountCsvRecords("C:\\Users\\MCA\\Downloads\\NBA_DATA.csv") - 1;
            //set under acheivers

            #region quicksort test
            //----

            //QUICKSORT TEST
            //double[] test = {5, 6, 4, 3};
            //string[] names = { "five", "six", "four", "three"};
            ////args - num array, low index, high index, str array
            //QuickSort(test, 0, test.Length - 1, names);
            //txtQuick.Visibility = "Visible";
            //txtQuick.Text = names[0] + names[1] + names[2] + names[3];

            //----- 
            #endregion

            //load player date into array
            playerData = DataToPlayers(records);    
            txtPrius.Text = Prius();
            txtGasGuzzler.Text = GasGuzzler();
            txtCharlieBrown.Text = CharlieBrown();
            txtTiger.Text = TigerUppercut();
            txtGordonGecko.Text = BestRegion();
            txtBangForBuck.Text = BangForBuck();
            txtOnTheFence.Text = OnTheFence();
            txtFoulTarget.Text = FreeThrow();
            UnderAchiever();
            OverAchiever();

        }//end main window
        private int CountCsvRecords(string file) {
            int total = 0;
            StreamReader infile = new StreamReader(file);
            while (infile.EndOfStream == false) {
                infile.ReadLine();
                total++;
            }//end while
            infile.Close(); 
            return total;
        }//end funciton

        private player[] DataToPlayers(int records) {
            //set return array to size of file
            player[] returnArray = new player[records];

            int CurrentRec = 0;
            //set file to player data csv file
            StreamReader file = new StreamReader("C:\\Users\\MCA\\Downloads\\NBA_DATA.csv");
            //skip header 
            file.ReadLine();
            
            while (file.EndOfStream == false && CurrentRec < records) {
                //set record to read in lines
                string record = file.ReadLine();
                //store lines in array split on commas
                string[] field = record.Split(",");
                //set current record values
                returnArray[CurrentRec].name                 = field[0];
                returnArray[CurrentRec].team                 = field[1];
                //rookie true or false
                returnArray[CurrentRec].rookie               = Convert.ToBoolean(int.Parse(field[2])); 
                returnArray[CurrentRec].rating               = double.Parse(field[3]);
                returnArray[CurrentRec].gamesPlayed          = double.Parse(field[4]);
                returnArray[CurrentRec].minutesPerGame       = double.Parse(field[5]);
                returnArray[CurrentRec].pointsPerGame        = double.Parse(field[6]);
                returnArray[CurrentRec].rebounds             = double.Parse(field[7]);
                returnArray[CurrentRec].assistsPerGame       = double.Parse(field[8]);
                returnArray[CurrentRec].shotPercentage       = double.Parse(field[9]);
                returnArray[CurrentRec].freethrowPercentage  = double.Parse(field[10]);
                
                //move record index up 
                CurrentRec++;
            }//end while
            file.Close();
            return returnArray;
        }//end function

        private void BubbleSort(double[] data, string[] name) {
            int maxPos = data.Length - 1;        
            bool swapped = true;

            while (swapped) {
                swapped = false;

               for (int index = 0; index < maxPos; index++) {
                    double currentVal = data[index];
                    double nextVal = data[index + 1];
                    string curName = name[index];
                    string nextName = name[index + 1];
                    //compare a pair of indexes
                    if (nextVal < currentVal) {
                        //if next value is less than current value
                        //store next value in placeholder
                        double storedVal = data[index + 1];
                        string empName = name[index + 1];

                        //swap next value and current value
                        data[index + 1] = data[index];
                        name[index + 1] = name[index];
                        //store current value into placeholder
                        data[index] = storedVal;
                        name[index] = empName;
                        //set swap to true
                        swapped = true;
                    }//end if
               }//end for
            }//end while
            //decreament max position
            maxPos--;
        }//end function

        private void BubbleSort(double[] data) {
            int maxPos = data.Length - 1;
            bool swapped = true;

            while (swapped) {
                swapped = false;

                for (int index = 0; index < maxPos; index++) {
                    double currentVal = data[index];
                    double nextVal = data[index + 1];
                    
                    //compare a pair of indexes
                    if (nextVal < currentVal) {
                        //if next value is less than current value
                        //store next value in placeholder
                        double storedVal = data[index + 1];
                     

                        //swap next value and current value
                        data[index + 1] = data[index];
                       
                        //store current value into placeholder
                        data[index] = storedVal;
                       
                        //set swap to true
                        swapped = true;
                    }//end if
                }//end for
            }//end while
            //decreament max position
            maxPos--;
        }//end function

        #region QuickSort functions

        //QUICKSORT - change copy version to run on nba data
        private void Swap(string [] str, double[] arr, int i, int j) {
            double temp = arr[i];
            arr[i] = arr[j];
            arr[j] = temp;
            string strTemp = str[i];
            str[i] = str[j];
            str[j] = strTemp;
        }//end function

        private int Partition(string[] str, double[] arr, int low, int high) {

            //funciton takes last elements as a pivot
            //places the pivot element at it's correct position
            //in sorted array and places all smaller elem to the left
            //of pivot and all greater elements to right of pivot

            //choose a pivot
            double pivot = arr[high];

            //index of smaller element and indicates
            //the right position of the pivot found so far
            int i = (low - 1);
            for (int j = low; j <= high - 1; j++) {
                //if current elem is smaller than the pivot
                if (arr[j] < pivot) {
                    //increment index of the smaller element
                    i++;
                    Swap(str, arr, i, j);
                    
                }//end if
            }//end for
            Swap(str, arr, i + 1, high);
            return i + 1;
        }//end function

        //takes in array, 1st index and last index
        private void QuickSort(double[] arr, int low, int high, string[] str) {
            //main quicksort implementation 
            //arr[] the array to be sorted !change to double array once done coding!
            //low = starting index
            //high = ending index

            if (low < high) {
                //pi is partitionning index, arr[] is now at right place

                int pi = Partition(str, arr, low, high);

                //seperately sort elements before and after partition index

                QuickSort(arr, low, pi - 1, str);
                QuickSort(arr, pi + 1, high, str);

            }//end if
        }//end function 
        #endregion

        private string Prius() {
            string[] names = new string[playerData.Length];
            double[] pointsPerMin = new double[playerData.Length];
            
            double totalMin = 0;
            double totalPoint = 0;
            string winner = "";
            
            for (int index = 0; index < playerData.Length; index++) {
                //add names on index, get totalMin

                if (playerData[index].pointsPerGame != 0) {

                    totalPoint = (playerData[index].pointsPerGame * playerData[index].gamesPlayed);
                    totalMin = (playerData[index].minutesPerGame * playerData[index].gamesPlayed);

                    names[index] = playerData[index].name;
                    pointsPerMin[index] = totalPoint / totalMin;

                    playerData[index].pointsPerMin = pointsPerMin[index];
                } //end if
               
            }//end for 

            //sort ppm array with names array, return names with highest ppm
            
            //BubbleSort(pointsPerMin, names);
            
            QuickSort(pointsPerMin, 0, pointsPerMin.Length - 1, names);

            Array.Reverse(names);
            winner = names[0];
            return "THE PRIUS AWARD: \n" + winner;
        }//end function

        private string GasGuzzler() {
            string[] names = new string[playerData.Length];
            double[] pointsPerMin = new double[playerData.Length];
            string[] test = new string[playerData.Length];
            double totalMin = 0;
            double totalPoint = 0;
            string winner = "";

            for (int index = 0; index < playerData.Length; index++) {
                //add names on index, get totalMin

                if (playerData[index].minutesPerGame != 0) {

                    totalPoint = (playerData[index].pointsPerGame * playerData[index].gamesPlayed);
                    totalMin = (playerData[index].minutesPerGame * playerData[index].gamesPlayed);

                    names[index] = playerData[index].name;
                    pointsPerMin[index] = totalPoint / totalMin;

                    playerData[index].pointsPerMin = pointsPerMin[index];
                }

                test[index] = $"{names[index]} ||| {pointsPerMin[index]}";
            }//end for 

            BubbleSort(pointsPerMin, names);
            
            winner = names[0];
            return "Gas Guzzler Award: \n" + winner;
        }//end function

        private string CharlieBrown() {

            //vars for winner

            string leastRating      = Rating(0);//PAUL GOERGE
            string leastGamesPlayed = Games(0);//JULIUS RANDALL
            string leastMpg         = Min(0);//ERIC MORELAND
            string leastPpg         = Points(0);//ANDREI KIRILENKO
            string leastRebounds    = Rebounds(0);//DAHNTEY JONES
            string leastAssists     = Assists(0);//ANDREI KIRILENKO
            string leastShots       = Shots(0);//ANDRE DAWKINS
            string leastFreeThrow   = Frees(0);//ANDREA BARGANI
            //return text for all winners
            return $"-worst humans overall-\n\nRATING: {leastRating}" +
                   $"\nGAMES PLAYED: {leastGamesPlayed}\nPLAY TIME: {leastMpg}" +
                   $"\nPOINTS: {leastPpg}\nREBOUNDS: {leastRebounds}\nASSISTS: {leastAssists}" +
                   $"\nSHOT%: {leastShots}\nFREE THROW%: {leastFreeThrow}";
        }//end function

        private string TigerUppercut() {
            //vars for winner
            string mostRating      = Rating(1);//JAMES HARDEN
            string mostGamesPlayed = Games(1);//ELFRIF PAYTON
            string mostMpg         = Min(1);//JIMMY BUTLER
            string mostPpg         = Points(1);//JAMES HARDEN
            string mostRebounds    = Rebounds(1);//DEANDRE JORDAN
            string mostAssists     = Assists(1);//JOHN WALL
            string mostShots       = Shots(1);//MITCH MCGARY
            string mostFreeThrows  = Frees(1);//ZORAN DRAGIC

            //return text for all winners
            return $"-best humans overall-\n\nRATING: {mostRating}" +
                   $"\nGAMES PLAYED: {mostGamesPlayed}\nPLAY TIME: {mostMpg}" +
                   $"\nPOINTS: {mostPpg}\nREBOUNDS: {mostRebounds}\nASSISTS: {mostAssists}" +
                   $"\nSHOT%: {mostShots}\nFREE THROW%: {mostFreeThrows}";
        }//end function

        #region Charlie/Tiger sub
        private string Rating(int i) {

            double[] rating = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                rating[index] = playerData[index].rating;
            }//end for

            BubbleSort(rating, names);
            //reverse array to set highest value, return name of winner
            Array.Reverse(names);
            //MOST
            winners[0] = names[names.Length - 1];
            //LEAST
            winners[1] = names[0];

            return winners[i];
        }//end function

        private string Games(int i) {

            double[] games = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 0;
            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                games[index] = playerData[index].gamesPlayed;
                //change zero values to skip
                if (playerData[index].gamesPlayed == 0) {
                    games[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(games, names);
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount - 1];



            return winners[i];
        }//end function

        private string Min(int i) {

            double[] minutes = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                minutes[index] = playerData[index].minutesPerGame;
                //change zero values to highest to skip
                if (playerData[index].minutesPerGame == 0) {
                    minutes[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(minutes, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function 

        private string Points(int i) {

            double[] datas = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                datas[index] = playerData[index].pointsPerGame;
                //change zero values to highest to skip
                if (playerData[index].pointsPerGame == 0) {
                    datas[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(datas, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function 

        private string Rebounds(int i) {

            double[] datas = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                datas[index] = playerData[index].rebounds;
                //change zero values to highest to skip
                if (playerData[index].rebounds == 0) {
                    datas[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(datas, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function 

        private string Assists(int i) {

            double[] datas = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                datas[index] = playerData[index].assistsPerGame;
                //change zero values to highest to skip
                if (playerData[index].assistsPerGame == 0) {
                    datas[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(datas, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function 

        private string Shots(int i) {

            double[] datas = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                datas[index] = playerData[index].shotPercentage;
                //change zero values to highest to skip
                if (playerData[index].shotPercentage == 0) {
                    datas[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(datas, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function 

        private string Frees(int i) {

            double[] datas = new double[playerData.Length];
            string[] names = new string[playerData.Length];
            string[] winners = new string[2];
            int zeroCount = 1;

            //fill arrays
            for (int index = 0; index < playerData.Length; index++) {
                names[index] = playerData[index].name;
                datas[index] = playerData[index].freethrowPercentage;
                //change zero values to highest to skip
                if (playerData[index].freethrowPercentage == 0) {
                    datas[index] = 10000000000;
                    zeroCount++;
                }
            }//end for

            BubbleSort(datas, names);
            //set lowest value, return name of winner
            winners[0] = names[0];
            winners[1] = names[names.Length - zeroCount];

            return winners[i];
        }//end function    
        #endregion
      
        private string BestRegion() {
            string winner = "";


            //ne - bos, bkn, ny, phi, tor
            //nw - min, okc, por, den, uta
            //se - cha, atl, mia, orl, was 
            //sw - hou, dal, sa, mem, no


            //central - chi, cle, det, ind, mil
            //pacific - gs, lac, lal, phx, sac

            //set vars/arrays to check regions
            string[] ne = { "BOS", "BKN", "NY", "PHI", "TOR" };
            string[] nw = { "MIN", "OKC", "POR", "DEN", "UTA" };
            string[] se = { "CHA", "ATL", "MIA", "ORL", "WAS" };
            string[] sw = { "HOU", "DAL", "SA", "MEM", "NO" };
            string[] cen = { "CHI", "CLE", "DET", "IND", "MIL" };
            string[] pac = { "GS", "LAC", "LAL", "PHX", "SAC" };
            double seTotal = 0;
            double swTotal = 0;
            double neTotal = 0;
            double nwTotal = 0;
            double cenTotal = 0;
            double pacTotal = 0;        
            string[] regions = {"SouthEast", "SouthWest", "NorthEast", "NorthWest", "Central", "Pacific"};
            

            for (int index = 0; index < playerData.Length; index++) {
                if (se.Contains(playerData[index].team)) {
                    seTotal += playerData[index].pointsPerGame;
                    
                }//end if
                if (sw.Contains(playerData[index].team)) {
                    swTotal += playerData[index].pointsPerGame;
                   
                }//end if
                if (ne.Contains(playerData[index].team)) {
                    neTotal += playerData[index].pointsPerGame;
                    
                }//end if
                if (nw.Contains(playerData[index].team)) {
                    nwTotal += playerData[index].pointsPerGame;
                    
                }//end if
                if (cen.Contains(playerData[index].team)) {
                    cenTotal += playerData[index].pointsPerGame;
                    
                }//end if
                if (pac.Contains(playerData[index].team)) {
                    pacTotal += playerData[index].pointsPerGame;
       
                }//end if

            }

            double[] scores = { seTotal, swTotal, neTotal, nwTotal, cenTotal, pacTotal };

            BubbleSort(scores, regions);
            winner = regions[scores.Length - 1];
            return "GORDON GECKO AWARD:\n" + winner;
        }//end function

        private string BangForBuck() {
            string winner    = "";
            int rookieCount  = 0;
            double[] times   = new double[playerData.Length];
            string[] rookies = new string[playerData.Length];
            //get count of rookies
            for (int index = 0; index < playerData.Length; index++) {
                if (playerData[index].rookie == true && playerData[index].minutesPerGame != 0) {
                    rookieCount++;
                    rookies[index] = playerData[index].name;
                    times[index] = playerData[index].minutesPerGame;
                }//end if
            }//end for



            BubbleSort(times, rookies);
            winner = rookies[times.Length - 1];
            return "THE BANG FOR YOUR BUCK AWARD:\n" + winner;
        }//end function

        private string OnTheFence() {
            string winner = "";
            int recordCount = 0;
            double totalPoints = 0;
            double avg = 0;
            string[] names = new string[playerData.Length];
            double[] points = new double[playerData.Length];
            
            //store names and points

            for (int index = 0;index < playerData.Length; index++) {
                recordCount++;
                totalPoints += playerData[index].pointsPerGame;
                names[index] = playerData[index].name;
                points[index] = playerData[index].pointsPerGame;

            }//end for

            //get average
            avg = Math.Round((totalPoints / recordCount), 1);


            for (int index = 0; index < playerData.Length; index++) {
                if (points[index] == avg) {
                    
                    winner = names[index];

                }//end if
            }//end for
            
            BubbleSort(points, names);
            return "ON THE FENCE AWARD:\n\n" + winner;
        }//end function

        private void UnderAchiever() {
            string[] unders = new string[playerData.Length];
            int diffCount = 0;
            int recordCount = 0;
            double totalPoints = 0;
            double avg = 0;
            string[] names = new string[playerData.Length];
            double[] points = new double[playerData.Length];

            //store names and points

            for (int index = 0; index < playerData.Length; index++) {
                recordCount++;
                totalPoints += playerData[index].pointsPerGame;
                names[index] = playerData[index].name;
                points[index] = playerData[index].pointsPerGame;

            }//end for

            //get average
            avg = Math.Round((totalPoints / recordCount), 1);

            for (int index = 0; index < playerData.Length; index++) {
                if (points[index] < avg) {
                    unders[index] = playerData[index].name;
                } else if (points[index] > avg) {
                    diffCount++;
                }//end if
            }//end funciton
            BubbleSort(points, unders);
            txtUnderAcheive.Text = "Under Achievers:\n\n";

            //print to textbox
            for (int i = 0; i < unders.Length - diffCount; i++) {
                txtUnderAcheive.Text += $"{unders[i]}\n";
            }//end for

        }//end function

        private void OverAchiever(){
            string[] overs = new string[playerData.Length];
            int diffCount = 0;
            int recordCount = 0;
            double totalPoints = 0;
            double avg = 0;
            string[] names = new string[playerData.Length];
            double[] points = new double[playerData.Length];

            //store names and points

            for (int index = 0; index < playerData.Length; index++) {
                recordCount++;
                totalPoints += playerData[index].pointsPerGame;
                names[index] = playerData[index].name;
                points[index] = playerData[index].pointsPerGame;

            }//end for

            //get average
            avg = Math.Round((totalPoints / recordCount), 1);

            for (int index = 0; index < playerData.Length; index++) {
                if (points[index] < avg) {
                    diffCount++;
                } else if (points[index] > avg) {
                    overs[index] = playerData[index].name;
                }//end if
            }//end funciton
            BubbleSort(points, overs);
            Array.Reverse(overs);
            txtOverAcheive.Text = "Over Achievers:\n\n";

            //print to textbox
            for (int i = 0; i < overs.Length - diffCount; i++) {
                txtOverAcheive.Text += $"{overs[i]}\n";
            }//end for

        }//end function

        private string FreeThrow() {
            //vars/arrays
            string winner = "";
            string[] names = new string[playerData.Length];
            double[] frees = new double[playerData.Length];
            double fr = 0;
            double shot = 0;
            
            for (int index = 0; index < playerData.Length; index++) {

                if (playerData[index].freethrowPercentage != 0 && playerData[index].shotPercentage != 0) {
                    fr = playerData[index].freethrowPercentage;
                    shot = playerData[index].shotPercentage;
                    //divide free throw% by shot%, highest value = winner
                    playerData[index].freesPerShot =  fr / shot;
                    frees[index] = playerData[index].freesPerShot;
                    names[index] = playerData[index].name;
                }//end if
            }//end for

            //sort and return highest value as winner
            BubbleSort(frees, names);
            winner = names[frees.Length - 1];
            return "THE FOUL TARGET AWARD:\n\n" + winner;

        }//end function

        //click events to show current award
        #region click events

        private void muiPrius_Click(object sender, RoutedEventArgs e) {

            txtPrius.Visibility = Visibility.Visible;
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiGas_Click(object sender, RoutedEventArgs e) {
            txtPrius.Visibility = Visibility.Hidden;
            txtGasGuzzler.Visibility = Visibility.Visible;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiFoul_Click(object sender, RoutedEventArgs e) {
            txtPrius.Visibility = Visibility.Hidden;
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Visible;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiOver_Click(object sender, RoutedEventArgs e) {
            txtPrius.Visibility = Visibility.Hidden;
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Visible;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiUnder_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Visible;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiFence_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Visible;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiBang_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Visible;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiGordon_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Visible;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiCharlie_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Visible;
            txtTiger.Visibility = Visibility.Hidden;
        }

        private void muiTiger_Click(object sender, RoutedEventArgs e) {
            txtGasGuzzler.Visibility = Visibility.Hidden;
            txtFoulTarget.Visibility = Visibility.Hidden;
            txtPrius.Visibility = Visibility.Hidden;
            txtOverAcheive.Visibility = Visibility.Hidden;
            txtUnderAcheive.Visibility = Visibility.Hidden;
            txtOnTheFence.Visibility = Visibility.Hidden;
            txtBangForBuck.Visibility = Visibility.Hidden;
            txtGordonGecko.Visibility = Visibility.Hidden;
            txtCharlieBrown.Visibility = Visibility.Hidden;
            txtTiger.Visibility = Visibility.Visible;

        }
        #endregion


    }//end main 

}
