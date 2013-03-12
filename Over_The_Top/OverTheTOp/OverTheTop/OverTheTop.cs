using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OverTheTop.Enemies;

namespace OverTheTop
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class OverTheTop : Microsoft.Xna.Framework.Game
    {
        //manages the graphics card
        readonly GraphicsDeviceManager _graphics;

        //used for holding the current gamestate
        GameState _currentGameState = GameState.Start;

        //declaration of instance of GameManager and EndGame class
        private GameManager _inGame;
        private EndGame _endGame;

        //declaration of a new spriteFont
        public static SpriteFont MenuFont;
        public static SpriteFont GameFont;

        //declaration of a new SpriteBatch
        private SpriteBatch _spriteBatch;

        //declaration of a new song 'gameOver'
        private SoundEffect _gameOver;

        //declaration of a new keyboardState
        private KeyboardState _keyboardState;

        //A boolean to say whether music is currently playing
        private Boolean _musicPlaying;

        //A texture which holds the game over image
        private Texture2D _youLoseText;


        //declaration of a new enumerated GameState
        private enum GameState
        {
            Start,
            InGame,
            GameOver
        };

        /// <summary>
        /// Constructor for OverTheTop. Sets up the graphics device, viewport
        /// width and height, and if the game plays in full screen 
        /// </summary>
        public OverTheTop()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.LoadContent will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            IsMouseVisible = false;
            //calls the LoadContent method
            LoadContent();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //initalises menuFont and gameFont with created SpriteFonts
            MenuFont = Content.Load<SpriteFont>("Fonts/MenuFont");
            GameFont = Content.Load<SpriteFont>("Fonts/GameFont");

            _gameOver = Content.Load<SoundEffect>(@"Sound/youLost");

            //initialises _gameOver to the youLost sound
            _gameOver = Content.Load<SoundEffect>(@"Sound/youLost");



            _youLoseText = Content.Load<Texture2D>(@"Images/gameOver");

            //loads _youLoseText with the losing screen image
            _youLoseText = Content.Load<Texture2D>(@"Images/gameOver");

            //Initiliase the two game states
            _inGame = new GameManager(Content);
            _endGame = new EndGame(Content);


            _inGame = new GameManager(Content);
            _endGame = new EndGame(Content);


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here

        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _keyboardState = Keyboard.GetState();
            //switch statement that performs update based
            //on the current gamestate
            switch (_currentGameState)
            {
                case GameState.Start:

                    if (_keyboardState.IsKeyDown(Keys.Enter))

                    if(_keyboardState.IsKeyDown(Keys.Enter))

                    {
                        _currentGameState = GameState.InGame;
                    }
                    break;
                case GameState.InGame:
                    _inGame.Update(gameTime, _spriteBatch);
                    if((PlayerTank.PlayerScore > 1000) || PlayerTank.PlayerHealth <=0)
                    {
                        _currentGameState = GameState.GameOver;
                    }
                    break;
                case GameState.GameOver:
                    
                    break;
            }

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        #region Draw
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();
            //switch statement that performs draw based
            //on the current gamestate
            switch (_currentGameState)
            {   
                case GameState.Start:
                    
                    //Welcome message
                    GraphicsDevice.Clear(Color.DarkOliveGreen);
                    _spriteBatch.DrawString(MenuFont, "Welcome to Over The Top, commander!", new Vector2(440, 50), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "Your objective is to test our new tank the Wertlos Kampfpanzer II", new Vector2(340, 100), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "In this... err... simulation a series of \"simulated\" enemies will", new Vector2(340, 150), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "attack you until you die. The score you tally is the amount", new Vector2(340, 200), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "we'll spend on your funeral! So try hard, ya hear?", new Vector2(380, 250), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "WASD to move, LMB to fire machine gun, RMB to fire smart bomb", new Vector2(340, 300), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "If you can make it to a 1000 points there's a surprise from HQ", new Vector2(340, 350), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "(Oh, if you happen to be a lecturer pushed for time, right shift will increase your score)", new Vector2(240, 400), Color.Black);
                    _spriteBatch.DrawString(MenuFont, "Press Enter to Begin", new Vector2(520, 600), Color.Brown);
                    break;
                case GameState.InGame:
                    //Playing the game
                    GraphicsDevice.Clear(Color.SaddleBrown);
                    _inGame.Draw(_spriteBatch);
                    break;
                case GameState.GameOver:
                    //If you win
                    if(PlayerTank.PlayerScore >= 1000)
                    {
                        _endGame.Draw(_spriteBatch);
                    }
                        //Else you lose
                    else
                    {
                        if (!_musicPlaying)

                        //If music isn't currently playing then play the music
                        if (!_musicPlaying)

                        {
                            _gameOver.Play();
                            _musicPlaying = true;
                        }
                        //Draw the screen to have the you lost information on it.
                        GraphicsDevice.Clear(Color.DarkGray);

                        _spriteBatch.Draw(_youLoseText, new Vector2(0, 0), Color.White);

                        _spriteBatch.DrawString(MenuFont, "Whaaaat?! You only scored " + PlayerTank.PlayerScore + "?!",
                            new Vector2(900, 150), Color.White);

                        _spriteBatch.DrawString(MenuFont, "Worse, you've ruined our prototype tank!", new Vector2(900, 200), Color.White);
                        _spriteBatch.DrawString(MenuFont, "Now what are we going to use to \ndrive to Tesco for a carryout?", new Vector2(900, 250), Color.White);
                        _spriteBatch.DrawString(MenuFont, "Eugh! What a mess...", new Vector2(900, 300), Color.White);
                        _spriteBatch.DrawString(MenuFont,
                                               "Created by Ronan O'Cuinn and Eoin Bleeks, Group 42. Thanks for playing!",
                                               new Vector2(400, 700), Color.White);
                    }
                    break;
            }
            _spriteBatch.End();
        }
        #endregion
    }
}
