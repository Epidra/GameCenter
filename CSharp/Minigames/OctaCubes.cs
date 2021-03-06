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
    class OctaCubes : Ghost {

        int[,] grid_main   = new int[16, 12];
        string looking = "Down";

        float alpha;
        bool alphaUP;

        Entity octanom;
        List<Entity> boulders   = new List<Entity>();
        List<Entity> explosions = new List<Entity>();

        string temp_movement;

        public OctaCubes(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
            id = _id;
            CM = _content;
            SK = _Shopkeeper;
            FM = _Filemanager;
            JK = _Jukebox;
            screensize_width  = (int)SK.screensize_main.X;
            screensize_height = (int)SK.screensize_main.Y;
            screensize_width_scale  = screenX;
            screensize_height_scale = screenY;
            random = new Random();
        }

        public override void Start2() {
            temp_movement = "empty";
            grid_main = new int[3, 3];
            score_level = 1;
            score_lives = FM.octaLives;
            Create_Grid();
            octanom = new Entity(1, new Vector2(0,0), new Vector2(0, 0));
            boulders.RemoveRange(0, boulders.Count);
            alpha = 1.00f;
            alphaUP = false;
        }

        public void Restart() {
            temp_movement = "empty";
            if(score_level < 10) score_level++;
            grid_main = new int[3 + score_level, 3 + score_level];
            Create_Grid();
            octanom = new Entity(1, new Vector2(0, 0), new Vector2(0, 0));
            boulders.RemoveRange(0, boulders.Count);
        }

        private void Create_Grid() {
            int index = grid_main.GetLength(0);
            for(int y = 0; y < grid_main.GetLength(1); y++) {
                for(int x = 0; x < grid_main.GetLength(0); x++) {
                    if(x < index) {
                        grid_main[x, y] = 1;
                    } else {
                        grid_main[x, y] = 0;
                    }
                }
                index--;
            }
        }

        public override string Update2() {
            if(alpha == 1 && octanom.Get_HP() > 0) {
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed)    { pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { temp_movement = "Up";    pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { temp_movement = "Down";  pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { temp_movement = "Left";  pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { temp_movement = "Right"; pressed_response = true; }
            }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            bool tempwin = true;
            for(int y = 0; y < grid_main.GetLength(1); y++) {
                for(int x = 0; x < grid_main.GetLength(0); x++) {
                    if(grid_main[x, y] == 1) tempwin = false;
                }
            }

            if(tempwin) {
                JK.Noise("Cleared");
                alpha -= 0.02f;
                alphaUP = false;
            }

            if(alpha != 1) {
                if(alphaUP) {
                    alpha += 0.02f;
                    if(alpha >= 1) alpha = 1;
                } else {
                    alpha -= 0.02f;
                    if(alpha <= 0) {
                        alphaUP = true;
                        Restart();
                    }
                }
            }

            if(score_lives == 0 && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            if(!active_pause && !active_gameover) {
                octanom.Update();
                foreach(Entity e in boulders) {
                    e.Update();
                }
                Command_Move();
            }

            foreach(Entity e in explosions) {
                if(e.Get_HP() > 15) {
                    explosions.Remove(e);
                    break;
                }
            }
            return "void";
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_spritesheet_octacubes, SK.Position_OctaCubes() + new Vector2(32 - 16, 8 - 32 - 8 * score_level), new Rectangle(128, 0, 64, 48), Color.White * alpha, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_octacubes, SK.Position_OctaCubes() + new Vector2(-32 - 16, 8 - 32 - 8 * score_level), new Rectangle(128, 0, 64, 48), Color.White * alpha, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            for(int y = 0; y < grid_main.GetLength(1); y++) {
                for(int x = 0; x < grid_main.GetLength(0); x++) {
                    if(grid_main[x, y] == 1) spriteBatch.Draw(SK.texture_spritesheet_octacubes, SK.Position_OctaCubes() + new Vector2(32 * x - 32 * y - 16, 32 * x + 32 * y + 8 - 8 * score_level), new Rectangle(0, 0, 64, 48), Color.White * alpha, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    if(grid_main[x, y] == 2) spriteBatch.Draw(SK.texture_spritesheet_octacubes, SK.Position_OctaCubes() + new Vector2(32 * x - 32 * y - 16, 32 * x + 32 * y + 8 - 8 * score_level), new Rectangle(64, 0, 64, 48), Color.White * alpha, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }
            }
            foreach(Entity c in boulders) { spriteBatch.Draw(SK.texture_static_boulder, SK.Position_OctaCubes() + c.Get_GridPos() + new Vector2(0, - 8 * score_level), Color.White * alpha); }
            if(octanom.Get_HP() > 0 && score_lives > 0) spriteBatch.Draw(SK.texture_spritesheet_octanom_head, SK.Position_OctaCubes() + new Vector2(octanom.Get_GridPos().X, octanom.Get_GridPos().Y - 8 * score_level), new Rectangle(0, octanom.Get_LookDirection() * 32, 32, 32), Color.White * alpha, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            foreach(Entity E in explosions) {
                spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_OctaCubes() + E.Get_Pos() - new Vector2(16), new Rectangle(1 + E.Get_HP() + (64 * (E.Get_HP()/2)), 1, 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                E.Change_HP(1);
            }
        }

        private void Create_Trianom() {
            if(random.Next(15) - score_level * 2 <= 0) {
                if(random.Next(2) == 0) {
                    boulders.Add(new Entity(1, new Vector2(0, -64), new Vector2( 2, 2), new Vector2(0, -1)));
                } else {
                    boulders.Add(new Entity(1, new Vector2(0, -64), new Vector2(-2, 2), new Vector2(-1, 0)));
                }
            }
        }

        private void Command_Move() {
            if(octanom.Get_Pos().X == octanom.Get_Next().X * 32 - octanom.Get_Next().Y * 32 && octanom.Get_Pos().Y == octanom.Get_Next().X * 32 + octanom.Get_Next().Y * 32) {
                octanom.Set_Vel(0,0);
                foreach(Entity t in boulders) {
                    if(octanom.Get_Next() == t.Get_Next() && octanom.Get_HP() > 0) {
                        JK.Noise("Explosion");
                        explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                        score_lives--;
                        //octanom.Set_HP(0);
                        boulders.Remove(t);
                        break;
                    }
                }
                /*if(octanom.Get_HP() == 0) {
                    octanom.Set_Pos(0,0);
                    octanom.Set_Next(new Vector2(0, 0));
                    octanom.Set_HP(1);
                    foreach(Entity t in boulders) {
                        if(t.Get_Next() == new Vector2(0, 0)) {
                            boulders.Remove(t);
                            break;
                        }
                    }
                }*/
                if(grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y] == 1) {
                    grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y]++;
                    score_points++;
                    JK.Noise("Place");
                }
                foreach(Entity t in boulders) {
                    t.Set_Vel(0,0);
                    if(t.Get_Next().X == grid_main.GetLength(0) || t.Get_Next().Y == grid_main.GetLength(1)) {
                        boulders.Remove(t);
                        break;
                    } else if(t.Get_Next().X >= 0 && t.Get_Next().Y >= 0) {
                        if(grid_main[(int)t.Get_Next().X, (int)t.Get_Next().Y] == 0) {
                            boulders.Remove(t);
                            break;
                        }
                    }
                }
                if(temp_movement != "null") looking = temp_movement;
                if(temp_movement == "Up" && octanom.Get_Next().Y > 0) {
                    if(grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y - 1] != 0) {
                        octanom.Set_Vel(2, -2);
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, octanom.Get_Next().Y - 1));
                        Create_Trianom();
                    }
                }
                if(temp_movement == "Down" && octanom.Get_Next().Y < grid_main.GetLength(1) - 1) {
                    if(grid_main[(int)octanom.Get_Next().X, (int)octanom.Get_Next().Y + 1] != 0) {
                        octanom.Set_Vel(-2, 2);
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, octanom.Get_Next().Y + 1));
                        Create_Trianom();
                    }
                }
                if(temp_movement == "Left" && octanom.Get_Next().X > 0) {
                    if(grid_main[(int)octanom.Get_Next().X - 1, (int)octanom.Get_Next().Y] != 0) {
                        octanom.Set_Vel(-2, -2);
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X - 1, octanom.Get_Next().Y));
                        Create_Trianom();
                    }
                }
                if(temp_movement == "Right" && octanom.Get_Next().X < grid_main.GetLength(0) - 1) {
                    if(grid_main[(int)octanom.Get_Next().X + 1, (int)octanom.Get_Next().Y] != 0) {
                        octanom.Set_Vel(2, 2);
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X + 1, octanom.Get_Next().Y));
                        Create_Trianom();
                    }
                }

                if(octanom.Get_Vel().X != 0 || octanom.Get_Vel().Y != 0) {
                    foreach(Entity t in boulders) {
                        if(t.Get_Vel().X == 0 && t.Get_Vel().Y == 0) {
                            if(t.Get_Next() == new Vector2(-1, 0)) {
                                t.Set_Next(t.Get_Next() + new Vector2(1, 0));
                                t.Set_Vel(2, 2);
                            } else if(t.Get_Next() == new Vector2(0, -1)) {
                                t.Set_Next(t.Get_Next() + new Vector2(0, 1));
                                t.Set_Vel(-2, 2);
                            } else {
                                if(random.Next(2) == 0) {
                                    t.Set_Next(t.Get_Next() + new Vector2(0, 1));
                                    t.Set_Vel(-2, 2);
                                } else {
                                    t.Set_Next(t.Get_Next() + new Vector2(1, 0));
                                    t.Set_Vel(2, 2);
                                }
                            }
                        }
                    }
                }

                temp_movement = "null";
            }
        }
        
    }
}
