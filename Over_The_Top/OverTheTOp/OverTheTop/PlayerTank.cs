using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using OverTheTop.Enemies;

namespace OverTheTop
{
    class PlayerTank : Sprite
    {
        //Audio playing variables
        private SoundEffect _squish, _buildingCollapse, _gunShot, _rocketFire;


        //String that holds the name of the images for the tank
        public String TankBodyName = "Images/body";
        public String TankTurretName = "Images/turret";

        //cooldown time for shooting bullets
        public float ShootBulletCooldown = 0.50f;

        //cooldown time for shooting rockets
        public float ShootRocketCooldown = 5.0f;

        //Player Score
        public static float PlayerScore = 0;

        //Number of smart bombs
        public static int SmartBombs = 3;

        //Player Health
        public static float PlayerHealth = 100;

        //set the starting position of the tank
        private const int StartPositionX = 200;
        private const int StartPositionY = 200;

        //set the acceleration speed of the tank 
        private const float TankAcceleration = 0.70f;
        private const float ReverseAcceleration = 10f;

        //set the max speed of the tank
        private const float MaxSpeed = 35.0f;

        //set a value for friction
        private const float Friction = 0.70f;

        //sets the speed for moving at an angle
        private const float TanVelocity = 9f;

        //initialise vector2 for the location of the mouse and the direction the turret is facing
        public Vector2 MouseLocation { get; set; }
        private Vector2 _turretDirection;

        //set the start speed and direction set to 0
        private Vector2 _direction = Vector2.Zero;
        private Vector2 _speed = Vector2.Zero;

        private List<Projectile> bulletList;
        private List<Projectile> rocketList;

        private Boolean[] _konamiCodes = new bool[10];

        private KeyboardState keyState;
        private MouseState mouse;

        private readonly List<FootSoldier> _footSoldiers;
        private FootSoldier _footSoldier;

        public PlayerTank(List<Projectile> pBulletList, List<Projectile> pRocketList)
        {
            bulletList = pBulletList;
            rocketList = pRocketList;

            _footSoldiers = EnemyController.footSoldiers;
            _footSoldier = EnemyController.footSoldier;
        }

        public void LoadContent(ContentManager contentManager)
        {
            Location = new Vector2(StartPositionX, StartPositionY);
            base.LoadTankContent(contentManager, Location, TankTurretName, TankBodyName);
            TurretSize = new Rectangle(0, 0, TurretSize.Width, TurretSize.Height);
            BodySize = new Rectangle(0, 0, BodySize.Width, BodySize.Height);

            _squish = contentManager.Load<SoundEffect>(@"Sound\squish");
            _buildingCollapse = contentManager.Load<SoundEffect>(@"Sound\collapse");
            _gunShot = contentManager.Load<SoundEffect>(@"Sound\gunShot");
            _rocketFire = contentManager.Load<SoundEffect>(@"Sound\rocketFire");
        }


        public void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            mouse = Mouse.GetState();

            ShootBulletCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            ShootRocketCooldown -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(mouse.LeftButton == ButtonState.Pressed && ShootBulletCooldown<=0.0f)
            {
                _gunShot.Play();
                ShootBullet();
                ShootBulletCooldown = 0.25f;
            }

            if(mouse.RightButton == ButtonState.Pressed && ShootRocketCooldown<=0.0f && SmartBombs!=0)
            {
                _rocketFire.Play();
                ShootRocket();
                ShootRocketCooldown = 10.0f;
                SmartBombs -= 1;
            }

            if (keyState.IsKeyDown(Keys.RightShift))
            {
                PlayerScore = 950;
            }

            CollisionDetect();

            UpdateTankTurretMovement(mouse);
            UpdateTankBodyMovement(keyState);
            UpdateTankMovement(gameTime, _speed, _direction, TurretRotation);
        }

        public void CollisionDetect()
        {
            Rectangle BodyTank = new Rectangle((int)Location.X, (int)Location.Y, BodySize.Width, BodySize.Height);

            foreach (var footSoldier in _footSoldiers.ToList())
            {
                Rectangle footSoldierRect = new Rectangle(
                    (int)footSoldier.TroopLocation.X - footSoldier.TroopTexture.Width / 2,
                    (int)footSoldier.TroopLocation.Y - footSoldier.TroopTexture.Height / 2,
                    footSoldier.TroopTexture.Width,
                    footSoldier.TroopTexture.Height
                );

                if (BodyTank.Intersects(footSoldierRect))
                {
                    _squish.Play();
                    PlayerHealth -= 5;
                    _footSoldiers.Remove(footSoldier);
                    
                }
            }

            Rectangle House = new Rectangle(750, 240, 200, 250);

            if (BodyTank.Intersects(House) & !MapDestroyed)
            {
                MapDestroyed = true;
                _buildingCollapse.Play();
            }
        }

        public void ShootBullet()
        {
            bulletList.Add(new Projectile(Location.X, Location.Y, this.TurretRotation));
        }

        public void ShootRocket()
        {
            rocketList.Add(new Projectile(Location.X, Location.Y, this.TurretRotation));
        }

        public void CheckBounds()
        {
            if(Location.X > 1280)
            {
                Location.X = 1280;
            }
            
            if(Location.X < 0)
            {
                Location.X = 0;
            }

            if(Location.Y > 720)
            {
                Location.Y = 720;
            }

            if(Location.Y < 0)
            {
                Location.Y = 0;
            }
        }

        public void UpdateTankTurretMovement(MouseState mouse)
        {
            MouseLocation = new Vector2(mouse.X, mouse.Y);

            _turretDirection = (Location) - MouseLocation;

            TurretRotation = (float)(Math.Atan2(_turretDirection.Y, _turretDirection.X));
        }

        public void UpdateTankBodyMovement(KeyboardState keyState)
        {
            CheckBounds();

            if (keyState.IsKeyDown(Keys.W))
            {
                _speed.X += TankAcceleration;
                _speed.Y += TankAcceleration;
                _direction.X = (float)Math.Cos(BodyRotation) * TanVelocity;
                _direction.Y = (float)Math.Sin(BodyRotation) * TanVelocity;

                if((_speed.X > MaxSpeed) || (_speed.Y > MaxSpeed))
                {
                    _speed.X = MaxSpeed;
                    _speed.Y = MaxSpeed;
                }
            }   
            else if(keyState.IsKeyUp(Keys.W))
            {
                _speed.X -= Friction;
                _speed.Y -= Friction;
                _direction.X = (float)Math.Cos(BodyRotation) * TanVelocity;
                _direction.Y = (float)Math.Sin(BodyRotation) * TanVelocity;

                if((_speed.X < 3.0f) || (_speed.Y < 3.0f))
                {
                    _speed.X = 0.0f;
                    _speed.Y = 0.0f;
                }
            }

            if(keyState.IsKeyDown(Keys.S))
            {
                _speed.X -= ReverseAcceleration;
                _speed.Y -= ReverseAcceleration;
                _direction.X = (float)Math.Cos(BodyRotation) * TanVelocity;
                _direction.Y = (float)Math.Sin(BodyRotation) * TanVelocity;
            }

            if (keyState.IsKeyDown(Keys.W) && (keyState.IsKeyDown(Keys.S)))
            {
                _speed.X = 0.0f;
                _speed.Y = 0.0f;
            }

            if(keyState.IsKeyDown(Keys.A))
            {
                BodyRotation -= 0.05f;
            }

            if(keyState.IsKeyDown(Keys.D))
            {
                BodyRotation += 0.05f;
            }


        }
    }
}
