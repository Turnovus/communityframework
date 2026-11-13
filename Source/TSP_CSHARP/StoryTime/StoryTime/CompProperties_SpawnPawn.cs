using RimWorld;
using Verse;

namespace StoryTime;

public class CompProperties_SpawnPawn : CompProperties_UseEffect
{
	public PawnKindDef pawnKind;

	public int amount = 1;

	public FactionDef forcedFaction;

	public bool usePlayerFaction = true;

	public string pawnSpawnedStringKey = "BeanManSpawnedMessage";

	public bool sendMessage = true;

	public CompProperties_SpawnPawn()
	{
		compClass = typeof(CompUseEffect_SpawnPerson);
	}
}
