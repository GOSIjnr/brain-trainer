using Godot;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class Data : Node
{
	[Export]
	public Dictionary<RarityLevel, Color> RarityColors { get; private set; } = new()
	{
		{ RarityLevel.Common, new("#B0B0B0") },
		{ RarityLevel.Uncommon, new("#1EFF00") },
		{ RarityLevel.Rare, new("#0070DD") },
		{ RarityLevel.Epic, new("#A335EE") },
		{ RarityLevel.Legendary, new("#FF8000") },
		{ RarityLevel.Mythic, new("#FF0000") },
		{ RarityLevel.Artifact, new("#E0115F") }
	};

	public enum RarityLevel
	{
		Common,
		Uncommon,
		Rare,
		Epic,
		Legendary,
		Mythic,
		Artifact,
	}
}
