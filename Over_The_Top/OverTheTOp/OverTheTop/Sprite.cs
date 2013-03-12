using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OverTheTop.Enemies;

namespace OverTheTop
{
    /// <summary>
    /// The Sprite class handles the drawing of the game 
    /// </summary>
    class Sprite
    {
        #region Map variables

        private Texture2D _mapTexture, _destroyedMapTexture;
        public static Boolean MapDestroyed = false;
        private readonly Vector2 _mapPosition = new Vector2(0, 0);
        #endregion

        #region Tank related variables
        //current position of the sprite
        public Vector2 Location;

        //texture object that is used when drawing the sprite
        private Texture2D _tankBody, _tankTurret;

        //name of the sprites texture
        public string Turret, Body;

        //rectangle size
        public Rectangle TurretSize, BodySize, TankRect;

        //rotation of the turret and tank body
        public float TurretRotation, BodyRotation;

        //for setting the origin of rotation
        private Vector2 _turretSource, _bodySource;
        #endregion 

        #region Troop related variables
        //Spawn location of the troop
        public Vector2 TroopSpawnLocation;

        //Texture used to draw the troop
        public Texture2D TroopTexture;

        //Name of the sprite texture
        public string TroopTextureString;

        //rectangle size
        public Rectangle TroopSize;

        //Rotation of the troop
        public float TroopRotation;

        //Setting the origin of rotation
        public Vector2 TroopSource;

        //A list for each enemy troop
        private List<FootSoldier> _enemyFoots; 

        #endregion

        #region Player Rocket Variables

        public Vector2 PRocketLocation;

        private Texture2D _pRocketTexture;

        private Rectangle _pRocketSize;

        #endregion

        #region Player Bullet Variables

        // current location of the bullet on screen
        public Vector2 PBulletLocation;

        //old location of the bullet
        public Vector2 PBulletOldLocation;

        //texture that holds the bullet image
        public Texture2D PBulletTexture;

        //name of the bullet image
        private string _pBullet;

        //rectangle that holds the bullet texture size
        private Rectangle _pBulletSize;

        #endregion

        #region Custom Cursor Variables

        //Variables for the cursor texture, the location path and the vector
        private Texture2D cursorTexture;
        private string cursorName;
        private Vector2 cursorLocation;

        #endregion 

        #region Debug Variables to be moved or deleted eventually

        public int CanYouSeeMe;
        #endregion

        public Sprite()
        {
            _enemyFoots = new List<FootSoldier>();
        }

        #region Load Cursor Content
        public void LoadCustomCursor(ContentManager contentManager, string cursorName, Vector2 cursorPosition)
        {
            //Load the content of the texture and set the location
            cursorTexture = contentManager.Load<Texture2D>(cursorName);
            cursorLocation = cursorPosition;
        }
        #endregion

        #region Load Tank Content
        public void LoadTankContent(ContentManager contentManager, Vector2 location, string spriteTurret, string spriteBody)
        {

            //Load the location
            Location = new Vector2(location.X, location.Y);

            //Load the tank turret
            _tankTurret = contentManager.Load<Texture2D>(spriteTurret);
            Turret = spriteTurret;
            TurretSize = new Rectangle(0, 0, _tankTurret.Width, _tankTurret.Height);
            _turretSource = new Vector2(45, 20);

            //Load the tank body
            _tankBody = contentManager.Load<Texture2D>(spriteBody);
            //Load the map
            _mapTexture = contentManager.Load<Texture2D>(@"Images\map");
            //Load the destroyed map
            _destroyedMapTexture = contentManager.Load<Texture2D>(@"Images\mapDestroyed");

            //Set the body
            Body = spriteBody;
            BodySize = new Rectangle(0, 0, _tankBody.Width, _tankBody.Height);
            _bodySource = new Vector2(_tankBody.Width/2, _tankBody.Height/2);
        }
        #endregion

