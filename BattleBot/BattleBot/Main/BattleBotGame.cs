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
            var graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
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
            Entity e;

            // background
            e = MakeSimpleEntity(new Rectangle(0, 0, 1600, 900), TextureAsset.BackgroundDark);
            e.StopEditing();

            // make an example entity.
            e =  MakeSimpleEntity(new Rectangle(100, 100, 200, 200), TextureAsset.TestSquare );

            e.AddComponent(new PointRotation()
            {
                center = new Point(300, 300),
                radius = 200,
                rotationalVelcocity = 1f,
                otherRotationVelocity = -.4f
            });

            e.StopEditing();

            // make another example entity
            e = MakeSimpleEntity(new Rectangle(100, 100, 100, 500), TextureAsset.TestSquare);

            e.AddComponent(new PointRotation()
            {
                center = new Point(900, 450),
                radius = 250,
                rotationalVelcocity = -1.5f,
                otherRotationVelocity = 4f
            });

            e.StopEditing();
        }

        // yeah, we're 100% going to need to sort out the entity creating.
        // the thing with making everything data is that data really doesn't belong in code.
        private Entity MakeSimpleEntity(Rectangle bounds, TextureAsset text)
        {
            Entity toReturn = new(engine);
            toReturn.AddComponent(new PixelCoords()
            {
                bounds = new RotatedRect(bounds, 0f, new(0, 0))
            });

            toReturn.AddComponent(new SimpleTexture()
            {
                texture = assetManager.getTexture(text),
                tint = Color.White
            });
            return toReturn;
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
