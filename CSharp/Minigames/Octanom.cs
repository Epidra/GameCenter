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
    class Octanom : Ghost {

        List<Entity> explosions = new List<Entity>();

        MapRoom MP = new MapRoom();

        int score_power_hit;

        int map = 0;

        bool[,] grid_main   = new bool[28, 23];
        bool[,] grid_points = new bool[28, 23];
        bool[,] grid_power  = new bool[28, 23];
        bool[,] grid_gate   = new bool[28, 23];

        Vector2 position_octanom_start = new Vector2(0,0);
        Vector2 position_trianom_start = new Vector2(0,0);

        Entity octanom;
        Entity[] trianom = new Entity[4];

        bool   powerup             = false;
        double powerup_timer_start = 0;
        double powerup_timer_break = 2000;

        string temp_movement;

        public Octanom(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
            id = _id;
            CM = _content;
            SK = _Shopkeeper;
            FM = _Filemanager;
            JK = _Jukebox;
            screensize_width  = (int)SK.screensize_main.X;
            screensize_height = (int)SK.screensize_main.Y;
            screensize_width_scale  = screenX;
            screensize_height_scale = screenY;
            random = new Random();
        }

        public override void Start2() {
            octanom    = new Entity(1, new Vector2(0,0), new Vector2( 0,0));
            trianom[0] = new Entity(1, new Vector2(0,0), new Vector2( 2,0), new Vector2(0,0));
            trianom[1] = new Entity(1, new Vector2(0,0), new Vector2(-2,0), new Vector2(0,0));
            trianom[2] = new Entity(1, new Vector2(0,0), new Vector2( 2,0), new Vector2(0,0));
            trianom[3] = new Entity(1, new Vector2(0,0), new Vector2(-2,0), new Vector2(0,0));
            score_lives = FM.octaLives;
            score_level = 1;
            score_power_hit = 1;
            grid_main   = new bool[28, 23];
            grid_points = new bool[28, 23];
            grid_power  = new bool[28, 23];
            map = random.Next(MP.octanom.Length);
            Load_Map();
            powerup = false;
            powerup_timer_start = 0;
            powerup_timer_break = 10000;
            temp_movement = "null";
            octanom.Set_Pos(position_octanom_start.X*32, position_octanom_start.Y*32);
        }

        private void Load_Map() {
            List<string> list = MP.octanom[map];
            int y = 0;
            int t = 0;
            foreach(string s in list) {
                for(int x = 0; x < 28; x++) {
                    switch(s.Substring(x, 1)) {
                        case " ": break;
                        case "X": grid_main  [x, y] = true; break;
                        case "O": grid_main  [x, y] = true; position_octanom_start = new Vector2(x, y); octanom.Set_Next(new Vector2(x, y)); break;
                        case "T": grid_main  [x, y] = true; position_trianom_start = new Vector2(x, y); break;
                        case "C": grid_points[x, y] = true; break;
                        case "P": grid_power [x, y] = true; break;
                        case "G": grid_gate  [x, y] = true; TrianomStart(x, y, t); t++; break;
                    }
                }
                y++;
            }
        }

        private void TrianomStart(int x, int y, int t) {
            trianom[t].Set_Pos(x * 32, y * 32);
            trianom[t].Set_Next(new Vector2(x + t%2 == 0 ? +1 : -1, y));
        }

        public void Restart() {
            octanom    = new Entity(1, new Vector2(0, 0), new Vector2( 0, 0));
            trianom[0] = new Entity(1, new Vector2(0, 0), new Vector2( 2, 0), new Vector2(0, 0));
            trianom[1] = new Entity(1, new Vector2(0, 0), new Vector2(-2, 0), new Vector2(0, 0));
            trianom[2] = new Entity(1, new Vector2(0, 0), new Vector2( 2, 0), new Vector2(0, 0));
            trianom[3] = new Entity(1, new Vector2(0, 0), new Vector2(-2, 0), new Vector2(0, 0));
            score_lives = FM.octaLives;
            score_level = 1;
            score_power_hit = 1;
            grid_main = new bool[28, 23];
            grid_points = new bool[28, 23];
            grid_power = new bool[28, 23];
            Load_Map();
            powerup = false;
            powerup_timer_start = 0;
            powerup_timer_break = 10000;
            temp_movement = "null";
            octanom.Set_Pos(position_octanom_start.X * 32, position_octanom_start.Y * 32);
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed)    { pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { if(SK.orientation <= 2) { temp_movement = "Up";    } else { temp_movement = "Right"; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { if(SK.orientation <= 2) { temp_movement = "Down";  } else { temp_movement = "Left";  } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { if(SK.orientation <= 2) { temp_movement = "Left";  } else { temp_movement = "Up";    } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { if(SK.orientation <= 2) { temp_movement = "Right"; } else { temp_movement = "Down";  } pressed_response = true; }
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(!active_pause && !active_gameover && !FM.active_transition) {

                octanom.Update(powerup);
                Command_Move_Octanom(gameTime);

                for(int i = 0; i < 4; i++) {
                    string sa = Command_Move_Trianom(trianom[i]);
                    if(sa == "up")    if(trianom[i].Get_Next().Y ==  0) { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X,                      21)); trianom[i].Set_Pos(trianom[i].Get_Pos().X          , SK.Position_Grid32().Y + 32 * 23); } else { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X  , trianom[i].Get_Next().Y-1)); }
                    if(sa == "down")  if(trianom[i].Get_Next().Y == 21) { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X,                       0)); trianom[i].Set_Pos(trianom[i].Get_Pos().X          , SK.Position_Grid32().Y - 32     ); } else { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X  , trianom[i].Get_Next().Y+1)); }
                    if(sa == "left")  if(trianom[i].Get_Next().X ==  0) { trianom[i].Set_Next(new Vector2(                     27, trianom[i].Get_Next().Y)); trianom[i].Set_Pos(SK.Position_Grid32().X + 32 * 28, trianom[i].Get_Pos().Y          ); } else { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X-1, trianom[i].Get_Next().Y  )); }
                    if(sa == "right") if(trianom[i].Get_Next().X == 27) { trianom[i].Set_Next(new Vector2(                      0, trianom[i].Get_Next().Y)); trianom[i].Set_Pos(SK.Position_Grid32().X - 32     , trianom[i].Get_Pos().Y          ); } else { trianom[i].Set_Next(new Vector2(trianom[i].Get_Next().X+1, trianom[i].Get_Next().Y  )); }
                    trianom[i].Update();
                    Command_Collision(trianom[i]);
                }

                bool temp_break = false; ;
                for(int x = 0; x < 28; x++) {
                    for(int y = 0; y < 22; y++) {
                        if(grid_points[x, y]) {
                            temp_break = true;
                            break;
                        }
                    }
                    if(temp_break) break;
                }
                if(!temp_break) {
                    JK.Noise("Cleared");
                    Restart();
                }

                if(gameTime.TotalGameTime.TotalMilliseconds > powerup_timer_start + powerup_timer_break && powerup) {
                    powerup = false;
                    score_power_hit = 1;
                }
            }

            if(score_lives <= 0) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            foreach(Entity e in explosions) {
                if(e.Get_HP() > 15) {
                    explosions.Remove(e);
                    break;
                }
            }
            return "void";
        }

        private Vector2 Get_GridPos(int x, int y) {
            if(SK.orientation <= 2) {
                return new Vector2(32 * x, 32 * y);
            } else {
                return new Vector2(32 * y, 32 * (27 - x));
            }
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_grid32, SK.Position_DisplayEdge() + SK.Position_BackgroundGrid(), Color.White);

            for(int x = 0; x < 28; x++) {
                for(int y = 0; y < 23; y++) {
                    if(grid_main[x, y]) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(0, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    if(grid_points[x, y]) spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(0, 64, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    if(grid_power[x, y]) spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(0, 32, 32, 32), Color.LightBlue, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }
            }

            Color color       = Color.White;
            if(powerup) color = Color.Blue;

            if(!active_gameover)
            spriteBatch.Draw(SK.texture_spritesheet_octanom_head, SK.Position_DisplayEdge() + SK.Position_Grid32() + octanom   .Get_GridPos(SK.orientation), new Rectangle(0, octanom   .Get_LookDirection(SK.orientation) * (32), 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_trianom,      SK.Position_DisplayEdge() + SK.Position_Grid32() + trianom[0].Get_GridPos(SK.orientation), new Rectangle(0, trianom[0].Get_LookDirection(SK.orientation) * (32), 32, 32), color,       0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_trianom,      SK.Position_DisplayEdge() + SK.Position_Grid32() + trianom[1].Get_GridPos(SK.orientation), new Rectangle(0, trianom[1].Get_LookDirection(SK.orientation) * (32), 32, 32), color,       0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_trianom,      SK.Position_DisplayEdge() + SK.Position_Grid32() + trianom[2].Get_GridPos(SK.orientation), new Rectangle(0, trianom[2].Get_LookDirection(SK.orientation) * (32), 32, 32), color,       0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_trianom,      SK.Position_DisplayEdge() + SK.Position_Grid32() + trianom[3].Get_GridPos(SK.orientation), new Rectangle(0, trianom[3].Get_LookDirection(SK.orientation) * (32), 32, 32), color,       0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            for(int x = 0; x < 28; x++) {
                for(int y = 0; y < 23; y++) {
                    if(grid_gate[x, y]) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(32, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    if(position_octanom_start == new Vector2(x, y)) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(64, 32, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    if(position_trianom_start == new Vector2(x, y)) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_GridPos(x, y), new Rectangle(96, 32, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }
            }

            foreach(Entity E in explosions) {
                spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_DisplayEdge() + SK.Position_Grid32() + E.Get_GridPos(SK.orientation) - new Vector2(16), new Rectangle(1 + E.Get_HP()/2 + (64 * (E.Get_HP() / 2)), 1, 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                E.Change_HP(1);
            }

        }

        private void Command_Move_Octanom(GameTime gameTime) {
            if(octanom.Get_Pos() == octanom.Get_Next() * 32) {
                string temp = "null";
                if(octanom.Get_Vel().Y < 0) temp = "Up";
                if(octanom.Get_Vel().Y > 0) temp = "Down";
                if(octanom.Get_Vel().X < 0) temp = "Left";
                if(octanom.Get_Vel().X > 0) temp = "Right";

                if(grid_points[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y]) {
                    grid_points[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y] = false;
                    score_points = score_points + 10;
                }

                if(grid_power[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y]) {
                    grid_power[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y] = false;
                    JK.Noise("Coin");
                    score_points = score_points + 100;
                    powerup = true;
                    powerup_timer_start = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if(temp_movement == "Up" && !grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y - 1]) {
                    octanom.Set_Next(octanom.Get_Next() + new Vector2(0, -1));
                    octanom.Set_Vel(0, -2);
                    temp_movement = "null";
                    if(octanom.Get_Next().Y == 0) {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, 21));
                        octanom.Set_Pos(octanom.Get_Pos().X, 32 * 23);
                    }
                } else
                if(temp_movement == "Down" && !grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y + 1]) {
                    octanom.Set_Next(octanom.Get_Next() + new Vector2(0, 1));
                    octanom.Set_Vel(0, 2);
                    temp_movement = "null";
                    if(octanom.Get_Next().Y == 21) {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, 0));
                        octanom.Set_Pos(octanom.Get_Pos().X, 32);
                    }
                } else
                if(temp_movement == "Left" && !grid_main[(int)octanom.Get_Next().X - 1, (int)octanom.Get_Next().Y]) {
                    octanom.Set_Next(octanom.Get_Next() + new Vector2(-1, 0));
                    octanom.Set_Vel(-2, 0);
                    temp_movement = "null";
                    if(octanom.Get_Pos().X == 0) {
                        octanom.Set_Next(new Vector2(27, octanom.Get_Next().Y));
                        octanom.Set_Pos(32 * 28, octanom.Get_Pos().Y);
                    }
                } else
                if(temp_movement == "Right" && !grid_main[(int)octanom.Get_Next().X + 1, (int)octanom.Get_Next().Y]) {
                    octanom.Set_Next(octanom.Get_Next() + new Vector2(1, 0));
                    octanom.Set_Vel(2, 0);
                    temp_movement = "null";
                    if(octanom.Get_Pos().X == 27 * 32) {
                        octanom.Set_Next(new Vector2(1, octanom.Get_Next().Y));
                        octanom.Set_Pos(0, octanom.Get_Pos().Y);
                    }
                } else {
                    if(temp == "Up") {
                        if(octanom.Get_Next().Y == 0) {
                            octanom.Set_Next(new Vector2(octanom.Get_Next().X, 21));
                            octanom.Set_Pos(octanom.Get_Pos().X, 32 * 23);
                        }
                    }
                    if(temp == "Down") {
                        if(octanom.Get_Next().Y == 21) {
                            octanom.Set_Next(new Vector2(octanom.Get_Next().X, 0));
                            octanom.Set_Pos(octanom.Get_Pos().X, 32);
                        }
                    }
                    if(temp == "Left") {
                        if(octanom.Get_Pos().X == 0) {
                            octanom.Set_Next(new Vector2(27, octanom.Get_Next().Y));
                            octanom.Set_Pos(32 * 28, octanom.Get_Pos().Y);
                        }
                    }
                    if(temp == "Right") {
                        if(octanom.Get_Pos().X == 27*32) {
                            octanom.Set_Next(new Vector2(1, octanom.Get_Next().Y));
                            octanom.Set_Pos(0, octanom.Get_Pos().Y);
                        }
                    }
                    if(temp == "Up") {
                        if(grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y - 1]) {
                            octanom.Set_Vel(0, 0);
                            JK.Noise("Place");
                        } else {
                            octanom.Set_Next(octanom.Get_Next() + new Vector2(0, -1));
                        }
                    }
                    if(temp == "Down") {
                        if(grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y + 1]) {
                            octanom.Set_Vel(0, 0);
                            JK.Noise("Place");
                        } else {
                            octanom.Set_Next(octanom.Get_Next() + new Vector2(0, 1));
                        }
                    }
                    if(temp == "Left") {
                        if(grid_main[(int)octanom.Get_Next().X - 1, (int)octanom.Get_Next().Y]) {
                            octanom.Set_Vel(0, 0);
                            JK.Noise("Place");
                        } else {
                            octanom.Set_Next(octanom.Get_Next() + new Vector2(-1, 0));
                        }
                    }
                    if(temp == "Right") {
                        if(grid_main[(int)octanom.Get_Next().X + 1, (int)octanom.Get_Next().Y]) {
                            octanom.Set_Vel(0, 0);
                            JK.Noise("Place");
                        } else {
                            octanom.Set_Next(octanom.Get_Next() + new Vector2(1, 0));
                        }
                    }
                }

            }
        }

        private string Command_Move_Trianom(Entity trianom) {
            if(trianom.Get_Pos() == trianom.Get_Next() * 32) {
                string temp = "null";
                if(trianom.Get_Vel().Y == -2) temp = "up";
                if(trianom.Get_Vel().Y ==  2) temp = "down";
                if(trianom.Get_Vel().X == -2) temp = "left";
                if(trianom.Get_Vel().X ==  2) temp = "right";

                string dir_next = temp;

                if(temp == "up") {
                    //trianom.Set_Next(trianom.Get_Next() + new Vector2(0, -1));
                    if(trianom.Get_Next().Y == 0) {
                        trianom.Set_Next(new Vector2(trianom.Get_Next().X, 21));
                        trianom.Set_Pos(trianom.Get_Pos().X, 32 * 23);
                    }
                }
                if(temp == "down") {
                    //trianom.Set_Next(trianom.Get_Next() + new Vector2(0, 1));
                    if(trianom.Get_Next().Y == 21) {
                        trianom.Set_Next(new Vector2(trianom.Get_Next().X, 0));
                        trianom.Set_Pos(trianom.Get_Pos().X, 32);
                    }
                }
                if(temp == "left") {
                    //trianom.Set_Next(trianom.Get_Next() + new Vector2(-1, 0));
                    if(trianom.Get_Pos().X == 0 || trianom.Get_Pos().X == -32) {
                        trianom.Set_Next(new Vector2(27, trianom.Get_Next().Y));
                        trianom.Set_Pos(32 * 28, trianom.Get_Pos().Y);
                    }
                }
                if(temp == "right") {
                    //trianom.Set_Next(trianom.Get_Next() + new Vector2(1, 0));
                    if(trianom.Get_Pos().X == 27*32) {
                        trianom.Set_Next(new Vector2(0, trianom.Get_Next().Y));
                        trianom.Set_Pos(-32, trianom.Get_Pos().Y);
                    }
                }

                if(temp == "up") {
                    if(!grid_main[(int)trianom.Get_Next().X - 1, (int)trianom.Get_Next().Y] || !grid_main[(int)trianom.Get_Next().X + 1, (int)trianom.Get_Next().Y]) {
                        string dir1 = "empty";
                        string dir2 = "empty";
                        string dir3 = "empty";
                        int dir_i = 0;
                        if(!grid_main[(int)trianom.Get_Next().X - 1, (int)trianom.Get_Next().Y]    ) { dir1 = "left"; dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X + 1, (int)trianom.Get_Next().Y]    ) { if(dir1 == "empty") { dir1 = "right"; } else { dir2 = "right"; } dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y - 1]) { if(dir1 == "empty") { dir1 = "up"; } else if(dir2 == "empty") { dir2 = "up"; } else { dir3 = "up"; } dir_i++; }
                        int temp_int = random.Next(dir_i) + 1;
                        if(temp_int == 1) dir_next = dir1;
                        if(temp_int == 2) dir_next = dir2;
                        if(temp_int == 3) dir_next = dir3;
                    }
                }
                if(temp == "down") {
                    if(!grid_main[(int)trianom.Get_Next().X - 1, (int)trianom.Get_Next().Y] || !grid_main[(int)trianom.Get_Next().X + 1, (int)trianom.Get_Next().Y]) {
                        string dir1 = "empty";
                        string dir2 = "empty";
                        string dir3 = "empty";
                        int dir_i = 0;
                        if(!grid_main[(int)trianom.Get_Next().X - 1, (int)trianom.Get_Next().Y]    ) { dir1 = "left"; dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X + 1, (int)trianom.Get_Next().Y]    ) { if(dir1 == "empty") { dir1 = "right"; } else { dir2 = "right"; } dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y + 1]) { if(dir1 == "empty") { dir1 = "down"; } else if(dir2 == "empty") { dir2 = "down"; } else { dir3 = "down"; } dir_i++; }
                        int temp_int = random.Next(dir_i) + 1;
                        if(temp_int == 1) dir_next = dir1;
                        if(temp_int == 2) dir_next = dir2;
                        if(temp_int == 3) dir_next = dir3;
                    }
                }
                if(temp == "left") {
                    if(!grid_main[(int)trianom.Get_Next().X, (int)trianom.Get_Next().Y - 1] || !grid_main[(int)trianom.Get_Next().X, (int)trianom.Get_Next().Y + 1]) {
                        string dir1 = "empty";
                        string dir2 = "empty";
                        string dir3 = "empty";
                        int dir_i = 0;
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y + 1]) { dir1 = "down"; dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y - 1]) { if(dir1 == "empty") { dir1 = "up"; } else { dir2 = "up"; } dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X - 1, (int)trianom.Get_Next().Y    ]) { if(dir1 == "empty") { dir1 = "left"; } else if(dir2 == "empty") { dir2 = "left"; } else { dir3 = "left"; } dir_i++; }
                        int temp_int = random.Next(dir_i) + 1;
                        if(temp_int == 1) dir_next = dir1;
                        if(temp_int == 2) dir_next = dir2;
                        if(temp_int == 3) dir_next = dir3;
                    }
                }
                if(temp == "right") {
                    if(!grid_main[(int)trianom.Get_Next().X, (int)trianom.Get_Next().Y - 1] || !grid_main[(int)trianom.Get_Next().X, (int)trianom.Get_Next().Y + 1]) {
                        string dir1 = "empty";
                        string dir2 = "empty";
                        string dir3 = "empty";
                        int dir_i = 0;
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y + 1]) { dir1 = "down"; dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X,     (int)trianom.Get_Next().Y - 1]) { if(dir1 == "empty") { dir1 = "up"; } else { dir2 = "up"; } dir_i++; }
                        if(!grid_main[(int)trianom.Get_Next().X + 1, (int)trianom.Get_Next().Y]) { if(dir1 == "empty") { dir1 = "right"; } else if(dir2 == "empty") { dir2 = "right"; } else { dir3 = "right"; } dir_i++; }
                        int temp_int = random.Next(dir_i) + 1;
                        if(temp_int == 1) dir_next = dir1;
                        if(temp_int == 2) dir_next = dir2;
                        if(temp_int == 3) dir_next = dir3;
                    }
                }

                if(dir_next == "up")    { trianom.Set_Vel( 0, -2); }
                if(dir_next == "down")  { trianom.Set_Vel( 0,  2); }
                if(dir_next == "left")  { trianom.Set_Vel(-2,  0); }
                if(dir_next == "right") { trianom.Set_Vel( 2,  0); }

                return dir_next;

            }

            return "null";
        }

        private void Command_Collision(Entity trianom) {
            if(!powerup) {
                if(octanom.Get_GridPos().X == trianom.Get_GridPos().X && octanom.Get_GridPos().Y - 16 < trianom.Get_GridPos().Y && trianom.Get_GridPos().Y < octanom.Get_GridPos().Y + 16) {
                    score_lives--;
                    JK.Noise("Explosion");
                    explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                    Command_Respawn(octanom);
                }
                if(octanom.Get_GridPos().Y == trianom.Get_GridPos().Y && octanom.Get_GridPos().X - 16 < trianom.Get_GridPos().X && trianom.Get_GridPos().X < octanom.Get_GridPos().X + 16) {
                    score_lives--;
                    JK.Noise("Explosion");
                    explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                    Command_Respawn(octanom);
                }
            } else {
                if(octanom.Get_GridPos().X == trianom.Get_GridPos().X && octanom.Get_GridPos().Y - 16 < trianom.Get_GridPos().Y && trianom.Get_GridPos().Y < octanom.Get_GridPos().Y + 16) {
                    score_points = score_points + 200 * score_power_hit * score_power_hit;
                    JK.Noise("Explosion");
                    explosions.Add(new Entity(0, trianom.Get_Pos(), new Vector2(0, 0)));
                    Command_Respawn(trianom);
                }
                if(octanom.Get_GridPos().Y == trianom.Get_GridPos().Y && octanom.Get_GridPos().X - 16 < trianom.Get_GridPos().X && trianom.Get_GridPos().X < octanom.Get_GridPos().X + 16) {
                    score_points = score_points + 200 * score_power_hit * score_power_hit;
                    JK.Noise("Explosion");
                    explosions.Add(new Entity(0, trianom.Get_Pos(), new Vector2(0, 0)));
                    Command_Respawn(trianom);
                }
            }
        }

        private void Command_Respawn(Entity _id) {
            if(_id == octanom) {
                octanom.Set_Pos(position_octanom_start.X * 32, position_octanom_start.Y * 32);
                octanom.Set_Vel(0, 0);
                octanom.Set_Next(position_octanom_start);
            }
            for(int i = 0; i < 4; i++) {
                if(_id == trianom[i]) {
                    trianom[i].Set_Pos(position_trianom_start.X * 32, position_trianom_start.Y * 32);
                    trianom[i].Set_Vel(0, 2);
                    trianom[i].Set_Next(new Vector2(position_trianom_start.X, position_trianom_start.Y + 1));
                }
            }
        }
    }
}
