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
    class Simon : Ghost {

        float[] alpha = new float[4];
        List<int> color_simon  = new List<int>();
        List<int> color_player = new List<int>();

        int alpha_pos = 0;

        bool[] alpha_player = new bool[4];

        bool active_player = false;
        bool result = false;

        public Simon(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            color_simon.RemoveRange(0, color_simon.Count);
            color_simon.Add(random.Next(4));
            color_simon.Add(random.Next(4));
            if(game_difficulty >= 2) color_simon.Add(random.Next(4));
            if(game_difficulty >= 4) color_simon.Add(random.Next(4));
            color_player.RemoveRange(0, color_player.Count);
            alpha[0] = 0.00f;
            alpha[1] = 0.00f;
            alpha[2] = 0.00f;
            alpha[3] = 0.00f;
            score_level = 1;
            alpha_pos = 0;
            result = false;
            active_player = false;
            alpha_player[0] = false;
            alpha_player[1] = false;
            alpha_player[2] = false;
            alpha_player[3] = false;
        }

        public void Restart() {
            score_level++;
            score_points += score_level;
            color_simon.Add(random.Next(4));
            if(game_difficulty >= 2) color_simon.Add(random.Next(4));
            if(game_difficulty >= 4) color_simon.Add(random.Next(4));
            color_player.RemoveRange(0, color_player.Count);
            alpha_pos = 0;
            result = false;
            active_player = false;
            alpha_player[0] = false;
            alpha_player[1] = false;
            alpha_player[2] = false;
            alpha_player[3] = false;
            alpha[0] = 0.00f;
            alpha[1] = 0.00f;
            alpha[2] = 0.00f;
            alpha[3] = 0.00f;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(active_player) Command_Arrow(0); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(active_player) Command_Arrow(3); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(active_player) Command_Arrow(1); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(active_player) Command_Arrow(2); pressed_response = true; }

            if(pressed_event_touch) {
                if(Collision_Button(false, new Rectangle((int)SK.Position_Simon().X, (int)SK.Position_Simon().Y, 500, 500))) {
                    pressed_response = true;
                    int x = (int)control_cursor.X - (int)SK.Position_Simon().X;
                    int y = (int)control_cursor.Y - (int)SK.Position_Simon().Y;
                    Color temp = TextureData[500*y  + x];
                    if(temp == new Color(new Vector4(255, 0, 0, 255))) {
                        Command_Arrow(0); // UP
                    } else if(temp == new Color(new Vector4(0, 255, 0, 255))) {
                        Command_Arrow(3); // DOWN
                    } else if(temp == new Color(new Vector4(255, 0, 255, 255))) {
                        Command_Arrow(1); // LEFT
                    } else if(temp == new Color(new Vector4(0, 255, 255, 255))) {
                        Command_Arrow(2); // RIGHT
                    }
                }
            }
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            for(int i = 0; i < 4; i++) {
                if(alpha_player[i]) {
                    if(alpha[i] < 1) alpha[i] += Get_Speed();
                    if(alpha[i] >= 1) { alpha[i] = 1.00f; alpha_player[i] = false; }
                } else {
                    if(alpha[i] > 0) alpha[i] -= Get_Speed();
                    if(alpha[i] <= 0) alpha[i] = 0.00f;
                }
            }

            if(!active_player) {
                if(alpha_pos < color_simon.Count) {
                    if(alpha[0] == 0 && alpha[1] == 0 && alpha[2] == 0 && alpha[3] == 0) {
                        alpha_pos++;
                        int temp = 0;
                        foreach(int c in color_simon) {
                            temp++;
                            if(alpha_pos == temp) {
                                alpha[c] += Get_Speed();
                                alpha_player[c] = true;
                                break;
                            }
                        }
                        if(alpha_pos == color_simon.Count) active_player = true;
                    }
                }
            }

            if(!active_gameover) {
                if(alpha[0] == 0 && alpha[1] == 0 && alpha[2] == 0 && alpha[3] == 0) {
                    if(result) {
                        Restart();
                    }
                    if(color_player.Count == color_simon.Count) {
                        bool error = false;
                        int index_simon = 0;
                        foreach(int c1 in color_simon) {
                            int index_player = 0;
                            foreach(int c2 in color_player) {
                                if(index_simon == index_player) {
                                    if(c1 != c2) error = true;
                                }
                                index_player++;
                            }
                            index_simon++;
                        }
                        if(error) {
                            GameOver(gameTime.TotalGameTime.TotalSeconds);
                            result = true;
                        } else {
                            result = true;
                            alpha[0] += Get_Speed(); alpha_player[0] = true;
                            alpha[1] += Get_Speed(); alpha_player[1] = true;
                            alpha[2] += Get_Speed(); alpha_player[2] = true;
                            alpha[3] += Get_Speed(); alpha_player[3] = true;
                            JK.Noise("Cleared");
                        }
                    }
                }
            }
            return "void";
        }

        private void Command_Arrow(int arrow) {
            if(active_player) {
                color_player.Add(arrow);
                alpha_player[arrow] = true;
                alpha[arrow] += Get_Speed();
                if(color_player.Count == color_simon.Count) {
                    active_player = false;
                }
            }
        }

        private float Get_Speed() {
            float f = 0.00f;
            f = (float)(score_level * game_difficulty) / 100;
            return f;
        }

        public override void Draw2() {

            spriteBatch.Draw(SK.texture_static_simon_red, SK.Position_Simon(), Get_Color(0));
            spriteBatch.Draw(SK.texture_static_simon_blue, SK.Position_Simon(), Get_Color(1));
            spriteBatch.Draw(SK.texture_static_simon_green, SK.Position_Simon(), Get_Color(2));
            spriteBatch.Draw(SK.texture_static_simon_yellow, SK.Position_Simon(), Get_Color(3));

            spriteBatch.Draw(SK.texture_static_simon_console, SK.Position_Simon(), Color.White);

            int index = 0;

            foreach(int c in color_player) {
                spriteBatch.Draw(SK.texture_spritesheet_minos_32x, new Vector2(SK.Position_Simon_Player().X, SK.Position_DisplayEdge().Y + 10 + 35 * index), Get_Mino(c), Color.White);
                index++;
            }

            index = 0;

            foreach(int c in color_simon) {
                if(result) {
                    spriteBatch.Draw(SK.texture_spritesheet_minos_32x, new Vector2(SK.Position_Simon_Simon().X, SK.Position_DisplayEdge().Y + 10 + 35 * index), Get_Mino(c), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_spritesheet_minos_32x, new Vector2(SK.Position_Simon_Simon().X, SK.Position_DisplayEdge().Y + 10 + 35 * index), Get_Mino(-1), Color.White);
                }
                index++;
            }
        }

        private Rectangle Get_Mino(int c) {
            switch(c) {
                case 0: return Get_Mino_Texture("fire", 0, 32);
                case 1: return Get_Mino_Texture("water", 0, 32);
                case 2: return Get_Mino_Texture("nature", 0, 32);
                case 3: return Get_Mino_Texture("thunder", 0, 32);
            }
            return Get_Mino_Texture("blank", 0, 32);
        }

        private Color Get_Color(int c) {
            return new Color(new Vector4(0.5f + alpha[c] / 2, 0.5f + alpha[c] / 2, 0.5f + alpha[c] / 2, 100));
        }
    }
}
