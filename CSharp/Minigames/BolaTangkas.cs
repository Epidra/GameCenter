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

namespace GameCenter {
    class BolaTangkas : Ghost {

        int[,] grid      = new  int[4,4];
        bool[,] grid_move = new bool[4,4];

        bool placing;
        bool active_timer;
        int timer;
        string direction;
        bool won;

        public BolaTangkas(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
            id = _id;
            CM = _content;
            SK = _Shopkeeper;
            FM = _Filemanager;
            JK = _Jukebox;
            screensize_width = (int)SK.screensize_main.X;
            screensize_height = (int)SK.screensize_main.Y;
            screensize_width_scale = screenX;
            screensize_height_scale = screenY;
            random = new Random();
        }

        public override void Start2() {
        }

        public override string Update2() {
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            return "void";
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_2048, SK.Position_DisplayEdge() + SK.Position_2048(), Color.DarkGray);
        }

    }
}
