namespace uiia_adventure.Systems;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using uiia_adventure.Core;
using uiia_adventure.Components;
using uiia_adventure.Globals;

public class TileRenderSystem : SystemBase
{
    private SpriteBatch _spriteBatch;

    public TileRenderSystem(SpriteBatch spriteBatch)
    {
        _spriteBatch = spriteBatch;
    }

    public override void Update(GameTime gameTime, List<GameObject> gameObjects)
    {
        _spriteBatch.Begin();

        foreach (GameObject obj in gameObjects)
        {
            if (!obj.HasComponent<TileMapComponent>())
                continue;
            var tileMap = obj.GetComponent<TileMapComponent>();

            foreach (var tile in tileMap.Grid)
            {
                int id = tile.Value;

                int x = tile.Key.X;
                int y = tile.Key.Y;

                int tileX = (id % tileMap.TilesPerRowSet) * tileMap.TileWidth;
                int tileY = (id / tileMap.TilesPerRowSet) * tileMap.TileHeight;

                var sourceRect = new Rectangle(tileX, tileY, tileMap.TileWidth, tileMap.TileHeight);
                var position = new Vector2(x * tileMap.TileWidth, y * tileMap.TileHeight);

                _spriteBatch.Draw(tileMap.Tileset, position, sourceRect, Color.White);
            }
        }

        _spriteBatch.End();
    }


}