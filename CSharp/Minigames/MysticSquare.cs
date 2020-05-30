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
    class MysticSquare : Ghost {

        int[,] grid      = new  int[4,4];
        bool[,] grid_move = new bool[4,4];

        bool active_timer;
        int timer;
        string direction;

        bool match;

        public MysticSquare(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
                    grid[x, y] = -1;
                    grid_move[x, y] = false;
                }
            }
            int i = 0;
            while(i < 15) {
                int x = random.Next(4);
                int y = random.Next(4);
                if(grid[x, y] == -1) {
                    grid[x, y] = i;
                    i++;
                }
            }
            active_timer = false;
            match = false;
            timer = 0;
            direction = "null";
        }

        public override string Update2() {
            if(active_timer) {
                timer = timer + 16;
                if(timer == 128) {
                    active_timer = false;
                    timer = 0;
                    Change();
                    Check();
                }
            } else {
                direction = "null";
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { Move("up"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { Move("down"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { Move("left"); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { Move("right"); pressed_response = true; }
            }
            if(pressed_event_touch) {
                for(int y = 0; y < 4; y++) {
                    for(int x = 0; x < 4; x++) {
                        if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + 8 + SK.Position_2048().X + 128 * x), (int)(SK.Position_DisplayEdge().Y + 8 + SK.Position_2048().Y + 128 * y), 128, 128))) {
                            if(y > 0) if(grid[x, y - 1] == -1) Move("up");
                            if(y < 3) if(grid[x, y + 1] == -1) Move("down");
                            if(x > 0) if(grid[x - 1, y] == -1) Move("left");
                            if(x < 3) if(grid[x + 1, y] == -1) Move("right");
                        }
                    }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(match && !active_gameover) {
                JK.Noise("Cleared");
                score_points = 1;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Move(string s) {
            for(int x = 0; x < 4; x++) {
                for(int y = 0; y < 4; y++) {
                    if(s == "up" && y > 0) if(grid[x, y - 1] == -1) grid_move[x, y] = true;
                    if(s == "down" && y < 3) if(grid[x, y + 1] == -1) grid_move[x, y] = true;
                    if(s == "left" && x > 0) if(grid[x - 1, y] == -1) grid_move[x, y] = true;
                    if(s == "right" && x < 3) if(grid[x + 1, y] == -1) grid_move[x, y] = true;
                    if(grid_move[x, y]) {
                        active_timer = true;
                        direction = s;
                    }
                }
            }
        }

        private void Change() {
            for(int x = 0; x < 4; x++) {
                for(int y = 0; y < 4; y++) {
                    if(direction == "up" && grid_move[x, y]) { grid[x, y - 1] = grid[x, y]; grid[x, y] = -1; grid_move[x, y] = false; }
                    if(direction == "down" && grid_move[x, y]) { grid[x, y + 1] = grid[x, y]; grid[x, y] = -1; grid_move[x, y] = false; }
                    if(direction == "left" && grid_move[x, y]) { grid[x - 1, y] = grid[x, y]; grid[x, y] = -1; grid_move[x, y] = false; }
                    if(direction == "right" && grid_move[x, y]) { grid[x + 1, y] = grid[x, y]; grid[x, y] = -1; grid_move[x, y] = false; }
                }
            }
        }

        private void Check() {
            match = false;
            if(grid[0, 0] == 0)
                if(grid[1, 0] == 1)
                    if(grid[2, 0] == 2)
                        if(grid[3, 0] == 3)
                            if(grid[0, 1] == 4)
                                if(grid[1, 1] == 5)
                                    if(grid[2, 1] == 6)
                                        if(grid[3, 1] == 7)
                                            if(grid[0, 2] == 8)
                                                if(grid[1, 2] == 9)
                                                    if(grid[2, 2] == 10)
                                                        if(grid[3, 2] == 11)
                                                            if(grid[0, 3] == 12)
                                                                if(grid[1, 3] == 13)
                                                                    if(grid[2, 3] == 14)
                                                                        if(grid[3, 3] == -1)
                                                                            match = true;
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
                if(id == 0) return 0;
                if(id == 1) return 1;
                if(id == 2) return 2;
                if(id == 3) return 3;
                if(id == 4) return 0;
                if(id == 5) return 1;
                if(id == 6) return 2;
                if(id == 7) return 3;
                if(id == 8) return 0;
                if(id == 9) return 1;
                if(id == 10) return 2;
                if(id == 11) return 3;
                if(id == 12) return 0;
                if(id == 13) return 1;
                if(id == 14) return 2;
                if(id == -1) return 3;
            } else {
                if(id == 0) return 0;
                if(id == 1) return 0;
                if(id == 2) return 0;
                if(id == 3) return 0;
                if(id == 4) return 1;
                if(id == 5) return 1;
                if(id == 6) return 1;
                if(id == 7) return 1;
                if(id == 8) return 2;
                if(id == 9) return 2;
                if(id == 10) return 2;
                if(id == 11) return 2;
                if(id == 12) return 3;
                if(id == 13) return 3;
                if(id == 14) return 3;
                if(id == -1) return 3;
            }
            return 0;
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_2048, SK.Position_DisplayEdge() + SK.Position_2048(), Color.White);
            for(int y = 0; y < 4; y++) {
                for(int x = 0; x < 4; x++) {
                    if(grid[x, y] == -1) {
                        if(active_gameover) spriteBatch.Draw(SK.texture_spritesheet_2048, new Vector2(SK.Position_DisplayEdge().X + 8 + SK.Position_2048().X + 128 * x, SK.Position_DisplayEdge().Y + 8 + SK.Position_2048().Y + 128 * y), new Rectangle(1 + (65 * Get_Spritesheet(true, grid[x, y])), 1 + (65 * Get_Spritesheet(false, grid[x, y])), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 2, SpriteEffects.None, 0.0f);
                    } else {
                        spriteBatch.Draw(SK.texture_spritesheet_mysticsquare, new Vector2(SK.Position_DisplayEdge().X + 8 + SK.Position_2048().X + 128 * x + Get_Direction(true, x, y), SK.Position_DisplayEdge().Y + 8 + SK.Position_2048().Y + 128 * y + (Get_Direction(false, x, y))), new Rectangle(1 + (65 * Get_Spritesheet(true, grid[x, y])), 1 + (65 * Get_Spritesheet(false, grid[x, y])), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 2, SpriteEffects.None, 0.0f);
                    }
                }
            }
        }

    }
}
