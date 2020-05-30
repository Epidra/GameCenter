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
    class Menu : Ghost {

        public bool  active_title       = true;
        public bool  active_transfield  = false; // Switch for moving between Menu Grids
        public int   active_infoside    = 0;
        public float[] alpha = new float[6]; // Transition gets controlled by active_field

        /// <summary>
        /// 0 - Main Grid
        /// 1 - Options
        /// 2 - Minos
        /// 3 - Statistics
        /// 4 - Info
        /// 5 - Store
        /// </summary>
        public int  active_field = 0;

        public int active_gameoption  = 0;
        public int active_difficulty  = 1;
        public int active_levelselect = 0;
        public int active_minooption  = 0;

        public float title_alpha = 0.00f;
        public bool  title_up = true;

        public int selector_start = 1;

        public int current_row_main      = 0;
        public int current_row_options   = 0;
        public int current_row_store     = 0;
        public int current_row_stats     = 0;
        public int current_row_mino      = 0;
        public int current_row_info      = 0;
        public int current_point_main    = 0;
        public int current_point_store   = 0;
        public int current_point_options = 0;
        public int current_point_mino    = 0;

        public Menu(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            alpha[0] = 1.00f;
            alpha[1] = 0.00f;
            alpha[2] = 0.00f;
            alpha[3] = 0.00f;
            alpha[4] = 0.00f;
            alpha[5] = 0.00f;
        }

        public override string Update3(GameTime gameTime) {
            if(active_title) {
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed || ButtonPressed(GhostKey.button_start) == GhostState.pressed) {
                    pressed_response = true;
                    active_title = false;
                }
            } else if(FM.active_transition) { // Transition between Screens - controlled elsewhere

            } else if(FM.active_firststart) { // Game started the first time
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { if(selector_start > 0)   selector_start--;  pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { if(selector_start < 2)   selector_start++;  pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { if(active_infoside == 0) active_infoside++; pressed_response = true; }
                if(active_infoside == 1) {
                    if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                        if(selector_start == 0) FM.purchased[FM.Convert("octanom")]   = 1;
                        if(selector_start == 1) FM.purchased[FM.Convert("snake")]     = 1;
                        if(selector_start == 2) FM.purchased[FM.Convert("octacubes")] = 1;
                        JK.Noise("Coin");
                        FM.Save();
                        FM.Special_Purchase(-1);
                        FM.active_firststart = false;
                        FM.active_transition = true;
                        active_infoside = 0;
                    }
                    if(pressed_event_touch) {
                        pressed_response = true;
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X - SK.position_grid_next, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(selector_start != 0) { selector_start = 0; } else { FM.purchased[FM.Convert("octanom")]   = 1; JK.Noise("Coin"); FM.Save(); FM.Special_Purchase(-1); FM.active_firststart = false; FM.active_transition = true; active_infoside = 0; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X                        , (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(selector_start != 1) { selector_start = 1; } else { FM.purchased[FM.Convert("snake")]     = 1; JK.Noise("Coin"); FM.Save(); FM.Special_Purchase(-1); FM.active_firststart = false; FM.active_transition = true; active_infoside = 0; } }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X + SK.position_grid_next, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(selector_start != 2) { selector_start = 2; } else { FM.purchased[FM.Convert("octacubes")] = 1; JK.Noise("Coin"); FM.Save(); FM.Special_Purchase(-1); FM.active_firststart = false; FM.active_transition = true; active_infoside = 0; } }
                    }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X - SK.position_grid_next, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { selector_start = 0; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X                        , (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { selector_start = 1; }
                    if(Collision_Button(true, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Position_FirstStart().X + SK.position_grid_next, (int)SK.Position_DisplayEdge().Y + (int)SK.Position_FirstStart().Y, SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { selector_start = 2; }
                }

            } else if(active_transfield) { // Moving between Games and Options/Store Screen
                for(int i = 0; i < 6; i++) {
                    if(active_field == i) {
                        alpha[i] += 0.04f;
                        if(alpha[i] >= 1) {
                            active_transfield = false;
                        }
                    } else if(alpha[i] > 0) {
                        alpha[i] -= 0.04f;
                    }
                }
            } else if(FM.active_info) { // active: Game Info Screen
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { // From Infofield to actual Game Screen
                    FM.active_transition = true;
                    pressed_response = true;
                    return "gamestart";
                }
                if(ButtonPressed(GhostKey.button_exit) == GhostState.pressed) { FM.active_transition = true; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed) { FM.active_transition = true; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)  { if (active_infoside > 0) { active_infoside--; if (active_infoside == 3) active_infoside = 2; pressed_response = true; } }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if (active_infoside < 4) { active_infoside++; if (active_infoside == 3) active_infoside = 4; pressed_response = true; } }

                if(active_infoside == 0) { // Game Option Info Screen

                    if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(active_gameoption > 0) active_gameoption--; }
                    if(0 < FM.purchased[FM.Convert("level select")]) {
                        if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(active_gameoption < 2) active_gameoption++; }
                    } else {
                        if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(active_gameoption < 1) active_gameoption++; }
                    }
                    if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                        pressed_response = true;
                        if(active_gameoption == 0) { // Start Game
                            FM.active_transition = true;
                            return "gamestart";
                        }
                        if(active_gameoption == 1) { // Difficulty
                                   if(active_difficulty == 0) { active_difficulty = 1;
                            } else if(active_difficulty == 1) { if(0 < FM.purchased[FM.Convert("hard mode")]) { active_difficulty = 2; } else if(0 < FM.purchased[FM.Convert("easy mode")]) { active_difficulty = 0; }
                            } else if(active_difficulty == 2) { if(0 < FM.purchased[FM.Convert("easy mode")]) { active_difficulty = 0; } else { active_difficulty = 1; }
                            }
                        }
                        if(active_gameoption == 2) { // Level Select
                            // NOT IMPLEMENTED
                        }
                    }
                    if(pressed_event_touch) {
                        pressed_response = true;
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Get_GridSize().X / 2 - SK.texture_menu_info_startgame.Width / 2, (int)SK.Position_DisplayEdge().Y + 100, SK.texture_menu_info_startgame.Width, SK.texture_menu_info_startgame.Height))) {
                            active_gameoption = 0;
                            FM.active_transition = true;
                            return "gamestart";
                        }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Get_GridSize().X / 2 - SK.texture_menu_info_difficulty.Width / 2, (int)SK.Position_DisplayEdge().Y + 250, SK.texture_menu_info_difficulty.Width, SK.texture_menu_info_difficulty.Height))) {
                            active_gameoption = 1;
                            if(active_difficulty == 0) { active_difficulty = 1;
                            } else if(active_difficulty == 1) { if(0 < FM.purchased[FM.Convert("hard mode")]) { active_difficulty = 2; } else if(0 < FM.purchased[FM.Convert("easy mode")]) { active_difficulty = 0; }
                            } else if(active_difficulty == 2) { if(0 < FM.purchased[FM.Convert("easy mode")]) { active_difficulty = 0; } else { active_difficulty = 1; }
                            }
                        }
                        if(Collision_Button(false, new Rectangle((int)SK.Position_DisplayEdge().X + (int)SK.Get_GridSize().X / 2 - SK.texture_menu_info_levelselect.Width / 2, (int)SK.Position_DisplayEdge().Y + 450, SK.texture_menu_info_levelselect.Width, SK.texture_menu_info_levelselect.Height))) {
                            if(0 < FM.purchased[FM.Convert("level select")]) {
                                active_gameoption = 2;
                            }

                        }
                    }
                }
                if(active_infoside == 3) { // Mino Info Screen
                    if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)   { if(active_minooption >  0) active_minooption--; }
                    if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(active_minooption < 10) active_minooption++; }
                }
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed) {
                    FM.active_transition = true;
                    pressed_response     = true;
                }
                // End of INFO SCREEN


            } else if(active_field == 5) { // active: Store
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { active_transfield = true; active_field = 3; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transfield = true; active_field = 0; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { pressed_response  = true;    current_row_store -= 1; if(current_row_store <                 0) current_row_store = SK.maxrows_store - 1; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { pressed_response  = true;    current_row_store += 1; if(current_row_store >= SK.maxrows_store) current_row_store = 0; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { pressed_response  = true; if(current_point_store > 0) current_point_store--; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { pressed_response  = true; if(current_point_store < 1) current_point_store++; }
                if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed && !pressed_event_touch) { // Purchases the selected article (Keyboard)
                    if(!pressed_event_touch) {
                        pressed_response = true;
                        if(!Get_PurchasedStore(current_row_store * 2 + current_point_store)) {
                            if(FM.coins >= FM.price[current_row_store * 2 + current_point_store]) {
                                FM.coins = FM.coins - FM.price[current_row_store * 2 + current_point_store];
                                FM.coins_bonus = FM.coins_bonus - FM.price[current_row_store * 2 + current_point_store];
                                FM.purchased[current_row_store * 2 + current_point_store]++;
                                JK.Noise("Coin");
                                FM.Special_Purchase(current_row_store * 2 + current_point_store);
                                FM.Save();
                            }
                        }
                    }
                }
                if(pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) { // Purchases the selected article (Touch)
                    pressed_response = true;
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y                                    - 25) { current_row_store -= 1; if(current_row_store <                 0) current_row_store = SK.maxrows_store - 1; }
                    if (control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) { current_row_store += 1; if(current_row_store >= SK.maxrows_store) current_row_store = 0;                    }
                    for(int i = 0; i < 2; i++) {
                        if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + SK.position_grid_next * i), (int)(SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y), SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) {
                            if(current_point_store != i) {
                                current_point_store = i;
                            } else {
                                if(!Get_PurchasedStore(current_row_store * 2 + current_point_store)) {
                                    if(FM.coins >= FM.price[current_row_store * 2 + current_point_store]) {
                                        FM.coins = FM.coins - FM.price[current_row_store * 2 + current_point_store];
                                        FM.coins_bonus = FM.coins_bonus - FM.price[current_row_store * 2 + current_point_store];
                                        FM.purchased[current_row_store * 2 + current_point_store]++;
                                        JK.Noise("Coin");
                                        FM.Special_Purchase(current_row_store * 2 + current_point_store);
                                        FM.Save();
                                    }
                                }
                            }
                        }
                    }
                }
            } else if(active_field == 4) { // active: Info
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { active_transfield = true; active_field = 3; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transfield = true; active_field = 5; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)   { current_row_info -= 1; if(current_row_info <                0) current_row_info = SK.maxrows_info - 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { current_row_info += 1; if(current_row_info >= SK.maxrows_info) current_row_info = 0;                   pressed_response = true; }
                if(pressed_event_touch) {
                    pressed_response = true;
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y                                    - 25) { current_row_info -= 1; if(current_row_info <                0) current_row_info = SK.maxrows_info - 1; }
                    if (control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) { current_row_info += 1; if(current_row_info >= SK.maxrows_info) current_row_info = 0;                    }
                }
            } else if(active_field == 3) { // active: Statistics
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { active_transfield = true; active_field = 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transfield = true; active_field = 5; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)   { current_row_stats -= 1; if(current_row_stats <                 0) current_row_stats = SK.maxrows_stats - 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { current_row_stats += 1; if(current_row_stats >= SK.maxrows_stats) current_row_stats = 0;                    pressed_response = true; }
                if(pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                    pressed_response = true;
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y                                    - 25) { current_row_stats -= 1; if(current_row_stats <                 0) current_row_stats = SK.maxrows_stats - 1; }
                    if (control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) { current_row_stats += 1; if(current_row_stats >= SK.maxrows_stats) current_row_stats =                    0; }
                }
            } else if(active_field == 2) { // active: Minos
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { active_transfield = true; active_field = 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transfield = true; active_field = 3; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)   { current_row_mino -= 1; if(current_row_mino <                 0) current_row_mino = SK.maxrows_minos - 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { current_row_mino += 1; if(current_row_mino >= SK.maxrows_minos) current_row_mino = 0;                    pressed_response = true; }
                if(pressed_event_touch) {
                    pressed_response = true;
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y                                    - 25) { current_row_mino -= 1; if(current_row_mino <                 0) current_row_mino = SK.maxrows_minos - 1; }
                    if (control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) { current_row_mino += 1; if(current_row_mino >= SK.maxrows_minos) current_row_mino =                    0; }
                }
            } else if(active_field == 1) { // active: Options
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { active_transfield = true; active_field = 0; pressed_response = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { active_transfield = true; active_field = 3; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { current_row_options -= 1; if(current_row_options <                   0) current_row_options = SK.maxrows_options - 1; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { current_row_options += 1; if(current_row_options >= SK.maxrows_options) current_row_options = 0;                      pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { if(current_point_options > 0) current_point_options--; pressed_response = true; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { if(current_point_options < 2) current_point_options++; pressed_response = true; }
                bool temp = false;
                if(pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                    pressed_response = true;
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y                                    - 25) { current_row_options -= 1; if(current_row_options <                   0) current_row_options = SK.maxrows_options - 1; }
                    if (control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) { current_row_options += 1; if(current_row_options >= SK.maxrows_options) current_row_options = 0;                      }
                    if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 0), (int)(SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(current_point_options != 0) { current_point_options = 0; } else { temp = true; } }
                    if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1), (int)(SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(current_point_options != 1) { current_point_options = 1; } else { temp = true; } }
                    if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4), (int)(SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) { if(current_point_options != 2) { current_point_options = 2; } else { temp = true; } }
                }
                if(!pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) {
                    temp = true;
                }
                if (temp){
                    string tempO = Command_Options(current_row_options * 3 + current_point_options);
                    if (tempO != "void") return tempO;
                }
            } else { // active: Main
                if(ButtonPressed(GhostKey.button_exit) == GhostState.pressed) return "exit";
#if DEBUG
                // Debug Cheats

                if(control_keyboard_new.IsKeyDown(Keys.F5) && control_keyboard_old.IsKeyUp(Keys.F5)) { // Bonus Coins
                    FM.coins += 10000;
                    FM.coins_bonus += 10000;
                    JK.Noise("Coin");
                }
                if(control_keyboard_new.IsKeyDown(Keys.F6) && control_keyboard_old.IsKeyUp(Keys.F6)) { // Purchase All
                    for(int i = 0; i < FM.purchased.Length; i++) {
                        FM.purchased[i]++;
                    }
                    FM.Special_Purchase(-1);
                    JK.Noise("Coin");
                }
                if(control_keyboard_new.IsKeyDown(Keys.F7) && control_keyboard_old.IsKeyUp(Keys.F7)) { // Remove All
                    for(int i = 0; i < FM.purchased.Length; i++) {
                        FM.purchased[i] = 0;
                    }
                    FM.Special_Purchase(-1);
                    JK.Noise("Explosion");
                }
                if(control_keyboard_new.IsKeyDown(Keys.F8) && control_keyboard_old.IsKeyUp(Keys.F8)) { // Play Debug Game
                    return "debuggame";
                }
#endif
                if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed)     { pressed_response = true; current_row_main -= 1; if(current_row_main <                0) current_row_main = SK.maxrows_main - 1; }
                if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed)   { pressed_response = true; current_row_main += 1; if(current_row_main >= SK.maxrows_main) current_row_main = 0;                   }
                if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed)   { pressed_response = true; if(current_point_main > 0) current_point_main--; }
                if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed)  { pressed_response = true; if(current_point_main < 4) current_point_main++; }
                if(!pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed)    { if(current_row_main * 5 + current_point_main < FM.highscore.Length) { if(0 < FM.purchased[current_row_main * 5 + current_point_main]) FM.active_transition = true; } }
                if(ButtonPressed(GhostKey.button_special_P1) == GhostState.pressed)  { pressed_response = true; active_field = 5; active_transfield = true; }
                if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { pressed_response = true; active_field = 1; active_transfield = true; }

                if(pressed_event_touch && ButtonPressed(GhostKey.button_ok_P1) != GhostState.released) {
                    pressed_response = true;
                    if(control_cursor.Y > SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.texture_menu_grid_full.Height + 25 && control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y) {
                        current_row_main += 1;
                        if(current_row_main >= SK.maxrows_main) current_row_main = 0;
                    }
                    if(control_cursor.Y < SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y - 25) {
                        current_row_main -= 1;
                        if(current_row_main < 0) current_row_main = SK.maxrows_main - 1;
                    }
                    for(int i = 0; i < 5; i++) {
                        if(Collision_Button(false, new Rectangle((int)(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * i), (int)(SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y), SK.texture_menu_grid_full.Width, SK.texture_menu_grid_full.Height))) {
                            if(current_point_main != i) {
                                current_point_main = i;
                            } else {
                                if(current_row_main * 5 + current_point_main < FM.highscore.Length) {
                                    if(0 < FM.purchased[current_row_main * 5 + current_point_main]) FM.active_transition = true;
                                }
                            }
                        }
                    }
                }

            }
            return "void";
        }

        public override void Draw2() {
            if(active_title) {
                if(FM.active_firststart) {
                    spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10       - FM.rolling_vertical), Color.White);
                    spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10 + 900 - FM.rolling_vertical), Color.White);
                }
                spriteBatch.Draw(SK.texture_background_logo_title, SK.Position_DisplayEdge() + SK.Position_Title_Logo(), Color.White);
                spriteBatch.Draw(SK.texture_background_logo_start, SK.Position_DisplayEdge() + SK.Position_Title_Start(), Color.White * title_alpha);

                if(title_up) {
                    title_alpha = title_alpha + 0.01f;
                    if(title_alpha >= 1.0f) title_up = false;
                } else {
                    title_alpha = title_alpha - 0.01f;
                    if(title_alpha <= 0.0f) title_up = true;
                }
            } else {

                // Draw Store
                if(alpha[5] > 0) {
                    for(int j = -3; j < 5; j++) {
                        for(int i = 0; i < 2; i++) {
                            int beta = 1; if(Get_StoreValid(current_row_store * 2 + j * 2 + i) && Get_PurchasedStore(current_row_store * 2 + j * 2 + i)) beta = 2;
                            spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + SK.position_grid_next * i + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y + SK.position_grid_row.Z * j), Get_StoreColor(current_row_store * 2 + j * 2 + i) * (alpha[5] / beta));
                            spriteBatch.Draw(SK.texture_menu_grid_store, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y + SK.position_grid_row.Z * j) + SK.Position_Store(i), Get_StoreColor(current_row_store * 2 + j * 2 + i) * (alpha[5] / beta));
                            if(Get_StoreValid(current_row_store * 2 + j * 2 + i)) {
                                spriteBatch.Draw(Get_StoreIcon(current_row_store * 2 + j * 2 + i), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + SK.position_grid_next * i + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y + SK.position_grid_row.Z * j), Color.White * (alpha[5] / beta));
                                if(!Get_PurchasedStore(current_row_store * 2 + j * 2 + i)) {
                                    Draw_Font(Get_StoreName(current_row_store * 2 + j * 2 + i), SK.Position_DisplayEdge().X + SK.Position_GridMain().X + (int)(SK.position_grid_next * 3.5 * i) + SK.position_grid_row.X * j + 20, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y + SK.texture_menu_grid_full.Height / 4 * i + SK.position_grid_row.Z * j + 45, Color.White * alpha[5], 0.5f);
                                    spriteBatch.DrawString(SK.font_score, "" + Get_Price(current_row_store * 2 + j * 2 + i), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + SK.position_grid_row.X * j + 75, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y + SK.position_grid_row.Z * j + 80) + SK.Position_Store(i), Color.Black * alpha[5]);
                                }
                            }
                        }
                    }
                    spriteBatch.Draw(SK.texture_menu_grid_selector, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridStore().X + SK.position_grid_next * current_point_store, SK.Position_DisplayEdge().Y + SK.Position_GridStore().Y), Color.White * alpha[5]);
                }

                // Draw Info
                if(alpha[4] > 0) {
                    for(int j = -3; j < 5; j++) {
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[4]);
                        spriteBatch.Draw(SK.texture_menu_grid_options, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[4]);
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[4]);

                        spriteBatch.Draw(Get_StatsIcon(86), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Color.White * alpha[4]);

                        spriteBatch.DrawString(SK.font_score, Get_InfoText(current_row_info + j, true), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j + 30, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j) + new Vector2(0, 25), Color.Black * alpha[4]);
                        spriteBatch.DrawString(SK.font_score, Get_InfoText(current_row_info + j, false), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j + 30, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j) + new Vector2(25, 75), Color.Black * alpha[4]);

                        spriteBatch.Draw(SK.texture_menu_grid_selector, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), Color.White * alpha[4]);
                    }
                }

                // Draw Statistics
                if(alpha[3] > 0) {
                    for(int j = -3; j < 5; j++) {
                        if(PurchasedStats(current_row_stats + j)) {
                            spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);
                            spriteBatch.Draw(SK.texture_menu_grid_options, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);
                            spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);

                            spriteBatch.Draw(Get_StatsIcon(current_row_stats + j), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);
                            spriteBatch.Draw(Get_StatsMedal(current_row_stats + j), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);
                            spriteBatch.Draw(Get_StatsGame(current_row_stats + j), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Color.White * alpha[3]);

                            spriteBatch.DrawString(SK.font_score, Get_StatsText(current_row_stats + j, true), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j + 30, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j) + new Vector2(0, 25), Color.Black * alpha[3]);
                            spriteBatch.DrawString(SK.font_score, Get_StatsText(current_row_stats + j, false), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j + 30, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j) + new Vector2(25, 75), Color.Black * alpha[3]);
                        } else {
                            spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3] * 0.5f);
                            spriteBatch.Draw(SK.texture_menu_grid_options, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3] * 0.5f);
                            spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[3] * 0.5f);
                        }
                    }
                }

                // Draw Minos
                if(alpha[2] > 0) {
                    for(int j = -3; j < 5; j++) {
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[2]);
                        spriteBatch.Draw(SK.texture_menu_grid_options, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[2]);
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[2]);

                        spriteBatch.Draw(Get_MinoIcon(current_row_mino + j), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Get_MinoColor(current_row_mino + j) * alpha[2]);

                        spriteBatch.Draw(SK.texture_menu_grid_selector, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * current_point_mino, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), Color.White * alpha[2]);
                    }
                }

                // Draw Options
                if(alpha[1] > 0) {
                    for(int j = -3; j < 5; j++) {
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);
                        spriteBatch.Draw(SK.texture_menu_grid_options, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);
                        spriteBatch.Draw(SK.texture_menu_grid_full, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);

                        spriteBatch.Draw(Get_OptionIcon(current_row_options * 3 + j * 3 + 0), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + +SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);
                        spriteBatch.Draw(Get_OptionIcon(current_row_options * 3 + j * 3 + 1), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);
                        spriteBatch.Draw(Get_OptionIcon(current_row_options * 3 + j * 3 + 2), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 4 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j), Color.White * alpha[1]);
                        spriteBatch.DrawString(SK.font_score, Get_OptionText(current_row_options + j, true), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j) + new Vector2(150, 25), Color.Black * alpha[1]);
                        spriteBatch.DrawString(SK.font_score, Get_OptionText(current_row_options + j, false), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * 1 + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y + SK.position_grid_row.Z * j) + new Vector2(175, 75), Color.Black * alpha[1]);
                    }
                    int temp = current_point_options; if(temp == 2) temp = 4;
                    spriteBatch.Draw(SK.texture_menu_grid_selector, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridOptions().X + SK.position_grid_next * temp, SK.Position_DisplayEdge().Y + SK.Position_GridOptions().Y), Color.White * alpha[1]);
                }

                // Draw Main
                if(alpha[0] > 0) {
                    for(int j = -3; j < 5; j++) {
                        for(int i = 0; i < 5; i++) {
                            spriteBatch.Draw(Get_GridTexture(current_row_main * 5 + j * 5 + i), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * i + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Get_GridColor(i + j * 5 + current_row_main * 5) * alpha[0]);
                            spriteBatch.Draw(Get_GridIcon(current_row_main * 5 + j * 5 + i), new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * i + SK.position_grid_row.X * j, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y + SK.position_grid_row.Z * j), Color.White * alpha[0]);
                        }
                    }
                    if(!FM.active_info) spriteBatch.Draw(SK.texture_menu_grid_selector, new Vector2(SK.Position_DisplayEdge().X + SK.Position_GridMain().X + SK.position_grid_next * current_point_main, SK.Position_DisplayEdge().Y + SK.Position_GridMain().Y), Color.White * alpha[0]);
                }

                //Draw Info
                float t = 0.00f; t = (float)FM.transition / 100;
                if(t > 0) {
                    spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10 - FM.rolling_vertical), Color.White * t);
                    spriteBatch.Draw(SK.texture_background_info, new Vector2(SK.Position_DisplayEdge().X - 10, SK.Position_DisplayEdge().Y - 10 + 900 - FM.rolling_vertical), Color.White * t);
                    spriteBatch.Draw(SK.texture_menu_info_board, new Vector2(SK.Position_DisplayEdge().X - 100 + FM.transition, SK.Position_DisplayEdge().Y - 10), null, Color.White * t, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                    spriteBatch.Draw(SK.texture_menu_info_board, new Vector2(SK.Position_DisplayEdge().X - 100 + FM.transition, SK.Position_DisplayEdge().Y - 10 + 450), null, Color.White * t, 0f, new Vector2(0, 0), 1f, SpriteEffects.FlipVertically, 0);
                    if(FM.active_firststart) {
                        if(active_infoside == 0) {
                            int pos = 0;
                            foreach(string s in SK.info_firststart[0]) {
                                spriteBatch.DrawString(SK.font_score, s, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(0, 30 * pos), Color.Black);
                                pos++;
                            }
                        } else {
                            int pos = 0;
                            foreach(string s in SK.info_firststart[1]) {
                                spriteBatch.DrawString(SK.font_score, s, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(0, 30 * pos), Color.Black);
                                pos++;
                            }
                            spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X - SK.position_grid_next, SK.Position_FirstStart().Y), Color.White);
                            spriteBatch.Draw(SK.Get_GridIcon(1, 0), SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X - SK.position_grid_next, SK.Position_FirstStart().Y), Color.White);
                            spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X, SK.Position_FirstStart().Y), Color.White);
                            spriteBatch.Draw(SK.Get_GridIcon(2, 0), SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X, SK.Position_FirstStart().Y), Color.White);
                            spriteBatch.Draw(SK.texture_menu_grid_full, SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X + SK.position_grid_next, SK.Position_FirstStart().Y), Color.White);
                            spriteBatch.Draw(SK.Get_GridIcon(3, 0), SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X + SK.position_grid_next, SK.Position_FirstStart().Y), Color.White);

                            if(selector_start == 0) Draw_Font(FM.name[1], SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2 - 31 * FM.name[1].Length / 2, SK.Position_DisplayEdge().Y + SK.Position_FirstStart().Y - 50, Color.White, 1);
                            if(selector_start == 1) Draw_Font(FM.name[2], SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2 - 31 * FM.name[2].Length / 2, SK.Position_DisplayEdge().Y + SK.Position_FirstStart().Y - 50, Color.White, 1);
                            if(selector_start == 2) Draw_Font(FM.name[3], SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2 - 31 * FM.name[3].Length / 2, SK.Position_DisplayEdge().Y + SK.Position_FirstStart().Y - 50, Color.White, 1);

                            spriteBatch.Draw(SK.texture_menu_grid_selector, SK.Position_DisplayEdge() + new Vector2(SK.Position_FirstStart().X - SK.position_grid_next + SK.position_grid_next * selector_start, SK.Position_FirstStart().Y), Color.White);
                        }

                    } else {
                        if(active_infoside == 0) { spriteBatch.Draw(SK.texture_menu_info_left, SK.Position_DisplayEdge() + new Vector2(10 - 100 + FM.transition, 400), Color.White * (t / 4)); } else { spriteBatch.Draw(SK.texture_menu_info_left, SK.Position_DisplayEdge() + new Vector2(10 - 100 + FM.transition, 400), Color.White); }
                        if(active_infoside == 4) { spriteBatch.Draw(SK.texture_menu_info_right, SK.Position_DisplayEdge() + new Vector2(10 - 100 + FM.transition, 500), Color.White * (t / 4)); } else { spriteBatch.Draw(SK.texture_menu_info_right, SK.Position_DisplayEdge() + new Vector2(10 - 100 + FM.transition, 500), Color.White); }

                        if(active_infoside == 0) { // Draw Start Options
                            //float t2 = 0 < FM.purchased[FM.Convert("level select")] ? t : 0.1f;
                            spriteBatch.Draw(SK.texture_menu_info_startgame, SK.Position_DisplayEdge() + new Vector2(105, 75), active_gameoption == 0 ? Color.DarkGray * t : Color.White * t);
                            spriteBatch.Draw(SK.texture_menu_info_difficulty, SK.Position_DisplayEdge() + new Vector2(105, 175), active_gameoption == 1 ? Color.DarkGray * t : Color.White * t);
                            //spriteBatch.Draw(SK.texture_menu_info_levelselect, SK.Position_DisplayEdge() + new Vector2(105, 275), active_gameoption == 2 ? Color.DarkGray * t2: Color.White * t2);

                            if(active_difficulty == 0) spriteBatch.Draw(SK.texture_menu_info_easy, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X - SK.texture_menu_info_easy.Width - 45, 175), Color.White * t);
                            if(active_difficulty == 1) spriteBatch.Draw(SK.texture_menu_info_medium, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X - SK.texture_menu_info_medium.Width - 45, 175), Color.White * t);
                            if(active_difficulty == 2) spriteBatch.Draw(SK.texture_menu_info_hard, SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X - SK.texture_menu_info_hard.Width - 45, 175), Color.White * t);
                        }
                        if(active_infoside == 1) { // Draw Description
                            int pos = 0;
                            foreach(string s in SK.info_description[current_row_main * 5 + current_point_main]) {
                                spriteBatch.DrawString(SK.font_score, s, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-10, 30 * pos), Color.Black * t);
                                pos++;
                            }
                        }
                        if(active_infoside == 2) { // Draw Control Info
                            int pos = 0;
                            foreach(int i in SK.info_control_icon[current_row_main * 5 + current_point_main]) {
                                spriteBatch.Draw(SK.Get_ControlIcon(i), SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(0, 100 * pos), Color.White * t);
                                pos++;
                            }
                            pos = 0;
                            foreach(string s in SK.info_control_text[current_row_main * 5 + current_point_main]) {
                                spriteBatch.DrawString(SK.font_score, s, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 100 * pos + 25), Color.Black * t);
                                pos++;
                            }
                        }
                        if(active_infoside == 3) { // Draw Mino Options
                            if(0 == FM.purchased[FM.Convert("fire mino")])    { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("fire mino"),    0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  0 * 50), null, active_minooption ==  0 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("fire mino"),    FM.purchased[FM.Convert("fire mino")]    - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  0 * 50), null, active_minooption ==  0 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("air mino")])     { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("air mino"),     0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  1 * 50), null, active_minooption ==  1 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("air mino"),     FM.purchased[FM.Convert("air mino")]     - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  1 * 50), null, active_minooption ==  1 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("thunder mino")]) { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("thunder mino"), 0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  2 * 50), null, active_minooption ==  2 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("thunder mino"), FM.purchased[FM.Convert("thunder mino")] - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  2 * 50), null, active_minooption ==  2 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("water mino")])   { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("water mino"),   0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  3 * 50), null, active_minooption ==  3 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("water mino"),   FM.purchased[FM.Convert("water mino")]   - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  3 * 50), null, active_minooption ==  3 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("ice mino")])     { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("ice mino"),     0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  4 * 50), null, active_minooption ==  4 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("ice mino"),     FM.purchased[FM.Convert("ice mino")]     - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  4 * 50), null, active_minooption ==  4 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("earth mino")])   { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("earth mino"),   0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  5 * 50), null, active_minooption ==  5 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("earth mino"),   FM.purchased[FM.Convert("earth mino")]   - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  5 * 50), null, active_minooption ==  5 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("metal mino")])   { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("metal mino"),   0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  6 * 50), null, active_minooption ==  6 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("metal mino"),   FM.purchased[FM.Convert("metal mino")]   - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  6 * 50), null, active_minooption ==  6 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("nature mino")])  { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("nature mino"),  0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  7 * 50), null, active_minooption ==  7 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("nature mino"),  FM.purchased[FM.Convert("nature mino")]  - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  7 * 50), null, active_minooption ==  7 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("light mino")])   { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("light mino"),   0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  8 * 50), null, active_minooption ==  8 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("light mino"),   FM.purchased[FM.Convert("light mino")]   - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  8 * 50), null, active_minooption ==  8 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("dark mino")])    { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("dark mino"),    0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 +  9 * 50), null, active_minooption ==  9 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("dark mino"),    FM.purchased[FM.Convert("dark mino")]    - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 +  9 * 50), null, active_minooption ==  9 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                            if(0 == FM.purchased[FM.Convert("gold mino")])    { spriteBatch.Draw(SK.Get_GridIcon(FM.Convert("gold mino"),    0), SK.Position_DisplayEdge() + new Vector2(85 - 8, 40 - 18 + 10 * 50), null, active_minooption == 10 ? Color.Black * t : Color.Black * t * 0.75f, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); } else { spriteBatch.Draw(SK.Get_GridIcon(-1, 0 /*FM.Convert("gold mino"),    FM.purchased[FM.Convert("gold mino")]    - 1*/), SK.Position_DisplayEdge() + new Vector2(85, 40 - 10 + 10 * 50), null, active_minooption == 10 ? Color.DarkGray * t : Color.White * t, 0, new Vector2(0,0), 0.5f, SpriteEffects.None, 0); }
                        }
                        if(active_infoside == 4) { // Draw Statistics
                            if(0 < FM.purchased[FM.Convert("highscore")]) spriteBatch.Draw(SK.texture_icon_highscore, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, -15), Color.White * t);
                            if(0 < FM.purchased[FM.Convert("coins earned")]) spriteBatch.Draw(SK.texture_icon_coins, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 85), Color.White * t);
                            if(0 < FM.purchased[FM.Convert("playtime")]) spriteBatch.Draw(SK.texture_icon_times_played, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 185), Color.White * t);
                            spriteBatch.Draw(SK.texture_icon_times_started, SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 285), Color.White * t);

                            if(0 < FM.purchased[FM.Convert("highscore")]) if(Get_ScoreValid(current_row_main * 5 + current_point_main, 0)) spriteBatch.Draw(Get_Medal(current_row_main * 5 + current_point_main, 0), SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 10), Color.White * t);
                            if(0 < FM.purchased[FM.Convert("coins earned")]) if(Get_ScoreValid(current_row_main * 5 + current_point_main, 1)) spriteBatch.Draw(Get_Medal(current_row_main * 5 + current_point_main, 1), SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 110), Color.White * t);
                            if(0 < FM.purchased[FM.Convert("playtime")]) if(Get_ScoreValid(current_row_main * 5 + current_point_main, 2)) spriteBatch.Draw(Get_Medal(current_row_main * 5 + current_point_main, 2), SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 210), Color.White * t);
                            if(Get_ScoreValid(current_row_main * 5 + current_point_main, 3)) spriteBatch.Draw(Get_Medal(current_row_main * 5 + current_point_main, 3), SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(-50, 310), Color.White * t);

                            if(0 < FM.purchased[FM.Convert("highscore")]) spriteBatch.DrawString(SK.font_score, "Highscore:", SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 25), Color.Black * t);
                            if(0 < FM.purchased[FM.Convert("coins earned")]) spriteBatch.DrawString(SK.font_score, "Highest Amount of Coins earned:", SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 125), Color.Black * t);
                            if(0 < FM.purchased[FM.Convert("playtime")]) spriteBatch.DrawString(SK.font_score, "Total Amount of Time played:", SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 225), Color.Black * t);
                            spriteBatch.DrawString(SK.font_score, "Times played:", SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 325), Color.Black * t);

                            if(0 < FM.purchased[FM.Convert("highscore")]) spriteBatch.DrawString(SK.font_score, "" + FM.highscore[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 60), Color.Black * t);
                            if(0 < FM.purchased[FM.Convert("coins earned")]) spriteBatch.DrawString(SK.font_score, "" + FM.highcoin[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 160), Color.Black * t);
                            if(0 < FM.purchased[FM.Convert("playtime")]) spriteBatch.DrawString(SK.font_score, "" + FM.hightimeS[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 260), Color.Black * t);
                            spriteBatch.DrawString(SK.font_score, "" + FM.highplay[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge() + SK.Position_InfoText() + new Vector2(100, 360), Color.Black * t);
                        }
                    }
                }

                if(SK.orientation <= 2) {
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(0, 0), Color.White);
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(0, 704 - 50), Color.White);
                } else {
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(-80, 0), Color.White);
                    spriteBatch.Draw(SK.texture_menu_panel, SK.Position_DisplayEdge() + new Vector2(-80, 864 - 50), Color.White);
                }

                if(active_field == 0) if(Get_PurchasedMain(current_row_main * 5 + current_point_main)) Draw_Title(Get_GridName(current_row_main   * 5 + current_point_main),  SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2, SK.Position_DisplayEdge().Y + 9, Color.White);
                if(active_field == 5)                                                                  Draw_Title(Get_StoreName(current_row_store * 2 + current_point_store), SK.Position_DisplayEdge().X + SK.Get_GridSize().X / 2, SK.Position_DisplayEdge().Y + 9, Color.White);
                //if(active_field == 0) if(0 < FM.purchased[FM.Convert("highscore")]) if(Get_PurchasedMain(current_row_main * 5 + current_point_main)) spriteBatch.DrawString(SK.font_score, "Highscore: " + FM.highscore[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge() + new Vector2(SK.Get_GridSize().X / 10 + 25, SK.Get_GridSize().Y - 50 + 9), Color.White);
                if(active_field == 0) if(0 < FM.purchased[FM.Convert("highscore")]) if(Get_PurchasedMain(current_row_main * 5 + current_point_main)) Draw_Values("" + FM.highscore[current_row_main * 5 + current_point_main], SK.Position_DisplayEdge().X + 125, SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y - 40, Color.White, false);
                Draw_Values("" + (FM.coins - FM.coins_bonus), SK.Position_DisplayEdge().X + SK.Get_GridSize().X - 125, SK.Position_DisplayEdge().Y + SK.Get_GridSize().Y - 40, Color.Gold, true);
            }
        }

        public override void Draw3() {
            if(!FM.active_firststart) {
                if(FM.active_info && !FM.speed) { // Info Screen
                    spriteBatch.Draw(SK.texture_hud_button_no,          new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_yes,         new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 0) { // Main Screen
                    spriteBatch.Draw(SK.texture_hud_button_store1,      new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_options2,    new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 1) { // Options Screen
                    spriteBatch.Draw(SK.texture_hud_button_main1,       new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_statistics2, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                    //spriteBatch.Draw(SK.texture_hud_button_mino2,       new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 2) { // Mino Screen
                    spriteBatch.Draw(SK.texture_hud_button_options1,    new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_statistics2, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 3) { // Statistics Screen
                    spriteBatch.Draw(SK.texture_hud_button_options1, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                    //spriteBatch.Draw(SK.texture_hud_button_mino1,       new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    //spriteBatch.Draw(SK.texture_hud_button_info2,       new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_store2, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 4) { // Info Screen
                    spriteBatch.Draw(SK.texture_hud_button_statistics1, new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_store2,      new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                } else if(active_field == 5) { // Store Screen
                    spriteBatch.Draw(SK.texture_hud_button_statistics1, new Rectangle(SK.Position_Button(true), new Point(FM.button_scale)), Color.White);
                    //spriteBatch.Draw(SK.texture_hud_button_info1,       new Rectangle(SK.Position_Button(true),  new Point(FM.button_scale)), Color.White);
                    spriteBatch.Draw(SK.texture_hud_button_main2,       new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                }
            } else {
                if(active_infoside == 0) {
                    spriteBatch.Draw(SK.texture_hud_button_yes, new Rectangle(SK.Position_Button(false), new Point(FM.button_scale)), Color.White);
                }
            }
        }

        private Texture2D Get_GridIcon(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(i < FM.purchased.Length) if(0 < FM.purchased[i])
                    return SK.Get_GridIcon(i, 0);
            return SK.texture_menu_grid_empty;
        }

        private Texture2D Get_StoreIcon(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            return SK.Get_GridIcon(i, FM.purchased[i]);
        }

        private string Command_Options(int i) {
            switch(i) {
                /* Music  Down   */ case  0: if(FM.system_volume_music >   0)   FM.system_volume_music -= 10; FM.system_active_music = true; JK.Resume(); break;
                /* Music  Active */ case  1: if(FM.system_active_music      ) { FM.system_active_music = false; JK.Pause(); } else { FM.system_active_music = true; JK.Resume(); } break;
                /* Music  Up     */ case  2: if(FM.system_volume_music < 100)   FM.system_volume_music += 10; FM.system_active_music = true; JK.Resume(); break;
                /* Sound  Down   */ case  3: if(FM.system_volume_sound >   0)   FM.system_volume_sound -= 10; FM.system_active_sound = true; break;
                /* Sound  Active */ case  4: if(FM.system_active_sound      ) { FM.system_active_sound = false; } else { FM.system_active_sound = true; } break;
                /* Sound  Up     */ case  5: if(FM.system_volume_sound < 100)   FM.system_volume_sound += 10; FM.system_active_sound = true; break;
                /* Track  Prev   */ case  6: JK.Select_Track(0); FM.system_active_music = true; if(FM.system_volume_music == 0) FM.system_volume_music = 10; break;
                /* Track  Random */ case  7: JK.Select_Track(2); FM.system_active_music = true; if(FM.system_volume_music == 0) FM.system_volume_music = 10; break;
                /* Track  Next   */ case  8: JK.Select_Track(1); FM.system_active_music = true; if(FM.system_volume_music == 0) FM.system_volume_music = 10; break;
                /* Bright Down   */ case  9: if(FM.system_brightness > 0) FM.system_brightness -= 20; break;
                /* Bright        */ case 10: break;
                /* Bright Up     */ case 11: if(FM.system_brightness < 100) FM.system_brightness += 20; break;
                /* Align  Left   */ case 12: if( FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) if(FM.system_alignededge) FM.system_alignededge = false; break;
                /* Align  Ads    */ case 13: if(!FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) { FM.Contact_Store(1); return "MalGC_Werbung"; }         break;
                /* Align  Right  */ case 14: if( FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) if(!FM.system_alignededge) FM.system_alignededge = true; break;
                /* Backgr Prev   */ case 15: Change_BGround(0); break;
                /* Backgr Random */ case 16: Change_BGround(2); break;
                /* Backgr Next   */ case 17: Change_BGround(1); break;
            }
            JK.Apply_Volume();
            return "void";
        }

        private Texture2D Get_OptionIcon(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_options * 3;
            if(i >= SK.maxrows_options * 3) i = i - SK.maxrows_options * 3;
            switch(i) {
                /* Music  Down   */ case  0: if(FM.system_volume_music >   0) { return SK.texture_options_minus;    } else { return SK.texture_menu_grid_full;    }
                /* Music  Active */ case  1: if(FM.system_active_music      ) { return SK.texture_options_music_on; } else { return SK.texture_options_music_off; }
                /* Music  Up     */ case  2: if(FM.system_volume_music < 100) { return SK.texture_options_plus;     } else { return SK.texture_menu_grid_full;    }
                /* Sound  Down   */ case  3: if(FM.system_volume_sound >   0) { return SK.texture_options_minus;    } else { return SK.texture_menu_grid_full;    }
                /* Sound  Active */ case  4: if(FM.system_active_sound      ) { return SK.texture_options_sound_on; } else { return SK.texture_options_sound_off; }
                /* Sound  Up     */ case  5: if(FM.system_volume_sound < 100) { return SK.texture_options_plus;     } else { return SK.texture_menu_grid_full;    }
                /* Track  Prev   */ case  6: return SK.texture_options_previous;
                /* Track  Random */ case  7: return SK.texture_options_shuffle;
                /* Track  Next   */ case  8: return SK.texture_options_next;
                /* Bright Down   */ case  9: if(FM.system_brightness > 0) { return SK.texture_options_minus; } else { return SK.texture_menu_grid_full; }
                /* Bright        */ case 10: return SK.texture_options_brightness;
                /* Bright Up     */ case 11: if(FM.system_brightness < 100) { return SK.texture_options_plus; } else { return SK.texture_menu_grid_full; }
                /* Align  Left   */ case 12: if(FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) { if(FM.system_alignededge) { return SK.texture_options_align_left; } else { return SK.texture_menu_grid_full; } } else { return SK.texture_menu_grid_full; }
                /* Align  Ads    */ case 13: if(FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) { return SK.texture_options_alignment; } else { return SK.texture_options_werbung; }
                /* Align  Right  */ case 14: if(FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) { if(!FM.system_alignededge) { return SK.texture_options_align_right; } else { return SK.texture_menu_grid_full; } } else { return SK.texture_menu_grid_full; }
                /* Backgr Prev   */ case 15: return SK.texture_options_previous;
                /* Backgr Random */ case 16: return SK.texture_options_shuffle;
                /* Backgr Next   */ case 17: return SK.texture_options_next;
            }
            return SK.texture_menu_grid_empty;
        }

        private string Get_OptionText(int _i, bool headline) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_options;
            if(i >= SK.maxrows_options) i = i - SK.maxrows_options;
            switch(i) {
                case 0: if(headline) { return "Music"; } else { if(FM.system_active_music) { return "" + FM.system_volume_music; } else { return ""; } }
                case 1: if(headline) { return "Sound"; } else { if(FM.system_active_sound) { return "" + FM.system_volume_sound; } else { return ""; } }
                case 2: if(headline) { return "Track"; } else { if(FM.system_active_music) { return JK.Get_TrackName(); } else { return ""; } }
                case 3: if(headline) { return "Brightness"; } else { return "" + (int)(FM.system_brightness); }
                case 4: if(headline) { if(!FM.lizenz.ProductLicenses["MalGC_Werbung"].IsActive) { return "Remove Ads"; } else { return "Alignment"; } } else { return ""; }
                case 5: if(headline) { return "Background"; } else { return FM.BGround_name[FM.current_background]; }
            }
            return "VOID";
        }

        private bool PurchasedStats(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_stats;
            if(i >= SK.maxrows_stats) i = i - SK.maxrows_stats;
            switch(i) {
                /* Highscore 1   */ case  0: return 0 < FM.purchased[FM.Convert("highscore")];
                /* Highscore 2   */ case  1: return 0 < FM.purchased[FM.Convert("highscore")];
                /* Highscore 3   */ case  2: return 0 < FM.purchased[FM.Convert("highscore")];
                /* Total Coins   */ case  3: return 0 < FM.purchased[FM.Convert("coins earned")];
                /* HighCoins 1   */ case  4: return 0 < FM.purchased[FM.Convert("coins earned")];
                /* HighCoins 2   */ case  5: return 0 < FM.purchased[FM.Convert("coins earned")];
                /* HighCoins 3   */ case  6: return 0 < FM.purchased[FM.Convert("coins earned")];
                /* Total Time    */ case  7: return 0 < FM.purchased[FM.Convert("playtime")];
                /* Gametime 1    */ case  8: return 0 < FM.purchased[FM.Convert("playtime")];
                /* GameTime 2    */ case  9: return 0 < FM.purchased[FM.Convert("playtime")];
                /* GameTime 3    */ case 10: return 0 < FM.purchased[FM.Convert("playtime")];
                /* Total Minos   */ //case 11: return 0 < FM.purchased[FM.Convert("minos used")];
                /* Mino Used 1   */ //case 12: return 0 < FM.purchased[FM.Convert("minos used")];
                /* Mino Used 2   */ //case 13: return 0 < FM.purchased[FM.Convert("minos used")];
                /* Mino Used 3   */ //case 14: return 0 < FM.purchased[FM.Convert("minos used")];
                /* Times Started */ case 11: return true;
            }
            return false;
        }

        private Texture2D Get_StatsIcon(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_stats;
            if(i >= SK.maxrows_stats) i = i - SK.maxrows_stats;
            switch(i) {
                /* Highscore 1   */ case  0: return SK.texture_icon_highscore;
                /* Highscore 2   */ case  1: return SK.texture_icon_highscore;
                /* Highscore 3   */ case  2: return SK.texture_icon_highscore;
                /* Total Coins   */ case  3: return SK.texture_icon_coins;
                /* HighCoins 1   */ case  4: return SK.texture_icon_coins;
                /* HighCoins 2   */ case  5: return SK.texture_icon_coins;
                /* HighCoins 3   */ case  6: return SK.texture_icon_coins;
                /* Total Time    */ case  7: return SK.texture_icon_times_played;
                /* Gametime 1    */ case  8: return SK.texture_icon_times_played;
                /* GameTime 2    */ case  9: return SK.texture_icon_times_played;
                /* GameTime 3    */ case 10: return SK.texture_icon_times_played;
                /* Total Minos   */ //case 11: return SK.texture_icon_mino;
                /* Mino Used 1   */ //case 12: return SK.texture_icon_mino;
                /* Mino Used 2   */ //case 13: return SK.texture_icon_mino;
                /* Mino Used 3   */ //case 14: return SK.texture_icon_mino;
                /* Times Started */ case 11: return SK.texture_icon_times_started;
            }
            return SK.texture_menu_grid_empty;
        }

        private Texture2D Get_MinoIcon(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_minos;
            if(i >= SK.maxrows_minos) i = i - SK.maxrows_minos;
            switch(i) {
                /* Fire    */ case  0: return  SK.Get_GridIcon(-1/*FM.Convert("fire mino"),   */, FM.purchased[FM.Convert("fire mino")] < 2 ? 0 : 1);
                /* Air     */ case  1: return  SK.Get_GridIcon(-1/*FM.Convert("air mino"),    */, FM.purchased[FM.Convert("air mino")] < 2 ? 0 : 1);
                /* Thunder */ case  2: return  SK.Get_GridIcon(-1/*FM.Convert("thunder mino"),*/, FM.purchased[FM.Convert("thunder mino")] < 2 ? 0 : 1);
                /* Water   */ case  3: return  SK.Get_GridIcon(-1/*FM.Convert("water mino"),  */, FM.purchased[FM.Convert("water mino")] < 2 ? 0 : 1);
                /* Ice     */ case  4: return  SK.Get_GridIcon(-1/*FM.Convert("ice mino"),    */, FM.purchased[FM.Convert("ice mino")] < 2 ? 0 : 1);
                /* Earth   */ case  5: return  SK.Get_GridIcon(-1/*FM.Convert("earth mino"),  */, FM.purchased[FM.Convert("earth mino")] < 2 ? 0 : 1);
                /* Metal   */ case  6: return  SK.Get_GridIcon(-1/*FM.Convert("metal mino"),  */, FM.purchased[FM.Convert("metal mino")] < 2 ? 0 : 1);
                /* Nature  */ case  7: return  SK.Get_GridIcon(-1/*FM.Convert("nature mino"), */, FM.purchased[FM.Convert("nature mino")] < 2 ? 0 : 1);
                /* Light   */ case  8: return  SK.Get_GridIcon(-1/*FM.Convert("light mino"),  */, FM.purchased[FM.Convert("light mino")] < 2 ? 0 : 1);
                /* Dark    */ case  9: return  SK.Get_GridIcon(-1/*FM.Convert("dark mino"),   */, FM.purchased[FM.Convert("dark mino")] < 2 ? 0 : 1);
                /* Gold    */ case 10: return  SK.Get_GridIcon(-1/*FM.Convert("gold mino"),   */, FM.purchased[FM.Convert("gold mino")] < 2 ? 0 : 1);
            }
            return SK.texture_menu_grid_empty;
        }

        private Color Get_MinoColor(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_minos;
            if(i >= SK.maxrows_minos) i = i - SK.maxrows_minos;
            switch(i) {
                /* Fire    */ case  0: return FM.purchased[FM.Convert("fire mino")   ] == 0 ? Color.Black : Color.White;
                /* Air     */ case  1: return FM.purchased[FM.Convert("air mino")    ] == 0 ? Color.Black : Color.White;
                /* Thunder */ case  2: return FM.purchased[FM.Convert("thunder mino")] == 0 ? Color.Black : Color.White;
                /* Water   */ case  3: return FM.purchased[FM.Convert("water mino")  ] == 0 ? Color.Black : Color.White;
                /* Ice     */ case  4: return FM.purchased[FM.Convert("ice mino")    ] == 0 ? Color.Black : Color.White;
                /* Earth   */ case  5: return FM.purchased[FM.Convert("earth mino")  ] == 0 ? Color.Black : Color.White;
                /* Metal   */ case  6: return FM.purchased[FM.Convert("metal mino")  ] == 0 ? Color.Black : Color.White;
                /* Nature  */ case  7: return FM.purchased[FM.Convert("nature mino") ] == 0 ? Color.Black : Color.White;
                /* Light   */ case  8: return FM.purchased[FM.Convert("light mino")  ] == 0 ? Color.Black : Color.White;
                /* Dark    */ case  9: return FM.purchased[FM.Convert("dark mino")   ] == 0 ? Color.Black : Color.White;
                /* Gold    */ case 10: return FM.purchased[FM.Convert("gold mino")   ] == 0 ? Color.Black : Color.White;   
            }
            return Color.Black;
        }

        private Texture2D Get_StatsMedal(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_stats;
            if(i >= SK.maxrows_stats) i = i - SK.maxrows_stats;
            switch(i) {
                /* Highscore 1   */ case  0: return SK.texture_icon_medal1;
                /* Highscore 2   */ case  1: return SK.texture_icon_medal2;
                /* Highscore 3   */ case  2: return SK.texture_icon_medal3;
                /* Total Coins   */ case  3: return SK.texture_icon_coins;
                /* HighCoins 1   */ case  4: return SK.texture_icon_medal1;
                /* HighCoins 2   */ case  5: return SK.texture_icon_medal2;
                /* HighCoins 3   */ case  6: return SK.texture_icon_medal3;
                /* Total Time    */ case  7: return SK.texture_icon_times_played;
                /* Gametime 1    */ case  8: return SK.texture_icon_medal1;
                /* GameTime 2    */ case  9: return SK.texture_icon_medal2;
                /* GameTime 3    */ case 10: return SK.texture_icon_medal3;
                /* Total Minos   */ //case 11: return SK.texture_icon_mino;
                /* Mino Used 1   */ //case 12: return SK.texture_icon_medal1;
                /* Mino Used 2   */ //case 13: return SK.texture_icon_medal2;
                /* Mino Used 3   */ //case 14: return SK.texture_icon_medal3;
                /* Times Started */ case 11: return SK.texture_icon_times_started;
            }
            return SK.texture_menu_grid_empty;
        }

        private Texture2D Get_StatsGame(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_stats;
            if(i >= SK.maxrows_stats) i = i - SK.maxrows_stats;
            switch(i) {
                /* Highscore 1   */ case  0: return SK.Get_GridIcon(FM.top_highscore[0], 0);
                /* Highscore 2   */ case  1: return SK.Get_GridIcon(FM.top_highscore[1], 0);
                /* Highscore 3   */ case  2: return SK.Get_GridIcon(FM.top_highscore[2], 0);
                /* Total Coins   */ case  3: return SK.texture_menu_grid_empty;
                /* HighCoins 1   */ case  4: return SK.Get_GridIcon(FM.top_highcoin[0], 0);
                /* HighCoins 2   */ case  5: return SK.Get_GridIcon(FM.top_highcoin[1], 0);
                /* HighCoins 3   */ case  6: return SK.Get_GridIcon(FM.top_highcoin[2], 0);
                /* Total Time    */ case  7: return SK.texture_menu_grid_empty;
                /* Gametime 1    */ case  8: return SK.Get_GridIcon(FM.top_hightime[0], 0);
                /* GameTime 2    */ case  9: return SK.Get_GridIcon(FM.top_hightime[1], 0);
                /* GameTime 3    */ case 10: return SK.Get_GridIcon(FM.top_hightime[2], 0);
                /* Total Minos   */ //case 11: return SK.texture_menu_grid_empty;
                /* Mino Used 1   */ //case 12: return SK.Get_GridIcon(FM.top_highmino[0] + 64, 0);
                /* Mino Used 2   */ //case 13: return SK.Get_GridIcon(FM.top_highmino[1] + 64, 0);
                /* Mino Used 3   */ //case 14: return SK.Get_GridIcon(FM.top_highmino[2] + 64, 0);
                /* Times Started */ case 11: return SK.texture_menu_grid_empty;
            }
            return SK.texture_menu_grid_empty;
        }

        private string Get_StatsText(int _i, bool headline) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_stats;
            if(i >= SK.maxrows_stats) i = i - SK.maxrows_stats;
            switch(i) {
                /* Highscore 1   */ case  0: if(headline) { return "Highscore 1st Place";        } else { return "" + FM.highscore[FM.top_highscore[0]]; }
                /* Highscore 2   */ case  1: if(headline) { return "Highscore 2nd Place";        } else { return "" + FM.highscore[FM.top_highscore[1]]; }
                /* Highscore 3   */ case  2: if(headline) { return "Highscore 3rd Place";        } else { return "" + FM.highscore[FM.top_highscore[2]]; }
                /* Total Coins   */ case  3: if(headline) { return "Total Coins Collected";      } else { return "" + FM.stats_coinscollected; }
                /* HighCoins 1   */ case  4: if(headline) { return "Highest Coins 1st Place";    } else { return "" + FM.highcoin[FM.top_highcoin[0]]; }
                /* HighCoins 2   */ case  5: if(headline) { return "Highest Coins 2nd Place";    } else { return "" + FM.highcoin[FM.top_highcoin[1]]; }
                /* HighCoins 3   */ case  6: if(headline) { return "Highest Coins 3rd Place";    } else { return "" + FM.highcoin[FM.top_highcoin[2]]; }
                /* Total Time    */ case  7: if(headline) { return "Total Time Played";          } else { return "" + FM.stats_totalplaytimeS; }
                /* Gametime 1    */ case  8: if(headline) { return "Most Time Played 1st Place"; } else { return "" + FM.hightimeS[FM.top_hightime[0]]; }
                /* GameTime 2    */ case  9: if(headline) { return "Most Time Played 2nd Place"; } else { return "" + FM.hightimeS[FM.top_hightime[1]]; }
                /* GameTime 3    */ case 10: if(headline) { return "Most Time Played 3rd Place"; } else { return "" + FM.hightimeS[FM.top_hightime[2]]; }
                /* Total Minos   */ //case 11: if(headline) { return "Total Used Minos";           } else { return "" + FM.stats_totalminosused; }
                /* Mino Used 1   */ //case 12: if(headline) { return "Most Used Mino 1st Place";   } else { return "" + FM.mino_used[FM.top_highmino[0]]; }
                /* Mino Used 2   */ //case 13: if(headline) { return "Most Used Mino 2nd Place";   } else { return "" + FM.mino_used[FM.top_highmino[1]]; }
                /* Mino Used 3   */ //case 14: if(headline) { return "Most Used Mino 3rd Place";   } else { return "" + FM.mino_used[FM.top_highmino[2]]; }
                /* Times Started */ case 11: if(headline) { return "Times This App Started";     } else { return "" + FM.stats_timesstarted; }
            }
            return "VOID";
        }

        private string Get_InfoText(int _i, bool headline) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_info;
            if(i >= SK.maxrows_info) i = i - SK.maxrows_info;
            switch(i) {
                /* ??? */ case  0: if(headline) { return "INFO BLOCK 01"; } else { return ""; }
                /* ??? */ case  1: if(headline) { return "INFO BLOCK 02"; } else { return ""; }
                /* ??? */ case  2: if(headline) { return "INFO BLOCK 03"; } else { return ""; }
                /* ??? */ case  3: if(headline) { return "INFO BLOCK 04"; } else { return ""; }
                /* ??? */ case  4: if(headline) { return "INFO BLOCK 05"; } else { return ""; }
                /* ??? */ case  5: if(headline) { return "INFO BLOCK 06"; } else { return ""; }
                /* ??? */ case  6: if(headline) { return "INFO BLOCK 07"; } else { return ""; }
                /* ??? */ case  7: if(headline) { return "INFO BLOCK 08"; } else { return ""; }
                /* ??? */ case  8: if(headline) { return "INFO BLOCK 09"; } else { return ""; }
                /* ??? */ case  9: if(headline) { return "INFO BLOCK 10"; } else { return ""; }
                /* ??? */ case 10: if(headline) { return "INFO BLOCK 11"; } else { return ""; }
                /* ??? */ case 11: if(headline) { return "INFO BLOCK 12"; } else { return ""; }
                /* ??? */ case 12: if(headline) { return "INFO BLOCK 13"; } else { return ""; }
                /* ??? */ case 13: if(headline) { return "INFO BLOCK 14"; } else { return ""; }
                /* ??? */ case 14: if(headline) { return "INFO BLOCK 15"; } else { return ""; }
                /* ??? */ case 15: if(headline) { return "INFO BLOCK 16"; } else { return ""; }
            }
            return "VOID";
        }

        private string Get_GridName(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            return FM.name[i];
        }

        private Texture2D Get_GridTexture(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(i < FM.purchased.Length) if(0 < FM.purchased[i])
                    return SK.texture_menu_grid_full;
            return SK.texture_menu_grid_empty;
        }

        private int Get_Price(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            if(i < FM.purchased.Length)
                return FM.price[i];
            return 0;
        }

        private bool Get_PurchasedStore(int _i) { // Return if the Store item is completely purchased
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            if(i < FM.purchased.Length) {
                if(FM.name[i] == "octalive") return 3 < FM.purchased[i];

                if(FM.name[i] == "fire mino")    return 1 < FM.purchased[i];
                if(FM.name[i] == "air mino")     return 1 < FM.purchased[i];
                if(FM.name[i] == "thunder mino") return 1 < FM.purchased[i];
                if(FM.name[i] == "water mino")   return 1 < FM.purchased[i];
                if(FM.name[i] == "ice mino")     return 1 < FM.purchased[i];
                if(FM.name[i] == "earth mino")   return 1 < FM.purchased[i];
                if(FM.name[i] == "metal mino")   return 1 < FM.purchased[i];
                if(FM.name[i] == "nature mino")  return 1 < FM.purchased[i];
                if(FM.name[i] == "light mino")   return 1 < FM.purchased[i];
                if(FM.name[i] == "dark mino")    return 1 < FM.purchased[i];
                if(FM.name[i] == "gold mino")    return 1 < FM.purchased[i];
                return 0 < FM.purchased[i];
            }
            return false;
        }

        private bool Get_PurchasedMain(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(i < FM.highscore.Length)
                return 0 < FM.purchased[i];
            return false;
        }

        private Color Get_GridColor(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5)
                return Color.Gray;
            if(i < FM.highscore.Length) {
                if(FM.type[i] == 0) return new Color(198, 169, 226);
                if(FM.type[i] == 1) return new Color(162, 193, 223);
                if(FM.type[i] == 2) return new Color(164, 224, 164);
                if(FM.type[i] == 3) return new Color(226, 226, 169);
                if(FM.type[i] == 4) return new Color(225, 168, 168);
                if(FM.type[i] == 5) return Color.LightSlateGray;
                if(FM.type[i] == 6) return Color.PaleTurquoise;
                if(FM.type[i] == 7) return Color.LightGray;
                if(FM.type[i] == 8) return Color.White;
            }
            return Color.Black;
        }

        private string Get_StoreName(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            return FM.name[i];
        }

        private Color Get_StoreColor(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            if(i < FM.purchased.Length) {
                if(FM.type[i] == 0) return new Color(198, 169, 226);
                if(FM.type[i] == 1) return new Color(162, 193, 223);
                if(FM.type[i] == 2) return new Color(164, 224, 164);
                if(FM.type[i] == 3) return new Color(226, 226, 169);
                if(FM.type[i] == 4) return new Color(225, 168, 168);
                if(FM.type[i] == 5) return Color.LightSlateGray;
                if(FM.type[i] == 6) return Color.PaleTurquoise;
                if(FM.type[i] == 7) return Color.LightGray;
                if(FM.type[i] == 8) return Color.White;
            }
            return Color.Black;
        }

        private bool Get_StoreValid(int _i) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_store * 2;
            if(i >= SK.maxrows_store * 2) i = i - SK.maxrows_store * 2;
            if(i < FM.purchased.Length)
                return true;
            return false;
        }



        private void Change_BGround(int i) {
            if(i == 0) { // Previous BG
                FM.current_background--;
                if(FM.current_background < 0) FM.current_background = FM.BGround_active.Length - 1;
                while(!FM.BGround_active[FM.current_background]) {
                    FM.current_background--;
                    if(FM.current_background < 0) FM.current_background = FM.BGround_active.Length - 1;
                }
            }
            if(i == 1) { // Next BG
                FM.current_background++;
                if(FM.current_background == FM.BGround_active.Length) FM.current_background = 0;
                while(!FM.BGround_active[FM.current_background]) {
                    FM.current_background++;
                    if(FM.current_background == FM.BGround_active.Length) FM.current_background = 0;
                }
            }
            if(i == 2) { // Random BG
                FM.current_background = random.Next(FM.BGround_active.Length);
                while(!FM.BGround_active[FM.current_background]) {
                    FM.current_background = random.Next(FM.BGround_active.Length);
                }
            }
        }

        private bool Get_ScoreValid(int _i, int id) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(id == 0) { if(FM.top_highscore[0] == i || FM.top_highscore[1] == i || FM.top_highscore[2] == i) return true; }
            if(id == 1) { if(FM.top_highcoin [0] == i || FM.top_highcoin [1] == i || FM.top_highcoin [2] == i) return true; }
            if(id == 2) { if(FM.top_hightime [0] == i || FM.top_hightime [1] == i || FM.top_hightime [2] == i) return true; }
            if(id == 3) { if(FM.top_highplay [0] == i || FM.top_highplay [1] == i || FM.top_highplay [2] == i) return true; }
            return false;
        }

        private Texture2D Get_Medal(int _i, int id) {
            int i = _i;
            if(i < 0) i = i + SK.maxrows_main * 5;
            if(i >= SK.maxrows_main * 5) i = i - SK.maxrows_main * 5;
            if(id == 0) {
                if(FM.top_highscore[0] == i) return SK.texture_icon_medal1;
                if(FM.top_highscore[1] == i) return SK.texture_icon_medal2;
                if(FM.top_highscore[2] == i) return SK.texture_icon_medal3;
            }
            if(id == 1) {
                if(FM.top_highcoin[0] == i) return SK.texture_icon_medal1;
                if(FM.top_highcoin[1] == i) return SK.texture_icon_medal2;
                if(FM.top_highcoin[2] == i) return SK.texture_icon_medal3;
            }
            if(id == 2) {
                if(FM.top_hightime[0] == i) return SK.texture_icon_medal1;
                if(FM.top_hightime[1] == i) return SK.texture_icon_medal2;
                if(FM.top_hightime[2] == i) return SK.texture_icon_medal3;
            }
            if(id == 3) {
                if(FM.top_highplay[0] == i) return SK.texture_icon_medal1;
                if(FM.top_highplay[1] == i) return SK.texture_icon_medal2;
                if(FM.top_highplay[2] == i) return SK.texture_icon_medal3;
            }
            if(id == 4) {
                if(FM.top_highmino[0] == i) return SK.texture_icon_medal1;
                if(FM.top_highmino[1] == i) return SK.texture_icon_medal2;
                if(FM.top_highmino[2] == i) return SK.texture_icon_medal3;
            }
            return SK.texture_menu_grid_empty;
        }

    }
}
