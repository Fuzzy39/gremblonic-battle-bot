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
          


            engine = new(GraphicsDevice);
            new PixelRenderingSystem(engine);
            new CameraRenderingSystem(engine);
            new CircleMovementSystem(engine);
            new CamTestSystem(engine);
            new InputSystem(engine);
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
                Bounds = new RotatedRect(new Rectangle(0, 0, 1600, 900), Angle.FromRadians(0f), new(0,0))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.BackgroundDark),
                Tint = Color.White,
                Depth = 0
            });
            e.AddComponent(new CameraComponent()
            {
                Position = new Vector2(0f, 0f),
                Scale = 100f,
                Rotation = Angle.FromDegrees(0),
                Depth = 0
            });
            //e.AddComponent(new CamTestComponent()
            //{
            //    SecondsToMove = 5f,
            //    Amplitude = new(10f, 10f, 300f),
            //    Base = new(0,0, 100f),
            //    Progress = 0f,
            //    Stage = CamTestComponent.CamTestStage.X
            //});
            e.StopEditing();


            /*
            // smaller camera
            e = new Entity(engine);
            e.AddComponent(new PixelBounds()
            {
                Bounds = new RotatedRect(new Rectangle(1200, 0, 400, 225), Angle.FromRadians(0f), new(0, 0))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.BackgroundDark),
                Tint = Color.White,
                Depth = 1
            });
            e.AddComponent(new CameraComponent()
            {
                Position = new Vector2(0f, 0f),
                Scale = 25f,
                Rotation = Angle.FromDegrees(0),
                Depth = 1
            });
            e.StopEditing();*/


            // make another example entity
            e = MakeSimpleEntity(new RectangleF(-.4f, -.4f, .8f, .8f), TextureAsset.Bot, 0);
            e.AddComponent(new PointRotation()
            {
                center = new Point(0, 0),
                radius = 2,
                rotationalVelcocity = -1f,
                otherRotationVelocity = -1f

            });
            e.StopEditing();
          

            int width = 5;
            int height = 5;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    e = MakeSimpleEntity(new RectangleF(x - (width / 2f), y - (height / 2f), 1, 1), TextureAsset.TestSquare, 0);
                    e.StopEditing();
                }
            }
            // make an example entity.




            //PointRotation rot = (PointRotation)e.FindComponent<PointRotation>();
            /*
            e.AddComponent(new InputComponent()
            {
                key = Keys.Space,
                action = () => 
                { 
                    rot.rotationalVelcocity = -rot.rotationalVelcocity;
                }
            });

            e.StopEditing();*/


         
        }

        // yeah, we're 100% going to need to sort out the entity creating.
        // the thing with making everything data is that data really doesn't belong in code.
        private Entity MakeSimpleEntity(RectangleF bounds, TextureAsset text, int depth)
        {
            Entity toReturn = new(engine);
            toReturn.AddComponent(new WorldBounds()
            {
                Bounds = new RotatedRect(bounds, Angle.FromRadians(0f), new(0, 0))
            });

            toReturn.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(text),
                Tint = Color.White,
                Depth = depth
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
