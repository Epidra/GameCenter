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
    class Klondike : Ghost {

        Vector2 selector_card = new Vector2(-1, -1);
        Vector2 selector_pos;

        List<Vector2>[] cards_field = new List<Vector2>[7];
        List<Vector2> cards_reserve = new List<Vector2>();
        List<Vector2> cards_stack = new List<Vector2>();
        List<Vector2>[] cards_finish = new List<Vector2>[4];

        public Klondike(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
                    stack.Add(new Vector2(x + 1, y));
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

            cards_field[0] = new List<Vector2>(); cards_field[0].AddRange(deck.GetRange(0, 1)); deck.RemoveRange(0, 1);
            cards_field[1] = new List<Vector2>(); cards_field[1].AddRange(deck.GetRange(0, 2)); deck.RemoveRange(0, 2);
            cards_field[2] = new List<Vector2>(); cards_field[2].AddRange(deck.GetRange(0, 3)); deck.RemoveRange(0, 3);
            cards_field[3] = new List<Vector2>(); cards_field[3].AddRange(deck.GetRange(0, 4)); deck.RemoveRange(0, 4);
            cards_field[4] = new List<Vector2>(); cards_field[4].AddRange(deck.GetRange(0, 5)); deck.RemoveRange(0, 5);
            cards_field[5] = new List<Vector2>(); cards_field[5].AddRange(deck.GetRange(0, 6)); deck.RemoveRange(0, 6);
            cards_field[6] = new List<Vector2>(); cards_field[6].AddRange(deck.GetRange(0, 7)); deck.RemoveRange(0, 7);

            cards_reserve.Clear();
            cards_reserve.AddRange(deck);
            cards_stack.Clear();


            cards_finish[0] = new List<Vector2>();
            cards_finish[1] = new List<Vector2>();
            cards_finish[2] = new List<Vector2>();
            cards_finish[3] = new List<Vector2>();

            selector_card = new Vector2(-1, -1);
            selector_pos = new Vector2(1, 1);
        }

        private void DrawReserve() {
            if(cards_reserve.Count > 0) {
                cards_stack.Add(cards_reserve[0]);
                cards_reserve.RemoveAt(0);
            } else {
                cards_reserve.AddRange(cards_stack);
                cards_stack.Clear();
            }
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(selector_pos.Y > 0) selector_pos.Y--; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(selector_pos.Y < 20) selector_pos.Y++; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(selector_pos.X > 0) selector_pos.X--; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(selector_pos.X < 20) selector_pos.X++; }

            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && pressed_event_touch) {
                for(int y = 4; y < 20; y++) {
                    for(int x = 0; x < 20; x++) {
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * x, (int)SK.Position_DisplayEdge().Y + 48 * y, 35, 48))) { TouchField(x, y); }
                    }
                }

                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 1, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { DrawReserve(); }
                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 4, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { TouchStack();  }

                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 *  8, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { TouchFinish(0); }
                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 11, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { TouchFinish(1); }
                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 14, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { TouchFinish(2); }
                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 17, (int)SK.Position_DisplayEdge().Y + 48 * 1, 35 * 2, 48 * 2))) { TouchFinish(3); }
            }

            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) {
                if(selector_pos.Y < 14) {
                    TouchField((int)selector_pos.X, (int)selector_pos.Y);
                }
                //if(selector_pos == new Vector2(7, 12)) if(cards_stack.Count > 0) CompareCards(28);
                if(selector_pos == new Vector2(1, 1)) DrawReserve();
                if(selector_pos == new Vector2(4, 1)) TouchStack();

                if(selector_pos == new Vector2( 8, 1)) TouchFinish(0);
                if(selector_pos == new Vector2(11, 1)) TouchFinish(1);
                if(selector_pos == new Vector2(14, 1)) TouchFinish(2);
                if(selector_pos == new Vector2(17, 1)) TouchFinish(3);
            }
            return "void";
        }

        private void TouchStack() {
            selector_card = new Vector2(-2, -2);
        }

        private void TouchFinish(int slot) {
            if(selector_card != new Vector2(-1, -1)) {
                if(selector_card.Y == -2) { // Cell-to-Finish
                    if(cards_finish[slot].Count == 0) {
                        cards_finish[slot].Add(cards_stack[cards_stack.Count - 1]);
                        cards_stack.RemoveAt(cards_stack.Count - 1);
                        selector_card = new Vector2(-1, -1);
                    } else {
                        if((cards_stack[cards_stack.Count - 1].X - 1 == cards_finish[slot][cards_finish[slot].Count - 1].X || (cards_stack[cards_stack.Count - 1].X == 1 && cards_finish[slot][cards_finish[slot].Count - 1].X == 13)) && cards_finish[slot][cards_finish[slot].Count - 1].Y == cards_stack[cards_stack.Count - 1].Y) {
                            cards_finish[slot].Add(cards_stack[cards_stack.Count - 1]);
                            cards_stack.RemoveAt(cards_stack.Count - 1);
                            selector_card = new Vector2(-1, -1);
                        }
                    }
                } else { // Field-to-Finish
                    if(selector_card.Y == cards_field[(int)selector_card.X].Count - 1) {
                        if(cards_finish[slot].Count == 0) {
                            cards_finish[slot].Add(cards_field[(int)selector_card.X][cards_field[(int)selector_card.X].Count - 1]);
                            cards_field[(int)selector_card.X].RemoveAt(cards_field[(int)selector_card.X].Count - 1);
                            selector_card = new Vector2(-1, -1);
                        } else {
                            if((cards_field[(int)selector_card.X][(int)selector_card.Y].X - 1 == cards_finish[slot][cards_finish[slot].Count - 1].X || (cards_field[(int)selector_card.X][(int)selector_card.Y].X == 1 && cards_finish[slot][cards_finish[slot].Count - 1].X == 13)) && cards_finish[slot][cards_finish[slot].Count - 1].Y == cards_field[(int)selector_card.X][(int)selector_card.Y].Y) {
                                cards_finish[slot].Add(cards_field[(int)selector_card.X][cards_field[(int)selector_card.X].Count - 1]);
                                cards_field[(int)selector_card.X].RemoveAt(cards_field[(int)selector_card.X].Count - 1);
                                selector_card = new Vector2(-1, -1);
                            }
                        }
                    }
                }
            }
        }

        private void TouchField(int x, int y) {
            int x2 = (x - 2) / 2; if(x2 > 6) x2 = 6;
            int y2 = y - 4;
            if(selector_card.Y == -2) {
                if(!MoveStack(x, y)) {
                    selector_card = new Vector2(-1, -1);
                }
            } else
            if(cards_field[x2].Count >= y2 - 1) {
                if(selector_card == new Vector2(-1, -1)) {
                    y2 = cards_field[x2].Count <= y2 ? cards_field[x2].Count - 1 : y - 4;
                    if(cards_field[x2].Count == 0) return;
                    float tempCard = cards_field[x2][y2].X;
                    float tempSuit = cards_field[x2][y2].Y;
                    for(int i = y2; i < cards_field[x2].Count; i++) {
                        if(i != cards_field[x2].Count - 1) {
                            if(((cards_field[x2][i].X - 1 != cards_field[x2][i + 1].X) && !(cards_field[x2][i].X == 1 && cards_field[x2][i + 1].X == 13)) || !DifferentColors(cards_field[x2][i].Y, cards_field[x2][i + 1].Y)) {
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
            int x2 = (x - 2) / 2;
            int y2 = cards_field[x2].Count - 1;
            if(selector_card.Y != -2) { // Field-to-Field
                if(cards_field[x2].Count == 0) {
                    cards_field[x2].AddRange(cards_field[(int)selector_card.X].GetRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y));
                    cards_field[(int)selector_card.X].RemoveRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y);
                    selector_card = new Vector2(-1, -1);
                    return true;
                } else {
                    if((cards_field[(int)selector_card.X][(int)selector_card.Y].X + 1 == cards_field[x2][y2].X || (cards_field[(int)selector_card.X][(int)selector_card.Y].X == 13 && cards_field[x2][y2].X == 1)) && DifferentColors(cards_field[x2][y2].Y, cards_field[(int)selector_card.X][(int)selector_card.Y].Y)) {
                        cards_field[x2].AddRange(cards_field[(int)selector_card.X].GetRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y));
                        cards_field[(int)selector_card.X].RemoveRange((int)selector_card.Y, cards_field[(int)selector_card.X].Count - (int)selector_card.Y);
                        selector_card = new Vector2(-1, -1);
                        return true;
                    }
                }
            } else { // Cell-to-Field
                if(cards_field[x2].Count == 0) {
                    cards_field[x2].Add(cards_stack[cards_stack.Count - 1]);
                    cards_stack.RemoveAt(cards_stack.Count - 1);
                    selector_card = new Vector2(-1, -1);
                    return true;
                } else {
                    if((cards_stack[cards_stack.Count - 1].X + 1 == cards_field[x2][y2].X || (cards_stack[cards_stack.Count - 1].X == 13 && cards_field[x2][y2].X == 1)) && DifferentColors(cards_field[x2][y2].Y, cards_stack[cards_stack.Count - 1].Y)) {
                        cards_field[x2].Add(cards_stack[cards_stack.Count - 1]);
                        cards_stack.RemoveAt(cards_stack.Count - 1);
                        selector_card = new Vector2(-1, -1);
                        return true;
                    }
                }
            }
            return false;
        }

        public override string Update3(GameTime gameTime) {
            if(cards_finish[0].Count == 13 && cards_finish[1].Count == 13 && cards_finish[2].Count == 13 && cards_finish[3].Count == 13 && !active_gameover) {
                score_points = 100;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private bool DifferentColors(float a, float b) {
            if(a == 0 || a == 1) if(b == 2 || b == 3) return true;
            if(a == 2 || a == 3) if(b == 0 || b == 1) return true;
            return false;
        }

        public override void Draw2() {

            for(int x = 0; x < 7; x++) {
                for(int y = 0; y < cards_field[x].Count; y++) {
                    spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 2 + 35 * x * 2, 48 * 4 + 48 * y), new Rectangle(1 + (int)(71 * cards_field[x][y].X), 1 + (int)(96 * cards_field[x][y].Y), 70, 95), selector_card == new Vector2(x, y) ? Color.Gold : Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }
            }

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 1, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 4, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  8, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 11, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 14, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 17, 48 * 1), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.10f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_reserve.Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 1, 48 * 1), new Rectangle(1, 1, 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_stack.Count   > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 4, 48 * 1), new Rectangle(1 + (int)(71 * cards_stack  [cards_stack.Count - 1].X), 1 + (int)(96 * cards_stack  [cards_stack.Count - 1].Y), 70, 95), selector_card == new Vector2(-2, -2) ? Color.Gold : Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_finish[0].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  8, 48 * 1), new Rectangle(1 + (int)(71 * cards_finish[0][cards_finish[0].Count - 1].X), 1 + (int)(96 * cards_finish[0][cards_finish[0].Count - 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_finish[1].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 11, 48 * 1), new Rectangle(1 + (int)(71 * cards_finish[1][cards_finish[1].Count - 1].X), 1 + (int)(96 * cards_finish[1][cards_finish[1].Count - 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_finish[2].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 14, 48 * 1), new Rectangle(1 + (int)(71 * cards_finish[2][cards_finish[2].Count - 1].X), 1 + (int)(96 * cards_finish[2][cards_finish[2].Count - 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_finish[3].Count > 0) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 17, 48 * 1), new Rectangle(1 + (int)(71 * cards_finish[3][cards_finish[3].Count - 1].X), 1 + (int)(96 * cards_finish[3][cards_finish[3].Count - 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * selector_pos.X, 48 * selector_pos.Y), new Rectangle(1, 1, 70, 95), Color.Blue * 0.25f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
        }

    }
}
