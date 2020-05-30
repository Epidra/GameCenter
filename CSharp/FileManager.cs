using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Store;
using Windows.Storage;
using Windows.ApplicationModel;

namespace GameCenter {
    public class FileManager {

        string FILE_ID = "SAVEFILECHARLIE";

        public LicenseInformation lizenz;

        string filename_HighScore = "UserProfile";

        static int max_store = 36;
        static int max_games = 25;

        public    int[] purchased = new    int[max_store];
        public    int[] highscore = new    int[max_games]; // Highest Score in Game
        public    int[] hightimeI = new    int[max_games]; // Total Time spent in Game - Int Format
        public string[] hightimeS = new string[max_games]; // Total Time spent in Game - String Format
        public    int[] highcoin  = new    int[max_games]; // Highest Amount of Coins Won
        public    int[] hightotal = new    int[max_games]; // Total Amount of Coins Won
        public    int[] highplay  = new    int[max_games]; // Amount of Times played
        public    int[] type      = new    int[max_store];
        public    int[] price     = new    int[max_store];
        public  float[] coincalc  = new  float[max_games];
        public string[] name      = new string[max_store];

        public   bool[] minos       = new bool[12];
        public   bool[] mino_active = new bool[12];
        public    int[] mino_used   = new  int[12];

        public   bool[] BGround_active = new   bool[5];
        public string[] BGround_name   = new string[5];

        public   bool[] music_active = new   bool[5];
        public string[] music_title  = new string[5];

        public   int system_volume_music = 10;
        public   int system_volume_sound = 100;
        public  bool system_active_music = true;
        public  bool system_active_sound = true;
        public float system_brightness   = 100;
        public  bool system_alignededge  = false;
        public int highlight_button = 0;
        public float highlight_timer = 0.00f;

        public bool active_firststart = false;

        public int current_background = 0;

        public int octaLives = 0;

        public string stats_totalplaytimeS = "";
        public    int stats_totalplaytimeI = 0;
        public    int stats_timesstarted   = 0;
        public    int stats_coinscollected = 0;
        public    int stats_totalminosused = 0;

        public int[] top_highscore = new int[3];
        public int[] top_hightime  = new int[3];
        public int[] top_highcoin  = new int[3];
        public int[] top_hightotal = new int[3];
        public int[] top_highplay  = new int[3];
        public int[] top_highmino  = new int[3];

        public    int rolling_horizontal;
        public    int rolling_vertical;
        public    int coins = 0;
        public    int coins_bonus = 0;
        public    int button_scale = 300;
        public   bool active_transition = false;
        public   bool active_info = false;
        public  float transition;
        private   int value = 2;
        public   bool speed;

        public FileManager(JukeBox JK) {
            Contact_Store(0);
            Fill_Fields();
            //Initialize(JK);
        }

        public async void Contact_Store(int i) {
            if(i == 0) { // Initialize
#if DEBUG
                StorageFolder proxyDataFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                StorageFile proxyFile = await proxyDataFolder.GetFileAsync("test-purchase.xml");
                await CurrentAppSimulator.ReloadSimulatorAsync(proxyFile);
                lizenz = CurrentAppSimulator.LicenseInformation;
#else
            lizenz = CurrentApp.LicenseInformation;
#endif
            }
            if(i == 1) { // Purchase Ad-free licence
                try {
#if DEBUG
                    PurchaseResults pr = await CurrentAppSimulator.RequestProductPurchaseAsync("MalGC_Werbung");
                    lizenz = CurrentAppSimulator.LicenseInformation;
#else
                    await CurrentApp.RequestProductPurchaseAsync("MalGC_Werbung");
                    lizenz = CurrentApp.LicenseInformation;
#endif
                } catch(Exception) {

                }
            }
        }

        public void Set_Hightlight(int index) {
            highlight_button = index;
            highlight_timer = 1.50f;
        }

        public void Special_Purchase(int id) {
            if(id == Convert("music pack 1")) {
                music_active[2] = true;
                music_active[3] = true;
                music_active[4] = true;
                //music_active[5] = true;
            }
            if(id == Convert("music pack 2")) {
                //music_active[6] = true;
                //music_active[7] = true;
                //music_active[8] = true;
                //music_active[9] = true;
            }
            if(id == Convert("music pack 3")) {
                //music_active[10] = true;
                //music_active[11] = true;
                //music_active[12] = true;
                //music_active[13] = true;
            }
            if(id == Convert("clouds bg")) {
                BGround_active[2] = true;
            }
            if(id == Convert("mountains bg")) {
                BGround_active[3] = true;
            }
            if(id == Convert("industry bg")) {
                BGround_active[4] = true;
            }

            if(id == Convert("fire mino"))    minos[ 1] = true;
            if(id == Convert("air mino"))     minos[ 2] = true;
            if(id == Convert("thunder mino")) minos[ 3] = true;
            if(id == Convert("water mino"))   minos[ 4] = true;
            if(id == Convert("ice mino"))     minos[ 5] = true;
            if(id == Convert("earth mino"))   minos[ 6] = true;
            if(id == Convert("metal mino"))   minos[ 7] = true;
            if(id == Convert("nature mino"))  minos[ 8] = true;
            if(id == Convert("light mino"))   minos[ 9] = true;
            if(id == Convert("dark mino"))    minos[10] = true;
            if(id == Convert("gold mino"))    minos[11] = true;

            Adjust_Prices();
        }

