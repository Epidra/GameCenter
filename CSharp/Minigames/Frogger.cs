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
    class Frogger : Ghost {

        List<Entity> explosions = new List<Entity>();
        List<Entity> lilypad    = new List<Entity>();
        List<Entity> trianom    = new List<Entity>();
        List<Entity> crate      = new List<Entity>();

        int[] field = new int[23];

        int map = 0;

        Entity octanom;

        string temp_movement;

        int counter = 0;

        bool moving = false;

        public Frogger(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            temp_movement = "empty";
            octanom = new Entity(1, new Vector2(32 * 13, 32 * 22), new Vector2(13, 22));
            lilypad.RemoveRange(0, lilypad.Count);
            trianom.RemoveRange(0, trianom.Count);
            crate.RemoveRange(0, crate.Count);
            map = 0;
            counter = 0;
            Build_Field();
            moving = false;
        }

        private void Build_Field() {
            lilypad.RemoveRange(0, lilypad.Count);
            trianom.RemoveRange(0, trianom.Count);
            crate.RemoveRange(0, crate.Count);
            int r = 0;
            field[0] = 0;
            field[1] = 0;
            r = random.Next(2) + 1;
            field[2] = r;
            field[3] = r;
            field[4] = r;
            field[5] = r;
            field[6] = r;

            field[7] = 0;
            field[8] = 0;
            r = random.Next(2) + 1;
            field[9] = r;
            field[10] = r;
            field[11] = r;
            field[12] = r;
            field[13] = r;

            field[14] = 0;
            field[15] = 0;
            r = random.Next(2) + 1;
            field[16] = r;
            field[17] = r;
            field[18] = r;
            field[19] = r;
            field[20] = r;

            field[21] = 0;
            field[22] = 0;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(SK.orientation <= 2) { temp_movement = "Up"; } else { temp_movement = "Right"; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(SK.orientation <= 2) { temp_movement = "Down"; } else { temp_movement = "Left"; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(SK.orientation <= 2) { temp_movement = "Left"; } else { temp_movement = "Up"; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(SK.orientation <= 2) { temp_movement = "Right"; } else { temp_movement = "Down"; } pressed_response = true; }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(!active_pause && !active_gameover) {
                octanom.Update();
                foreach(Entity e in lilypad) {
                    if(moving) {
                        if(e.Get_Next().Y == octanom.Get_Next().Y || e.Get_Next().Y == octanom.Get_Pos().Y / 32) {

                        } else {
                            e.Update();
                        }
                    } else {
                        e.Update();
                    }
                }
                foreach(Entity e in trianom) {
                    e.Update();
                    if(octanom.Get_Pos().X < e.Get_Pos().X + 16 && octanom.Get_Pos().X > e.Get_Pos().X - 16 && e.Get_Pos().Y == octanom.Get_Pos().Y) {
                        octanom.Set_HP(0);
                        JK.Noise("Explosion");
                        explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                    }
                }
                foreach(Entity e in crate) {
                    e.Update();
                }
                if(octanom.Get_Pos().X < -16 || octanom.Get_Pos().X > 28 * 32) {
                    octanom.Set_HP(0);
                    JK.Noise("Explosion");
                    explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                }
                if(octanom.Get_HP() == 0 && !active_gameover) {
                    GameOver(gameTime.TotalGameTime.TotalSeconds);
                }
                counter += 2;
                if(counter >= 32) {
                    counter = 0;
                    Summon(20);
                    Summon(19);
                    Summon(18);
                    Summon(17);
                    Summon(16);

                    Summon(13);
                    Summon(12);
                    Summon(11);
                    Summon(10);
                    Summon(9);

                    Summon(6);
                    Summon(5);
                    Summon(4);
                    Summon(3);
                    Summon(2);
                }
                foreach(Entity e in lilypad) {
                    if(e.Get_Pos().X < -32 || e.Get_Pos().X > 28 * 32) {
                        lilypad.Remove(e);
                        break;
                    }
                }
                foreach(Entity e in trianom) {
                    if(e.Get_Pos().X < -32 || e.Get_Pos().X > 28 * 32) {
                        lilypad.Remove(e);
                        break;
                    }
                }
                Command_Move(gameTime);
            }

            foreach(Entity e in explosions) {
                if(e.Get_HP() > 15) {
                    explosions.Remove(e);
                    break;
                }
            }
            return "void";
        }

        private void Summon(int pos) {
            bool left = true; if(pos % 2 != 0) left = false;
            if(field[pos] == 1) {
                int r = random.Next(10 + map*5); if(r > 80) r = 80;
                if(random.Next(100) + r >= 100) {
                    if(left) { trianom.Add(new Entity(1, new Vector2(-1 * 32, 32 * pos), new Vector2(2, 0), new Vector2(0, pos))); } else { trianom.Add(new Entity(1, new Vector2(28 * 32, 32 * pos), new Vector2(-2, 0), new Vector2(27, pos))); }
                }
            } else {
                int r = random.Next(100 - map*5); if(r < 5) r = 5;
                if(-random.Next(100) + r <= 0) {
                    if(left) { lilypad.Add(new Entity(1, new Vector2(-1 * 32, 32 * pos), new Vector2(2, 0), new Vector2(0, pos))); } else { lilypad.Add(new Entity(1, new Vector2(28 * 32, 32 * pos), new Vector2(-2, 0), new Vector2(27, pos))); }
                }
            }
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_grid32, SK.Position_DisplayEdge() + SK.Position_BackgroundGrid(), Color.White);
            if(SK.orientation <= 2) {
                for(int x = 0; x < 28; x++) {
                    for(int y = 0; y < 23; y++) {
                        if(field[y] == 0) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * x, 32 * y), new Rectangle(32 * 2, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                        if(field[y] == 1) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * x, 32 * y), new Rectangle(32 * 3, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                        if(field[y] == 2) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * x, 32 * y), new Rectangle(32 * 4, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    }
                }
                foreach(Entity c in lilypad) { spriteBatch.Draw(SK.texture_spritesheet_octanom_tail, SK.Position_DisplayEdge() + SK.Position_Grid32() + c.Get_Pos(), new Rectangle(0, 0, 32, 32), Color.LightGreen, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f); }
                foreach(Entity c in crate) { spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + c.Get_Pos(), new Rectangle(0, 32, 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f); }

                if(octanom.Get_HP() > 0) spriteBatch.Draw(SK.texture_spritesheet_octanom_head, SK.Position_DisplayEdge() + SK.Position_Grid32() + octanom.Get_Pos(), new Rectangle(0, octanom.Get_LookDirection() * (32), 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

                foreach(Entity c in trianom) {
                    spriteBatch.Draw(SK.texture_spritesheet_trianom, SK.Position_DisplayEdge() + SK.Position_Grid32() + c.Get_Pos(), new Rectangle(0, c.Get_LookDirection() * (32), 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }

                foreach(Entity E in explosions) {
                    spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_DisplayEdge() + SK.Position_Grid32() + E.Get_Pos() - new Vector2(16), new Rectangle(1 + E.Get_HP() + (64 * (E.Get_HP()/2)), 1, 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    E.Change_HP(1);
                }



            } else {
                for(int x = 0; x < 28; x++) {
                    for(int y = 0; y < 23; y++) {
                        if(field[y] == 0) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * y, 32 * (27 - x)), new Rectangle(32 * 2, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                        if(field[y] == 1) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * y, 32 * (27 - x)), new Rectangle(32 * 3, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                        if(field[y] == 2) spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(32 * y, 32 * (27 - x)), new Rectangle(32 * 4, 0, 32, 32), Color.WhiteSmoke, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    }
                }
                foreach(Entity c in lilypad) { spriteBatch.Draw(SK.texture_spritesheet_octanom_tail, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(c.Get_Pos().Y, 27 * 32 - c.Get_Pos().X), new Rectangle(0, 0, 32, 32), Color.LightGreen, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f); }
                foreach(Entity c in crate) { spriteBatch.Draw(SK.texture_spritesheet_octagames, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(c.Get_Pos().Y, 27 * 32 - c.Get_Pos().X), new Rectangle(0, 32, 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f); }

                if(octanom.Get_HP() > 0) spriteBatch.Draw(SK.texture_spritesheet_octanom_head, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(octanom.Get_Pos().Y, 27 * 32 - octanom.Get_Pos().X), new Rectangle(0, octanom.Get_LookDirection() * (32), 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

                foreach(Entity c in trianom) {
                    spriteBatch.Draw(SK.texture_spritesheet_trianom, SK.Position_DisplayEdge() + SK.Position_Grid32() + new Vector2(c.Get_Pos().Y, 27 * 32 - c.Get_Pos().X), new Rectangle(0, c.Get_LookDirection() * (32), 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                }

                foreach(Entity E in explosions) {
                    spriteBatch.Draw(SK.texture_spritesheet_explosion, SK.Position_DisplayEdge() + new Vector2(E.Get_Pos().Y - 16, E.Get_Pos().X - 16 - 27 * 32), new Rectangle(1 + E.Get_HP() + (64 * (E.Get_HP()/2)), 1, 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    E.Change_HP(1);
                }

            }
        }

        private void Command_Move(GameTime gameTime) {
            if(octanom.Get_Pos() == octanom.Get_Next() * 32) {
                octanom.Set_Vel(0, 0);
                moving = false;

                if(field[(int)octanom.Get_Next().Y] == 2) {
                    bool splash = true;
                    foreach(Entity e in lilypad) {
                        if(e.Get_Pos() == octanom.Get_Pos()) {
                            splash = false;
                            octanom.Set_Vel(e.Get_Vel().X, 0);
                            if(temp_movement == "null") {
                                if(e.Get_Vel().X > 0) octanom.Set_Next(new Vector2(octanom.Get_Next().X + 1, octanom.Get_Next().Y));
                                if(e.Get_Vel().X < 0) octanom.Set_Next(new Vector2(octanom.Get_Next().X - 1, octanom.Get_Next().Y));
                            }
                        }
                    }
                    if(splash) {
                        octanom.Set_HP(0);
                        JK.Noise("Explosion");
                        explosions.Add(new Entity(0, octanom.Get_Pos(), new Vector2(0, 0)));
                    }
                }

                if(octanom.Get_Next().Y < 2) {
                    octanom.Set_Next(new Vector2(octanom.Get_Next().X, 22));
                    octanom.Set_Pos(octanom.Get_Pos().X, 22 * 32);
                    map++;
                    Build_Field();
                }

                bool move = true;
                if(temp_movement == "Up") {
                    if(field[(int)octanom.Get_Next().Y - 1] == 2 && counter != 0) { move = false; } else { moving = true; }
                } else if(temp_movement == "Down") {
                    if(field[(int)octanom.Get_Next().Y + 1] == 2 && counter != 0) { move = false; } else { moving = true; }
                } else if(temp_movement == "Left" || temp_movement == "Right") {
                    if(field[(int)octanom.Get_Next().Y] == 2 && counter != 0) { move = false; } else { moving = true; }
                }
                if(move) {

                    if(temp_movement == "Up") {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, octanom.Get_Next().Y - 1));
                        octanom.Set_Vel(0, -2);
                    } else
                    if(temp_movement == "Down") {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X, octanom.Get_Next().Y + 1));
                        octanom.Set_Vel(0, 2);
                    } else
                    if(temp_movement == "Left") {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X - 1, octanom.Get_Next().Y));
                        octanom.Set_Vel(-2, 0);
                    } else
                    if(temp_movement == "Right") {
                        octanom.Set_Next(new Vector2(octanom.Get_Next().X + 1, octanom.Get_Next().Y));
                        octanom.Set_Vel(2, 0);
                    }
                    temp_movement = "null";
                }
            }
        }
        
    }
}
