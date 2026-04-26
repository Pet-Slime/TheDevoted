using BaseLib.Abstracts;
using Devoted.DevotedCode.Extensions;
using Godot;

namespace Devoted.DevotedCode.Character;

public class DevotedPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => Devoted.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}