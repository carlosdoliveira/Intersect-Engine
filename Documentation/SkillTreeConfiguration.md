# Skill Tree Configuration System

## Overview

The Skill Tree Configuration System allows each class in Intersect Engine to have:
1. **Base Skill Tree**: Innate abilities that unlock as the player levels up
2. **Specializations**: Class specializations (e.g., Warrior → Paladin or Berserker) with unique skill paths

## Components

### SkillTreeRewardType
Defines what a skill tree node grants:
- `None`: No reward (connector nodes)
- `Spell`: Grants a spell/ability
- `StatBonus`: Provides stat bonuses (Attack, Defense, etc.)
- `VitalBonus`: Provides vital bonuses (Health, Mana)
- `PassiveAbility`: Grants a passive effect

### SkillTreeNode
Represents a single node in the skill tree:
- **Id**: Unique identifier
- **Name**: Display name
- **Description**: What the node provides
- **Icon**: Visual representation
- **RequiredLevel**: Minimum level to unlock
- **PrerequisiteNodeIds**: Nodes that must be unlocked first
- **SpecializationId**: Which specialization this belongs to (null for base tree)
- **PositionX/Y**: UI placement coordinates
- **RewardType**: Type of reward
- **RewardSpellId**: Spell granted (if applicable)
- **StatBonuses**: Dictionary of stat increases
- **VitalBonuses**: Dictionary of vital increases
- **SkillPointCost**: Points required to unlock
- **CustomData**: Extensible custom data

### SkillTreeSpecialization
Represents a class specialization:
- **Id**: Unique identifier
- **Name**: Specialization name (e.g., "Paladin")
- **Description**: Focus and benefits
- **Icon**: Visual representation
- **RequiredLevel**: When specialization becomes available
- **RequiredSkillPoints**: Minimum points to choose
- **MutuallyExclusiveWithIds**: Other specializations that conflict
- **Nodes**: List of nodes in this specialization
- **DisplayOrder**: Sort order

### ClassSkillTree
Main container for a class's skill tree:
- **ClassId**: Links to ClassDescriptor
- **BaseNodes**: Innate abilities for all players
- **Specializations**: Available specializations
- **SkillPointsPerLevel**: Points gained per level
- **MaxSkillPoints**: Optional cap (0 = unlimited)
- **AllowReset**: Whether reset is allowed
- **ResetCost**: Cost to reset

## Example Configuration

### Warrior Class with Paladin and Berserker Specializations

