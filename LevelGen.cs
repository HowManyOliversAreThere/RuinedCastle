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
        switch (level.Session.LevelData.Name)
            {
                case "a-00":
                    GenerateLevelA00(level);
                    break;
                case "a-01":
                    GenerateLevelA01(level);
                    break;
            }
    }

    private void GenerateLevelA00(Level level)
    {
        // Get level width
        int levelwidth = level.Bounds.Width / 8;
        int levelheight = level.Bounds.Height / 8;
        int startX = 10;
        int endX = levelwidth - 10;

        for (int x = startX; x < endX; x++)
        {
            float progress = (float)(x - startX) / (endX - startX);
            for (int y = 4; y < levelheight; y++)
            {
                // Make platform always exist at start and taper off towards the end
                if (_random.NextDouble() < 0.05 + (1 - progress) * 0.5)
                {
                    level.Add(new CrumblePlatform(new Vector2(x * 8, y * 8), 8));
                }
            }
        }
    }

    private void GenerateLevelA01(Level level)
    {
        // Generate Crumble platforms starting at frequency of end of A00 and tapering to nothing
        // and generate dash crystals, starting with none and increasing with progress
        int levelheight = level.Bounds.Height / 8;
        int startX = level.Bounds.Left / 8 + 10;
        int endX = level.Bounds.Right / 8 - 16;

        for (int x = startX; x < endX; x++)
        {
            float progress = (float)(x - startX) / (endX - startX);
            for (int y = 4; y < levelheight; y++)
            {
                // Make platform always exist at start and taper off towards the end
                if (_random.NextDouble() < 0.05 * (1 - progress))
                {
                    level.Add(new CrumblePlatform(new Vector2(x * 8, y * 8), 8));
                }

                // Make dash crystal appear more frequently with progress
                if (_random.NextDouble() < 0.001 + progress * 0.005)
                {
                    level.Add(new Refill(new Vector2(x * 8, y * 8), false, false));
                }
            }
        }
    }
}
