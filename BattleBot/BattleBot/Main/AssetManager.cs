using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleBot.Main
{



    public enum TextureAsset
    {
        TestSquare,
        Bot,
        BackgroundDark,
        Wall,
        Floor
    }

    internal class AssetManager
    {


        private Dictionary<TextureAsset, Texture2D> textures;

        public AssetManager(ContentManager content)
        {
            textures = new()
            {
                { TextureAsset.TestSquare, content.Load<Texture2D>("TestSquare") },
                { TextureAsset.Bot, content.Load<Texture2D>("Bot") },
                { TextureAsset.BackgroundDark, content.Load<Texture2D>("BackgroundDark") },
                { TextureAsset.Wall, content.Load<Texture2D>("Wall") },
                { TextureAsset.Floor, content.Load<Texture2D>("Floor") }
            };

        }

        public Texture2D getTexture(TextureAsset text)
        {
            return textures[text];
        }
    }

}
