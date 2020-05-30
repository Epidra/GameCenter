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
    class Tetris : Ghost {

        bool active_hold;

        string container_next;
        string container_hold;
        string container_current;

        double time_last;
        double time_break;

        string[] tetro = new string[7];

        bool[,] grid_base  = new   bool[10,20];
        string[,] grid_color = new string[10,20];

        Vector2[] tetromino = new Vector2[4];

        bool won;

        int line1;
        int line2;
        int line3;
        int line4;

        int alpha;

        public Tetris(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            container_next = "empty";
            container_hold = "empty";
            container_current = "empty";
            container_current = Tetromino_Roll();
            container_next = Tetromino_Roll();
            Tetromino_Create();
            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < 20; j++) {
                    grid_base[i, j] = false;
                    grid_color[i, j] = "empty";
                }
            }
            time_break = 250 - 25 * game_difficulty;
            time_last = 0;
            won = false;
            line1 = -1;
            line1 = -1;
            line1 = -1;
            line1 = -1;
            alpha = 255;
            Colorize();
        }

        private void Colorize() {
            bool[] used = new bool[10];
            int index = 0;
            for(int i = 0; i < 10; i++) {
                used[i] = false;
                if(FM.minos[i + 1]) index++;
            }

            tetro[0] = "blank";
            tetro[1] = "blank";
            tetro[2] = "blank";
            tetro[3] = "blank";
            tetro[4] = "blank";
            tetro[5] = "blank";
            tetro[6] = "blank";

            int count = 0;
            while(count < index && count < 7) {
                int r = random.Next(10);
                if(!used[r] && FM.minos[r + 1]) {
                    tetro[count] = IntToMino(r);
                    used[r] = true;
                    count++;
                }
            }
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

        private string TetroToMino(string t) {
            if(t == "I") return tetro[0];
            if(t == "O") return tetro[1];
            if(t == "S") return tetro[2];
            if(t == "Z") return tetro[3];
            if(t == "L") return tetro[4];
            if(t == "J") return tetro[5];
            if(t == "T") return tetro[6];
            return "blank";
        }

        public override string Update2() {
            if(alpha == 255) {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { Command_Hold(); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) { Command_Hold(); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { Tetromino_Drop(); }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.hold) { Tetromino_Fall(); }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { Command_Strafe("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { Command_Strafe("right"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_left_P1) == GhostState.pressed) { Command_Turn("right"); pressed_response = true; }
                if(ButtonPressed(GhostKey.button_right_P1) == GhostState.pressed) { Command_Turn("left"); pressed_response = true; }
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
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(alpha == 255) {
                if(gameTime.TotalGameTime.TotalMilliseconds > time_last + time_break - score_level * 5 * game_difficulty && !active_pause && !active_gameover && !FM.active_transition) {
                    Tetromino_Fall();
                    time_last = gameTime.TotalGameTime.TotalMilliseconds;
                }

                if(won && !active_gameover) {
                    GameOver(gameTime.TotalGameTime.TotalSeconds);
                }
            } else {
                alpha -= 10;
                if(alpha <= 0) {
                    alpha = 255;
                    Command_Collapse();
                }
            }
            return "void";
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
                        if(y == line1 || y == line2 || y == line3 || y == line4) {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + new Vector2(32 * x, 32 * y), Get_Mino_Texture(TetroToMino(grid_color[x, y]), 0, 32), new Color(alpha, alpha, alpha));
                        } else {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + new Vector2(32 * x, 32 * y), Get_Mino_Texture(TetroToMino(grid_color[x, y]), 0, 32), Color.White);
                        }
                    }
                }
            }

            if(alpha == 255) {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[0] * 32, Get_Mino_Texture(TetroToMino(container_current), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[1] * 32, Get_Mino_Texture(TetroToMino(container_current), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[2] * 32, Get_Mino_Texture(TetroToMino(container_current), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, SK.Position_DisplayEdge() + SK.Position_Tetris_Field() + tetromino[3] * 32, Get_Mino_Texture(TetroToMino(container_current), 0, 32), Color.White);
            }

            if(container_next != "empty")
                Get_Tetromino(SK.Position_DisplayEdge() + SK.Position_Tetris_Next() + new Vector2(16, 16), container_next);
            if(container_hold != "empty" && 0 < FM.purchased[FM.Convert("hold")])
                Get_Tetromino(SK.Position_DisplayEdge() + SK.Position_Tetris_Hold() + new Vector2(16, 16), container_hold);
        }

        private void Get_Tetromino(Vector2 position, string tetro) {
            if(tetro == "I") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 0), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 96), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "O") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "S") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(80, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(16, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "Z") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(16, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(80, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "L") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 16), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 48), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 80), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 80), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "J") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 16), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 48), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(64, 80), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(32, 80), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
            if(tetro == "T") {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(16, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(80, 32), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, position + new Vector2(48, 64), Get_Mino_Texture(TetroToMino(tetro), 0, 32), Color.White);
            }
        }

        private void Command_Collapse() {
            if(line4 != -1) {
                for(int i = line4; i > 0; i--) {
                    grid_base[0, i] = grid_base[0, i - 1];
                    grid_base[1, i] = grid_base[1, i - 1];
                    grid_base[2, i] = grid_base[2, i - 1];
                    grid_base[3, i] = grid_base[3, i - 1];
                    grid_base[4, i] = grid_base[4, i - 1];
                    grid_base[5, i] = grid_base[5, i - 1];
                    grid_base[6, i] = grid_base[6, i - 1];
                    grid_base[7, i] = grid_base[7, i - 1];
                    grid_base[8, i] = grid_base[8, i - 1];
                    grid_base[9, i] = grid_base[9, i - 1];
                    grid_color[0, i] = grid_color[0, i - 1];
                    grid_color[1, i] = grid_color[1, i - 1];
                    grid_color[2, i] = grid_color[2, i - 1];
                    grid_color[3, i] = grid_color[3, i - 1];
                    grid_color[4, i] = grid_color[4, i - 1];
                    grid_color[5, i] = grid_color[5, i - 1];
                    grid_color[6, i] = grid_color[6, i - 1];
                    grid_color[7, i] = grid_color[7, i - 1];
                    grid_color[8, i] = grid_color[8, i - 1];
                    grid_color[9, i] = grid_color[9, i - 1];
                }
            }
            if(line3 != -1) {
                for(int i = line3; i > 0; i--) {
                    grid_base[0, i] = grid_base[0, i - 1];
                    grid_base[1, i] = grid_base[1, i - 1];
                    grid_base[2, i] = grid_base[2, i - 1];
                    grid_base[3, i] = grid_base[3, i - 1];
                    grid_base[4, i] = grid_base[4, i - 1];
                    grid_base[5, i] = grid_base[5, i - 1];
                    grid_base[6, i] = grid_base[6, i - 1];
                    grid_base[7, i] = grid_base[7, i - 1];
                    grid_base[8, i] = grid_base[8, i - 1];
                    grid_base[9, i] = grid_base[9, i - 1];
                    grid_color[0, i] = grid_color[0, i - 1];
                    grid_color[1, i] = grid_color[1, i - 1];
                    grid_color[2, i] = grid_color[2, i - 1];
                    grid_color[3, i] = grid_color[3, i - 1];
                    grid_color[4, i] = grid_color[4, i - 1];
                    grid_color[5, i] = grid_color[5, i - 1];
                    grid_color[6, i] = grid_color[6, i - 1];
                    grid_color[7, i] = grid_color[7, i - 1];
                    grid_color[8, i] = grid_color[8, i - 1];
                    grid_color[9, i] = grid_color[9, i - 1];
                }
            }
            if(line2 != -1) {
                for(int i = line2; i > 0; i--) {
                    grid_base[0, i] = grid_base[0, i - 1];
                    grid_base[1, i] = grid_base[1, i - 1];
                    grid_base[2, i] = grid_base[2, i - 1];
                    grid_base[3, i] = grid_base[3, i - 1];
                    grid_base[4, i] = grid_base[4, i - 1];
                    grid_base[5, i] = grid_base[5, i - 1];
                    grid_base[6, i] = grid_base[6, i - 1];
                    grid_base[7, i] = grid_base[7, i - 1];
                    grid_base[8, i] = grid_base[8, i - 1];
                    grid_base[9, i] = grid_base[9, i - 1];
                    grid_color[0, i] = grid_color[0, i - 1];
                    grid_color[1, i] = grid_color[1, i - 1];
                    grid_color[2, i] = grid_color[2, i - 1];
                    grid_color[3, i] = grid_color[3, i - 1];
                    grid_color[4, i] = grid_color[4, i - 1];
                    grid_color[5, i] = grid_color[5, i - 1];
                    grid_color[6, i] = grid_color[6, i - 1];
                    grid_color[7, i] = grid_color[7, i - 1];
                    grid_color[8, i] = grid_color[8, i - 1];
                    grid_color[9, i] = grid_color[9, i - 1];
                }
            }
            if(line1 != -1) {
                for(int i = line1; i > 0; i--) {
                    grid_base[0, i] = grid_base[0, i - 1];
                    grid_base[1, i] = grid_base[1, i - 1];
                    grid_base[2, i] = grid_base[2, i - 1];
                    grid_base[3, i] = grid_base[3, i - 1];
                    grid_base[4, i] = grid_base[4, i - 1];
                    grid_base[5, i] = grid_base[5, i - 1];
                    grid_base[6, i] = grid_base[6, i - 1];
                    grid_base[7, i] = grid_base[7, i - 1];
                    grid_base[8, i] = grid_base[8, i - 1];
                    grid_base[9, i] = grid_base[9, i - 1];
                    grid_color[0, i] = grid_color[0, i - 1];
                    grid_color[1, i] = grid_color[1, i - 1];
                    grid_color[2, i] = grid_color[2, i - 1];
                    grid_color[3, i] = grid_color[3, i - 1];
                    grid_color[4, i] = grid_color[4, i - 1];
                    grid_color[5, i] = grid_color[5, i - 1];
                    grid_color[6, i] = grid_color[6, i - 1];
                    grid_color[7, i] = grid_color[7, i - 1];
                    grid_color[8, i] = grid_color[8, i - 1];
                    grid_color[9, i] = grid_color[9, i - 1];
                }
            }
            line1 = -1;
            line2 = -1;
            line3 = -1;
            line4 = -1;
            alpha = 255;
            JK.Noise("Explosion");
        }

        private void Command_Strafe(string _direction) {
            int dir = 0;
            if(_direction == "left") {
                dir = -1;
                if(tetromino[0].X > 0) {
                    if(tetromino[1].X > 0) {
                        if(tetromino[2].X > 0) {
                            if(tetromino[3].X > 0) {
                                if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                                    if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                        if(!grid_base[(int)tetromino[2].X + dir, (int)tetromino[2].Y]) {
                                            if(!grid_base[(int)tetromino[3].X + dir, (int)tetromino[3].Y]) {
                                                tetromino[0].X = tetromino[0].X + dir;
                                                tetromino[1].X = tetromino[1].X + dir;
                                                tetromino[2].X = tetromino[2].X + dir;
                                                tetromino[3].X = tetromino[3].X + dir;
                                            }
                                        }
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
                            if(tetromino[3].X < 9) {
                                if(!grid_base[(int)tetromino[0].X + dir, (int)tetromino[0].Y]) {
                                    if(!grid_base[(int)tetromino[1].X + dir, (int)tetromino[1].Y]) {
                                        if(!grid_base[(int)tetromino[2].X + dir, (int)tetromino[2].Y]) {
                                            if(!grid_base[(int)tetromino[3].X + dir, (int)tetromino[3].Y]) {
                                                tetromino[0].X = tetromino[0].X + dir;
                                                tetromino[1].X = tetromino[1].X + dir;
                                                tetromino[2].X = tetromino[2].X + dir;
                                                tetromino[3].X = tetromino[3].X + dir;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Command_Turn(string _direction) {
            Vector2 tempV1 = tetromino[0] - tetromino[1];
            Vector2 tempV2 = tetromino[0] - tetromino[2];
            Vector2 tempV3 = tetromino[0] - tetromino[3];
            if(_direction == "left") {
                if(tempV1.X == 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y); } else
                if(tempV1.X == 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y); } else
                if(tempV1.X < 0 && tempV1.Y == 0) { tempV1 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y == 0) { tempV1 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV1.X); } else
                if(tempV1.X < 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y - tempV1.X); } else
                if(tempV1.X < 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y - tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y - tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X + tempV1.Y, tetromino[0].Y - tempV1.X); }

                if(tempV2.X == 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y); } else
                if(tempV2.X == 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y); } else
                if(tempV2.X < 0 && tempV2.Y == 0) { tempV2 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y == 0) { tempV2 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV2.X); } else
                if(tempV2.X < 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y - tempV2.X); } else
                if(tempV2.X < 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y - tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y - tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X + tempV2.Y, tetromino[0].Y - tempV2.X); }

                if(tempV3.X == 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y); } else
                if(tempV3.X == 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y); } else
                if(tempV3.X < 0 && tempV3.Y == 0) { tempV3 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y == 0) { tempV3 = new Vector2(tetromino[0].X, tetromino[0].Y - tempV3.X); } else
                if(tempV3.X < 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y - tempV3.X); } else
                if(tempV3.X < 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y - tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y - tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X + tempV3.Y, tetromino[0].Y - tempV3.X); }
            }
            if(_direction == "right") {
                if(tempV1.X == 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y); } else
                if(tempV1.X == 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y); } else
                if(tempV1.X < 0 && tempV1.Y == 0) { tempV1 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y == 0) { tempV1 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV1.X); } else
                if(tempV1.X < 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y + tempV1.X); } else
                if(tempV1.X < 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y + tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y < 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y + tempV1.X); } else
                if(tempV1.X > 0 && tempV1.Y > 0) { tempV1 = new Vector2(tetromino[0].X - tempV1.Y, tetromino[0].Y + tempV1.X); }

                if(tempV2.X == 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y); } else
                if(tempV2.X == 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y); } else
                if(tempV2.X < 0 && tempV2.Y == 0) { tempV2 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y == 0) { tempV2 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV2.X); } else
                if(tempV2.X < 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y + tempV2.X); } else
                if(tempV2.X < 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y + tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y < 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y + tempV2.X); } else
                if(tempV2.X > 0 && tempV2.Y > 0) { tempV2 = new Vector2(tetromino[0].X - tempV2.Y, tetromino[0].Y + tempV2.X); }

                if(tempV3.X == 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y); } else
                if(tempV3.X == 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y); } else
                if(tempV3.X < 0 && tempV3.Y == 0) { tempV3 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y == 0) { tempV3 = new Vector2(tetromino[0].X, tetromino[0].Y + tempV3.X); } else
                if(tempV3.X < 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y + tempV3.X); } else
                if(tempV3.X < 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y + tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y < 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y + tempV3.X); } else
                if(tempV3.X > 0 && tempV3.Y > 0) { tempV3 = new Vector2(tetromino[0].X - tempV3.Y, tetromino[0].Y + tempV3.X); }
            }
            if(tempV1.X > -1 && tempV1.X < 10 && tempV1.Y > -1 && tempV1.Y < 20) {
                if(tempV2.X > -1 && tempV2.X < 10 && tempV2.Y > -1 && tempV2.Y < 20) {
                    if(tempV3.X > -1 && tempV3.X < 10 && tempV3.Y > -1 && tempV3.Y < 20) {
                        if(!grid_base[(int)tempV1.X, (int)tempV1.Y]) {
                            if(!grid_base[(int)tempV2.X, (int)tempV2.Y]) {
                                if(!grid_base[(int)tempV3.X, (int)tempV3.Y]) {
                                    tetromino[1] = tempV1;
                                    tetromino[2] = tempV2;
                                    tetromino[3] = tempV3;
                                }
                            }
                        }
                    }
                }
            }

        }

        private void Command_Hold() {
            if(0 < FM.purchased[FM.Convert("hold")]) {
                if(active_hold) {
                    active_hold = false;
                    if(container_hold == "empty") {
                        container_hold = container_current;
                        container_current = container_next;
                        container_next = Tetromino_Roll();
                    } else {
                        string temp;
                        temp = container_hold;
                        container_hold = container_current;
                        container_current = temp;
                    }
                    Tetromino_Create();
                }
            }
        }

        private string Tetromino_Roll() {
            int temp = random.Next(7);
            if(temp == 0) return "I";
            if(temp == 1) return "O";
            if(temp == 2) return "S";
            if(temp == 3) return "Z";
            if(temp == 4) return "L";
            if(temp == 5) return "J";
            if(temp == 6) return "T";
            return "O";
        }

        private void Tetromino_Create() {
            if(container_current == "I") {
                tetromino[0] = new Vector2(4, 1); // OOXO
                tetromino[1] = new Vector2(4, 0); // OOXO
                tetromino[2] = new Vector2(4, 2); // OOXO
                tetromino[3] = new Vector2(4, 3); // OOXO
            }
            if(container_current == "O") {
                tetromino[0] = new Vector2(4, 0); // OOOO
                tetromino[1] = new Vector2(4, 1); // OXXO
                tetromino[2] = new Vector2(5, 0); // OXXO
                tetromino[3] = new Vector2(5, 1); // OOOO
            }
            if(container_current == "S") {
                tetromino[0] = new Vector2(5, 0); // OOOO
                tetromino[1] = new Vector2(6, 0); // OOXX
                tetromino[2] = new Vector2(4, 1); // OXXO
                tetromino[3] = new Vector2(5, 1); // OOOO
            }
            if(container_current == "Z") {
                tetromino[0] = new Vector2(5, 0); // OOOO
                tetromino[1] = new Vector2(4, 0); // XXOO
                tetromino[2] = new Vector2(5, 1); // OXXO
                tetromino[3] = new Vector2(6, 1); // OOOO
            }
            if(container_current == "L") {
                tetromino[0] = new Vector2(4, 2); // OXOO
                tetromino[1] = new Vector2(4, 0); // OXOO
                tetromino[2] = new Vector2(4, 1); // OXXO
                tetromino[3] = new Vector2(5, 2); // OOOO
            }
            if(container_current == "J") {
                tetromino[0] = new Vector2(5, 2); // OOXO
                tetromino[1] = new Vector2(5, 0); // OOXO
                tetromino[2] = new Vector2(5, 1); // OXXO
                tetromino[3] = new Vector2(4, 2); // OOOO
            }
            if(container_current == "T") {
                tetromino[0] = new Vector2(5, 0); // OOOO
                tetromino[1] = new Vector2(4, 0); // OXXX
                tetromino[2] = new Vector2(6, 0); // OOXO
                tetromino[3] = new Vector2(5, 1); // OOOO
            }
        }

        private void Tetromino_Drop() {
            int tempPoints = score_points;
            while(score_points == tempPoints) {
                Tetromino_Fall();
            }
        }

        private void Tetromino_Fall() {
            if(tetromino[0].Y < 19 && tetromino[1].Y < 19 && tetromino[2].Y < 19 && tetromino[3].Y < 19) {
                if(!grid_base[(int)tetromino[0].X, (int)tetromino[0].Y + 1] && !grid_base[(int)tetromino[1].X, (int)tetromino[1].Y + 1] && !grid_base[(int)tetromino[2].X, (int)tetromino[2].Y + 1] && !grid_base[(int)tetromino[3].X, (int)tetromino[3].Y + 1]) {
                    tetromino[0].Y = tetromino[0].Y + 1;
                    tetromino[1].Y = tetromino[1].Y + 1;
                    tetromino[2].Y = tetromino[2].Y + 1;
                    tetromino[3].Y = tetromino[3].Y + 1;
                } else {
                    Tetromino_Place();
                }
            } else {
                Tetromino_Place();
            }
        }

        private void Tetromino_Place() {
            JK.Noise("Place");
            active_hold = true;
            grid_base[(int)tetromino[0].X, (int)tetromino[0].Y] = true;
            grid_base[(int)tetromino[1].X, (int)tetromino[1].Y] = true;
            grid_base[(int)tetromino[2].X, (int)tetromino[2].Y] = true;
            grid_base[(int)tetromino[3].X, (int)tetromino[3].Y] = true;
            grid_color[(int)tetromino[0].X, (int)tetromino[0].Y] = container_current;
            grid_color[(int)tetromino[1].X, (int)tetromino[1].Y] = container_current;
            grid_color[(int)tetromino[2].X, (int)tetromino[2].Y] = container_current;
            grid_color[(int)tetromino[3].X, (int)tetromino[3].Y] = container_current;
            score_points = score_points + 2 * game_difficulty;
            if(tetromino[0].Y == 0) won = true;
            container_current = container_next;
            string container_temp = container_next;
            if((container_next = Tetromino_Roll()) == container_temp) {
                if(random.Next(2) == 0) {
                    container_next = Tetromino_Roll();
                }
            }
            Tetromino_Create();
            line1 = -1;
            line2 = -1;
            line3 = -1;
            line4 = -1;
            for(int i = 19; i > -1; i--) {
                if(grid_base[0, i] && grid_base[1, i] && grid_base[2, i] && grid_base[3, i] && grid_base[4, i] && grid_base[5, i] && grid_base[6, i] && grid_base[7, i] && grid_base[8, i] && grid_base[9, i]) {
                    score_lives++;
                    score_points += 50 * game_difficulty;
                    if(line1 == -1) {
                        line1 = i;
                    } else if(line2 == -1) {
                        line2 = i;
                    } else if(line3 == -1) {
                        line3 = i;
                    } else if(line4 == -1) {
                        line4 = i;
                    }
                    alpha -= 5;
                }
            }
            if(score_lives > (1 + score_level) * 10) {
                score_level++;
            }
            if(line1 != -1 && line2 != -1 && line3 != -1 && line4 != -1) {
                score_points = score_points + 250 * game_difficulty * (score_level + 1);
            }
        }
    }
}
