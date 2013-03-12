using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OverTheTop.Enemies;

namespace OverTheTop
{
    internal class Projectile : Sprite
    {
        #region Bullet properties
        //String that holds the name of the location of the bullet image
        public String BulletName = "Images/bullet";

        //position of the bullet
        public Vector2 BulletPosition;

        //speed of the bullet
        private const float BulletSpeed = 10f;
        //direction of the bullet
        private readonly Vector2 _bulletDirection = Vector2.Zero;

        public bool IsVisible { get; set; }
        #endregion

        #region Rocket Properties
        //String that holds the name of the location of the rocket image
        public String RocketName = "Images/rocket";

        //variable to hold rocket position
        public Vector2 RocketPosition;

        //Variable to hold the angle the rocket faces
        public float RocketRotation;

        //speed of the rocket
        private const float RocketSpeed = 5f;
        //direction of the rocket
        private readonly Vector2 _rocketDirection = Vector2.Zero;
        #endregion

        public String RocketExplosion = "Images/Explosion";

        private List<FootSoldier> footSoldiers;
        private FootSoldier footSoldier;

        #region Projectile Contructor
        public Projectile(float locationX, float locationY, float pAngle )
        {
            //Set the positions
            BulletPosition = new Vector2(locationX, locationY);
            RocketPosition = new Vector2(locationX, locationY);

            //Set directions
            _bulletDirection = new Vector2((float)Math.Cos(pAngle)*BulletSpeed, (float)Math.Sin(pAngle)*BulletSpeed);
            _rocketDirection = new Vector2((float)Math.Cos(pAngle)*RocketSpeed, (float)Math.Sin(pAngle)*RocketSpeed);

            //set rotation
            RocketRotation = pAngle;

            footSoldiers = EnemyController.footSoldiers;
            footSoldier = EnemyController.footSoldier;


        }
        #endregion

        #region Load Proj Content
        public void LoadProjectileContent(ContentManager contentManager)
        {
            //Load content
            PlayerBulletLoadContent(contentManager, BulletName, BulletPosition);
            PlayerRocketLoadContent(contentManager, RocketName, RocketPosition);
        }
        #endregion

        #region update bullet
        public Boolean UpdateBullet(GameTime gameTime)
        {
            //Cbeck for collision detection and to see if it has gone off screen
            Boolean removeBullet = false;
            BulletPosition -= _bulletDirection;
            if(CollisionDetectBullet())
            {
                removeBullet = true;
            }

            if(PBulletLocation.X > 1280 || PBulletLocation.X < 0)
            {
                removeBullet = true;
            }

            if(PBulletLocation.Y > 720 || PBulletLocation.Y < 0)
            {
                removeBullet = true;
            }

            return removeBullet;
        }
        #endregion

        #region collision detect bullet
        public Boolean CollisionDetectBullet()
        {
            Boolean collisionDetected = false;

            Rectangle collision = new Rectangle(
                (int) BulletPosition.X,
                (int) BulletPosition.Y,
                1,
                1
                );

            //Check if each bullet intersects each footsoldier
            foreach (var footSoldier in footSoldiers.ToList())
            {
                Rectangle footSoldierRect = new Rectangle(
                    (int)footSoldier.TroopLocation.X - footSoldier.TroopTexture.Width/2,
                    (int)footSoldier.TroopLocation.Y - footSoldier.TroopTexture.Height/2,
                    footSoldier.TroopTexture.Width,
                    footSoldier.TroopTexture.Height
                    );

                if(collision.Intersects(footSoldierRect))
                {
                    footSoldiers.Remove(footSoldier);
                    collisionDetected = true;
                    PlayerTank.PlayerScore += 10;
                }

            }
            return collisionDetected;
        }
        #endregion

        #region Collision detect rocket
        public Boolean CollisionDetectRocket(SpriteBatch spriteBatch)
        {
            
            Boolean collisionDetected = false;

            Rectangle collision = new Rectangle(
            (int)RocketPosition.X,
            (int)RocketPosition.Y,
            1,
            1
            );
            //Collision detect the rocket against each soldier
            foreach (var footSoldier in footSoldiers.ToList())
            {
                Rectangle footSoldierRect = new Rectangle(
                    (int)footSoldier.TroopLocation.X - footSoldier.TroopTexture.Width / 2,
                    (int)footSoldier.TroopLocation.Y - footSoldier.TroopTexture.Height / 2,
                    footSoldier.TroopTexture.Width,
                    footSoldier.TroopTexture.Height
                    );

                if (collision.Intersects(footSoldierRect))
                {
                    //Render a circle and then collision detect it
                    //Circle doesnt work. Rocket functions as a smart 
                    spriteBatch.Begin();
                    spriteBatch.Draw(GameManager._rocketExplosion, footSoldier.TroopLocation, Color.White);
                    
                    spriteBatch.End();

                    footSoldiers.Clear();
                    collisionDetected = true;

                    PlayerTank.PlayerScore += 200;
                }

            }

            return collisionDetected;
        }
        #endregion

        #region update rocket
        public Boolean UpdateRocket(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Boolean removeRocket = false;
            //Update the rocket
            RocketPosition -= _rocketDirection;
            if(CollisionDetectRocket(spriteBatch))
            {
                removeRocket = true;
                
                //particleEngine.
                //particleEngine.Draw(spriteBatch);
            }
            return removeRocket;
        }
        #endregion

        #region draw bullet
        public void DrawBullet(SpriteBatch spriteBatch, Texture2D pBulletTexture)
        {
            //Draw the bullet
            spriteBatch.Draw(pBulletTexture, BulletPosition, new Rectangle(0, 0, pBulletTexture.Width, pBulletTexture.Height), Color.Black, 0f,
                new Vector2(pBulletTexture.Width/2, pBulletTexture.Height/2),
                2.0f, SpriteEffects.None, 1.0f);
        }
        #endregion

        #region draw the rocket
        public void DrawRocket(SpriteBatch spriteBatch, Texture2D pRocketTexture)
        {
            //Draw the rocket
            spriteBatch.Draw(pRocketTexture, RocketPosition, new Rectangle(0, 0, pRocketTexture.Width, pRocketTexture.Height), Color.White, RocketRotation,
                new Vector2(pRocketTexture.Width/2, pRocketTexture.Height/2),
                1.0f, SpriteEffects.None, 1.0f);   
        }
        #endregion

    }
}
