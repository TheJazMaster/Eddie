using CobaltCoreModding.Definitions.ExternalItems;
using System;
using System.Collections.Generic;

namespace TheJazMaster.Eddie;

public interface IDuoArtifactsApi
{
	ExternalDeck DuoArtifactDeck { get; }

	void RegisterDuoArtifact(Type type, IEnumerable<Deck> combo);
	void RegisterDuoArtifact<TArtifact>(IEnumerable<Deck> combo) where TArtifact : Artifact;
	IReadOnlySet<Deck>? GetDuoArtifactOwnership(Artifact artifact);
}