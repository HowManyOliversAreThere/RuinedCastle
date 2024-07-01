using Celeste;
using Celeste.Mod;

public class RuinedCastle : EverestModule
{
    public static RuinedCastle Instance;

    public RuinedCastle()
    {
        Instance = this;
    }

    public override void Load()
    {
        On.Celeste.Level.LoadLevel += OnLoadLevel;
    }

    public override void Unload()
    {
        On.Celeste.Level.LoadLevel -= OnLoadLevel;
    }

    private void OnLoadLevel(On.Celeste.Level.orig_LoadLevel orig, Level self, Player.IntroTypes playerIntro, bool isFromLoader)
    {
        orig(self, playerIntro, isFromLoader);
        LevelGen(self);
    }

    private void LevelGen(Level level)
    {
        if (level.Session.Area.GetSID() == "RuinedCastle/RuinedCastle/RuinedCastle")
        {
            ProceduralLevelGenerator generator = new(DateTime.Now.GetHashCode());
            generator.GenerateLevel(level);
        }
    }
}