```csharp
var warriorClass = ClassDescriptor.Get(warriorClassId);

// Configure skill tree
warriorClass.SkillTree.SkillPointsPerLevel = 1;
warriorClass.SkillTree.AllowReset = true;

// Base Tree - Innate Warrior Abilities
var powerAttackNode = new SkillTreeNode
{
    Name = "Power Attack",
    Description = "A devastating melee strike",
    RequiredLevel = 1,
    RewardType = SkillTreeRewardType.Spell,
    RewardSpellId = powerAttackSpellId,
    PositionX = 0,
    PositionY = 0
};

var enduranceNode = new SkillTreeNode
{
    Name = "Endurance Training",
    Description = "Increases maximum health",
    RequiredLevel = 3,
    RewardType = SkillTreeRewardType.VitalBonus,
    VitalBonuses = new Dictionary<Vital, long> { { Vital.Health, 50 } },
    PositionX = 0,
    PositionY = 1
};

var battleCryNode = new SkillTreeNode
{
    Name = "Battle Cry",
    Description = "Inspire nearby allies",
    RequiredLevel = 5,
    RewardType = SkillTreeRewardType.Spell,
    RewardSpellId = battleCrySpellId,
    PrerequisiteNodeIds = new List<Guid> { powerAttackNode.Id },
    PositionX = 0,
    PositionY = 2
};

warriorClass.SkillTree.BaseNodes.AddRange(new[] {
    powerAttackNode,
    enduranceNode,
    battleCryNode
});

// Paladin Specialization
var paladinSpec = new SkillTreeSpecialization
{
    Name = "Paladin",
    Description = "Holy warrior specializing in healing and divine magic",
    RequiredLevel = 10,
    RequiredSkillPoints = 5,
    DisplayOrder = 0
};

paladinSpec.Nodes.Add(new SkillTreeNode
{
    Name = "Holy Strike",
    Description = "Smite enemies with divine power",
    RequiredLevel = 10,
    RewardType = SkillTreeRewardType.Spell,
    RewardSpellId = holyStrikeSpellId,
    SpecializationId = paladinSpec.Id,
    PositionX = 1,
    PositionY = 0
});

paladinSpec.Nodes.Add(new SkillTreeNode
{
    Name = "Divine Shield",
    Description = "Protect yourself with holy energy",
    RequiredLevel = 15,
    RewardType = SkillTreeRewardType.Spell,
    RewardSpellId = divineShieldSpellId,
    SpecializationId = paladinSpec.Id,
    PrerequisiteNodeIds = new List<Guid> { paladinSpec.Nodes[0].Id },
    PositionX = 1,
    PositionY = 1
});

// Berserker Specialization
var berserkerSpec = new SkillTreeSpecialization
{
    Name = "Berserker",
    Description = "Rage-fueled warrior specializing in devastating physical attacks",
    RequiredLevel = 10,
    RequiredSkillPoints = 5,
    DisplayOrder = 1,
    MutuallyExclusiveWithIds = new List<Guid> { paladinSpec.Id }
};

berserkerSpec.Nodes.Add(new SkillTreeNode
{
    Name = "Rage",
    Description = "Enter a berserker rage, increasing damage",
    RequiredLevel = 10,
    RewardType = SkillTreeRewardType.Spell,
    RewardSpellId = rageSpellId,
    SpecializationId = berserkerSpec.Id,
    PositionX = 2,
    PositionY = 0
});

berserkerSpec.Nodes.Add(new SkillTreeNode
{
    Name = "Frenzy",
    Description = "Increases attack speed dramatically",
    RequiredLevel = 15,
    RewardType = SkillTreeRewardType.StatBonus,
    StatBonuses = new Dictionary<Stat, int> { { Stat.Speed, 20 } },
    SpecializationId = berserkerSpec.Id,
    PrerequisiteNodeIds = new List<Guid> { berserkerSpec.Nodes[0].Id },
    PositionX = 2,
    PositionY = 1
});

// Make specializations mutually exclusive
paladinSpec.MutuallyExclusiveWithIds.Add(berserkerSpec.Id);

// Add specializations to class
warriorClass.SkillTree.Specializations.AddRange(new[] {
    paladinSpec,
    berserkerSpec
});

// Validate the configuration
var (isValid, errors) = warriorClass.SkillTree.Validate();
if (!isValid)
{
    foreach (var error in errors)
    {
        Console.WriteLine($"Validation Error: {error}");
    }
}

// Save the class
ClassDescriptor.SaveAll();
```

## Validation

The `ClassSkillTree.Validate()` method checks for:
- Duplicate node IDs
- Invalid prerequisite references
- Circular dependencies
- Invalid specialization references

Always validate your skill tree configuration before saving.

## Future Enhancements

The system is designed to support:
- Visual UI editor (position data already included)
- Player skill tree state tracking
- Reset mechanics
- Custom passive abilities
- Item requirements for nodes
- Quest requirements for specializations

## Database Storage

The skill tree is stored as a JSON-serialized column in the Classes table. The `JsonSkillTree` property on `ClassDescriptor` handles serialization/deserialization automatically.

## Notes

- Each class automatically gets an empty skill tree when created
- Skill trees are saved with the class configuration
- The ClassId is automatically set when a ClassDescriptor is created
- Position coordinates are for future UI implementation
