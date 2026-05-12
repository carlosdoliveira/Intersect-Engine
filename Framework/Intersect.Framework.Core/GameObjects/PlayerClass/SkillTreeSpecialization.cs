using Newtonsoft.Json;

namespace Intersect.Framework.Core.GameObjects.PlayerClass;

/// <summary>
/// Represents a class specialization in the skill tree system
/// (e.g., Warrior -> Paladin or Berserker, Archer -> Hunter)
/// </summary>
public partial class SkillTreeSpecialization
{
    /// <summary>
    /// Unique identifier for this specialization
    /// </summary>
    [JsonProperty]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Name of the specialization (e.g., "Paladin", "Berserker", "Hunter")
    /// </summary>
    [JsonProperty]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the specialization's focus and benefits
    /// </summary>
    [JsonProperty]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Icon filename or path for visual representation
    /// </summary>
    [JsonProperty]
    public string Icon { get; set; } = string.Empty;

    /// <summary>
    /// Minimum character level required to choose this specialization
    /// </summary>
    [JsonProperty]
    public int RequiredLevel { get; set; } = 10;

    /// <summary>
    /// Minimum skill points required to be eligible for this specialization
    /// </summary>
    [JsonProperty]
    public int RequiredSkillPoints { get; set; }

    /// <summary>
    /// List of specialization IDs that are mutually exclusive with this one
    /// (e.g., if player chooses Paladin, they cannot choose Berserker)
    /// </summary>
    [JsonProperty]
    public List<Guid> MutuallyExclusiveWithIds { get; set; } = [];

    /// <summary>
    /// Skill tree nodes specific to this specialization
    /// </summary>
    [JsonProperty]
    public List<SkillTreeNode> Nodes { get; set; } = [];

    /// <summary>
    /// Order/priority for display purposes
    /// </summary>
    [JsonProperty]
    public int DisplayOrder { get; set; }
}
