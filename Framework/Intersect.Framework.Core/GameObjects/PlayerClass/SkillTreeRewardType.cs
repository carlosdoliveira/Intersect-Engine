namespace Intersect.Framework.Core.GameObjects.PlayerClass;

/// <summary>
/// Defines the type of reward granted by a skill tree node
/// </summary>
public enum SkillTreeRewardType
{
    /// <summary>
    /// No reward (used for transitional/connector nodes)
    /// </summary>
    None = 0,

    /// <summary>
    /// Grants a spell/ability to the player
    /// </summary>
    Spell,

    /// <summary>
    /// Provides stat bonuses (Attack, Defense, etc.)
    /// </summary>
    StatBonus,

    /// <summary>
    /// Provides vital bonuses (Health, Mana)
    /// </summary>
    VitalBonus,

    /// <summary>
    /// Grants a passive ability or effect
    /// </summary>
    PassiveAbility,
}
