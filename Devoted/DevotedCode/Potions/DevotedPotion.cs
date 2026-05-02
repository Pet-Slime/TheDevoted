using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Extensions;

namespace Devoted.DevotedCode.Potions;

[Pool(typeof(DevotedPotionPool))]
public abstract class DevotedPotion : CustomPotionModel
{
    public override string CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PackedPotionImagePath();

    public override string CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".PackedPotionImagePath();
}