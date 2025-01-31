using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EngineCore.Components
{


    public class ChunkData : Component
    {
        public static readonly EntityType TileType = (Entity e) =>
        {
            // this will be fine, temporarily.
            return e.Components.Count == 1 && e.HasComponent<SimpleTexture>();
        };

        private List<List<Entity?>> tiles = [[]];

        public required List<List<Entity?>> Tiles
        {
            get {  return tiles; }
            set
            {
                int height = value[0].Count;

                foreach (List<Entity?> list in value)
                {
                    if(list.Count != height)
                    {
                        throw new InvalidOperationException("Chunk data must be rectangular.");
                    }

                    foreach (Entity? e in list)
                    {
                        if (e == null) continue;

                        if (!e.IsOfType(TileType))
                        {
                            throw new InvalidOperationException("Invalid entity for chunk.");
                        }
                        
                    }

                }
                tiles = value;
            }
        }
    }
}
