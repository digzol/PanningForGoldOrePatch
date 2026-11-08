using AlphaSkills;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace PanningForOrePatch.HarmonyPatches
{
    [HarmonyPatch(typeof(AlphaSkills_Mineable_TrySpawnYield_Patch))]
    [HarmonyPatch("ExtraYields")]
    [HarmonyPatch(new Type[] { typeof(Map), typeof(bool), typeof(Pawn), typeof(Mineable) })]
    public class PanningForOre_AlphaSkills_ExtraYields_Patch_Patch
    {
        private static readonly FieldInfo s_VanillaGold = AccessTools.Field(typeof(ThingDefOf), nameof(ThingDefOf.Gold));
        private static readonly MethodInfo s_GetGoldOre = AccessTools.Method(typeof(PanningForOre_AlphaSkills_ExtraYields_Patch_Patch), nameof(GetGoldOre));
        private static ThingDef? s_GoldOreDef;

        public static ThingDef GetGoldOre()
        {
            return s_GoldOreDef ??= DefDatabase<ThingDef>.GetNamed("DankPyon_GoldOre");
        }

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int replaced = 0;
            foreach(var ins in instructions)
            {
                if (ins.LoadsField(s_VanillaGold))
                {
                    var call = new CodeInstruction(System.Reflection.Emit.OpCodes.Call, s_GetGoldOre);
                    call.labels.AddRange(ins.labels);
                    call.blocks.AddRange(ins.blocks);
                    replaced++;
                    yield return call;
                    continue;
                }
                yield return ins;
            }

            if (replaced != 1)
                Log.Warning($"[Panning For Ore Patch] Expected to replace 1 ThingDefOf.Gold, replaced {replaced} instead.");
            else
                Log.Message($"[Panning For Ore Patch] Successfully replaced 1 instance of ThingDefOf.Gold.");
        }
    }
}
