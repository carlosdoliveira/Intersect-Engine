using Newtonsoft.Json;

namespace Intersect.Framework.Core.GameObjects.PlayerClass;

/// <summary>
/// Main container for a class's skill tree configuration
/// </summary>
public partial class ClassSkillTree
{
    /// <summary>
    /// Class ID this skill tree belongs to
    /// </summary>
    [JsonProperty]
    public Guid ClassId { get; set; }

    /// <summary>
    /// Base skill tree nodes available to all players of this class
    /// These represent innate abilities that unlock as the player levels up
    /// </summary>
    [JsonProperty]
    public List<SkillTreeNode> BaseNodes { get; set; } = [];

    /// <summary>
    /// Available specializations for this class
    /// </summary>
    [JsonProperty]
    public List<SkillTreeSpecialization> Specializations { get; set; } = [];

    /// <summary>
    /// Number of skill points gained per level
    /// </summary>
    [JsonProperty]
    public int SkillPointsPerLevel { get; set; } = 1;

    /// <summary>
    /// Maximum total skill points a character can accumulate (0 = unlimited)
    /// </summary>
    [JsonProperty]
    public int MaxSkillPoints { get; set; }

    /// <summary>
    /// Whether players can reset their skill trees
    /// </summary>
    [JsonProperty]
    public bool AllowReset { get; set; } = true;

    /// <summary>
    /// Cost to reset skill tree (item ID or gold amount - implementation specific)
    /// </summary>
    [JsonProperty]
    public long ResetCost { get; set; }

    /// <summary>
    /// Validates the skill tree configuration for circular dependencies and invalid references
    /// </summary>
    /// <returns>Tuple of (isValid, list of error messages)</returns>
    public (bool IsValid, List<string> Errors) Validate()
    {
        var errors = new List<string>();
        var allNodes = new Dictionary<Guid, SkillTreeNode>();

        // Collect all nodes
        foreach (var node in BaseNodes)
        {
            if (allNodes.ContainsKey(node.Id))
            {
                errors.Add($"Duplicate node ID found: {node.Id} ({node.Name})");
            }
            else
            {
                allNodes[node.Id] = node;
            }
        }

        foreach (var specialization in Specializations)
        {
            foreach (var node in specialization.Nodes)
            {
                if (allNodes.ContainsKey(node.Id))
                {
                    errors.Add($"Duplicate node ID found: {node.Id} ({node.Name}) in specialization {specialization.Name}");
                }
                else
                {
                    allNodes[node.Id] = node;
                }
            }
        }

        // Validate prerequisites
        foreach (var node in allNodes.Values)
        {
            foreach (var prereqId in node.PrerequisiteNodeIds)
            {
                if (!allNodes.ContainsKey(prereqId))
                {
                    errors.Add($"Node '{node.Name}' references non-existent prerequisite: {prereqId}");
                }
            }
        }

        // Check for circular dependencies
        foreach (var node in allNodes.Values)
        {
            if (HasCircularDependency(node, allNodes, new HashSet<Guid>()))
            {
                errors.Add($"Circular dependency detected involving node: {node.Name}");
            }
        }

        // Validate specialization mutual exclusivity
        foreach (var spec in Specializations)
        {
            foreach (var exclusiveId in spec.MutuallyExclusiveWithIds)
            {
                var exclusiveSpec = Specializations.FirstOrDefault(s => s.Id == exclusiveId);
                if (exclusiveSpec == null)
                {
                    errors.Add($"Specialization '{spec.Name}' references non-existent mutually exclusive specialization: {exclusiveId}");
                }
            }
        }

        return (errors.Count == 0, errors);
    }

    private bool HasCircularDependency(
        SkillTreeNode node,
        Dictionary<Guid, SkillTreeNode> allNodes,
        HashSet<Guid> visitedNodes)
    {
        if (visitedNodes.Contains(node.Id))
        {
            return true;
        }

        visitedNodes.Add(node.Id);

        foreach (var prereqId in node.PrerequisiteNodeIds)
        {
            if (allNodes.TryGetValue(prereqId, out var prereqNode))
            {
                if (HasCircularDependency(prereqNode, allNodes, new HashSet<Guid>(visitedNodes)))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
