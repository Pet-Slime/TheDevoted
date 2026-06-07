using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers.ChimePowers;
using Devoted.DevotedCode.Powers.FaithPowers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class BellStrike() : DevotedCard(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
{

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Strike];
    
    public override IEnumerable<CardKeyword> CanonicalKeywords => [ MyCustomEnums.Toll ];

    protected override IEnumerable<DynamicVar> CanonicalVars =>
        [new PowerVar<ChimeDamagePower>(1M), new DamageVar(5, ValueProp.Move)];


    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<ChimeDamagePower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        BellStrike cardSource = this;
        ArgumentNullException.ThrowIfNull((object)cardPlay.Target, "cardPlay.Target");
        AttackCommand attackCommand = await DamageCmd.Attack(cardSource.DynamicVars.Damage.BaseValue)
            .FromCard((CardModel)cardSource).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_slash")
            .Execute(choiceContext);
        ChimeDamagePower vigorPower = await PowerCmd.Apply<ChimeDamagePower>(choiceContext,
            cardSource.Owner.Creature, (Decimal)cardSource.DynamicVars["ChimeDamagePower"].IntValue,
            cardSource.Owner.Creature, (CardModel)cardSource);
    }


    protected override void OnUpgrade()
    {
        DynamicVars["ChimeDamagePower"].UpgradeValueBy(1m);
        DynamicVars.Damage.UpgradeValueBy(2m);
    }
}