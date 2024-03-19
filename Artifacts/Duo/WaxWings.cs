using TheJazMaster.Eddie.Cards;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace TheJazMaster.Eddie.Artifacts;

[ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
public class WaxWings : Artifact, ISmugHook
{
	static ISogginsApi? SogginsAPI => Manifest.Instance.SogginsApi;

	bool active = false;

	public override void OnQueueEmptyDuringPlayerTurn(State state, Combat combat)
	{
		if (SogginsAPI != null) {
			active = SogginsAPI.IsOversmug(state, state.ship);
		}
	}

	public void OnCardBotchedBySmug(State state, Combat combat, Card card) {
		if (active) {
			Pulse();
			combat.Queue(new List<CardAction> {
				new ADelay {
					time = 0.3
				},
				new AEnergy {
					changeAmount = 2
				}
			});
		}
	}
}