        // Types
        // 0 - OctaGames
        // 1 - Mino Games
        // 2 - Standard Games
        // 3 - Card Games
        // 4 - Casino Games
        // 5 - AddOn / Expansion
        // 6 - OctaLives
        // 7 - Minos
        // 8 - Music Packs & Backgrounds & Statistics / Other

        private void Fill_Fields() {

            for(int i = 0; i < 3; i++) {
                top_highscore[i] = 0;
                top_hightime [i] = 0;
                top_highcoin [i] = 0;
                top_hightotal[i] = 0;
                top_highplay [i] = 0;
            }

            for(int i = 0; i < highscore.Length; i++) {
                highscore[i] = 0;
                highcoin [i] = 0;
                hightotal[i] = 0;
                hightimeI[i] = 0;
                highplay [i] = 0;
            }
            int z = 0;
            purchased[z] = 0; type[z] = 0; coincalc[z] = 1    ; name[z++] = "octatravels";   // OctaTravels
            purchased[z] = 0; type[z] = 0; coincalc[z] = 0.01f; name[z++] = "octanom";       // Octanom
            purchased[z] = 0; type[z] = 0; coincalc[z] = 0.01f; name[z++] = "snake";         // Snake
            purchased[z] = 0; type[z] = 0; coincalc[z] = 1    ; name[z++] = "octacubes";     // OctaCubes
            purchased[z] = 0; type[z] = 0; coincalc[z] = 1    ; name[z++] = "sokoban";       // Sokoban
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "octapow";       // OctaPOW
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "bouldernom";    // BoulderNom
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "octabattle";    // OctaBattle
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "arkanoid";      // Arkanoid
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "space invader"; // Space Invader
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "octajump";      // OctaJump
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "octaflight";    // OctaFlight
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "bombuzal";      // Bombuzal
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "elements";      // Elements
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "octapanic";     // OctaPanic
            purchased[z] = 0; type[z] = 1; coincalc[z] = 0.01f; name[z++] = "tetris";        // Tetris
            purchased[z] = 0; type[z] = 1; coincalc[z] = 0.01f; name[z++] = "columns";       // Columns
            purchased[z] = 0; type[z] = 1; coincalc[z] = 0.01f; name[z++] = "mean minos";    // Mean Minos
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "wicked minos";  // Wicked Minos
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "mino garden";   // Mino Garden
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "matchmakers";   // Matchmakers
            purchased[z] = 0; type[z] = 1; coincalc[z] = 1    ; name[z++] = "memory";        // Memory
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "8 colors";      // 8 Colors
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "swing";         // Swing
            purchased[z] = 0; type[z] = 1; coincalc[z] = 1    ; name[z++] = "mastermind";    // Mastermind
            purchased[z] = 0; type[z] = 2; coincalc[z] = 0.01f; name[z++] = "2048";          // 2048
            purchased[z] = 0; type[z] = 2; coincalc[z] = 1    ; name[z++] = "minesweeper";   // Minesweeper
            purchased[z] = 0; type[z] = 2; coincalc[z] = 3    ; name[z++] = "tictactoe";     // Tic Tac Toe
            purchased[z] = 0; type[z] = 2; coincalc[z] = 5    ; name[z++] = "rpsls";         // RPSLS
            purchased[z] = 0; type[z] = 2; coincalc[z] = 1    ; name[z++] = "simon";         // Simon
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "picross";       // Picross
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "halma";         // Halma
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "circuits";      // Circuits
          //purchased[z] = 0; type[z] = 2; coincalc[z] = 100  ; name[z++] = "mystic square"; // Mystic Square
          //purchased[z] = 0; type[z] = 2; coincalc[z] = 100  ; name[z++] = "sudoku";        // Sudoku
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "locomotion";    // Locomotion
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "pipe mania";    // Pipe Mania
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "lazer light";   // Lazer Light
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "split ball";    // Split Ball
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 7.77f; name[z++] = "black hole";    // Black Hole
            purchased[z] = 0; type[z] = 3; coincalc[z] = 1    ; name[z++] = "klondike";      // Spider
            purchased[z] = 0; type[z] = 3; coincalc[z] = 0.01f; name[z++] = "tripeak";       // TriPeak
            purchased[z] = 0; type[z] = 3; coincalc[z] = 1    ; name[z++] = "spider";        // Klondike
            purchased[z] = 0; type[z] = 3; coincalc[z] = 0.01f; name[z++] = "pyramid";       // Pyramid
            purchased[z] = 0; type[z] = 3; coincalc[z] = 1    ; name[z++] = "free cell";     // Free Cell
            purchased[z] = 0; type[z] = 4; coincalc[z] = 1    ; name[z++] = "black jack";    // Black Jack
            purchased[z] = 0; type[z] = 4; coincalc[z] = 1    ; name[z++] = "baccarat";      // Baccarat
            purchased[z] = 0; type[z] = 4; coincalc[z] = 1    ; name[z++] = "video poker";   // Video Poker
            purchased[z] = 0; type[z] = 4; coincalc[z] = 1    ; name[z++] = "slot machine";  // Slot Machine
            purchased[z] = 0; type[z] = 4; coincalc[z] = 1    ; name[z++] = "roulette";      // Roulette
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "rouge et noir"; // Rouge et Noir
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "acey deucey";   // Acey Deucey
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "bola tangkas";  // Bola Tangkas
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "craps";         // Craps
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "sic bo";        // Sic Bo
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "fan-tan";       // Fan-Tan
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "pai gow";       // Pai Gow
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "keno";          // Keno
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "tien gow";      // Tien Gow
          //purchased[z] = 0; type[z] = 9; coincalc[z] = 1    ; name[z++] = "yacht";         // Yacht
          //purchased[z] = 0; type[z] = 5;                      name[z++] = "level select";  // Level Select
            purchased[z] = 0; type[z] = 5;                      name[z++] = "easy mode";     // Difficulty: Easy
            purchased[z] = 0; type[z] = 5;                      name[z++] = "hard mode";     // Difficulty: Hard
            purchased[z] = 0; type[z] = 5;                      name[z++] = "hold";          // Hold Button
            purchased[z] = 0; type[z] = 5;                      name[z++] = "highroller";    // High Roller
            purchased[z] = 0; type[z] = 6;                      name[z++] = "octalive";      // OctaLive
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "fire mino";     // Mino: Fire
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "air mino";      // Mino: Air
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "thunder mino";  // Mino: Thunder
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "water mino";    // Mino: Water
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "ice mino";      // Mino: Ice
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "earth mino";    // Mino: Earth
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "metal mino";    // Mino: Metal
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "nature mino";   // Mino: Nature
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "light mino";    // Mino: Light
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "dark mino";     // Mino: Dark
          //purchased[z] = 0; type[z] = 7;                      name[z++] = "gold mino";     // Mino: Gold
            purchased[z] = 0; type[z] = 8;                      name[z++] = "clouds bg";     // Background: Clouds
            purchased[z] = 0; type[z] = 8;                      name[z++] = "mountains bg";  // Background: Mountains
          //purchased[z] = 0; type[z] = 8;                      name[z++] = "industry bg";   // Background: Industy
            purchased[z] = 0; type[z] = 8;                      name[z++] = "music pack 1";  // Music Pack 1
          //purchased[z] = 0; type[z] = 8;                      name[z++] = "music pack 2";  // Music Pack 2
          //purchased[z] = 0; type[z] = 8;                      name[z++] = "music pack 3";  // Music Pack 3
            purchased[z] = 0; type[z] = 8;                      name[z++] = "highscore";     // Statistics: Highscore
            purchased[z] = 0; type[z] = 8;                      name[z++] = "coins earned";  // Statistics: Most earned Coins
            purchased[z] = 0; type[z] = 8;                      name[z++] = "playtime";      // Statistics: Playtime
          //purchased[z] = 0; type[z] = 8;                      name[z++] = "minos used";    // Statistics: Minos used

            minos[ 0] = true; mino_active[ 0] = false; mino_used[ 0] = 0;
            minos[ 1] = true; mino_active[ 1] = false; mino_used[ 1] = 0;
            minos[ 2] = true; mino_active[ 2] = false; mino_used[ 2] = 0;
            minos[ 3] = true; mino_active[ 3] = false; mino_used[ 3] = 0;
            minos[ 4] = true; mino_active[ 4] = false; mino_used[ 4] = 0;
            minos[ 5] = true; mino_active[ 5] = false; mino_used[ 5] = 0;
            minos[ 6] = true; mino_active[ 6] = false; mino_used[ 6] = 0;
            minos[ 7] = true; mino_active[ 7] = false; mino_used[ 7] = 0;
            minos[ 8] = true; mino_active[ 8] = false; mino_used[ 8] = 0;
            minos[ 9] = true; mino_active[ 9] = false; mino_used[ 9] = 0;
            minos[10] = true; mino_active[10] = false; mino_used[10] = 0;
            minos[11] = true; mino_active[11] = false; mino_used[11] = 0;

            BGround_active[0] = true;  BGround_name[0] = "Space Grid";
            BGround_active[1] = true;  BGround_name[1] = "Countryside";
            BGround_active[2] = false; BGround_name[2] = "Clouds";
            BGround_active[2] = false; BGround_name[3] = "Mountains";
            BGround_active[2] = false; BGround_name[4] = "Industry";

            music_active[ 0] = true;  music_title[ 0] = "technoGEEK";
            music_active[ 1] = true;  music_title[ 1] = "MegaWall";
            music_active[ 2] = false; music_title[ 2] = "DST TowerDef1";
            music_active[ 3] = false; music_title[ 3] = "DST TowerDef2";
            music_active[ 4] = false; music_title[ 4] = "DST TowerDef3";
            //music_active[ 5] = false; music_title[ 5] = "Lunar Harvest";
            //music_active[ 6] = false; music_title[ 6] = "Pack 2 Track 1";
            //music_active[ 7] = false; music_title[ 7] = "Pack 2 Track 2";
            //music_active[ 8] = false; music_title[ 8] = "Pack 2 Track 3";
            //music_active[ 9] = false; music_title[ 9] = "Pack 2 Track 4";
            //music_active[10] = false; music_title[10] = "Pack 3 Track 1";
            //music_active[11] = false; music_title[11] = "Pack 3 Track 2";
            //music_active[12] = false; music_title[12] = "Pack 3 Track 3";
            //music_active[13] = false; music_title[13] = "Pack 3 Track 4";
        }

