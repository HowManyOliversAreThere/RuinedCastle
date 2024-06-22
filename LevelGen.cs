using Celeste;
using Microsoft.Xna.Framework;
using Monocle;

public class ProceduralLevelGenerator
{
    private Random _random;

    public ProceduralLevelGenerator(int seed)
    {
        _random = new Random(seed);
    }

    public void GenerateLevel(Level level)
    {
        // Get level width
        int levelwidth = level.Bounds.Width / 8;
        int levelheight = level.Bounds.Height / 8;
        Sprite stone = new Sprite(GFX.Game, "objects/stone/");

        for (int x = 10; x < levelwidth - 10; x++)
        {
            for (int y = 0; y < levelheight; y++)
            {
                if (_random.NextDouble() < 0.1)
                {
                    level.Add(new CrumblePlatform(new Vector2(x * 8, y * 8), 3));
                    // level.Add(new SolidTiles(new Vector2(x, y), new VirtualMap<char>(1, 1, (char)3 )));
                    // level.Add(new Solid(new Vector2(x * 8, y * 8), 8, 8, true));
                }
            }
        }
    }
}
