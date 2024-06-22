using Celeste;
using Celeste.Mod;
using Microsoft.Xna.Framework;

public class RuinedCastle : EverestModule
{
    public static RuinedCastle Instance;

    public RuinedCastle()
    {
        Instance = this;
    }

    public override void Load()
    {
        Console.WriteLine("RuinedCastle loaded");
        On.Celeste.Level.Begin += OnBeginLevel;
        On.Celeste.Level.Reload += OnReloadLevel;
    }

    public override void Unload()
    {
        On.Celeste.Level.Begin -= OnBeginLevel;
        On.Celeste.Level.Reload -= OnReloadLevel;
    }

    private void OnBeginLevel(On.Celeste.Level.orig_Begin orig, Level self)
    {
        LevelGen(self);
        orig(self);
    }

    private void OnReloadLevel(On.Celeste.Level.orig_Reload orig, Level self)
    {
        LevelGen(self);
        orig(self);
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