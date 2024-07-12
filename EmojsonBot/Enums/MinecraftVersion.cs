using Discord.Interactions;

namespace EmojsonBot.Enums;

public enum MinecraftVersion
{
    [ChoiceDisplay("< 1.21")]
    Legacy,

    [ChoiceDisplay("1.21 +")]
    Modern
}