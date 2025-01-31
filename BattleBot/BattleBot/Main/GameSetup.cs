using BattleBot.Components;
using EngineCore;
using EngineCore.Components;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Main
{

    // this thing is barely a class, a pile of garbage
    internal class GameSetup
    {
        AssetManager assetManager;
        Engine engine;

        Entity wallTile;
        Entity floorTile;

        public GameSetup(AssetManager assetManager, Engine engine) 
        {
            this.assetManager = assetManager;
            this.engine = engine;

            wallTile = new Entity(engine);
            wallTile.AddComponent(new SimpleTexture() 
            { 
                Texture=assetManager.getTexture(TextureAsset.Wall),
                Tint = Color.White 
            });
            wallTile.StopEditing();

            floorTile = new Entity(engine);
            floorTile.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.Floor),
                Tint = Color.White
            });
            floorTile.StopEditing();
        }


        public void CameraTest()
        {
            SetupCameras(true, true, 100, new(0));
            setupInputTest();

            // background tiles
            int width = 5;
            int height = 5;
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Entity e = MakeSimpleEntity(new RectangleF(x - (width / 2f), y - (height / 2f), 1, 1), TextureAsset.Floor, (byte)DrawPriority.Background, 1);
                    e.StopEditing();
                }
            }
        }


        public void ChunkTest()
        {
            SetupCameras(true, true, 75, new(3.5f));

            SetupTestChunk(new(0, 0), new(7, 7), false, 1);
            SetupTestChunk(new(1, 0), new(7, 7), true, 1);
            SetupTestChunk(new(-1, 0), new(7, 7), true, 1);
            SetupTestChunk(new(0, -1), new(7, 7), true, 1);
            SetupTestChunk(new(0, 1), new(7, 7), true, 1);
         
        }




        private void SetupCameras(bool minimap, bool doTesting, float defaultScale, Vector2 center)
        {
            // camera
            Entity e = new Entity(engine);
            e.AddComponent(new PixelBounds()
            {
                Bounds = new RotatedRect(new Rectangle(0, 0, 1600, 900), Angle.FromRadians(0f), new(0, 0))
            });
            e.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(TextureAsset.BackgroundDark),
                Tint = Color.White,
                Priority = (byte)DrawPriority.Background
            });
            e.AddComponent(new CameraComponent()
            {
                Position = center,
                Scale = defaultScale,
                Rotation = Angle.FromDegrees(0),
                Priority = (byte)DrawPriority.Background,
                MapID = 1
            });
            if (doTesting)
            {
                e.AddComponent(new CamTestComponent()
                {
                    SecondsToMove = 4.2f,
                    Amplitude = new(10f, 10f, defaultScale/2f),
                    Base = new(center.X, center.Y, defaultScale),
                    Progress = 0f,
                    Stage = CamTestComponent.CamTestStage.X
                });
            }
            e.StopEditing();


            if (!minimap) return;
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
                Priority = (byte)DrawPriority.Foreground
            });
            e.AddComponent(new CameraComponent()
            {
                Position = center,
                Scale = defaultScale/4f,
                Rotation = Angle.FromDegrees(0),
                Priority = (byte)DrawPriority.Foreground,
                MapID = 1
            });
            e.StopEditing();

        }


        private void setupInputTest()
        {
            // make an example entity that spins real good
            Entity e = MakeSimpleEntity(new RectangleF(-.4f, -.4f, .8f, .8f), TextureAsset.Bot, (byte)DrawPriority.Foreground, 1);
            e.AddComponent(new PointRotation()
            {
                center = new Point(0, 0),
                radius = 2,
                rotationalVelcocity = -1f,
                otherRotationVelocity = -1f

            });

            // input test
            PointRotation rot = (PointRotation)e.FindComponent<PointRotation>();
            e.AddComponent(new InputComponent()
            {
                key = Keys.Space,
                action = () =>
                {
                    rot.rotationalVelcocity = -rot.rotationalVelcocity;
                }
            });

            e.StopEditing();

        }



        // yeah, we're 100% going to need to sort out the entity creating.
        // the thing with making everything data is that data really doesn't belong in code.
        private Entity MakeSimpleEntity(RectangleF bounds, TextureAsset text, byte depth, int mapID)
        {
            Entity toReturn = new(engine);
            toReturn.AddComponent(new MapBounds()
            {
                Bounds = new RotatedRect(bounds, Angle.FromRadians(0f), new(0, 0)),
                MapID = mapID
            });

            toReturn.AddComponent(new SimpleTexture()
            {
                Texture = assetManager.getTexture(text),
                Tint = Color.White,
                Priority = depth
            });
            return toReturn;
        }



        private void SetupTestChunk(Point ChunkCoords, Point size, bool walls, int mapID)
        {
            Entity chunk = new(engine);
            chunk.AddComponent(new MapBounds() {
                Bounds = new RotatedRect(new Rectangle(ChunkCoords * size, size), Angle.FromDegrees(0), new(0) ),
                MapID = mapID
            });

            List<List<Entity >> list = new List<List<Entity>>();
            for (int x = 0; x < size.X; x++) 
            {
                list.Add([]);
                for (int y = 0; y < size.Y; y++)
                {
                    if(walls &&(x == 0 || y == 0 || x == size.X-1 || y== size.Y-1))
                    {
                        list[x].Add(wallTile);
                        continue;
                    }

                    if(size.X/2 == x && size.Y/2 == y)
                    {
                        list[x].Add(wallTile);
                        continue;
                    }

                    list[x].Add(floorTile);
                }
            }



            chunk.AddComponent(new ChunkData()
            {
                Tiles = list
            });
            chunk.StopEditing();
        }



    }
}
