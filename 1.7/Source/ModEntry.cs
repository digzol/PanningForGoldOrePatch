using HarmonyLib;
using Verse;

namespace PanningForGoldOrePatch
{
    internal class ModEntry: Mod
    {
        public ModEntry(ModContentPack pack): base(pack)
        {
            new Harmony(nameof(ModEntry)).PatchAll();
        }
    }
}
