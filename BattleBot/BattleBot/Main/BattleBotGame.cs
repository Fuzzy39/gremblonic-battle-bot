using BattleBot.Components;
using BattleBot.Systems;
using EngineCore;
using EngineCore.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using Util;
using Util.Graphics;

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
            new CircleMovementSystem(engine);
            engine.Initialize();

      

            base.Initialize();
           
        }

        protected override void LoadContent()
        {
            assetManager = new(Content);

            // make an example entity.
            Entity e = new(engine);

            e.AddComponent(new PixelCoords()
            {
                bounds = new RotatedRect( new Rectangle(100, 100, 200, 200), 0f, new(200,200))  
            });

            e.AddComponent(new SimpleTexture()
            {
                texture = assetManager.getTexture(TextureAsset.TestSquare),
                tint = Color.White
            });

            e.AddComponent(new PointRotation()
            {
                center = new Point(300, 300),
                radius = 200,
                rotationalVelcocity = 1f,
                otherRotationVelocity = -.4f
            });

            e.StopEditing();


            e = new(engine);

            e.AddComponent(new PixelCoords()
            {
                bounds = new RotatedRect(new Rectangle(100, 100, 100, 500), 1f, new(150, 300))
            });

            e.AddComponent(new SimpleTexture()
            {
                texture = assetManager.getTexture(TextureAsset.TestSquare),
                tint = Color.White
            });

            e.AddComponent(new PointRotation()
            {
                center = new Point(900, 450),
                radius = 250,
                rotationalVelcocity = -1.5f,
                otherRotationVelocity = 4f
            });

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
