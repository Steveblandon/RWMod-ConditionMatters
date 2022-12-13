namespace ConditionMatters
{
    using System;
    using System.Reflection;
    using HarmonyLib;
    using RimWorld;
    using Verse;
    
    [StaticConstructorOnStartup]
    public static class ConditionMattersHarmonyPatch
    {
        static ConditionMattersHarmonyPatch()
        {
            Harmony harmony = new Harmony("SteveZero.ConditionMattersHarmonyPatch");
            MethodInfo original = AccessTools.Method(typeof(VerbProperties), "AdjustedArmorPenetration", new Type[]
            {
                typeof(Tool),
                typeof(Pawn),
                typeof(Thing),
                typeof(HediffComp_VerbGiver)
            }, null);
            harmony.PatchAll();
            harmony.Patch(original, null, new HarmonyMethod(typeof(ConditionMattersHarmonyPatch), "PatchAdjustedArmorPenetration", null), null, null);
        }

        public static void PatchAdjustedArmorPenetration(Tool tool, Pawn attacker, Thing equipment, HediffComp_VerbGiver hediffCompSource, ref float __result)
        {
            StatDef namedSilentFail = DefDatabase<StatDef>.GetNamedSilentFail("CM_MeleeArmorPen_PartsHolder");
            bool flag = namedSilentFail != null && equipment != null;
            if (flag)
            {
                StatRequest statRequest = (attacker == null) ? StatRequest.For(equipment) : StatRequest.For(equipment, attacker);
                foreach (StatPart statPart in namedSilentFail.parts)
                {
                    statPart.TransformValue(statRequest, ref __result);
                }
            }
        }

        private const string MOD_NAME = "Condition Matters";

        private const string STATDEF_ARMORPEN_PARTS_HOLDER = "CM_MeleeArmorPen_PartsHolder";
    }
}
