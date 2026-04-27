using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Models;

namespace Devoted.DevotedCode;

public static class MyCustomEnums
{

    [CustomEnum, KeywordProperties(AutoKeywordPosition.Before)]
    public static CardKeyword Faith;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Devotion;
    [CustomEnum, KeywordProperties(AutoKeywordPosition.After)]
    public static CardKeyword Zeal;

    
    public static bool IsFaith(this CardModel card)
    {
        return card.Keywords.Contains(Faith);
    }
    
    public static bool IsDevotion(this CardModel card)
    {
        return card.Keywords.Contains(Devotion);
    }
    
    public static bool IsZeal(this CardModel card)
    {
        return card.Keywords.Contains(Zeal);
    }
    

}