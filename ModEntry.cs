using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Shockah.DuoArtifacts;
using Shockah.Kokoro;
using Shockah.Soggins;
using TheJazMaster.Eddie.Artifacts;
using TheJazMaster.Eddie.Cards;
using TheJazMaster.TyAndSasha;

namespace TheJazMaster.Eddie;

public sealed class ModEntry : SimpleMod {
    internal static ModEntry Instance { get; private set; } = null!;


    internal Harmony Harmony { get; } = null!;
	internal IKokoroApi.IV2 KokoroApi { get; } = null!;
	internal IMoreDifficultiesApi? MoreDifficultiesApi { get; } 
    internal IApi? DuoArtifactsApi { get; } 
	internal ITyAndSashaApi? TyAndSashaApi { get; } 
    internal ISogginsApi? SogginsApi { get; } 
    internal IDraculaApi? DraculaApi { get; }


	internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations = null!;
	internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations = null!;
    

    internal IPlayableCharacterEntryV2 EddieCharacter { get; } = null!;
	internal Deck EddieDeck { get; }

    internal Spr PowerCellIcon { get; }
	internal Spr PowerCellSprite { get; }
	internal Spr EnergyIcon { get; }
	internal Spr DiscountHandIcon { get; }

	internal Spr FreeMarkerSprite { get; }
	internal Spr ShortCircuitOverlaySprite { get; }

	internal static IReadOnlyList<Type> AllCards { get; } = [
        typeof(PowerCell),
        typeof(PowerNap),
        typeof(Channel),
        typeof(Interference),
        typeof(EnergyBolt),
        typeof(Borrow),
        typeof(Rummage),
        typeof(RefundShot),
        typeof(SolarSailing),
        // typeof(OffPeak),

        typeof(ShortTermSolution),
        typeof(ChargeShields),
        typeof(ChargeCannons),
        typeof(ChargeThrusters),
        typeof(PowerSink),
        typeof(Amplify),
        typeof(GarageSale),
        // typeof(DysonSphere),

        typeof(Jumpstart),
        typeof(Circuit),
        typeof(Innovation),
        typeof(RenewableResource),
        typeof(GammaRay),
        // typeof(Hyperfocus),

        typeof(Surge),
        typeof(ReverseEngineer),

        typeof(EddieExe),
		typeof(Lightweight),

        typeof(BasicMove),
        typeof(BasicRubble)
    ];

    internal static IReadOnlyList<Type> AllArtifacts { get; } = [
		typeof(CircuitBoard),
		typeof(FrazzledWires),
		typeof(ElectromagneticCoil),
		typeof(FissionChamber),
        typeof(SunLamp),
	
		typeof(SolarPanels),
		typeof(DeconstructionGoggles),


        typeof(EmergencyThrusters)
	];

    internal static IReadOnlyList<Type> AllDuoArtifacts { get; } = [
		typeof(PerfectInsulation),
		typeof(UltraLightBatteries),
		typeof(OverdriveFeedback),
		typeof(Thunderstrike),
        typeof(EmergencyVentilator),
		typeof(VersionControl),
		typeof(Spellboard),
        typeof(VirtualTreadmill),
        typeof(WaxWings),
	];

