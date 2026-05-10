using System;
using System.IO;
using Nanoray.PluginManager;
using Nickel;

namespace TheJazMaster.Eddie;

internal interface IDialogueArtifact {
	public void InjectDialogue();
}

internal interface IRegisterableCard
{
	public static ICardEntry Register(Type type, Deck deck, Rarity rarity, Spr? art = null, bool dontOffer = false) {
		return ModEntry.Instance.Helper.Content.Cards.RegisterCard("Eddie.Cards." + type.Name, new()
		{
			CardType = type,
			Meta = new()
			{
				deck = deck,
				rarity = rarity,
				upgradesTo = [Upgrade.A, Upgrade.B],
				dontOffer = dontOffer
			},
			Art = art,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["card", type.Name, "name"]).Localize
		});
	}

	static abstract void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package);
}

internal interface IRegisterableArtifact
{
	public static IArtifactEntry Register(Type type, Deck deck, ArtifactPool[] pools, Spr sprite, bool unremovable = false) {
		return ModEntry.Instance.Helper.Content.Artifacts.RegisterArtifact("Eddie.Artifacts." + type.Name, new()
		{
			ArtifactType = type,
			Meta = new()
			{
				owner = deck,
				pools = pools,
				unremovable = unremovable
			},
			Sprite = sprite,
			Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", type.Name, "name"]).Localize,
			Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", type.Name, "description"]).Localize
		});
	}

	static abstract void Register(Deck deck, IModHelper helper, IPluginPackage<IModManifest> package);
}