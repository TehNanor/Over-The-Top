using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace OverTheTop
{
    internal class EndGame
    {
        private readonly SpriteFont _endGameFont;
        private readonly Video _video;
        private readonly VideoPlayer _videoPlayer;

        public EndGame(ContentManager content)
        {
            _video = content.Load<Video>(@"Movies\EndGame");
            _endGameFont = content.Load<SpriteFont>(@"Fonts\MenuFont");

            _videoPlayer = new VideoPlayer();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _videoPlayer.Play(_video);

            if (_videoPlayer.State == MediaState.Stopped)
            {
                _videoPlayer.IsLooped = true;
                _videoPlayer.Play(_video);
            }

            Texture2D videoTexture = null;

            if (_videoPlayer.State != MediaState.Stopped)
                videoTexture = _videoPlayer.GetTexture();

            if (videoTexture != null)
            {
                spriteBatch.Draw(videoTexture, new Rectangle(0, 0, 1280, 720), Color.White);

                spriteBatch.DrawString(_endGameFont, "Well Done Commander!", new Vector2(640, 100), Color.Black);
                spriteBatch.DrawString(_endGameFont, "Wasn't it all worth it in the end?", new Vector2(340, 560),
                                       Color.Black);
            }
        }
    }
}