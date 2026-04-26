using BaseLib.Abstracts;
using BaseLib.Utils;
using Devoted.DevotedCode.Character;

namespace Devoted.DevotedCode.Potions;

[Pool(typeof(DevotedPotionPool))]
public abstract class DevotedPotion : CustomPotionModel;