        public int Convert(string id) {
            for(int i = 0; i < name.Length; i++) {
                if(name[i] == id) return i;
            }
            return 0;
        }

        public void Transite() {
            int x = 1;
            if(speed) x = 2;
            if(active_transition) {
                if(active_info) {
                    transition = transition - value * x;
                    if(transition <= 0) {
                        active_transition = false;
                        active_info = false;
                        speed = false;
                    }
                } else {
                    transition = transition + value * x;
                    if(transition >= 100) {
                        active_transition = false;
                        active_info = true;
                        speed = false;
                    }
                }
            }
        }

        StorageFile file_score;

        private void Load_System(string s) {
            string[] loader = s.Split(':');
            if(loader.Length > 0) system_volume_music = int.Parse(loader[0]);
            if(loader.Length > 1) system_active_music = IntToBool(int.Parse(loader[1]));
            if(loader.Length > 2) system_volume_sound = int.Parse(loader[2]);
            if(loader.Length > 3) system_active_sound = IntToBool(int.Parse(loader[3]));
            if(loader.Length > 4) button_scale        = int.Parse(loader[4]);
            if(loader.Length > 5) system_brightness   = int.Parse(loader[5]);
            if(loader.Length > 6) system_alignededge  = IntToBool(int.Parse(loader[6]));
            if(loader.Length > 7) current_background  = int.Parse(loader[7]);
        }

