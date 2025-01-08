using EngineCore.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleBot.Main
{
    public class BattleBotGame : Game
    {
     
        private IBatchRenderer renderer;
        private AssetManager assetManager;

        public BattleBotGame()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            renderer = new ScaledRenderer(GraphicsDevice);
            base.Initialize();
            Window.AllowUserResizing = true;
        }

        protected override void LoadContent()
        {
            assetManager = new(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

          
            renderer.Begin();
            renderer.Draw(assetManager.getTexture(TextureAsset.TestSquare), new Rectangle(0, 0, 200, 200), Color.White);
            renderer.End();

            base.Draw(gameTime);
        }
    }
}
