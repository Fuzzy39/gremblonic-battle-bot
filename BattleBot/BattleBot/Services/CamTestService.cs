using BattleBot.Components;
using EngineCore;
using EngineCore.Components;
using EngineCore.Rendering;
using EngineCore.Util;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Services
{
    internal class CamTestService : ServiceBase
    {

        // The Entity Type that this service cares about. More complex services may have seperate distinct types.
        public static readonly EntityType EntityType = e => e.HasComponent<CamTestComponent>() && e.HasComponent<CameraComponent>();


        public CamTestService(Engine e) : base(e)
        {
            entities.Add(EntityType, []);
        }


        public override void Update(GameTime gameTime)
        {
            // some updating
            foreach (Entity entity in entities[EntityType])
            {
                CameraComponent cam = entity.FindComponent<CameraComponent>();
                CamTestComponent testState = entity.FindComponent<CamTestComponent>();

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
                        cam.Position = new(getPosInDominantAxis(testState.Amplitude.X, testState.Base.X, testState.Progress), testState.Base.Y);
                        break;
                    case CamTestComponent.CamTestStage.Y:
                        cam.Position = new(testState.Base.X, getPosInDominantAxis(testState.Amplitude.Y, testState.Base.Y, testState.Progress));
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
            if (progress <= .25f)
            {
                return MathHelper.SmoothStep(basePos, basePos + move, progress * 4);
            }
            if (progress <= .75f)
            {
                return MathHelper.SmoothStep(basePos + move, basePos - move, (progress - .25f) * 2f);
            }

            return MathHelper.SmoothStep(basePos - move, basePos, (progress - .75f) * 4f);
        }
    }
}