        private void Load_Stats(string s) {
            string[] loader = s.Split(':');
            if(loader.Length > 0) stats_timesstarted = int.Parse(loader[0]);
        }

        private void Load_AddOn(string s) {
            string[] loader = s.Split(':');
            if(loader.Length >  0) purchased[Convert("level select")] = int.Parse(loader[ 0]);
            if(loader.Length >  1) purchased[Convert("easy mode")]    = int.Parse(loader[ 1]);
            if(loader.Length >  2) purchased[Convert("hard mode")]    = int.Parse(loader[ 2]);
            if(loader.Length >  3) purchased[Convert("hold")]         = int.Parse(loader[ 3]);
            if(loader.Length >  4) purchased[Convert("highroller")]   = int.Parse(loader[ 4]);
            if(loader.Length >  5) purchased[Convert("octalive")]     = int.Parse(loader[ 5]);
            if(loader.Length >  6) purchased[Convert("fire mino")]    = int.Parse(loader[ 6]);
            if(loader.Length >  7) purchased[Convert("air mino")]     = int.Parse(loader[ 7]);
            if(loader.Length >  8) purchased[Convert("thunder mino")] = int.Parse(loader[ 8]);
            if(loader.Length >  9) purchased[Convert("water mino")]   = int.Parse(loader[ 9]);
            if(loader.Length > 10) purchased[Convert("ice mino")]     = int.Parse(loader[10]);
            if(loader.Length > 11) purchased[Convert("earth mino")]   = int.Parse(loader[11]);
            if(loader.Length > 12) purchased[Convert("metal mino")]   = int.Parse(loader[12]);
            if(loader.Length > 13) purchased[Convert("nature mino")]  = int.Parse(loader[13]);
            if(loader.Length > 14) purchased[Convert("light mino")]   = int.Parse(loader[14]);
            if(loader.Length > 15) purchased[Convert("dark mino")]    = int.Parse(loader[15]);
            if(loader.Length > 16) purchased[Convert("gold mino")]    = int.Parse(loader[16]);
            if(loader.Length > 17) purchased[Convert("clouds bg")]    = int.Parse(loader[17]);
            if(loader.Length > 18) purchased[Convert("mountains bg")] = int.Parse(loader[18]);
            if(loader.Length > 19) purchased[Convert("industry bg")]  = int.Parse(loader[19]);
            if(loader.Length > 20) purchased[Convert("music pack 1")] = int.Parse(loader[20]);
            if(loader.Length > 21) purchased[Convert("music pack 2")] = int.Parse(loader[21]);
            if(loader.Length > 22) purchased[Convert("music pack 3")] = int.Parse(loader[22]);
            if(loader.Length > 23) purchased[Convert("highscore")]    = int.Parse(loader[23]);
            if(loader.Length > 24) purchased[Convert("coins earned")] = int.Parse(loader[24]);
            if(loader.Length > 25) purchased[Convert("playtime")]     = int.Parse(loader[25]);
            if(loader.Length > 26) purchased[Convert("minos used")]   = int.Parse(loader[26]);
        }

        private void Load_Game(string s, int id) {
            string[] loader = s.Split(':');
            if(loader.Length > 0) purchased[id] = int.Parse(loader[0]);
            if(loader.Length > 1) highscore[id] = int.Parse(loader[1]);
            if(loader.Length > 2) highcoin [id] = int.Parse(loader[2]);
            if(loader.Length > 3) hightotal[id] = int.Parse(loader[3]);
            if(loader.Length > 4) hightimeI[id] = int.Parse(loader[4]);
            if(loader.Length > 5) highplay [id] = int.Parse(loader[5]);
        }

