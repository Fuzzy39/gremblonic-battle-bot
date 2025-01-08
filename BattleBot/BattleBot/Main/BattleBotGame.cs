using BattleBot.Components;
using BattleBot.Systems;
using EngineCore;
using EngineCore.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BattleBot.Main
{
    public class BattleBotGame : Game
    {

        private Engine engine;
        private AssetManager assetManager;

        public BattleBotGame()
        {
            new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = true;
            engine = new(GraphicsDevice);
            new PixelRenderingSystem(engine);
            engine.Initialize();

            

            // that's rather gross.


            base.Initialize();
           
        }

        protected override void LoadContent()
        {
            assetManager = new(Content);

            // make an example entity.
            Entity e = new(engine);

            PixelCoords p = new()
            {
                bounds = new Rectangle(100, 100, 200, 200)
            };
            e.AddComponent(p);

            SimpleTexture st = new()
            {
                texture = assetManager.getTexture(TextureAsset.TestSquare),
                tint = Color.White
            };
            e.AddComponent(st);

            e.StopEditing();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            engine.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            engine.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