	internal readonly string NeutralAnim;
	internal readonly string GameoverAnim;
	internal readonly string SeriousAnim;
	internal readonly string DisappointedAnim;
	internal readonly string OnEdgeAnim;
	internal readonly string NothingAnim;
	internal readonly string ExcitedAnim;
	internal readonly string RestingAnim;
	internal readonly string AnnoyedAnim;
	internal readonly string AnnoyedLeftAnim;
	internal readonly string ExplainsAnim;
	internal readonly string SquintAnim;
	internal readonly string WorriedAnim;

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
	{
        Instance = this;
		Harmony = new(package.Manifest.UniqueName);
		KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
		MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");
		SogginsApi = helper.ModRegistry.GetApi<ISogginsApi>("Shockah.Soggins");
		DuoArtifactsApi = helper.ModRegistry.GetApi<IApi>("Shockah.DuoArtifacts");
		TyAndSashaApi = helper.ModRegistry.GetApi<ITyAndSashaApi>("TheJazMaster.TyAndSasha");
		DraculaApi = helper.ModRegistry.GetApi<IDraculaApi>("Shockah.Dracula");

		AnyLocalizations = new JsonLocalizationProvider(
			tokenExtractor: new SimpleLocalizationTokenExtractor(),
			localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"I18n/{locale}.json").OpenRead()
		);
		Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
			new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
		);

        helper.Events.OnModLoadPhaseFinished += (_, phase) => {
            if (phase == ModLoadPhase.AfterDbInit) {
                ArtifactDialogue.Inject();
                CombatDialogue.Inject();
                EventDialogue.Inject();
                Memories.Inject();
            }
        };

		_ = new ShortCircuitManager();
		_ = new StatusManager();
        _ = new XIncreaseManager();
        _ = new CheapManager();
        _ = new ArtifactInterfaceManager();


		EddieDeck = Helper.Content.Decks.RegisterDeck("Eddie.EddieDeck", new()
		{
			Definition = new() {
				color = new Color("E6E164"),
				titleColor = Colors.black
			},
			DefaultCardArt = StableSpr.cards_colorless,
			BorderSprite = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/EddieCardFrame.png")).Sprite,
			Name = AnyLocalizations.Bind(["character", "eddie", "name"]).Localize,
		}).Deck;

        EddieCharacter = Helper.Content.Characters.V2.RegisterPlayableCharacter("Eddie.Character.Eddie", new()
		{
			Deck = EddieDeck,
			Description = AnyLocalizations.Bind(["character", "eddie", "description"]).Localize,
			BorderSprite = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/EddieFrame.png")).Sprite,
			Starters = new() {
                cards = {
                    new Channel(),
                    new PowerNap()
                }
            },
			NeutralAnimation = RegisterAnimation(EddieDeck, "Neutral"),
			MiniAnimation = RegisterAnimation(EddieDeck, "Mini"),
		});

		// All animations
		NeutralAnim = EddieCharacter.Configuration.NeutralAnimation!.Value.LoopTag;
        GameoverAnim = RegisterAnimation(EddieDeck, "Gameover").LoopTag;
        SeriousAnim = RegisterAnimation(EddieDeck, "Serious").LoopTag;
        DisappointedAnim = RegisterAnimation(EddieDeck, "Disappointed").LoopTag;
        OnEdgeAnim = RegisterAnimation(EddieDeck, "OnEdge").LoopTag;
        NothingAnim = RegisterAnimation(EddieDeck, "Nothing").LoopTag;
        ExcitedAnim = RegisterAnimation(EddieDeck, "Excited").LoopTag;
        RestingAnim = RegisterAnimation(EddieDeck, "Resting").LoopTag;
        AnnoyedAnim = RegisterAnimation(EddieDeck, "Annoyed").LoopTag;
        AnnoyedLeftAnim = RegisterAnimation(EddieDeck, "AnnoyedLeft").LoopTag;
        ExplainsAnim = RegisterAnimation(EddieDeck, "Explains").LoopTag;
        SquintAnim = RegisterAnimation(EddieDeck, "Squint").LoopTag;
        WorriedAnim = RegisterAnimation(EddieDeck, "Worried").LoopTag;

		Helper.ModRegistry.AwaitApiOrNull<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties", (api) => {
			if (api == null) return;

			api.RegisterAltStarters(EddieDeck, new() {
				cards = {
                    new Interference(),
                    new PowerCell()
                }
			});
		});

		PowerCellSprite = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/power_cell.png")).Sprite;
		PowerCellIcon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/icons/power_cell.png")).Sprite;
		EnergyIcon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/icons/energy.png")).Sprite;
        DiscountHandIcon = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/icons/discount_hand.png")).Sprite;
		FreeMarkerSprite = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile($"Sprites/free_marker.png")).Sprite;
		ShortCircuitOverlaySprite = Helper.Content.Sprites.RegisterSprite(Package.PackageRoot.GetRelativeFile("Sprites/short_circuit_overlay.png")).Sprite;

        foreach (var cardType in AllCards)
			AccessTools.DeclaredMethod(cardType, nameof(IRegisterableCard.Register))?.Invoke(null, [EddieDeck, Helper, Package]);
		foreach (var artifactType in AllArtifacts.Concat(AllDuoArtifacts))
			AccessTools.DeclaredMethod(artifactType, nameof(IRegisterableCard.Register))?.Invoke(null, [EddieDeck, Helper, Package]);


        List<IPartEntry> parts = [];
        foreach (string str in new string[] {"cannon", "scaffolding", "wing", "cockpit", "missiles"}) {
            parts.Add(helper.Content.Ships.RegisterPart($"solarsail_{str}", new() {
                Sprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/ship/{str}.png")).Sprite
            }));
        }
        helper.Content.Ships.RegisterShip("solarsail", new() {
            Name = AnyLocalizations.Bind(["ship", "name"]).Localize,
            Description = AnyLocalizations.Bind(["ship", "description"]).Localize,
            UnderChassisSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"Sprites/ship/chassis.png")).Sprite,
            Ship = new() {
                artifacts = {
                    new ShieldPrep(), new EmergencyThrusters()
                },
                cards = {
                    new CannonColorless(),
                    new BasicShieldColorless(),
                    new DodgeColorless(),
                    new BasicMove(),
                    new BasicRubble(),
                },
                ship = new() {
					x = 7,
					hull = 8,
					hullMax = 8,
					shieldMaxBase = 3,
					isPlayerShip = true,
                    parts = {
                        new Part {
                            type = PType.cannon,
                            skin = parts[0].UniqueName,
                            damageModifier = PDamMod.armor
                        },
                        new Part {
                            type = PType.empty,
                            skin = parts[1].UniqueName
                        },
                        new Part {
                            type = PType.wing,
                            skin = parts[2].UniqueName
                        },
                        new Part {
                            type = PType.cockpit,
                            skin = parts[3].UniqueName
                        },
                        new Part {
                            type = PType.missiles,
                            skin = parts[4].UniqueName
                        }
                    }
                }
            }
        });
        
        // helper.Events.OnLoadStringsForLocale += (_, args) => {
        //     args.Localizations["char.Eddie.EddieDeck"] = Localizations.Localize(["character", "eddie", "name"]);
        // };
        helper.Events.OnSaveLoaded += (_, state) => {
            int ind = state.characters.FindIndex(c => c.type == "Eddie.EddieDeck");
            if (ind != -1) {
                state.characters[ind] = new Character {
                    type = EddieCharacter.CharacterType,
                    deckType = EddieDeck
                };
            }
        };

        Harmony.PatchAll();
    }

	private CharacterAnimationConfigurationV2 RegisterAnimation(Deck deck, string name) =>
		Helper.Content.Characters.V2.RegisterCharacterAnimation("Eddie.Animation.Eddie" + name, new()
		{
			CharacterType = deck.Key(),
			LoopTag = name.ToLower(),
			Frames = [.. Enumerable.Range(1, 10)
				.Select(i => Package.PackageRoot.GetRelativeFile($"Sprites/portraits/{name}/Eddie{name}_{i}.png"))
				.TakeWhile(f => f.Exists)
				.Select(f => Helper.Content.Sprites.RegisterSprite(f).Sprite)]
        }).Configuration;
    
    public override IEddieApi GetApi(IModManifest requestingMod) => new ApiImplementation();
}