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
    class Mastermind : Ghost {

        string[] bolt = new string[6];
        string[] slot = new string[6];
        string[] lamp = new string[6];

        Vector2 selector = new Vector2(0,0);

        bool won = false;

        public Mastermind(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            slot[0] = "fire";
            slot[1] = "fire";
            slot[2] = "fire";
            slot[3] = "fire";
            slot[4] = "fire";
            slot[5] = "fire";
            lamp[0] = "empty";
            lamp[1] = "empty";
            lamp[2] = "empty";
            lamp[3] = "empty";
            lamp[4] = "empty";
            lamp[5] = "empty";
            bolt[0] = "empty";
            bolt[1] = "empty";
            bolt[2] = "empty";
            bolt[3] = "empty";
            bolt[4] = "empty";
            bolt[5] = "empty";
            Command_Lock();
            won = false;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { if(!active_gameover) Command_Unlock(); pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) { if(!active_gameover) Command_Click(); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(selector.Y == 1) selector.Y--; pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(selector.Y == 0) selector.Y++; pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(selector.X > 0) selector.X--; pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(selector.X < 5) selector.X++; pressed_response = true; }

            if(pressed_event_touch) {
                for(int i = 0; i < 6; i++) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Mastermind_Lamps().X + 71 * i, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Mastermind_Lamps().Y + 350, 71, 71))) { selector = new Vector2(i, 0); Command_Click(); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Mastermind_Lamps().X + 71 * i, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Mastermind_Lamps().Y + 470, 71, 71))) { selector = new Vector2(i, 1); Command_Click(); }
                }
            }

            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(won && !active_gameover) {
                score_points = 50 - score_lives;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        public override void Draw2() {
            //spriteBatch.Draw(SK.texture_background_mastermind1, SK.Position_DisplayEdge() + SK.Position_Mastermind1(), Color.White);

            for(int i = 0; i < 6; i++) {
                int y = 0;
                if(lamp[i] == "green") y = 3;
                if(lamp[i] == "yellow") y = 2;
                if(lamp[i] == "red") y = 1;
                spriteBatch.Draw(SK.texture_spritesheet_lamps, SK.Position_DisplayEdge() + new Vector2(SK.Position_Mastermind_Lamps().X + 71 * i, SK.Position_Mastermind_Lamps().Y), new Rectangle(0, 64 * y, 64, 64), Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

                spriteBatch.Draw(SK.texture_casino_bet_up,         SK.Position_DisplayEdge() + new Vector2(SK.Position_Mastermind_Lamps().X + 71 * i - 45, SK.Position_Mastermind_Lamps().Y + 350), null, selector == new Vector2(i, 0) ? Color.Gold : Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + new Vector2(SK.Position_Mastermind_Lamps().X + 71 * i     , SK.Position_Mastermind_Lamps().Y + 400), Get_Mino_Texture(slot[i], "middle"),               Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(SK.texture_casino_bet_down,       SK.Position_DisplayEdge() + new Vector2(SK.Position_Mastermind_Lamps().X + 71 * i - 45, SK.Position_Mastermind_Lamps().Y + 470), null, selector == new Vector2(i, 1) ? Color.Gold : Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

                //spriteBatch.Draw(SK.texture_spritesheet_minos_64x, new Vector2(429 + 138 * i, 303), Get_Mino_Texture(slot[i], "upper"), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
                //spriteBatch.Draw(SK.texture_spritesheet_minos_64x, new Vector2(429 + 138 * i, 439), Get_Mino_Texture(slot[i], "lower"), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
                //spriteBatch.Draw(SK.texture_spritesheet_minos_64x, new Vector2(429 + 138 * i, 371), Get_Mino_Texture(slot[i], "middle"), Color.White, 0, new Vector2(0, 0), 2, SpriteEffects.None, 0);
            }
            //spriteBatch.Draw(SK.texture_background_mastermind2, SK.Position_DisplayEdge() + SK.Position_Mastermind2(), Color.White);
        }

        public override void Draw3() {
            if(!active_gameover && !active_pause) {
                spriteBatch.Draw(SK.texture_hud_button_questionmark, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
            }
        }

        private Rectangle Get_Mino_Texture(string _id, string _pos) {
            if(_pos == "middle") {
                if(_id == "fire") return Get_Mino_Texture("fire", 3, 64);
                if(_id == "air") return Get_Mino_Texture("air", 3, 64);
                if(_id == "thunder") return Get_Mino_Texture("thunder", 3, 64);
                if(_id == "water") return Get_Mino_Texture("water", 3, 64);
                if(_id == "ice") return Get_Mino_Texture("ice", 3, 64);
                if(_id == "earth") return Get_Mino_Texture("earth", 3, 64);
                if(_id == "metal") return Get_Mino_Texture("metal", 3, 64);
                if(_id == "nature") return Get_Mino_Texture("nature", 3, 64);
                if(_id == "light") return Get_Mino_Texture("light", 3, 64);
                if(_id == "dark") return Get_Mino_Texture("dark", 3, 64);
            }
            if(_pos == "upper") {
                if(_id == "fire") return Get_Mino_Texture("dark", 1, 64);
                if(_id == "air") return Get_Mino_Texture("fire", 1, 64);
                if(_id == "thunder") return Get_Mino_Texture("air", 1, 64);
                if(_id == "water") return Get_Mino_Texture("thunder", 1, 64);
                if(_id == "ice") return Get_Mino_Texture("water", 1, 64);
                if(_id == "earth") return Get_Mino_Texture("ice", 1, 64);
                if(_id == "metal") return Get_Mino_Texture("earth", 1, 64);
                if(_id == "nature") return Get_Mino_Texture("metal", 1, 64);
                if(_id == "light") return Get_Mino_Texture("nature", 1, 64);
                if(_id == "dark") return Get_Mino_Texture("light", 1, 64);
            }
            if(_pos == "lower") {
                if(_id == "fire") return Get_Mino_Texture("air", 1, 64);
                if(_id == "air") return Get_Mino_Texture("thunder", 1, 64);
                if(_id == "thunder") return Get_Mino_Texture("water", 1, 64);
                if(_id == "water") return Get_Mino_Texture("ice", 1, 64);
                if(_id == "ice") return Get_Mino_Texture("earth", 1, 64);
                if(_id == "earth") return Get_Mino_Texture("metal", 1, 64);
                if(_id == "metal") return Get_Mino_Texture("nature", 1, 64);
                if(_id == "nature") return Get_Mino_Texture("light", 1, 64);
                if(_id == "light") return Get_Mino_Texture("dark", 1, 64);
                if(_id == "dark") return Get_Mino_Texture("fire", 1, 64);
            }
            return Get_Mino_Texture("blank", 1, 64);
        }

        private void Command_Click() {
            slot[(int)selector.X] = Command_Cycle((int)selector.X, (int)selector.Y);
            //for(int y = 0; y < 2; y++) {
            //    for(int x = 0; x < 6; x++) {
            //        if(SK.Position_DisplayEdge().X + SK.Collision_Mastermind_Arrow().X + 138 * x < cursorX && cursorX < SK.Position_DisplayEdge().X + SK.Collision_Mastermind_Arrow().X + SK.Collision_Mastermind_Arrow().Width + 138 * x) {
            //            if(SK.Position_DisplayEdge().Y + SK.Collision_Mastermind_Arrow().Y + 253 * y < cursorY && cursorY < SK.Position_DisplayEdge().Y + SK.Collision_Mastermind_Arrow().Y + SK.Collision_Mastermind_Arrow().Height + 253 * y) {
            //                slot[x] = Command_Cycle(x, y);
            //            }
            //        }
            //    }
            //}
            //if(SK.Position_DisplayEdge().X + SK.Collision_Mastermind_Lamp().X < cursorX && cursorX < SK.Position_DisplayEdge().X + SK.Collision_Mastermind_Lamp().X + SK.Collision_Mastermind_Lamp().Width) {
            //    if(SK.Position_DisplayEdge().Y + SK.Collision_Mastermind_Lamp().Y < cursorY && cursorY < SK.Position_DisplayEdge().Y + SK.Collision_Mastermind_Lamp().Y + SK.Collision_Mastermind_Lamp().Height) {
            //        if(!active_gameover) Command_Unlock();
            //    }
            //}
        }

        private string Command_Cycle(int i, int j) {
            if(j == 0) { // Up
                     if(slot[i] == "fire") return "dark";
                else if(slot[i] == "air") return "fire";
                else if(slot[i] == "thunder") return "air";
                else if(slot[i] == "water") return "thunder";
                else if(slot[i] == "ice") return "water";
                else if(slot[i] == "earth") return "ice";
                else if(slot[i] == "metal") return "earth";
                else if(slot[i] == "nature") return "metal";
                else if(slot[i] == "light") return "nature";
                else if(slot[i] == "dark") return "light";
            }
            if(j == 1) { // Down
                     if(slot[i] == "fire") return "air";
                else if(slot[i] == "air") return "thunder";
                else if(slot[i] == "thunder") return "water";
                else if(slot[i] == "water") return "ice";
                else if(slot[i] == "ice") return "earth";
                else if(slot[i] == "earth") return "metal";
                else if(slot[i] == "metal") return "nature";
                else if(slot[i] == "nature") return "light";
                else if(slot[i] == "light") return "dark";
                else if(slot[i] == "dark") return "fire";
            }
            return "blank";
        }

        private void Command_Lock() {
            for(int i = 0; i < 6; i++) {
                int r = random.Next(10);
                if(r == 0) bolt[i] = "fire";
                if(r == 1) bolt[i] = "air";
                if(r == 2) bolt[i] = "thunder";
                if(r == 3) bolt[i] = "water";
                if(r == 4) bolt[i] = "ice";
                if(r == 5) bolt[i] = "earth";
                if(r == 6) bolt[i] = "metal";
                if(r == 7) bolt[i] = "nature";
                if(r == 8) bolt[i] = "light";
                if(r == 9) bolt[i] = "dark";
            }
        }

        private void Command_Unlock() {
            JK.Noise("Place");
            score_lives++;

            int unlocked = 0;

            lamp[0] = "empty";
            lamp[1] = "empty";
            lamp[2] = "empty";
            lamp[3] = "empty";
            lamp[4] = "empty";
            lamp[5] = "empty";

            bool[] locked = new bool[6];
            locked[0] = false;
            locked[1] = false;
            locked[2] = false;
            locked[3] = false;
            locked[4] = false;
            locked[5] = false;

            // search for green
            for(int i = 0; i < 6; i++) {
                if(slot[i] == bolt[i]) {
                    unlocked++;
                    lamp[i] = "green";
                    locked[i] = true;
                }
            }

            // search for yellow
            for(int i = 0; i < 6; i++) {
                if(lamp[i] != "green") { // if not already green
                    for(int j = 0; j < 6; j++) {
                        if(bolt[i] == slot[j] && !locked[j]) { // finds a match in a different slot that isnt already used
                            lamp[i] = "yellow";
                            locked[j] = true;
                            break;
                        }
                    }
                }
            }

            // colors red
            for(int i = 0; i < 6; i++) {
                if(lamp[i] == "empty") {
                    lamp[i] = "red";
                }
            }

            if(unlocked == 6) {
                won = true;
            } else {

            }
        }
    }
}