        #region Load Troop Content
        public void LoadTroopContent(ContentManager contentManager, string spriteTroop)
        {
            //Load the troop texture and set the variables
            TroopTexture = contentManager.Load<Texture2D>(spriteTroop);
            TroopSize = new Rectangle(0, 0, TroopTexture.Width, TroopTexture.Height);
            TroopSource = new Vector2(TroopTexture.Width/2, TroopTexture.Height/2);
        }
        #endregion

        #region Load Bullet Content
        public void PlayerBulletLoadContent(ContentManager contentManager, string pBulletName, Vector2 bulletPosition)
        {
            //Load the bullet content and set the variables
            PBulletLocation = bulletPosition;
            PBulletTexture = contentManager.Load<Texture2D>(pBulletName);
            _pBulletSize = new Rectangle(0, 0, PBulletTexture.Width, PBulletTexture.Height);
        }
        #endregion

        #region Load Rocket Content
        public void PlayerRocketLoadContent(ContentManager contentManager, string pRocketName, Vector2 rocketPosition)
        {
            //Load the rocket content and set the variables
            PRocketLocation = rocketPosition;
            _pRocketTexture = contentManager.Load<Texture2D>(pRocketName);
            _pRocketSize = new Rectangle(0, 0, _pRocketSize.Width, _pRocketSize.Height);
        }
        #endregion

        #region Update Tank Movement
        //update tank sprite
        public void UpdateTankMovement(GameTime gameTime, Vector2 speed, Vector2 direction, float turretRotation)
        {
            //Set the location
            Location += direction*speed*(float) gameTime.ElapsedGameTime.TotalSeconds;
            TurretRotation = turretRotation;
        }
        #endregion

        #region Draw Tank Movement
        //draw sprite
        public void DrawTank(SpriteBatch spriteBatch)
        {
            //Draw the tank movement
            FootSoldier.PlayerLocation(Location);

            //If the map isn't destroyed, use the normal map texture, if it is desryoed load the other one
            spriteBatch.Draw(!MapDestroyed ? _mapTexture : _destroyedMapTexture, _mapPosition, Color.White);

            //Draw the tank body and turret
            spriteBatch.Draw(_tankBody, Location, BodySize,
                Color.White, BodyRotation, _bodySource, 1.0f, SpriteEffects.None, 1);
            spriteBatch.Draw(_tankTurret, Location, TurretSize,
            Color.White, TurretRotation, _turretSource, 1.0f, SpriteEffects.None, 1);
            //Draw the hud
            spriteBatch.DrawString(OverTheTop.GameFont, "Funeral Fund: "+PlayerTank.PlayerScore.ToString(), new Vector2(0, 0), Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
            spriteBatch.DrawString(OverTheTop.GameFont, "Health: "+PlayerTank.PlayerHealth.ToString(), new Vector2(1000, 0), Color.White);
            spriteBatch.DrawString(OverTheTop.GameFont, "Smart Bombs: " + PlayerTank.SmartBombs.ToString(), new Vector2(600, 0), Color.White);



        }
        #endregion

        #region Draw Troop Movement
        //Draw troop
        public void DrawTroop(SpriteBatch spriteBatch, Vector2 troopLocation)
        {
            //Draw the troop movement
            spriteBatch.Draw(TroopTexture, troopLocation, TroopSize,
                                Color.White, TroopRotation, TroopSource, 1.0f, SpriteEffects.None, 1);
        }
        #endregion

        #region Update Cursor Movement

        public void UpdateCursor(GameTime gameTime, Vector2 cursorLoc)
        {
            //Set the cursor location
            cursorLocation = cursorLoc;
        }

        #endregion

        #region Draw Cursor

        public void DrawCursor(SpriteBatch spriteBatch)
        {
            //Draw the cursor
            spriteBatch.Draw(cursorTexture, cursorLocation, new Rectangle(0, 0, cursorTexture.Width, cursorTexture.Height), 
                Color.White, 0f, new Vector2(cursorTexture.Width/2, cursorTexture.Height/2),
                1f, SpriteEffects.None, 1.0f);

        }

        #endregion







    }
}
