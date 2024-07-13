using Discord.Interactions;

namespace EmojsonBot.Enums;

public enum MinecraftVersion
{
    [ChoiceDisplay("1_20-or-older")]
    Legacy,

    [ChoiceDisplay("1_21-and-later")]
    Modern
}