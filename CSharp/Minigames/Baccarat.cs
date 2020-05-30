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
    class Baccarat : Ghost {

        List<Vector3> cards_player = new List<Vector3>();
        List<Vector3> cards_dealer = new List<Vector3>();

        int betmulti = 0;

        int value_player;
        int value_dealer;

        int selector_bet;
        int selector_card;

        bool betting;
        bool end;

        int status;

        bool active_player;

        int bet;

        public Baccarat(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            active_player = false;
            bet = 0;
            status = 0;
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
                if(betting) {
                    betting = false;
                    active_player = true;
                    JK.Noise("CardStart");
                    cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                    value_player = 0;
                    foreach(Vector3 v in cards_player) {
                        if(v.X == 13) {
                            value_player += 1;
                        } else if(v.X <= 9) {
                            value_player = value_player + (int)v.X + 1;
                        }
                    }
                    value_dealer = 0;
                    foreach(Vector3 v in cards_dealer) {
                        if(v.X == 13) {
                            value_dealer += 1;
                        } else if(v.X <= 9) {
                            value_dealer = value_dealer + (int)v.X + 1;
                        }
                    }
                    while(value_player >= 10) { value_player -= 10; }
                    while(value_dealer >= 10) { value_dealer -= 10; }
                    if(value_player >= 8 || value_dealer >= 8) {
                        status = 1;
                        Result();
                    } else {
                        status = 2;
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
                    if(selector_card == 0) Add_Card(true);
                    if(selector_card == 1) Add_Card(false);
                }
            }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(FM.purchased[FM.Convert("highroller")] == 1 ? betmulti < 4 : betmulti < 2) betmulti++; } }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(betmulti > 0) betmulti--; } }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet > 0) selector_bet--; } else { if(selector_card > 0) selector_card--; } }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { pressed_response = true; if(betting) { if(selector_bet < 1) selector_bet++; } else { if(selector_card < 1) selector_card++; } }
            if(ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                pressed_response = true;
                if(!betting && active_player) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Hit().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Hit().Y, SK.Collision_BlackJack_Hit().Width, SK.Collision_BlackJack_Hit().Height))) { if(selector_card == 0) { Add_Card(true); } else { selector_card = 0; } }
                    if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + SK.Collision_BlackJack_Stand().X, (int)SK.Position_DisplayEdge().Y + SK.Collision_BlackJack_Stand().Y, SK.Collision_BlackJack_Stand().Width, SK.Collision_BlackJack_Stand().Height))) { if(selector_card == 1) { Add_Card(false); } else { selector_card = 1; } }
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
            if(end && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void Add_Card(bool player) {
            if(player) {
                value_player = 0;
                JK.Noise("CardDraw");
                cards_player.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                foreach(Vector3 v in cards_player) {
                    if(v.X == 13) {
                        value_player += 1;
                    } else if(v.X <= 9) {
                        value_player = value_player + (int)v.X + 1;
                    }
                }
                while(value_player >= 10) { value_player -= 10; }
            }

            bool temp_draw = false;

            if(cards_player.Count == 2 || value_dealer <= 3) { temp_draw = true; } else if(value_dealer == 4 && value_player <= 7) { temp_draw = true; } else if(value_dealer == 5 && value_player >= 4 && value_player <= 7) { temp_draw = true; } else if(value_dealer == 6 && value_player >= 6 && value_player <= 7) { temp_draw = true; }

            if(temp_draw) {
                JK.Noise("CardDraw");
                cards_dealer.Add(new Vector3(random.Next(13) + 1, random.Next(4), 400));
                value_dealer = 0;
                foreach(Vector3 v in cards_dealer) {
                    if(v.X == 13) {
                        value_dealer += 1;
                    } else if(v.X <= 9) {
                        value_dealer = value_dealer + (int)v.X + 1;
                    }
                }
                while(value_dealer >= 10) { value_dealer -= 10; }
            }
            Result();
        }

        private void Result() {
            active_player = false;
            end = true;
            if(status == 2) status = 3;
            if(value_dealer > value_player) { coins_plus = -bet; }
            if(value_dealer < value_player) { coins_plus = bet; }
            if(value_dealer == value_player) { coins_plus = 0; }
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

                if(status == 1) spriteBatch.DrawString(SK.font_score, "Natural Draw!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("Natural Draw!") / 2 + new Vector2(0, -35), Color.Black);
                if(status == 2) spriteBatch.DrawString(SK.font_score, "continue drawing", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("continue drawing") / 2 + new Vector2(0, -35), Color.Black);
                if(status == 2) { }
                spriteBatch.DrawString(SK.font_score, "" + value_player, SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("" + value_player) / 2 + new Vector2(-25, 0), Color.Black);
                spriteBatch.DrawString(SK.font_score, "" + value_dealer, SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("" + value_dealer) / 2 + new Vector2(25, 0), Color.Black);
            }

            if(!betting && end) {
                if(value_dealer < value_player) spriteBatch.DrawString(SK.font_score, "The Player Wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The Player Wins!") / 2 + new Vector2(0, 35), Color.Black);
                if(value_dealer > value_player) spriteBatch.DrawString(SK.font_score, "The House Wins!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("The House Wins!") / 2 + new Vector2(0, 35), Color.Black);
                if(value_dealer == value_player) spriteBatch.DrawString(SK.font_score, "Tie!", SK.Position_DisplayEdge() + SK.Get_GridSize() / 2 - SK.font_score.MeasureString("Tie!") / 2 + new Vector2(0, 35), Color.Black);
            }

            if(!betting) {
                spriteBatch.Draw(SK.texture_casino_bet_grid, SK.Position_DisplayEdge() + SK.Position_Bet_Grid2(), Color.White);
            }

            if(!betting && !end) {
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
            if(!active_pause && !active_gameover) {
                if(betting) {
                    spriteBatch.Draw(SK.texture_hud_button_yes, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                }
                if(end) {
                    spriteBatch.Draw(SK.texture_hud_button_replay, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                }
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
