using AlphaSkills;
using HarmonyLib;
using RimWorld;
using System.Reflection;
using Verse;

namespace PanningForGoldOrePatch
{
    [StaticConstructorOnStartup]
    public static class ModEntry
    {
        public static readonly ThingDef? s_goldOreDef;

        static ModEntry()
        {
            s_goldOreDef = DefDatabase<ThingDef>.GetNamed("DankPyon_GoldOre");

            if (s_goldOreDef != null)
            {
                var harmony = new Harmony(nameof(ModEntry));
                PanningForGoldOre_AlphaSkills_ExtraYields_Patch_Patch.Apply(harmony);
            }
            else
            {
                Log.Message($"[{(nameof(ModEntry))}] Expected DankPyon_GoldOre to be loaded but it was not found. The patch won't be applied.");
            }
        }

        public class PanningForGoldOre_AlphaSkills_ExtraYields_Patch_Patch
        {
            private static readonly FieldInfo s_vanillaGold = AccessTools.Field(typeof(ThingDefOf), nameof(ThingDefOf.Gold));
            private static readonly FieldInfo s_goldOre = AccessTools.Field(typeof(ModEntry), nameof(s_goldOreDef));

            public static void Apply(Harmony harmony)
            {
                Log.Message($"[{(nameof(ModEntry))}] Patching Alpha Skill's ExtraYields patch...");
                harmony.Patch(
                    original: AccessTools.Method(typeof(AlphaSkills_Mineable_TrySpawnYield_Patch),
                                                 nameof(AlphaSkills_Mineable_TrySpawnYield_Patch.ExtraYields),
                                                 new Type[] { typeof(Map), typeof(bool), typeof(Pawn), typeof(Mineable) }),
                    transpiler: new HarmonyMethod(typeof(PanningForGoldOre_AlphaSkills_ExtraYields_Patch_Patch), nameof(ExtraYields_Transpiler))
                );
            }

            public static IEnumerable<CodeInstruction> ExtraYields_Transpiler(IEnumerable<CodeInstruction> instructions)
            {
                int replaced = 0;
                foreach (var ins in instructions)
                {
                    if (ins.LoadsField(s_vanillaGold))
                    {
                        ins.operand = s_goldOre;
                        replaced++;
                    }
                    yield return ins;
                }

                if (replaced != 1)
                    Log.Warning($"[{(nameof(ModEntry))}] Expected to replace 1 ThingDefOf.Gold, replaced {replaced} instead.");
                else
                    Log.Message($"[{(nameof(ModEntry))}] Successfully replaced 1 instance of ThingDefOf.Gold.");
            }
        }
    }
}
