using Devoted.DevotedCode.Powers.FaithPowers;
using Devoted.DevotedCode.Powers.PenancePowers;
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

  
public class PenancePotion : DevotedPotion
{
    public override PotionRarity Rarity => PotionRarity.Uncommon;

    public override PotionUsage Usage => PotionUsage.CombatOnly;

    public override TargetType TargetType => TargetType.Self;
    
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceEnergyPower>(2M)];

    public override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceEnergyPower>(),];
    
    protected override async Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
    {
        PenancePotion faithPotion = this;
        PotionModel.AssertValidForTargetedPotion(target);
        NCombatRoom.Instance?.PlaySplashVfx(target, new Color("fd2155"));
        PenanceEnergyPower faithPower = await PowerCmd.Apply<PenanceEnergyPower>(choiceContext, target, faithPotion.DynamicVars["PenanceEnergyPower"].BaseValue, faithPotion.Owner.Creature, (CardModel) null);
    }
}