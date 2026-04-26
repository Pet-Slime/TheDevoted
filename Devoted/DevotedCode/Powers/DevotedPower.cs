using BaseLib.Abstracts;
using BaseLib.Extensions;
using Devoted.DevotedCode.Extensions;
using Godot;

namespace Devoted.DevotedCode.Powers;

public abstract class DevotedPower : CustomPowerModel
{
    //Loads from Devoted/images/powers/your_power.png
    public override string CustomPackedIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PowerImagePath();
    public override string CustomBigIconPath => $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".BigPowerImagePath();
}