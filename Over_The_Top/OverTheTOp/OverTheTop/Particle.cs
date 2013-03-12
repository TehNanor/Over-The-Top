//Code adapted from the website http://rbwhitaker.wikidot.com/2d-particle-engine-2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OverTheTop
{
    public class Particle
    {

        public Texture2D Texture { get; set; } //the texture that is to be drawn
        public Vector2 Position { get; set; } //the position of the particle
        public Vector2 Velocity { get; set; } //the speed of the particle
        public float Angle { get; set; } //current particle rotational angle
        public float AngularVelocity { get; set; } //the speed that the angle is changing
        public Color Colour { get; set; } //the colour of the particle
        public float Size { get; set; } //the size of the particle
        public int TimeAlive { get; set; } //the amount of time before the particle is removed

        public Particle(Texture2D texture, Vector2 position, Vector2 velocity,
                    float angle, float angularVelocity, Color colour, float size, int timeAlive)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Colour = colour;
            Size = size;
            TimeAlive = timeAlive;
        }

        public void Update()
        {
            TimeAlive--;
            Position += Velocity;
            Angle = AngularVelocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            spriteBatch.Draw(Texture, Position, sourceRectangle, Colour,
                Angle, origin, Size, SpriteEffects.None, 0f);
        }
    }
}
