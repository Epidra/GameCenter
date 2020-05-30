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
    class Memory : Ghost {

        string[,] grid_main = new string[13,10];

        Vector2 selector;

        bool    selected_A;
        bool    selected_B;
        Vector2 selected_A_pos;
        Vector2 selected_B_pos;

        float alpha;
        float beta;

        bool timer;
        int  time;

        public Memory(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            selected_A = false;
            selected_B = false;
            timer = false;
            score_lives = 4;
            score_level = 1;
            selector = new Vector2(2, 1);
            selected_A_pos = new Vector2(0, 0);
            selected_B_pos = new Vector2(0, 0);
            Command_Create_Grid();
            alpha = 1.00f;
            beta = 0.00f;
            time = 100;
        }

        public void Restart() {
            score_level++;
            score_lives += score_level * 3;
            Command_Create_Grid();
            alpha = 1.00f;
            beta = 0.00f;
            time = 100;
        }

        public override string Update2() {
            if(beta == 1.00f) {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { Command_Enter(); pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.Y > 0) selector.Y--; pressed_response = true; } else { if(selector.X < 12) selector.X++; pressed_response = true; } }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.Y < 9) selector.Y++; pressed_response = true; } else { if(selector.X > 0) selector.X--; pressed_response = true; } }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.X > 0) selector.X--; pressed_response = true; } else { if(selector.Y > 0) selector.Y--; pressed_response = true; } }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.X < 12) selector.X++; pressed_response = true; } else { if(selector.Y < 9) selector.Y++; pressed_response = true; } }
                if(selected_A && selected_B && !timer) {
                    timer = true;
                }
                if(SK.orientation <= 2) {
                    for(int x = 0; x < 13; x++) {
                        for(int y = 0; y < 10; y++) {
                            if(pressed_event_touch) {
                                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Grid64().X + 64 * x, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Grid64().Y + 64 * y, 64, 64))) {
                                    if(selector == new Vector2(x, y)) {
                                        JK.Noise("Place");
                                        if(!selected_A) {
                                            if(grid_main[x, y] != "empty") {
                                                selected_A = true;
                                                selected_A_pos = new Vector2(x, y);
                                            }
                                        } else if(!selected_B) {
                                            if(grid_main[x, y] != "empty" && selected_A_pos != new Vector2(x, y)) {
                                                selected_B = true;
                                                selected_B_pos = new Vector2(x, y);
                                            }
                                        }
                                    } else { selector = new Vector2(x, y); }
                                }
                            } else {
                                if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Grid64().X + 64 * x, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Grid64().Y + 64 * y, 64, 64))) { selector = new Vector2(x, y); }
                            }
                        }
                    }
                } else {
                    for(int x = 0; x < 10; x++) {
                        for(int y = 0; y < 13; y++) {
                            if(pressed_event_touch) {
                                if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Grid64().X + 64 * x, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Grid64().Y + 64 * y, 64, 64))) {
                                    if(selector == new Vector2(12 - y, x)) {
                                        JK.Noise("Place");
                                        if(!selected_A) {
                                            if(grid_main[12 - y, x] != "empty") {
                                                selected_A = true;
                                                selected_A_pos = new Vector2(12 - y, x);
                                            }
                                        } else if(!selected_B) {
                                            if(grid_main[12 - y, x] != "empty" && selected_A_pos != new Vector2(12 - y, x)) {
                                                selected_B = true;
                                                selected_B_pos = new Vector2(12 - y, x);
                                            }
                                        }
                                    } else { selector = new Vector2(12 - y, x); }
                                }
                            } else {
                                if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Grid64().X + 64 * x, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Grid64().Y + 64 * y, 64, 64))) {
                                    selector = new Vector2(12 - y, x);
                                }
                            }
                        }
                    }
                }
            } else {
                beta += 0.05f;
                if(beta >= 1) beta = 1.00f;
            }
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(score_lives == 0 && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            if(timer) {
                if(grid_main[(int)selected_A_pos.X, (int)selected_A_pos.Y] == grid_main[(int)selected_B_pos.X, (int)selected_B_pos.Y]) {
                    alpha -= 0.05f;
                }
                time -= 5;
                if(time <= 0) {
                    alpha = 1.00f;
                    time = 100;
                    if(grid_main[(int)selected_A_pos.X, (int)selected_A_pos.Y] == grid_main[(int)selected_B_pos.X, (int)selected_B_pos.Y]) {
                        if(grid_main[(int)selected_A_pos.X, (int)selected_A_pos.Y] == "gold") score_points += 4;
                        grid_main[(int)selected_A_pos.X, (int)selected_A_pos.Y] = "empty";
                        grid_main[(int)selected_B_pos.X, (int)selected_B_pos.Y] = "empty";
                        score_points++;
                    } else { score_lives--; }
                    bool temp = false;
                    for(int x = 0; x < 13; x++) {
                        for(int y = 0; y < 10; y++) {
                            if(grid_main[x, y] != "empty") temp = true;
                        }
                    }
                    if(!temp) {
                        JK.Noise("Cleared");
                        Restart();
                    }
                    selected_A = false;
                    selected_B = false;
                    timer = false;
                }
            }
            return "void";
        }

        public override void Draw2() {
            if(SK.orientation <= 2) {
                for(int x = 0; x < 13; x++) {
                    for(int y = 0; y < 10; y++) {
                        if(selected_A && selected_A_pos == new Vector2(x, y)) {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), Get_Mino_Texture(grid_main[x, y], 1, 64), Color.White * alpha);
                        } else if(selected_B && selected_B_pos == new Vector2(x, y)) {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), Get_Mino_Texture(grid_main[x, y], 1, 64), Color.White * alpha);
                        } else {
                            if(grid_main[x, y] != "empty") { spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), new Rectangle(0, 0, 64, 64), Color.White * beta); }
                        }
                    }
                }
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * selector.X, 64 * selector.Y), new Rectangle(0, 0, 64, 64), Color.Violet * 0.25f);
            } else {
                for(int x = 0; x < 13; x++) {
                    for(int y = 0; y < 10; y++) {
                        if(selected_A && selected_A_pos == new Vector2(x, y)) {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), Get_Mino_Texture(grid_main[x, y], 1, 64), Color.White * alpha);
                        } else if(selected_B && selected_B_pos == new Vector2(x, y)) {
                            spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), Get_Mino_Texture(grid_main[x, y], 1, 64), Color.White * alpha);
                        } else {
                            if(grid_main[x, y] != "empty") { spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), new Rectangle(0, 0, 64, 64), Color.White * beta); }
                        }
                    }
                }
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * selector.Y, 64 * (12 - selector.X)), new Rectangle(0, 0, 64, 64), Color.Violet * 0.25f);
            }
        }

        private void Command_Create_Grid() {
            for(int x = 0; x < 13; x++) {
                for(int y = 0; y < 10; y++) {
                    grid_main[x, y] = "empty";
                }
            }
            int filler = 0;
            if(score_level == 1) filler = 8;
            if(score_level == 2) filler = 12;
            if(score_level == 3) filler = 16;
            if(score_level == 4) filler = 20;
            if(score_level == 5) filler = 24;
            if(score_level == 6) filler = 30;
            if(score_level == 7) filler = 36;
            if(score_level == 8) filler = 42;
            if(score_level == 9) filler = 48;
            if(score_level == 10) filler = 56;
            if(score_level == 11) filler = 64;
            if(score_level == 12) filler = 72;
            if(score_level == 13) filler = 80;
            if(score_level == 14) filler = 90;
            if(score_level == 15) filler = 100;
            if(score_level == 16) filler = 110;
            if(score_level == 17) filler = 120;
            if(score_level >= 18) filler = 130;

            int color_fire    = 0;
            int color_air     = 0;
            int color_thunder = 0;
            int color_water   = 0;
            int color_ice     = 0;
            int color_earth   = 0;
            int color_metal   = 0;
            int color_nature  = 0;
            int color_light   = 0;
            int color_dark    = 0;
            while(filler > 0) {
                if(filler > 0) { color_fire += 2; filler -= 2; }
                if(filler > 0) { color_air += 2; filler -= 2; }
                if(filler > 0) { color_thunder += 2; filler -= 2; }
                if(filler > 0) { color_water += 2; filler -= 2; }
                if(filler > 0) { color_ice += 2; filler -= 2; }
                if(filler > 0) { color_earth += 2; filler -= 2; }
                if(filler > 0) { color_metal += 2; filler -= 2; }
                if(filler > 0) { color_nature += 2; filler -= 2; }
                if(filler > 0) { color_light += 2; filler -= 2; }
                if(filler > 0) { color_dark += 2; filler -= 2; }
            }
            int X1 = 0; int X2 = 0; int Y1 = 0; int Y2 = 0;
            if(score_level == 1) { X1 = 4; X2 = 4; Y1 = 4; Y2 = 2; }
            if(score_level == 2) { X1 = 4; X2 = 4; Y1 = 4; Y2 = 3; }
            if(score_level == 3) { X1 = 4; X2 = 4; Y1 = 3; Y2 = 4; }
            if(score_level == 4) { X1 = 4; X2 = 5; Y1 = 3; Y2 = 4; }
            if(score_level == 5) { X1 = 3; X2 = 6; Y1 = 3; Y2 = 4; }
            if(score_level == 6) { X1 = 3; X2 = 6; Y1 = 3; Y2 = 5; }
            if(score_level == 7) { X1 = 3; X2 = 6; Y1 = 2; Y2 = 6; }
            if(score_level == 8) { X1 = 3; X2 = 7; Y1 = 2; Y2 = 6; }
            if(score_level == 9) { X1 = 2; X2 = 8; Y1 = 2; Y2 = 6; }
            if(score_level == 10) { X1 = 2; X2 = 8; Y1 = 2; Y2 = 7; }
            if(score_level == 11) { X1 = 2; X2 = 8; Y1 = 1; Y2 = 8; }
            if(score_level == 12) { X1 = 2; X2 = 9; Y1 = 1; Y2 = 8; }
            if(score_level == 13) { X1 = 1; X2 = 10; Y1 = 1; Y2 = 8; }
            if(score_level == 14) { X1 = 1; X2 = 10; Y1 = 1; Y2 = 9; }
            if(score_level == 15) { X1 = 1; X2 = 10; Y1 = 0; Y2 = 10; }
            if(score_level == 16) { X1 = 1; X2 = 11; Y1 = 0; Y2 = 10; }
            if(score_level == 17) { X1 = 0; X2 = 12; Y1 = 0; Y2 = 10; }
            if(score_level >= 18) { X1 = 0; X2 = 13; Y1 = 0; Y2 = 10; }
            for(int x = 0; x < X2; x++) {
                for(int y = 0; y < Y2; y++) {
                    bool temp = false;
                    while(!temp) {
                        int r = random.Next(10);
                        if(r == 0 && color_fire > 0) { grid_main[X1 + x, Y1 + y] = "fire"; color_fire--; temp = true; }
                        if(r == 1 && color_air > 0) { grid_main[X1 + x, Y1 + y] = "air"; color_air--; temp = true; }
                        if(r == 2 && color_thunder > 0) { grid_main[X1 + x, Y1 + y] = "thunder"; color_thunder--; temp = true; }
                        if(r == 3 && color_water > 0) { grid_main[X1 + x, Y1 + y] = "water"; color_water--; temp = true; }
                        if(r == 4 && color_ice > 0) { grid_main[X1 + x, Y1 + y] = "ice"; color_ice--; temp = true; }
                        if(r == 5 && color_earth > 0) { grid_main[X1 + x, Y1 + y] = "earth"; color_earth--; temp = true; }
                        if(r == 6 && color_metal > 0) { grid_main[X1 + x, Y1 + y] = "metal"; color_metal--; temp = true; }
                        if(r == 7 && color_nature > 0) { grid_main[X1 + x, Y1 + y] = "nature"; color_nature--; temp = true; }
                        if(r == 8 && color_light > 0) { grid_main[X1 + x, Y1 + y] = "light"; color_light--; temp = true; }
                        if(r == 9 && color_dark > 0) { grid_main[X1 + x, Y1 + y] = "dark"; color_dark--; temp = true; }
                    }
                }
            }
            if(score_level >= 3 && FM.minos[11]) {
                if(random.Next(10) == 0) {
                    int i = 2;
                    while(i != 0) {
                        int x = random.Next(13);
                        int y = random.Next(10);
                        if(grid_main[x, y] == "fire") {
                            grid_main[x, y] = "gold";
                            i--;
                        }
                    }
                }
            }
        }

        private void Command_Enter() {
            JK.Noise("Place");
            if(!selected_A) {
                if(grid_main[(int)selector.X, (int)selector.Y] != "empty") {
                    selected_A = true;
                    selected_A_pos = new Vector2((int)selector.X, (int)selector.Y);
                }
            } else if(!selected_B) {
                if(grid_main[(int)selector.X, (int)selector.Y] != "empty" && selected_A_pos != new Vector2((int)selector.X, (int)selector.Y)) {
                    selected_B = true;
                    selected_B_pos = new Vector2((int)selector.X, (int)selector.Y);
                }
            }
        }
    }
}
