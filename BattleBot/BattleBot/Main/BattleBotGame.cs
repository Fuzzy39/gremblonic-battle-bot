using BattleBot.Components;
using BattleBot.Systems;
using EngineCore;
using EngineCore.Rendering;
using EngineCore.Util;
using EngineCore.Util.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
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
          


            engine = new(GraphicsDevice);
            new PixelRenderingSystem(engine);
            new CameraRenderingSystem(engine);
            new CircleMovementSystem(engine);
            engine.Initialize();

      

            base.Initialize();
           
        }

        protected override void LoadContent()
        {
            assetManager = new(Content);
            Entity e;

            // camera
            e = new Entity(engine);
            e.AddComponent(new PixelBounds()
            {
                Bounds = new RotatedRect(new Rectangle(0, 0, 1600, 900), 0f, new(0,0))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.BackgroundDark),
                Tint = Color.White
            });
            e.AddComponent(new CameraComponent()
            {
                Position = new Vector2(.5f, .5f),
                Scale = 100f,
                Rotation = Angle.FromDegrees(0)
            });
            e.StopEditing();


            e = new Entity(engine);
            e.AddComponent(new PixelBounds()
            {
                Bounds = new RotatedRect(new Rectangle(1200, 0, 400, 225), 0f, new(0, 0))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.BackgroundDark),
                Tint = Color.White
            });
            e.AddComponent(new CameraComponent()
            {
                Position = new Vector2(.5f, .5f),
                Scale = 25f,
                Rotation = Angle.FromDegrees(0)
            });
            e.StopEditing();


            // make an example entity.
            e =  MakeSimpleEntity(new Rectangle(0, 0, 2, 2), TextureAsset.TestSquare );
            e.StopEditing();

            // make another example entity
            e = MakeSimpleEntity(new Rectangle(-1, -1, 1, 1), TextureAsset.TestSquare);
            e.StopEditing();


            // make an example entity.
            e = new Entity(engine);
            e.AddComponent(new WorldBounds()
            {
                Bounds = new RotatedRect(new RectangleF(-.5f, 1f, 1, 2), Angle.FromDegrees(45).Radians, new(.5f, .5f))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.Bot),
                Tint = Color.White
            });
         
            e.StopEditing();
        }

        // yeah, we're 100% going to need to sort out the entity creating.
        // the thing with making everything data is that data really doesn't belong in code.
        private Entity MakeSimpleEntity(Rectangle bounds, TextureAsset text)
        {
            Entity toReturn = new(engine);
            toReturn.AddComponent(new WorldBounds()
            {
                Bounds = new RotatedRect(bounds, 0f, new(0, 0))
            });

            toReturn.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(text),
                Tint = Color.White
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
