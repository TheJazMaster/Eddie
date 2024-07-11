using TheJazMaster.Eddie.Actions;

namespace TheJazMaster.Eddie.Cards;

public class BasicRubble : Card
{

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
	public override CardData GetData(State state) => new() {
		cost = 0,
		recycle = upgrade == Upgrade.B,
		flippable = upgrade == Upgrade.A
	};

	public override List<CardAction> GetActions(State s, Combat c) => [
		new AMove {
			dir = 1,
			targetPlayer = true
		}
	];
}