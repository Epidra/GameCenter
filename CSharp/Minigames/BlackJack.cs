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
    class BlackJack : Ghost {

        List<Vector3> cards_player = new List<Vector3>();
        List<Vector3> cards_dealer = new List<Vector3>();

        int hiddencard;

        int betmulti = 0;

        int value_player;
        int value_dealer;

        int selector_bet;
        int selector_card;

        bool betting;
        bool end;

        bool active_player;

        int bet;

        public BlackJack(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            cards_player.RemoveRange(0, cards_player.Count);
            cards_dealer.RemoveRange(0, cards_dealer.Count);
            value_player = 0;
            value_dealer = 0;
            selector_bet = 1;
            selector_card = 0;
            betting = true;
            end = false;
            active_player = true;
            bet = 0;
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
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) {
                pressed_response = true;
                if(betting) {
                    betting = false;
                    cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    JK.Noise("CardStart");
                    hiddencard = 400;
                    value_player = 0;
                    int ace = 0;
                    foreach(Vector3 v in cards_player) {
                        if(v.X == 13) {
                            ace++;
                        } else if(v.X <= 9) {
                            value_player = value_player + (int)v.X + 1;
                        } else {
                            value_player = value_player + 10;
                        }
                    }
                    if(ace > 0) {
                        while(ace > 0) {
                            if(value_player <= 10) {
                                value_player = value_player + 11;
                            } else {
                                value_player = value_player + 1;
                            }
                            ace--;
                        }
                    }
                    if(value_player == 21) {
                        end = true;
                        active_player = false;
                        coins_plus = bet * 2;
                    }
                }
            }
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
                if(!betting && active_player) {
                    if(selector_card == 0) Add_Card();
                    if(selector_card == 1) active_player = false;
                }
            }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; } }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(betmulti > 0) betmulti--; } }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet > 0) selector_bet--; } else { if(selector_card > 0) selector_card--; } }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet < 1) selector_bet++; } else { if(selector_card < 1) selector_card++; } }
            if(ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                pressed_response = true;
                if(!betting && active_player) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Hit().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Hit().Y, SK.Collision_BlackJack_Hit().Width, SK.Collision_BlackJack_Hit().Height))) { if(selector_card == 0) { Add_Card(); } else { selector_card = 0; } }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Stand().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Stand().Y, SK.Collision_BlackJack_Stand().Width, SK.Collision_BlackJack_Stand().Height))) { if(selector_card == 1) { active_player = false; } else { selector_card = 1; } }
                }
                if(betting && !end) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { bet = bet - Get_Bet(); if(bet < 0) bet = 0; selector_bet = 0; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { bet = bet + Get_Bet(); if(bet > coins_old) bet = coins_old; selector_bet = 1; }

                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up1().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Up2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Up2().Y, SK.texture_casino_bet_up.Width, SK.texture_casino_bet_up.Height))) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down1().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down1().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Down2().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Down2().Y, SK.texture_casino_bet_down.Width, SK.texture_casino_bet_down.Height))) { if(betmulti > 0) betmulti--; }
                }
            }
            if(control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                if(!betting && active_player) {
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Hit().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Hit().Y, SK.Collision_BlackJack_Hit().Width, SK.Collision_BlackJack_Hit().Height))) { selector_card = 0; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Stand().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Stand().Y, SK.Collision_BlackJack_Hit().Width, SK.Collision_BlackJack_Hit().Height))) { selector_card = 1; }
                }
                if(betting && !end) {
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Minus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Minus().Y, SK.texture_casino_bet_minus.Width, SK.texture_casino_bet_minus.Height))) { selector_bet = 0; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Bet_Plus().X, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Bet_Plus().Y, SK.texture_casino_bet_plus.Width, SK.texture_casino_bet_plus.Height))) { selector_bet = 1; }
                }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            for(int i = 0; i < cards_player.Count; i++) { if(cards_player[i].Z > 0) cards_player[i] = new Vector3(cards_player[i].X, cards_player[i].Y, cards_player[i].Z - 5); }
            for(int i = 0; i < cards_dealer.Count; i++) { if(cards_dealer[i].Z > 0) cards_dealer[i] = new Vector3(cards_dealer[i].X, cards_dealer[i].Y, cards_dealer[i].Z - 5); }
            if(hiddencard > 0) hiddencard -= 5;
            if(!active_player && !betting && !end) {
                Add_Card();
            }
            if(end && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Add_Card() {
            if(active_player) {
                JK.Noise("CardDraw");
                value_player = 0;
                cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                int ace = 0;
                foreach(Vector3 v in cards_player) {
                    if(v.X == 13) {
                        ace++;
                    } else if(v.X <= 9) {
                        value_player = value_player + (int)v.X + 1;
                    } else {
                        value_player = value_player + 10;
                    }
                }
                if(ace > 0) {
                    while(ace > 0) {
                        if(value_player <= 10) {
                            value_player = value_player + 11;
                        } else {
                            value_player = value_player + 1;
                        }
                        ace--;
                    }
                }
                if(value_player > 21) {
                    active_player = false;
                    end = true;
                }
            } else {
                value_dealer = 0;
                int ace = 0;
                foreach(Vector3 v in cards_dealer) {
                    if(v.X == 13) {
                        ace++;
                    } else if(v.X <= 9) {
                        value_dealer = value_dealer + (int)v.X + 1;
                    } else {
                        value_dealer = value_dealer + 10;
                    }
                }
                if(ace > 0) {
                    while(ace > 0) {
                        if(value_dealer <= 10) {
                            value_dealer = value_dealer + 11;
                        } else {
                            value_dealer = value_dealer + 1;
                        }
                        ace--;
                    }
                }
                if(value_dealer < 17) {
                    value_dealer = 0;
                    JK.Noise("CardDraw");
                    cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    ace = 0;
                    foreach(Vector3 v in cards_dealer) {
                        if(v.X == 13) {
                            ace++;
                        } else if(v.X <= 9) {
                            value_dealer = value_dealer + (int)v.X + 1;
                        } else {
                            value_dealer = value_dealer + 10;
                        }
                    }
                    if(ace > 0) {
                        while(ace > 0) {
                            if(value_dealer <= 10) {
                                value_dealer = value_dealer + 11;
                            } else {
                                value_dealer = value_dealer + 1;
                            }
                            ace--;
                        }
                    }
                }
                if(value_dealer > 16) end = true;
            }
            if(end) {
                if(value_dealer > 21) {
                    coins_plus = bet;
                } else if(value_player > 21) {
                    coins_plus = -bet;
                } else if(value_player == value_dealer && cards_player.Count > cards_dealer.Count) {
                    coins_plus = bet;
                } else if(value_player == value_dealer && cards_player.Count == cards_dealer.Count) {
                    coins_plus = 0;
                } else if(value_player == value_dealer && cards_player.Count < cards_dealer.Count) {
                    coins_plus = -bet;
                } else if(value_player > value_dealer) {
                    coins_plus = bet;
                } else if(value_player < value_dealer) {
                    coins_plus = -bet;
                }
            }
        }

        public override void Draw2() {
            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.LightBlue); } else { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.LightBlue); }
            int i = 0;
            foreach(Vector3 v in cards_player) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 9 + 50 * i, SK.Get_GridSize().Y / 9 * 4 + 20 * i + v.Z), new Rectangle(1 + (int)(71 * v.X), 1 + (int)(96 * v.Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 2, SpriteEffects.None, 0.0f);
                i++;
            }
            i = 0;
            foreach(Vector3 v in cards_dealer) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 9 * 5 + 50 * i + v.Z, SK.Get_GridSize().Y / 9 + 15 * i), new Rectangle(1 + (int)(71 * v.X), 1 + (int)(96 * v.Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 2, SpriteEffects.None, 0.0f);
                i++;
            }
            if(active_player && !betting) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 9 * 5 + 50 + hiddencard, SK.Get_GridSize().Y / 9 + 15), new Rectangle(1, 1, 70, 95), Color.White, 0.0f, new Vector2(0, 0), 2, SpriteEffects.None, 0.0f);
            }
            if(betting) {
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

            } else {
                if(!end) {
                    if(selector_card == 0) spriteBatch.Draw(SK.texture_static_blackjack_hit, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Hit().X, SK.Collision_BlackJack_Hit().Y), Color.White);
                    if(selector_card == 1) spriteBatch.Draw(SK.texture_static_blackjack_hit, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Hit().X, SK.Collision_BlackJack_Hit().Y), Color.White * 0.20f);
                    if(selector_card == 0) spriteBatch.Draw(SK.texture_static_blackjack_stand, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Stand().X, SK.Collision_BlackJack_Stand().Y), Color.White * 0.20f);
                    if(selector_card == 1) spriteBatch.Draw(SK.texture_static_blackjack_stand, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Stand().X, SK.Collision_BlackJack_Stand().Y), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_static_blackjack_hit, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Hit().X, SK.Collision_BlackJack_Hit().Y), Color.White * 0.20f);
                    spriteBatch.Draw(SK.texture_static_blackjack_stand, SK.Position_DisplayEdge() + new Vector2(SK.Collision_BlackJack_Stand().X, SK.Collision_BlackJack_Stand().Y), Color.White * 0.20f);
                }
            }

            if(!betting && end) {
                if(value_dealer > 21) {
                    spriteBatch.DrawString(SK.font_score, "The House gone bust!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The House gone bust!") / 2, Color.Black);
                } else if(value_player > 21) {
                    spriteBatch.DrawString(SK.font_score, "The Player gone bust!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The Player gone bust!") / 2, Color.Black);
                } else if(value_player == value_dealer && cards_player.Count > cards_dealer.Count) {
                    spriteBatch.DrawString(SK.font_score, "The Player wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The Player wins!") / 2, Color.Black);
                } else if(value_player == value_dealer && cards_player.Count == cards_dealer.Count) {
                    spriteBatch.DrawString(SK.font_score, "DRAW", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("DRAW") / 2, Color.Black);
                } else if(value_player == 21 && cards_player.Count == 2) {
                    spriteBatch.DrawString(SK.font_score, "BLACK JACK", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("BLACK JACK") / 2, Color.Black);
                } else if(value_player == value_dealer && cards_player.Count < cards_dealer.Count) {
                    spriteBatch.DrawString(SK.font_score, "The House wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The House wins!") / 2, Color.Black);
                } else if(value_player > value_dealer) {
                    spriteBatch.DrawString(SK.font_score, "The Player wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The Player wins!") / 2, Color.Black);
                } else {
                    spriteBatch.DrawString(SK.font_score, "The House wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The House wins!") / 2, Color.Black);
                }
            }

            if(!betting) {
                spriteBatch.Draw(SK.texture_casino_bet_grid, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2(), Color.White);
            }

            if(!betting && !end) {
                spriteBatch.DrawString(SK.font_score, "" + value_player, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(50, 45), Color.LightGray);
                spriteBatch.DrawString(SK.font_score, "" + bet, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(235 - SK.font_score.MeasureString("" + bet).X, 45), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + coins_old, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(235, 120) - SK.font_score.MeasureString("" + coins_old), Color.DarkGoldenrod);
            }

            if(!betting && end) {
                if(coins_plus > 0)
                    spriteBatch.DrawString(SK.font_score, "+" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(235 - SK.font_score.MeasureString("+" + coins_plus).X, 45), Color.LimeGreen);
                if(coins_plus < 0)
                    spriteBatch.DrawString(SK.font_score, "" + coins_plus, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(235 - SK.font_score.MeasureString("" + coins_plus).X, 45), Color.IndianRed);
                spriteBatch.DrawString(SK.font_score, "" + FM.coins, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2() + new Vector2(235, 120) - SK.font_score.MeasureString("" + FM.coins), Color.DarkGoldenrod);
            }
        }

        public override void Draw3() {
            if(betting) {
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
