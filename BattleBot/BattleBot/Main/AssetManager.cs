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
        TestSquare
    }

    internal class AssetManager
    {


        private Dictionary<TextureAsset, Texture2D> textures;

        public AssetManager(ContentManager content)
        {
            textures = new();
            textures.Add(TextureAsset.TestSquare, content.Load<Texture2D>("TestSquare"));
        }

        public Texture2D getTexture(TextureAsset text)
        {
            return textures[text]; 
        }
    }

}
