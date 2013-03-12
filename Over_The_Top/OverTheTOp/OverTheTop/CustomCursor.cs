using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OverTheTop
{
    /// <summary>
    /// CustomCursor class that is a child of the sprite class in order to access it's methods.
    /// This class allows the standard mouse cursor to be changed with a customised one
    /// </summary>
    class CustomCursor : Sprite
    {
        //declare a spriteBatch
        private SpriteBatch _spriteBatch;

        //declare and initialise a string that holds the location of the image
        private const String _cursor = "Images/tempCursor";

        //vector that will hold the current position of the cursor
        private Vector2 _cursorPosition;

        /// <summary>
        /// Load content that is needed for the cursor
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            //call the LoadCustomCursor method in the sprite class passing in the contentManager,
            //cursor image locaion and the position of the cursor

            base.LoadCustomCursor(contentManager, _cursor, _cursorPosition);

        }

        /// <summary>
        /// This method will update the cursor position with the position of
        /// the mouse
        /// </summary>
        /// <param name="mouse"></param>
        public void UpdateCursorPosition(MouseState mouse)
        {
            _cursorPosition.X = mouse.X;
            _cursorPosition.Y = mouse.Y;
        }

        /// <summary>
        /// Update method will be called 60 times per second and will update the 
        /// cursor position and then call the UpdateCursor method from the Sprite class
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            MouseState mouse = Mouse.GetState();
            UpdateCursorPosition(mouse);
            base.UpdateCursor(gameTime, _cursorPosition);
        }
        
    }
}
