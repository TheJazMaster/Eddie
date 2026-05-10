using System.Reflection;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Cards;

public class BasicRubble : Card, IRegisterableCard
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Deck.colorless,
			Rarity.common,
			StableSpr.cards_GoatDrone,
			true
		);
	}

	public override CardData GetData(State state) => new() {
		cost = upgrade == Upgrade.A ? 0 : 1
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new ASpawn {
			thing = new Asteroid {
				bubbleShield = upgrade == Upgrade.B
			}
		}
	];
}

public class BasicMove : Card
{
	public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package) {
		IRegisterableCard.Register(MethodBase.GetCurrentMethod()!.DeclaringType!, Deck.colorless,
			Rarity.common,
			null,
			true
		);
	}

	public override CardData GetData(State state) => new() {
		cost = 0,
		recycle = upgrade == Upgrade.B,
		flippable = upgrade == Upgrade.A,
		art = flipped ? StableSpr.cards_ScootLeft : StableSpr.cards_ScootRight
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AMove {
			dir = 1,
			targetPlayer = true
		}
	];
}