using BaseLib.Utils;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode.Keywords;


[HarmonyPatch(typeof(CardModel))]
public class Waxed
{
    public static readonly SpireField<CardModel, bool> IsWax = new(
        () => false);
    

    
    public static LocString WaxKeywordPrefix =>
        new("card_keywords", "DEVOTED-WAXED.wasCardPrefix");
    
    public static LocString MeltedWaxKeywordPrefix =>
        new("card_keywords", "DEVOTED-WAXED.wasCardPrefixMelting");
    
    [HarmonyPatch(nameof(CardModel.Title), MethodType.Getter)]
    [HarmonyPrefix]
    private static bool PrefixTitle(CardModel __instance, ref string __result)
    {
        if (!__instance.Keywords.Contains(MyCustomEnums.Waxed))
            return true;

        var prefix = WaxKeywordPrefix;
        if (IsWax.Get(__instance))
        {
            prefix = MeltedWaxKeywordPrefix;
        }
        
        string baseTitle = prefix.GetFormattedText() 
                           + __instance.TitleLocString.GetFormattedText();

        __result = baseTitle + GetUpgradeSuffix(__instance);
        return false;
    }

    private static string GetUpgradeSuffix(CardModel card)
    {
        if (!card.IsUpgraded)
            return "";

        if (card.MaxUpgradeLevel <= 1)
            return "+";

        return $"+{card.CurrentUpgradeLevel}";
    }
    
    [HarmonyPatch(nameof(CardModel.OnPlayWrapper))]
    [HarmonyPrefix]
    private static void OnPlayHOok(CardModel __instance, PlayerChoiceContext choiceContext,
        Creature? target,
        bool isAutoPlay,
        ResourceInfo resources,
        bool skipCardPileVisuals = false)
    {
        if (__instance.Keywords.Contains(MyCustomEnums.Waxed))
        {
            if (IsWax.Get(__instance))
            {
                if (!__instance.Keywords.Contains(CardKeyword.Exhaust))
                    CardCmd.ApplyKeyword(__instance, CardKeyword.Exhaust);
            }
            else
            {
                IsWax.Set(__instance, true);
            }
        }
    }
}