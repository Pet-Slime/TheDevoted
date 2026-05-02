using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.VowPowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;

[Pool(typeof(DevotedCardPool))]
public class VotiveStrike() : DevotedCard(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    
    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];  
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<VowOfStrikesPower>(2M),
        new DamageVar(16, ValueProp.Move)
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.Static(StaticHoverTip.ReplayStatic)];
    
    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {    
        VotiveStrike card = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        AttackCommand attackCommand2 = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).FromCard((CardModel) card).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash").Execute(choiceContext);
        VowOfStrikesPower vigorPower = await PowerCmd.Apply<VowOfStrikesPower>(choiceContext, card.Owner.Creature, (Decimal) card.DynamicVars["VowOfStrikesPower"].IntValue, card.Owner.Creature, (CardModel) card);
    }
    
    protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}