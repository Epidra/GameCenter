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
    class Sudoku : Ghost {

        Vector2 selector_grid = new Vector2(4,4);
        int selector_number = 0;

        bool active_selection = false;

        int[,] grid        = new int[9,9];
        int[,] grid_mirror = new int[9,9];

        bool match;

        public Sudoku(string _id, ContentManager _content, ShopKeeper _Shopkeeper, FileManager _Filemanager, JukeBox _Jukebox, float screenX, float screenY) : base(_id, _content, _Shopkeeper, _Filemanager, _Jukebox, screenX, screenY) {
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
            match = false;
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    grid[x, y] = 0;
                }
            }

            int r = random.Next(9)+1;
            grid[4, 4] = r;
            grid_mirror[4, 4] = r;
            Generate_Square(5, 3, 0);
            Generate_Square(5, 0, 3);
            Generate_Square(5, 6, 3);
            Generate_Square(5, 3, 6);

            Generate_Square(5, 0, 0);
            Generate_Square(5, 6, 0);
            Generate_Square(5, 0, 6);
            Generate_Square(5, 6, 6);
        }

        private void Generate_Square(int index2, int xi, int yi) {
            int index = 1;
            while(index < 5) {
                int x = random.Next(3);
                int y = random.Next(3);
                int i = random.Next(9);
                if(grid[0, yi + y] != i && grid[1, yi + y] != i && grid[2, yi + y] != i && grid[3, yi + y] != i && grid[4, yi + y] != i && grid[5, yi + y] != i && grid[6, yi + y] != i && grid[7, yi + y] != i && grid[8, yi + y] != i) {
                    if(grid[xi + x, 0] != i && grid[xi + x, 1] != i && grid[xi + x, 2] != i && grid[xi + x, 3] != i && grid[xi + x, 4] != i && grid[xi + x, 5] != i && grid[xi + x, 6] != i && grid[xi + x, 7] != i && grid[xi + x, 8] != i) {
                        if(grid[xi + 0, yi + 0] != i && grid[xi + 1, yi + 0] != i && grid[xi + 2, yi + 0] != i && grid[xi + 0, yi + 1] != i && grid[xi + 1, yi + 1] != i && grid[xi + 2, yi + 1] != i && grid[xi + 0, yi + 2] != i && grid[xi + 1, yi + 2] != i && grid[xi + 2, yi + 2] != i) {
                            grid[xi + x, yi + y] = i;
                            grid_mirror[xi + x, yi + y] = i;
                            index++;
                        }
                    }
                }
            }
        }

        private void Select() {
            if(grid_mirror[(int)selector_grid.X, (int)selector_grid.Y] == 0) {
                active_selection = true;
            }
        }

        private void Insert() {
            grid[(int)selector_grid.X, (int)selector_grid.Y] = selector_number + 1;
            Check();
            active_selection = false;
        }

        private void Check() {
            bool[] match_vert = new bool[9];
            bool[] match_hori = new bool[9];
            bool[] match_cube = new bool[9];
            int[] n = new int[10];
            for(int i = 0; i < 9; i++) { n[i] = 0; }

            for(int y = 0; y < 9; y++) {
                for(int i = 0; i < 9; i++) { n[i] = 0; }
                for(int x = 0; x < 9; x++) {
                    n[grid[x, y]]++;
                }
                match_vert[y] = false;
                if(n[1] == 1 && n[2] == 1 && n[3] == 1 && n[4] == 1 && n[5] == 1 && n[6] == 1 && n[7] == 1 && n[8] == 1 && n[9] == 1) match_vert[y] = true;
            }

            for(int x = 0; x < 9; x++) {
                for(int i = 0; i < 9; i++) { n[i] = 0; }
                for(int y = 0; y < 9; y++) {
                    n[grid[x, y]]++;
                }
                match_hori[x] = false;
                if(n[1] == 1 && n[2] == 1 && n[3] == 1 && n[4] == 1 && n[5] == 1 && n[6] == 1 && n[7] == 1 && n[8] == 1 && n[9] == 1) match_hori[x] = true;
            }

            for(int y = 0; y < 3; y++) {
                for(int x = 0; x < 3; x++) {
                    for(int i = 0; i < 9; i++) { n[i] = 0; }
                    for(int yi = y * 3; yi < y * 3 + 3; yi++) {
                        for(int xi = x * 3; xi < x * 3 + 3; xi++) {
                            n[grid[xi, yi]]++;
                        }
                    }
                    match_cube[y * 3 + x] = false;
                    if(n[1] == 1 && n[2] == 1 && n[3] == 1 && n[4] == 1 && n[5] == 1 && n[6] == 1 && n[7] == 1 && n[8] == 1 && n[9] == 1) match_hori[y * 3 + x] = true;
                }
            }

            match = true;
            for(int i = 0; i < 9; i++) {
                if(!match_vert[i]) match = false;
                if(!match_hori[i]) match = false;
                if(!match_cube[i]) match = false;
            }
        }

        public override string Update2() {
            if(ButtonPressed(GhostKey.button_function_P1) == GhostState.pressed) { if(!active_gameover) pressed_response = true; }
            if(ButtonPressed(GhostKey.button_ok_P1) == GhostState.pressed) { if(active_selection) { Insert(); } else { Select(); } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_up_P1) == GhostState.pressed) { if(!active_selection) { if(selector_grid.Y > 0) selector_grid.Y--; } else { } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_down_P1) == GhostState.pressed) { if(!active_selection) { if(selector_grid.Y < 8) selector_grid.Y++; } else { } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_left_P1) == GhostState.pressed) { if(!active_selection) { if(selector_grid.X > 0) selector_grid.X--; } else { if(selector_number > 0) selector_number--; } pressed_response = true; }
            if(ButtonPressed(GhostKey.arrow_right_P1) == GhostState.pressed) { if(!active_selection) { if(selector_grid.X < 8) selector_grid.X++; } else { if(selector_number < 8) selector_number++; } pressed_response = true; }
            if(pressed_event_touch) {
                pressed_response = true;
                for(int y = 0; y < 9; y++) {
                    for(int x = 0; x < 9; x++) {
                        if(Collision_Button(false, new Rectangle((int)SK.Position_Sudoku_Grid().X + 64 * x, (int)SK.Position_Sudoku_Grid().Y + 64 * y, 64, 64))) {
                            if(selector_grid == new Vector2(x, y)) {
                                if(!active_selection) { Select(); }
                            } else {
                                selector_grid = new Vector2(x, y);
                            }
                        }
                    }
                }
                for(int x = 0; x < 9; x++) {
                    if(Collision_Button(false, new Rectangle((int)SK.Position_Sudoku_Selection().X + 64 * x, (int)SK.Position_Sudoku_Selection().Y, 64, 64))) {
                        if(selector_number == x) {
                            if(active_selection) { Insert(); }
                        } else {
                            selector_number = x;
                        }
                    }
                }
            }
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    if(Collision_Button(true, new Rectangle((int)SK.Position_Sudoku_Grid().X + 64 * x, (int)SK.Position_Sudoku_Grid().Y + 64 * y, 64, 64))) { if(!active_selection) selector_grid = new Vector2(x, y); }
                }
            }
            for(int x = 0; x < 9; x++) {
                if(Collision_Button(true, new Rectangle((int)SK.Position_Sudoku_Selection().X + 64 * x, (int)SK.Position_Sudoku_Selection().Y, 64, 64))) { if(active_selection) selector_number = x; }
            }
            return "null";
        }

        public override string Update3(GameTime gameTime) {
            if(match && !active_gameover) {
                JK.Noise("Cleared");
                score_points = 1;
                GameOver(gameTime.TotalGameTime.TotalSeconds);
            }
            return "void";
        }

        public override void Draw2() {
            spriteBatch.Draw(SK.texture_background_sudoku, SK.Position_Sudoku_Grid(), Color.White);
            for(int y = 0; y < 9; y++) {
                for(int x = 0; x < 9; x++) {
                    if(grid_mirror[x, y] != 0) {
                        if(grid[x, y] != 0) spriteBatch.Draw(SK.texture_spritesheet_sudoku, SK.Position_Sudoku_Grid() + new Vector2(x * 64, y * 64), new Rectangle(1 + Get_Number(true, grid[x, y]) * 65, 1 + Get_Number(false, grid[x, y]) * 65, 64, 64), Color.Black, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    } else {
                        if(grid[x, y] != 0) spriteBatch.Draw(SK.texture_spritesheet_sudoku, SK.Position_Sudoku_Grid() + new Vector2(x * 64, y * 64), new Rectangle(1 + Get_Number(true, grid[x, y]) * 65, 1 + Get_Number(false, grid[x, y]) * 65, 64, 64), Color.White, 0.0f, new Vector2(0, 0), 1, SpriteEffects.None, 0.0f);
                    }
                }
            }

            if(active_selection) {
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_Sudoku_Grid() + new Vector2(selector_grid.X * 64, selector_grid.Y * 64), new Rectangle(0, 0, 64, 64), Color.Azure * 1.00f);
            } else {
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_Sudoku_Grid() + new Vector2(selector_grid.X * 64, selector_grid.Y * 64), null, Color.Azure * 0.50f);
            }

            for(int x = 0; x < 9; x++) {
                spriteBatch.Draw(SK.texture_spritesheet_sudoku, SK.Position_Sudoku_Selection() + new Vector2(x * 64, 0), new Rectangle(1 + Get_Number(true, x + 1) * 65, 1 + Get_Number(false, x + 1) * 65, 64, 64), Color.White);
            }
            if(active_selection) {
                spriteBatch.Draw(SK.texture_spritesheet_minos_64x, SK.Position_Sudoku_Selection() + new Vector2(selector_number * 64, 0), new Rectangle(0, 0, 64, 64), Color.Blue * 0.50f);
            }
        }

        private int Get_Number(bool xy, int i) {
            if(xy) {
                if(i == 1) return 0;
                if(i == 2) return 1;
                if(i == 3) return 2;
                if(i == 4) return 0;
                if(i == 5) return 1;
                if(i == 6) return 2;
                if(i == 7) return 0;
                if(i == 8) return 1;
                if(i == 9) return 2;
            } else {
                if(i == 1) return 0;
                if(i == 2) return 0;
                if(i == 3) return 0;
                if(i == 4) return 1;
                if(i == 5) return 1;
                if(i == 6) return 1;
                if(i == 7) return 2;
                if(i == 8) return 2;
                if(i == 9) return 2;
            }
            return i;
        }

    }
}