        private string Save_System() {
            string s = "";
            s = s.Insert(s.Length, ""  + system_volume_music);
            s = s.Insert(s.Length, ":" + BoolToInt(system_active_music));
            s = s.Insert(s.Length, ":" + system_volume_sound);
            s = s.Insert(s.Length, ":" + BoolToInt(system_active_sound));
            s = s.Insert(s.Length, ":" + button_scale);
            s = s.Insert(s.Length, ":" + system_brightness);
            s = s.Insert(s.Length, ":" + BoolToInt(system_alignededge));
            s = s.Insert(s.Length, ":" + current_background);
            return s;
        }

        private string Save_Stats() {
            string s = "";
            s = s.Insert(s.Length, ""  + stats_timesstarted);
            s = s.Insert(s.Length, ":" + mino_used[0]);
            s = s.Insert(s.Length, ":" + mino_used[1]);
            s = s.Insert(s.Length, ":" + mino_used[2]);
            s = s.Insert(s.Length, ":" + mino_used[3]);
            s = s.Insert(s.Length, ":" + mino_used[4]);
            s = s.Insert(s.Length, ":" + mino_used[5]);
            s = s.Insert(s.Length, ":" + mino_used[6]);
            s = s.Insert(s.Length, ":" + mino_used[7]);
            s = s.Insert(s.Length, ":" + mino_used[8]);
            s = s.Insert(s.Length, ":" + mino_used[9]);
            s = s.Insert(s.Length, ":" + mino_used[10]);
            s = s.Insert(s.Length, ":" + mino_used[11]);
            return s;
        }

