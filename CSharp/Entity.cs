using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GameCenter {
    class Entity {
            // Absolute Vectors
        Vector2 position = new Vector2(0.000f, 0.000f); // The actual Position of the Entity
        Vector2 distance = new Vector2(0.000f, 0.000f); // In-Tile Movement, controls if it moves over 32 pixel
        Vector2 velocity = new Vector2(0.000f, 0.000f); // The Speed it moves with each Tick
            // Relative Vectors
        Vector2 next = new Vector2(0,0);

        int lookDirection = 0;

        int    hp = 0;

        public Entity(int _hp, Vector2 _position, Vector2 _next) {
            hp = _hp;
            position = _position;
            distance = new Vector2(0.000f, 0.000f);
            velocity = new Vector2(0.000f, 0.000f);
            next = _next;
        }

        public Entity(int _hp, Vector2 _position, Vector2 _velocity, Vector2 _next) {
            hp = _hp;
            position = _position;
            distance = new Vector2(0.000f, 0.000f);
            velocity = _velocity;
            next = _next;
            if(velocity.X < 0) lookDirection = 1;
            if(velocity.X > 0) lookDirection = 3;
            if(velocity.Y < 0) lookDirection = 2;
            if(velocity.Y > 0) lookDirection = 0;
        }

        public void Update() { // Default
            Update(false);
        }

        public void Update(bool powered_up) { // Only for Octanom
            if(powered_up) {
                distance += (velocity * 2);
            } else {
                distance += velocity;
            }
            if(distance.X <= -32 || distance.X >= 32) { position.X += distance.X < 0 ? -32 : 32; distance.X = 0.000f; } // += distance.X < 0 ? 32 : -32; }
            if(distance.Y <= -32 || distance.Y >= 32) { position.Y += distance.Y < 0 ? -32 : 32; distance.Y = 0.000f; } // += distance.Y < 0 ? 32 : -32; }
        }

        public void Forward() {
            position.X += distance.X < 0 ? -32 : 32; distance.X = 0.000f; // += distance.X < 0 ? 32 : -32; }
            position.Y += distance.Y < 0 ? -32 : 32; distance.Y = 0.000f; // += distance.Y < 0 ? 32 : -32; }
        }

        public Vector2 Get_GridPos(int orientation) { if(orientation <= 2) return new Vector2((int)(position.X + distance.X), (int)(position.Y + distance.Y)); else return new Vector2((int)(position.Y + distance.Y), (int)(27 * 32 - position.X - distance.X)); }
        public Vector2 Get_GridPos()                {                      return new Vector2((int)(position.X + distance.X), (int)(position.Y + distance.Y)); }
        public Vector2 Get_Pos()  { return position; }
        public Vector2 Get_Vel()  { return velocity; }
        public Vector2 Get_Dis()  { return distance; }
        public Vector2 Get_Next() { return next; }
        public int     Get_HP()   { return hp; }
        public int Get_LookDirection() {
            return lookDirection;
        }
        public int Get_LookDirection(int orientation) {
            if(lookDirection == 0) if(orientation <= 2) return 0; else return 3; // Down
            if(lookDirection == 1) if(orientation <= 2) return 1; else return 0; // Left
            if(lookDirection == 2) if(orientation <= 2) return 2; else return 1; // Up
            if(lookDirection == 3) if(orientation <= 2) return 3; else return 2; // Right
            return 0;
        }

        public void Set_Pos(float x, float y) { position = new Vector2(x, y); distance = new Vector2(0.000f, 0.000f); }
        public void Set_Vel(float x, float y) {
            velocity = new Vector2(x, y);
            if(velocity.X < 0) lookDirection = 1;
            if(velocity.X > 0) lookDirection = 3;
            if(velocity.Y < 0) lookDirection = 2;
            if(velocity.Y > 0) lookDirection = 0;
        }
        public void Set_HP(int i) { hp = i; }
        public void Set_Next(Vector2 i) { next = i; }
        public void Change_HP(int i) { hp = hp + i; }
        public void Set_Motion(float x, float y) { // ???
            velocity = new Vector2(x, y);
            if(velocity.X < 0) lookDirection = 1;
            if(velocity.X > 0) lookDirection = 3;
            if(velocity.Y < 0) lookDirection = 2;
            if(velocity.Y > 0) lookDirection = 0;
            if(velocity.Y < 0) next.Y--;
            if(velocity.Y > 0) next.Y++;
            if(velocity.X < 0) next.X--;
            if(velocity.X > 0) next.X++;
        }
    }
}
