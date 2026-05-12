using Intersect.Enums;
using Newtonsoft.Json;

namespace Intersect.Framework.Core.GameObjects.PlayerClass;

/// <summary>
/// Represents a single node in a skill tree
/// </summary>
public partial class SkillTreeNode
{
    /// <summary>
    /// Unique identifier for this node
    /// </summary>
    [JsonProperty]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Display name of the skill or ability
    /// </summary>
    [JsonProperty]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of what this node provides
    /// </summary>
    [JsonProperty]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Icon filename or path for visual representation
    /// </summary>
    [JsonProperty]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Minimum character level required to unlock this node
    /// </summary>
    [JsonProperty]
    public int RequiredLevel { get; set; }

    /// <summary>
    /// List of node IDs that must be unlocked before this node becomes available
    /// </summary>
    [JsonProperty]
    public List<Guid> PrerequisiteNodeIds { get; set; } = [];

    /// <summary>
    /// Specialization this node belongs to (null for base tree nodes)
    /// </summary>
    [JsonProperty]
    public Guid? SpecializationId { get; set; }

    /// <summary>
    /// X position for UI placement (for future visual editor)
    /// </summary>
    [JsonProperty]
    public int PositionX { get; set; }

    /// <summary>
    /// Y position for UI placement (for future visual editor)
    /// </summary>
    [JsonProperty]
    public int PositionY { get; set; }

    /// <summary>
    /// Type of reward this node provides
    /// </summary>
    [JsonProperty]
    public SkillTreeRewardType RewardType { get; set; }

    /// <summary>
    /// ID of the spell granted (if RewardType is Spell)
    /// </summary>
    [JsonProperty]
    public Guid? RewardSpellId { get; set; }

    /// <summary>
    /// Stat bonuses granted by this node (if RewardType is StatBonus)
    /// Key: Stat enum value, Value: bonus amount
    /// </summary>
    [JsonProperty]
    public Dictionary<Stat, int> StatBonuses { get; set; } = [];

    /// <summary>
    /// Vital bonuses granted by this node (if RewardType is VitalBonus)
    /// Key: Vital enum value, Value: bonus amount
    /// </summary>
    [JsonProperty]
    public Dictionary<Vital, long> VitalBonuses { get; set; } = [];

    /// <summary>
    /// Number of skill points required to unlock this node
    /// </summary>
    [JsonProperty]
    public int SkillPointCost { get; set; } = 1;

    /// <summary>
    /// Custom data for passive abilities or future expansion
    /// </summary>
    [JsonProperty]
    public Dictionary<string, string> CustomData { get; set; } = [];
}
