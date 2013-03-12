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
    class GameManager
    {
        //private ParticleEngine particleEngine;
        private readonly ContentManager _contentManager;

        private readonly List<Projectile> _bulletList;
        private readonly List<Projectile> _rocketList;

        private readonly PlayerTank _tank;
        private readonly CustomCursor _cursor;

        private List<FootSoldier> _footSoldiers;

        private EnemyController _enemyController;

        //troop spawn data
        private TimeSpan _troopSpawnTime;
        private TimeSpan _previousTroopSpawnTime;

        private Texture2D _rocketTexture;
        private Texture2D _bulletTexture;
        public static Texture2D _rocketExplosion;
        //private Texture2D footSoliderTexture;


        public GameManager(ContentManager content)
        {
            _contentManager = content;

            _bulletList = new List<Projectile>();
            _rocketList = new List<Projectile>();

            _enemyController = new EnemyController();
            _footSoldiers = new List<FootSoldier>();

            _previousTroopSpawnTime = TimeSpan.Zero;
            _troopSpawnTime = TimeSpan.FromSeconds(0.5f);

            _tank = new PlayerTank(_bulletList, _rocketList);
            _cursor = new CustomCursor();

            LoadGameContent();
        }

        public void LoadGameContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            //_spriteBatch = new SpriteBatch(GraphicsDevice);

            _tank.LoadContent(_contentManager);

            _cursor.LoadContent(_contentManager);

            _enemyController.LoadContent(_contentManager);

            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(_contentManager.Load<Texture2D>(@"Images\Explosion"));
            //particleEngine = new ParticleEngine(textures, new Vector2(400, 240));

            _bulletTexture = _contentManager.Load<Texture2D>(@"Images\bullet");
            _rocketTexture = _contentManager.Load<Texture2D>(@"Images\rocket");
            _rocketExplosion = _contentManager.Load<Texture2D>(@"Images\rocketExplosion");

            // TODO: use this.Content to load your game content here
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // TODO: Add your update logic here
            _cursor.Update(gameTime);
            _tank.Update(gameTime);

            if (gameTime.TotalGameTime - _previousTroopSpawnTime > _troopSpawnTime)
            {
                _previousTroopSpawnTime = gameTime.TotalGameTime;
                _enemyController.TroopSpawner(gameTime);
            }
            

            foreach (Projectile bullet in _bulletList.ToList())
            {
                Boolean remove = bullet.UpdateBullet(gameTime);
                if (remove)
                    _bulletList.Remove(bullet);
            }

            foreach (Projectile rocket in _rocketList.ToList())
            {
                Boolean remove = rocket.UpdateRocket(gameTime, spriteBatch);
                if (remove)
                    _rocketList.Remove(rocket);
            }

            //enemyController.Update(gameTime, );
            
            //particleEngine.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //particleEngine.Update();
            

            // TODO: Update troop
            //enemyController.Update(gameTime, _spriteBatch);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Draw(SpriteBatch _spriteBatch)
        {
            _tank.DrawTank(_spriteBatch);
            _cursor.DrawCursor(_spriteBatch);

            _enemyController.Update(_spriteBatch);

            //particleEngine.Draw(_spriteBatch);

            // TODO: Add your drawing code here
            foreach (Projectile bullet in _bulletList)
            {
                bullet.DrawBullet(_spriteBatch, _bulletTexture);
            }

            foreach (Projectile rocket in _rocketList)
            {
                rocket.DrawRocket(_spriteBatch, _rocketTexture);
            }


            foreach (FootSoldier foot in _footSoldiers)
            {
                foot.Draw(_spriteBatch);
            }


        }
    }
}
