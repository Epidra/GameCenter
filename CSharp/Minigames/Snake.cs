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
    class Snake : Ghost {

        Entity octanom_head;
        List<Entity>  octanom_tail = new List<Entity> ();

        string temp_movement; // Player Input

        int pointer;

        Vector2[] point = new Vector2[5];

        bool active_move_tail;

        bool won;

        public Snake(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            active_move_tail = true;
            pointer = -1;
            temp_movement = "empty";
            octanom_head = new Entity(1, new Vector2(32 * 15, 32 * 15), new Vector2(15, 15));
            point[0] = new Vector2(-1, -1);
            point[1] = new Vector2(-1, -1);
            point[2] = new Vector2(-1, -1);
            point[3] = new Vector2(-1, -1);
            point[4] = new Vector2(-1, -1);
            Command_Spawn_Point();
            octanom_tail.RemoveRange(0, octanom_tail.Count);
            won = false;
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed)    { pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { if(SK.orientation <= 2) { temp_movement = "Up";    } else { temp_movement = "Right"; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { if(SK.orientation <= 2) { temp_movement = "Down";  } else { temp_movement = "Left";  } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { if(SK.orientation <= 2) { temp_movement = "Left";  } else { temp_movement = "Up";    } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { if(SK.orientation <= 2) { temp_movement = "Right"; } else { temp_movement = "Down";  } pressed_response = true; }
            return "void";
        }

        public override string Update3(GameTime gameTime) {
            if(won && !active_gameover) {
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }

            if(!active_pause && !active_gameover) {
                octanom_head.Update();
                if(active_move_tail) {
                    foreach(Entity tail in octanom_tail) { tail.Update(); }
                }
                Command_Move(gameTime);
                Command_Collision();
            }
            return "void";
        }

        private Vector2 Get_CoinPos(int i) {
            if(SK.orientation <= 2) { return new Vector2(32 * point[i].X,            32 * point[i].Y);  }
                               else { return new Vector2(32 * point[i].Y, (27 * 32 - 32 * point[i].X)); }
        }

        public override void Draw2() {
            //if(SK.orientation <= 2) {
            //    spriteBatch.Draw(SK.texture_background_grid32, SK.Position_DisplayEdge() + SK.Position_BackgroundGrid() + new Vector2(32, 0), Color.White);
            //} else {
            //    spriteBatch.Draw(SK.texture_background_grid32, SK.Position_DisplayEdge() + SK.Position_BackgroundGrid() + new Vector2(0, 32), Color.White);
            //}

            spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_CoinPos(0), new Rectangle(0, 0, 32, 32), new Color(255, 255, 225), 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_CoinPos(1), new Rectangle(0, 0, 32, 32), new Color(255, 225, 225), 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_CoinPos(2), new Rectangle(0, 0, 32, 32), new Color(225, 255, 225), 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_CoinPos(3), new Rectangle(0, 0, 32, 32), new Color(225, 225, 255), 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            spriteBatch.Draw(SK.texture_spritesheet_coin, SK.Position_DisplayEdge() + SK.Position_Grid32() + Get_CoinPos(4), new Rectangle(0, 0, 32, 32), new Color(255, 255, 255), 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            spriteBatch.Draw(SK.texture_spritesheet_octanom_head, SK.Position_DisplayEdge() + SK.Position_Grid32() + octanom_head.Get_GridPos(SK.orientation), new Rectangle(0, octanom_head.Get_LookDirection(SK.orientation) * 32, 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);

            foreach(Entity tail in octanom_tail) {
                spriteBatch.Draw(SK.texture_spritesheet_octanom_tail, SK.Position_DisplayEdge() + SK.Position_Grid32() + tail.Get_GridPos(SK.orientation), new Rectangle(0, tail.Get_LookDirection(SK.orientation) * 32, 32, 32), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
            }

        }

        private float Speed() { return (game_difficulty * 1.5f) + ((float)score_points / 5000); }

        private void Command_Move(GameTime gameTime) {
            if(octanom_head.Get_Pos() == octanom_head.Get_Next() * 32) {
                Vector2 position = octanom_head.Get_Pos();
                if(!active_move_tail) {
                    active_move_tail = true;
                }
                // current moving direction
                string temp = "null";
                // Cancels out moving in opposite direction (and running into own tail)
                if(octanom_head.Get_Vel().Y < 0) { temp = "Up";    if(temp_movement == "Down")  temp_movement = "null"; }
                if(octanom_head.Get_Vel().Y > 0) { temp = "Down";  if(temp_movement == "Up")    temp_movement = "null"; }
                if(octanom_head.Get_Vel().X < 0) { temp = "Left";  if(temp_movement == "Right") temp_movement = "null"; }
                if(octanom_head.Get_Vel().X > 0) { temp = "Right"; if(temp_movement == "Left")  temp_movement = "null"; }

                // Updates Velocity
                if(temp_movement == "Up")    { octanom_head.Set_Vel(       0, -Speed()); }
                if(temp_movement == "Down")  { octanom_head.Set_Vel(       0,  Speed()); }
                if(temp_movement == "Left")  { octanom_head.Set_Vel(-Speed(),        0); }
                if(temp_movement == "Right") { octanom_head.Set_Vel( Speed(),        0); }

                // Updates Next()
                     if(temp_movement == "Up"    && octanom_head.Get_Next().Y !=  0) { octanom_head.Set_Motion(       0, -Speed()); temp_movement = "null"; }
                else if(temp_movement == "Up"    && octanom_head.Get_Next().Y ==  0) { octanom_head.Set_Motion(       0,        0); temp_movement = "null"; }
                else if(temp_movement == "Down"  && octanom_head.Get_Next().Y != 22) { octanom_head.Set_Motion(       0,  Speed()); temp_movement = "null"; }
                else if(temp_movement == "Down"  && octanom_head.Get_Next().Y == 22) { octanom_head.Set_Motion(       0,        0); temp_movement = "null"; }
                else if(temp_movement == "Left"  && octanom_head.Get_Next().X !=  0) { octanom_head.Set_Motion(-Speed(),        0); temp_movement = "null"; }
                else if(temp_movement == "Left"  && octanom_head.Get_Next().X ==  0) { octanom_head.Set_Motion(       0,        0); temp_movement = "null"; }
                else if(temp_movement == "Right" && octanom_head.Get_Next().X != 27) { octanom_head.Set_Motion( Speed(),        0); temp_movement = "null"; }
                else if(temp_movement == "Right" && octanom_head.Get_Next().X == 27) { octanom_head.Set_Motion(       0,        0); temp_movement = "null"; }
                else if(temp == "Up"    && octanom_head.Get_Next().Y !=  0) {                    octanom_head.Set_Next  (octanom_head.Get_Next() + new Vector2( 0, -1)); }
                else if(temp == "Up"    && octanom_head.Get_Next().Y ==  0) { JK.Noise("Place"); octanom_head.Set_Motion(                                       0,  0);  }
                else if(temp == "Down"  && octanom_head.Get_Next().Y != 22) {                    octanom_head.Set_Next  (octanom_head.Get_Next() + new Vector2( 0,  1)); }
                else if(temp == "Down"  && octanom_head.Get_Next().Y == 22) { JK.Noise("Place"); octanom_head.Set_Motion(                                       0,  0);  }
                else if(temp == "Left"  && octanom_head.Get_Next().X !=  0) {                    octanom_head.Set_Next  (octanom_head.Get_Next() + new Vector2(-1,  0)); }
                else if(temp == "Left"  && octanom_head.Get_Next().X ==  0) { JK.Noise("Place"); octanom_head.Set_Motion(                                       0,  0);  }
                else if(temp == "Right" && octanom_head.Get_Next().X != 27) {                    octanom_head.Set_Next  (octanom_head.Get_Next() + new Vector2( 1,  0)); }
                else if(temp == "Right" && octanom_head.Get_Next().X == 27) { JK.Noise("Place"); octanom_head.Set_Motion(                                       0,  0);  }
                foreach(Entity tail in octanom_tail) {
                         if(position.X > tail.Get_Pos().X) { tail.Set_Pos(position.X - 32, position.Y     ); tail.Set_Vel( Speed(),        0); }
                    else if(position.X < tail.Get_Pos().X) { tail.Set_Pos(position.X + 32, position.Y     ); tail.Set_Vel(-Speed(),        0); }
                    else if(position.Y > tail.Get_Pos().Y) { tail.Set_Pos(position.X     , position.Y - 32); tail.Set_Vel(       0,  Speed()); }
                    else if(position.Y < tail.Get_Pos().Y) { tail.Set_Pos(position.X     , position.Y + 32); tail.Set_Vel(       0, -Speed()); }
                    position = tail.Get_Pos();
                }
            }
            if(octanom_head.Get_Vel() == new Vector2(0, 0)) {
                active_move_tail = false;
            }
        }

        private void Command_Collision() {
            for(int i = 0; i < 5; i++) {
                if(octanom_head.Get_GridPos() == 32 * point[i]) {
                    JK.Noise("Coin");
                    score_points += 50 * game_difficulty;
                    pointer = i;
                    Command_Spawn_Point();
                    Command_Spawn_Tail();
                }
            }
            if(octanom_tail.Count > 1) {
                foreach(Entity tail in octanom_tail) {
                    if(octanom_head.Get_GridPos().X == tail.Get_GridPos().X && octanom_head.Get_GridPos().Y - 16 < tail.Get_GridPos().Y && tail.Get_GridPos().Y < octanom_head.Get_GridPos().Y + 16) {
                        JK.Noise("Place");
                        won = true;
                    }
                }
            }
        }

        private void Command_Spawn_Point() {
            bool temp_break = false;
            int x = 0;
            int y = 0;
            for(int i = 0; i < 5; i++) {
                if(pointer == i || (pointer == -1 && i < FM.octaLives)) {
                    int b = 0;
                    temp_break = false;
                    while(!temp_break) {
                        b++;
                        bool temp_internal = false;
                        x = random.Next(25) + 1;
                        y = random.Next(20) + 1;
                        if(b < 10) {
                            if(octanom_head.Get_Next() != new Vector2(x, y)) {
                                foreach(Entity tail in octanom_tail) {
                                    if(tail.Get_GridPos() == new Vector2(x, y) * 32) {
                                        temp_internal = true;
                                        break;
                                    }
                                }
                                for(int j = 0; j < 5; j++) {
                                    if(point[j] == new Vector2(x, y))
                                        temp_internal = true;
                                }
                                if(!temp_internal) {
                                    temp_break = true;
                                }
                            }
                        } else {
                            if(octanom_head.Get_Next() != new Vector2(x, y)) {
                                temp_break = true;
                            }
                        }
                        point[i] = new Vector2(x, y);
                    }
                }
            }
        }

        private void Command_Spawn_Tail() {
            Vector2 pos = octanom_head.Get_GridPos();
            int i = 0;
            foreach(Entity tail in octanom_tail) {
                if(i + 1 == octanom_tail.Count) {
                    pos = tail.Get_GridPos();
                }
                i++;
            }
            octanom_tail.Add(new Entity(1, pos, new Vector2(0, 0)));
        }
    }
}
