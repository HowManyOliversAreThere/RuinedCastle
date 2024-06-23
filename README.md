# Ruined Castle

This is a mod for [Celeste](https://www.celestegame.com/) which adds _Ruined Castle_, a (currently) single-screen Proof-of-Concept map featuring randomly generated obstacles.

Check the mod out on [Gamebanana](https://gamebanana.com/mods/523196) for easy install to try it out.

## Running the mod

1. Install [Everest](https://everestapi.github.io/)
2. Install dotnet 7.0
3. Clone repo into the `Mods` folder of your Celeste install
4. Run `dotnet build` in the root of the repo
5. Adjust `everest.yaml` to point to the build RuinedCastle.dll instead of the root dir
    - On Windows with dotnet 7.0 this should be `bin\Debug\net7.0\RuinedCastle.dll`

## Publishing the mod

1. Ensure `everest.yaml`:
    1. Just has the DLL set as `RuinedCastle.dll` (not pointing to local built version)
    2. Has the version bumped if required
2. Run `package.bat`
3. Upload to Gamebanana
