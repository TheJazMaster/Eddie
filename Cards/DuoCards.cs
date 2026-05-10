using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie.Cards;

public class Lightweight : Card, IRegisterableCard
{
    public static void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package)
    {
        if (ModEntry.Instance.DuoArtifactsApi != null)
            IRegisterableCard.Register(
                MethodBase.GetCurrentMethod()!.DeclaringType!,
                ModEntry.Instance.DuoArtifactsApi!.DuoArtifactVanillaDeck,
                Rarity.common,
                StableSpr.cards_Fleetfoot,
                true
            );
    }

    public override CardData GetData(State state) => new() {
        cost = 0,
        singleUse = true,
        temporary = true
    };

    public override List<CardAction> GetActions(State s, Combat c) => [
        new AStatus {
            targetPlayer = true,
            status = Status.hermes,
            statusAmount = 1
        }
    ];
}