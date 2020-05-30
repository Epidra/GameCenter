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
    class MeanMinos : Ghost {

        bool active_hold;

        string[] container_next = new string[2];
        string[] container_hold = new string[2];
        string[] container_current = new string[2];

        double time_last;
        double time_break;

        bool[,] grid_base  = new   bool[10,20];
        string[,] grid_color = new string[10,20];

        Vector2[] tetromino = new Vector2[2];

        List<Vector2> clear = new List<Vector2>();

        bool won;

        int alpha;

        public MeanMinos(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            container_next[0] = Domino_Roll();
            container_next[1] = Domino_Roll();
            container_hold[0] = "empty";
            container_hold[1] = "empty";
            container_current[0] = Domino_Roll();
            container_current[1] = Domino_Roll();
            Domino_Create();
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
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { Domino_Drop(); }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.hold) { Domino_Fall(); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { Command_Strafe("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { Command_Strafe("right"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_left_P1) == GhostState.pressed) { Command_Turn("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_right_P1) == GhostState.pressed) { Command_Turn("right"); pressed_response = true; }
                //if(control_keyboard_new.IsKeyDown(Keys.Down)) { Tetromino_Fall(); };
                //if(control_gamepad_new.IsConnected) {
                //    if(control_gamepad_new.DPad.Down == ButtonState.Pressed) { Tetromino_Fall(); }
                //    if(control_gamepad_new.Buttons.LeftShoulder == ButtonState.Pressed && control_gamepad_old.Buttons.LeftShoulder == ButtonState.Released) { Command_Turn("right"); }
                //    if(control_gamepad_new.Buttons.RightShoulder == ButtonState.Pressed && control_gamepad_old.Buttons.RightShoulder == ButtonState.Released) { Command_Turn("left"); }
                //}
                //if(control_keyboard_new.IsKeyDown(Keys.N) && control_keyboard_old.IsKeyUp(Keys.N)) { Command_Turn("right"); };
                //if(control_keyboard_new.IsKeyDown(Keys.M) && control_keyboard_old.IsKeyUp(Keys.M)) { Command_Turn("left"); };

                if(pressed_event_touch) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Hold().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Hold().Y, 160, 160))) { pressed_response = true; Command_Hold(); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Left().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Left().Y, 160, 160))) { pressed_response = true; Command_Turn("left"); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Tetris_Right().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Tetris_Right().Y, 160, 160))) { pressed_response = true; Command_Turn("right"); }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(alpha == 255) {
                if(gameTime.TotalGameTime.TotalMilliseconds > time_last + time_break - score_level * 5 * game_difficulty && !active_pause && !active_gameover && !FM.active_transition) {
                    Domino_Fall();
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

        private void Domino_Drop() {
            int tempPoints = score_points;
            while(score_points == tempPoints) {
                Domino_Fall();
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
            }

            if(container_next[0] != "empty")
                Get_Domino(SK.Position_DisplayEdge() + SK.Position_Tetris_Next() + new Vector2(16, 16), container_next);
            if(container_hold[0] != "empty" && 0 < FM.purchased[FM.Convert("hold")])
                Get_Domino(SK.Position_DisplayEdge() + SK.Position_Tetris_Hold() + new Vector2(16, 16), container_hold);
        }

        private void Get_Domino(Vector2 position, string[] tetro) {
            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32), Get_Mino_Texture(tetro[1], 0, 32), Color.White);
            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64), Get_Mino_Texture(tetro[0], 0, 32), Color.White);
        }

        private void Command_Collapse() {
            // Gravity after match found and cleared
            int temp = 0;
            for(int y = 18; y >= 0; y--) {
                for(int x = 0; x < 10; x++) {
                    if(grid_base[x, y]) {
                        temp = 0;
                        while(y + temp + 1 < 20 && !grid_base[x, y + temp + 1]) {
                            temp++;
                        }
                        if(temp != 0) {
                            grid_base[x, y + temp] = true;
                            grid_base[x, y] = false;
                            grid_color[x, y + temp] = grid_color[x, y];
                            grid_color[x, y] = "empty";
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
                        if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                            if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                tetromino[0].X = tetromino[0].X + dir;
                                tetromino[1].X = tetromino[1].X + dir;
                            }
                        }
                    }
                }
            }
            if(_direction == "right") {
                dir = 1;
                if(tetromino[0].X < 9) {
                    if(tetromino[1].X < 9) {
                        if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                            if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                tetromino[0].X = tetromino[0].X + dir;
                                tetromino[1].X = tetromino[1].X + dir;
                            }
                        }
                    }
                }
            }
        }

        private void Command_Turn(string _direction) {

            int pos = 0; // Position of the rotatable Mino
            if(tetromino[0].Y > tetromino[1].Y) pos = 1; // Up
            if(tetromino[0].X > tetromino[1].X) pos = 2; // Left
            if(tetromino[0].Y < tetromino[1].Y) pos = 3; // Down
            if(tetromino[0].X < tetromino[1].X) pos = 4; // Right
            
            

            if(_direction == "left") {
                if(pos == 1) { if(tetromino[0].X - 1 >=  0 && !grid_base[(int)tetromino[0].X - 1, (int)tetromino[0].Y    ]) { tetromino[1] = new Vector2(tetromino[0].X - 1, tetromino[0].Y    ); } }
                if(pos == 2) { if(tetromino[0].Y + 1 <= 19 && !grid_base[(int)tetromino[0].X    , (int)tetromino[0].Y + 1]) { tetromino[1] = new Vector2(tetromino[0].X    , tetromino[0].Y + 1); } }
                if(pos == 3) { if(tetromino[0].X + 1 <=  9 && !grid_base[(int)tetromino[0].X + 1, (int)tetromino[0].Y    ]) { tetromino[1] = new Vector2(tetromino[0].X + 1, tetromino[0].Y    ); } }
                if(pos == 4) { if(tetromino[0].Y - 1 >=  0 && !grid_base[(int)tetromino[0].X    , (int)tetromino[0].Y - 1]) { tetromino[1] = new Vector2(tetromino[0].X    , tetromino[0].Y - 1); } }
            }
            if(_direction == "right") {
                if(pos == 1) { if(tetromino[0].X + 1 <=  9 && !grid_base[(int)tetromino[0].X + 1, (int)tetromino[0].Y    ]) { tetromino[1] = new Vector2(tetromino[0].X + 1, tetromino[0].Y    ); } }
                if(pos == 2) { if(tetromino[0].Y - 1 >=  0 && !grid_base[(int)tetromino[0].X    , (int)tetromino[0].Y - 1]) { tetromino[1] = new Vector2(tetromino[0].X    , tetromino[0].Y - 1); } }
                if(pos == 3) { if(tetromino[0].X - 1 >=  0 && !grid_base[(int)tetromino[0].X - 1, (int)tetromino[0].Y    ]) { tetromino[1] = new Vector2(tetromino[0].X - 1, tetromino[0].Y    ); } }
                if(pos == 4) { if(tetromino[0].Y + 1 <= 19 && !grid_base[(int)tetromino[0].X    , (int)tetromino[0].Y + 1]) { tetromino[1] = new Vector2(tetromino[0].X    , tetromino[0].Y + 1); } }
            }

        }

        private void Command_Hold() {
            if(0 < FM.purchased[FM.Convert("hold")]) {
                if(active_hold) {
                    active_hold = false;
                    if(container_hold[0] == "empty") {
                        container_hold = container_current;
                        container_hold = container_current;
                        container_current = container_next;
                        container_current = container_next;
                        container_next[0] = Domino_Roll();
                        container_next[1] = Domino_Roll();
                    } else {
                        string[] temp = new string[2];
                        temp[0] = container_hold[0];
                        temp[1] = container_hold[1];
                        container_hold[0] = container_current[0];
                        container_hold[1] = container_current[1];
                        container_current[0] = temp[0];
                        container_current[1] = temp[1];
                    }
                    Domino_Create();
                }
            }
        }

        private string Domino_Roll() {
            return IntToMino(random.Next(8));
        }

        private void Domino_Create() {
            tetromino[0] = new Vector2(4, 1);
            tetromino[1] = new Vector2(4, 0);
        }

        private void Domino_Fall() {
            if(tetromino[0].Y < 19 && tetromino[1].Y < 19) {
                if(!grid_base[(int)tetromino[0].X, (int)tetromino[0].Y + 1] && !grid_base[(int)tetromino[1].X, (int)tetromino[1].Y + 1]) {
                    tetromino[0].Y = tetromino[0].Y + 1;
                    tetromino[1].Y = tetromino[1].Y + 1;
                } else {
                    Domino_Place();
                }
            } else {
                Domino_Place();
            }
        }

        private void Domino_Place() {
            JK.Noise("Place");
            active_hold = true;
            grid_base[(int)tetromino[0].X, (int)tetromino[0].Y] = true;
            grid_base[(int)tetromino[1].X, (int)tetromino[1].Y] = true;
            grid_color[(int)tetromino[0].X, (int)tetromino[0].Y] = container_current[0];
            grid_color[(int)tetromino[1].X, (int)tetromino[1].Y] = container_current[1];
            score_points = score_points + 2 * game_difficulty;
            if(tetromino[1].Y == 0) won = true;
            container_current[0] = container_next[0];
            container_current[1] = container_next[1];
            container_next[0] = Domino_Roll();
            container_next[1] = Domino_Roll();
            Domino_Create();
            Check_Field();
        }

        List<Vector2> clear_temp = new List<Vector2>();

        private void Check_Field() {
            int points = 0;
            int bonus = 0;
            clear_temp.Clear();
            for(int y = 19; y >= 0; y--) {
                for(int x = 0; x < 10; x++) {
                    if(!IsCleared(x, y)) {
                        Pathfinder(x, y);
                        if(clear_temp.Count >= 4) {
                            points += (clear_temp.Count * 10);
                            bonus++;
                            clear.AddRange(clear_temp);
                        }
                        clear_temp.Clear();
                    }
                }
            }

            if(points > 0) {
                alpha -= 5;
                score_points += (points * game_difficulty * bonus * (score_level + 1));
                score_lives++;
                if(score_lives > (1 + score_level) * 10) {
                    score_level++;
                }
                //Command_Collapse();
            }
        }

        private void Pathfinder(int x, int y) {
            clear_temp.Add(new Vector2(x, y));
            if(y - 1 >=  0 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x    , y - 1] && !IsClearedTemp(x    , y - 1)) Pathfinder(x    , y - 1);
            if(y + 1 <= 19 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x    , y + 1] && !IsClearedTemp(x    , y + 1)) Pathfinder(x    , y + 1);
            if(x - 1 >=  0 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x - 1, y    ] && !IsClearedTemp(x - 1, y    )) Pathfinder(x - 1, y    );
            if(x + 1 <=  9 && grid_color[x, y] != "empty" && grid_color[x, y] == grid_color[x + 1, y    ] && !IsClearedTemp(x + 1, y    )) Pathfinder(x + 1, y    );
        }

        private bool IsClearedTemp(int x, int y) {
            foreach(Vector2 v in clear_temp) {
                if(v == new Vector2(x, y)) return true;
            }
            return false;
        }

    }
}
