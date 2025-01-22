using EngineCore;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Components
{

    /// <summary>
    /// A rather narrow test component
    /// </summary>
    internal class CamTestComponent : Component
    {

        internal enum CamTestStage
        {
            X,
            Y,
            Zoom,
            Rotation
        }

        /// <summary>
        /// The distance, in tiles, that the camera will move in the X, then the Y directions, then Zoom, in pixels/tile.
        /// </summary>
        public Vector3 Amplitude { get; set; }

        /// <summary>
        /// default pos.
        /// </summary>
        public Vector3 Base { get; set; }


        /// <summary>
        /// The time in seconds in which one of these movements will be completed
        /// </summary>
        public float SecondsToMove {  get; set; }





        /// <summary>
        /// The current progress through one of the test steps
        /// </summary>
        public float Progress { get; set; }

        public CamTestStage Stage { get; set; }

    }
}
