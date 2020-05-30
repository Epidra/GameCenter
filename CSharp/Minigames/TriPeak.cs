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
    class TriPeak : Ghost {

        Vector2 selector_pos;

        int combo;

        Vector2[] cards_field = new Vector2[28];
        List<Vector2> cards_stack = new List<Vector2>();
        List<Vector2> cards_reserve = new List<Vector2>();

        public TriPeak(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            score_lives = 2;

            combo = 0;

            List<Vector2> deck = ShuffleDeck();

            for(int i = 0; i < 28; i++) {
                cards_field[i] = deck[i];
            }
            selector_pos = new Vector2(9, 12);
            deck.RemoveRange(0, 28);
            cards_reserve.AddRange(deck);
            cards_stack.Clear();
            DrawReserve();
        }

        public void Restart() {

            List<Vector2> deck = ShuffleDeck();

            combo = 0;

            for(int i = 0; i < 28; i++) {
                cards_field[i] = deck[i];
            }
            deck.RemoveRange(0, 28);
            cards_reserve.AddRange(deck);
            cards_stack.Clear();
            DrawReserve();
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

                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + 35 * 13, (int)SK.Position_DisplayEdge().Y + 48 * 11, 35 * 2, 48 * 2))) { DrawReserve(); ; }
            }

            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) {
                if(selector_pos.Y < 14) {
                    TouchField((int)selector_pos.X, (int)selector_pos.Y);
                }
                //if(selector_pos == new Vector2(7, 12)) if(cards_stack.Count > 0) CompareCards(28);
                if(selector_pos == new Vector2(13, 11)) DrawReserve();
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(score_lives <= -1 && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        private void DrawReserve() {
            combo = 0;
            if(cards_reserve.Count > 0) {
                cards_stack.Add(cards_reserve[0]);
                cards_reserve.RemoveAt(0);
            } else {
                score_lives--;
                Restart();
            }
        }

        private void TouchField(int x, int y) {
            if(x ==  3 && y == 1) if(IsCardSelectable( 0)) { CompareCards( 0); }
            if(x ==  4 && y == 1) if(IsCardSelectable( 0)) { CompareCards( 0); }
            if(x ==  9 && y == 1) if(IsCardSelectable( 1)) { CompareCards( 1); }
            if(x == 10 && y == 1) if(IsCardSelectable( 1)) { CompareCards( 1); }
            if(x == 15 && y == 1) if(IsCardSelectable( 2)) { CompareCards( 2); }
            if(x == 16 && y == 1) if(IsCardSelectable( 2)) { CompareCards( 2); }

            if(x ==  2 && y == 2) if(IsCardSelectable( 3)) { CompareCards( 3); }
            if(x ==  3 && y == 2) if(IsCardSelectable( 3)) { CompareCards( 3); } else if(IsCardSelectable( 0)) { CompareCards( 0); }
            if(x ==  4 && y == 2) if(IsCardSelectable( 4)) { CompareCards( 4); } else if(IsCardSelectable( 0)) { CompareCards( 0); }
            if(x ==  5 && y == 2) if(IsCardSelectable( 4)) { CompareCards( 4); }
            if(x ==  8 && y == 2) if(IsCardSelectable( 5)) { CompareCards( 5); }
            if(x ==  9 && y == 2) if(IsCardSelectable( 5)) { CompareCards( 5); } else if(IsCardSelectable( 1)) { CompareCards( 1); }
            if(x == 10 && y == 2) if(IsCardSelectable( 6)) { CompareCards( 6); } else if(IsCardSelectable( 1)) { CompareCards( 1); }
            if(x == 11 && y == 2) if(IsCardSelectable( 6)) { CompareCards( 6); }
            if(x == 14 && y == 2) if(IsCardSelectable( 7)) { CompareCards( 7); }
            if(x == 15 && y == 2) if(IsCardSelectable( 7)) { CompareCards( 7); } else if(IsCardSelectable( 2)) { CompareCards( 2); }
            if(x == 16 && y == 2) if(IsCardSelectable( 8)) { CompareCards( 8); } else if(IsCardSelectable( 2)) { CompareCards( 2); }
            if(x == 17 && y == 2) if(IsCardSelectable( 8)) { CompareCards( 8); }

            if(x ==  1 && y == 3) if(IsCardSelectable( 9)) { CompareCards( 9); }
            if(x ==  2 && y == 3) if(IsCardSelectable( 9)) { CompareCards( 9); } else if(IsCardSelectable( 3)) { CompareCards( 3); }
            if(x ==  3 && y == 3) if(IsCardSelectable(10)) { CompareCards(10); } else if(IsCardSelectable( 3)) { CompareCards( 3); }
            if(x ==  4 && y == 3) if(IsCardSelectable(10)) { CompareCards(10); } else if(IsCardSelectable( 4)) { CompareCards( 4); }
            if(x ==  5 && y == 3) if(IsCardSelectable(11)) { CompareCards(11); } else if(IsCardSelectable( 4)) { CompareCards( 4); }
            if(x ==  6 && y == 3) if(IsCardSelectable(11)) { CompareCards(11); }
            if(x ==  7 && y == 3) if(IsCardSelectable(12)) { CompareCards(12); }
            if(x ==  8 && y == 3) if(IsCardSelectable(12)) { CompareCards(12); } else if(IsCardSelectable( 5)) { CompareCards( 5); }
            if(x ==  9 && y == 3) if(IsCardSelectable(13)) { CompareCards(13); } else if(IsCardSelectable( 5)) { CompareCards( 5); }
            if(x == 10 && y == 3) if(IsCardSelectable(13)) { CompareCards(13); } else if(IsCardSelectable( 6)) { CompareCards( 6); }
            if(x == 11 && y == 3) if(IsCardSelectable(14)) { CompareCards(14); } else if(IsCardSelectable( 6)) { CompareCards( 6); }
            if(x == 12 && y == 3) if(IsCardSelectable(14)) { CompareCards(14); }
            if(x == 13 && y == 3) if(IsCardSelectable(15)) { CompareCards(15); }
            if(x == 14 && y == 3) if(IsCardSelectable(15)) { CompareCards(15); } else if(IsCardSelectable( 7)) { CompareCards( 7); }
            if(x == 15 && y == 3) if(IsCardSelectable(16)) { CompareCards(16); } else if(IsCardSelectable( 7)) { CompareCards( 7); }
            if(x == 16 && y == 3) if(IsCardSelectable(16)) { CompareCards(16); } else if(IsCardSelectable( 8)) { CompareCards( 8); }
            if(x == 17 && y == 3) if(IsCardSelectable(17)) { CompareCards(17); } else if(IsCardSelectable( 8)) { CompareCards( 8); }
            if(x == 18 && y == 3) if(IsCardSelectable(17)) { CompareCards(17); }

            if(x ==  0 && y == 4) if(IsCardSelectable(18)) { CompareCards(18); }
            if(x ==  1 && y == 4) if(IsCardSelectable(18)) { CompareCards(18); } else if(IsCardSelectable( 9)) { CompareCards( 9); }
            if(x ==  2 && y == 4) if(IsCardSelectable(19)) { CompareCards(19); } else if(IsCardSelectable( 9)) { CompareCards( 9); }
            if(x ==  3 && y == 4) if(IsCardSelectable(19)) { CompareCards(19); } else if(IsCardSelectable(10)) { CompareCards(10); }
            if(x ==  4 && y == 4) if(IsCardSelectable(20)) { CompareCards(20); } else if(IsCardSelectable(10)) { CompareCards(10); }
            if(x ==  5 && y == 4) if(IsCardSelectable(20)) { CompareCards(20); } else if(IsCardSelectable(11)) { CompareCards(11); }
            if(x ==  6 && y == 4) if(IsCardSelectable(21)) { CompareCards(21); } else if(IsCardSelectable(11)) { CompareCards(11); }
            if(x ==  7 && y == 4) if(IsCardSelectable(21)) { CompareCards(21); } else if(IsCardSelectable(12)) { CompareCards(12); }
            if(x ==  8 && y == 4) if(IsCardSelectable(22)) { CompareCards(22); } else if(IsCardSelectable(12)) { CompareCards(12); }
            if(x ==  9 && y == 4) if(IsCardSelectable(22)) { CompareCards(22); } else if(IsCardSelectable(13)) { CompareCards(13); }
            if(x == 10 && y == 4) if(IsCardSelectable(23)) { CompareCards(23); } else if(IsCardSelectable(13)) { CompareCards(13); }
            if(x == 11 && y == 4) if(IsCardSelectable(23)) { CompareCards(23); } else if(IsCardSelectable(14)) { CompareCards(14); }
            if(x == 12 && y == 4) if(IsCardSelectable(24)) { CompareCards(24); } else if(IsCardSelectable(14)) { CompareCards(14); }
            if(x == 13 && y == 4) if(IsCardSelectable(24)) { CompareCards(24); } else if(IsCardSelectable(15)) { CompareCards(15); }
            if(x == 14 && y == 4) if(IsCardSelectable(25)) { CompareCards(25); } else if(IsCardSelectable(15)) { CompareCards(15); }
            if(x == 15 && y == 4) if(IsCardSelectable(25)) { CompareCards(25); } else if(IsCardSelectable(16)) { CompareCards(16); }
            if(x == 16 && y == 4) if(IsCardSelectable(26)) { CompareCards(26); } else if(IsCardSelectable(16)) { CompareCards(16); }
            if(x == 17 && y == 4) if(IsCardSelectable(26)) { CompareCards(26); } else if(IsCardSelectable(17)) { CompareCards(17); }
            if(x == 18 && y == 4) if(IsCardSelectable(27)) { CompareCards(27); } else if(IsCardSelectable(17)) { CompareCards(17); }
            if(x == 19 && y == 4) if(IsCardSelectable(27)) { CompareCards(27); }

            if(x ==  0 && y == 5) if(IsCardSelectable(18)) { CompareCards(18); }
            if(x ==  1 && y == 5) if(IsCardSelectable(18)) { CompareCards(18); }
            if(x ==  2 && y == 5) if(IsCardSelectable(19)) { CompareCards(19); }
            if(x ==  3 && y == 5) if(IsCardSelectable(19)) { CompareCards(19); }
            if(x ==  4 && y == 5) if(IsCardSelectable(20)) { CompareCards(20); }
            if(x ==  5 && y == 5) if(IsCardSelectable(20)) { CompareCards(20); }
            if(x ==  6 && y == 5) if(IsCardSelectable(21)) { CompareCards(21); }
            if(x ==  7 && y == 5) if(IsCardSelectable(21)) { CompareCards(21); }
            if(x ==  8 && y == 5) if(IsCardSelectable(22)) { CompareCards(22); }
            if(x ==  9 && y == 5) if(IsCardSelectable(22)) { CompareCards(22); }
            if(x == 10 && y == 5) if(IsCardSelectable(23)) { CompareCards(23); }
            if(x == 11 && y == 5) if(IsCardSelectable(23)) { CompareCards(23); }
            if(x == 12 && y == 5) if(IsCardSelectable(24)) { CompareCards(24); }
            if(x == 13 && y == 5) if(IsCardSelectable(24)) { CompareCards(24); }
            if(x == 14 && y == 5) if(IsCardSelectable(25)) { CompareCards(25); }
            if(x == 15 && y == 5) if(IsCardSelectable(25)) { CompareCards(25); }
            if(x == 16 && y == 5) if(IsCardSelectable(26)) { CompareCards(26); }
            if(x == 17 && y == 5) if(IsCardSelectable(26)) { CompareCards(26); }
            if(x == 18 && y == 5) if(IsCardSelectable(27)) { CompareCards(27); }
            if(x == 19 && y == 5) if(IsCardSelectable(27)) { CompareCards(27); }
        }

        private bool IsCardSelectable(int id) {
            switch(id) {
                case  0: if(cards_field[ 0] != new Vector2(-1, -1) && cards_field[ 3] == new Vector2(-1, -1) && cards_field[ 4] == new Vector2(-1, -1)) return true; break;
                case  1: if(cards_field[ 1] != new Vector2(-1, -1) && cards_field[ 5] == new Vector2(-1, -1) && cards_field[ 6] == new Vector2(-1, -1)) return true; break;
                case  2: if(cards_field[ 2] != new Vector2(-1, -1) && cards_field[ 7] == new Vector2(-1, -1) && cards_field[ 8] == new Vector2(-1, -1)) return true; break;

                case  3: if(cards_field[ 3] != new Vector2(-1, -1) && cards_field[ 9] == new Vector2(-1, -1) && cards_field[10] == new Vector2(-1, -1)) return true; break;
                case  4: if(cards_field[ 4] != new Vector2(-1, -1) && cards_field[10] == new Vector2(-1, -1) && cards_field[11] == new Vector2(-1, -1)) return true; break;
                case  5: if(cards_field[ 5] != new Vector2(-1, -1) && cards_field[12] == new Vector2(-1, -1) && cards_field[13] == new Vector2(-1, -1)) return true; break;
                case  6: if(cards_field[ 6] != new Vector2(-1, -1) && cards_field[13] == new Vector2(-1, -1) && cards_field[14] == new Vector2(-1, -1)) return true; break;
                case  7: if(cards_field[ 7] != new Vector2(-1, -1) && cards_field[15] == new Vector2(-1, -1) && cards_field[16] == new Vector2(-1, -1)) return true; break;
                case  8: if(cards_field[ 8] != new Vector2(-1, -1) && cards_field[16] == new Vector2(-1, -1) && cards_field[17] == new Vector2(-1, -1)) return true; break;

                case  9: if(cards_field[ 9] != new Vector2(-1, -1) && cards_field[18] == new Vector2(-1, -1) && cards_field[19] == new Vector2(-1, -1)) return true; break;
                case 10: if(cards_field[10] != new Vector2(-1, -1) && cards_field[19] == new Vector2(-1, -1) && cards_field[20] == new Vector2(-1, -1)) return true; break;
                case 11: if(cards_field[11] != new Vector2(-1, -1) && cards_field[20] == new Vector2(-1, -1) && cards_field[21] == new Vector2(-1, -1)) return true; break;
                case 12: if(cards_field[12] != new Vector2(-1, -1) && cards_field[21] == new Vector2(-1, -1) && cards_field[22] == new Vector2(-1, -1)) return true; break;
                case 13: if(cards_field[13] != new Vector2(-1, -1) && cards_field[22] == new Vector2(-1, -1) && cards_field[23] == new Vector2(-1, -1)) return true; break;
                case 14: if(cards_field[14] != new Vector2(-1, -1) && cards_field[23] == new Vector2(-1, -1) && cards_field[24] == new Vector2(-1, -1)) return true; break;
                case 15: if(cards_field[15] != new Vector2(-1, -1) && cards_field[24] == new Vector2(-1, -1) && cards_field[25] == new Vector2(-1, -1)) return true; break;
                case 16: if(cards_field[16] != new Vector2(-1, -1) && cards_field[25] == new Vector2(-1, -1) && cards_field[26] == new Vector2(-1, -1)) return true; break;
                case 17: if(cards_field[17] != new Vector2(-1, -1) && cards_field[26] == new Vector2(-1, -1) && cards_field[27] == new Vector2(-1, -1)) return true; break;

                case 18: if(cards_field[18] != new Vector2(-1, -1)) return true; break;
                case 19: if(cards_field[19] != new Vector2(-1, -1)) return true; break;
                case 20: if(cards_field[20] != new Vector2(-1, -1)) return true; break;
                case 21: if(cards_field[21] != new Vector2(-1, -1)) return true; break;
                case 22: if(cards_field[22] != new Vector2(-1, -1)) return true; break;
                case 23: if(cards_field[23] != new Vector2(-1, -1)) return true; break;
                case 24: if(cards_field[24] != new Vector2(-1, -1)) return true; break;
                case 25: if(cards_field[25] != new Vector2(-1, -1)) return true; break;
                case 26: if(cards_field[26] != new Vector2(-1, -1)) return true; break;
                case 27: if(cards_field[27] != new Vector2(-1, -1)) return true; break;
            }
            return false;
        }

        private void CompareCards(int id) {

            if(cards_field[id].X + 1 == cards_stack[cards_stack.Count - 1].X || cards_field[id].X - 1 == cards_stack[cards_stack.Count - 1].X || (cards_field[id].X == 1 && cards_stack[cards_stack.Count - 1].X == 13) || (cards_field[id].X == 13 && cards_stack[cards_stack.Count - 1].X == 1)) {
                combo++;
                score_points += 50 * combo;
                cards_stack.Add(cards_field[id]);
                cards_field[id] = new Vector2(-1, -1);
                if(cards_field[0].Y == -1 && cards_field[1].Y == -1 && cards_field[2].Y == -1) Restart();
            }
        }

        public override void Draw2() {
            //spriteBatch.Draw(SK.texture_background_2048, SK.Position_DisplayEdge() + SK.Position_2048(), Color.DarkGray);

            if(cards_field[ 0] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  3, 48 * 1), new Rectangle(1 + (int)(71 * cards_field[ 0].X), 1 + (int)(96 * cards_field[ 0].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 1] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  9, 48 * 1), new Rectangle(1 + (int)(71 * cards_field[ 1].X), 1 + (int)(96 * cards_field[ 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 2] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 15, 48 * 1), new Rectangle(1 + (int)(71 * cards_field[ 2].X), 1 + (int)(96 * cards_field[ 2].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_field[ 3] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  2, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 3].X), 1 + (int)(96 * cards_field[ 3].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 4] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  4, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 4].X), 1 + (int)(96 * cards_field[ 4].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 5] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  8, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 5].X), 1 + (int)(96 * cards_field[ 5].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 6] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 10, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 6].X), 1 + (int)(96 * cards_field[ 6].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 7] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 14, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 7].X), 1 + (int)(96 * cards_field[ 7].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[ 8] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 16, 48 * 2), new Rectangle(1 + (int)(71 * cards_field[ 8].X), 1 + (int)(96 * cards_field[ 8].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_field[ 9] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  1, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[ 9].X), 1 + (int)(96 * cards_field[ 9].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[10] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  3, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[10].X), 1 + (int)(96 * cards_field[10].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[11] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  5, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[11].X), 1 + (int)(96 * cards_field[11].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[12] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  7, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[12].X), 1 + (int)(96 * cards_field[12].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[13] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  9, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[13].X), 1 + (int)(96 * cards_field[13].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[14] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 11, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[14].X), 1 + (int)(96 * cards_field[14].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[15] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 13, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[15].X), 1 + (int)(96 * cards_field[15].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[16] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 15, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[16].X), 1 + (int)(96 * cards_field[16].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[17] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 17, 48 * 3), new Rectangle(1 + (int)(71 * cards_field[17].X), 1 + (int)(96 * cards_field[17].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_field[18] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  0, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[18].X), 1 + (int)(96 * cards_field[18].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[19] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  2, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[19].X), 1 + (int)(96 * cards_field[19].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[20] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  4, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[20].X), 1 + (int)(96 * cards_field[20].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[21] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  6, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[21].X), 1 + (int)(96 * cards_field[21].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[22] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 *  8, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[22].X), 1 + (int)(96 * cards_field[22].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[23] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 10, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[23].X), 1 + (int)(96 * cards_field[23].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[24] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 12, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[24].X), 1 + (int)(96 * cards_field[24].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[25] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 14, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[25].X), 1 + (int)(96 * cards_field[25].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[26] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 16, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[26].X), 1 + (int)(96 * cards_field[26].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            if(cards_field[27] != new Vector2(-1, -1)) spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 18, 48 * 4), new Rectangle(1 + (int)(71 * cards_field[27].X), 1 + (int)(96 * cards_field[27].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            if(cards_stack.Count > 0) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 7, 48 * 11), new Rectangle(1 + (int)(71 * cards_stack[cards_stack.Count - 1].X), 1 + (int)(96 * cards_stack[cards_stack.Count - 1].Y), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            } else {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 7, 48 * 11), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.25f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            }

            if(cards_reserve.Count > 0) {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 13, 48 * 11), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            } else {
                spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * 13, 48 * 11), new Rectangle(1 + (int)(71 * 0), 1 + (int)(96 * 0), 70, 95), Color.White * 0.25f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            }

            spriteBatch.Draw(SK.texture_spritesheet_cards, SK.Position_DisplayEdge() + new Vector2(35 * selector_pos.X, 48 * selector_pos.Y), new Rectangle(1, 1, 70, 95), Color.Blue * 0.25f, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

        }

    }
}
