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
    class ShopKeeper {

        FileManager FM;

        public SpriteFont font_score;

        public int orientation = 0;

        public int frame_max     = 8;
        public int frame_stopper = 8;

        public int maxrows_main    =  5;
        public int maxrows_options =  6;
        public int maxrows_store   = 18;
        public int maxrows_stats   = 12;
        public int maxrows_minos   = 11;
        public int maxrows_info    =  1;

        public List<string>[] info_firststart  = new List<string>[2];

        public string[] info_store = new string[36];

        public List<string>[] info_description  = new List<string>[25];
        public List<int>   [] info_control_icon = new List<int>   [25];
        public List<string>[] info_control_text = new List<string>[25];

        public Vector2 screensize_main = new Vector2(1280, 720);

        public Texture2D texture_background_casing1;
        public Texture2D texture_background_casing2;
        public Texture2D texture_background_dropshadow;
        public Texture2D texture_background_spacegrid;
        public Texture2D texture_background_countryside;
        public Texture2D texture_background_info;
        public Texture2D texture_background_logo_title;
        public Texture2D texture_background_logo_start;
        public Texture2D texture_background_tetris;
        public Texture2D texture_background_2048;
        public Texture2D texture_background_gameover;
        public Texture2D texture_background_pause;
        public Texture2D texture_background_roulette;
        public Texture2D texture_background_slotmachine1;
        public Texture2D texture_background_slotmachine2;
        public Texture2D texture_background_canvas;
        public Texture2D texture_background_grid32;
        public Texture2D texture_background_grid64;
        public Texture2D texture_background_sudoku;
        public Texture2D texture_background_clouds;
        public Texture2D texture_background_mountains;
        public Texture2D texture_background_industry;

        public Texture2D texture_hud_button_pad;
        public Texture2D texture_hud_button_empty_left;
        public Texture2D texture_hud_button_empty_right;
        public Texture2D texture_hud_button_yes;
        public Texture2D texture_hud_button_no;
        public Texture2D texture_hud_button_back;
        public Texture2D texture_hud_button_info1;
        public Texture2D texture_hud_button_info2;
        public Texture2D texture_hud_button_main1;
        public Texture2D texture_hud_button_main2;
        public Texture2D texture_hud_button_mino1;
        public Texture2D texture_hud_button_mino2;
        public Texture2D texture_hud_button_options1;
        public Texture2D texture_hud_button_options2;
        public Texture2D texture_hud_button_statistics1;
        public Texture2D texture_hud_button_statistics2;
        public Texture2D texture_hud_button_store1;
        public Texture2D texture_hud_button_store2;
        public Texture2D texture_hud_button_flag;
        public Texture2D texture_hud_button_pause;
        public Texture2D texture_hud_button_questionmark;
        public Texture2D texture_hud_button_replay;
        public Texture2D texture_hud_button_resume;
        public Texture2D texture_hud_button_select;
        public Texture2D texture_hud_button_spin;
        public Texture2D texture_highlight_button_left;
        public Texture2D texture_highlight_button_right;
        public Texture2D texture_highlight_arrow_up;
        public Texture2D texture_highlight_arrow_left;
        public Texture2D texture_highlight_arrow_right;
        public Texture2D texture_highlight_arrow_down;
        public Texture2D texture_colormap;
        public Texture2D texture_menu_grid_empty;
        public Texture2D texture_menu_grid_full;
        public Texture2D texture_menu_grid_selector;
        public Texture2D texture_menu_grid_options;
        public Texture2D texture_menu_grid_store;
        public Texture2D texture_menu_grid_statistics;
        public Texture2D texture_menu_panel;
        public Texture2D texture_menu_info_board;
        public Texture2D texture_menu_info_left;
        public Texture2D texture_menu_info_right;
        public Texture2D texture_menu_info_difficulty;
        public Texture2D texture_menu_info_easy;
        public Texture2D texture_menu_info_medium;
        public Texture2D texture_menu_info_hard;
        public Texture2D texture_menu_info_levelselect;
        public Texture2D texture_menu_info_startgame;

        public Texture2D texture_casino_bet_down;
        public Texture2D texture_casino_bet_grid;
        public Texture2D texture_casino_bet_left;
        public Texture2D texture_casino_bet_minus;
        public Texture2D texture_casino_bet_plus;
        public Texture2D texture_casino_bet_right;
        public Texture2D texture_casino_bet_up;

        public Texture2D texture_spritesheet_2048;
        public Texture2D texture_spritesheet_cards;
        public Texture2D texture_spritesheet_coin;
        public Texture2D texture_spritesheet_explosion;
        public Texture2D texture_spritesheet_lamps;
        public Texture2D texture_spritesheet_minesweeper;
        public Texture2D texture_spritesheet_octanom_head;
        public Texture2D texture_spritesheet_octanom_tail;
        public Texture2D texture_spritesheet_slotmachine;
        public Texture2D texture_spritesheet_sokoban;
        public Texture2D texture_spritesheet_trianom;
        public Texture2D texture_spritesheet_octagames;
        public Texture2D texture_spritesheet_mysticsquare;
        public Texture2D texture_spritesheet_octacubes;
        public Texture2D texture_spritesheet_sudoku;
        public Texture2D texture_spritesheet_minos_32x;
        public Texture2D texture_spritesheet_minos_64x;
        public Texture2D texture_spritesheet_font;
        public Texture2D texture_spritesheet_values;

        public Texture2D texture_static_blackjack_hit;
        public Texture2D texture_static_blackjack_stand;
        public Texture2D texture_static_roulette_ball;
        public Texture2D texture_static_roulette_chip;
        public Texture2D texture_static_roulette_minus1;
        public Texture2D texture_static_roulette_minus2;
        public Texture2D texture_static_roulette_plus1;
        public Texture2D texture_static_roulette_plus2;
        public Texture2D texture_static_roulette_wheel;
        public Texture2D texture_static_rpsls_lizard;
        public Texture2D texture_static_rpsls_paper;
        public Texture2D texture_static_rpsls_rock;
        public Texture2D texture_static_rpsls_scissor;
        public Texture2D texture_static_rpsls_spock;
        public Texture2D texture_static_slotmachine_line1;
        public Texture2D texture_static_slotmachine_line2;
        public Texture2D texture_static_slotmachine_line3;
        public Texture2D texture_static_slotmachine_line4;
        public Texture2D texture_static_slotmachine_line5;
        public Texture2D texture_static_slotmachine_point1;
        public Texture2D texture_static_slotmachine_point2;
        public Texture2D texture_static_slotmachine_point3;
        public Texture2D texture_static_slotmachine_point4;
        public Texture2D texture_static_slotmachine_point5;
        public Texture2D texture_static_tetris_next;
        public Texture2D texture_static_tetris_hold;
        public Texture2D texture_static_tetris_left;
        public Texture2D texture_static_tetris_right;
        public Texture2D texture_static_tictactoe_circle;
        public Texture2D texture_static_tictactoe_cross;
        public Texture2D texture_static_tictactoe_line;
        public Texture2D texture_static_videopoker_hold;
        public Texture2D texture_static_simon_console;
        public Texture2D texture_static_simon_red;
        public Texture2D texture_static_simon_blue;
        public Texture2D texture_static_simon_green;
        public Texture2D texture_static_simon_yellow;
        public Texture2D texture_static_boulder;

        Texture2D texture_icon_octatravels;
        Texture2D texture_icon_octanom;
        Texture2D texture_icon_snake;
        Texture2D texture_icon_octacubes;
        Texture2D texture_icon_sokoban;
        Texture2D texture_icon_octapow;
        Texture2D texture_icon_bouldernom;
        Texture2D texture_icon_octabattle;
        Texture2D texture_icon_arkanoid;
        Texture2D texture_icon_spaceinvader;
        Texture2D texture_icon_octajump;
        Texture2D texture_icon_octaflight;
        Texture2D texture_icon_bombuzal;
        Texture2D texture_icon_elements;
        Texture2D texture_icon_octapanic;
        Texture2D texture_icon_tetris;
        Texture2D texture_icon_columns;
        Texture2D texture_icon_meanminos;
        Texture2D texture_icon_wickedminos;
        Texture2D texture_icon_minogarden;
        Texture2D texture_icon_matchmakers;
        Texture2D texture_icon_memory;
        Texture2D texture_icon_8colors;
        Texture2D texture_icon_swing;
        Texture2D texture_icon_mastermind;
        Texture2D texture_icon_2048;
        Texture2D texture_icon_minesweeper;
        Texture2D texture_icon_tictactoe;
        Texture2D texture_icon_rpsls;
        Texture2D texture_icon_simon;
        Texture2D texture_icon_picross;
        Texture2D texture_icon_halma;
        Texture2D texture_icon_circuits;
        Texture2D texture_icon_mysticsquare;
        Texture2D texture_icon_sudoku;
        Texture2D texture_icon_locomotion;
        Texture2D texture_icon_pipemania;
        Texture2D texture_icon_lazerlight;
        Texture2D texture_icon_splitball;
        Texture2D texture_icon_blackhole;
        Texture2D texture_icon_spider;
        Texture2D texture_icon_pyramid;
        Texture2D texture_icon_klondike;
        Texture2D texture_icon_tripeak;
        Texture2D texture_icon_freecell;
        Texture2D texture_icon_blackjack;
        Texture2D texture_icon_baccarat;
        Texture2D texture_icon_videopoker;
        Texture2D texture_icon_slotmachine;
        Texture2D texture_icon_roulette;
        Texture2D texture_icon_rougeetnoir;
        Texture2D texture_icon_aceydeucey;
        Texture2D texture_icon_balotangkas;
        Texture2D texture_icon_craps;
        Texture2D texture_icon_sicbo;
        Texture2D texture_icon_fantan;
        Texture2D texture_icon_paigow;
        Texture2D texture_icon_keno;
        Texture2D texture_icon_tiengow;
        Texture2D texture_icon_yacht;
        Texture2D texture_icon_select;
        Texture2D texture_icon_easy;
        Texture2D texture_icon_hard;
        Texture2D texture_icon_hold;
        Texture2D texture_icon_highroller;
        Texture2D texture_icon_octalive1;
        Texture2D texture_icon_octalive2;
        Texture2D texture_icon_octalive3;
        Texture2D texture_icon_octalive4;
        Texture2D texture_icon_mino_fire_basic;
        Texture2D texture_icon_mino_fire_power;
        Texture2D texture_icon_mino_air_basic;
        Texture2D texture_icon_mino_air_power;
        Texture2D texture_icon_mino_thunder_basic;
        Texture2D texture_icon_mino_thunder_power;
        Texture2D texture_icon_mino_water_basic;
        Texture2D texture_icon_mino_water_power;
        Texture2D texture_icon_mino_ice_basic;
        Texture2D texture_icon_mino_ice_power;
        Texture2D texture_icon_mino_earth_basic;
        Texture2D texture_icon_mino_earth_power;
        Texture2D texture_icon_mino_metal_basic;
        Texture2D texture_icon_mino_metal_power;
        Texture2D texture_icon_mino_nature_basic;
        Texture2D texture_icon_mino_nature_power;
        Texture2D texture_icon_mino_light_basic;
        Texture2D texture_icon_mino_light_power;
        Texture2D texture_icon_mino_dark_basic;
        Texture2D texture_icon_mino_dark_power;
        Texture2D texture_icon_mino_gold_basic;
        Texture2D texture_icon_mino_gold_power;
        Texture2D texture_icon_clouds;
        Texture2D texture_icon_mountains;
        Texture2D texture_icon_industry;
        Texture2D texture_icon_musicpack1;
        Texture2D texture_icon_musicpack2;
        Texture2D texture_icon_musicpack3;
        Texture2D texture_icon_info;

        public Texture2D texture_icon_highscore;
        public Texture2D texture_icon_coins;
        public Texture2D texture_icon_times_played;
        public Texture2D texture_icon_times_started;
        public Texture2D texture_icon_mino;
        public Texture2D texture_icon_medal1;
        public Texture2D texture_icon_medal2;
        public Texture2D texture_icon_medal3;

        Texture2D texture_control_touch;
        Texture2D texture_control_pad;
        Texture2D texture_control_enter;
        Texture2D texture_control_pause;
        Texture2D texture_control_minesweeper;
        Texture2D texture_control_questionmark;
        Texture2D texture_control_hold;
        Texture2D texture_control_turnleft;
        Texture2D texture_control_turnright;

        public Texture2D texture_options_plus;
        public Texture2D texture_options_minus;
        public Texture2D texture_options_music_on;
        public Texture2D texture_options_music_off;
        public Texture2D texture_options_sound_on;
        public Texture2D texture_options_sound_off;
        public Texture2D texture_options_next;
        public Texture2D texture_options_previous;
        public Texture2D texture_options_shuffle;
        public Texture2D texture_options_brightness;
        public Texture2D texture_options_werbung;
        public Texture2D texture_options_alignment;
        public Texture2D texture_options_align_left;
        public Texture2D texture_options_align_right;

        public ShopKeeper(ContentManager c) {
            Load_Textures(c);
            Load_Lists();
        }

        public void Add_FileManager(FileManager fm) {
            FM = fm;
        }

        public Texture2D Get_GridIcon(int id, int stage) {
            switch(id) {
                case -1: return texture_icon_info;
                case  0: return texture_icon_octatravels;
                case  1: return texture_icon_octanom;
                case  2: return texture_icon_snake;
                case  3: return texture_icon_octacubes;
                case  4: return texture_icon_sokoban;
                //case  5: return texture_icon_octapow;
                //case  6: return texture_icon_bouldernom;
                //case  7: return texture_icon_octabattle;
                //case  8: return texture_icon_arkanoid;
                //case  9: return texture_icon_spaceinvader;
                //case 10: return texture_icon_octajump;
                //case 11: return texture_icon_octaflight;
                //case 12: return texture_icon_bombuzal;
                //case 13: return texture_icon_elements;
                //case 14: return texture_icon_octapanic;
                case  5: return texture_icon_tetris;
                case  6: return texture_icon_columns;
                case  7: return texture_icon_meanminos;
                //case 18: return texture_icon_wickedminos;
                //case 19: return texture_icon_minogarden;
                //case 20: return texture_icon_matchmakers;
                case  8: return texture_icon_memory;
                //case 22: return texture_icon_8colors;
                //case 23: return texture_icon_swing;
                case  9: return texture_icon_mastermind;
                case 10: return texture_icon_2048;
                case 11: return texture_icon_minesweeper;
                case 12: return texture_icon_tictactoe;
                case 13: return texture_icon_rpsls;
                case 14: return texture_icon_simon;
                //case 30: return texture_icon_picross;
                //case 31: return texture_icon_halma;
                //case 32: return texture_icon_circuits;
                //case 33: return texture_icon_mysticsquare;
                //case 34: return texture_icon_sudoku;
                //case 35: return texture_icon_locomotion;
                //case 36: return texture_icon_pipemania;
                //case 37: return texture_icon_lazerlight;
                //case 38: return texture_icon_splitball;
                //case 39: return texture_icon_blackhole;
                case 15: return texture_icon_klondike;
                case 16: return texture_icon_tripeak;
                case 17: return texture_icon_spider;
                case 18: return texture_icon_pyramid;
                case 19: return texture_icon_freecell;
                case 20: return texture_icon_blackjack;
                case 21: return texture_icon_baccarat;
                case 22: return texture_icon_videopoker;
                case 23: return texture_icon_slotmachine;
                case 24: return texture_icon_roulette;
                //case 50: return texture_icon_rougeetnoir;
                //case 51: return texture_icon_aceydeucey;
                //case 52: return texture_icon_balotangkas;
                //case 53: return texture_icon_craps;
                //case 54: return texture_icon_sicbo;
                //case 55: return texture_icon_fantan;
                //case 56: return texture_icon_paigow;
                //case 57: return texture_icon_keno;
                //case 58: return texture_icon_tiengow;
                //case 59: return texture_icon_yacht;
                //case 60: return texture_icon_select;
                case 25: return texture_icon_easy;
                case 26: return texture_icon_hard;
                case 27: return texture_icon_hold;
                case 28: return texture_icon_highroller;
                case 29:
                    if(stage == 0) return texture_icon_octalive1;
                    if(stage == 1) return texture_icon_octalive2;
                    if(stage == 2) return texture_icon_octalive3;
                    if(stage >= 3) return texture_icon_octalive4; break;
                //case 66:
                //    if(stage == 0) return texture_icon_mino_fire_basic;
                //    if(stage >= 1) return texture_icon_mino_fire_power; break;
                //case 67:
                //    if(stage == 0) return texture_icon_mino_air_basic;
                //    if(stage >= 1) return texture_icon_mino_air_power; break;
                //case 68:
                //    if(stage == 0) return texture_icon_mino_thunder_basic;
                //    if(stage >= 1) return texture_icon_mino_thunder_power; break;
                //case 69:
                //    if(stage == 0) return texture_icon_mino_water_basic;
                //    if(stage >= 1) return texture_icon_mino_water_power; break;
                //case 70:
                //    if(stage == 0) return texture_icon_mino_ice_basic;
                //    if(stage >= 1) return texture_icon_mino_ice_power; break;
                //case 71:
                //    if(stage == 0) return texture_icon_mino_earth_basic;
                //    if(stage >= 1) return texture_icon_mino_earth_power; break;
                //case 72:
                //    if(stage == 0) return texture_icon_mino_metal_basic;
                //    if(stage >= 1) return texture_icon_mino_metal_power; break;
                //case 73:
                //    if(stage == 0) return texture_icon_mino_nature_basic;
                //    if(stage >= 1) return texture_icon_mino_nature_power; break;
                //case 74:
                //    if(stage == 0) return texture_icon_mino_light_basic;
                //    if(stage >= 1) return texture_icon_mino_light_power; break;
                //case 75:
                //    if(stage == 0) return texture_icon_mino_dark_basic;
                //    if(stage >= 1) return texture_icon_mino_dark_power; break;
                //case 76:
                //    if(stage == 0) return texture_icon_mino_gold_basic;
                //    if(stage >= 1) return texture_icon_mino_gold_power; break;
                case 30: return texture_icon_clouds;
                case 31: return texture_icon_mountains;
                //case 79: return texture_icon_industry;
                case 32: return texture_icon_musicpack1;
                //case 81: return texture_icon_musicpack2;
                //case 82: return texture_icon_musicpack3;
                case 33: return texture_icon_highscore;
                case 34: return texture_icon_coins;
                case 35: return texture_icon_times_played;
                //case 86: return texture_icon_mino;
            }
            return texture_menu_grid_empty;
        }

        public Texture2D Get_ControlIcon(int i) {
            switch(i) {
                case 0: return texture_control_touch;
                case 1: return texture_control_pad;
                case 2: return texture_control_enter;
                case 3: return texture_control_pause;
                case 4: return texture_control_questionmark;
                case 5: return texture_control_minesweeper;
                case 6: return texture_control_hold;
                case 7: return texture_control_turnleft;
                case 8: return texture_control_turnright;
            }
            return texture_control_pad;
        }

        public Vector2 Position_DisplayEdge() {
            switch(orientation) {
                case 1: if(!FM.system_alignededge) { return new Vector2(308, 8); } else { return new Vector2(408, 8); }  // Landscape
                case 2: if(!FM.system_alignededge) { return new Vector2(108, 8); } else { return new Vector2(8, 8); }  // Landscape Inverted
                case 3: if(!FM.system_alignededge) { return new Vector2(8, 108); } else { return new Vector2(8, 8); }  // Portrait
                case 4: if(!FM.system_alignededge) { return new Vector2(8, 108); } else { return new Vector2(8, 8); }  // Portrait Inverted
            }
            return new Vector2(0, 0);
        }

        public Point Position_Pad() {
            switch(orientation) {
                case 1: return new Point(0, 720 - FM.button_scale);
                case 2: return new Point(1280 - FM.button_scale, 720 - FM.button_scale);
                case 3: return new Point(0, 1280 - FM.button_scale);
                case 4: return new Point(720 - FM.button_scale, 1280 - FM.button_scale);
            }
            return new Point(0, 0);
        }

        public Point Position_Button(bool leftORright) {
            if(leftORright) {
                switch(orientation) {
                    case 1: return new Point(0, 720 - FM.button_scale - FM.button_scale / 2);
                    case 2: return new Point(1280 - FM.button_scale, 720 - FM.button_scale - FM.button_scale / 2);
                    case 3: return new Point(720 - FM.button_scale / 2, 1280 - FM.button_scale);
                    case 4: return new Point(0, 1280 - FM.button_scale);
                }
            } else {
                switch(orientation) {
                    case 1: return new Point(0, 720 - FM.button_scale - FM.button_scale / 2);
                    case 2: return new Point(1280 - FM.button_scale, 720 - FM.button_scale - FM.button_scale / 2);
                    case 3: return new Point(720 - FM.button_scale, 1280 - FM.button_scale / 2);
                    case 4: return new Point(-FM.button_scale / 2, 1280 - FM.button_scale / 2);
                }
            }
            return new Point(0, 0);
        }

        /// <summary>
        /// For the next Row of Grid Tiles
        /// X - Shift to the Left
        /// Y - Shift Down
        /// Z - Shift Up
        /// </summary>
        public Vector3 position_grid_row    = new Vector3(69, 121, 120);
        public int     position_grid_next   = 138;                   // For Grid Tile directly to the right
        public Vector2 position_store_left  = new Vector2(-265,  3); // positions relative to first grid field
        public Vector2 position_store_right = new Vector2( 279, 40); // positions relative to first grid field

        public Vector2 Position_Title_Logo() {
            if(orientation <= 2) { return new Vector2(144, 0); } else { return new Vector2(64, 0); }
        }

        public Vector2 Position_Title_Start() {
            if(orientation <= 2) { return new Vector2(225, 512); } else { return new Vector2(145, 662); }
        }
        public Vector2 Position_GridMain() { // For the first in the main row
            if(orientation <= 2) { return new Vector2(80, 180); } else { return new Vector2(0, 265); }
        }
        public Vector2 Position_GridStore() { // For the left field in the main row
            if(orientation <= 2) { return new Vector2(290, 180); } else { return new Vector2(210, 265); }
        }
        public Vector2 Position_GridOptions() { // For the main row
            if(orientation <= 2) { return new Vector2(80, 180); } else { return new Vector2(0, 265); }
        }
        public Vector2 Position_Store(int i) {
            if(i == 0) { return new Vector2(-205, 3); }   // To left
            else { return new Vector2(279, 40); }
        } // To right
        public Texture2D Get_Background(int i) { // For Current Background
            switch(i) {
                case 0: return texture_background_spacegrid;
                case 1: return texture_background_countryside;
                case 2: return texture_background_clouds;
                case 3: return texture_background_mountains;
                case 4: return texture_background_industry;
            }
            return texture_background_spacegrid;
        }
        public Vector2 Position_InfoText() {
            if(orientation <= 2) { return new Vector2(150, 75); } else { return new Vector2(125, 75); }
        }
        public Vector2 Get_GridSize() {
            if(orientation <= 2) { return new Vector2(texture_background_dropshadow.Width, texture_background_dropshadow.Height); } else { return new Vector2(texture_background_dropshadow.Height, texture_background_dropshadow.Width); }
        }
        public Vector2 Position_2048() {
            if(orientation <= 2) { return new Vector2(156, 75); } else { return new Vector2(75, 156); }
        }
        public Vector2 Position_Mastermind1() {
            if(orientation <= 2) { return new Vector2(22, 68); } else { return new Vector2(-57, 70); }
        }
        public Vector2 Position_Mastermind2() {
            if(orientation <= 2) { return new Vector2(21, 319); } else { return new Vector2(-61, 321); }
        }
        public Vector2 Position_Mastermind_Lamps() {
            if(orientation <= 2) { return new Vector2(222, 68); } else { return new Vector2(142, 70); }
        }
        public Rectangle Collision_Mastermind_Lamp() {
            if(orientation <= 2) { return new Rectangle(222, 68, 458, 102); } else { return new Rectangle(142, 70, 458, 102); }
        }
        public Rectangle Collision_Mastermind_Arrow() {
            if(orientation <= 2) { return new Rectangle(21, 319 - 125, 138, 125); } else { return new Rectangle(-61, 321 - 125, 138, 125); }
        }
        public Vector2 Position_Grid64() {
            if(orientation <= 2) { return new Vector2(16, 48); } else { return new Vector2(48, 16); }
        }
        public Vector2 Position_Grid32() {
            if(orientation <= 2) { return new Vector2(-16, -16); } else { return new Vector2(-16, -16); }
        }
        public Vector2 Position_BackgroundGrid() {
            if(orientation <= 2) { return new Vector2(-48, -16); } else { return new Vector2(-16, -48); }
        }
        public Vector2 Position_Tetris_Field() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 320 / 2, Get_GridSize().Y / 2 - 640 / 2); } else { return new Vector2(Get_GridSize().X / 2 - 320 / 2, Get_GridSize().Y / 2 - 640 / 2); }
        }
        public Vector2 Position_Tetris_Next() {
            if(orientation == 1) { return new Vector2(50, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160 - 50 - 160); }
            if(orientation == 2) { return new Vector2(Get_GridSize().X - 50 - 160, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160 - 50 - 160); }
            if(orientation == 3) { return new Vector2(15, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160 - 15 - 160); }
            if(orientation == 4) { return new Vector2(Get_GridSize().X - 15 - 160, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160 - 15 - 160); }
            return new Vector2(0, 0);
        }
        public Vector2 Position_Tetris_Hold() {
            if(orientation == 1) { return new Vector2(50, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160); }
            if(orientation == 2) { return new Vector2(Get_GridSize().X - 50 - 160, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160); }
            if(orientation == 3) { return new Vector2(15, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160); }
            if(orientation == 4) { return new Vector2(Get_GridSize().X - 15 - 160, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160); }
            return new Vector2(0, 0);
        }
        public Vector2 Position_Tetris_Left() {
            if(orientation == 1) { return new Vector2(Get_GridSize().X - 50 - 160, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160 - 50 - 160); }
            if(orientation == 2) { return new Vector2(50, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160 - 50 - 160); }
            if(orientation == 3) { return new Vector2(Get_GridSize().X - 15 - 160, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160 - 15 - 160); }
            if(orientation == 4) { return new Vector2(15, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160 - 15 - 160); }
            return new Vector2(0, 0);
        }
        public Vector2 Position_Tetris_Right() {
            if(orientation == 1) { return new Vector2(Get_GridSize().X - 50 - 160, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160); }
            if(orientation == 2) { return new Vector2(50, Get_GridSize().Y / 2 + 640 / 2 - 50 - 160); }
            if(orientation == 3) { return new Vector2(Get_GridSize().X - 15 - 160, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160); }
            if(orientation == 4) { return new Vector2(15, Get_GridSize().Y / 2 + 640 / 2 - 15 - 160); }
            return new Vector2(0, 0);
        }
        public Vector2 Position_TicTacToe() {
            if(orientation <= 2) { return new Vector2(75, 0); } else { return new Vector2(0, 75); }
        }
        public Rectangle Collision_BlackJack_Hit() {
            if(orientation <= 2) { return new Rectangle(85, 600, 112, 58); } else { return new Rectangle(50, 750, 112, 58); }
        }
        public Rectangle Collision_BlackJack_Stand() {
            if(orientation <= 2) { return new Rectangle(255, 600, 112, 58); } else { return new Rectangle(220, 750, 112, 58); }
        }
        public Vector2 Position_Bet_Grid() {
            if(orientation <= 2) { return new Vector2(290, 280); } else { return new Vector2(210, 365); }
        }
        public Vector2 Position_Bet_Grid2() {
            if(orientation <= 2) { return Get_GridSize() - new Vector2(282, 164); } else { return Get_GridSize() - new Vector2(282, 164); }
        }
        public Vector2 Position_Bet_Grid3() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 141, Get_GridSize().Y - 164); } else { return new Vector2(Get_GridSize().X / 2 - 141, Get_GridSize().Y - 164); }
        }
        public Vector2 Position_Bet_Minus() {
            if(orientation <= 2) { return new Vector2(152, 280); } else { return new Vector2(72, 365); }
        }
        public Vector2 Position_Bet_Plus() {
            if(orientation <= 2) { return new Vector2(566, 280); } else { return new Vector2(486, 365); }
        }
        public Vector2 Position_Bet_Up1() {
            if(orientation <= 2) { return new Vector2(145, 229); } else { return new Vector2(65, 314); }
        }
        public Vector2 Position_Bet_Up2() {
            if(orientation <= 2) { return new Vector2(559, 229); } else { return new Vector2(479, 314); }
        }
        public Vector2 Position_Bet_Down1() {
            if(orientation <= 2) { return new Vector2(145, 443); } else { return new Vector2(65, 528); }
        }
        public Vector2 Position_Bet_Down2() {
            if(orientation <= 2) { return new Vector2(559, 443); } else { return new Vector2(479, 528); }
        }
        public Vector2 Position_Bet_Left() {
            if(orientation <= 2) { return new Vector2(101, 280); } else { return new Vector2(21, 365); }
        }
        public Vector2 Position_Bet_Right() {
            if(orientation <= 2) { return new Vector2(704, 280); } else { return new Vector2(624, 365); }
        }
        public Vector2 Position_VideoPoker_Hold1() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 75 - 330, 50); } else { return new Vector2(Get_GridSize().X / 2 - 75 - 330, 50); }
        }
        public Vector2 Position_VideoPoker_Hold2() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 75 - 165, 50); } else { return new Vector2(Get_GridSize().X / 2 - 75 - 165, 50); }
        }
        public Vector2 Position_VideoPoker_Hold3() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 75, 50); } else { return new Vector2(Get_GridSize().X / 2 - 75, 50); }
        }
        public Vector2 Position_VideoPoker_Hold4() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 75 + 165, 50); } else { return new Vector2(Get_GridSize().X / 2 - 75 + 165, 50); }
        }
        public Vector2 Position_VideoPoker_Hold5() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - 75 + 330, 50); } else { return new Vector2(Get_GridSize().X / 2 - 75 + 330, 50); }
        }
        public Vector2 Position_SlotMachine_Row1() {
            if(orientation <= 2) { return new Vector2(190, 186); } else { return new Vector2(110, 186); }
        }
        public Vector2 Position_SlotMachine_Row2() {
            if(orientation <= 2) { return new Vector2(365, 186); } else { return new Vector2(285, 186); }
        }
        public Vector2 Position_SlotMachine_Row3() {
            if(orientation <= 2) { return new Vector2(540, 186); } else { return new Vector2(460, 186); }
        }
        public Vector2 Position_Roulette_Chip() {
            if(orientation <= 2) { return new Vector2(90, 90); } else { return new Vector2(10, 170); }
        }

        public Vector2 Position_Roulette_Chip_Next() {
            if(orientation <= 2) { return new Vector2(26, 49); } else { return new Vector2(26, 49); }
        }
        public Vector2 Position_Simon() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(250, 250); } else { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(250, 250); }
        }
        public Vector2 Position_Simon_Player() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(324, 200); } else { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(324, 200); }
        }
        public Vector2 Position_Simon_Simon() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(-260, 200); } else { return Position_DisplayEdge() + Get_GridSize() / 2 - new Vector2(-260, 200); }
        }
        public Vector2 Position_FirstStart() {
            if(orientation <= 2) { return new Vector2(Get_GridSize().X / 2 - texture_menu_grid_full.Width / 2, Get_GridSize().Y - 250); } else { return new Vector2(Get_GridSize().X / 2 - texture_menu_grid_full.Width / 2, Get_GridSize().Y - 250); }
        }
        public Vector2 Position_Sudoku_Grid() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-texture_background_sudoku.Width / 2, -texture_background_sudoku.Height / 2); } else { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-texture_background_sudoku.Width / 2, -texture_background_sudoku.Height / 2); }
        }
        public Vector2 Position_Sudoku_Selection() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-texture_background_sudoku.Width / 2, -texture_background_sudoku.Height / 2 + 64 * 9 + 15); } else { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-texture_background_sudoku.Width / 2, -texture_background_sudoku.Height / 2 + 64 * 9 + 15); }
        }
        public Vector2 Position_OctaCubes() {
            if(orientation <= 2) { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-16, -100); } else { return Position_DisplayEdge() + Get_GridSize() / 2 + new Vector2(-16, -100); }
        }
















        private void Load_Textures(ContentManager Content) {
            font_score = Content.Load<SpriteFont>("Fonts/FontScore");

            texture_background_casing1 = Content.Load<Texture2D>("Graphics/Background/Casing1");
            texture_background_casing2 = Content.Load<Texture2D>("Graphics/Background/Casing2");
            texture_background_dropshadow = Content.Load<Texture2D>("Graphics/Background/DropShadow");
            texture_background_spacegrid = Content.Load<Texture2D>("Graphics/Background/SpaceGrid");
            texture_background_countryside = Content.Load<Texture2D>("Graphics/Background/Countryside");
            texture_background_info = Content.Load<Texture2D>("Graphics/Background/Info");
            texture_background_logo_start = Content.Load<Texture2D>("Graphics/Background/LogoStart");
            texture_background_logo_title = Content.Load<Texture2D>("Graphics/Background/LogoTitle");
            texture_background_tetris = Content.Load<Texture2D>("Graphics/Background/Tetris");
            texture_background_2048 = Content.Load<Texture2D>("Graphics/Background/2048");
            texture_background_gameover = Content.Load<Texture2D>("Graphics/Background/GameOver");
            texture_background_pause = Content.Load<Texture2D>("Graphics/Background/Pause");
            texture_background_roulette = Content.Load<Texture2D>("Graphics/Background/Roulette");
            texture_background_slotmachine1 = Content.Load<Texture2D>("Graphics/Background/SlotMachine1");
            texture_background_slotmachine2 = Content.Load<Texture2D>("Graphics/Background/SlotMachine2");
            texture_background_canvas = Content.Load<Texture2D>("Graphics/Background/Canvas");
            texture_background_grid32 = Content.Load<Texture2D>("Graphics/Background/Grid32");
            texture_background_grid64 = Content.Load<Texture2D>("Graphics/Background/Grid64");
            texture_background_sudoku = Content.Load<Texture2D>("Graphics/Background/Sudoku");
            texture_background_clouds = Content.Load<Texture2D>("Graphics/Background/Clouds");
            texture_background_mountains = Content.Load<Texture2D>("Graphics/Background/Mountains");
            texture_background_industry = Content.Load<Texture2D>("Graphics/Background/Industry");

            texture_spritesheet_2048 = Content.Load<Texture2D>("Graphics/Spritesheet/2048");
            texture_spritesheet_cards = Content.Load<Texture2D>("Graphics/Spritesheet/Cards");
            texture_spritesheet_coin = Content.Load<Texture2D>("Graphics/Spritesheet/Coin");
            texture_spritesheet_explosion = Content.Load<Texture2D>("Graphics/Spritesheet/Explosion");
            texture_spritesheet_lamps = Content.Load<Texture2D>("Graphics/Spritesheet/MasterMindLamps");
            texture_spritesheet_minesweeper = Content.Load<Texture2D>("Graphics/Spritesheet/Minesweeper");
            texture_spritesheet_octanom_head = Content.Load<Texture2D>("Graphics/Spritesheet/OctanomHead");
            texture_spritesheet_octanom_tail = Content.Load<Texture2D>("Graphics/Spritesheet/OctanomTail");
            texture_spritesheet_slotmachine = Content.Load<Texture2D>("Graphics/Spritesheet/SlotMachine");
            texture_spritesheet_sokoban = Content.Load<Texture2D>("Graphics/Spritesheet/Sokoban");
            texture_spritesheet_trianom = Content.Load<Texture2D>("Graphics/Spritesheet/Trianom");
            texture_spritesheet_octagames = Content.Load<Texture2D>("Graphics/Spritesheet/OctaGames");
            texture_spritesheet_mysticsquare = Content.Load<Texture2D>("Graphics/Spritesheet/MysticSquare");
            texture_spritesheet_octacubes = Content.Load<Texture2D>("Graphics/Spritesheet/OctaCubes");
            texture_spritesheet_sudoku = Content.Load<Texture2D>("Graphics/Spritesheet/Sudoku");
            texture_spritesheet_minos_32x = Content.Load<Texture2D>("Graphics/Spritesheet/Minos32x");
            texture_spritesheet_minos_64x = Content.Load<Texture2D>("Graphics/Spritesheet/Minos64x");
            texture_spritesheet_font = Content.Load<Texture2D>("Graphics/Spritesheet/Font");
            texture_spritesheet_values = Content.Load<Texture2D>("Graphics/Spritesheet/Values");

            texture_static_blackjack_hit = Content.Load<Texture2D>("Graphics/Static/BlackJackHit");
            texture_static_blackjack_stand = Content.Load<Texture2D>("Graphics/Static/BlackJackStand");
            texture_static_roulette_ball = Content.Load<Texture2D>("Graphics/Static/RouletteBall");
            texture_static_roulette_chip = Content.Load<Texture2D>("Graphics/Static/RouletteChip");
            texture_static_roulette_minus1 = Content.Load<Texture2D>("Graphics/Static/RouletteMinus1");
            texture_static_roulette_minus2 = Content.Load<Texture2D>("Graphics/Static/RouletteMinus2");
            texture_static_roulette_plus1 = Content.Load<Texture2D>("Graphics/Static/RoulettePlus1");
            texture_static_roulette_plus2 = Content.Load<Texture2D>("Graphics/Static/RoulettePlus2");
            texture_static_roulette_wheel = Content.Load<Texture2D>("Graphics/Static/RouletteWheel");
            texture_static_rpsls_lizard = Content.Load<Texture2D>("Graphics/Static/RPSLSLizard");
            texture_static_rpsls_paper = Content.Load<Texture2D>("Graphics/Static/RPSLSPaper");
            texture_static_rpsls_rock = Content.Load<Texture2D>("Graphics/Static/RPSLSRock");
            texture_static_rpsls_scissor = Content.Load<Texture2D>("Graphics/Static/RPSLSScissor");
            texture_static_rpsls_spock = Content.Load<Texture2D>("Graphics/Static/RPSLSSpock");
            texture_static_slotmachine_line1 = Content.Load<Texture2D>("Graphics/Static/SlotMachineLine1");
            texture_static_slotmachine_line2 = Content.Load<Texture2D>("Graphics/Static/SlotMachineLine2");
            texture_static_slotmachine_line3 = Content.Load<Texture2D>("Graphics/Static/SlotMachineLine3");
            texture_static_slotmachine_line4 = Content.Load<Texture2D>("Graphics/Static/SlotMachineLine4");
            texture_static_slotmachine_line5 = Content.Load<Texture2D>("Graphics/Static/SlotMachineLine5");
            texture_static_slotmachine_point1 = Content.Load<Texture2D>("Graphics/Static/SlotMachinePoint1");
            texture_static_slotmachine_point2 = Content.Load<Texture2D>("Graphics/Static/SlotMachinePoint2");
            texture_static_slotmachine_point3 = Content.Load<Texture2D>("Graphics/Static/SlotMachinePoint3");
            texture_static_slotmachine_point4 = Content.Load<Texture2D>("Graphics/Static/SlotMachinePoint4");
            texture_static_slotmachine_point5 = Content.Load<Texture2D>("Graphics/Static/SlotMachinePoint5");
            texture_static_tetris_next = Content.Load<Texture2D>("Graphics/Static/TetrisNext");
            texture_static_tetris_hold = Content.Load<Texture2D>("Graphics/Static/TetrisHold");
            texture_static_tetris_left = Content.Load<Texture2D>("Graphics/Static/TetrisLeft");
            texture_static_tetris_right = Content.Load<Texture2D>("Graphics/Static/TetrisRight");
            texture_static_tictactoe_circle = Content.Load<Texture2D>("Graphics/Static/TTTCircle");
            texture_static_tictactoe_cross = Content.Load<Texture2D>("Graphics/Static/TTTCross");
            texture_static_tictactoe_line = Content.Load<Texture2D>("Graphics/Static/TTTLine");
            texture_static_videopoker_hold = Content.Load<Texture2D>("Graphics/Static/VideoPokerHold");
            texture_static_simon_console = Content.Load<Texture2D>("Graphics/Static/SimonConsole");
            texture_static_simon_red = Content.Load<Texture2D>("Graphics/Static/SimonRed");
            texture_static_simon_blue = Content.Load<Texture2D>("Graphics/Static/SimonBlue");
            texture_static_simon_green = Content.Load<Texture2D>("Graphics/Static/SimonGreen");
            texture_static_simon_yellow = Content.Load<Texture2D>("Graphics/Static/SimonYellow");
            texture_static_boulder = Content.Load<Texture2D>("Graphics/Static/Boulder");

            texture_hud_button_pad = Content.Load<Texture2D>("Graphics/HUD/ButtonPad");
            texture_hud_button_empty_left = Content.Load<Texture2D>("Graphics/HUD/ButtonEmptyLeft");
            texture_hud_button_empty_right = Content.Load<Texture2D>("Graphics/HUD/ButtonEmptyRight");
            texture_hud_button_yes = Content.Load<Texture2D>("Graphics/HUD/ButtonYes");
            texture_hud_button_no = Content.Load<Texture2D>("Graphics/HUD/ButtonNo");
            texture_hud_button_back = Content.Load<Texture2D>("Graphics/HUD/ButtonBack");
            texture_hud_button_info1 = Content.Load<Texture2D>("Graphics/HUD/ButtonInfo1");
            texture_hud_button_info2 = Content.Load<Texture2D>("Graphics/HUD/ButtonInfo2");
            texture_hud_button_main1 = Content.Load<Texture2D>("Graphics/HUD/ButtonMain1");
            texture_hud_button_main2 = Content.Load<Texture2D>("Graphics/HUD/ButtonMain2");
            texture_hud_button_mino1 = Content.Load<Texture2D>("Graphics/HUD/ButtonMino1");
            texture_hud_button_mino2 = Content.Load<Texture2D>("Graphics/HUD/ButtonMino2");
            texture_hud_button_options1 = Content.Load<Texture2D>("Graphics/HUD/ButtonOptions1");
            texture_hud_button_options2 = Content.Load<Texture2D>("Graphics/HUD/ButtonOptions2");
            texture_hud_button_statistics1 = Content.Load<Texture2D>("Graphics/HUD/ButtonStatistics1");
            texture_hud_button_statistics2 = Content.Load<Texture2D>("Graphics/HUD/ButtonStatistics2");
            texture_hud_button_store1 = Content.Load<Texture2D>("Graphics/HUD/ButtonStore1");
            texture_hud_button_store2 = Content.Load<Texture2D>("Graphics/HUD/ButtonStore2");
            texture_hud_button_flag = Content.Load<Texture2D>("Graphics/HUD/ButtonFlag");
            texture_hud_button_pause = Content.Load<Texture2D>("Graphics/HUD/ButtonPause");
            texture_hud_button_questionmark = Content.Load<Texture2D>("Graphics/HUD/ButtonQuestionmark");
            texture_hud_button_replay = Content.Load<Texture2D>("Graphics/HUD/ButtonReplay");
            texture_hud_button_resume = Content.Load<Texture2D>("Graphics/HUD/ButtonResume");
            texture_hud_button_select = Content.Load<Texture2D>("Graphics/HUD/ButtonSelect");
            texture_hud_button_spin = Content.Load<Texture2D>("Graphics/HUD/ButtonSpin");
            texture_colormap = Content.Load<Texture2D>("Graphics/HUD/Colormap");

            texture_highlight_button_left = Content.Load<Texture2D>("Graphics/HUD/HighlightButtonLeft");
            texture_highlight_button_right = Content.Load<Texture2D>("Graphics/HUD/HighlightButtonRight");
            texture_highlight_arrow_up = Content.Load<Texture2D>("Graphics/HUD/HighlightArrowUp");
            texture_highlight_arrow_left = Content.Load<Texture2D>("Graphics/HUD/HighlightArrowLeft");
            texture_highlight_arrow_right = Content.Load<Texture2D>("Graphics/HUD/HighlightArrowRight");
            texture_highlight_arrow_down = Content.Load<Texture2D>("Graphics/HUD/HighlightArrowDown");

            texture_menu_grid_empty = Content.Load<Texture2D>("Graphics/Menu/GridEmpty");
            texture_menu_grid_full = Content.Load<Texture2D>("Graphics/Menu/GridFull");
            texture_menu_grid_selector = Content.Load<Texture2D>("Graphics/Menu/GridSelector");
            texture_menu_grid_options = Content.Load<Texture2D>("Graphics/Menu/GridOptions");
            texture_menu_grid_store = Content.Load<Texture2D>("Graphics/Menu/GridStore");
            texture_menu_grid_statistics = Content.Load<Texture2D>("Graphics/Menu/GridStatistics");
            texture_menu_panel = Content.Load<Texture2D>("Graphics/Menu/Panel");
            texture_menu_info_board = Content.Load<Texture2D>("Graphics/Menu/InfoBoard");
            texture_menu_info_left = Content.Load<Texture2D>("Graphics/Menu/InfoLeft");
            texture_menu_info_right = Content.Load<Texture2D>("Graphics/Menu/InfoRight");
            texture_menu_info_difficulty = Content.Load<Texture2D>("Graphics/Menu/InfoDifficulty");
            texture_menu_info_easy = Content.Load<Texture2D>("Graphics/Menu/InfoEasy");
            texture_menu_info_medium = Content.Load<Texture2D>("Graphics/Menu/InfoMedium");
            texture_menu_info_hard = Content.Load<Texture2D>("Graphics/Menu/InfoHard");
            texture_menu_info_levelselect = Content.Load<Texture2D>("Graphics/Menu/InfoLevelSelect");
            texture_menu_info_startgame = Content.Load<Texture2D>("Graphics/Menu/InfoStartGame");

            texture_control_touch = Content.Load<Texture2D>("Graphics/Menu/ControlTouch");
            texture_control_pad = Content.Load<Texture2D>("Graphics/Menu/ControlPad");
            texture_control_enter = Content.Load<Texture2D>("Graphics/Menu/ControlEnter");
            texture_control_pause = Content.Load<Texture2D>("Graphics/Menu/ControlPause");
            texture_control_minesweeper = Content.Load<Texture2D>("Graphics/Menu/ControlMinesweeper");
            texture_control_questionmark = Content.Load<Texture2D>("Graphics/Menu/ControlQuestionmark");
            texture_control_hold = Content.Load<Texture2D>("Graphics/Menu/ControlHold");
            texture_control_turnleft = Content.Load<Texture2D>("Graphics/Menu/ControlTurnLeft");
            texture_control_turnright = Content.Load<Texture2D>("Graphics/Menu/ControlTurnRight");

            texture_options_plus = Content.Load<Texture2D>("Graphics/Menu/IconPlus");
            texture_options_minus = Content.Load<Texture2D>("Graphics/Menu/IconMinus");
            texture_options_music_on = Content.Load<Texture2D>("Graphics/Menu/IconMusicOn");
            texture_options_music_off = Content.Load<Texture2D>("Graphics/Menu/IconMusicOff");
            texture_options_sound_on = Content.Load<Texture2D>("Graphics/Menu/IconAudioOn");
            texture_options_sound_off = Content.Load<Texture2D>("Graphics/Menu/IconAudioOff");
            texture_options_next = Content.Load<Texture2D>("Graphics/Menu/IconTrackNext");
            texture_options_previous = Content.Load<Texture2D>("Graphics/Menu/IconTrackPrevious");
            texture_options_shuffle = Content.Load<Texture2D>("Graphics/Menu/IconTrackShuffle");
            texture_options_brightness = Content.Load<Texture2D>("Graphics/Menu/IconBrightness");
            texture_options_werbung = Content.Load<Texture2D>("Graphics/Menu/IconWerbung");
            texture_options_alignment = Content.Load<Texture2D>("Graphics/Menu/IconAlignment");
            texture_options_align_left = Content.Load<Texture2D>("Graphics/Menu/IconAlignLeft");
            texture_options_align_right = Content.Load<Texture2D>("Graphics/Menu/IconAlignRight");

            texture_icon_octatravels = Content.Load<Texture2D>("Graphics/Menu/IconOctaTravels");
            texture_icon_octanom = Content.Load<Texture2D>("Graphics/Menu/IconOctanom");
            texture_icon_snake = Content.Load<Texture2D>("Graphics/Menu/IconSnake");
            texture_icon_octacubes = Content.Load<Texture2D>("Graphics/Menu/IconOctaCubes");
            texture_icon_sokoban = Content.Load<Texture2D>("Graphics/Menu/IconSokoban");
            texture_icon_octapow = Content.Load<Texture2D>("Graphics/Menu/IconOctaPOW");
            texture_icon_bouldernom = Content.Load<Texture2D>("Graphics/Menu/IconBoulderNom");
            texture_icon_octabattle = Content.Load<Texture2D>("Graphics/Menu/IconOctaBattle");
            texture_icon_arkanoid = Content.Load<Texture2D>("Graphics/Menu/IconArkanoid");
            texture_icon_spaceinvader = Content.Load<Texture2D>("Graphics/Menu/IconSpaceInvader");
            texture_icon_octajump = Content.Load<Texture2D>("Graphics/Menu/IconOctaJump");
            texture_icon_octaflight = Content.Load<Texture2D>("Graphics/Menu/IconOctaFlight");
            texture_icon_bombuzal = Content.Load<Texture2D>("Graphics/Menu/IconBombuzal");
            texture_icon_elements = Content.Load<Texture2D>("Graphics/Menu/IconElements");
            texture_icon_octapanic = Content.Load<Texture2D>("Graphics/Menu/IconOctaPanic");
            texture_icon_tetris = Content.Load<Texture2D>("Graphics/Menu/IconTetris");
            texture_icon_columns = Content.Load<Texture2D>("Graphics/Menu/IconColumns");
            texture_icon_meanminos = Content.Load<Texture2D>("Graphics/Menu/IconMeanMinos");
            texture_icon_wickedminos = Content.Load<Texture2D>("Graphics/Menu/IconWickedMinos");
            texture_icon_minogarden = Content.Load<Texture2D>("Graphics/Menu/IconMinoGarden");
            texture_icon_matchmakers = Content.Load<Texture2D>("Graphics/Menu/IconMatchmakers");
            texture_icon_memory = Content.Load<Texture2D>("Graphics/Menu/IconMemory");
            texture_icon_8colors = Content.Load<Texture2D>("Graphics/Menu/Icon8Colors");
            texture_icon_swing = Content.Load<Texture2D>("Graphics/Menu/IconSwing");
            texture_icon_mastermind = Content.Load<Texture2D>("Graphics/Menu/IconMastermind");
            texture_icon_2048 = Content.Load<Texture2D>("Graphics/Menu/Icon2048");
            texture_icon_minesweeper = Content.Load<Texture2D>("Graphics/Menu/IconMinesweeper");
            texture_icon_tictactoe = Content.Load<Texture2D>("Graphics/Menu/IconTicTacToe");
            texture_icon_rpsls = Content.Load<Texture2D>("Graphics/Menu/IconRPSLS");
            texture_icon_simon = Content.Load<Texture2D>("Graphics/Menu/IconSimon");
            texture_icon_picross = Content.Load<Texture2D>("Graphics/Menu/IconPicross");
            texture_icon_halma = Content.Load<Texture2D>("Graphics/Menu/IconHalma");
            texture_icon_circuits = Content.Load<Texture2D>("Graphics/Menu/IconCircuits");
            texture_icon_mysticsquare = Content.Load<Texture2D>("Graphics/Menu/IconMysticSquare");
            texture_icon_sudoku = Content.Load<Texture2D>("Graphics/Menu/IconSudoku");
            texture_icon_locomotion = Content.Load<Texture2D>("Graphics/Menu/IconLocomotion");
            texture_icon_pipemania = Content.Load<Texture2D>("Graphics/Menu/IconPipeMania");
            texture_icon_lazerlight = Content.Load<Texture2D>("Graphics/Menu/IconLazerLight");
            texture_icon_splitball = Content.Load<Texture2D>("Graphics/Menu/IconSplitBall");
            texture_icon_blackhole = Content.Load<Texture2D>("Graphics/Menu/IconBlackHole");
            texture_icon_spider = Content.Load<Texture2D>("Graphics/Menu/IconSpider");
            texture_icon_pyramid = Content.Load<Texture2D>("Graphics/Menu/IconPyramid");
            texture_icon_klondike = Content.Load<Texture2D>("Graphics/Menu/IconKlondike");
            texture_icon_tripeak = Content.Load<Texture2D>("Graphics/Menu/IconTriPeak");
            texture_icon_freecell = Content.Load<Texture2D>("Graphics/Menu/IconFreeCell");
            texture_icon_blackjack = Content.Load<Texture2D>("Graphics/Menu/IconBlackJack");
            texture_icon_baccarat = Content.Load<Texture2D>("Graphics/Menu/IconBaccarat");
            texture_icon_videopoker = Content.Load<Texture2D>("Graphics/Menu/IconVideoPoker");
            texture_icon_slotmachine = Content.Load<Texture2D>("Graphics/Menu/IconSlotMachine");
            texture_icon_roulette = Content.Load<Texture2D>("Graphics/Menu/IconRoulette");
            texture_icon_rougeetnoir = Content.Load<Texture2D>("Graphics/Menu/IconRougeEtNoir");
            texture_icon_aceydeucey = Content.Load<Texture2D>("Graphics/Menu/IconAceyDeucey");
            texture_icon_balotangkas = Content.Load<Texture2D>("Graphics/Menu/IconBaloTangkas");
            texture_icon_craps = Content.Load<Texture2D>("Graphics/Menu/IconCraps");
            texture_icon_sicbo = Content.Load<Texture2D>("Graphics/Menu/IconSicBo");
            texture_icon_fantan = Content.Load<Texture2D>("Graphics/Menu/IconFanTan");
            texture_icon_paigow = Content.Load<Texture2D>("Graphics/Menu/IconPaiGow");
            texture_icon_keno = Content.Load<Texture2D>("Graphics/Menu/IconKeno");
            texture_icon_tiengow = Content.Load<Texture2D>("Graphics/Menu/IconTienGow");
            texture_icon_yacht = Content.Load<Texture2D>("Graphics/Menu/IconYacht");
            texture_icon_select = Content.Load<Texture2D>("Graphics/Menu/IconSelect");
            texture_icon_easy = Content.Load<Texture2D>("Graphics/Menu/IconEasy");
            texture_icon_hard = Content.Load<Texture2D>("Graphics/Menu/IconHard");
            texture_icon_hold = Content.Load<Texture2D>("Graphics/Menu/IconHold");
            texture_icon_highroller = Content.Load<Texture2D>("Graphics/Menu/IconHighRoller");
            texture_icon_octalive1 = Content.Load<Texture2D>("Graphics/Menu/IconOctaLive1");
            texture_icon_octalive2 = Content.Load<Texture2D>("Graphics/Menu/IconOctaLive2");
            texture_icon_octalive3 = Content.Load<Texture2D>("Graphics/Menu/IconOctaLive3");
            texture_icon_octalive4 = Content.Load<Texture2D>("Graphics/Menu/IconOctaLive4");
            texture_icon_mino_fire_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoFireBasic");
            texture_icon_mino_fire_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoFirePower");
            texture_icon_mino_air_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoAirBasic");
            texture_icon_mino_air_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoAirPower");
            texture_icon_mino_thunder_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoThunderBasic");
            texture_icon_mino_thunder_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoThunderPower");
            texture_icon_mino_water_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoWaterBasic");
            texture_icon_mino_water_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoWaterPower");
            texture_icon_mino_ice_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoIceBasic");
            texture_icon_mino_ice_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoIcePower");
            texture_icon_mino_earth_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoEarthBasic");
            texture_icon_mino_earth_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoEarthPower");
            texture_icon_mino_metal_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoMetalBasic");
            texture_icon_mino_metal_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoMetalPower");
            texture_icon_mino_nature_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoNatureBasic");
            texture_icon_mino_nature_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoNaturePower");
            texture_icon_mino_light_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoLightBasic");
            texture_icon_mino_light_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoLightPower");
            texture_icon_mino_dark_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoDarkBasic");
            texture_icon_mino_dark_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoDarkPower");
            texture_icon_mino_gold_basic = Content.Load<Texture2D>("Graphics/Menu/IconMinoGoldBasic");
            texture_icon_mino_gold_power = Content.Load<Texture2D>("Graphics/Menu/IconMinoGoldPower");
            texture_icon_clouds = Content.Load<Texture2D>("Graphics/Menu/IconClouds");
            texture_icon_mountains = Content.Load<Texture2D>("Graphics/Menu/IconMountains");
            texture_icon_industry = Content.Load<Texture2D>("Graphics/Menu/IconIndustry");
            texture_icon_musicpack1 = Content.Load<Texture2D>("Graphics/Menu/IconMusicPack1");
            texture_icon_musicpack2 = Content.Load<Texture2D>("Graphics/Menu/IconMusicPack2");
            texture_icon_musicpack3 = Content.Load<Texture2D>("Graphics/Menu/IconMusicPack3");
            texture_icon_info = Content.Load<Texture2D>("Graphics/Menu/IconInfo");

            texture_icon_highscore = Content.Load<Texture2D>("Graphics/Menu/IconHighscore");
            texture_icon_coins = Content.Load<Texture2D>("Graphics/Menu/IconCoins");
            texture_icon_medal1 = Content.Load<Texture2D>("Graphics/Menu/IconMedal1");
            texture_icon_medal2 = Content.Load<Texture2D>("Graphics/Menu/IconMedal2");
            texture_icon_medal3 = Content.Load<Texture2D>("Graphics/Menu/IconMedal3");
            texture_icon_times_played = Content.Load<Texture2D>("Graphics/Menu/IconTimesPlayed");
            texture_icon_times_started = Content.Load<Texture2D>("Graphics/Menu/IconTimesStarted");
            texture_icon_mino = Content.Load<Texture2D>("Graphics/Menu/IconMino");

            texture_casino_bet_down = Content.Load<Texture2D>("Graphics/Casino/BetDown");
            texture_casino_bet_grid = Content.Load<Texture2D>("Graphics/Casino/BetGrid");
            texture_casino_bet_left = Content.Load<Texture2D>("Graphics/Casino/BetLeft");
            texture_casino_bet_minus = Content.Load<Texture2D>("Graphics/Casino/BetMinus");
            texture_casino_bet_plus = Content.Load<Texture2D>("Graphics/Casino/BetPlus");
            texture_casino_bet_right = Content.Load<Texture2D>("Graphics/Casino/BetRight");
            texture_casino_bet_up = Content.Load<Texture2D>("Graphics/Casino/BetUp");
        }

        private void Load_Lists() {

            info_firststart[0] = new List<string>();
            info_firststart[1] = new List<string>();

            for(int i = 0; i < info_description.Length; i++) {
                info_description[i]  = new List<string>();
                info_control_icon[i] = new List<int>();
                info_control_text[i] = new List<string>();

                info_control_icon[i] = Get_Control_Icon(i);
            }
            for(int i = 0; i < info_store.Length; i++) {
                info_store[i] = "MISSING INFO!";
            }
        }

        public void Load_Info() {
            info_firststart[0] = Get_Description(-2);
            info_firststart[1] = Get_Description(-1);
            for(int i = 0; i < info_description.Length; i++) {
                info_description[i]  = Get_Description(i);
                info_control_text[i] = Get_Control_Text(i);
            }
            for(int i = 0; i < info_store.Length; i++) {
                //info_store[i] = TEXT_STORE("en-US", i);
            }
        }

        private List<string> Get_Description(int index) {
            List<string> list = new List<string>();
            int length = orientation <= 2 ? 45 : 38;
            string text = TEXT_DESCRIPTION("en-US", index);
            char[] charlist = text.ToCharArray();

            if(charlist.Length <= length) {
                list.Add(text);
            } else {
                int point_last = 0;
                while(point_last + length < text.Length) {
                    if(charlist.Length > length) {
                        int point_next = 0;
                        for(int x = 0; x < length; x++) {
                            if(charlist[x + point_last] == ' ') point_next = x;
                        }
                        list.Add(text.Substring(point_last, point_next));
                        point_last += point_next;
                    }
                }
                list.Add(text.Substring(point_last, text.Length - point_last));
            }
            return list;
        }

        private List<int> Get_Control_Icon(int index) {
            List<int> list = new List<int>();
            string text = TEXT_CONTROL_ICON(index);
            string[] numbers = text.Split(':');
            foreach(string s in numbers) {
                list.Add(int.Parse(s));
            }
            return list;
        }

        private List<string> Get_Control_Text(int index) {
            List<string> list = new List<string>();
            string text = TEXT_CONTROL_TEXT("en-US", index);
            string[] infos = text.Split(':');
            foreach(string s in infos) {
                list.Add(s);
            }
            return list;
        }

        private string TEXT_DESCRIPTION(string lang, int index) {
            switch(lang) {
                case "en-US":
                    switch(index) {
                        case -2: return "Welcome to the Game Center. Let's give You a quick overview. All Games are used with the Buttons on your side. The arrows move things around and the right button does special actions in certain games. The left button controls the pause menu ingame. The Menu and certain games can be interacted with by touching on the gamescreen. To continue press the right button.";
                        case -1: return "It's Dangerous to go alone. Take one of these.";
                        case  0: return "OctaNom feels adventurous and decides to walk the longer road to his favourite snack bar. Little does he know about the dangers ahead. Guide OctaNom through the terrain, use the lilypads to cross the water and don't let him get smashed by the TriaTrains.";
                        case  1: return "Octanom is hungry and this time he laid eyes on the not-so-secret treasure vault of the Trianoms. Navigate him through the Labyrinth, avoid the guards and collect the little dots. Keep an eye out for the big cookies. An all devouring secret lies in them.";
                        case  2: return "OctaNom is hungry and he demands cookies! Navigate bim through the field and collect every cookies you can find. But beware, with each tasty cookie devoured, OctaNom gets bigger and longer, so avoid running him into himself, munching it's own tail sounds terrible hurtful.";
                        case  3: return "OctaNom needs some excersize and what better oppurtunity is there than the seemingly endless steps right under a dangerously crumbling cliff. Navigate OctaNom on the steps to light up all fields he steps on. Avoid on your way the falling rocks the slowly make their way from the non steppable red fields down to the bottom. But beware, different colored rocks will behave differently.";
                        case  4: return "OctaNom can't find his favourity cookie jar and so he decided it's finally time to clean up his basement. Move OctaNom through his basement and push all boxes on the x-marked spots. It doesn't matter which box lands on which spot and use the revert button once you push a box into a place you can't get it out anymore.";
                        //case  5: return "# Play against 3 enemies and see who will stand last";
                        //case  6: return "# Dig through the ground and don't get hit by falling boulders";
                        //case  7: return "# Defend your Base against against enemy forces";
                        //case  8: return "# Hit the ball and take out all Minos";
                        //case  9: return "# Protect the city against hordes of enemies";
                        //case 10: return "# Jump onto platforms to reach higher places";
                        //case 11: return "# Fly through a maze of Minos without smashing into them";
                        //case 12: return "# Clear the field from all bombs and don't get caught in the explosion";
                        //case 13: return "# Travel through the Labyrinth and find out what lies at the end";
                        //case 14: return "# Trap the enemy and keep them away from them";
                        case  5: return "Play one of the most classic games, ever. Tetris. Move and rotate the falling Tetrominos to build a full row for points and maximum destruction.";
                        case  6: return "Move the falling Treminos and align the Minos into rows of 3 or more. You can cycle the minos in the Treminos but not rotate it. Once landed the Minos will fall indepently if a Mino beneath them get's deytroyed. Use this to your adventage and build combos of Mino rows.";
                        case  7: return "Move and rotate the falling Dominos and connect 4 Minos or more of the same color together. It doesn't matter how the connection is shaped. Once the Domino hits something it breaks apart and the minos fall seperately. Let this work for you and build up combos of connected Minos.";
                        //case 18: return "Bring order into this chaotic board and switch 2 neighboring minos with each other and build up rows or 3 or more of the same color. ";
                        //case 19: return "# Plant Minos in your Garden to rows of 3 or more";
                        //case 20: return "# Cycle your rows to build lines of the same Minos";
                        case  8: return "Remember where the Minos are and find matching pairs";
                        //case 22: return "# Choose a color and dominate the board";
                        //case 23: return "# Balance the scales and build rows of 3 Minos or more";
                        case  9: return "Find the right combination to unlock the lock";
                        case 10: return "Crash 2 Number into each other and create higher numbers";
                        case 11: return "Tap the Board and find all bombs without hitting them";
                        case 12: return "Beat the system with a three-in-a-row";
                        case 13: return "Play a Game of Rock-Paper-Scissors-Lizard-Spock";
                        case 14: return "Remember and replay the shown color combination";
                        //case 30: return "# Follow the numbers and draw the right picture";
                        //case 31: return "# Hop over all figures until only 1 still stands";
                        //case 32: return "# Connect the circuit and fill out all fields in the board";
                        //case 33: return "# Move the tiles around until the board is sorted again";
                        //case 34: return "# Fill out the field with numbers without 2 of the same in a line";
                        //case 35: return "# Control the switches and lead the trains to their destination";
                        //case 36: return "# Build a system of pipes and keep it flowing as long as possible";
                        //case 37: return "# Place various props to let the light find its goal";
                        //case 38: return "# Trap the ball into ever smaller fields";
                        //case 39: return "# Smash your marbles into each other for points and blocks";
                        case 15: return "Play a game of Klondike Solitaire";
                        case 16: return "Play a game of TriPeak Solitaire";
                        case 17: return "Play a game of Spider Solitaire";
                        case 18: return "Play a game of Pyramid Solitaire";
                        case 19: return "Play a game if FreeCell Solitaire";
                        case 20: return "Try your luck on the cards table";
                        case 21: return "Try your luck on the cards table";
                        case 22: return "Try your luck on the cards table";
                        case 23: return "Try your luck on the Slot Machine";
                        case 24: return "Try your luck on the Roulette Table";
                        //case 50: return "# Try your luck on the cards table";
                        //case 51: return "# Try your luck on the cards table";
                        //case 52: return "# Try your luck ob the cards table";
                        //case 53: return "# Try your luck on the dice table";
                        //case 54: return "# Try your luck on the dice table";
                        //case 55: return "# Try your luck with some chips";
                        //case 56: return "# Try your luck with a set of Dominos";
                        //case 57: return "# Try your luck on Keno";
                        //case 58: return "# Try your luck on the dice table";
                        //case 59: return "# Try your luck on the dice table";
                    }
                    break;
                case "de-DE":
                    switch(index) {
                        case -2: return "TEXT FOR FIRST START EN";
                        case -1: return "It's Dangerous to go alone. Take one of these.";
                        case  0: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  1: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  2: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  3: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  4: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  5: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  6: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  7: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  8: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case  9: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 10: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 11: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 12: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 13: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 14: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 15: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 16: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 17: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 18: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 19: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 20: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 21: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 22: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 23: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 24: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 25: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 26: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 27: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 28: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 29: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 30: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 31: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 32: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 33: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 34: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 35: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 36: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 37: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 38: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 39: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 40: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 41: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 42: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 43: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 44: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 45: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 46: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 47: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 48: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 49: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 50: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 51: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 52: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 53: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 54: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 55: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 56: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 57: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 58: return "FEHLENDERBESCHREIBUNGSTEXT";
                        case 59: return "FEHLENDERBESCHREIBUNGSTEXT";
                    }
                    break;
            }
            return "INVALID STRING";
        }

        private string TEXT_CONTROL_ICON(int index) {
            switch(index) {
                case  0: return "1";
                case  1: return "1";
                case  2: return "1";
                case  3: return "1";
                case  4: return "1";
                case  5: return "1:6:7:8";
                case  6: return "1:6:7:8";
                case  7: return "1:6:7:8";
                case  8: return "0:1:2";
                case  9: return "0:1:2";
                case 10: return "1";
                case 11: return "0:1:2";
                case 12: return "0:1:2";
                case 13: return "0:1:2";
                case 14: return "0:1";
                case 15: return "0:1:2";
                case 16: return "0:1:2";
                case 17: return "0:1:2";
                case 18: return "0:1:2";
                case 19: return "0:1:2";
                case 20: return "0:1:2";
                case 21: return "0:1:2";
                case 22: return "0:1:2";
                case 23: return "0:1:2";
                case 24: return "0:1:2";
                case 25: return "0:1:2";
                case 26: return "0:1:2";
                case 27: return "0:1:2";
                case 28: return "0:1:2";
                case 29: return "0:1:2";
                case 30: return "0:1:2";
                case 31: return "0:1:2";
                case 32: return "0:1:2";
                case 33: return "0:1:2";
                case 34: return "0:1:2";
                case 35: return "0:1:2";
                case 36: return "0:1:2";
                case 37: return "0:1:2";
                case 38: return "0:1:2";
                case 39: return "0:1:2";
                case 40: return "0:1:2";
                case 41: return "0:1:2";
                case 42: return "0:1:2";
                case 43: return "0:1:2";
                case 44: return "0:1:2";
                case 45: return "0:1:2";
                case 46: return "0:1:2";
                case 47: return "0:1:2";
                case 48: return "0:1:2";
                case 49: return "0:1:2";
                case 50: return "0:1:2";
                case 51: return "0:1:2";
                case 52: return "0:1:2";
                case 53: return "0:1:2";
                case 54: return "0:1:2";
                case 55: return "0:1:2";
                case 56: return "0:1:2";
                case 57: return "0:1:2";
                case 58: return "0:1:2";
                case 59: return "0:1:2";
            }
            return "0";
        }

        private string TEXT_CONTROL_TEXT(string lang, int index) {
            switch(lang) {
                case "en-US":
                    switch(index) {
                        case  0: return "Move Octanom around";
                        case  1: return "Move Octanom around";
                        case  2: return "Move Octanom around";
                        case  3: return "Move Octanom around";
                        case  4: return "Move Octanom around";
                        case  5: return "Move the faling Tetromino:Switch out your Tetromino with your reserved one:Turn your Tetromino counterclockwise:Turn your Tetromino clockwise";
                        case  6: return "Move the faling Column:Switch out your Columns with your reserved one:Rotate the Column colors counterclockwise:Rotate the Column colors clockwise";
                        case  7: return "Move the faling Domino:Switch out your Domino with your reserved one:Turn your Domino counterclockwise:Turn your Domino clockwise";
                        case  8: return "Select and uncover a Mino:Select a Mino:Uncover the selected Mino";
                        case  9: return "Rotate the Locks:Select a Lock:Rotate the selected Lock";
                        case 10: return "Move the Numbers around";
                        case 11: return "Select and uncover a Tile:Select a Tile:Uncover the selected Tile";
                        case 12: return "Place your Mark:Find a Position:Place your Mark";
                        case 13: return "Choose and confirm your Mark:Choose your Mark:Confirm your Mark";
                        case 14: return "Play a Color:Play a Color";
                        case 15: return "Select a Card:Select a Card:Confirm Selection";
                        case 16: return "Select a Card:Select a Card:Confirm Selection";
                        case 17: return "Select a Card:Select a Card:Confirm Selection";
                        case 18: return "Select a Card:Select a Card:Confirm Selection";
                        case 19: return "Select a Card:Select a Card:Confirm Selection";
                        case 20: return "Choose an option:Choose an option:Confirm your option";
                        case 21: return "Choose an option:Choose an option:Confirm your option";
                        case 22: return "Choose an option:Choose an option:Confirm your option";
                        case 23: return "Choose an option:Choose an option:Confirm your option";
                        case 24: return "Choose an option:Choose an option:Confirm your option";
                        case 25: return "MISSINGCONTROLTEXT";
                        case 26: return "MISSINGCONTROLTEXT";
                        case 27: return "MISSINGCONTROLTEXT";
                        case 28: return "MISSINGCONTROLTEXT";
                        case 29: return "MISSINGCONTROLTEXT";
                        case 30: return "MISSINGCONTROLTEXT";
                        case 31: return "MISSINGCONTROLTEXT";
                        case 32: return "MISSINGCONTROLTEXT";
                        case 33: return "MISSINGCONTROLTEXT";
                        case 34: return "MISSINGCONTROLTEXT";
                        case 35: return "MISSINGCONTROLTEXT";
                        case 36: return "MISSINGCONTROLTEXT";
                        case 37: return "MISSINGCONTROLTEXT";
                        case 38: return "MISSINGCONTROLTEXT";
                        case 39: return "MISSINGCONTROLTEXT";
                        case 40: return "MISSINGCONTROLTEXT";
                        case 41: return "MISSINGCONTROLTEXT";
                        case 42: return "MISSINGCONTROLTEXT";
                        case 43: return "MISSINGCONTROLTEXT";
                        case 44: return "MISSINGCONTROLTEXT";
                        case 45: return "MISSINGCONTROLTEXT";
                        case 46: return "MISSINGCONTROLTEXT";
                        case 47: return "MISSINGCONTROLTEXT";
                        case 48: return "MISSINGCONTROLTEXT";
                        case 49: return "MISSINGCONTROLTEXT";
                        case 50: return "MISSINGCONTROLTEXT";
                        case 51: return "MISSINGCONTROLTEXT";
                        case 52: return "MISSINGCONTROLTEXT";
                        case 53: return "MISSINGCONTROLTEXT";
                        case 54: return "MISSINGCONTROLTEXT";
                        case 55: return "MISSINGCONTROLTEXT";
                        case 56: return "MISSINGCONTROLTEXT";
                        case 57: return "MISSINGCONTROLTEXT";
                        case 58: return "MISSINGCONTROLTEXT";
                        case 59: return "MISSINGCONTROLTEXT";
                    }
                    break;
                case "de-DE":
                    switch(index) {
                        case  0: return "FEHLENDERBEDIENTEXT";
                        case  1: return "FEHLENDERBEDIENTEXT";
                        case  2: return "FEHLENDERBEDIENTEXT";
                        case  3: return "FEHLENDERBEDIENTEXT";
                        case  4: return "FEHLENDERBEDIENTEXT";
                        case  5: return "FEHLENDERBEDIENTEXT";
                        case  6: return "FEHLENDERBEDIENTEXT";
                        case  7: return "FEHLENDERBEDIENTEXT";
                        case  8: return "FEHLENDERBEDIENTEXT";
                        case  9: return "FEHLENDERBEDIENTEXT";
                        case 10: return "FEHLENDERBEDIENTEXT";
                        case 11: return "FEHLENDERBEDIENTEXT";
                        case 12: return "FEHLENDERBEDIENTEXT";
                        case 13: return "FEHLENDERBEDIENTEXT";
                        case 14: return "FEHLENDERBEDIENTEXT";
                        case 15: return "FEHLENDERBEDIENTEXT";
                        case 16: return "FEHLENDERBEDIENTEXT";
                        case 17: return "FEHLENDERBEDIENTEXT";
                        case 18: return "FEHLENDERBEDIENTEXT";
                        case 19: return "FEHLENDERBEDIENTEXT";
                        case 20: return "FEHLENDERBEDIENTEXT";
                        case 21: return "FEHLENDERBEDIENTEXT";
                        case 22: return "FEHLENDERBEDIENTEXT";
                        case 23: return "FEHLENDERBEDIENTEXT";
                        case 24: return "FEHLENDERBEDIENTEXT";
                        case 25: return "FEHLENDERBEDIENTEXT";
                        case 26: return "FEHLENDERBEDIENTEXT";
                        case 27: return "FEHLENDERBEDIENTEXT";
                        case 28: return "FEHLENDERBEDIENTEXT";
                        case 29: return "FEHLENDERBEDIENTEXT";
                        case 30: return "FEHLENDERBEDIENTEXT";
                        case 31: return "FEHLENDERBEDIENTEXT";
                        case 32: return "FEHLENDERBEDIENTEXT";
                        case 33: return "FEHLENDERBEDIENTEXT";
                        case 34: return "FEHLENDERBEDIENTEXT";
                        case 35: return "FEHLENDERBEDIENTEXT";
                        case 36: return "FEHLENDERBEDIENTEXT";
                        case 37: return "FEHLENDERBEDIENTEXT";
                        case 38: return "FEHLENDERBEDIENTEXT";
                        case 39: return "FEHLENDERBEDIENTEXT";
                        case 40: return "FEHLENDERBEDIENTEXT";
                        case 41: return "FEHLENDERBEDIENTEXT";
                        case 42: return "FEHLENDERBEDIENTEXT";
                        case 43: return "FEHLENDERBEDIENTEXT";
                        case 44: return "FEHLENDERBEDIENTEXT";
                        case 45: return "FEHLENDERBEDIENTEXT";
                        case 46: return "FEHLENDERBEDIENTEXT";
                        case 47: return "FEHLENDERBEDIENTEXT";
                        case 48: return "FEHLENDERBEDIENTEXT";
                        case 49: return "FEHLENDERBEDIENTEXT";
                        case 50: return "FEHLENDERBEDIENTEXT";
                        case 51: return "FEHLENDERBEDIENTEXT";
                        case 52: return "FEHLENDERBEDIENTEXT";
                        case 53: return "FEHLENDERBEDIENTEXT";
                        case 54: return "FEHLENDERBEDIENTEXT";
                        case 55: return "FEHLENDERBEDIENTEXT";
                        case 56: return "FEHLENDERBEDIENTEXT";
                        case 57: return "FEHLENDERBEDIENTEXT";
                        case 58: return "FEHLENDERBEDIENTEXT";
                        case 59: return "FEHLENDERBEDIENTEXT";
                    }
                    break;
            }
            return "INVALID STRING";
        }

    }
}