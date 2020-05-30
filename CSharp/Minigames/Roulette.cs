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
    class Roulette : Ghost {

        Vector2 selector;

        int[,] grid = new int[27, 7];

        int betmulti = 0;

        float rotation_wheel;
        float rotation_ball;

        bool spinning;
        bool betting;
        bool placing;
        bool end;

        int selector_bet;

        int bet;
        int result;
        int timer;

        public Roulette(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
        private int Get_Bet() {
            switch(betmulti) {
                case 0: return 1;
                case 1: return 10;
                case 2: return 100;
                case 3: return 1000;
                case 4: return 10000;
            }
            return 1;
        }

        public override void Start2() {
            for(int y = 0; y < 7; y++) {
                for(int x = 0; x < 27; x++) {
                    grid[x, y] = 0;
                }
            }
            selector = new Vector2(0, 2);
            rotation_wheel = 0.00f;
            rotation_ball = 0.00f;
            spinning = false;
            betting = true;
            placing = false;
            end = false;
            bet = 0;
            result = 0;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { Spin(); pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) {
                pressed_response = true;
                if(betting && !end) {
                    if(selector_bet == 0) {
                        bet = bet - Get_Bet();
                        if(bet < 0) bet = 0;
                    }
                    if(selector_bet == 1) {
                        bet = bet + Get_Bet();
                        if(bet > coins_old) bet = coins_old;
                    }
                }
                if(!betting && !end) {
                    Place((int)selector.X, (int)selector.Y);
                }
            }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; } else { if(selector.Y > 0) selector.Y--; } }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(betmulti > 0) betmulti--; } else { if(selector.Y < 6) selector.Y++; } }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet > 0) selector_bet--; } else { if(selector.X > 0) selector.X--; } }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet < 1) selector_bet++; } else { if(selector.X < 26) selector.X++; } }
            if(ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                pressed_response = true;
                if(placing) {
                    for(int y = 0; y < 7; y++) {
                        for(int x = 0; x < 27; x++) {
                            if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Roulette_Chip().Y + y * (int)SK.Position_Roulette_Chip_Next().Y, (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip_Next().Y))) { if(selector == new Vector2(x, y)) { Place(x, y); } else { selector = new Vector2(x, y); } }
                        }
                    }
                } else if(betting) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { bet = bet - Get_Bet(); if(bet < 0) bet = 0; selector_bet = 0; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { bet = bet + Get_Bet(); if(bet > coins_old) bet = coins_old; selector_bet = 1; }

                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up1().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up2().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down1().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down2().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                }
            }
            if(control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                if(placing) {
                    for(int y = 0; y < 7; y++) {
                        for(int x = 0; x < 27; x++) {
                            if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Roulette_Chip().Y + y * (int)SK.Position_Roulette_Chip_Next().Y, (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip_Next().Y))) { if(GridValid(x, y)) selector = new Vector2(x, y); }
                        }
                    }
                } else if(betting) {
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { selector_bet = 0; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { selector_bet = 1; }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(spinning) {
                rotation_wheel = rotation_wheel + 0.01f;
                rotation_ball = rotation_ball + 0.02f;
            }
            if(!spinning && !end && timer != 0) {
                if(timer > 20) {
                    rotation_wheel = rotation_wheel + 0.01f;
                    rotation_ball = rotation_ball + 0.01f;
                }
                timer--;
            }
            if(end && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private bool GridValid(int x, int y) {
            if(x == 1) return false;
            if(x == 25) return false;
            if(new Vector2(x, y) == new Vector2(0, 0)) return false;
            if(new Vector2(x, y) == new Vector2(0, 1)) return false;
            if(new Vector2(x, y) == new Vector2(0, 3)) return false;
            if(new Vector2(x, y) == new Vector2(0, 4)) return false;
            if(new Vector2(x, y) == new Vector2(0, 5)) return false;
            if(new Vector2(x, y) == new Vector2(0, 6)) return false;

            if(new Vector2(x, y) == new Vector2(26, 1)) return false;
            if(new Vector2(x, y) == new Vector2(26, 3)) return false;
            if(new Vector2(x, y) == new Vector2(26, 5)) return false;
            if(new Vector2(x, y) == new Vector2(26, 6)) return false;

            if(y == 5) {
                if(x == 5 || x == 13 || x == 21) {
                    return true;
                } else {
                    return false;
                }
            }
            if(y == 6) {
                if(x == 3 || x == 7 || x == 11 || x == 15 || x == 19 || x == 23) {
                    return true;
                } else {
                    return false;
                }
            }
            return true;
        }

        private void Place(int x, int y) {
            if(GridValid(x, y)) {
                if(grid[x, y] == 0) {
                    grid[x, y] = bet;
                } else {
                    grid[x, y] = 0;
                }
            }
        }

        private void Spin() {
            if(betting) {
                betting = false;
                placing = true;
            } else if(placing) {
                placing = false;
                spinning = true;
            } else if(spinning) {
                spinning = false;
                timer = 100 + random.Next(200);
            } else if(timer == 0) {
                Result();
                end = true;
            }
        }

        public void Result() {
            // 1 Number == 0,1698... Radian
            int n = 0;

            float rotation = rotation_wheel + rotation_ball + 0.085f;
            while(rotation > 0.1698 * 37) {
                rotation = rotation - (0.1698f * 37);
            }

            for(int i = 0; i < 37; i++) {
                if(0.17 * i < rotation && rotation < 0.17 * i + 0.17) {
                    n = i;
                }
            }

            if(n == 0) result = 36;
            if(n == 1) result = 11;
            if(n == 2) result = 30;
            if(n == 3) result = 8;
            if(n == 4) result = 23;
            if(n == 5) result = 10;
            if(n == 6) result = 5;
            if(n == 7) result = 24;
            if(n == 8) result = 16;
            if(n == 9) result = 33;
            if(n == 10) result = 1;
            if(n == 11) result = 20;
            if(n == 12) result = 14;
            if(n == 13) result = 31;
            if(n == 14) result = 9;
            if(n == 15) result = 22;
            if(n == 16) result = 18;
            if(n == 17) result = 29;
            if(n == 18) result = 7;
            if(n == 19) result = 28;
            if(n == 20) result = 12;
            if(n == 21) result = 35;
            if(n == 22) result = 3;
            if(n == 23) result = 26;
            if(n == 24) result = 0;
            if(n == 25) result = 32;
            if(n == 26) result = 15;
            if(n == 27) result = 19;
            if(n == 28) result = 4;
            if(n == 29) result = 21;
            if(n == 30) result = 2;
            if(n == 31) result = 25;
            if(n == 32) result = 17;
            if(n == 33) result = 34;
            if(n == 34) result = 6;
            if(n == 35) result = 27;
            if(n == 36) result = 13;

            for(int y = 0; y < 7; y++) { // Take Bet
                for(int x = 0; x < 27; x++) {
                    if(grid[x, y] != 0) coins_plus = coins_plus - grid[x, y];
                }
            }

            if(grid[2, 4] != 0 && result == 1) coins_plus += grid[2, 4] * 36; if(grid[2, 2] != 0 && result == 2) coins_plus += grid[2, 2] * 36; if(grid[2, 0] != 0 && result == 3) coins_plus += grid[2, 0] * 36;    // Direct Bet -  1 -  2 -  3
            if(grid[4, 4] != 0 && result == 4) coins_plus += grid[4, 4] * 36; if(grid[4, 2] != 0 && result == 5) coins_plus += grid[4, 2] * 36; if(grid[4, 0] != 0 && result == 6) coins_plus += grid[4, 0] * 36;    // Direct Bet -  4 -  5 -  6
            if(grid[6, 4] != 0 && result == 7) coins_plus += grid[6, 4] * 36; if(grid[6, 2] != 0 && result == 8) coins_plus += grid[6, 2] * 36; if(grid[6, 0] != 0 && result == 9) coins_plus += grid[6, 0] * 36;    // Direct Bet -  7 -  8 -  9
            if(grid[8, 4] != 0 && result == 10) coins_plus += grid[8, 4] * 36; if(grid[8, 2] != 0 && result == 11) coins_plus += grid[8, 2] * 36; if(grid[8, 0] != 0 && result == 12) coins_plus += grid[8, 0] * 36;    // Direct Bet - 10 - 11 - 12
            if(grid[10, 4] != 0 && result == 13) coins_plus += grid[10, 4] * 36; if(grid[10, 2] != 0 && result == 14) coins_plus += grid[10, 2] * 36; if(grid[10, 0] != 0 && result == 15) coins_plus += grid[10, 0] * 36;    // Direct Bet - 13 - 14 - 15
            if(grid[12, 4] != 0 && result == 16) coins_plus += grid[12, 4] * 36; if(grid[12, 2] != 0 && result == 17) coins_plus += grid[12, 2] * 36; if(grid[12, 0] != 0 && result == 18) coins_plus += grid[12, 0] * 36;    // Direct Bet - 16 - 17 - 18
            if(grid[14, 4] != 0 && result == 19) coins_plus += grid[14, 4] * 36; if(grid[14, 2] != 0 && result == 20) coins_plus += grid[14, 2] * 36; if(grid[14, 0] != 0 && result == 21) coins_plus += grid[14, 0] * 36;    // Direct Bet - 19 - 20 - 21
            if(grid[16, 4] != 0 && result == 22) coins_plus += grid[16, 4] * 36; if(grid[16, 2] != 0 && result == 23) coins_plus += grid[16, 2] * 36; if(grid[16, 0] != 0 && result == 24) coins_plus += grid[16, 0] * 36;    // Direct Bet - 22 - 23 - 24
            if(grid[18, 4] != 0 && result == 25) coins_plus += grid[18, 4] * 36; if(grid[18, 2] != 0 && result == 26) coins_plus += grid[18, 2] * 36; if(grid[18, 0] != 0 && result == 27) coins_plus += grid[18, 0] * 36;    // Direct Bet - 25 - 26 - 27
            if(grid[20, 4] != 0 && result == 28) coins_plus += grid[20, 4] * 36; if(grid[20, 2] != 0 && result == 29) coins_plus += grid[20, 2] * 36; if(grid[20, 0] != 0 && result == 30) coins_plus += grid[20, 0] * 36;    // Direct Bet - 28 - 29 - 30
            if(grid[22, 4] != 0 && result == 31) coins_plus += grid[22, 4] * 36; if(grid[22, 2] != 0 && result == 32) coins_plus += grid[22, 2] * 36; if(grid[22, 0] != 0 && result == 33) coins_plus += grid[22, 0] * 36;    // Direct Bet - 31 - 32 - 33
            if(grid[24, 4] != 0 && result == 34) coins_plus += grid[24, 4] * 36; if(grid[24, 2] != 0 && result == 35) coins_plus += grid[24, 2] * 36; if(grid[24, 0] != 0 && result == 36) coins_plus += grid[24, 0] * 36;    // Direct Bet - 34 - 35 - 36

            if(grid[3, 4] != 0 && (result == 1 || result == 4)) coins_plus += grid[3, 4] * 18; if(grid[3, 2] != 0 && (result == 2 || result == 5)) coins_plus += grid[3, 2] * 18; if(grid[3, 0] != 0 && (result == 3 || result == 6)) coins_plus += grid[3, 0] * 18;    // Vertical Bet -  1/ 4 -  2/ 5 -  3/ 6
            if(grid[5, 4] != 0 && (result == 4 || result == 7)) coins_plus += grid[5, 4] * 18; if(grid[5, 2] != 0 && (result == 5 || result == 8)) coins_plus += grid[5, 2] * 18; if(grid[5, 0] != 0 && (result == 6 || result == 9)) coins_plus += grid[5, 0] * 18;    // Vertical Bet -  4/ 7 -  5/ 7 -  6/ 9
            if(grid[7, 4] != 0 && (result == 7 || result == 10)) coins_plus += grid[7, 4] * 18; if(grid[7, 2] != 0 && (result == 8 || result == 11)) coins_plus += grid[7, 2] * 18; if(grid[7, 0] != 0 && (result == 9 || result == 12)) coins_plus += grid[7, 0] * 18;    // Vertical Bet -  7/10 -  8/11 -  9/12
            if(grid[9, 4] != 0 && (result == 10 || result == 13)) coins_plus += grid[9, 4] * 18; if(grid[9, 2] != 0 && (result == 11 || result == 14)) coins_plus += grid[9, 2] * 18; if(grid[9, 0] != 0 && (result == 12 || result == 15)) coins_plus += grid[9, 0] * 18;    // Vertical Bet - 10/13 - 11/14 - 12/15
            if(grid[11, 4] != 0 && (result == 13 || result == 16)) coins_plus += grid[11, 4] * 18; if(grid[11, 2] != 0 && (result == 14 || result == 17)) coins_plus += grid[11, 2] * 18; if(grid[11, 0] != 0 && (result == 15 || result == 18)) coins_plus += grid[11, 0] * 18;    // Vertical Bet - 13/16 - 14/17 - 15/18
            if(grid[13, 4] != 0 && (result == 16 || result == 19)) coins_plus += grid[13, 4] * 18; if(grid[13, 2] != 0 && (result == 17 || result == 20)) coins_plus += grid[13, 2] * 18; if(grid[13, 0] != 0 && (result == 18 || result == 21)) coins_plus += grid[13, 0] * 18;    // Vertical Bet - 16/19 - 17/20 - 18/21
            if(grid[15, 4] != 0 && (result == 19 || result == 22)) coins_plus += grid[15, 4] * 18; if(grid[15, 2] != 0 && (result == 20 || result == 23)) coins_plus += grid[15, 2] * 18; if(grid[15, 0] != 0 && (result == 21 || result == 24)) coins_plus += grid[15, 0] * 18;    // Vertical Bet - 19/22 - 20/23 - 21/24
            if(grid[17, 4] != 0 && (result == 22 || result == 25)) coins_plus += grid[17, 4] * 18; if(grid[17, 2] != 0 && (result == 23 || result == 26)) coins_plus += grid[17, 2] * 18; if(grid[17, 0] != 0 && (result == 24 || result == 27)) coins_plus += grid[17, 0] * 18;    // Vertical Bet - 22/25 - 23/26 - 24/27
            if(grid[19, 4] != 0 && (result == 25 || result == 28)) coins_plus += grid[19, 4] * 18; if(grid[19, 2] != 0 && (result == 26 || result == 29)) coins_plus += grid[19, 2] * 18; if(grid[19, 0] != 0 && (result == 27 || result == 30)) coins_plus += grid[19, 0] * 18;    // Vertical Bet - 25/28 - 26/29 - 27/30
            if(grid[21, 4] != 0 && (result == 28 || result == 31)) coins_plus += grid[21, 4] * 18; if(grid[21, 2] != 0 && (result == 29 || result == 32)) coins_plus += grid[21, 2] * 18; if(grid[21, 0] != 0 && (result == 30 || result == 33)) coins_plus += grid[21, 0] * 18;    // Vertical Bet - 28/31 - 29/32 - 30/33
            if(grid[23, 4] != 0 && (result == 31 || result == 34)) coins_plus += grid[23, 4] * 18; if(grid[23, 2] != 0 && (result == 32 || result == 35)) coins_plus += grid[23, 2] * 18; if(grid[23, 0] != 0 && (result == 33 || result == 36)) coins_plus += grid[23, 0] * 18;    // Vertical Bet - 31/34 - 32/35 - 33/36

            if(grid[2, 3] != 0 && (result == 1 || result == 2)) coins_plus += grid[2, 3] * 18; if(grid[2, 1] != 0 && (result == 2 || result == 3)) coins_plus += grid[2, 1] * 18;    // Horizontal Bet -  1/ 2 -  2/ 3
            if(grid[4, 3] != 0 && (result == 4 || result == 5)) coins_plus += grid[4, 3] * 18; if(grid[4, 1] != 0 && (result == 5 || result == 6)) coins_plus += grid[4, 1] * 18;    // Horizontal Bet -  4/ 5 -  5/ 6
            if(grid[6, 3] != 0 && (result == 7 || result == 8)) coins_plus += grid[6, 3] * 18; if(grid[6, 1] != 0 && (result == 8 || result == 9)) coins_plus += grid[6, 1] * 18;    // Horizontal Bet -  7/ 8 -  8/ 9
            if(grid[8, 3] != 0 && (result == 10 || result == 11)) coins_plus += grid[8, 3] * 18; if(grid[8, 1] != 0 && (result == 11 || result == 12)) coins_plus += grid[8, 1] * 18;    // Horizontal Bet - 10/11 - 11/12
            if(grid[10, 3] != 0 && (result == 13 || result == 14)) coins_plus += grid[10, 3] * 18; if(grid[10, 1] != 0 && (result == 14 || result == 15)) coins_plus += grid[10, 1] * 18;    // Horizontal Bet - 13/14 - 14/15
            if(grid[12, 3] != 0 && (result == 16 || result == 17)) coins_plus += grid[12, 3] * 18; if(grid[12, 1] != 0 && (result == 17 || result == 18)) coins_plus += grid[12, 1] * 18;    // Horizontal Bet - 16/17 - 17/18
            if(grid[14, 3] != 0 && (result == 19 || result == 20)) coins_plus += grid[14, 3] * 18; if(grid[14, 1] != 0 && (result == 20 || result == 21)) coins_plus += grid[14, 1] * 18;    // Horizontal Bet - 19/20 - 20/21
            if(grid[16, 3] != 0 && (result == 22 || result == 23)) coins_plus += grid[16, 3] * 18; if(grid[16, 1] != 0 && (result == 23 || result == 24)) coins_plus += grid[16, 1] * 18;    // Horizontal Bet - 22/23 - 23/24
            if(grid[18, 3] != 0 && (result == 25 || result == 26)) coins_plus += grid[18, 3] * 18; if(grid[18, 1] != 0 && (result == 26 || result == 27)) coins_plus += grid[18, 1] * 18;    // Horizontal Bet - 25/26 - 26/27
            if(grid[20, 3] != 0 && (result == 28 || result == 29)) coins_plus += grid[20, 3] * 18; if(grid[20, 1] != 0 && (result == 29 || result == 30)) coins_plus += grid[20, 1] * 18;    // Horizontal Bet - 28/29 - 29/30
            if(grid[22, 3] != 0 && (result == 31 || result == 32)) coins_plus += grid[22, 3] * 18; if(grid[22, 1] != 0 && (result == 32 || result == 33)) coins_plus += grid[22, 1] * 18;    // Horizontal Bet - 31/32 - 32/33
            if(grid[24, 3] != 0 && (result == 34 || result == 35)) coins_plus += grid[24, 3] * 18; if(grid[24, 1] != 0 && (result == 35 || result == 36)) coins_plus += grid[24, 1] * 18;    // Horizontal Bet - 34/35 - 35/36

            if(grid[3, 3] != 0 && (result == 1 || result == 2 || result == 4 || result == 5)) coins_plus += grid[3, 3] * 18; if(grid[3, 1] != 0 && (result == 2 || result == 3 || result == 5 || result == 6)) coins_plus += grid[3, 1] * 18;    // Cross Bet -  1/ 2/ 4/ 5 -  2/ 3/ 5/ 6
            if(grid[5, 3] != 0 && (result == 4 || result == 5 || result == 7 || result == 8)) coins_plus += grid[5, 3] * 18; if(grid[5, 1] != 0 && (result == 5 || result == 6 || result == 8 || result == 9)) coins_plus += grid[5, 1] * 18;    // Cross Bet -  4/ 5/ 7/ 8 -  5/ 6/ 8/ 9
            if(grid[7, 3] != 0 && (result == 7 || result == 8 || result == 10 || result == 11)) coins_plus += grid[7, 3] * 18; if(grid[7, 1] != 0 && (result == 8 || result == 9 || result == 11 || result == 12)) coins_plus += grid[7, 1] * 18;    // Cross Bet -  7/ 8/10/11 -  8/ 9/11/12
            if(grid[9, 3] != 0 && (result == 10 || result == 11 || result == 13 || result == 14)) coins_plus += grid[9, 3] * 18; if(grid[9, 1] != 0 && (result == 11 || result == 12 || result == 14 || result == 15)) coins_plus += grid[9, 1] * 18;    // Cross Bet - 10/11/13/14 - 11/12/14/15
            if(grid[11, 3] != 0 && (result == 13 || result == 14 || result == 16 || result == 17)) coins_plus += grid[11, 3] * 18; if(grid[11, 1] != 0 && (result == 14 || result == 15 || result == 17 || result == 18)) coins_plus += grid[11, 1] * 18;    // Cross Bet - 13/14/16/17 - 14/15/17/18
            if(grid[13, 3] != 0 && (result == 16 || result == 17 || result == 19 || result == 20)) coins_plus += grid[13, 3] * 18; if(grid[13, 1] != 0 && (result == 17 || result == 18 || result == 20 || result == 21)) coins_plus += grid[13, 1] * 18;    // Cross Bet - 16/17/19/20 - 17/18/20/21
            if(grid[15, 3] != 0 && (result == 19 || result == 20 || result == 22 || result == 23)) coins_plus += grid[15, 3] * 18; if(grid[15, 1] != 0 && (result == 20 || result == 21 || result == 23 || result == 24)) coins_plus += grid[15, 1] * 18;    // Cross Bet - 19/20/22/23 - 20/21/23/24
            if(grid[17, 3] != 0 && (result == 22 || result == 23 || result == 25 || result == 26)) coins_plus += grid[17, 3] * 18; if(grid[17, 1] != 0 && (result == 23 || result == 24 || result == 26 || result == 27)) coins_plus += grid[17, 1] * 18;    // Cross Bet - 22/23/25/26 - 23/24/26/27
            if(grid[19, 3] != 0 && (result == 25 || result == 26 || result == 28 || result == 29)) coins_plus += grid[19, 3] * 18; if(grid[19, 1] != 0 && (result == 26 || result == 27 || result == 29 || result == 30)) coins_plus += grid[19, 1] * 18;    // Cross Bet - 25/26/28/29 - 26/27/29/30
            if(grid[21, 3] != 0 && (result == 28 || result == 29 || result == 31 || result == 32)) coins_plus += grid[21, 3] * 18; if(grid[21, 1] != 0 && (result == 29 || result == 30 || result == 32 || result == 33)) coins_plus += grid[21, 1] * 18;    // Cross Bet - 28/29/31/32 - 29/30/32/33
            if(grid[23, 3] != 0 && (result == 31 || result == 32 || result == 34 || result == 35)) coins_plus += grid[23, 3] * 18; if(grid[23, 1] != 0 && (result == 32 || result == 33 || result == 35 || result == 36)) coins_plus += grid[23, 1] * 18;    // Cross Bet - 31/32/34/35 - 32/33/35/36

            if(grid[5, 5] != 0) { if(result >= 1 && result <= 12) coins_plus += grid[5, 5] * 3; } // 1st 12
            if(grid[13, 5] != 0) { if(result >= 13 && result <= 24) coins_plus += grid[13, 5] * 3; } // 2nd 12
            if(grid[21, 5] != 0) { if(result >= 25 && result <= 36) coins_plus += grid[21, 5] * 3; } // 3rd 12

            if(grid[26, 4] != 0) { if(result % 3 == 1) coins_plus += grid[26, 4] * 3; } // 2 to 1 (1)
            if(grid[26, 2] != 0) { if(result % 3 == 2) coins_plus += grid[26, 2] * 3; } // 2 to 1 (2)
            if(grid[26, 0] != 0) { if(result % 3 == 0) coins_plus += grid[26, 0] * 3; } // 2 to 1 (3)

            if(grid[3, 6] != 0) { if(result >= 1 && result <= 18) coins_plus += grid[3, 6] * 2; } //  1 to 18
            if(grid[23, 6] != 0) { if(result >= 19 && result <= 36) coins_plus += grid[23, 6] * 2; } // 19 to 36

            if(grid[7, 6] != 0) { if(result % 2 == 0) coins_plus += grid[7, 6] * 2; } // EVEN
            if(grid[19, 6] != 0) { if(result % 2 == 1) coins_plus += grid[19, 6] * 2; } // ODD

            if(grid[0, 2] != 0 && result == 0) { coins_plus += grid[0, 2] * 36; } // Direct 0

            if(grid[11, 6] != 0) { if(result == 1 || result == 3 || result == 5 || result == 7 || result == 9 || result == 12 || result == 14 || result == 16 || result == 18 || result == 19 || result == 21 || result == 23 || result == 25 || result == 27 || result == 30 || result == 32 || result == 34 || result == 36) { coins_plus += grid[11, 6] * 2; } } // RED
            if(grid[15, 6] != 0) { if(result == 2 || result == 4 || result == 6 || result == 8 || result == 10 || result == 11 || result == 13 || result == 15 || result == 17 || result == 20 || result == 22 || result == 24 || result == 26 || result == 28 || result == 29 || result == 31 || result == 33 || result == 35) { coins_plus += grid[15, 6] * 2; } } // BLACK
        }

        public override void Draw2() {

            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.ForestGreen); } else { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.ForestGreen); }

            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_roulette, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.White); } else { spriteBatch.Draw(SK.texture_background_roulette, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.White); }

            for(int y = 0; y < 5; y++) {
                for(int x = 0; x < 27; x++) {
                    if(grid[x, y] != 0) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + y * (int)SK.Position_Roulette_Chip_Next().Y), Color.White);
                }
            }
            for(int x = 0; x < 27; x++) { if(grid[x, 5] != 0) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + 250 + 20), Color.White); }
            for(int x = 0; x < 27; x++) { if(grid[x, 6] != 0) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + 295 + 10), Color.White); }


            for(int y = 0; y < 5; y++) {
                for(int x = 0; x < 27; x++) {
                    if(selector == new Vector2(x, y)) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + y * (int)SK.Position_Roulette_Chip_Next().Y), Get_Color(x, y));
                }
            }
            for(int x = 0; x < 27; x++) { if(selector == new Vector2(x, 5)) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + 250 + 20), Get_Color(x, 5)); }
            for(int x = 0; x < 27; x++) { if(selector == new Vector2(x, 6)) spriteBatch.Draw(SK.texture_static_roulette_chip, SK.Position_DisplayEdge() + new Vector2((int)SK.Position_Roulette_Chip().X + x * (int)SK.Position_Roulette_Chip_Next().X, (int)SK.Position_Roulette_Chip().Y + 295 + 10), Get_Color(x, 6)); }

            if(betting && !end) {
                spriteBatch.Draw(SK.texture_casino_bet_grid, SK.Position_DisplayEdge() + SK.Position_Bet_Grid(), Color.White);
                spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Minus(), Color.White);
                spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Plus(), Color.White);

                                  spriteBatch.Draw(SK.texture_casino_bet_minus, SK.Position_DisplayEdge() + SK.Position_Bet_Minus() + Get_PMVario(0), Color.White);
                if(betmulti >= 1) spriteBatch.Draw(SK.texture_casino_bet_minus, SK.Position_DisplayEdge() + SK.Position_Bet_Minus() + Get_PMVario(1), Color.White);
                if(betmulti >= 2) spriteBatch.Draw(SK.texture_casino_bet_minus, SK.Position_DisplayEdge() + SK.Position_Bet_Minus() + Get_PMVario(2), Color.White);
                if(betmulti >= 3) spriteBatch.Draw(SK.texture_casino_bet_minus, SK.Position_DisplayEdge() + SK.Position_Bet_Minus() + Get_PMVario(3), Color.White);
                if(betmulti >= 4) spriteBatch.Draw(SK.texture_casino_bet_minus, SK.Position_DisplayEdge() + SK.Position_Bet_Minus() + Get_PMVario(4), Color.White);

                                  spriteBatch.Draw(SK.texture_casino_bet_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Plus() + Get_PMVario(0), Color.White);
                if(betmulti >= 1) spriteBatch.Draw(SK.texture_casino_bet_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Plus() + Get_PMVario(1), Color.White);
                if(betmulti >= 2) spriteBatch.Draw(SK.texture_casino_bet_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Plus() + Get_PMVario(2), Color.White);
                if(betmulti >= 3) spriteBatch.Draw(SK.texture_casino_bet_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Plus() + Get_PMVario(3), Color.White);
                if(betmulti >= 4) spriteBatch.Draw(SK.texture_casino_bet_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Plus() + Get_PMVario(4), Color.White);

                spriteBatch.Draw(SK.texture_casino_bet_up, SK.Position_DisplayEdge() + SK.Position_Bet_Up1(), Color.White);
                spriteBatch.Draw(SK.texture_casino_bet_up, SK.Position_DisplayEdge() + SK.Position_Bet_Up2(), Color.White);
                spriteBatch.Draw(SK.texture_casino_bet_down, SK.Position_DisplayEdge() + SK.Position_Bet_Down1(), Color.White);
                spriteBatch.Draw(SK.texture_casino_bet_down, SK.Position_DisplayEdge() + SK.Position_Bet_Down2(), Color.White);

                if(selector_bet == 0) spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Minus(), Color.Red * 0.50f);
                if(selector_bet == 1) spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Plus(), Color.Red * 0.50f);

                spriteBatch.DrawString(SK.font_score, "BET:", SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(50, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "COIN:", SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(50, 120 - SK.font_score.MeasureString("COIN:").Y), Color.DarkGoldenrod);
                spriteBatch.DrawString(SK.font_score, "" + bet, SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(235 - SK.font_score.MeasureString("" + bet).X, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + coins_old, SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(235, 120) - SK.font_score.MeasureString("" + coins_old), Color.DarkGoldenrod);
            }

            if(!betting) {
                spriteBatch.Draw(SK.texture_casino_bet_grid, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3(), Color.White);
            }

            if(!betting && !end) {
                spriteBatch.DrawString(SK.font_score, "" + bet, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("" + bet).X, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + coins_old, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235, 120) - SK.font_score.MeasureString("" + coins_old), Color.DarkGoldenrod);
            }

            if(!betting && end) {
                spriteBatch.DrawString(SK.font_score, "" + result, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(50, 45), Color.LightGray);
                if(coins_plus > 0)
                    spriteBatch.DrawString(SK.font_score, "+" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("+" + coins_plus).X, 45), Color.LimeGreen);
                if(coins_plus < 0)
                    spriteBatch.DrawString(SK.font_score, "" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("" + coins_plus).X, 45), Color.IndianRed);
                spriteBatch.DrawString(SK.font_score, "" + FM.coins, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235, 120) - SK.font_score.MeasureString("" + FM.coins), Color.DarkGoldenrod);
            }

            if(!betting && !placing && !end) {
                spriteBatch.Draw(SK.texture_static_roulette_wheel, SK.Position_DisplayEdge() + SK.Get_GridSize() / 2, null, Color.White, -rotation_wheel, new Vector2((int)587 / 2, (int)585 / 2), 1, SpriteEffects.None, 0);
                spriteBatch.Draw(SK.texture_static_roulette_ball, SK.Position_DisplayEdge() + SK.Get_GridSize() / 2, null, Color.White, rotation_ball, new Vector2((int)587 / 2, (int)585 / 2), 1, SpriteEffects.None, 0);
            }
        }

        public override void Draw3() {
            if(end) {
                spriteBatch.Draw(SK.texture_hud_button_replay, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
            } else if(betting) {
                spriteBatch.Draw(SK.texture_hud_button_spin, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
            } else if(spinning) {
                spriteBatch.Draw(SK.texture_hud_button_spin, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
            } else if(timer == 0) {
                spriteBatch.Draw(SK.texture_hud_button_spin, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
            }
        }

        private Color Get_Color(int x, int y) {
            if(GridValid(x, y)) {
                return Color.Blue * 0.50f;
            } else {
                return Color.Blue * 0.10f;
            }
        }

        private Vector2 Get_PMVario(int i) {
            if(betmulti == 1) {
                if(i == 0) return new Vector2(5, -5);
                if(i == 1) return new Vector2(-5, 5);
            }
            if(betmulti == 2) {
                if(i == 0) return new Vector2(10, -10);
                if(i == 1) return new Vector2(0, 0);
                if(i == 2) return new Vector2(-10, 10);
            }
            if(betmulti == 3) {
                if(i == 0) return new Vector2(15, -15);
                if(i == 1) return new Vector2(5, -5);
                if(i == 2) return new Vector2(-5, 5);
                if(i == 3) return new Vector2(-15, 15);
            }
            if(betmulti == 4) {
                if(i == 0) return new Vector2( 20, -20);
                if(i == 1) return new Vector2( 10, -10);
                if(i == 2) return new Vector2(  0,   0);
                if(i == 3) return new Vector2(-10,  10);
                if(i == 4) return new Vector2(-20,  20);
            }
            return new Vector2(0, 0);
        }

    }
}
