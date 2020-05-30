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
    class Minesweeper : Ghost {

        Vector2 selector;

        List<Vector2> FieldList = new List<Vector2>();

        bool active_flag;

        bool[,] grid_cover = new bool[13,10];
        bool[,] grid_flag  = new bool[13,10];
        int[,] grid_base  = new  int[13,10];

        float alpha;

        bool won;

        List<Entity> explosions = new List<Entity>();

        public Minesweeper(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            alpha = 1.00f;
            active_flag = false;
            selector = new Vector2(5, 5);
            FieldList.RemoveRange(0, FieldList.Count);
            score_level = 1;
            Create_Field();
            won = false;
        }

        public void Restart() {
            FieldList.RemoveRange(0, FieldList.Count);
            score_points += score_level * 3;
            score_level++;
            Create_Field();
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { if(active_flag) { active_flag = false; } else { active_flag = true; } pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { Command_Grid_Enter(); pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.Y > 0) selector.Y--; pressed_response = true; } else { if(selector.X < 12) selector.X++; pressed_response = true; } }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.Y < 9) selector.Y++; pressed_response = true; } else { if(selector.X > 0) selector.X--; pressed_response = true; } }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.X > 0) selector.X--; pressed_response = true; } else { if(selector.Y > 0) selector.Y--; pressed_response = true; } }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(SK.orientation <= 2) { if(selector.X < 12) selector.X++; pressed_response = true; } else { if(selector.Y < 9) selector.Y++; pressed_response = true; } }

            if(SK.orientation <= 2) {
                for(int x = 0; x < 13; x++) {
                    for(int y = 0; y < 10; y++) {
                        if(pressed_event_touch) {
                            if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_Grid64().X + 64 * x, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_Grid64().Y + 64 * y, 64, 64))) {
                                if(selector == new Vector2(x, y)) {
                                    Command_Grid_Enter();
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
                                    Command_Grid_Enter();
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
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(alpha < 1) {
                alpha += 0.05f;
                if(alpha >= 1) {
                    Restart();
                }
            }

            foreach(Entity e in explosions) {
                if(e.Get_HP() > 15) {
                    explosions.Remove(e);
                    break;
                }
            }

            if(won && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        public override void Draw2() {
            if(SK.orientation <= 2) {
                for(int y = 0; y < 10; y++) {
                    for(int x = 0; x < 13; x++) {
                        int i = grid_base[x, y];
                        if(grid_flag[x, y]) {
                            i = 12;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        } else if(grid_cover[x, y]) {
                            i = 11;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        } else {
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        }
                    }
                }
                foreach(Entity E in explosions) {
                    float x = E.Get_GridPos().X;
                    float y = E.Get_GridPos().Y;
                    Color color = Color.White;
                    spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_DisplayEdge() + new Vector2((int)(SK.Position_Grid64().X + 32 * x), (int)(SK.Position_Grid64().Y + 32 * y)), new Rectangle(1 + E.Get_HP()/2 + (64 * E.Get_HP()/2), 1, 64, 64), color, 0.0f, new Vector2(0, 0), 2F, SpriteEffects.None, 0.0f);
                    E.Change_HP(1);
                }
                spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2((int)(64 * selector.X), (int)(64 * selector.Y)), new Rectangle(1, 1 + 11 + (64 * 11), 64, 64), Color.Yellow * 0.25f, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
            } else {
                for(int x = 0; x < 13; x++) {
                    for(int y = 0; y < 10; y++) {
                        int i = grid_base[x, y];
                        if(grid_flag[x, y]) {
                            i = 12;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        } else if(grid_cover[x, y]) {
                            i = 11;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        } else {
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        }
                    }
                }
                foreach(Entity E in explosions) {
                    float x = E.Get_GridPos().X;
                    float y = E.Get_GridPos().Y;
                    Color color = Color.White;
                    spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_DisplayEdge() + new Vector2((int)(SK.Position_Grid64().X + 32 * y), (int)(SK.Position_Grid64().Y + 32 * (12 - x))), new Rectangle(1 + E.Get_HP() / 2 + (64 * E.Get_HP() / 2), 1, 64, 64), color, 0.0f, new Vector2(0, 0), 2F, SpriteEffects.None, 0.0f);
                    E.Change_HP(1);
                }
                spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2((int)(64 * selector.Y), (int)(64 * (12 - selector.X))), new Rectangle(1, 1 + 11 + (64 * 11), 64, 64), Color.Yellow * 0.25f, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
            }

            if(alpha < 1) {
                if(SK.orientation <= 2) {
                    for(int y = 0; y < 10; y++) {
                        for(int x = 0; x < 13; x++) {
                            int i = 11;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * x, 64 * y), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White * alpha, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        }
                    }
                } else {
                    for(int x = 0; x < 13; x++) {
                        for(int y = 0; y < 10; y++) {
                            int i = 11;
                            spriteBatch.Draw(SK.texture_spritesheet_minesweeper, SK.Position_DisplayEdge() + SK.Position_Grid64() + new Vector2(64 * y, 64 * (12 - x)), new Rectangle(1, 1 + i + (64 * i), 64, 64), Color.White * alpha, 0.0f, new Vector2(0, 0), 1F, SpriteEffects.None, 0.0f);
                        }
                    }
                }
            }

        }

        public override void Draw3() {
            if(!active_gameover && !active_pause) {
                if(active_flag) {
                    spriteBatch.Draw(SK.texture_hud_button_flag, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_hud_button_select, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                }
            }
        }

        private void Create_Field() {
            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < 13; j++) {
                    grid_base[j, i] = 0;
                    grid_cover[j, i] = true;
                    grid_flag[j, i] = false;
                }
            }
            for(int i = 0; i < score_level * 3; i++) {
                int x = random.Next(13);
                int y = random.Next(10);
                if(grid_base[x, y] != 9) {
                    grid_base[x, y] = 9;
                } else {
                    i--;
                }
            }
            for(int y = 0; y < 10; y++) {
                for(int x = 0; x < 13; x++) {
                    if(grid_base[x, y] != 9) {
                        int count = 0;
                        if(x > 0 && y > 0) if(grid_base[x - 1, y - 1] == 9) count++; // -X -Y
                        if(y > 0) if(grid_base[x, y - 1] == 9) count++; //    -Y
                        if(x < 12 && y > 0) if(grid_base[x + 1, y - 1] == 9) count++; // +X -Y
                        if(x < 12) if(grid_base[x + 1, y] == 9) count++; // +X
                        if(x < 12 && y < 9) if(grid_base[x + 1, y + 1] == 9) count++; // +X +Y
                        if(y < 9) if(grid_base[x, y + 1] == 9) count++; //    +Y
                        if(x > 0 && y < 9) if(grid_base[x - 1, y + 1] == 9) count++; // -X +Y
                        if(x > 0) if(grid_base[x - 1, y] == 9) count++; // -X
                        grid_base[x, y] = count;
                    }
                }
            }
        }

        private void Command_Grid_Enter() {
            if(grid_cover[(int)selector.X, (int)selector.Y]) {
                if(active_flag) {
                    if(grid_flag[(int)selector.X, (int)selector.Y]) {
                        grid_flag[(int)selector.X, (int)selector.Y] = false;
                    } else {
                        grid_flag[(int)selector.X, (int)selector.Y] = true;
                    }
                } else {
                    grid_cover[(int)selector.X, (int)selector.Y] = false;
                    if(grid_base[(int)selector.X, (int)selector.Y] == 9) {
                        grid_base[(int)selector.X, (int)selector.Y] = 10;
                        JK.Noise("Explosion");
                        explosions.Add(new Entity(0, new Vector2(SK.Position_Grid64().X + selector.X * 64, SK.Position_Grid64().Y + selector.Y * 64), new Vector2(0, 0)));
                        Uncover_Bombs();
                        won = true;
                    } else {
                        if(grid_base[(int)selector.X, (int)selector.Y] == 0) {
                            JK.Noise("Place");
                            FieldList.Add(selector);
                            Uncover_Tiles();
                        }
                        bool temp = false;
                        for(int i = 0; i < 10; i++) {
                            for(int j = 0; j < 13; j++) {
                                if(grid_base[j, i] != 9) {
                                    if(grid_cover[j, i]) {
                                        temp = true;
                                    }
                                }
                            }
                        }
                        if(!temp) {
                            alpha = 0.00f;
                            JK.Noise("Cleared");
                        }
                    }
                }
            }
        }

        private void Uncover_Tiles() {
            while(FieldList.Count > 0) {
                bool temp0 = false;
                foreach(Vector2 v in FieldList) {
                    if(v.X > 0 && v.Y > 0) { if(grid_cover[(int)v.X - 1, (int)v.Y - 1] && !grid_flag[(int)v.X - 1, (int)v.Y - 1]) { grid_cover[(int)v.X - 1, (int)v.Y - 1] = false; if(grid_base[(int)v.X - 1, (int)v.Y - 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X - 1, v.Y - 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X - 1, v.Y - 1)); temp0 = true; break; } } } } } // -X -Y
                    if(v.Y > 0) { if(grid_cover[(int)v.X, (int)v.Y - 1] && !grid_flag[(int)v.X, (int)v.Y - 1]) { grid_cover[(int)v.X, (int)v.Y - 1] = false; if(grid_base[(int)v.X, (int)v.Y - 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X, v.Y - 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X, v.Y - 1)); temp0 = true; break; } } } } } //    -Y
                    if(v.X < 12 && v.Y > 0) { if(grid_cover[(int)v.X + 1, (int)v.Y - 1] && !grid_flag[(int)v.X + 1, (int)v.Y - 1]) { grid_cover[(int)v.X + 1, (int)v.Y - 1] = false; if(grid_base[(int)v.X + 1, (int)v.Y - 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X + 1, v.Y - 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X + 1, v.Y - 1)); temp0 = true; break; } } } } } // +X -Y
                    if(v.X < 12) { if(grid_cover[(int)v.X + 1, (int)v.Y] && !grid_flag[(int)v.X + 1, (int)v.Y]) { grid_cover[(int)v.X + 1, (int)v.Y] = false; if(grid_base[(int)v.X + 1, (int)v.Y] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X + 1, v.Y) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X + 1, v.Y)); temp0 = true; break; } } } } } // +X
                    if(v.X < 12 && v.Y < 9) { if(grid_cover[(int)v.X + 1, (int)v.Y + 1] && !grid_flag[(int)v.X + 1, (int)v.Y + 1]) { grid_cover[(int)v.X + 1, (int)v.Y + 1] = false; if(grid_base[(int)v.X + 1, (int)v.Y + 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X + 1, v.Y + 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X + 1, v.Y + 1)); temp0 = true; break; } } } } } // +X +Y
                    if(v.Y < 9) { if(grid_cover[(int)v.X, (int)v.Y + 1] && !grid_flag[(int)v.X, (int)v.Y + 1]) { grid_cover[(int)v.X, (int)v.Y + 1] = false; if(grid_base[(int)v.X, (int)v.Y + 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X, v.Y + 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X, v.Y + 1)); temp0 = true; break; } } } } } //    +Y
                    if(v.X > 0 && v.Y < 9) { if(grid_cover[(int)v.X - 1, (int)v.Y + 1] && !grid_flag[(int)v.X - 1, (int)v.Y + 1]) { grid_cover[(int)v.X - 1, (int)v.Y + 1] = false; if(grid_base[(int)v.X - 1, (int)v.Y + 1] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X - 1, v.Y + 1) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X - 1, v.Y + 1)); temp0 = true; break; } } } } } // -X +Y
                    if(v.X > 0) { if(grid_cover[(int)v.X - 1, (int)v.Y] && !grid_flag[(int)v.X - 1, (int)v.Y]) { grid_cover[(int)v.X - 1, (int)v.Y] = false; if(grid_base[(int)v.X - 1, (int)v.Y] == 0) { bool temp = false; foreach(Vector2 v2 in FieldList) { if(new Vector2(v.X - 1, v.Y) == v2) { temp = true; break; } if(!temp) { FieldList.Add(new Vector2(v.X - 1, v.Y)); temp0 = true; break; } } } } } // -X
                    break;
                }
                if(!temp0) FieldList.RemoveAt(0);
            }
        }

        private void Uncover_Bombs() {
            for(int y = 0; y < 10; y++) {
                for(int x = 0; x < 13; x++) {
                    if(grid_base[x, y] == 9) {
                        grid_cover[x, y] = false;
                    }
                }
            }
        }
    }
}
