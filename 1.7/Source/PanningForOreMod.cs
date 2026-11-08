using HarmonyLib;
using Verse;

namespace PanningForOrePatch
{
    internal class PanningForOreMod: Mod
    {
        public PanningForOreMod(ModContentPack pack): base(pack)
        {
            new Harmony(nameof(PanningForOreMod)).PatchAll();
        }
    }
}