        private string Save_AddOn() {
            string s = "";
            s = s.Insert(s.Length, ""  + purchased[Convert("level select")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("easy mode")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("hard mode")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("hold")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("highroller")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("octalive")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("fire mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("air mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("thunder mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("water mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("ice mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("earth mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("metal mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("nature mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("light mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("dark mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("gold mino")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("clouds bg")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("mountains bg")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("industry bg")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("music pack 1")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("music pack 2")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("music pack 3")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("highscore")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("coins earned")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("playtime")]);
            s = s.Insert(s.Length, ":" + purchased[Convert("minos used")]);
            return s;
        }

        private string Save_Game(int id) {
            string s = "";
            s = s.Insert(s.Length, ""  + purchased[id]);
            s = s.Insert(s.Length, ":" + highscore[id]);
            s = s.Insert(s.Length, ":" + highcoin[id]);
            s = s.Insert(s.Length, ":" + hightotal[id]);
            s = s.Insert(s.Length, ":" + hightimeI[id]);
            s = s.Insert(s.Length, ":" + highplay[id]);
            return s;
        }

        public async void Initialize(JukeBox JK) {
            string[] temp = new string[65]; // Number of all items in savefile
            file_score = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename_HighScore, CreationCollisionOption.OpenIfExists);
            IList<string> list = await FileIO.ReadLinesAsync(file_score);
            if(list.Count == 0 || list[0] != FILE_ID) {
                active_firststart = true;
                active_transition = true;
                file_score = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename_HighScore, CreationCollisionOption.ReplaceExisting);
#pragma warning disable IDE0028 // Initialisierung der Sammlung vereinfachen
                List<string> list_temp = new List<string>();
#pragma warning restore IDE0028 // Initialisierung der Sammlung vereinfachen

                list_temp.Add(FILE_ID);
                list_temp.Add("0");
                list_temp.CopyTo(temp, 0);
                list = list_temp;
                await FileIO.WriteLinesAsync(file_score, list_temp);
            } else {
                list.CopyTo(temp, 0);
            }

            // ID 0 for Save File ID
            if(list.Count >  1) coins = int.Parse(temp[1]);
            if(list.Count >  2) Load_System(temp[2]);
            if(list.Count >  3) Load_Stats(temp[3]);
            if(list.Count >  4) Load_AddOn(temp[4]);
            if(list.Count >  5) Load_Game(temp[ 5], Convert("octatravels"));
            if(list.Count >  6) Load_Game(temp[ 6], Convert("octanom"));
            if(list.Count >  7) Load_Game(temp[ 7], Convert("snake"));
            if(list.Count >  8) Load_Game(temp[ 8], Convert("octacubes"));
            if(list.Count >  9) Load_Game(temp[ 9], Convert("sokoban"));
            if(list.Count > 10) Load_Game(temp[10], Convert("tetris"));
            if(list.Count > 11) Load_Game(temp[11], Convert("tictactoe"));
            if(list.Count > 12) Load_Game(temp[12], Convert("rpsls"));
            if(list.Count > 13) Load_Game(temp[13], Convert("mastermind"));
            if(list.Count > 14) Load_Game(temp[14], Convert("simon"));
            if(list.Count > 15) Load_Game(temp[15], Convert("memory"));
            if(list.Count > 16) Load_Game(temp[16], Convert("minesweeper"));
            if(list.Count > 17) Load_Game(temp[17], Convert("2048"));
            if(list.Count > 18) Load_Game(temp[18], Convert("mystic square"));
            if(list.Count > 19) Load_Game(temp[19], Convert("sudoku"));
            if(list.Count > 20) Load_Game(temp[20], Convert("black jack"));
            if(list.Count > 21) Load_Game(temp[21], Convert("baccarat"));
            if(list.Count > 22) Load_Game(temp[22], Convert("video poker"));
            if(list.Count > 23) Load_Game(temp[23], Convert("slot machine"));
            if(list.Count > 24) Load_Game(temp[24], Convert("roulette"));
            if(list.Count > 25) Load_Game(temp[25], Convert("octapow"));
            if(list.Count > 26) Load_Game(temp[26], Convert("bouldernom"));
            if(list.Count > 27) Load_Game(temp[27], Convert("octabattle"));
            if(list.Count > 28) Load_Game(temp[28], Convert("arkanoid"));
            if(list.Count > 29) Load_Game(temp[29], Convert("space invader"));
            if(list.Count > 30) Load_Game(temp[30], Convert("octajump"));
            if(list.Count > 31) Load_Game(temp[31], Convert("octaflight"));
            if(list.Count > 32) Load_Game(temp[32], Convert("bombuzal"));
            if(list.Count > 33) Load_Game(temp[33], Convert("elements"));
            if(list.Count > 34) Load_Game(temp[34], Convert("octapanic"));
            if(list.Count > 35) Load_Game(temp[35], Convert("columns"));
            if(list.Count > 36) Load_Game(temp[36], Convert("mean minos"));
            if(list.Count > 37) Load_Game(temp[37], Convert("wicked minos"));
            if(list.Count > 38) Load_Game(temp[38], Convert("mino garden"));
            if(list.Count > 39) Load_Game(temp[39], Convert("matchmakers"));
            if(list.Count > 40) Load_Game(temp[40], Convert("8 colors"));
            if(list.Count > 41) Load_Game(temp[41], Convert("swing"));
            if(list.Count > 42) Load_Game(temp[42], Convert("picross"));
            if(list.Count > 43) Load_Game(temp[43], Convert("halma"));
            if(list.Count > 44) Load_Game(temp[44], Convert("circuits"));
            if(list.Count > 45) Load_Game(temp[45], Convert("locomotion"));
            if(list.Count > 46) Load_Game(temp[46], Convert("pipe mania"));
            if(list.Count > 47) Load_Game(temp[47], Convert("lazer light"));
            if(list.Count > 48) Load_Game(temp[48], Convert("split ball"));
            if(list.Count > 49) Load_Game(temp[49], Convert("black hole"));
            if(list.Count > 50) Load_Game(temp[50], Convert("spider"));
            if(list.Count > 51) Load_Game(temp[51], Convert("pyramid"));
            if(list.Count > 52) Load_Game(temp[52], Convert("klondike"));
            if(list.Count > 53) Load_Game(temp[53], Convert("tripeak"));
            if(list.Count > 54) Load_Game(temp[54], Convert("free cell"));
            if(list.Count > 55) Load_Game(temp[55], Convert("rouge et noir"));
            if(list.Count > 56) Load_Game(temp[56], Convert("acey deucey"));
            if(list.Count > 57) Load_Game(temp[57], Convert("bola tangkas"));
            if(list.Count > 58) Load_Game(temp[58], Convert("craps"));
            if(list.Count > 59) Load_Game(temp[59], Convert("sic bo"));
            if(list.Count > 60) Load_Game(temp[60], Convert("fan-tan"));
            if(list.Count > 61) Load_Game(temp[61], Convert("pai gow"));
            if(list.Count > 62) Load_Game(temp[62], Convert("keno"));
            if(list.Count > 63) Load_Game(temp[63], Convert("tien gow"));
            if(list.Count > 64) Load_Game(temp[64], Convert("yacht"));

            Adjust_Prices();

            JK.Apply_Volume();

            if(purchased[Convert("music pack 1")] > 0) {
                music_active[2] = true;
                music_active[3] = true;
                music_active[4] = true;
                //music_active[5] = true;
            }

            if(purchased[Convert("music pack 2")] > 0) {
                //music_active[2] = true;
                //music_active[3] = true;
                //music_active[4] = true;
                //music_active[5] = true;
            }

            if(purchased[Convert("music pack 3")] > 0) {
                //music_active[2] = true;
                //music_active[3] = true;
                //music_active[4] = true;
                //music_active[5] = true;
            }

            if(purchased[Convert("clouds bg")] > 0) {
                BGround_active[2] = true;
            }

            if(purchased[Convert("mountains bg")] > 0) {
                BGround_active[3] = true;
            }

            if(purchased[Convert("industry bg")] > 0) {
                BGround_active[4] = true;
            }

            if(0 < purchased[Convert("fire mino")])    minos[ 1] = true;
            if(0 < purchased[Convert("air mino")])     minos[ 2] = true;
            if(0 < purchased[Convert("thunder mino")]) minos[ 3] = true;
            if(0 < purchased[Convert("water mino")])   minos[ 4] = true;
            if(0 < purchased[Convert("ice mino")])     minos[ 5] = true;
            if(0 < purchased[Convert("earth mino")])   minos[ 6] = true;
            if(0 < purchased[Convert("metal mino")])   minos[ 7] = true;
            if(0 < purchased[Convert("nature mino")])  minos[ 8] = true;
            if(0 < purchased[Convert("light mino")])   minos[ 9] = true;
            if(0 < purchased[Convert("dark mino")])    minos[10] = true;
            if(0 < purchased[Convert("gold mino")])    minos[11] = true;

            if(system_active_music) {
                JK.Select_Track(2);
                JK.Resume();
            }

            Podium();

            stats_timesstarted++;
        }

        private int BoolToInt(bool b) { if(b) { return 1; } else { return 0; } }
        private bool IntToBool(int i) { if(i == 1) { return true; } else { return false; } }

        public async void Save() {
            file_score = await ApplicationData.Current.LocalFolder.CreateFileAsync(filename_HighScore, CreationCollisionOption.ReplaceExisting);
#pragma warning disable IDE0028 // Initialisierung der Sammlung vereinfachen
            List<string> list = new List<string>();
#pragma warning restore IDE0028 // Initialisierung der Sammlung vereinfachen

            list.Add(FILE_ID);
            list.Add("" + coins);
            list.Add("" + Save_System());
            list.Add("" + Save_Stats());
            list.Add("" + Save_AddOn());
            list.Add("" + Save_Game(Convert("octatravels")));
            list.Add("" + Save_Game(Convert("octanom")));
            list.Add("" + Save_Game(Convert("snake")));
            list.Add("" + Save_Game(Convert("octacubes")));
            list.Add("" + Save_Game(Convert("sokoban")));
            list.Add("" + Save_Game(Convert("tetris")));
            list.Add("" + Save_Game(Convert("tictactoe")));
            list.Add("" + Save_Game(Convert("rpsls")));
            list.Add("" + Save_Game(Convert("mastermind")));
            list.Add("" + Save_Game(Convert("simon")));
            list.Add("" + Save_Game(Convert("memory")));
            list.Add("" + Save_Game(Convert("minesweeper")));
            list.Add("" + Save_Game(Convert("2048")));
            list.Add("" + Save_Game(Convert("mystic square")));
            list.Add("" + Save_Game(Convert("sudoku")));
            list.Add("" + Save_Game(Convert("black jack")));
            list.Add("" + Save_Game(Convert("baccarat")));
            list.Add("" + Save_Game(Convert("video poker")));
            list.Add("" + Save_Game(Convert("slot machine")));
            list.Add("" + Save_Game(Convert("roulette")));
            list.Add("" + Save_Game(Convert("octapow")));
            list.Add("" + Save_Game(Convert("bouldernom")));
            list.Add("" + Save_Game(Convert("octabattle")));
            list.Add("" + Save_Game(Convert("arkanoid")));
            list.Add("" + Save_Game(Convert("space invader")));
            list.Add("" + Save_Game(Convert("octajump")));
            list.Add("" + Save_Game(Convert("octaflight")));
            list.Add("" + Save_Game(Convert("bombuzal")));
            list.Add("" + Save_Game(Convert("elements")));
            list.Add("" + Save_Game(Convert("octapanic")));
            list.Add("" + Save_Game(Convert("columns")));
            list.Add("" + Save_Game(Convert("mean minos")));
            list.Add("" + Save_Game(Convert("wicked minos")));
            list.Add("" + Save_Game(Convert("mino garden")));
            list.Add("" + Save_Game(Convert("matchmakers")));
            list.Add("" + Save_Game(Convert("8 colors")));
            list.Add("" + Save_Game(Convert("swing")));
            list.Add("" + Save_Game(Convert("picross")));
            list.Add("" + Save_Game(Convert("halma")));
            list.Add("" + Save_Game(Convert("circuits")));
            list.Add("" + Save_Game(Convert("locomotion")));
            list.Add("" + Save_Game(Convert("pipe mania")));
            list.Add("" + Save_Game(Convert("lazer light")));
            list.Add("" + Save_Game(Convert("split ball")));
            list.Add("" + Save_Game(Convert("black hole")));
            list.Add("" + Save_Game(Convert("spider")));
            list.Add("" + Save_Game(Convert("pyramid")));
            list.Add("" + Save_Game(Convert("klondike")));
            list.Add("" + Save_Game(Convert("tripeak")));
            list.Add("" + Save_Game(Convert("free cell")));
            list.Add("" + Save_Game(Convert("rouge et noir")));
            list.Add("" + Save_Game(Convert("acey deucey")));
            list.Add("" + Save_Game(Convert("bola tangkas")));
            list.Add("" + Save_Game(Convert("craps")));
            list.Add("" + Save_Game(Convert("sic bo")));
            list.Add("" + Save_Game(Convert("fan-tan")));
            list.Add("" + Save_Game(Convert("pai gow")));
            list.Add("" + Save_Game(Convert("keno")));
            list.Add("" + Save_Game(Convert("tien gow")));
            list.Add("" + Save_Game(Convert("yacht")));

#pragma warning disable CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist
            FileIO.WriteLinesAsync(file_score, list);
#pragma warning restore CS4014 // Da dieser Aufruf nicht abgewartet wird, wird die Ausführung der aktuellen Methode fortgesetzt, bevor der Aufruf abgeschlossen ist

            Podium();
        }

        private void Adjust_Prices() {
            int[] count = new int[10];
            int[] start = new int[10];
            int[] plus  = new int[10];

            count[0] = 0; start[0] =    0; plus[0] = 100; // 0 - OctaGames
            count[1] = 0; start[1] =  200; plus[1] =  80; // 1 - Mino Games
            count[2] = 0; start[2] =  400; plus[2] =  60; // 2 - Standard Games
            count[3] = 0; start[3] =  600; plus[3] =  40; // 3 - Card Games
            count[4] = 0; start[4] =  800; plus[4] =  20; // 4 - Casino Games
            count[5] = 0; start[5] =  500; plus[5] =  50; // 5 - Expansions
            count[6] = 0; start[6] =  150; plus[6] = 150; // 6 - OctaLives
            count[7] = 0; start[7] =   75; plus[7] =  25; // 7 - Minos
            count[8] = 0; start[8] =  250; plus[8] = 125; // 8 - Other
            count[9] = 0; start[9] = 9999; plus[9] =   0; // 9 - Error

            for(int i = 0; i < type.Length; i++) {
                count[type[i]] += purchased[i];
            }

            octaLives = count[6] + 1;

            for(int i = 0; i < type.Length; i++) {
                price[i] = start[type[i]] + plus[type[i]] * count[type[i]];
            }
        }

        private void Podium() {

            stats_coinscollected = 0;
            stats_totalplaytimeI = 0;
            stats_totalminosused = 0;

            for(int i = 0; i < 3; i++) {
                top_highscore[i] = i;
                top_hightime [i] = i;
                top_highcoin [i] = i;
                top_hightotal[i] = i;
                top_highplay [i] = i;
                top_highmino [i] = i + 1;
            }

            for(int i = 0; i < highscore.Length; i++) {

                stats_coinscollected += hightotal[i];
                stats_totalplaytimeI += hightimeI[i];

                if(highscore[i] > highscore[top_highscore[0]]) {
                    top_highscore[2] = top_highscore[1];
                    top_highscore[1] = top_highscore[0];
                    top_highscore[0] = i;
                } else if(highscore[i] > highscore[top_highscore[1]]) {
                    top_highscore[2] = top_highscore[1];
                    top_highscore[1] = i;
                } else if(highscore[i] > highscore[top_highscore[2]]) {
                    top_highscore[2] = i;
                }

                if(hightimeI[i] > hightimeI[top_hightime[0]]) {
                    top_hightime[2] = top_hightime[1];
                    top_hightime[1] = top_hightime[0];
                    top_hightime[0] = i;
                } else if(hightimeI[i] > hightimeI[top_hightime[1]]) {
                    top_hightime[2] = top_hightime[1];
                    top_hightime[1] = i;
                } else if(hightimeI[i] > hightimeI[top_hightime[2]]) {
                    top_hightime[2] = i;
                }

                if(highcoin[i] > highcoin[top_highcoin[0]]) {
                    top_highcoin[2] = top_highcoin[1];
                    top_highcoin[1] = top_highcoin[0];
                    top_highcoin[0] = i;
                } else if(highcoin[i] > highcoin[top_highcoin[1]]) {
                    top_highcoin[2] = top_highcoin[1];
                    top_highcoin[1] = i;
                } else if(highcoin[i] > highcoin[top_highcoin[2]]) {
                    top_highcoin[2] = i;
                }

                if(hightotal[i] > hightotal[top_hightotal[0]]) {
                    top_hightotal[2] = top_hightotal[1];
                    top_hightotal[1] = top_hightotal[0];
                    top_hightotal[0] = i;
                } else if(hightotal[i] > hightotal[top_hightotal[1]]) {
                    top_hightotal[2] = top_hightotal[1];
                    top_hightotal[1] = i;
                } else if(hightotal[i] > hightotal[top_hightotal[2]]) {
                    top_hightotal[2] = i;
                }

                if(highplay[i] > highplay[top_highscore[0]]) {
                    top_highplay[2] = top_highplay[1];
                    top_highplay[1] = top_highplay[0];
                    top_highplay[0] = i;
                } else if(highplay[i] > highplay[top_highplay[1]]) {
                    top_highplay[2] = top_highplay[1];
                    top_highplay[1] = i;
                } else if(highplay[i] > highplay[top_highplay[2]]) {
                    top_highplay[2] = i;
                }

                int timeSec =  hightimeI[i];
                int timeMin = 0;
                int timeHour = 0;
                while(timeSec > 60) {
                    timeSec -= 60;
                    timeMin++;
                }
                while(timeMin > 60) {
                    timeMin -= 60;
                    timeHour++;
                }

                hightimeS[i] = "" + timeHour + ":" + timeMin + ":" + timeSec;

            }

            for(int i = 0; i < 12; i++) {
                stats_totalminosused += mino_used[i];

                if(mino_used[i] > mino_used[top_highmino[0]]) {
                    top_highmino[2] = top_highmino[1];
                    top_highmino[1] = top_highmino[0];
                    top_highscore[0] = i;
                } else if(mino_used[i] > mino_used[top_highmino[1]]) {
                    top_highmino[2] = top_highmino[1];
                    top_highmino[1] = i;
                } else if(mino_used[i] > mino_used[top_highmino[2]]) {
                    top_highmino[2] = i;
                }
            }

            int totaltimeSec =  stats_totalplaytimeI;
            int totaltimeMin = 0;
            int totaltimeHour = 0;
            while(totaltimeSec > 60) {
                totaltimeSec -= 60;
                totaltimeMin++;
            }
            while(totaltimeMin > 60) {
                totaltimeMin -= 60;
                totaltimeHour++;
            }

            stats_totalplaytimeS = "" + totaltimeHour + ":" + totaltimeMin + ":" + totaltimeSec;
        }

    }
}