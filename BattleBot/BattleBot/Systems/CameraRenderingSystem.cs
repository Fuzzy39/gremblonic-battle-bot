using BattleBot.Components;
using EngineCore;
using EngineCore.Rendering;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Systems
{
    internal class CameraRenderingSystem : EngineCore.System
	{

		private class CameraRenderable : EngineCore.System
		{

			public List<Entity> list { get => entities; }
			public CameraRenderable(Engine e) : base(e)
			{
				AddRequiredComponent(typeof(WorldBounds));
				AddRequiredComponent(typeof(SimpleTexture));
				Initialize(e);
			}

			protected override void Draw(GameTime gameTime){ }
            protected override void Update(GameTime gameTime) { }

		}

		/// <summary>
		/// Abstracts a Camera Entity.
		/// </summary>
		private class Camera
		{ 
		
			private Entity entity;

			
			public Camera(Entity e)
			{
				entity = e;
				if(e.FindComponent<CameraComponent>().Count() != 1 ||
					e.FindComponent<PixelBounds>().Count() != 1)
				{
					throw new ArgumentException("This Entity "+e.ToString()+" Cannot be used as a camera.");
				}
			}     

			public RotatedRect WorldBounds
			{
				get
				{
					RotatedRect toReturn = new RotatedRect (
						new Vector2(0, 0),
						PixelBounds.Size / Component.Scale, 
						-Component.Rotation,
						new(.5f, .5f) 
					);

					toReturn.Center = Component.Position;

					return toReturn;
				}
			}

			public CameraComponent Component
			{
				get
				{
					return (CameraComponent)entity.FindComponent<CameraComponent>().First();
				}
			}

			public RotatedRect PixelBounds 
			{
				get 
				{
                    return ((PixelBounds)entity.FindComponent<PixelBounds>().First()).Bounds;
                } 
			
			}

			public RotatedRect ToPixelBounds(RotatedRect worldBounds)
			{
				Vector2 size = worldBounds.Size * Component.Scale;
				Angle rotation = Component.Rotation + worldBounds.Rotation;


				RotatedRect RenderBounds = new RotatedRect(new RectangleF(0,0, PixelBounds.Width, PixelBounds.Height), Angle.FromRadians(0), new() );

				// a glorious transformation. Hopefully it works.
				Vector2 internalRep = this.WorldBounds.ToInternalRepresentation(worldBounds.TopLeft);
				
                Vector2 pos = RenderBounds.FromInternalRepresentation(internalRep);

				// oh god hopefully this doesn't explode.
				return new(pos, size, rotation, new());

			
			}


			public void Render(Renderer renderer, Entity renderable)
			{
                // we must convert world bou
                RotatedRect renderableWorldBounds = ((WorldBounds)renderable.FindComponent<WorldBounds>().First()).Bounds;
				RotatedRect spriteBounds = ToPixelBounds(renderableWorldBounds);
				
				SimpleTexture st = (SimpleTexture)renderable.FindComponent<SimpleTexture>().First();
				renderer.Draw(st.Texture, spriteBounds, st.Tint);
            }

		}

        private BatchRenderer renderer;
        private CameraRenderable renderables;
		

		public CameraRenderingSystem(Engine e) : base(e)
		{
			renderables = new CameraRenderable(e);
			renderer = e.Renderer;

			AddRequiredComponent(typeof(PixelBounds));
			AddRequiredComponent(typeof(CameraComponent));

			PrimaryComponent = typeof(CameraComponent);
			Initialize(e);
		}

        protected override void Update(GameTime gameTime)
        {
            // no update
        }


        protected override void PreDraw(GameTime gameTime)
        {
			// save textures to draw later.
            foreach (Entity camera in entities)
            {
                RenderCamera(new Camera(camera));
            }
        }

        protected override void Draw(GameTime gameTime)
		{

			//Camera test = new(entities.First());
			//test.Component.Rotation.Radians += .01f; 
            foreach (Entity camera in entities)
            {
				Camera cam = new Camera(camera);
                // actually draw results to the screen
                renderer.Draw(cam.Component.RenderTarget, cam.PixelBounds, Color.White);
            }
        }


		private void RenderCamera(Camera camera)
		{

		

			// create a target to render to.
			RenderTarget2D target = camera.Component.RenderTarget;
			if(target == null || target.Bounds.Size != camera.PixelBounds.Size.ToPoint())
			{
                target = renderer.CreateTarget(camera.PixelBounds.Size.ToPoint());
				camera.Component.RenderTarget = target;
            }
			
            renderer.StartTarget(target);

			// render to it
            foreach (Entity toRender in renderables.list)
			{
				RotatedRect bounds = ((WorldBounds)toRender.FindComponent<WorldBounds>().First()).Bounds;
				//if (camera.WorldBounds.Intersects(bounds))
				{
					camera.Render(renderer, toRender);
				}
			}

			renderer.EndTarget();

			
		}

		
	}
}
