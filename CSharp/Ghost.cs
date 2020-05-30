using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;

namespace GameCenter {
    class Ghost {

        // missing stuff
        public bool active_pause;
        public bool active_gameover;
        public bool active_highscore;

        public int game_difficulty;
        public int game_level;

        public int score_points;
        public int score_gold;
        public int score_lives;
        public int score_level;
        public int coins_old;
        public int coins_plus;

        public double time_started;

        /// <summary>
        /// The Screen's personal ID
        /// Must be unique
        /// </summary>
        public string id;
        /// <summary> The Content Manager </summary>
        public ContentManager CM;
        /// <summary> The Shop Keeper </summary>
        public ShopKeeper SK;
        /// <summary> The File Manager </summary>
        public FileManager FM;
        /// <summary> The Juke Box </summary>
        public JukeBox JK;
        /// <summary> The Screen's internal Width </summary>
        public int screensize_width;
        /// <summary> The Screen's internal Height </summary>
        public int screensize_height;
        /// <summary> The percentual Difference between the internal and actual Screen Width </summary>
        public float screensize_width_scale;
        /// <summary> The percentual Difference between the internal and actual Screen Height </summary>
        public float screensize_height_scale;
        /// <summary> The Standard Random Number Generator </summary>
        public Random random;
        /// <summary> The Keyboard Control System (current Cycle) </summary>
        public KeyboardState control_keyboard_new;
        /// <summary> The Keyboard Control System (previous Cycle) </summary>
        public KeyboardState control_keyboard_old;
        /// <summary> The Mouse Control System (Current Cycle) </summary>
        public MouseState control_mouse_new;
        /// <summary> The Mouse Control System (Previous Cycle) </summary>
        public MouseState control_mouse_old;
        /// <summary> The Touch Control System </summary>
        public TouchCollection control_touch;
        /// <summary> The GamePad Control System (current Cycle) </summary>
        public GamePadState control_gamepad_P1_new;
        public GamePadState control_gamepad_P2_new;
        /// <summary> The GamePad Control System (previous Cycle) </summary>
        public GamePadState control_gamepad_P1_old;
        public GamePadState control_gamepad_P2_old;
        /// <summary>
        /// The Current Position of the Mouse Cursor
        /// Gets updated on Mouse Clicks, Touches, and Mouse Movements
        /// Is already scaled to Screen Resolution
        /// </summary>
        public Vector2 control_cursor;
        /// <summary> The Graphics Device </summary>
        public GraphicsDevice graphicsDevice;
        /// <summary> The Sprite Batch </summary>
        public SpriteBatch spriteBatch;
        /// <summary> A workaround responsive call to let system better handle single Mouse Clicks </summary>
        public bool pressed_response;
        /// <summary> Handles all incoming calls from UP Arrows </summary>
        //public bool pressed_arrow_up;
        /// <summary> Handles all incoming calls from DOWN Arrows </summary>
        //public bool pressed_arrow_down;
        /// <summary> Handles all incoming calls from LEFT Arrows </summary>
        //public bool pressed_arrow_left;
        /// <summary> Handles all incoming calls from RIGHT Arrows </summary>
        //public bool pressed_arrow_right;
        /// <summary>
        /// Handles all incoming calls from OK Buttons
        /// See: A-Button, SPACE
        /// </summary>
        //public bool pressed_button_OK;
        /// <summary>
        /// Handles all incoming calls from CANCEL Buttons
        /// See: B-Button, BackSpace
        /// </summary>
        //public bool pressed_button_right;
        /// <summary>
        /// Handles all incoming calls from Special Buttons
        /// See: X-Button, CTRL
        /// </summary>
        //public bool pressed_button_left;
        /// <summary>
        /// Handles all incoming calls from Menu Buttons
        /// See: Y-Button, TAB
        /// </summary>
        //public bool pressed_button_menu;
        /// <summary>
        /// Handles all incoming calls from Start and Pause Buttons
        /// See: START-Button, ENTER
        /// </summary>
        //public bool pressed_button_start;
        /// <summary>
        /// Handles all incoming calls from Back and Exit Buttons
        /// See: BACK-Button, ESC
        /// </summary>
        //public bool pressed_button_exit;
        /// <summary>
        /// Handles all incoming calls from Mouse Clicks and Touches
        /// Needs special call for response
        /// </summary>
        public bool pressed_event_touch;
        /// <summary>
        /// Used for the Digital Pad
        /// </summary>
        public Color[] TextureData;

        public int frame_index;   // curent index for draw
        public int frame_max;     // when index goes back to 0
        public int frame_counter; // internal counter
        public int frame_stopper; // when counter changes index
        public bool frame_switch; // for turning back frame

        public Ghost(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) {
            id = _id;
            CM = _content;
            SK = _Shopkeeper;
            FM = _Filemanager;
            JK = _Jukebox;
            screensize_width = (int)SK.screensize_main.X;
            screensize_height = (int)SK.screensize_main.Y;
            screensize_width_scale = (screenX / (float)screensize_width);
            screensize_height_scale = (screenY / (float)screensize_height);
            random = new Random();
        }

        public void Resize(float screenX, float screenY) {
            screensize_width_scale = screenX;
            screensize_height_scale = screenY;
        }

