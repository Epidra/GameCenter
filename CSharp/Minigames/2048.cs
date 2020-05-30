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
    class _2048 : Ghost {

        int[,] grid      = new  int[4,4];
        bool[,] grid_move = new bool[4,4];

        bool placing;
        bool active_timer;
        int timer;
        string direction;
        bool won;

        public _2048(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            for(int y = 0; y < 4; y++) {
                for(int x = 0; x < 4; x++) {
                    grid[x, y] = 0;
                    grid_move[x, y] = false;
                }
            }
            placing = false;
            grid[0, 0] = random.Next(1) + 1;
            grid[3, 0] = random.Next(1) + 1;
            grid[0, 3] = random.Next(1) + 1;
            grid[3, 3] = random.Next(1) + 1;
            active_timer = false;
            timer = 0;
            direction = "null";
            won = false;
        }

        public override string Update2() {
            if(active_timer) {
                timer = timer + 16;
                if(timer == 128) {
                    active_timer = false;
                    timer = 0;
                    Change();
                    Move(direction);
                    placing = true;
                }
            } else {
                if(placing) {
                    Place();
                    placing = false;
                }
                direction = "null";
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { Move("up"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { Move("down"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { Move("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { Move("right"); pressed_response = true; }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(won && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Move(string s) {
            if(s == "up") {
                for(int y = 1; y < 4; y++) {
                    for(int x = 0; x < 4; x++) {
                        if(grid[x, y] != 0) {
                            if(grid[x, y - 1] == 0 || grid[x, y - 1] == grid[x, y]) {
                                grid_move[x, y] = true;
                                active_timer = true;
                                direction = s;
                            }
                        }
                    }
                }
            }
            if(s == "down") {
                for(int y = 2; y > -1; y--) {
                    for(int x = 3; x > -1; x--) {
                        if(grid[x, y] != 0) {
                            if(grid[x, y + 1] == 0 || grid[x, y + 1] == grid[x, y]) {
                                grid_move[x, y] = true;
                                active_timer = true;
                                direction = s;
                            }
                        }
                    }
                }
            }
            if(s == "left") {
                for(int x = 1; x < 4; x++) {
                    for(int y = 0; y < 4; y++) {
                        if(grid[x, y] != 0) {
                            if(grid[x - 1, y] == 0 || grid[x - 1, y] == grid[x, y]) {
                                grid_move[x, y] = true;
                                active_timer = true;
                                direction = s;
                            }
                        }
                    }
                }
            }
            if(s == "right") {
                for(int x = 2; x > -1; x--) {
                    for(int y = 3; y > -1; y--) {
                        if(grid[x, y] != 0) {
                            if(grid[x + 1, y] == 0 || grid[x + 1, y] == grid[x, y]) {
                                grid_move[x, y] = true;
                                active_timer = true;
                                direction = s;
                            }
                        }
                    }
                }
            }
        }

        private void Change() {
            if(direction == "up") {
                for(int y = 1; y < 4; y++) {
                    for(int x = 0; x < 4; x++) {
                        if(grid_move[x, y]) {
                            if(grid[x, y - 1] == 0) {
                                grid[x, y - 1] = grid[x, y];
                            } else {
                                grid[x, y - 1]++;
                                Add_Points(grid[x, y - 1]);
                            }
                            grid[x, y] = 0;
                        }
                    }
                }
            }
            if(direction == "down") {
                for(int y = 2; y > -1; y--) {
                    for(int x = 3; x > -1; x--) {
                        if(grid_move[x, y]) {
                            if(grid[x, y + 1] == 0) {
                                grid[x, y + 1] = grid[x, y];
                            } else {
                                grid[x, y + 1]++;
                                Add_Points(grid[x, y + 1]);
                            }
                            grid[x, y] = 0;
                        }
                    }
                }
            }
            if(direction == "left") {
                for(int x = 1; x < 4; x++) {
                    for(int y = 0; y < 4; y++) {
                        if(grid_move[x, y]) {
                            if(grid[x - 1, y] == 0) {
                                grid[x - 1, y] = grid[x, y];
                            } else {
                                grid[x - 1, y]++;
                                Add_Points(grid[x - 1, y]);
                            }
                            grid[x, y] = 0;
                        }
                    }
                }
            }
            if(direction == "right") {
                for(int x = 2; x > -1; x--) {
                    for(int y = 3; y > -1; y--) {
                        if(grid_move[x, y]) {
                            if(grid[x + 1, y] == 0) {
                                grid[x + 1, y] = grid[x, y];
                            } else {
                                grid[x + 1, y]++;
                                Add_Points(grid[x + 1, y]);
                            }
                            grid[x, y] = 0;
                        }
                    }
                }
            }
            for(int y = 0; y < 4; y++) {
                for(int x = 0; x < 4; x++) {
                    grid_move[x, y] = false;
                }
            }
        }

        private void Place() {
            for(int i = 0; i < 24; i++) {
                int x = random.Next(4);
                int y = random.Next(4);
                if(grid[x, y] == 0) {
                    grid[x, y] = 1;
                    break;
                }
            }
            Check();
        }

        private void Check() {
            bool b = false;
            for(int y = 1; y < 4; y++) {
                for(int x = 0; x < 4; x++) {
                    if(grid[x, y] != 0) {
                        if(grid[x, y - 1] == 0 || grid[x, y - 1] == grid[x, y]) {
                            b = true;
                            break;
                        }
                    }
                }
                if(b) break;
            }
            for(int y = 2; y > -1; y--) {
                for(int x = 3; x > -1; x--) {
                    if(grid[x, y] != 0) {
                        if(grid[x, y + 1] == 0 || grid[x, y + 1] == grid[x, y]) {
                            b = true;
                            break;
                        }
                    }
                }
                if(b) break;
            }
            for(int x = 1; x < 4; x++) {
                for(int y = 0; y < 4; y++) {
                    if(grid[x, y] != 0) {
                        if(grid[x - 1, y] == 0 || grid[x - 1, y] == grid[x, y]) {
                            b = true;
                            break;
                        }
                    }
                }
                if(b) break;
            }
            for(int x = 2; x > -1; x--) {
                for(int y = 3; y > -1; y--) {
                    if(grid[x, y] != 0) {
                        if(grid[x + 1, y] == 0 || grid[x + 1, y] == grid[x, y]) {
                            b = true;
                            break;
                        }
                    }
                }
                if(b) break;
            }
            if(!b) {
                won = true;
            }
        }

        private int Get_Direction(bool horizontal, int x, int y) {
            if(direction == "null")
                return 0;
            if(horizontal && direction == "left") if(grid_move[x, y]) return -timer;
            if(horizontal && direction == "right") if(grid_move[x, y]) return timer;
            if(!horizontal && direction == "up") if(grid_move[x, y]) return -timer;
            if(!horizontal && direction == "down") if(grid_move[x, y]) return timer;
            return 0;
        }

        private int Get_Spritesheet(bool x, int id) {
            if(x) {
                if(id == 1) return 0;
                if(id == 2) return 1;
                if(id == 3) return 2;
                if(id == 4) return 3;
                if(id == 5) return 0;
                if(id == 6) return 1;
                if(id == 7) return 2;
                if(id == 8) return 3;
                if(id == 9) return 0;
                if(id == 10) return 1;
                if(id == 11) return 2;
                if(id == 12) return 3;
            } else {
                if(id == 1) return 0;
                if(id == 2) return 0;
                if(id == 3) return 0;
                if(id == 4) return 0;
                if(id == 5) return 1;
                if(id == 6) return 1;
                if(id == 7) return 1;
                if(id == 8) return 1;
                if(id == 9) return 2;
                if(id == 10) return 2;
                if(id == 11) return 2;
                if(id == 12) return 2;
            }
            return 0;
        }

        private void Add_Points(int i) {
            if(i == 1) score_points = score_points + 2;
            if(i == 2) score_points = score_points + 4;
            if(i == 3) score_points = score_points + 8;
            if(i == 4) score_points = score_points + 16;
            if(i == 5) score_points = score_points + 32;
            if(i == 6) score_points = score_points + 64;
            if(i == 7) score_points = score_points + 128;
            if(i == 8) score_points = score_points + 256;
            if(i == 9) score_points = score_points + 512;
            if(i == 10) score_points = score_points + 1024;
            if(i == 11) score_points = score_points + 2048;
            if(i == 12) score_points = score_points + 4096;
            if(i == 13) score_points = score_points + 8192;
            if(i == 14) score_points = score_points + 16384;
            if(i == 15) score_points = score_points + 32768;
            if(i == 16) score_points = score_points + 65536;
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_2048, SK.Position_DisplayEdge() + SK.Position_2048(), Color.White);
            for(int y = 0; y < 4; y++) {
                for(int x = 0; x < 4; x++) {
                    if(grid[x, y] != 0) {
                        spriteBatch.Draw(SK.texture_spritesheet_2048, new Vector2(SK.Position_DisplayEdge().X + 19 + SK.Position_2048().X + 128 * x + Get_Direction(true, x, y), SK.Position_DisplayEdge().Y + 19 + SK.Position_2048().Y + 128 * y + (Get_Direction(false, x, y))), new Rectangle((128 * Get_Spritesheet(true, grid[x, y])), (128 * Get_Spritesheet(false, grid[x, y])), 128, 128), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

    }
}
