using MegaCrit.Sts2.Core.Entities.Powers;

namespace Devoted.DevotedCode.Powers;


public class PenanceHealPower : DevotedPower
{
    public override PowerType Type => PowerType.Buff;
    public override PowerStackType StackType => PowerStackType.Counter;

    public override bool AllowNegative => false;
}