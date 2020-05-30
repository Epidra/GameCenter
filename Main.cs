using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Windows.Devices.Sensors;
using Microsoft.Advertising.WinRT.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace GameCenter {
    public class Main : Game {
        GraphicsDeviceManager graphics;
        SpriteBatch           spriteBatch;

        ShopKeeper  Shopkeeper;
        FileManager Filemanager;
        JukeBox     Jukebox;

        Menu MasterMenu;
        Ghost MasterDebug;
        Ghost[] MasterGame = new Ghost[25];

        RenderTarget2D renderTarget;
        RenderTarget2D renderLandscape;
        RenderTarget2D renderPortrait;

        DisplayOrientation orientation = DisplayOrientation.Unknown;

        int active_screen = -1; //  -1 for Menu, -2 for Debug Game

        int screen_width;
        float screen_width_scale  = 0.00F;
        int screen_height;
        float screen_height_scale = 0.00F;

        bool loaded_banner = false;

        AdControl adControl_X_L;
        AdControl adControl_X_P;
        int marginLeft;
        int marginDown;

        public Main() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize() {
            Shopkeeper = new ShopKeeper(Content);
            Jukebox = new JukeBox();
            Filemanager = new FileManager(Jukebox);
            Shopkeeper.Add_FileManager(Filemanager);
            screen_width  = (int)Shopkeeper.screensize_main.X;
            screen_height = (int)Shopkeeper.screensize_main.Y;
            screen_width_scale  = ((float)Window.ClientBounds.Width / (float)screen_width);
            screen_height_scale = ((float)Window.ClientBounds.Height / (float)screen_height);

            Filemanager.Initialize(Jukebox);

            MasterMenu = new Menu("menu", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterDebug = new PipeMania("pipe mania", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[0] = new Frogger("octatravels", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[1] = new Octanom("octanom", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[2] = new Snake("snake", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[3] = new OctaCubes("octacubes", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[4] = new Sokoban("sokoban", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[5] = new OctaPOW("octapow", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[6] = new BoulderNom("bouldernom", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[7] = new OctaBattle("octabattle", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[8] = new Arkanoid("arkanoid", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[9] = new SpaceInvader("space invader", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[10] = new OctaJump("octajump", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[11] = new OctaFlight("octaflight", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[12] = new Bombuzal("bombuzal", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[13] = new Elements("elements", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[14] = new OctaPanic("octapanic", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[5] = new Tetris("tetris", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[6] = new Columns("columns", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[7] = new MeanMinos("mean minos", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[18] = new WickedMinos("wicked minos", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[19] = new MinoGarden("mino garden", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[20] = new Matchmakers("matchmakers", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[8] = new Memory("memory", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[22] = new _8Colors("8 colors", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[23] = new Swing("swing", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[9] = new Mastermind("mastermind", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[10] = new _2048("2048", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[11] = new Minesweeper("minesweeper", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[12] = new TicTacToe("tictactoe", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[13] = new RPSLS("rpsls", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[14] = new Simon("simon", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[30] = new Picross("picross", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[31] = new Halma("halma", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[32] = new Circuits("circuits", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[33] = new MysticSquare("mystic square", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[34] = new Sudoku("sudoku", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[35] = new Locomotion("locomotion", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[36] = new PipeMania("pipe mania", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[37] = new LazerLight("lazer light", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[38] = new SplitBall("split ball", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[39] = new BlackHole("black hole", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[15] = new Klondike("klondike", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[16] = new TriPeak("tripeak", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[17] = new Spider("spider", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[18] = new Pyramid("pyramid", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[19] = new FreeCell("free cell", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[20] = new BlackJack("black jack", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[21] = new Baccarat("baccarat", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[22] = new VideoPoker("video poker", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[23] = new SlotMachine("slot machine", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            MasterGame[24] = new Roulette("roulette", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[50] = new RougeEtNoir("rouge et noir", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[51] = new AceyDeucey("acey deucey", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[52] = new BolaTangkas("bola tangkas", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[53] = new Craps("craps", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[54] = new SicBo("sic bo", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[55] = new FanTan("fan-tan", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[56] = new PaiGow("pai gow", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[57] = new Keno("keno", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[58] = new TienGow("tien gow", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);
            //MasterGame[59] = new Yacht("yacht", Content, Shopkeeper, Filemanager, Jukebox, screen_width_scale, screen_height_scale);

            IsMouseVisible = true;

            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight | DisplayOrientation.Portrait | DisplayOrientation.PortraitDown;
            base.Initialize();
        }

        public void Add_Control(AdControl _adControl_L, AdControl _adControl_P, int _marginLeft, int _marginDown) {
            adControl_X_L = _adControl_L;
            adControl_X_P = _adControl_P;
            marginLeft = _marginLeft;
            marginDown = _marginDown;
        }
        public void Stop_Ads() {
            adControl_X_L.Suspend();
            adControl_X_P.Suspend();
        }

        protected override void LoadContent() {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            renderTarget = new RenderTarget2D(GraphicsDevice, 1280, 1280);
            renderLandscape = new RenderTarget2D(GraphicsDevice, 1280, 720);
            renderPortrait = new RenderTarget2D(GraphicsDevice, 720, 1280);

            Jukebox.Load_Content(Content, Filemanager);
            MasterMenu.Load_Content(GraphicsDevice, spriteBatch);
            MasterDebug.Load_Content(GraphicsDevice, spriteBatch);

            for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Load_Content(GraphicsDevice, spriteBatch);

            orientation = Window.CurrentOrientation;
            if(orientation == DisplayOrientation.LandscapeLeft) Shopkeeper.orientation = 1;
            if(orientation == DisplayOrientation.LandscapeRight) Shopkeeper.orientation = 2;
            if(orientation == DisplayOrientation.Portrait) Shopkeeper.orientation = 3;
            if(orientation == DisplayOrientation.PortraitDown) Shopkeeper.orientation = 4;
            //if(Filemanager.lizenz != null) Change_AdBanner();
            Shopkeeper.Load_Info();

            

            //swapChainPanel.Width = (float)Window.ClientBounds.Width / (float)screen_width;
            //swapChainPanel.Height = (float)Window.ClientBounds.Height / (float)screen_height;

            if(Shopkeeper.orientation <= 2) {
                MasterMenu.Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
                MasterDebug.Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
                for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
            } else {
                MasterMenu.Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
                MasterDebug.Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
                for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
            }
        }


        protected override void UnloadContent() {
            MasterMenu.Unload_Content();
            MasterDebug.Unload_Content();
            for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Unload_Content();
        }


        protected override void Update(GameTime gameTime) {
            if(!loaded_banner) {
                loaded_banner = true;
                Start_AdBanner();
                if(Filemanager.lizenz != null) Change_AdBanner();
                orientation = DisplayOrientation.Unknown;

#if(!DEBUG)
            if(Filemanager.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) {
                Change_AdBanner();
            }
#endif
            }
            if(orientation != Window.CurrentOrientation) {
                orientation = Window.CurrentOrientation;
                if(orientation == DisplayOrientation.LandscapeLeft) Shopkeeper.orientation = 1;
                if(orientation == DisplayOrientation.LandscapeRight) Shopkeeper.orientation = 2;
                if(orientation == DisplayOrientation.Portrait) Shopkeeper.orientation = 3;
                if(orientation == DisplayOrientation.PortraitDown) Shopkeeper.orientation = 4;
                if(Filemanager.lizenz != null) Change_AdBanner();
                if(Shopkeeper.orientation == 2) {
                    adControl_X_L.BorderThickness = new Thickness(0, -marginDown / 3*2, -marginLeft / 3*2, 0);
                } else {
                    adControl_X_L.BorderThickness = new Thickness(-marginLeft / 3*2, -marginDown / 3*2, 0, 0);
                }
                Shopkeeper.Load_Info();
            }

            if(Shopkeeper.orientation <= 2) {
                if(screen_width_scale != ((float)Window.ClientBounds.Width / (float)screen_width) || screen_height_scale != ((float)Window.ClientBounds.Height / (float)screen_height)) {
                    MasterMenu.Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
                    MasterDebug.Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
                    for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Resize(((float)Window.ClientBounds.Width / (float)screen_width), ((float)Window.ClientBounds.Height / (float)screen_height));
                }
            } else {
                if(screen_width_scale != ((float)Window.ClientBounds.Width / (float)screen_height) || screen_height_scale != ((float)Window.ClientBounds.Height / (float)screen_width)) {
                    MasterMenu.Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
                    MasterDebug.Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
                    for(int i = 0; i < MasterGame.Length; i++) MasterGame[i].Resize(((float)Window.ClientBounds.Width / (float)screen_height), ((float)Window.ClientBounds.Height / (float)screen_width));
                }
            }
            string temp = "";
            if(active_screen == -1) {
                temp = MasterMenu.Update(gameTime);
            } else if(active_screen == -2) {
                temp = MasterDebug.Update(gameTime);
            } else {
                temp = MasterGame[active_screen].Update(gameTime);
            }

            if(temp == "menu") {
                active_screen = -1;
            }
            if(temp == "debuggame") {
                active_screen = -2;
            }

            if(temp == "MalGC_Werbung") {
                Change_AdBanner();
            }

            if(temp == "gamestart") {
                active_screen = MasterMenu.current_row_main * 5 + MasterMenu.current_point_main;
                MasterGame[active_screen].Start(gameTime, ConvertDiff(), MasterMenu.active_levelselect);
            }
            base.Update(gameTime);
        }

        private int ConvertDiff() {
            if(MasterMenu.active_difficulty == 0) return 1;
            if(MasterMenu.active_difficulty == 1) return 2;
            if(MasterMenu.active_difficulty == 2) return 4;
            return 0;
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.SetRenderTarget(renderTarget);
            if(active_screen == -1) {
                MasterMenu.Draw();
            } else if(active_screen == -2) {
                MasterDebug.Draw();
            } else {
                MasterGame[active_screen].Draw();
            }
            GraphicsDevice.SetRenderTarget(null);

            GraphicsDevice.SetRenderTarget(Get_RenderTarget());
            spriteBatch.Begin();
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Draw(renderTarget, new Vector2(0, 0), Color.White);
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
            spriteBatch.Draw(Get_RenderTarget(), Window.ClientBounds, new Color(new Vector4(0.5f + Filemanager.system_brightness / 200, 0.5f + Filemanager.system_brightness / 200, 0.5f + Filemanager.system_brightness / 200, 100)));
            spriteBatch.End();
            Filemanager.Transite();

            base.Draw(gameTime);
        }

        private RenderTarget2D Get_RenderTarget() {
            if(Shopkeeper.orientation <= 2) {
                return renderLandscape;
            } else {
                return renderPortrait;
            }
        }

        private void Start_AdBanner() {
            adControl_X_L.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            adControl_X_P.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Change_AdBanner() {
            if(!Filemanager.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) {
                if(orientation == DisplayOrientation.LandscapeLeft)  { adControl_X_L.Visibility = Windows.UI.Xaml.Visibility.Visible; adControl_X_P.Visibility = Windows.UI.Xaml.Visibility.Collapsed; adControl_X_L.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Left; }
                if(orientation == DisplayOrientation.LandscapeRight) { adControl_X_L.Visibility = Windows.UI.Xaml.Visibility.Visible; adControl_X_P.Visibility = Windows.UI.Xaml.Visibility.Collapsed; adControl_X_L.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Right; }
                if(orientation == DisplayOrientation.Portrait)       { adControl_X_P.Visibility = Windows.UI.Xaml.Visibility.Visible; adControl_X_L.Visibility = Windows.UI.Xaml.Visibility.Collapsed; adControl_X_P.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center; }
                if(orientation == DisplayOrientation.PortraitDown)   { adControl_X_P.Visibility = Windows.UI.Xaml.Visibility.Visible; adControl_X_L.Visibility = Windows.UI.Xaml.Visibility.Collapsed; adControl_X_P.HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center; }
            } else {
                adControl_X_L.Suspend();
                adControl_X_P.Suspend();
            }
        }
    }
}