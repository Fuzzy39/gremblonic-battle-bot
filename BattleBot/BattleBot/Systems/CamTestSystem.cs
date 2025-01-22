using BattleBot.Components;
using EngineCore;
using EngineCore.Rendering;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Systems
{
    internal class CamTestSystem : EngineCore.System
    {
        public CamTestSystem(Engine e) : base(e)
        {
            AddRequiredComponent(typeof(CamTestComponent));
            AddRequiredComponent(typeof(CameraComponent));
            PrimaryComponent = typeof(CamTestComponent);
            Initialize(e);
        }

        protected override void Draw(GameTime gameTime)
        {
            // no drawing
        }

        protected override void Update(GameTime gameTime)
        {
            // some updating
            foreach (Entity entity in entities)
            {
                CameraComponent cam = (CameraComponent)entity.FindComponent<CameraComponent>().First();
                CamTestComponent testState = (CamTestComponent)entity.FindComponent<CamTestComponent>().First();

                // update progress
                if (testState.Progress == 1)
                {
                    testState.Progress = 0;
                    testState.Stage = testState.Stage switch
                    {
                        CamTestComponent.CamTestStage.X => CamTestComponent.CamTestStage.Y,
                        CamTestComponent.CamTestStage.Y => CamTestComponent.CamTestStage.Zoom,
                        CamTestComponent.CamTestStage.Zoom => CamTestComponent.CamTestStage.Rotation,
                        CamTestComponent.CamTestStage.Rotation => CamTestComponent.CamTestStage.X,
                        _ => CamTestComponent.CamTestStage.X
                    };
                }

                testState.Progress = MathHelper.Clamp(
                    (float)(testState.Progress + gameTime.ElapsedGameTime.TotalSeconds / testState.SecondsToMove),
                    0,
                    1
                 );

                // Now, what do?
               

                switch (testState.Stage)
                {
                    case CamTestComponent.CamTestStage.X:
                        cam.Position = new(getPosInDominantAxis(testState.Amplitude.X, testState.Base.X, testState.Progress), 0);
                        break;
                    case CamTestComponent.CamTestStage.Y:
                        cam.Position = new(0, getPosInDominantAxis(testState.Amplitude.Y, testState.Base.Y, testState.Progress));
                        break;
                    case CamTestComponent.CamTestStage.Zoom:
                        cam.Scale = getPosInDominantAxis(testState.Amplitude.Z, testState.Base.Z, testState.Progress);
                        break;
                    default:
                        cam.Rotation = Angle.FromRadians(MathHelper.TwoPi * testState.Progress);
                        break;
                }
            }
        }

        private float getPosInDominantAxis(float amplitude, float basePos, float progress)
        {
            float move = amplitude / 2f;
            if(progress<=.25f)
            {
                return MathHelper.SmoothStep(basePos, basePos+move, progress * 4);
            }
            if(progress<=.75f)
            {
                return MathHelper.SmoothStep(basePos+move, basePos-move, (progress - .25f) * 2f);
            }

            return MathHelper.SmoothStep(basePos-move, basePos, (progress - .75f) * 4f);
        }
    }
}
