using BattleBot.Components;
using BattleBot.Services;
using EngineCore;
using EngineCore.Components;
using EngineCore.Rendering;
using EngineCore.Util;
using EngineCore.Util.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BattleBot.Main
{
    public class BattleBotGame : Game
    {

        private Engine engine;
        private AssetManager assetManager;

        public BattleBotGame()
        {
            var graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Window.AllowUserResizing = true;

            // setup engine systems
            engine = new(GraphicsDevice);
            new CircleMovementService(engine);
            new CamTestService(engine);
            engine.Initialize();

      

            base.Initialize();
           
        }

        protected override void LoadContent()
        {
            assetManager = new(Content);
            GameSetup setup = new GameSetup(assetManager, engine);
            setup.ChunkTest();

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
           

            engine.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
