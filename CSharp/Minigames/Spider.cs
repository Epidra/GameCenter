﻿using System;
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
    class Spider : Ghost {

        Vector2 selector_card = new Vector2(-1, -1);
        Vector2 selector_pos;

        List<Vector2>[] cards_field = new List<Vector2>[10];
        List<Vector2>[] cards_reserve = new List<Vector2>[5];
        List<Vector2> cards_finish = new List<Vector2>();

        public Spider(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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

        private List<Vector2> ShuffleDeck() {
            List<Vector2> stack = new List<Vector2>();
            List<Vector2> deck = new List<Vector2>();

            for(int y = 0; y < 4; y++) {
                for(int x = 0; x < 13; x++) {
                    stack.Add(new Vector2(x + 1, game_difficulty == 1 ? 0 : game_difficulty == 2 ? y/2 : y));
                    stack.Add(new Vector2(x + 1, game_difficulty == 1 ? 0 : game_difficulty == 2 ? y / 2 : y));
                }
            }

            while(stack.Count > 0) {
                int r = random.Next(stack.Count - 1);
                deck.Add(stack[r]);
                stack.RemoveAt(r);
            }

            return deck;
        }

        public override void Start2() {

            List<Vector2> deck = ShuffleDeck();

            cards_field[0] = new List<Vector2>(); cards_field[0].AddRange(deck.GetRange(0, 6)); deck.RemoveRange(0, 6);
            cards_field[1] = new List<Vector2>(); cards_field[1].AddRange(deck.GetRange(0, 6)); deck.RemoveRange(0, 6);
            cards_field[2] = new List<Vector2>(); cards_field[2].AddRange(deck.GetRange(0, 6)); deck.RemoveRange(0, 6);
            cards_field[3] = new List<Vector2>(); cards_field[3].AddRange(deck.GetRange(0, 6)); deck.RemoveRange(0, 6);
            cards_field[4] = new List<Vector2>(); cards_field[4].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[5] = new List<Vector2>(); cards_field[5].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[6] = new List<Vector2>(); cards_field[6].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[7] = new List<Vector2>(); cards_field[7].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[8] = new List<Vector2>(); cards_field[8].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[9] = new List<Vector2>(); cards_field[9].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);

            cards_reserve[0] = new List<Vector2>(); cards_reserve[0].AddRange(deck.GetRange(0, 10)); deck.RemoveRange(0, 10);
            cards_reserve[1] = new List<Vector2>(); cards_reserve[1].AddRange(deck.GetRange(0, 10)); deck.RemoveRange(0, 10);
            cards_reserve[2] = new List<Vector2>(); cards_reserve[2].AddRange(deck.GetRange(0, 10)); deck.RemoveRange(0, 10);
            cards_reserve[3] = new List<Vector2>(); cards_reserve[3].AddRange(deck.GetRange(0, 10)); deck.RemoveRange(0, 10);
            cards_reserve[4] = new List<Vector2>(); cards_reserve[4].AddRange(deck.GetRange(0, 10)); deck.RemoveRange(0, 10);

            selector_card = new Vector2(-1, -1);
            selector_pos = new Vector2(0, 0);
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(selector_pos.Y > 0) selector_pos.Y--; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(selector_pos.Y < 15) selector_pos.Y++; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(selector_pos.X > 0) selector_pos.X--; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(selector_pos.X < 20) selector_pos.X++; }

            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && pressed_event_touch) {
                for(int y = 0; y < 14; y++) {
                    for(int x = 0; x < 20; x++) {
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * x, (int)SK.Position_DisplayEdge().Y + 48 * y, 35, 48))) { TouchField(x, y); }
                    }
                }
                //if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 7, (int)SK.Position_DisplayEdge().Y + 48 * 12, 35 * 2, 48 * 2))) { if(cards_stack.Count > 0) CompareCards(28); }

                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 16, (int)SK.Position_DisplayEdge().Y + 48 * 12, 35 * 8, 48 * 2))) { DrawReserve(); ; }
            }

            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) {
                if(selector_pos.Y < 14) {
                    TouchField((int)selector_pos.X, (int)selector_pos.Y);
                }
                //if(selector_pos == new Vector2(7, 12)) if(cards_stack.Count > 0) CompareCards(28);
                if(selector_pos == new Vector2(15, 12)) DrawReserve();
                if(selector_pos == new Vector2(16, 12)) DrawReserve();
                if(selector_pos == new Vector2(17, 12)) DrawReserve();
                if(selector_pos == new Vector2(18, 12)) DrawReserve();
                if(selector_pos == new Vector2(19, 12)) DrawReserve();
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(cards_finish.Count == 8 && !active_gameover) {
                score_points = 100;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void DrawReserve() {
            if(cards_reserve[0].Count > 0) {
                for(int x = 0; x < 10; x++) {
                    cards_field[x].Add(cards_reserve[0][x]);
                }
                cards_reserve[0].Clear();
            } else if(cards_reserve[1].Count > 0) {
                for(int x = 0; x < 10; x++) {
                    cards_field[x].Add(cards_reserve[1][x]);
                }
                cards_reserve[1].Clear();
            } else if(cards_reserve[2].Count > 0) {
                for(int x = 0; x < 10; x++) {
                    cards_field[x].Add(cards_reserve[2][x]);
                }
                cards_reserve[2].Clear();
            } else if(cards_reserve[3].Count > 0) {
                for(int x = 0; x < 10; x++) {
                    cards_field[x].Add(cards_reserve[3][x]);
                }
                cards_reserve[3].Clear();
            } else if(cards_reserve[4].Count > 0) {
                for(int x = 0; x < 10; x++) {
                    cards_field[x].Add(cards_reserve[4][x]);
                }
                cards_reserve[4].Clear();
            }
        }

        private void TouchField(int x, int y) {
            if(cards_field[x/2].Count >= y - 1) {
                if(selector_card == new Vector2(-1, -1)) {
                    int x2 = x / 2;
                    int y2 = cards_field[x/2].Count < y ? cards_field[x/2].Count-1 : y - 1;
                    float tempCard = cards_field[x2][y2].X;
                    for(int i = y2; i < cards_field[x2].Count; i++) {
                        if(i != cards_field[x2].Count - 1) {
                            if((cards_field[x2][i].X - 1 != cards_field[x2][i + 1].X) && !(cards_field[x2][i].X == 1 && cards_field[x2][i + 1].X == 13)) {
                                return;
                            }
                        }
                    }
                    selector_card = new Vector2(x2, y2);
                } else {
                    if(!MoveStack(x, y)) {
                        selector_card = new Vector2(-1, -1);
                    }
                }
            }
        }

        private bool MoveStack(int x, int y) {
            int x2 = x / 2;
            int y2 = cards_field[x2].Count - 1;
            if(cards_field[x2].Count == 0) {
                cards_field[x2].AddRange(cards_field[(int)selector_card.X].GetRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y));
                cards_field[(int)selector_card.X].RemoveRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y);
                selector_card = new Vector2(-1, -1);
                ClearRow(x2);
                return true;
            } else {
                if(cards_field[(int)selector_card.X][(int)selector_card.Y].X +1 == cards_field[x2][y2].X || (cards_field[(int)selector_card.X][(int)selector_card.Y].X == 13 && cards_field[x2][y2].X == 1)) {
                    cards_field[x2].AddRange(cards_field[(int)selector_card.X].GetRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y));
                    cards_field[(int)selector_card.X].RemoveRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y);
                    selector_card = new Vector2(-1, -1);
                    ClearRow(x2);
                    return true;
                }
            }
            return false;
        }

        private void ClearRow(int row) {
            if(cards_field[row].Count >= 13) {
                for(int i = 0; i < cards_field[row].Count; i++) {
                    if(cards_field[row][i].X == 12) {
                        for(int j = 0; j < cards_field[row].Count-1; j++) {
                            if(12 - j == 0 && cards_field[row][i + j].X == 13 && cards_field[row][i].Y == cards_field[row][i + j].Y) {
                                cards_finish.Add(cards_field[row][cards_field[row].Count - 1]);
                                cards_field[row].RemoveRange(cards_field[row].Count - 13, 13);
                                return;
                            }
                            if(12 - j != cards_field[row][i + j].X || cards_field[row][i].Y != cards_field[row][i + j].Y) break;
                            
                        }
                    }
                }
            }
        }

        public override void Draw2() {
            for(int x = 0; x < 10; x++) {
                for(int y = 0; y < cards_field[x].Count; y++) {
                    //if(y == cards_field[x].Count - 1) {
                        spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * x*2, 48 * 1 + 48 * y), new Rectangle(1 + (int)(71 * cards_field[x][y].X), 1 + (int)(96 * cards_field[x][y].Y), 70, 95), selector_card == new Vector2(x, y) ? Color.Gold : Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    //} else {
                    //    spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 0, 48 * 1 + 48 * y), new Rectangle(1, 1, 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    //}
                }
            }

            if(cards_reserve[4].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 15, 48 * 12), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_reserve[3].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 16, 48 * 12), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_reserve[2].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 17, 48 * 12), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_reserve[1].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 18, 48 * 12), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_reserve[0].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 19, 48 * 12), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            int i = 0;
            foreach(Vector2 v in cards_finish) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 2 + 35*i, 48 * 12), new Rectangle(1 + (int)(71 * v.X), 1 + (int)(96 * v.Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                i++;
            }

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * selector_pos.X, 48 * selector_pos.Y), new Rectangle(1, 1, 70, 95), Color.Blue * 0.25f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
        }

    }
}
