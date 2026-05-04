using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Keywords;
using Devoted.DevotedCode.Powers;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Devoted.DevotedCode.Cards.Common;


[Pool(typeof(DevotedCardPool))]
public class Mortification() : DevotedCard(2, CardType.Skill, CardRarity.Common, TargetType.Self)
{
    
    protected override HashSet<CardTag> CanonicalTags => [MyCustomEnums.PenanceTrigger];
    protected override IEnumerable<DynamicVar> CanonicalVars => [new DamageVar(4M, ValueProp.Unpowered | ValueProp.Move), new BlockVar(16M, ValueProp.Move)];
    
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        Mortification cardSource = this;
        VfxCmd.PlayOnCreatureCenter(cardSource.Owner.Creature, "vfx/vfx_bloody_impact");
        IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, cardSource.Owner.Creature, cardSource.DynamicVars.Damage, (CardModel) cardSource);
        await CreatureCmd.TriggerAnim(cardSource.Owner.Creature, "Cast", cardSource.Owner.Character.CastAnimDelay);
        SfxCmd.Play("event:/sfx/characters/ironclad/ironclad_bloodwall");
        VfxCmd.PlayOnCreature(cardSource.Owner.Creature, "vfx/vfx_blood_wall");
        Decimal num = await CreatureCmd.GainBlock(cardSource.Owner.Creature, cardSource.DynamicVars.Block, cardPlay);
    }

    protected override void OnUpgrade() => this.DynamicVars.Block.UpgradeValueBy(4M);
}