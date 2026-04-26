using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;
using Devoted.DevotedCode.Extensions;
using Godot;

namespace Devoted.DevotedCode.Relics;

[Pool(typeof(DevotedRelicPool))]
public abstract class DevotedRelic : CustomRelicModel
{
    public override string PackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".RelicImagePath();

    protected override string PackedIconOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}_outline.png".RelicImagePath();

    protected override string BigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigRelicImagePath();
}