using EngineCore.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{

    /// <summary>
    /// A camera component contains the information that describes a camera's position in a game-world.
    /// </summary>
    public class CameraComponent : Component
    {
        /// <summary>
        /// The center of the view of this camera, in game-world coordinates.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// the size, in (virtual)pixels, of one 'tile' in the game-world for this camera.
        /// </summary>
        required public float Scale { get; set; }


        /// <summary>   
        /// The angle, where positive values are clockwise, that the camera will represent the game world.
        /// Increase the value, the world as viewed by the camera rotates clockwise about the camera's center.
        /// </summary>
        required public Angle Rotation { get; set; }


        // this should maybe be broken off into its own component or something?
        // targetTexture?

        /// <summary>
        /// Holds the graphics data for what the camera sees.
        /// </summary>
        public RenderTarget2D? RenderTarget { get; set; }

        /// <summary>
        /// What order this camera will be rendered. Higher values are rendered first.
        /// </summary>
        public byte Priority { get; set; }
    }
}
