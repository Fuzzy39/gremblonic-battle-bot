using Microsoft.Xna.Framework;

namespace EngineCore
{
    /// <summary>
    /// Defines a system for the game. To be added to the engine and work as expected, the method <see cref="EngineCore.Engine.OnSystemCreated(Service)"/> must be called.
    /// </summary>
    public interface Service
    {
        /// <summary>
        /// Whether the system is running. When false, the engine will still call <see cref="OnEntityChanged(Entity)"/>
        /// and <see cref="OnEntityDestroyed(Entity)"/>, but will not make update or draw calls.
        /// </summary>
        public bool IsRunning { get; set; }


        /// <summary>
        /// Called when any entity in the game is changed, by adding or removing a component.
        /// </summary>
        /// <param name="e"></param>
        internal protected void OnEntityChanged(Entity e);

        /// <summary>
        /// Called when any entity is destroyed.
        /// </summary>
        /// <param name="e"></param>
        internal protected void OnEntityDestroyed(Entity e);



        internal protected void Update(GameTime gameTime);

        /// <summary>
        /// Predrawing is for drawing to render targets before rendering the final image. Do NOT draw any sprites or graphics to the window during a predraw call.
        /// </summary>
        /// <param name="gameTime"></param>
        internal protected void PreDraw(GameTime gameTime);

        internal protected void Draw(GameTime gameTime);



    }
        
}