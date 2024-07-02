using Celeste;
using Celeste.Mod;
using Microsoft.Xna.Framework;
using Monocle;

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
        ImpermanentCrumblePlatform.P_Crumble = new ParticleType
			{
				Color = Calc.HexToColor("847E87"),
				FadeMode = ParticleType.FadeModes.Late,
				Size = 1f,
				Direction = 1.5707964f,
				SpeedMin = 5f,
				SpeedMax = 25f,
				LifeMin = 0.8f,
				LifeMax = 1f,
				Acceleration = Vector2.UnitY * 20f
			};
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