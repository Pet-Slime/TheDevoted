using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Powers.FaithPowers;
using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Uncommon;


  
[Pool(typeof(DevotedCardPool))]
public class CadenceOfLight() : DevotedCard(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    
    
    protected override bool HasEnergyCostX => true;
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new CalculationBaseVar(1M), new ExtraDamageVar(1M), 
    
        new CalculatedDamageVar(ValueProp.Move).WithMultiplier((Func<CardModel, Creature, Decimal>) ((card, _) =>
        {
            ICombatState combatState = card.CombatState;
            return (Decimal) (combatState != null ? combatState.PlayerCreatures.Where<Creature>((Func<Creature, bool>) (c => c.IsAlive)).Sum<Creature>((Func<Creature, int>) (c => c.GetPowerAmount<FaithPower>())) : 0);
        }))];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<FaithPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        CadenceOfLight card = this;
        ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
        int num1 = card.ResolveEnergyXValue();
        if (num1 > 0)
        {
            Color color = new Color("FFFFFF80");
            double num2 = SaveManager.Instance.PrefsSave.FastMode == FastModeType.Fast ? 0.2 : 0.3;
            NCombatRoom instance1 = NCombatRoom.Instance;
            if (instance1 != null)
                instance1.CombatVfxContainer.AddChildSafely((Node) NHorizontalLinesVfx.Create(color, 0.8 + (double) Mathf.Min(8, num1) * num2));
            SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_whirlwind");
            NRun instance2 = NRun.Instance;
            if (instance2 != null)
                instance2.GlobalUi.AddChildSafely((Node) NSmokyVignetteVfx.Create(color, color));
        }
        AttackCommand attackCommand = await DamageCmd.Attack(card.DynamicVars.CalculatedDamage).WithHitCount(num1).FromCard((CardModel) card).Targeting(cardPlay.Target).WithHitFx("vfx/vfx_attack_lightning").Execute(choiceContext);
    }


    protected override void OnUpgrade()
    {
        DynamicVars.CalculationBase.UpgradeValueBy(1m);
    }
}