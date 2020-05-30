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
    class TicTacToe : Ghost {

        Vector2 selector;

        int winline;

        int result;

        string[,] grid_main  = new string[3, 3];

        bool active_player;

        int marks_placed;

        float alpha;

        public TicTacToe(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            active_player = true;
            grid_main[0, 0] = "empty"; grid_main[1, 0] = "empty"; grid_main[2, 0] = "empty";
            grid_main[0, 1] = "empty"; grid_main[1, 1] = "empty"; grid_main[2, 1] = "empty";
            grid_main[0, 2] = "empty"; grid_main[1, 2] = "empty"; grid_main[2, 2] = "empty";
            marks_placed = 0;
            winline = 0;
            selector = new Vector2(1, 1);
            result = 0;
            alpha = 0.01f;
        }

        public void Restart() {
            active_player = true;
            grid_main[0, 0] = "empty"; grid_main[1, 0] = "empty"; grid_main[2, 0] = "empty";
            grid_main[0, 1] = "empty"; grid_main[1, 1] = "empty"; grid_main[2, 1] = "empty";
            grid_main[0, 2] = "empty"; grid_main[1, 2] = "empty"; grid_main[2, 2] = "empty";
            marks_placed = 0;
            winline = 0;
            selector = new Vector2(1, 1);
            result = 0;
        }

        public override string Update2() {
            if(alpha == 1) {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                    pressed_response = true;
                    if(grid_main[(int)selector.X, (int)selector.Y] == "empty") {
                        grid_main[(int)selector.X, (int)selector.Y] = "Cross";
                        active_player = false;
                        marks_placed++;
                        Check();
                    }
                }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(selector.Y > 0) selector.Y--; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(selector.Y < 2) selector.Y++; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(selector.X > 0) selector.X--; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(selector.X < 2) selector.X++; pressed_response = true; }
                if(pressed_event_touch) {
                    pressed_response = true;
                    Command_Turn_Player();
                }

                for(int x = 0; x < 3; x++) {
                    for(int y = 0; y < 3; y++) {
                        if(Collision_Button(true, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_TicTacToe().X + SK.texture_static_tictactoe_cross.Width * x), (int)(SK.Position_DisplayEdge().Y + SK.Position_TicTacToe().Y + SK.texture_static_tictactoe_cross.Width * y), SK.texture_static_tictactoe_cross.Width, SK.texture_static_tictactoe_cross.Width))) {
                            if(active_player) selector = new Vector2(x, y);
                        }
                    }
                }
            } else {
                alpha -= 0.02f;
                if(alpha <= 0) {
                    alpha = 1.00f;
                    Restart();
                }
            }
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(marks_placed == 9) {
                winline = 0;
                result = 2;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            if(result == 1 && alpha == 1) {
                score_points++;
                JK.Noise("Cleared");
                alpha -= 0.02f;
            }

            if(result == 2 && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            if(!active_player && !active_pause && result == 0) {
                Command_Turn_Computer();
            }
            return "void";
        }

        public override void Draw2() {
            if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.MediumSeaGreen); } else { spriteBatch.Draw(SK.texture_background_canvas, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.MediumSeaGreen); }

            spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(10, 230), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(10, 230 * 2), null, Color.White, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);

            spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(230, 10), null, Color.White, -1.5708f, new Vector2(700, 0), 1, SpriteEffects.None, 0);
            spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(230 * 2, 10), null, Color.White, -1.5708f, new Vector2(700, 0), 1, SpriteEffects.None, 0);

            for(int x = 0; x < 3; x++) {
                for(int y = 0; y < 3; y++) {
                    if(grid_main[x, y] == "Cross") spriteBatch.Draw(SK.texture_static_tictactoe_cross, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_TicTacToe().X + SK.texture_static_tictactoe_cross.Width * x), (int)(SK.Position_DisplayEdge().Y + SK.Position_TicTacToe().Y + SK.texture_static_tictactoe_cross.Height * y), SK.texture_static_tictactoe_cross.Width, SK.texture_static_tictactoe_cross.Height), Color.White * alpha);
                    if(grid_main[x, y] == "Circle") spriteBatch.Draw(SK.texture_static_tictactoe_circle, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_TicTacToe().X + SK.texture_static_tictactoe_circle.Width * x), (int)(SK.Position_DisplayEdge().Y + SK.Position_TicTacToe().Y + SK.texture_static_tictactoe_circle.Height * y), SK.texture_static_tictactoe_circle.Width, SK.texture_static_tictactoe_circle.Height), Color.White * alpha);
                }
            }

            if(!active_gameover) {
                spriteBatch.Draw(SK.texture_static_tictactoe_cross, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_TicTacToe().X + SK.texture_static_tictactoe_cross.Width * selector.X), (int)(SK.Position_DisplayEdge().Y + SK.Position_TicTacToe().Y + SK.texture_static_tictactoe_cross.Height * selector.Y), SK.texture_static_tictactoe_cross.Width, SK.texture_static_tictactoe_cross.Height), Color.Yellow * 0.50f);
            }

            if(winline == 1) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(100, 100), null, Color.Gray * alpha, 1.5708f / 2, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if(winline == 2) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(100, 240 * 3 - 100), null, Color.Gray * alpha, -1.5708f / 2, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if(winline == 3) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(10, 110), null, Color.Gray * alpha, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if(winline == 4) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(10, 110 + 240), null, Color.Gray * alpha, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if(winline == 5) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(10, 110 + 240 * 2), null, Color.Gray * alpha, 0, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            if(winline == 6) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(110, 10), null, Color.Gray * alpha, -1.5708f, new Vector2(700, 0), 1, SpriteEffects.None, 0);
            if(winline == 7) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(110 + 240, 10), null, Color.Gray * alpha, -1.5708f, new Vector2(700, 0), 1, SpriteEffects.None, 0);
            if(winline == 8) spriteBatch.Draw(SK.texture_static_tictactoe_line, SK.Position_DisplayEdge() + SK.Position_TicTacToe() + new Vector2(110 + 240 * 2, 10), null, Color.Gray * alpha, -1.5708f, new Vector2(700, 0), 1, SpriteEffects.None, 0);
        }

        private void Check() {
            if(grid_main[0, 0] == "Cross" && grid_main[0, 1] == "Cross" && grid_main[0, 2] == "Cross") { winline = 6; result = 1; }
            if(grid_main[1, 0] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[1, 2] == "Cross") { winline = 7; result = 1; }
            if(grid_main[2, 0] == "Cross" && grid_main[2, 1] == "Cross" && grid_main[2, 2] == "Cross") { winline = 8; result = 1; }
            if(grid_main[0, 0] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[2, 2] == "Cross") { winline = 1; result = 1; }
            if(grid_main[2, 0] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[0, 2] == "Cross") { winline = 2; result = 1; }
            if(grid_main[0, 0] == "Cross" && grid_main[1, 0] == "Cross" && grid_main[2, 0] == "Cross") { winline = 3; result = 1; }
            if(grid_main[0, 1] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[2, 1] == "Cross") { winline = 4; result = 1; }
            if(grid_main[0, 2] == "Cross" && grid_main[1, 2] == "Cross" && grid_main[2, 2] == "Cross") { winline = 5; result = 1; }

            if(grid_main[0, 0] == "Circle" && grid_main[0, 1] == "Circle" && grid_main[0, 2] == "Circle") { winline = 6; if(!active_gameover) result = 2; }
            if(grid_main[1, 0] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[1, 2] == "Circle") { winline = 7; if(!active_gameover) result = 2; }
            if(grid_main[2, 0] == "Circle" && grid_main[2, 1] == "Circle" && grid_main[2, 2] == "Circle") { winline = 8; if(!active_gameover) result = 2; }
            if(grid_main[0, 0] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[2, 2] == "Circle") { winline = 1; if(!active_gameover) result = 2; }
            if(grid_main[2, 0] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[0, 2] == "Circle") { winline = 2; if(!active_gameover) result = 2; }
            if(grid_main[0, 0] == "Circle" && grid_main[1, 0] == "Circle" && grid_main[2, 0] == "Circle") { winline = 3; if(!active_gameover) result = 2; }
            if(grid_main[0, 1] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[2, 1] == "Circle") { winline = 4; if(!active_gameover) result = 2; }
            if(grid_main[0, 2] == "Circle" && grid_main[1, 2] == "Circle" && grid_main[2, 2] == "Circle") { winline = 5; if(!active_gameover) result = 2; }
        }

        private void Command_Turn_Player() {
            if(active_player) {
                for(int x = 0; x < 3; x++) {
                    for(int y = 0; y < 3; y++) {
                        if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_TicTacToe().X + SK.texture_static_tictactoe_cross.Width * x), (int)(SK.Position_DisplayEdge().Y + SK.Position_TicTacToe().Y + SK.texture_static_tictactoe_cross.Width * y), SK.texture_static_tictactoe_cross.Width, SK.texture_static_tictactoe_cross.Width))) {
                            if(grid_main[x, y] == "empty") {
                                grid_main[x, y] = "Cross";
                                selector = new Vector2(x, y);
                                active_player = false;
                                marks_placed++;
                                Check();
                            }
                        }
                    }
                }
            }
        }

        private void Command_Turn_Computer() {
            if(!active_player) {
                marks_placed++;
                if(grid_main[1, 1] == "empty") {
                    grid_main[1, 1] = "Circle";
                    active_player = true;
                } else

                if(grid_main[0, 0] == "Circle" && grid_main[0, 1] == "Circle" && grid_main[0, 2] == "empty") {
                    /*[X][X][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Circle" && grid_main[0, 1] == "empty" && grid_main[0, 2] == "Circle") {
                    /*[X][ ][X]*/
                    grid_main[0, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[0, 1] == "Circle" && grid_main[0, 2] == "Circle") {
                    /*[ ][X][X]*/
                    grid_main[0, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else

                if(grid_main[1, 0] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[1, 2] == "empty") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 2] = "Circle";
                    /*[X][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[1, 0] == "Circle" && grid_main[1, 1] == "empty" && grid_main[1, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[X][ ][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[1, 0] == "empty" && grid_main[1, 1] == "Circle" && grid_main[1, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 0] = "Circle";
                    /*[ ][X][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else

                if(grid_main[2, 0] == "Circle" && grid_main[2, 1] == "Circle" && grid_main[2, 2] == "empty") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][X][ ]*/
                } else
                if(grid_main[2, 0] == "Circle" && grid_main[2, 1] == "empty" && grid_main[2, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][X]*/
                } else
                if(grid_main[2, 0] == "empty" && grid_main[2, 1] == "Circle" && grid_main[2, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][X][X]*/
                } else

                if(grid_main[0, 0] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[2, 2] == "empty") {
                    /*[X][ ][ ]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Circle" && grid_main[1, 1] == "empty" && grid_main[2, 2] == "Circle") {
                    /*[X][ ][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[1, 1] == "Circle" && grid_main[2, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 0] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else

                if(grid_main[0, 2] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[2, 0] == "empty") {
                    /*[ ][ ][X]*/
                    grid_main[2, 0] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 2] == "Circle" && grid_main[1, 1] == "empty" && grid_main[2, 0] == "Circle") {
                    /*[ ][ ][X]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else
                if(grid_main[0, 2] == "empty" && grid_main[1, 1] == "Circle" && grid_main[2, 0] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else

                if(grid_main[0, 0] == "Circle" && grid_main[1, 0] == "Circle" && grid_main[2, 0] == "empty") {
                    /*[X][ ][ ]*/
                    grid_main[2, 0] = "Circle";
                    /*[X][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Circle" && grid_main[1, 0] == "empty" && grid_main[2, 0] == "Circle") {
                    /*[X][ ][ ]*/
                    grid_main[1, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[1, 0] == "Circle" && grid_main[2, 0] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 0] = "Circle";
                    /*[X][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else

                if(grid_main[0, 1] == "Circle" && grid_main[1, 1] == "Circle" && grid_main[2, 1] == "empty") {
                    /*[ ][X][ ]*/
                    grid_main[2, 1] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 1] == "Circle" && grid_main[1, 1] == "empty" && grid_main[2, 1] == "Circle") {
                    /*[ ][X][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][X][ ]*/
                } else
                if(grid_main[0, 1] == "empty" && grid_main[1, 1] == "Circle" && grid_main[2, 1] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 1] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][X][ ]*/
                } else

                if(grid_main[0, 2] == "Circle" && grid_main[1, 2] == "Circle" && grid_main[2, 2] == "empty") {
                    /*[ ][ ][X]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][ ][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 2] == "Circle" && grid_main[1, 2] == "empty" && grid_main[2, 2] == "Circle") {
                    /*[ ][ ][X]*/
                    grid_main[1, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else
                if(grid_main[0, 2] == "empty" && grid_main[1, 2] == "Circle" && grid_main[2, 2] == "Circle") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][ ][X]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else


                if(grid_main[0, 0] == "Cross" && grid_main[0, 1] == "Cross" && grid_main[0, 2] == "empty") {
                    /*[X][X][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Cross" && grid_main[0, 1] == "empty" && grid_main[0, 2] == "Cross") {
                    /*[X][ ][X]*/
                    grid_main[0, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[0, 1] == "Cross" && grid_main[0, 2] == "Cross") {
                    /*[ ][X][X]*/
                    grid_main[0, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else

                if(grid_main[1, 0] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[1, 2] == "empty") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 2] = "Circle";
                    /*[X][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[1, 0] == "Cross" && grid_main[1, 1] == "empty" && grid_main[1, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[X][ ][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[1, 0] == "empty" && grid_main[1, 1] == "Cross" && grid_main[1, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[1, 0] = "Circle";
                    /*[ ][X][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else

                if(grid_main[2, 0] == "Cross" && grid_main[2, 1] == "Cross" && grid_main[2, 2] == "empty") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][X][ ]*/
                } else
                if(grid_main[2, 0] == "Cross" && grid_main[2, 1] == "empty" && grid_main[2, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][X]*/
                } else
                if(grid_main[2, 0] == "empty" && grid_main[2, 1] == "Cross" && grid_main[2, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[2, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][X][X]*/
                } else

                if(grid_main[0, 0] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[2, 2] == "empty") {
                    /*[X][ ][ ]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Cross" && grid_main[1, 1] == "empty" && grid_main[2, 2] == "Cross") {
                    /*[X][ ][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[1, 1] == "Cross" && grid_main[2, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 0] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else

                if(grid_main[0, 2] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[2, 0] == "empty") {
                    /*[ ][ ][X]*/
                    grid_main[2, 0] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 2] == "Cross" && grid_main[1, 1] == "empty" && grid_main[2, 0] == "Cross") {
                    /*[ ][ ][X]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else
                if(grid_main[0, 2] == "empty" && grid_main[1, 1] == "Cross" && grid_main[2, 0] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else

                if(grid_main[0, 0] == "Cross" && grid_main[1, 0] == "Cross" && grid_main[2, 0] == "empty") {
                    /*[X][ ][ ]*/
                    grid_main[2, 0] = "Circle";
                    /*[X][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 0] == "Cross" && grid_main[1, 0] == "empty" && grid_main[2, 0] == "Cross") {
                    /*[X][ ][ ]*/
                    grid_main[1, 0] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else
                if(grid_main[0, 0] == "empty" && grid_main[1, 0] == "Cross" && grid_main[2, 0] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 0] = "Circle";
                    /*[X][ ][ ]*/
                    active_player = true;
                    /*[X][ ][ ]*/
                } else

                if(grid_main[0, 1] == "Cross" && grid_main[1, 1] == "Cross" && grid_main[2, 1] == "empty") {
                    /*[ ][X][ ]*/
                    grid_main[2, 1] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 1] == "Cross" && grid_main[1, 1] == "empty" && grid_main[2, 1] == "Cross") {
                    /*[ ][X][ ]*/
                    grid_main[1, 1] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][X][ ]*/
                } else
                if(grid_main[0, 1] == "empty" && grid_main[1, 1] == "Cross" && grid_main[2, 1] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 1] = "Circle";
                    /*[ ][X][ ]*/
                    active_player = true;
                    /*[ ][X][ ]*/
                } else

                if(grid_main[0, 2] == "Cross" && grid_main[1, 2] == "Cross" && grid_main[2, 2] == "empty") {
                    /*[ ][ ][X]*/
                    grid_main[2, 2] = "Circle";
                    /*[ ][ ][X]*/
                    active_player = true;
                    /*[ ][ ][ ]*/
                } else
                if(grid_main[0, 2] == "Cross" && grid_main[1, 2] == "empty" && grid_main[2, 2] == "Cross") {
                    /*[ ][ ][X]*/
                    grid_main[1, 2] = "Circle";
                    /*[ ][ ][ ]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else
                if(grid_main[0, 2] == "empty" && grid_main[1, 2] == "Cross" && grid_main[2, 2] == "Cross") {
                    /*[ ][ ][ ]*/
                    grid_main[0, 2] = "Circle";
                    /*[ ][ ][X]*/
                    active_player = true;
                    /*[ ][ ][X]*/
                } else

                if(!active_player) {
                    bool tempbreak = false;
                    while(!tempbreak) {
                        int temprand = random.Next(8);
                        switch(temprand) {
                            case 0: if(grid_main[0, 0] == "empty") { grid_main[0, 0] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 1: if(grid_main[0, 2] == "empty") { grid_main[0, 2] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 2: if(grid_main[2, 0] == "empty") { grid_main[2, 0] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 3: if(grid_main[2, 2] == "empty") { grid_main[2, 2] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 4: if(grid_main[1, 0] == "empty") { grid_main[1, 0] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 5: if(grid_main[1, 2] == "empty") { grid_main[1, 2] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 6: if(grid_main[0, 1] == "empty") { grid_main[0, 1] = "Circle"; active_player = true; tempbreak = true; } break;
                            case 7: if(grid_main[2, 1] == "empty") { grid_main[2, 1] = "Circle"; active_player = true; tempbreak = true; } break;
                        }
                    }
                }
                Check();
            }
        }
    }
}