        public void Load_Content(GraphicsDevice _graphicsdevice, SpriteBatch _spritebatch) {
            graphicsDevice = _graphicsdevice;
            spriteBatch = _spritebatch;
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.Pinch | GestureType.PinchComplete;
            TextureData = new Color[(int)(SK.texture_colormap.Width) * (int)(SK.texture_colormap.Height)];
            SK.texture_colormap.GetData(TextureData);
            pressed_response = false;
            frame_max = SK.frame_max;
            frame_stopper = SK.frame_stopper;
            frame_counter = 0;
            frame_index = 0;
            frame_switch = false;
            Load_Content2();
        }

        public void Unload_Content() {
            Unload_Content2();
        }

        public void Start(GameTime gameTime, int difficulty, int levelselect) {
            game_difficulty = difficulty;
            game_level = levelselect;
            score_points = 0;
            score_gold = 0;
            score_level = 0;
            score_lives = 0;
            coins_old = FM.coins;
            coins_plus = 0;
            active_gameover = false;
            active_highscore = false;
            active_pause = false;
            time_started = gameTime.TotalGameTime.TotalSeconds;
            Start2();
        }

        public string Update(GameTime gameTime) {
            control_keyboard_new = Keyboard.GetState();
            control_mouse_new = Mouse.GetState();
            control_touch = TouchPanel.GetState();
            control_gamepad_P1_new = GamePad.GetState(PlayerIndex.One);
            control_gamepad_P2_new = GamePad.GetState(PlayerIndex.Two);

            // Reset Keys
            pressed_event_touch = false;

            // Resize Buttons (Mousewheel)
            if(control_mouse_new.ScrollWheelValue < control_mouse_old.ScrollWheelValue) { if(FM.button_scale > 100) FM.button_scale = FM.button_scale - 2; }
            if(control_mouse_new.ScrollWheelValue > control_mouse_old.ScrollWheelValue) { if(FM.button_scale < 500) FM.button_scale = FM.button_scale + 2; }

            // Move Mouse
            if(control_mouse_new.Position != control_mouse_old.Position) {
                control_cursor = new Vector2(control_mouse_new.Position.X / screensize_width_scale, control_mouse_new.Position.Y / screensize_height_scale);
            }

            // Press Mouse Button
            if(control_mouse_new.LeftButton == ButtonState.Pressed && !pressed_response) {
                control_cursor = new Vector2(control_mouse_new.Position.X / screensize_width_scale, control_mouse_new.Position.Y / screensize_height_scale);
                pressed_event_touch = true;
            }

            // Detected Touch Input
            while(TouchPanel.IsGestureAvailable) {
                var gesture = TouchPanel.ReadGesture();
                switch(gesture.GestureType) {
                    // Resize Buttons (Touch Gesture)
                    case GestureType.Pinch:
                        Vector2 oldPosition  = gesture.Position  - gesture.Delta;
                        Vector2 oldPosition2 = gesture.Position2 - gesture.Delta2;
                        float newDistance = Vector2.Distance(gesture.Position, gesture.Position2);
                        float oldDistance = Vector2.Distance(     oldPosition,      oldPosition2);
                        float scaleFactor = newDistance / oldDistance;
                        if(scaleFactor < 1.0F) { if(FM.button_scale > 100) FM.button_scale = FM.button_scale - 2; }
                        if(scaleFactor > 1.0F) { if(FM.button_scale < 500) FM.button_scale = FM.button_scale + 2; }
                        break;
                    case GestureType.PinchComplete:
                        break;
                }
            }

            if(control_touch.Count > 0 && !pressed_response) {
                foreach(TouchLocation l in control_touch) {
                    control_cursor = new Vector2(l.Position.X / screensize_width_scale, l.Position.Y / screensize_height_scale);
                    pressed_event_touch = true;
                }
            }

            if(control_touch.Count == 0 && control_mouse_new.LeftButton == ButtonState.Released && control_mouse_old.LeftButton == ButtonState.Released) {
                pressed_response = false;
            }

            string update = "void";
            if(id != "menu") {
                if(!FM.active_transition && FM.active_info) {
                    FM.speed = true;
                    FM.active_transition = true;
                    return "menu";
                } else if(active_gameover) {
                    if(ButtonPressed(GhostKey.button_function_P1)  == GhostState.pressed) { FM.speed = true; FM.active_transition = true; pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_exit)       == GhostState.pressed) { FM.speed = true; FM.active_transition = true; pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed) { Start(gameTime, game_difficulty, game_level); pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_start)      == GhostState.pressed) { Start(gameTime, game_difficulty, game_level); pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_ok_P1)      == GhostState.pressed) { Start(gameTime, game_difficulty, game_level); pressed_response = true; }
                } else if(active_pause) {
                    if(ButtonPressed(GhostKey.button_function_P1)  == GhostState.pressed) { FM.speed = true; FM.active_transition = true; pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_exit)       == GhostState.pressed) { FM.speed = true; FM.active_transition = true; pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed) { if(active_pause) { active_pause = false; } else { active_pause = true; } pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_start)      == GhostState.pressed) { if(active_pause) { active_pause = false; } else { active_pause = true; } pressed_response = true; }
                } else if(FM.active_transition) {

                } else {
                    if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed) { if(active_pause) { active_pause = false; } else {
                            active_pause = true; } pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_exit)       == GhostState.pressed) { if(active_pause) { active_pause = false; } else {
                            active_pause = true; } pressed_response = true; }
                    if(ButtonPressed(GhostKey.button_start)      == GhostState.pressed) { if(active_pause) { active_pause = false; } else {
                            active_pause = true; } pressed_response = true; }
                    update = Update2();
                }
            }

            update = Update3(gameTime);
            control_keyboard_old = control_keyboard_new;
            control_mouse_old = control_mouse_new;
            control_gamepad_P1_old = control_gamepad_P1_new;
            control_gamepad_P2_old = control_gamepad_P2_new;
            return update;
        }

        public void GameOver(double time) {
            if(!active_gameover) {
                if(FM.type[FM.Convert(id)] != 4) {
                    coins_plus = (int)(score_points * FM.coincalc[FM.Convert(id)]);
                    coins_plus += score_gold * 5;
                } else {
                    score_points = coins_plus;
                }
                FM.coins += coins_plus;
                FM.coins_bonus += coins_plus;
                if(score_points > FM.highscore[FM.Convert(id)]) {
                    FM.highscore[FM.Convert(id)] = score_points;
                    active_highscore = true;
                }
                if(coins_plus > FM.highcoin[FM.Convert(id)]) {
                    FM.highcoin[FM.Convert(id)] = coins_plus;
                }
                FM.hightotal[FM.Convert(id)] += coins_plus;
                FM.hightimeI[FM.Convert(id)] += (int)(time - time_started);
                FM.highplay[FM.Convert(id)]++;
                FM.Save();
                active_gameover = true;
            }
        }

        public void Draw() {
            frame_counter++;
            if(frame_counter >= frame_stopper) {
                frame_counter = 0;
                if(!frame_switch) { frame_index++; } else { frame_index--; }
                if(frame_index == frame_max - 1) {
                    frame_switch = true;
                }
                if(frame_index == 0) {
                    frame_switch = false;
                }
            }
            spriteBatch.Begin();
            graphicsDevice.Clear(Color.Transparent);

            if(SK.orientation <= 2) {
                spriteBatch.Draw(SK.Get_Background(FM.current_background), new Vector2(SK.Position_DisplayEdge().X - 10 - FM.rolling_horizontal, SK.Position_DisplayEdge().Y - 160), Color.White);
                spriteBatch.Draw(SK.Get_Background(FM.current_background), new Vector2(SK.Position_DisplayEdge().X - 10 + 900 - FM.rolling_horizontal, SK.Position_DisplayEdge().Y - 160), Color.White);
            } else {
                spriteBatch.Draw(SK.Get_Background(FM.current_background), new Vector2(SK.Position_DisplayEdge().X - 10 - FM.rolling_horizontal, SK.Position_DisplayEdge().Y - 30), Color.White);
                spriteBatch.Draw(SK.Get_Background(FM.current_background), new Vector2(SK.Position_DisplayEdge().X - 10 + 900 - FM.rolling_horizontal, SK.Position_DisplayEdge().Y - 30), Color.White);
            }

            Draw2();

            if(id != "menu" && FM.type[FM.Convert(id)] != 4) {

                if(score_points != 0) {
                    if(score_points > FM.highscore[FM.Convert(id)]) {
                        spriteBatch.DrawString(SK.font_score, "" + score_points, new Vector2(SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("" + score_points).Length() / 2, SK.Position_DisplayEdge().Y + 35), Color.Gold);
                    } else {
                        spriteBatch.DrawString(SK.font_score, "" + score_points, new Vector2(SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("" + score_points).Length() / 2, SK.Position_DisplayEdge().Y + 35), Color.White);
                    }
                }
                if(score_lives != 0) {
                    spriteBatch.DrawString(SK.font_score, "" + score_lives, new Vector2(SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 4 - SK.font_score.MeasureString("" + score_lives).Length() / 2, SK.Position_DisplayEdge().Y + 35), Color.White);
                }

                if(score_level != 0) {
                    spriteBatch.DrawString(SK.font_score, "" + score_level, new Vector2(SK.Position_DisplayEdge().X + SK.Get_GridSize().X * 3 / 4 - SK.font_score.MeasureString("" + score_level).Length() / 2, SK.Position_DisplayEdge().Y + 35), Color.White);
                }
            }

            if(active_pause) {
                if(SK.orientation <= 2) {
                    spriteBatch.Draw(SK.texture_background_pause, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_background_pause, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.White);
                }
            }

            if(active_gameover && FM.type[FM.Convert(id)] != 4) {
                if(SK.orientation <= 2) { spriteBatch.Draw(SK.texture_background_gameover, new Vector2(SK.Position_DisplayEdge().X, SK.Position_DisplayEdge().Y - 80), Color.White); } else { spriteBatch.Draw(SK.texture_background_gameover, new Vector2(SK.Position_DisplayEdge().X - 80, SK.Position_DisplayEdge().Y), Color.White); }
                if(score_points > 0) {
                    spriteBatch.DrawString(SK.font_score, "Score:", SK.Position_DisplayEdge() + new Vector2(-200 + SK.Get_GridSize().X / 2, 300), Color.Black);
                    spriteBatch.DrawString(SK.font_score, "" + score_points, SK.Position_DisplayEdge() + new Vector2(+200 + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("" + score_points).Length(), 300), Color.Black);
                    spriteBatch.DrawString(SK.font_score, "Current Highscore:", SK.Position_DisplayEdge() + new Vector2(-200 + SK.Get_GridSize().X / 2, 350), Color.Black);
                    spriteBatch.DrawString(SK.font_score, "" + FM.highscore[FM.Convert(id)], SK.Position_DisplayEdge() + new Vector2(+200 + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("" + FM.highscore[FM.Convert(id)]).Length(), 350), Color.Black);
                    if(active_highscore) {
                        spriteBatch.DrawString(SK.font_score, "New Highscore!", SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("New Highscore!").Length() / 2, 400), Color.DarkBlue);
                    }
                }
                if(coins_plus != 0) {
                    spriteBatch.DrawString(SK.font_score, coins_old + " C", SK.Position_DisplayEdge() + new Vector2(+200 + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString(coins_old + " C").Length(), 500), Color.Yellow);
                    spriteBatch.DrawString(SK.font_score, "+" + coins_plus + " C", SK.Position_DisplayEdge() + new Vector2(+200 + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString("+" + coins_plus + " C").Length(), 540), Color.Green);
                    spriteBatch.DrawString(SK.font_score, FM.coins + " C", SK.Position_DisplayEdge() + new Vector2(+200 + SK.Get_GridSize().X / 2 - SK.font_score.MeasureString(FM.coins + " C").Length(), 580), Color.Yellow);
                }
            }

            if(id != "menu") {
                float t = 0.00f; t = (float)FM.transition / 100;
                spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10 - FM.rolling_vertical), Color.White * t);
                spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10 + 900 - FM.rolling_vertical), Color.White * t);
                spriteBatch.Draw(SK.texture_menu_info_board, new Vector2(SK.Position_DisplayEdge().X - 100 + FM.transition, SK.Position_DisplayEdge().Y - 10), null, Color.White * t, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                spriteBatch.Draw(SK.texture_menu_info_board, new Vector2(SK.Position_DisplayEdge().X - 100 + FM.transition, SK.Position_DisplayEdge().Y - 10 + 450), null, Color.White * t, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipVertically, 0);
                if(SK.orientation <= 2) {
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(0, -50 + FM.transition / 2), Color.White);
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(0, 704 + 50 - FM.transition / 2), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(-80, -50 + FM.transition / 2), Color.White);
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(-80, 864 + 50 - FM.transition / 2), Color.White);
                }
            }


            switch(SK.orientation) {
                case 1: spriteBatch.Draw(SK.texture_background_dropshadow, SK.Position_DisplayEdge(), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0); break;
                case 2: spriteBatch.Draw(SK.texture_background_dropshadow, SK.Position_DisplayEdge(), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0); break;
                case 3: spriteBatch.Draw(SK.texture_background_dropshadow, SK.Position_DisplayEdge(), null, Color.White, -1.5708f, new Vector2(864, 0), 1f, SpriteEffects.None, 0); break;
                case 4: spriteBatch.Draw(SK.texture_background_dropshadow, SK.Position_DisplayEdge(), null, Color.White, -1.5708f, new Vector2(864, 0), 1f, SpriteEffects.FlipVertically, 0); break;
            }

            if(!FM.system_alignededge) {
                switch(SK.orientation) {
                    case 1: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(-100, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0); break;
                    case 2: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0); break;
                    case 3: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 100), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.None, 0); break;
                    case 4: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 100), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.FlipVertically, 0); break;
                }
            } else {
                switch(SK.orientation) {
                    case 1: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0); break;
                    case 2: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(-100, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0); break;
                    case 3: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 0), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.None, 0); break;
                    case 4: spriteBatch.Draw(SK.texture_background_casing1, new Vector2(0, 0), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.FlipVertically, 0); break;
                }
            }

            if(!FM.system_alignededge) {
                switch(SK.orientation) {
                    case 1: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(-100, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0); break;
                    case 2: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0); break;
                    case 3: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 100), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.None, 0); break;
                    case 4: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 100), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.FlipVertically, 0); break;
                }
            } else {
                switch(SK.orientation) {
                    case 1: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0); break;
                    case 2: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(-100, 0), null, Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipHorizontally, 0); break;
                    case 3: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 0), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.None, 0); break;
                    case 4: spriteBatch.Draw(SK.texture_background_casing2, new Vector2(0, 0), null, Color.White, -1.5708f, new Vector2(1280, 0), 1f, SpriteEffects.FlipVertically, 0); break;
                }
            }

            spriteBatch.Draw(SK.texture_hud_button_pad, new Rectangle(SK.Position_Pad(), new Point(FM.button_scale)), Color.White);
            spriteBatch.Draw(SK.texture_hud_button_empty_left, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
            spriteBatch.Draw(SK.texture_hud_button_empty_right, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);

            if(id != "menu") {
                if(active_gameover) {
                    spriteBatch.Draw(SK.texture_hud_button_back, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_replay, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                } else if(active_pause) {
                    spriteBatch.Draw(SK.texture_hud_button_back, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_resume, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_hud_button_pause, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                }
            }

            Draw3();

            if(FM.highlight_timer > 0) {
                FM.highlight_timer -= 0.05f;
                switch(FM.highlight_button) {
                    case 0: spriteBatch.Draw(SK.texture_highlight_button_left,  new Rectangle(SK.Position_Button( true), new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                    case 1: spriteBatch.Draw(SK.texture_highlight_button_right, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                    case 2: spriteBatch.Draw(SK.texture_highlight_arrow_up,     new Rectangle(SK.Position_Pad(),         new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                    case 3: spriteBatch.Draw(SK.texture_highlight_arrow_left,   new Rectangle(SK.Position_Pad(),         new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                    case 4: spriteBatch.Draw(SK.texture_highlight_arrow_right,  new Rectangle(SK.Position_Pad(),         new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                    case 5: spriteBatch.Draw(SK.texture_highlight_arrow_down,   new Rectangle(SK.Position_Pad(),         new Point(FM.button_scale)), Color.White * FM.highlight_timer); break;
                }
            }

            if(FM.coins_bonus != 0) {
                if(FM.coins_bonus > 0) {
                    if(FM.coins_bonus < 10) {
                        FM.coins_bonus = 0;
                    } else {
                        FM.coins_bonus -= 10;
                    }
                }
                if(FM.coins_bonus < 0) {
                    if(FM.coins_bonus > -10) {
                        FM.coins_bonus = 0;
                    } else {
                        FM.coins_bonus += 10;
                    }
                }
            }

            FM.rolling_horizontal++; if(FM.rolling_horizontal >= 900) FM.rolling_horizontal = 0;
            FM.rolling_vertical++; if(FM.rolling_vertical >= 900) FM.rolling_vertical = 0;

            spriteBatch.End();
            graphicsDevice.SetRenderTarget(null);
        }

        public virtual void Start2() { }
        public virtual void Load_Content2() { }
        public virtual void Unload_Content2() { }
        public virtual string Update2() { return "void"; }
        public virtual string Update3(GameTime gameTime) { return "void"; }
        public virtual void Draw2() { }
        public virtual void Draw3() { }

        public GhostState ButtonPressed(GhostKey id) {
            if(pressed_event_touch) {
                if(Collision_Button(true, new Rectangle((int)SK.Position_Pad().X, (int)SK.Position_Pad().Y, FM.button_scale, FM.button_scale))) {
                    int x = (int)control_cursor.X + (500-FM.button_scale)/2 - SK.Position_Pad().X;
                    int y = (int)control_cursor.Y + (500-FM.button_scale)/2 - SK.Position_Pad().Y;
                    Color temp = TextureData[500*y  + x];
                    if(temp == new Color(new Vector4(255, 0, 0, 255))) { // UP
                        FM.Set_Hightlight(2);
                        if(id == GhostKey.arrow_up_P1) {
                            if(!pressed_response) {
                                pressed_response = true;
                                return GhostState.pressed;
                            } else {
                                return GhostState.hold;
                            }
                        }
                        
                    } else if(temp == new Color(new Vector4(0, 255, 0, 255))) { // DOWN
                        FM.Set_Hightlight(5);
                        if(id == GhostKey.arrow_down_P1) {
                            if(!pressed_response) {
                                pressed_response = true;
                                return GhostState.pressed;
                            } else {
                                return GhostState.hold;
                            }
                        }
                    } else if(temp == new Color(new Vector4(0,0,0,0))) { // LEFT
                        FM.Set_Hightlight(3);
                        if(id == GhostKey.arrow_left_P1) {
                            if(!pressed_response) {
                                pressed_response = true;
                                return GhostState.pressed;
                            } else {
                                return GhostState.hold;
                            }
                        }
                    } else if(temp == new Color(new Vector4(0, 255, 255, 255))) { // RIGHT
                        FM.Set_Hightlight(4);
                        if(id == GhostKey.arrow_right_P1) {
                            if(!pressed_response) {
                                pressed_response = true;
                                return GhostState.pressed;
                            } else {
                                return GhostState.hold;
                            }
                        }
                    }
                } else if(Collision_Button(true, new Rectangle((int)SK.Position_Button(true).X, (int)SK.Position_Button(true).Y, FM.button_scale / 2, FM.button_scale / 2))) {
                    FM.Set_Hightlight(0);
                    if(id == GhostKey.button_special_P1) {
                        if(!pressed_response) {
                            pressed_response = true;
                            return GhostState.pressed;
                        } else {
                            return GhostState.hold;
                        }
                    }
                } else if(Collision_Button(true, new Rectangle((int)SK.Position_Button(false).X + FM.button_scale / 2, (int)SK.Position_Button(false).Y, FM.button_scale / 2, FM.button_scale / 2))) {
                    FM.Set_Hightlight(1);
                    if(id == GhostKey.button_function_P1) {
                        if(!pressed_response) {
                            pressed_response = true;
                            return GhostState.pressed;
                        } else {
                            return GhostState.hold;
                        }
                    }
                } else if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X, (int)SK.Position_DisplayEdge().Y, (int)SK.Get_GridSize().X, (int)SK.Get_GridSize().Y))) {
                    pressed_event_touch = true;
                    if(id == GhostKey.button_ok_P1) {
                        if(!pressed_response) {
                            pressed_response = true;
                            return GhostState.pressed;
                        } else {
                            return GhostState.hold;
                        }
                    }
                }
            }
            switch(id) {
                // Arrows Player 1
                case GhostKey.arrow_up_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.Up) && control_keyboard_old.IsKeyUp(Keys.Up)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Up)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadUp) && control_gamepad_P1_old.IsButtonUp(Buttons.DPadUp)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadUp)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_down_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.Down) && control_keyboard_old.IsKeyUp(Keys.Down)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Down)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadDown) && control_gamepad_P1_old.IsButtonUp(Buttons.DPadDown)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadDown)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_left_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.Left) && control_keyboard_old.IsKeyUp(Keys.Left)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Left)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadLeft) && control_gamepad_P1_old.IsButtonUp(Buttons.DPadLeft)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadLeft)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_right_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.Right) && control_keyboard_old.IsKeyUp(Keys.Right)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Right)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadRight) && control_gamepad_P1_old.IsButtonUp(Buttons.DPadRight)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.DPadRight)) { return GhostState.hold; }
                    return GhostState.released;

                // Arrows Player 2
                case GhostKey.arrow_up_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Up) && control_keyboard_old.IsKeyUp(Keys.Up)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Up)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadUp) && control_gamepad_P2_old.IsButtonUp(Buttons.DPadUp)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadUp)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_down_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Down) && control_keyboard_old.IsKeyUp(Keys.Down)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Down)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadDown) && control_gamepad_P2_old.IsButtonUp(Buttons.DPadDown)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadDown)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_left_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.A) && control_keyboard_old.IsKeyUp(Keys.A)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.A)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadLeft) && control_gamepad_P2_old.IsButtonUp(Buttons.DPadLeft)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadLeft)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.arrow_right_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Right) && control_keyboard_old.IsKeyUp(Keys.Right)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Right)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadRight) && control_gamepad_P2_old.IsButtonUp(Buttons.DPadRight)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.DPadRight)) { return GhostState.hold; }
                    return GhostState.released;

                // Buttons Player 1
                case GhostKey.button_ok_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.Space) && control_keyboard_old.IsKeyUp(Keys.Space)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Space)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.A) && control_gamepad_P1_old.IsButtonUp(Buttons.A)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.A)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_function_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.RightControl) && control_keyboard_old.IsKeyUp(Keys.RightControl)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.RightControl)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.B) && control_gamepad_P1_old.IsButtonUp(Buttons.B)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.B)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_special_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.LeftControl) && control_keyboard_old.IsKeyUp(Keys.LeftControl)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.LeftControl)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.X) && control_gamepad_P1_old.IsButtonUp(Buttons.X)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.X)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_menu_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.RightShift) && control_keyboard_old.IsKeyUp(Keys.RightShift)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.RightShift)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.Y) && control_gamepad_P1_old.IsButtonUp(Buttons.Y)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.Y)) { return GhostState.hold; }
                    return GhostState.released;

                // Buttons Player 2
                case GhostKey.button_ok_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Tab) && control_keyboard_old.IsKeyUp(Keys.Tab)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Tab)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.A) && control_gamepad_P2_old.IsButtonUp(Buttons.A)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.A)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_function_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.E) && control_keyboard_old.IsKeyUp(Keys.E)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.E)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.B) && control_gamepad_P2_old.IsButtonUp(Buttons.B)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.B)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_special_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Q) && control_keyboard_old.IsKeyUp(Keys.Q)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Q)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.X) && control_gamepad_P2_old.IsButtonUp(Buttons.X)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.X)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_menu_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.R) && control_keyboard_old.IsKeyUp(Keys.R)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.R)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.Y) && control_gamepad_P2_old.IsButtonUp(Buttons.Y)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.Y)) { return GhostState.hold; }
                    return GhostState.released;

                // Trigger Player 1
                case GhostKey.button_left_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.N) && control_keyboard_old.IsKeyUp(Keys.N)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.N)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.LeftShoulder) && control_gamepad_P1_old.IsButtonUp(Buttons.LeftShoulder)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.LeftShoulder)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_right_P1:
                    if(control_keyboard_new.IsKeyDown(Keys.M) && control_keyboard_old.IsKeyUp(Keys.M)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.M)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.RightShoulder) && control_gamepad_P1_old.IsButtonUp(Buttons.RightShoulder)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.RightShoulder)) { return GhostState.hold; }
                    return GhostState.released;

                // Trigger Player 2
                case GhostKey.button_left_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.Y) && control_keyboard_old.IsKeyUp(Keys.Y)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Y)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.LeftShoulder) && control_gamepad_P2_old.IsButtonUp(Buttons.LeftShoulder)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.LeftShoulder)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_right_P2:
                    if(control_keyboard_new.IsKeyDown(Keys.X) && control_keyboard_old.IsKeyUp(Keys.X)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.X)) { return GhostState.hold; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.RightShoulder) && control_gamepad_P2_old.IsButtonUp(Buttons.RightShoulder)) { return GhostState.pressed; }
                    if(control_gamepad_P2_new.IsButtonDown(Buttons.RightShoulder)) { return GhostState.hold; }
                    return GhostState.released;

                // Special Buttons
                case GhostKey.button_start:
                    if(control_keyboard_new.IsKeyDown(Keys.Enter) && control_keyboard_old.IsKeyUp(Keys.Enter)) { return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Enter)) { return GhostState.hold; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.Start) && control_gamepad_P1_old.IsButtonUp(Buttons.Start)) { return GhostState.pressed; }
                    if(control_gamepad_P1_new.IsButtonDown(Buttons.Start)) { return GhostState.hold; }
                    return GhostState.released;

                case GhostKey.button_exit:
                    if(control_keyboard_new.IsKeyDown(Keys.Escape) && control_keyboard_old.IsKeyUp(Keys.Escape)) {
                        return GhostState.pressed; }
                    if(control_keyboard_new.IsKeyDown(Keys.Escape)) {
                        return GhostState.hold; }
                    //if(control_gamepad_P1_new.IsButtonDown(Buttons.Back) && control_gamepad_P1_old.IsButtonUp(Buttons.Back)) {
                    //    return GhostState.pressed; }
                    //if(control_gamepad_P1_new.IsButtonDown(Buttons.Back)) {
                    //    return GhostState.hold; }
                    return GhostState.released;
            }
            return 0;
        }

        public enum GhostKey {
            arrow_up_P1, arrow_down_P1, arrow_left_P1, arrow_right_P1,
            arrow_up_P2, arrow_down_P2, arrow_left_P2, arrow_right_P2,
            button_ok_P1, button_function_P1, button_special_P1, button_menu_P1,
            button_ok_P2, button_function_P2, button_special_P2, button_menu_P2,
            button_left_P1, button_right_P1, button_left_P2, button_right_P2,
            button_start, button_exit
        };

        public enum GhostState {
            released, pressed, hold
        };

        public bool Collision_Button(bool hover, Rectangle rect) {
            if(!hover) {
                if(rect.X < control_cursor.X && control_cursor.X < rect.X + rect.Width) {
                    if(rect.Y < control_cursor.Y && control_cursor.Y < rect.Y + rect.Height) {
                        pressed_response = true;
                        return true;
                    }
                }
            }
            if(hover) {
                if(rect.X < control_cursor.X && control_cursor.X < rect.X + rect.Width) {
                    if(rect.Y < control_cursor.Y && control_cursor.Y < rect.Y + rect.Height) {
                        return true;
                    }
                }
            }
            return false;
        }

        public Rectangle Get_Mino_Texture(string s, int type, int size) {
            if(s == "blank") if(FM.minos[0]) return new Rectangle(0 * size, type * size, size, size); else return new Rectangle(0 * size, type * size + 5 * size, size, size);
            if(s == "fire") if(FM.minos[1]) return new Rectangle(1 * size, type * size, size, size); else return new Rectangle(1 * size, type * size + 5 * size, size, size);
            if(s == "air") if(FM.minos[2]) return new Rectangle(2 * size, type * size, size, size); else return new Rectangle(2 * size, type * size + 5 * size, size, size);
            if(s == "thunder") if(FM.minos[3]) return new Rectangle(3 * size, type * size, size, size); else return new Rectangle(3 * size, type * size + 5 * size, size, size);
            if(s == "water") if(FM.minos[4]) return new Rectangle(4 * size, type * size, size, size); else return new Rectangle(4 * size, type * size + 5 * size, size, size);
            if(s == "ice") if(FM.minos[5]) return new Rectangle(5 * size, type * size, size, size); else return new Rectangle(5 * size, type * size + 5 * size, size, size);
            if(s == "earth") if(FM.minos[6]) return new Rectangle(6 * size, type * size, size, size); else return new Rectangle(6 * size, type * size + 5 * size, size, size);
            if(s == "metal") if(FM.minos[7]) return new Rectangle(7 * size, type * size, size, size); else return new Rectangle(7 * size, type * size + 5 * size, size, size);
            if(s == "nature") if(FM.minos[8]) return new Rectangle(8 * size, type * size, size, size); else return new Rectangle(8 * size, type * size + 5 * size, size, size);
            if(s == "light") if(FM.minos[9]) return new Rectangle(9 * size, type * size, size, size); else return new Rectangle(9 * size, type * size + 5 * size, size, size);
            if(s == "dark") if(FM.minos[10]) return new Rectangle(10 * size, type * size, size, size); else return new Rectangle(10 * size, type * size + 5 * size, size, size);
            if(s == "gold") if(FM.minos[11]) return new Rectangle(11 * size, type * size, size, size); else return new Rectangle(11 * size, type * size + 5 * size, size, size);
            return new Rectangle(0, 0, size, size);
        }

        public void Draw_Font(string text, float posX, float posY, Color color, float scale) {
            int width = 0;
            foreach(char c in text) {
                spriteBatch.Draw(SK.texture_spritesheet_font, new Vector2(posX + width * scale, posY), Get_FontPos(c), color, 0, new Vector2(0, 0), scale, SpriteEffects.None, 0);
                width += Get_FontWidth(c);
            }
        }

        public void Draw_Title(string text, float posX, float posY, Color color) {
            int width = 0;
            int length = 0;
            foreach(char c in text) {
                length += Get_FontWidth(c);
            }
            length /= 2;
            foreach(char c in text) {
                spriteBatch.Draw(SK.texture_spritesheet_font, new Vector2(posX - length + width, posY), Get_FontPos(c), color);
                width += Get_FontWidth(c);
            }
        }

        public void Draw_Values(string text, float posX, float posY, Color color, bool revert) {
            int width = 0;
            if(revert) {
                int length = text.Length*21;
                foreach(char c in text) {
                    spriteBatch.Draw(SK.texture_spritesheet_values, new Vector2(posX + width - length, posY), new Rectangle(int.Parse(""+c) * 20, 0, 20, 28), color);
                    width += 21;
                }
            } else {
                foreach(char c in text) {
                    spriteBatch.Draw(SK.texture_spritesheet_values, new Vector2(posX + width, posY), new Rectangle(int.Parse(""+c) * 20, 0, 20, 28), color);
                    width += 21;
                }
            }
        }

        public int Get_FontWidth(char s) {
            if(s == 'a') return 32;
            if(s == 'b') return 26;
            if(s == 'c') return 25;
            if(s == 'd') return 28;
            if(s == 'e') return 20;
            if(s == 'f') return 19;
            if(s == 'g') return 29;
            if(s == 'h') return 29;
            if(s == 'i') return 10;
            if(s == 'j') return 19;
            if(s == 'k') return 28;
            if(s == 'l') return 20;
            if(s == 'm') return 31;
            if(s == 'n') return 29;
            if(s == 'o') return 30;
            if(s == 'p') return 25;
            if(s == 'q') return 30;
            if(s == 'r') return 25;
            if(s == 's') return 21;
            if(s == 't') return 26;
            if(s == 'u') return 27;
            if(s == 'v') return 30;
            if(s == 'w') return 31;
            if(s == 'x') return 29;
            if(s == 'y') return 27;
            if(s == 'z') return 26;
            if(s == '0') return 24;
            if(s == '1') return 16;
            if(s == '2') return 22;
            if(s == '3') return 22;
            if(s == '4') return 25;
            if(s == '5') return 22;
            if(s == '6') return 24;
            if(s == '7') return 23;
            if(s == '8') return 24;
            if(s == '9') return 24;
            if(s == ' ') return 24;
            if(s == '-') return 12;
            return 32;
        }

        public Rectangle Get_FontPos(char s) {
            if(s == 'a') return new Rectangle(  0,  0, 32, 32);
            if(s == 'b') return new Rectangle( 32,  0, 32, 32);
            if(s == 'c') return new Rectangle( 64,  0, 32, 32);
            if(s == 'd') return new Rectangle( 96,  0, 32, 32);
            if(s == 'e') return new Rectangle(128,  0, 32, 32);
            if(s == 'f') return new Rectangle(160,  0, 32, 32);
            if(s == 'g') return new Rectangle(192,  0, 32, 32);
            if(s == 'h') return new Rectangle(224,  0, 32, 32);
            if(s == 'i') return new Rectangle(256,  0, 32, 32);
            if(s == 'j') return new Rectangle(288,  0, 32, 32);
            if(s == 'k') return new Rectangle(320,  0, 32, 32);
            if(s == 'l') return new Rectangle(352,  0, 32, 32);
            if(s == 'm') return new Rectangle(384,  0, 32, 32);
            if(s == 'n') return new Rectangle(416,  0, 32, 32);
            if(s == 'o') return new Rectangle(448,  0, 32, 32);
            if(s == 'p') return new Rectangle(480,  0, 32, 32);
            if(s == 'q') return new Rectangle(512,  0, 32, 32);
            if(s == 'r') return new Rectangle(544,  0, 32, 32);
            if(s == 's') return new Rectangle(576,  0, 32, 32);
            if(s == 't') return new Rectangle(608,  0, 32, 32);
            if(s == 'u') return new Rectangle(640,  0, 32, 32);
            if(s == 'v') return new Rectangle(672,  0, 32, 32);
            if(s == 'w') return new Rectangle(704,  0, 32, 32);
            if(s == 'x') return new Rectangle(736,  0, 32, 32);
            if(s == 'y') return new Rectangle(768,  0, 32, 32);
            if(s == 'z') return new Rectangle(800,  0, 32, 32);
            if(s == '0') return new Rectangle(  0, 32, 32, 32);
            if(s == '1') return new Rectangle( 32, 32, 32, 32);
            if(s == '2') return new Rectangle( 64, 32, 32, 32);
            if(s == '3') return new Rectangle( 96, 32, 32, 32);
            if(s == '4') return new Rectangle(128, 32, 32, 32);
            if(s == '5') return new Rectangle(160, 32, 32, 32);
            if(s == '6') return new Rectangle(192, 32, 32, 32);
            if(s == '7') return new Rectangle(224, 32, 32, 32);
            if(s == '8') return new Rectangle(256, 32, 32, 32);
            if(s == '9') return new Rectangle(288, 32, 32, 32);
            if(s == ' ') return new Rectangle(320, 32, 32, 32);
            if(s == '-') return new Rectangle(352, 32, 32, 32);
            return new Rectangle(736, 0, 32, 32);
        }

    }
}
