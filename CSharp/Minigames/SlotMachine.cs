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

    class SlotMachine : Ghost {

        int selector_bet;

        int betmulti = 0;

        int[,] grid = new int[9,3];
        int speed1 = 12;
        int speed2 = 16;
        int speed3 = 24;

        bool end;

        bool active_spin;
        bool active_wheel1;
        bool active_wheel2;
        bool active_wheel3;

        bool active_line1;
        bool active_line2;
        bool active_line3;
        bool active_line4;
        bool active_line5;

        int pos_wheel1;
        int pos_wheel2;
        int pos_wheel3;

        int bet;
        int multi;

        public SlotMachine(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            for(int y = 0; y < 3; y++) {
                for(int x = 0; x < 9; x++) {
                    grid[x, y] = random.Next(10);
                    if(grid[x, y] == 3) {
                        if(0 == random.Next(3))
                            grid[x, y] = 0;
                    }
                }
            }
            active_spin = false;
            active_wheel1 = false;
            active_wheel2 = false;
            active_wheel3 = false;
            active_line1 = false;
            active_line2 = false;
            active_line3 = false;
            active_line4 = false;
            active_line5 = false;
            bet = 0;
            multi = 1;
            selector_bet = 1;
            pos_wheel1 = 0;
            pos_wheel2 = 0;
            pos_wheel3 = 0;
            end = false;
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

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { Spin(); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; if(!active_spin) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; } }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { pressed_response = true; if(!active_spin) { if(betmulti > 0) betmulti--; } }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { pressed_response = true; if(!active_spin) { if(selector_bet > 0) { selector_bet--; } else { if(multi > 1) { multi--; Change_Bet(1); } } } }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { pressed_response = true; if(!active_spin) { if(selector_bet < 1) { selector_bet++; } else { if(multi < 5) { multi++; Change_Bet(2); } } } }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) {
                pressed_response = true;
                if(!active_spin && !end) {
                    Change_Bet(0);
                }
            }
            if(ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                pressed_response = true;
                if(!active_spin && !end) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { selector_bet = 0; Change_Bet(0); }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { selector_bet = 1; Change_Bet(0); }

                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up1().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up2().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down1().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down2().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Left().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Left().Y, SK.texture_casino_bet_left.Width, SK.texture_casino_bet_left.Height))) { if(multi > 1) { multi--; Change_Bet(1); } }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Right().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Right().Y, SK.texture_casino_bet_right.Width, SK.texture_casino_bet_right.Height))) { if(multi < 5) { multi++; Change_Bet(2); } }
                }
            }
            if(control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                if(!active_spin && !end) {
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { selector_bet = 0; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { selector_bet = 1; }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(active_spin) {
                if(active_wheel1) {
                    pos_wheel1 = pos_wheel1 + speed1;
                } else {
                    if(pos_wheel1 != 0) {
                        pos_wheel1 = pos_wheel1 + speed1 / 2;
                    }
                }
                if(active_wheel2) {
                    pos_wheel2 = pos_wheel2 + speed2;
                } else {
                    if(pos_wheel2 != 0) {
                        pos_wheel2 = pos_wheel2 + speed2 / 2;
                    }
                }
                if(active_wheel3) {
                    pos_wheel3 = pos_wheel3 + speed3;
                } else {
                    if(pos_wheel3 != 0) {
                        pos_wheel3 = pos_wheel3 + speed3 / 2;
                    }
                }
                if(pos_wheel1 >= 120) {
                    pos_wheel1 = 0;
                    Rotate(0);
                }
                if(pos_wheel2 >= 120) {
                    pos_wheel2 = 0;
                    Rotate(1);
                }
                if(pos_wheel3 >= 120) {
                    pos_wheel3 = 0;
                    Rotate(2);
                }
            }
            if(active_spin && !active_wheel1 && !active_wheel2 && !active_wheel3 && !end && pos_wheel1 == 0 && pos_wheel2 == 0 && pos_wheel3 == 0) {
                end = true;
                coins_plus = 0 - bet * multi;
                if(multi >= 1) {
                    if(grid[4, 0] == 0) { coins_plus = coins_plus + bet / multi; active_line1 = true; }
                    if(grid[4, 1] == 0) { coins_plus = coins_plus + bet / multi; active_line1 = true; }
                    if(grid[4, 2] == 0) { coins_plus = coins_plus + bet / multi; active_line1 = true; }
                    if(grid[4, 0] == grid[4, 1] && grid[4, 1] == grid[4, 2] && grid[4, 0] == grid[4, 2]) {
                        if(grid[4, 0] == 1) coins_plus = coins_plus + (bet * 5);
                        if(grid[4, 0] == 2) coins_plus = coins_plus + (bet * 6);
                        if(grid[4, 0] == 3) coins_plus = coins_plus + (bet * 7);
                        if(grid[4, 0] == 4) coins_plus = coins_plus + (bet * 1);
                        if(grid[4, 0] == 5) coins_plus = coins_plus + (bet * 2);
                        if(grid[4, 0] == 6) coins_plus = coins_plus + (bet * 2);
                        if(grid[4, 0] == 7) coins_plus = coins_plus + (bet * 2);
                        if(grid[4, 0] == 8) coins_plus = coins_plus + (bet * 2);
                        if(grid[4, 0] == 9) coins_plus = coins_plus + (bet * 2);
                        active_line1 = true;
                    }
                }
                if(multi >= 2) {
                    if(grid[3, 0] == 0) { coins_plus = coins_plus + bet / multi; active_line2 = true; }
                    if(grid[3, 1] == 0) { coins_plus = coins_plus + bet / multi; active_line2 = true; }
                    if(grid[3, 2] == 0) { coins_plus = coins_plus + bet / multi; active_line2 = true; }
                    if(grid[3, 0] == grid[3, 1] && grid[3, 1] == grid[3, 2] && grid[3, 0] == grid[3, 2]) {
                        if(grid[3, 0] == 1) coins_plus = coins_plus + (bet * 5);
                        if(grid[3, 0] == 2) coins_plus = coins_plus + (bet * 6);
                        if(grid[3, 0] == 3) coins_plus = coins_plus + (bet * 7);
                        if(grid[3, 0] == 4) coins_plus = coins_plus + (bet * 1);
                        if(grid[3, 0] == 5) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 6) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 7) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 8) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 9) coins_plus = coins_plus + (bet * 2);
                        active_line2 = true;
                    }
                }
                if(multi >= 3) {
                    if(grid[5, 0] == 0) { coins_plus = coins_plus + bet / multi; active_line3 = true; }
                    if(grid[5, 1] == 0) { coins_plus = coins_plus + bet / multi; active_line3 = true; }
                    if(grid[5, 2] == 0) { coins_plus = coins_plus + bet / multi; active_line3 = true; }
                    if(grid[5, 0] == grid[5, 1] && grid[5, 1] == grid[5, 2] && grid[5, 0] == grid[5, 2]) {
                        if(grid[5, 0] == 1) coins_plus = coins_plus + (bet * 5);
                        if(grid[5, 0] == 2) coins_plus = coins_plus + (bet * 6);
                        if(grid[5, 0] == 3) coins_plus = coins_plus + (bet * 7);
                        if(grid[5, 0] == 4) coins_plus = coins_plus + (bet * 1);
                        if(grid[5, 0] == 5) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 6) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 7) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 8) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 9) coins_plus = coins_plus + (bet * 2);
                        active_line3 = true;
                    }
                }
                if(multi >= 4) {
                    if(grid[3, 0] == grid[4, 1] && grid[4, 1] == grid[5, 2] && grid[3, 0] == grid[5, 2]) {
                        if(grid[3, 0] == 1) coins_plus = coins_plus + (bet * 5);
                        if(grid[3, 0] == 2) coins_plus = coins_plus + (bet * 6);
                        if(grid[3, 0] == 3) coins_plus = coins_plus + (bet * 7);
                        if(grid[3, 0] == 4) coins_plus = coins_plus + (bet * 1);
                        if(grid[3, 0] == 5) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 6) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 7) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 8) coins_plus = coins_plus + (bet * 2);
                        if(grid[3, 0] == 9) coins_plus = coins_plus + (bet * 2);
                        active_line4 = true;
                    }
                }
                if(multi >= 5) {
                    if(grid[5, 0] == grid[4, 1] && grid[4, 1] == grid[3, 2] && grid[5, 0] == grid[3, 2]) {
                        if(grid[5, 0] == 1) coins_plus = coins_plus + (bet * 5);
                        if(grid[5, 0] == 2) coins_plus = coins_plus + (bet * 6);
                        if(grid[5, 0] == 3) coins_plus = coins_plus + (bet * 7);
                        if(grid[5, 0] == 4) coins_plus = coins_plus + (bet * 1);
                        if(grid[5, 0] == 5) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 6) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 7) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 8) coins_plus = coins_plus + (bet * 2);
                        if(grid[5, 0] == 9) coins_plus = coins_plus + (bet * 2);
                        active_line5 = true;
                    }
                }
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Rotate(int i) {
            int temp = grid[0,i];
            grid[0, i] = grid[1, i];
            grid[1, i] = grid[2, i];
            grid[2, i] = grid[3, i];
            grid[3, i] = grid[4, i];
            grid[4, i] = grid[5, i];
            grid[5, i] = grid[6, i];
            grid[6, i] = grid[7, i];
            grid[7, i] = grid[8, i];
            grid[8, i] = temp;
        }

        private void Change_Bet(int i) {
            if(i == 0) { // Normal
                if(selector_bet == 0) { bet = bet - Get_Bet() * multi; if(bet < 0) bet = 0; }
                if(selector_bet == 1) { bet = bet + Get_Bet() * multi; if(bet > coins_old) bet = coins_old; }
            }
            if(i == 1) { // Lower Multi
                int temp;
                temp = bet / (multi + 1);
                bet = temp * multi;
            }
            if(i == 2) { // Higher Multi
                int temp;
                temp = bet / (multi - 1);
                bet = temp * multi;
                if(bet > coins_old) bet = coins_old;
            }
        }

        private void Spin() {
            if(!active_spin) {
                active_spin = true;
                active_wheel1 = true;
                active_wheel2 = true;
                active_wheel3 = true;
            } else {
                if(active_wheel1) {
                    if(pos_wheel1 < 30) {
                        pos_wheel1 = 0;
                    }
                    active_wheel1 = false;
                } else if(active_wheel2) {
                    if(pos_wheel2 < 30) {
                        pos_wheel2 = 0;
                    }
                    active_wheel2 = false;
                } else if(active_wheel3) {
                    if(pos_wheel3 < 30) {
                        pos_wheel3 = 0;
                    }
                    active_wheel3 = false;
                }
            }
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_slotmachine1, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.texture_background_slotmachine1.Width / 2, 0), Color.White);
            if(active_spin) {
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 - 120 * 4 - pos_wheel1), new Rectangle(1 + (116 * grid[0, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 - 120 * 3 - pos_wheel1), new Rectangle(1 + (116 * grid[1, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 - 120 * 2 - pos_wheel1), new Rectangle(1 + (116 * grid[2, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 - 120 * 1 - pos_wheel1), new Rectangle(1 + (116 * grid[3, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 - pos_wheel1), new Rectangle(1 + (116 * grid[4, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 + 120 * 1 - pos_wheel1), new Rectangle(1 + (116 * grid[5, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 + 120 * 2 - pos_wheel1), new Rectangle(1 + (116 * grid[6, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 + 120 * 3 - pos_wheel1), new Rectangle(1 + (116 * grid[7, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(8, 0 + 120 * 4 - pos_wheel1), new Rectangle(1 + (116 * grid[8, 0]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 - 120 * 4 - pos_wheel2), new Rectangle(1 + (116 * grid[0, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 - 120 * 3 - pos_wheel2), new Rectangle(1 + (116 * grid[1, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 - 120 * 2 - pos_wheel2), new Rectangle(1 + (116 * grid[2, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 - 120 * 1 - pos_wheel2), new Rectangle(1 + (116 * grid[3, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 - pos_wheel2), new Rectangle(1 + (116 * grid[4, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 + 120 * 1 - pos_wheel2), new Rectangle(1 + (116 * grid[5, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 + 120 * 2 - pos_wheel2), new Rectangle(1 + (116 * grid[6, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 + 120 * 3 - pos_wheel2), new Rectangle(1 + (116 * grid[7, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row2() + new Vector2(8, 0 + 120 * 4 - pos_wheel2), new Rectangle(1 + (116 * grid[8, 1]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 - 120 * 4 - pos_wheel3), new Rectangle(1 + (116 * grid[0, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 - 120 * 3 - pos_wheel3), new Rectangle(1 + (116 * grid[1, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 - 120 * 2 - pos_wheel3), new Rectangle(1 + (116 * grid[2, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 - 120 * 1 - pos_wheel3), new Rectangle(1 + (116 * grid[3, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 - pos_wheel3), new Rectangle(1 + (116 * grid[4, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 + 120 * 1 - pos_wheel3), new Rectangle(1 + (116 * grid[5, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 + 120 * 2 - pos_wheel3), new Rectangle(1 + (116 * grid[6, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 + 120 * 3 - pos_wheel3), new Rectangle(1 + (116 * grid[7, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(SK.texture_spritesheet_slotmachine, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row3() + new Vector2(8, 0 + 120 * 4 - pos_wheel3), new Rectangle(1 + (116 * grid[8, 2]), 1, 115, 115), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            }
            if(end) {
                if(active_line1) spriteBatch.Draw(SK.texture_static_slotmachine_line1, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 40 - SK.Position_SlotMachine_Row1().Y), Color.White);
                if(active_line2) spriteBatch.Draw(SK.texture_static_slotmachine_line2, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 40 - SK.Position_SlotMachine_Row1().Y), Color.White);
                if(active_line3) spriteBatch.Draw(SK.texture_static_slotmachine_line3, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 40 - SK.Position_SlotMachine_Row1().Y), Color.White);
                if(active_line4) spriteBatch.Draw(SK.texture_static_slotmachine_line4, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 40 - SK.Position_SlotMachine_Row1().Y), Color.White);
                if(active_line5) spriteBatch.Draw(SK.texture_static_slotmachine_line5, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 40 - SK.Position_SlotMachine_Row1().Y), Color.White);
            }

            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_slotmachine2, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 5), Color.White); } else { spriteBatch.Draw(SK.texture_background_slotmachine2, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y - 5), Color.White); }

            if(multi >= 1) spriteBatch.Draw(SK.texture_static_slotmachine_point1, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 243 - SK.Position_SlotMachine_Row1().Y), Color.White);
            if(multi >= 2) spriteBatch.Draw(SK.texture_static_slotmachine_point2, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 142 - SK.Position_SlotMachine_Row1().Y), Color.White);
            if(multi >= 3) spriteBatch.Draw(SK.texture_static_slotmachine_point3, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 346 - SK.Position_SlotMachine_Row1().Y), Color.White);
            if(multi >= 4) spriteBatch.Draw(SK.texture_static_slotmachine_point4, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 35 - SK.Position_SlotMachine_Row1().Y), Color.White);
            if(multi >= 5) spriteBatch.Draw(SK.texture_static_slotmachine_point5, SK.Position_DisplayEdge() + SK.Position_SlotMachine_Row1() + new Vector2(0, 455 - SK.Position_SlotMachine_Row1().Y), Color.White);
            if(!active_spin && !end) {
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
                spriteBatch.Draw(SK.texture_casino_bet_left, SK.Position_DisplayEdge() + SK.Position_Bet_Left(), Color.White);
                spriteBatch.Draw(SK.texture_casino_bet_right, SK.Position_DisplayEdge() + SK.Position_Bet_Right(), Color.White);

                if(selector_bet == 0) spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Minus(), Color.Red * 0.50f);
                if(selector_bet == 1) spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + SK.Position_Bet_Plus(), Color.Red * 0.50f);

                spriteBatch.DrawString(SK.font_score, "BET:", SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(50, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "COIN:", SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(50, 120 - SK.font_score.MeasureString("COIN:").Y), Color.DarkGoldenrod);
                spriteBatch.DrawString(SK.font_score, multi + "x" + bet, SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(235 - SK.font_score.MeasureString(multi + "x" + bet).X, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + coins_old, SK.Position_DisplayEdge() + SK.Position_Bet_Grid() + new Vector2(235, 120) - SK.font_score.MeasureString("" + coins_old), Color.DarkGoldenrod);

            }

            if(active_spin || end) {
                spriteBatch.Draw(SK.texture_casino_bet_grid, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3(), Color.White);
            }

            if(active_spin && !end) {
                spriteBatch.DrawString(SK.font_score, multi + "x" + bet, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString(multi + "x" + bet).X, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + coins_old, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235, 120) - SK.font_score.MeasureString("" + coins_old), Color.DarkGoldenrod);
            }

            if(end) {
                if(coins_plus > 0)
                    spriteBatch.DrawString(SK.font_score, "+" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("+" + coins_plus).X, 45), Color.LimeGreen);
                if(coins_plus < 0)
                    spriteBatch.DrawString(SK.font_score, "" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("" + coins_plus).X, 45), Color.IndianRed);
                spriteBatch.DrawString(SK.font_score, "" + FM.coins, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235, 120) - SK.font_score.MeasureString("" + FM.coins), Color.DarkGoldenrod);
            }
        }

        public override void Draw3() {
            if(!active_gameover && !active_pause && !end) {
                spriteBatch.Draw(SK.texture_hud_button_spin, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
            }
            if(end) {
                spriteBatch.Draw(SK.texture_hud_button_replay, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
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
