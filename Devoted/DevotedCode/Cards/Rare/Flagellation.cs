using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers;
using Devoted.DevotedCode.Powers.PenancePowers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Combat.History.Entries;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Rare;

[Pool(typeof(DevotedCardPool))]
internal class Flagellation() : DevotedCard(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
{
    
    private const string _calculatedHitsKey = "CalculatedHits";
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new PowerVar<PenanceRagePower>(1M), 
        new DamageVar(2M, ValueProp.Move),
        new CalculationBaseVar(1M), 
        new CalculationExtraVar(1M), 
        new CalculatedVar("CalculatedHits").WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) => (Decimal) CombatManager.Instance.History.CardPlaysFinished.Count<CardPlayFinishedEntry>((Func<CardPlayFinishedEntry, bool>) (e => e.HappenedThisTurn(card.CombatState) && e.CardPlay.Card.Type == CardType.Attack && e.CardPlay.Card.Owner == card.Owner))))
    
    
    ];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<PenanceRagePower>()];
    
    protected override async Task OnPlay(MegaCrit.Sts2.Core.GameActions.Multiplayer.PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {    
        Flagellation card = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.Damage.BaseValue).WithHitCount((int) ((CalculatedVar) card.DynamicVars["CalculatedHits"]).Calculate(cardPlay.Target)).FromCard((CardModel) card).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_blunt", tmpSfx: "blunt_attack.mp3").Execute(choiceContext);
        PenanceRagePower vigorPower = await PowerCmd.Apply<PenanceRagePower>(choiceContext, card.Owner.Creature, (Decimal) card.DynamicVars["PenanceRagePower"].IntValue * card.DynamicVars["CalculatedHits"].IntValue, card.Owner.Creature, (CardModel) card);
    }
    
    protected override void OnUpgrade() => this.DynamicVars["PenanceRagePower"].UpgradeValueBy(1M);
}