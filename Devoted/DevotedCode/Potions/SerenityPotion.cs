using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
using Devoted.DevotedCode.Powers.SerenityPowers;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Potions;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Potions;

  
public class SerenityPotion : DevotedPotion
{
    public override PotionRarity Rarity => PotionRarity.Rare;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.AnyAlly;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<SerenityBlockPower>(2M)];
    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<SerenityBlockPower>(),];

    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        SerenityPotion faithPotion = this;
        PotionModel.AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("fd2155"));
        SerenityBlockPower faithPower = await PowerCmd.Apply<SerenityBlockPower>(choiceContext, target, faithPotion.DynamicVars["SerenityBlockPower"].BaseValue, faithPotion.Owner.Creature, (CardModel) null);
    }
}