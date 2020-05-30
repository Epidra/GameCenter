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
    class Columns : Ghost {

        bool active_hold;

        string[] container_next = new string[3];
        string[] container_hold = new string[3];
        string[] container_current = new string[3];

        double time_last;
        double time_break;

        bool[,] grid_base  = new   bool[10,20];
        string[,] grid_color = new string[10,20];

        Vector2[] tetromino = new Vector2[3];

        List<Vector2> clear = new List<Vector2>();

        bool won;

        int alpha;

        public Columns(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            active_hold = true;
            container_next[0] = Column_Roll();
            container_next[1] = Column_Roll();
            container_next[2] = Column_Roll();
            container_hold[0] = "empty";
            container_hold[1] = "empty";
            container_hold[2] = "empty";
            container_current[0] = Column_Roll();
            container_current[1] = Column_Roll();
            container_current[2] = Column_Roll();
            Column_Create();
            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < 20; j++) {
                    grid_base[i, j] = false;
                    grid_color[i, j] = "empty";
                }
            }
            time_break = 250 - 25 * game_difficulty;
            time_last = 0;
            won = false;
            clear.Clear();
            alpha = 255;
        }

        private string IntToMino(int i) {
            if(i == 0) return "fire";
            if(i == 1) return "air";
            if(i == 2) return "thunder";
            if(i == 3) return "water";
            if(i == 4) return "ice";
            if(i == 5) return "earth";
            if(i == 6) return "metal";
            if(i == 7) return "nature";
            if(i == 8) return "light";
            if(i == 9) return "dark";
            return "blank";
        }

        public override string Update2() {
            if(alpha == 255) {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { Command_Hold(); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) { Command_Hold(); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { Column_Drop(); }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.hold) { Column_Fall(); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { Command_Strafe("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { Command_Strafe("right"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_left_P1) == GhostState.pressed) { Command_Cycle("right"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_right_P1) == GhostState.pressed) { Command_Cycle("left"); pressed_response = true; }
                //if(control_keyboard_new.IsKeyDown(Keys.Down)) { Tetromino_Fall(); };
                //if(control_gamepad_new.IsConnected) {
                //    if(control_gamepad_new.DPad.Down == ButtonState.Pressed) { Tetromino_Fall(); }
                //    if(control_gamepad_new.Buttons.LeftShoulder == ButtonState.Pressed && control_gamepad_old.Buttons.LeftShoulder == ButtonState.Released) { Command_Turn("right"); }
                //    if(control_gamepad_new.Buttons.RightShoulder == ButtonState.Pressed && control_gamepad_old.Buttons.RightShoulder == ButtonState.Released) { Command_Turn("left"); }
                //}
                //if(control_keyboard_new.IsKeyDown(Keys.N) && control_keyboard_old.IsKeyUp(Keys.N)) { Command_Cycle("right"); };
                //if(control_keyboard_new.IsKeyDown(Keys.M) && control_keyboard_old.IsKeyUp(Keys.M)) { Command_Cycle("left"); };

                if(pressed_event_touch) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Hold().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Hold().Y, 160, 160))) { pressed_response = true; Command_Hold(); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Left().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Left().Y, 160, 160))) { pressed_response = true; Command_Cycle("left"); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Right().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Right().Y, 160, 160))) { pressed_response = true; Command_Cycle("right"); }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(alpha == 255) {
                if(gameTime.TotalGameTime.TotalMilliseconds > time_last + time_break - score_level * 5 * game_difficulty && !active_pause && !active_gameover && !FM.active_transition) {
                    Column_Fall();
                    time_last = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if(won && !active_gameover) {
                    GameOver(gameTime.TotalGameTime.TotalSeconds);
                }
            } else {
                alpha -= 10;
                if(alpha <= 0) {
                    alpha = 255;
                    for(int y = 0; y < 20; y++) {
                        for(int x = 0; x < 10; x++) {
                            if(IsCleared(x, y)) {
                                grid_base[x, y] = false;
                                grid_color[x, y] = "empty";
                            }
                        }
                    }
                    Command_Collapse();
                }
            }
            return "void";
        }

        private void Column_Drop() {
            int tempPoints = score_points;
            while(score_points == tempPoints) {
                Column_Fall();
            }
        }

        private bool IsCleared(int x, int y) {
            foreach(Vector2 v in clear) {
                if(v == new Vector2(x, y)) return true;
            }
            return false;
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_tetris, SK.Position_DisplayEdge() + SK.Position_Tetris_Field(), Color.White);
            spriteBatch.Draw(SK.texture_static_tetris_next, SK.Position_DisplayEdge() + SK.Position_Tetris_Next(), Color.White);
            if(0 < FM.purchased[FM.Convert("hold")])
                spriteBatch.Draw(SK.texture_static_tetris_hold, SK.Position_DisplayEdge() + SK.Position_Tetris_Hold(), Color.White);
            spriteBatch.Draw(SK.texture_static_tetris_left, SK.Position_DisplayEdge() + SK.Position_Tetris_Left(), Color.White);
            spriteBatch.Draw(SK.texture_static_tetris_right, SK.Position_DisplayEdge() + SK.Position_Tetris_Right(), Color.White);

            for(int y = 0; y < 20; y++) {
                for(int x = 0; x < 10; x++) {
                    if(grid_base[x, y]) {
                        spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + new Vector2(32 * x, 32 * y), Get_Mino_Texture(grid_color[x, y], 0, 32), IsCleared(x, y) ? new Color(alpha, alpha, alpha) : Color.White);
                    }
                }
            }

            if(alpha == 255) {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[0] * 32, Get_Mino_Texture(container_current[0], 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[1] * 32, Get_Mino_Texture(container_current[1], 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[2] * 32, Get_Mino_Texture(container_current[2], 0, 32), Color.White);
            }

            if(container_next[0] != "empty")
                Get_Column(SK.Position_DisplayEdge() + SK.Position_Tetris_Next() + new Vector2(16, 16), container_next);
            if(container_hold[0] != "empty" && 0 < FM.purchased[FM.Convert("hold")])
                Get_Column(SK.Position_DisplayEdge() + SK.Position_Tetris_Hold() + new Vector2(16, 16), container_hold);
        }

        private void Get_Column(Vector2 position, string[] tetro) {
            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48,  0+16), Get_Mino_Texture(tetro[0], 0, 32), Color.White);
            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32+16), Get_Mino_Texture(tetro[1], 0, 32), Color.White);
            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64+16), Get_Mino_Texture(tetro[2], 0, 32), Color.White);
        }

        private void Command_Collapse() {
            // Gravity after match found and cleared
            int temp = 0;
            for(int y = 18; y >= 0; y--) {
                for(int x = 0; x < 10; x++) {
                    if(grid_base[x, y]) {
                        temp = 0;
                        if(!grid_base[x, y + 1]) {
                            temp++;
                            if(y+2 < 20 && !grid_base[x, y + 2]) {
                                temp++;
                                if(y+3 < 20 && !grid_base[x, y + 3]) {
                                    temp++;
                                }
                            }
                        }
                        if(temp != 0) {
                            grid_base[x, y + temp] = true;
                            grid_base[x, y       ] = false;
                            grid_color[x, y + temp] = grid_color[x, y];
                            grid_color[x, y       ] = "empty";
                        }
                    }
                }
            }
            clear.Clear();
            alpha = 255;
            Check_Field();
            JK.Noise("Explosion");
        }

        private void Command_Strafe(string _direction) {
            int dir = 0;
            if(_direction == "left") {
                dir = -1;
                if(tetromino[0].X > 0) {
                    if(tetromino[1].X > 0) {
                        if(tetromino[2].X > 0) {
                            if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                                if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                    if(!grid_base[(int)tetromino[2].X + dir, (int)tetromino[2].Y]) {
                                        tetromino[0].X = tetromino[0].X + dir;
                                        tetromino[1].X = tetromino[1].X + dir;
                                        tetromino[2].X = tetromino[2].X + dir;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if(_direction == "right") {
                dir = 1;
                if(tetromino[0].X < 9) {
                    if(tetromino[1].X < 9) {
                        if(tetromino[2].X < 9) {
                            if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                                if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                    if(!grid_base[(int)tetromino[2].X + dir, (int)tetromino[2].Y]) {
                                        tetromino[0].X = tetromino[0].X + dir;
                                        tetromino[1].X = tetromino[1].X + dir;
                                        tetromino[2].X = tetromino[2].X + dir;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Command_Cycle(string _direction) {
            if(_direction == "left") {
                string temp = container_current[0];
                container_current[0] = container_current[1];
                container_current[1] = container_current[2];
                container_current[2] = temp;
            }
            if(_direction == "right") {
                string temp = container_current[2];
                container_current[2] = container_current[1];
                container_current[1] = container_current[0];
                container_current[0] = temp;
            }

        }

        private void Command_Hold() {
            if(0 < FM.purchased[FM.Convert("hold")]) {
                if(active_hold) {
                    active_hold = false;
                    if(container_hold[0] == "empty") {
                        container_hold[0] = container_current[0];
                        container_hold[1] = container_current[1];
                        container_hold[2] = container_current[2];
                        container_current[0] = container_next[0];
                        container_current[1] = container_next[1];
                        container_current[2] = container_next[2];
                        container_next[0] = Column_Roll();
                        container_next[1] = Column_Roll();
                        container_next[2] = Column_Roll();
                    } else {
                        string[] temp = new string[3];
                        temp[0] = container_hold[0];
                        temp[1] = container_hold[1];
                        temp[2] = container_hold[2];
                        container_hold[0] = container_current[0];
                        container_hold[1] = container_current[1];
                        container_hold[2] = container_current[2];
                        container_current[0] = temp[0];
                        container_current[1] = temp[1];
                        container_current[2] = temp[2];
                    }
                    Column_Create();
                }
            }
        }

        private string Column_Roll() {
            return IntToMino(random.Next(8));
        }

        private void Column_Create() {
            tetromino[0] = new Vector2(4, 0);
            tetromino[1] = new Vector2(4, 1);
            tetromino[2] = new Vector2(4, 2);
        }

        private void Column_Fall() {
            if(tetromino[0].Y < 19 && tetromino[1].Y < 19 && tetromino[2].Y < 19) {
                if(!grid_base[(int)tetromino[0].X, (int)tetromino[0].Y + 1] && !grid_base[(int)tetromino[1].X, (int)tetromino[1].Y + 1] && !grid_base[(int)tetromino[2].X, (int)tetromino[2].Y + 1]) {
                    tetromino[0].Y = tetromino[0].Y + 1;
                    tetromino[1].Y = tetromino[1].Y + 1;
                    tetromino[2].Y = tetromino[2].Y + 1;
                } else {
                    Column_Place();
                }
            } else {
                Column_Place();
            }
        }

        private void Column_Place() {
            JK.Noise("Place");
            active_hold = true;
            grid_base[(int)tetromino[0].X, (int)tetromino[0].Y] = true;
            grid_base[(int)tetromino[1].X, (int)tetromino[1].Y] = true;
            grid_base[(int)tetromino[2].X, (int)tetromino[2].Y] = true;
            grid_color[(int)tetromino[0].X, (int)tetromino[0].Y] = container_current[0];
            grid_color[(int)tetromino[1].X, (int)tetromino[1].Y] = container_current[1];
            grid_color[(int)tetromino[2].X, (int)tetromino[2].Y] = container_current[2];
            score_points = score_points + 2 * game_difficulty;
            if(tetromino[0].Y == 0) won = true;
            container_current[0] = container_next[0];
            container_current[1] = container_next[1];
            container_current[2] = container_next[2];
            container_next[0] = Column_Roll();
            container_next[1] = Column_Roll();
            container_next[2] = Column_Roll();
            Column_Create();
            Check_Field();
        }

        private void Check_Field() {

            int points = 0;
            int bonus = 0;

            for(int y = 19; y >= 0; y--) {
                for(int x = 0; x < 10; x++) {
                    if(x < 8 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x + 1, y] && grid_color[x, y] == grid_color[x + 2, y]) {
                        clear.Add(new Vector2(x, y)); clear.Add(new Vector2(x + 1, y)); clear.Add(new Vector2(x + 2, y));
                        points += (30 + bonus);
                        bonus += 10;
                    }
                    if(y > 1 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x, y - 1] && grid_color[x, y] == grid_color[x, y - 2]) {
                        clear.Add(new Vector2(x, y)); clear.Add(new Vector2(x, y - 1)); clear.Add(new Vector2(x, y - 2));
                        points += (30 + bonus);
                        bonus += 10;
                    }
                    if(x < 8 && y > 1 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x + 1, y - 1] && grid_color[x, y] == grid_color[x + 2, y - 2]) {
                        clear.Add(new Vector2(x, y)); clear.Add(new Vector2(x + 1, y - 1)); clear.Add(new Vector2(x + 2, y - 2));
                        points += (30 + bonus);
                        bonus += 10;
                    }
                }
            }

            if(points > 0) {
                alpha -= 5;
                score_points += (points * game_difficulty * (score_level + 1));
                score_lives++;
                if(score_lives > (1 + score_level) * 10) {
                    score_level++;
                }
                //Command_Collapse();
            }
        }

    }
}
