using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using Devoted.DevotedCode.Cards.Basic;
using Devoted.DevotedCode.Cards.Common;
using Devoted.DevotedCode.Cards.Uncommon;
using Devoted.DevotedCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Relics;

namespace Devoted.DevotedCode.Character;



public class Devoted : PlaceholderCharacterModel
{
    public const string CharacterId = "Devoted";

    public static readonly Color Color = new("F4F1E8");

    public override Color NameColor => Color;
    public override CharacterGender Gender => CharacterGender.Feminine;
    public override int StartingHp => 80;

    public override IEnumerable<CardModel> StartingDeck =>
    [
        ModelDb.Card<DivineStrike>(),
        ModelDb.Card<SerenityCalmMind>(),
        ModelDb.Card<BreathingExercise>(),
        ModelDb.Card<StrikeDevoted>(),
        ModelDb.Card<StrikeDevoted>(),
        ModelDb.Card<DefendDevoted>(),
        ModelDb.Card<DefendDevoted>(),
        ModelDb.Card<DefendDevoted>(),
        ModelDb.Card<TestOfFaith>(),
        ModelDb.Card<WoundCare>()
    ];

    public override IReadOnlyList<RelicModel> StartingRelics =>
    [
        ModelDb.Relic<BurningBlood>()
    ];

    public override CardPoolModel CardPool => ModelDb.CardPool<DevotedCardPool>();
    public override RelicPoolModel RelicPool => ModelDb.RelicPool<DevotedRelicPool>();
    public override PotionPoolModel PotionPool => ModelDb.PotionPool<DevotedPotionPool>();

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_devote.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_devote.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_devote_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_devote.png".CharacterUiPath();
}