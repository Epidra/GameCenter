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
    class RPSLS : Ghost {

        int selector_player;
        int selector_comp;
        int selector_anim;

        int transition_player1;
        int transition_player2;
        int transition_player3;
        int transition_player4;
        int transition_player5;
        int transition_comp;

        int lastPlayer;
        int lastComp;
        float alpha;

        bool transition_up;

        Vector2 position_grid = new Vector2(0,0);

        int result;

        bool won;

        public RPSLS(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            selector_player = 2;
            selector_anim = random.Next(5) + 1;
            selector_comp = random.Next(5) + 1;
            transition_player1 = 0;
            transition_player2 = 0;
            transition_player3 = 0;
            transition_player4 = 0;
            transition_player5 = 0;
            transition_comp = 0;
            transition_up = true;
            lastPlayer = 0;
            lastComp = 0;
            alpha = 0.00f;
            result = 0;
            won = false;
        }

        public void Restart() {
            selector_player = 2;
            selector_anim = random.Next(5) + 1;
            selector_comp = random.Next(5) + 1;
            transition_player1 = 0;
            transition_player2 = 0;
            transition_player3 = 0;
            transition_player4 = 0;
            transition_player5 = 0;
            transition_comp = 0;
            transition_up = true;
            if(result != 1) {
                score_points = 0;
            }
            result = 0;
            active_highscore = false;
            active_gameover = false;
            active_pause = false;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { Compare(); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(selector_player > 1) selector_player--; pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(selector_player < 5) selector_player++; pressed_response = true; }
            if(pressed_event_touch) {
                pressed_response = true;
                if(SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y + position_grid.Y - 172 < control_cursor.Y && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y + position_grid.Y) {
                    for(int i = 0; i < 5; i++) {
                        if(SK.Position_DisplayEdge().X + position_grid.X + 172 * i < control_cursor.X && control_cursor.X < SK.Position_DisplayEdge().X + position_grid.X + 172 * i + 172) {
                            if(i + 1 != selector_player) {
                                selector_player = i + 1;
                            } else {
                                Compare();
                            }
                        }
                    }
                }
            }
            if(control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                if(SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y + position_grid.Y - 172 < control_cursor.Y && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y + position_grid.Y) {
                    for(int i = 0; i < 5; i++) {
                        if(SK.Position_DisplayEdge().X + position_grid.X + 172 * i < control_cursor.X && control_cursor.X < SK.Position_DisplayEdge().X + position_grid.X + 172 * i + 172) {
                            selector_player = i + 1;
                        }
                    }
                }
            }

            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(transition_player1 < 60 && selector_player == 1) transition_player1++;
            if(transition_player1 > 0 && selector_player != 1) transition_player1--;
            if(transition_player2 < 60 && selector_player == 2) transition_player2++;
            if(transition_player2 > 0 && selector_player != 2) transition_player2--;
            if(transition_player3 < 60 && selector_player == 3) transition_player3++;
            if(transition_player3 > 0 && selector_player != 3) transition_player3--;
            if(transition_player4 < 60 && selector_player == 4) transition_player4++;
            if(transition_player4 > 0 && selector_player != 4) transition_player4--;
            if(transition_player5 < 60 && selector_player == 5) transition_player5++;
            if(transition_player5 > 0 && selector_player != 5) transition_player5--;
            if(transition_up) {
                if(transition_comp < 30)
                    transition_comp++;
                if(transition_comp == 30) {
                    transition_up = false;
                }
            } else {
                if(transition_comp > 0)
                    transition_comp--;
                if(transition_comp == 0) {
                    transition_up = true;
                    selector_anim = random.Next(5) + 1;
                }
            }
            if(won && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            if(alpha > 0)
                alpha -= 0.01f;
            return "void";
        }

        private void Compare() {
            result = 3;
            if(selector_player == 1 && (selector_comp == 4 || selector_comp == 3)) result = 1;
            if(selector_player == 2 && (selector_comp == 1 || selector_comp == 5)) result = 1;
            if(selector_player == 3 && (selector_comp == 2 || selector_comp == 4)) result = 1;
            if(selector_player == 4 && (selector_comp == 5 || selector_comp == 2)) result = 1;
            if(selector_player == 5 && (selector_comp == 3 || selector_comp == 1)) result = 1;
            if(selector_comp == 1 && (selector_player == 4 || selector_player == 3)) result = 2;
            if(selector_comp == 2 && (selector_player == 1 || selector_player == 5)) result = 2;
            if(selector_comp == 3 && (selector_player == 2 || selector_player == 4)) result = 2;
            if(selector_comp == 4 && (selector_player == 5 || selector_player == 2)) result = 2;
            if(selector_comp == 5 && (selector_player == 3 || selector_player == 1)) result = 2;
            lastPlayer = selector_player;
            lastComp = selector_comp;
            alpha = 1.00f;
            if(result == 1) {
                score_points++;
                JK.Noise("Cleared");
                Restart();
            } else {
                won = true;
            }
        }

        public override void Draw2() {
            if(result == 0) {
                spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 0, SK.Get_GridSize().Y - 150 + position_grid.Y - transition_player1), Color.White);
                spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 1, SK.Get_GridSize().Y - 150 + position_grid.Y - transition_player2), Color.White);
                spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 2, SK.Get_GridSize().Y - 150 + position_grid.Y - transition_player3), Color.White);
                spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 3, SK.Get_GridSize().Y - 150 + position_grid.Y - transition_player4), Color.White);
                spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 4, SK.Get_GridSize().Y - 150 + position_grid.Y - transition_player5), Color.White);

                int temp = 0;
                if(selector_anim == 1) { temp = transition_comp; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 0, 0 + temp), Color.White);
                if(selector_anim == 2) { temp = transition_comp; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 1, 0 + temp), Color.White);
                if(selector_anim == 3) { temp = transition_comp; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 2, 0 + temp), Color.White);
                if(selector_anim == 4) { temp = transition_comp; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 3, 0 + temp), Color.White);
                if(selector_anim == 5) { temp = transition_comp; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 4, 0 + temp), Color.White);
            } else {
                int temp = 0;
                if(selector_player == 1) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 0, SK.Get_GridSize().Y - 150 + position_grid.Y - temp), Color.White);
                if(selector_player == 2) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 1, SK.Get_GridSize().Y - 150 + position_grid.Y - temp), Color.White);
                if(selector_player == 3) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 2, SK.Get_GridSize().Y - 150 + position_grid.Y - temp), Color.White);
                if(selector_player == 4) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 3, SK.Get_GridSize().Y - 150 + position_grid.Y - temp), Color.White);
                if(selector_player == 5) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 4, SK.Get_GridSize().Y - 150 + position_grid.Y - temp), Color.White);

                if(selector_comp == 1) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 0, 0 + temp), Color.White);
                if(selector_comp == 2) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 1, 0 + temp), Color.White);
                if(selector_comp == 3) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 2, 0 + temp), Color.White);
                if(selector_comp == 4) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 3, 0 + temp), Color.White);
                if(selector_comp == 5) { temp = 60; } else { temp = 0; }
                spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(position_grid.X + 172 * 4, 0 + temp), Color.White);
            }

            if(alpha > 0) {
                if(lastPlayer == 1) spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 - 150), Color.White * alpha);
                if(lastPlayer == 2) spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 - 150), Color.White * alpha);
                if(lastPlayer == 3) spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 - 150), Color.White * alpha);
                if(lastPlayer == 4) spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 - 150), Color.White * alpha);
                if(lastPlayer == 5) spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 - 150), Color.White * alpha);
                if(lastComp == 1) spriteBatch.Draw(SK.texture_static_rpsls_rock, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 + 50), Color.White * alpha);
                if(lastComp == 2) spriteBatch.Draw(SK.texture_static_rpsls_paper, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 + 50), Color.White * alpha);
                if(lastComp == 3) spriteBatch.Draw(SK.texture_static_rpsls_scissor, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 + 50), Color.White * alpha);
                if(lastComp == 4) spriteBatch.Draw(SK.texture_static_rpsls_lizard, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 + 50), Color.White * alpha);
                if(lastComp == 5) spriteBatch.Draw(SK.texture_static_rpsls_spock, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - 172 / 2, SK.Get_GridSize().Y / 2 + 50), Color.White * alpha);
            }

            if(active_gameover) {
                if(result == 1) {
                    spriteBatch.DrawString(SK.font_score, "You Won!", SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("You Won!").X, 250), Color.Black);
                } else if(result == 2) {
                    spriteBatch.DrawString(SK.font_score, "You Lost!", SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("You Lost!").X, 250), Color.Black);
                } else {
                    spriteBatch.DrawString(SK.font_score, "Draw", SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("Draw").X, 250), Color.Black);
                }
            }
        }

    }
}
