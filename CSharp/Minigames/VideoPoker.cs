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
    class VideoPoker : Ghost {

        bool end;
        bool betting;

        int betmulti = 0;

        string hand;

        bool movingIN;
        int move;

        Vector2 card1;
        Vector2 card2;
        Vector2 card3;
        Vector2 card4;
        Vector2 card5;

        bool hold1;
        bool hold2;
        bool hold3;
        bool hold4;
        bool hold5;

        bool transition_up;
        float trans0;
        float trans1;
        float trans2;
        float trans3;
        float trans4;
        float trans5;

        int selector_hold;
        int selector_bet;

        bool active_transition;

        int bet;

        public VideoPoker(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            active_transition = false;
            transition_up = true;
            movingIN = true;
            move = 0;
            card1 = new Vector2(random.Next(13) + 1, random.Next(4));
            card2 = new Vector2(random.Next(13) + 1, random.Next(4));
            card3 = new Vector2(random.Next(13) + 1, random.Next(4));
            card4 = new Vector2(random.Next(13) + 1, random.Next(4));
            card5 = new Vector2(random.Next(13) + 1, random.Next(4));
            hold1 = false;
            hold2 = false;
            hold3 = false;
            hold4 = false;
            hold5 = false;
            trans0 = 0.00f;
            trans1 = 0.00f;
            trans2 = 0.00f;
            trans3 = 0.00f;
            trans4 = 0.00f;
            trans5 = 0.00f;
            bet = 0;
            end = false;
            betting = true;
            hand = "empty";
            selector_bet = 1;
            selector_hold = 0;
        }

        public override string Update2() {
            if(move != 0) {
                if(movingIN) {
                    move += 5;
                    if(move == 165) {
                        movingIN = false;
                        Sort();
                    }
                } else {
                    move -= 5;
                    if(move == 0) {
                        Result();
                    }
                }
            } else if(active_transition) {
                if(transition_up) {
                    if(trans0 < 2.00f) {
                        trans0 = trans0 + 0.05f;
                        if(betting) {
                            trans1 = trans1 + 0.05f;
                            trans2 = trans2 + 0.05f;
                            trans3 = trans3 + 0.05f;
                            trans4 = trans4 + 0.05f;
                            trans5 = trans5 + 0.05f;
                        } else {
                            if(!hold1) trans1 = trans1 + 0.05f;
                            if(!hold2) trans2 = trans2 + 0.05f;
                            if(!hold3) trans3 = trans3 + 0.05f;
                            if(!hold4) trans4 = trans4 + 0.05f;
                            if(!hold5) trans5 = trans5 + 0.05f;
                        }
                    } else {

                        transition_up = false;
                        active_transition = false;
                        if(betting) {
                            betting = false;
                        } else {
                            move += 5;
                            movingIN = true;
                        }
                    }
                } else {
                    if(trans0 > 0.00f) {
                        trans0 = trans0 - 0.05f;
                        if(betting) {
                            trans1 = trans1 - 0.05f;
                            trans2 = trans2 - 0.05f;
                            trans3 = trans3 - 0.05f;
                            trans4 = trans4 - 0.05f;
                            trans5 = trans5 - 0.05f;
                        } else {
                            if(!hold1) trans1 = trans1 - 0.05f;
                            if(!hold2) trans2 = trans2 - 0.05f;
                            if(!hold3) trans3 = trans3 - 0.05f;
                            if(!hold4) trans4 = trans4 - 0.05f;
                            if(!hold5) trans5 = trans5 - 0.05f;
                        }
                    } else {
                        transition_up = true;
                        if(!hold1) card1 = new Vector2(random.Next(13) + 1, random.Next(4));
                        if(!hold2) card2 = new Vector2(random.Next(13) + 1, random.Next(4));
                        if(!hold3) card3 = new Vector2(random.Next(13) + 1, random.Next(4));
                        if(!hold4) card4 = new Vector2(random.Next(13) + 1, random.Next(4));
                        if(!hold5) card5 = new Vector2(random.Next(13) + 1, random.Next(4));
                    }
                }
            } else {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transition = true; pressed_response = true; }
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
                        if(selector_hold == 1) { if(hold1) hold1 = false; else { hold1 = true; } }
                        if(selector_hold == 2) { if(hold2) hold2 = false; else { hold2 = true; } }
                        if(selector_hold == 3) { if(hold3) hold3 = false; else { hold3 = true; } }
                        if(selector_hold == 4) { if(hold4) hold4 = false; else { hold4 = true; } }
                        if(selector_hold == 5) { if(hold5) hold5 = false; else { hold5 = true; } }
                    }
                }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; } }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(betmulti > 0) betmulti--; } }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet > 0) selector_bet--; } else { if(selector_hold > 1) selector_hold--; } }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet < 1) selector_bet++; } else { if(selector_hold < 5) selector_hold++; } }
                if(ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                    pressed_response = true;
                    if(!betting) {
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold1().Y, 150, 500))) { selector_hold = 1; if(hold1) { hold1 = false; } else { hold1 = true; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold2().Y, 150, 500))) { selector_hold = 2; if(hold2) { hold2 = false; } else { hold2 = true; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold3().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold3().Y, 150, 500))) { selector_hold = 3; if(hold3) { hold3 = false; } else { hold3 = true; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold4().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold4().Y, 150, 500))) { selector_hold = 4; if(hold4) { hold4 = false; } else { hold4 = true; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold5().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold5().Y, 150, 500))) { selector_hold = 5; if(hold5) { hold5 = false; } else { hold5 = true; } }
                    } else {
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { bet = bet - Get_Bet(); if(bet < 0) bet = 0; selector_bet = 0; }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { bet = bet + Get_Bet(); if(bet > coins_old) bet = coins_old; selector_bet = 1; }

                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up1().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up2().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down1().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down2().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                    }
                }
                if(control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                    if(!betting) {
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold1().Y, 150, 500))) { selector_hold = 1; }
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold2().Y, 150, 500))) { selector_hold = 2; }
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold3().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold3().Y, 150, 500))) { selector_hold = 3; }
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold4().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold4().Y, 150, 500))) { selector_hold = 4; }
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_VideoPoker_Hold5().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_VideoPoker_Hold5().Y, 150, 500))) { selector_hold = 5; }
                    } else {
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { selector_bet = 0; }
                        if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { selector_bet = 1; }
                    }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(end && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Sort() {
            if(card1.X > card5.X) {
                Vector2 z = card1;
                card1 = card2;
                card2 = card3;
                card3 = card4;
                card4 = card5;
                card5 = z;
            }
            if(card1.X > card4.X) {
                Vector2 z = card1;
                card1 = card2;
                card2 = card3;
                card3 = card4;
                card4 = z;
            }
            if(card1.X > card3.X) {
                Vector2 z = card1;
                card1 = card2;
                card2 = card3;
                card3 = z;
            }
            if(card1.X > card2.X) {
                Vector2 z = card1;
                card1 = card2;
                card2 = z;
            }
            if(card2.X > card5.X) {
                Vector2 z = card2;
                card2 = card3;
                card3 = card4;
                card4 = card5;
                card5 = z;
            }
            if(card2.X > card4.X) {
                Vector2 z = card2;
                card2 = card3;
                card3 = card4;
                card4 = z;
            }
            if(card2.X > card3.X) {
                Vector2 z = card2;
                card2 = card3;
                card3 = z;
            }
            if(card3.X > card5.X) {
                Vector2 z = card3;
                card3 = card4;
                card4 = card5;
                card5 = z;
            }
            if(card3.X > card4.X) {
                Vector2 z = card3;
                card3 = card4;
                card4 = z;
            }
            if(card4.X > card5.X) {
                Vector2 z = card4;
                card4 = card5;
                card5 = z;
            }
        }

        private void Result() {
            if(card1.X == 9 && card2.X == 10 && card3.X == 11 && card4.X == 12 && card5.X == 13 && card1.Y == card2.Y && card1.Y == card3.Y && card1.Y == card4.Y && card1.Y == card5.Y) {
                hand = "ROYAL FLUSH!!";
                coins_plus = bet * 256;
            } else if(card1.X <= 9 && card1.X + 1 == card2.X && card1.X + 2 == card3.X && card1.X + 3 == card4.X && card1.X + 4 == card5.X && card1.Y == card2.Y && card1.Y == card3.Y && card1.Y == card4.Y && card1.Y == card5.Y) {
                hand = "Straight Flush";
                coins_plus = bet * 51;
            } else if(card1.X == card2.X && card1.X == card3.X && card1.X == card4.X && card1.X != card5.X) {
                hand = "4 of a Kind";
                coins_plus = bet * 26;
            } else if(card2.X == card3.X && card2.X == card4.X && card2.X == card5.X && card2.X != card1.X) {
                hand = "4 of a Kind";
                coins_plus = bet * 26;
            } else if(card1.X == card2.X && card1.X == card3.X && card1.X != card4.X && card4.X == card5.X) {
                hand = "Full House";
                coins_plus = bet * 9;
            } else if(card1.X == card2.X && card1.X != card3.X && card3.X == card4.X && card3.X == card5.X) {
                hand = "Full House";
                coins_plus = bet * 9;
            } else if(card1.Y == card2.Y && card1.Y == card3.Y && card1.Y == card4.Y && card1.Y == card5.Y) {
                hand = "Flush";
                coins_plus = bet * 9;
            } else if(card1.X <= 9 && card1.X + 1 == card2.X && card1.X + 2 == card3.X && card1.X + 3 == card4.X && card1.X + 4 == card5.X) {
                hand = "Straight";
                coins_plus = bet * 4;
            } else if(card1.X == card2.X && card1.X == card3.X && card1.X != card4.X && card1.X != card5.X) {
                hand = "3 of a Kind";
                coins_plus = bet * 3;
            } else if(card2.X == card3.X && card2.X == card4.X && card2.X != card1.X && card2.X != card5.X) {
                hand = "3 of a Kind";
                coins_plus = bet * 3;
            } else if(card3.X == card4.X && card3.X == card5.X && card3.X != card1.X && card3.X != card2.X) {
                hand = "3 of a Kind";
                coins_plus = bet * 3;
            } else if(card1.X == card2.X && card3.X == card4.X) {
                hand = "Two Pair";
                coins_plus = bet * 2;
            } else if(card1.X == card2.X && card4.X == card5.X) {
                hand = "Two Pair";
                coins_plus = bet * 2;
            } else if(card2.X == card3.X && card4.X == card5.X) {
                hand = "Two Pair";
                coins_plus = bet * 2;
            } else if(card1.X >= 10 && card1.X == card2.X) {
                hand = "Jacks or Better";
                coins_plus = bet * 1;
            } else if(card2.X >= 10 && card2.X == card3.X) {
                hand = "Jacks or Better";
                coins_plus = bet * 1;
            } else if(card3.X >= 10 && card3.X == card4.X) {
                hand = "Jacks or Better";
                coins_plus = bet * 1;
            } else if(card4.X >= 10 && card4.X == card5.X) {
                hand = "Jacks or Better";
                coins_plus = bet * 1;
            } else {

            }
            coins_plus = coins_plus - bet;
            end = true;
        }

        public override void Draw2() {

            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.PaleTurquoise); } else { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.PaleTurquoise); }

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold1() + new Vector2(70 + move * 2, 150), new Rectangle(1 + (int)(71 * card1.X), 1 + (int)(96 * card1.Y), 70, 95), Color.White, 0.0f, new Vector2(35, 47), trans1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold2() + new Vector2(70 + move, 150), new Rectangle(1 + (int)(71 * card2.X), 1 + (int)(96 * card2.Y), 70, 95), Color.White, 0.0f, new Vector2(35, 47), trans2, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold3() + new Vector2(70, 150), new Rectangle(1 + (int)(71 * card3.X), 1 + (int)(96 * card3.Y), 70, 95), Color.White, 0.0f, new Vector2(35, 47), trans3, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold4() + new Vector2(70 - move, 150), new Rectangle(1 + (int)(71 * card4.X), 1 + (int)(96 * card4.Y), 70, 95), Color.White, 0.0f, new Vector2(35, 47), trans4, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold5() + new Vector2(70 - move * 2, 150), new Rectangle(1 + (int)(71 * card5.X), 1 + (int)(96 * card5.Y), 70, 95), Color.White, 0.0f, new Vector2(35, 47), trans5, SpriteEffects.None, 0.0f);

            if(hold1) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold1(), Color.White); } else { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold1(), Color.Gray * 0.25f); }
            if(hold2) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold2(), Color.White); } else { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold2(), Color.Gray * 0.25f); }
            if(hold3) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold3(), Color.White); } else { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold3(), Color.Gray * 0.25f); }
            if(hold4) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold4(), Color.White); } else { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold4(), Color.Gray * 0.25f); }
            if(hold5) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold5(), Color.White); } else { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold5(), Color.Gray * 0.25f); }

            if(!betting && !end) {
                if(selector_hold == 1) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold1(), Color.Pink * 0.50f); }
                if(selector_hold == 2) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold2(), Color.Pink * 0.50f); }
                if(selector_hold == 3) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold3(), Color.Pink * 0.50f); }
                if(selector_hold == 4) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold4(), Color.Pink * 0.50f); }
                if(selector_hold == 5) { spriteBatch.Draw(SK.texture_static_videopoker_hold, SK.Position_DisplayEdge() + SK.Position_VideoPoker_Hold5(), Color.Pink * 0.50f); }
            }

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
                spriteBatch.DrawString(SK.font_score, hand, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.font_score.MeasureString(hand).X / 2, SK.Position_Bet_Grid3().Y - 25), Color.Black);
                if(coins_plus > 0)
                    spriteBatch.DrawString(SK.font_score, "+" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("+" + coins_plus).X, 45), Color.LimeGreen);
                if(coins_plus < 0)
                    spriteBatch.DrawString(SK.font_score, "" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235 - SK.font_score.MeasureString("" + coins_plus).X, 45), Color.IndianRed);
                spriteBatch.DrawString(SK.font_score, "" + FM.coins, SK.Position_DisplayEdge() + SK.Position_Bet_Grid3() + new Vector2(235, 120) - SK.font_score.MeasureString("" + FM.coins), Color.DarkGoldenrod);
            }
        }

        public override void Draw3() {
            if(!active_gameover && !active_pause && !end) {
                spriteBatch.Draw(SK.texture_hud_button_yes, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
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
