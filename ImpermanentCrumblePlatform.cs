using System.Collections;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Monocle;

namespace Celeste;

// Copy of CrumblePlatform that disappears once crumbled
public class ImpermanentCrumblePlatform : Solid
{
    public static ParticleType P_Crumble;

    private List<Image> images;

    private List<Image> outline;

    private List<Coroutine> falls;

    private List<int> fallOrder;

    private ShakerList shaker;

    private LightOcclude occluder;

    private Coroutine outlineFader;

    public string OverrideTexture;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public ImpermanentCrumblePlatform(Vector2 position, float width)
        : base(position, width, 8f, safe: false)
    {
        EnableAssistModeChecks = false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public ImpermanentCrumblePlatform(EntityData data, Vector2 offset)
        : this(data.Position + offset, data.Width)
    {
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public override void Added(Scene scene)
    {
        AreaData areaData = AreaData.Get(scene);
        string crumbleBlock = areaData.CrumbleBlock;
        if (OverrideTexture != null)
        {
            areaData.CrumbleBlock = OverrideTexture;
        }

        orig_Added(scene);
        areaData.CrumbleBlock = crumbleBlock;
    }

    private IEnumerator Sequence()
    {
        while (true)
        {
            bool onTop;
            if (GetPlayerOnTop() != null)
            {
                onTop = true;
                Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
            }
            else
            {
                if (GetPlayerClimbing() == null)
                {
                    yield return null;
                    continue;
                }

                onTop = false;
                Input.Rumble(RumbleStrength.Medium, RumbleLength.Medium);
            }

            Audio.Play("event:/game/general/platform_disintegrate", Center);
            shaker.ShakeFor(onTop ? 0.6f : 1f, removeOnFinish: false);
            foreach (Image image in images)
            {
                Console.WriteLine("Crumble: " + P_Crumble.ToString());
                Console.WriteLine("Position: " + Position);
                Console.WriteLine("Image Position: " + image.Position);
                SceneAs<Level>().Particles.Emit(P_Crumble, 2, Position + image.Position + new Vector2(0f, 2f), Vector2.One * 3f);
            }

            for (int i = 0; i < (onTop ? 1 : 3); i++)
            {
                yield return 0.2f;
                foreach (Image image2 in images)
                {
                    SceneAs<Level>().Particles.Emit(P_Crumble, 2, Position + image2.Position + new Vector2(0f, 2f), Vector2.One * 3f);
                }
            }

            float timer = 0.4f;
            if (onTop)
            {
                while (timer > 0f && GetPlayerOnTop() != null)
                {
                    yield return null;
                    timer -= Engine.DeltaTime;
                }
            }
            else
            {
                while (timer > 0f)
                {
                    yield return null;
                    timer -= Engine.DeltaTime;
                }
            }

            outlineFader.Replace(OutlineFade(1f));
            occluder.Visible = false;
            Collidable = false;
            float num = 0.05f;
            for (int j = 0; j < 4; j++)
            {
                for (int k = 0; k < images.Count; k++)
                {
                    if (k % 4 - j == 0)
                    {
                        falls[k].Replace(TileOut(images[fallOrder[k]], num * (float)j));
                    }
                }
            }

            RemoveSelf();
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerator OutlineFade(float to)
    {
        float from = 1f - to;
        for (float t = 0f; t < 1f; t += Engine.DeltaTime * 2f)
        {
            Color color = Color.White * (from + (to - from) * Ease.CubeInOut(t));
            foreach (Image item in outline)
            {
                item.Color = color;
            }

            yield return null;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private IEnumerator TileOut(Image img, float delay)
    {
        img.Color = Color.Gray;
        yield return delay;
        float distance = (img.X * 7f % 3f + 1f) * 12f;
        Vector2 from = img.Position;
        for (float time = 0f; time < 1f; time += Engine.DeltaTime / 0.4f)
        {
            yield return null;
            img.Position = from + Vector2.UnitY * Ease.CubeIn(time) * distance;
            img.Color = Color.Gray * (1f - time);
            img.Scale = Vector2.One * (1f - time * 0.5f);
        }

        img.Visible = false;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public void orig_Added(Scene scene)
    {
        base.Added(scene);
        MTexture mTexture = GFX.Game["objects/crumbleBlock/outline"];
        outline = new List<Image>();
        if (Width <= 8f)
        {
            Image image = new(mTexture.GetSubtexture(24, 0, 8, 8))
            {
                Color = Color.White * 0f
            };
            Add(image);
            outline.Add(image);
        }
        else
        {
            for (int i = 0; i < Width; i += 8)
            {
                int num = (i != 0) ? ((i > 0 && i < Width - 8f) ? 1 : 2) : 0;
                Image image2 = new(mTexture.GetSubtexture(num * 8, 0, 8, 8))
                {
                    Position = new Vector2(i, 0f),
                    Color = Color.White * 0f
                };
                Add(image2);
                outline.Add(image2);
            }
        }

        Add(outlineFader = new Coroutine());
        outlineFader.RemoveOnComplete = false;
        images = new List<Image>();
        falls = new List<Coroutine>();
        fallOrder = new List<int>();
        MTexture mTexture2 = GFX.Game["objects/crumbleBlock/" + AreaData.Get(scene).CrumbleBlock];
        for (int j = 0; j < Width; j += 8)
        {
            int num2 = (int)((Math.Abs(X) + j) / 8f) % 4;
            Image image3 = new Image(mTexture2.GetSubtexture(num2 * 8, 0, 8, 8))
            {
                Position = new Vector2(4 + j, 4f)
            };
            image3.CenterOrigin();
            Add(image3);
            images.Add(image3);
            Coroutine coroutine = new()
            {
                RemoveOnComplete = false
            };
            falls.Add(coroutine);
            Add(coroutine);
            fallOrder.Add(j / 8);
        }

        fallOrder.Shuffle();
        Add(new Coroutine(Sequence()));
        Add(shaker = new ShakerList(images.Count, on: false, [MethodImpl(MethodImplOptions.NoInlining)] (Vector2[] v) =>
        {
            for (int k = 0; k < images.Count; k++)
            {
                images[k].Position = new Vector2(4 + k * 8, 4f) + v[k];
            }
        }));
        Add(occluder = new LightOcclude(0.2f));
    }
}