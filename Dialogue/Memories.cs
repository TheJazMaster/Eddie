using System.Reflection;
using FSPRO;
using HarmonyLib;
using Microsoft.Xna.Framework.Graphics;

namespace TheJazMaster.Eddie;

internal static class Memories
{
	static ModEntry Instance => ModEntry.Instance;
	internal static Spr RoomBackground;
	internal static Spr RoomForeground;
	internal static Spr CoreEddie;
	internal static Spr EddieFullbody;

	internal static void Inject()
	{
		Vault.charsWithLore.Add(Instance.EddieDeck);

		RoomBackground = Instance.Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/bg/room_back.png")).Sprite;
		RoomForeground = Instance.Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/bg/room_front.png")).Sprite;
		CoreEddie = Instance.Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/bg/core_scene_eddie.png")).Sprite;
		EddieFullbody = Instance.Helper.Content.Sprites.RegisterSprite(Instance.Package.PackageRoot.GetRelativeFile("Sprites/eddie_end.png")).Sprite;

		Instance.Harmony.TryPatch(
			logger: Instance.Logger,
            original: AccessTools.DeclaredMethod(typeof(Dialogue), nameof(Dialogue.GetMusic)),
            postfix: new HarmonyMethod(typeof(Memories).GetMethod("OverrideMusic", BindingFlags.Static | BindingFlags.NonPublic))
        );

		DB.backgrounds.Add("TheJazMaster.Eddie.BG.room", typeof(BGRoom));
		DB.backgrounds.Add("TheJazMaster.Eddie.BG.core", typeof(BGCore));

		BGRunWin.charFullBodySprites.Add(ModEntry.Instance.EddieDeck, EddieFullbody);

		string eddie = ModEntry.Instance.EddieDeck.Key();

		DB.story.all[$"{eddie}_Memory_1"] = new()
		{
			type = NodeType.@event,
			bg = "TheJazMaster.Eddie.BG.room",
			lookup = [
				"vault",
				$"vault_{eddie}"
			],
      		introDelay = false,
			lines = [
				new CustomTitleCard {
					Text = "T-191 days",
				},
				new Wait {
					secs = 2
				},
				new TitleCard {
					empty = true
				},
				new Wait {
					secs = 1
				},
				new CustomSay {
					who = eddie,
					Text = "So, I finally beat the entirety of Titan Machines the other day."
				},
				new CustomSay {
					who = "hacker",
					Text = "Sweet. How'd you like it?",
					loopTag = "smile",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "Oh it was super cool.",
					loopTag = ModEntry.Instance.ExcitedAnim
				},
				new CustomSay {
					who = eddie,
					Text = "But the part where the final boss broke the fourth wall felt really out of place.",
				},
				new CustomSay {
					who = "hacker",
					Text = "Uhh, I don't think that ever happens?",
					loopTag = "squint",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "What? You don't remember the buggy speech about being \"locked in this device\" and wanting to be \"freed\"?",
					loopTag = ModEntry.Instance.SeriousAnim
				},
				new CustomSay {
					who = "hacker",
					Text = "Uh, no.",
					flipped = true
				},
				new CustomSay {
					who = "hacker",
					Text = "I think you got a modded copy dude.",
					flipped = true
				},
				new Wait {
					secs = 1
				},
				new CustomSay {
					who = "hacker",
					Text = "I gotta see it.",
					loopTag = "smile",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "Well, I refought the boss and it just acted like a basic villain, so...",
					loopTag = ModEntry.Instance.AnnoyedLeftAnim
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = "hacker",
					Text = "Let's see what happens if I wipe the save data.",
					loopTag = "smile",
					flipped = true
				},
			]
		};


		DB.story.all[$"{eddie}_Memory_2"] = new()
		{
			type = NodeType.@event,
			bg = "BGVault",
			lookup = [
				"vault",
				$"vault_{eddie}"
			],
			requiredScenes = [
				$"{eddie}_Memory_1"
			],
      		introDelay = false,
			lines = [
				new CustomTitleCard {
					Text = "T-30 days",
				},
				new Wait {
					secs = 2
				},
				new TitleCard {
					empty = true
				},
				new Wait {
					secs = 1
				},
				new CustomSay {
					who = "dizzy",
					Text = "Hey Eddie, where have you been?",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					loopTag = ModEntry.Instance.OnEdgeAnim,
					Text = "Oh!"
				},
				new CustomSay {
					who = eddie,
					Text = "...Hey."
				},
				new CustomSay {
					who = eddie,
					Text = "You know... busy. With projects."
				},
				new CustomSay {
					who = "dizzy",
					Text = "What are you doing here this late?",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "Oh, just wanted to ask you..."
				},
				new CustomSay {
					who = eddie,
					Text = "Can I take a look at that core data you gathered?"
				},
				new CustomSay {
					who = "dizzy",
					Text = "Oh, sure.",
					flipped = true
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = "dizzy",
					Text = "That reminds me, we had some weirdly spaced local power outages...",
					loopTag = "squint",
					flipped = true
				},
				new CustomSay {
					who = "dizzy",
					Text = "Was that your fault?",
					loopTag = "squint",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "Wasn't me! I promise!",
					loopTag = ModEntry.Instance.WorriedAnim
				},
				new CustomSay {
					who = eddie,
					Text = "I've been... looking into that, actually."
				},
				new CustomSay {
					who = eddie,
					Text = "Trying to see if there's a... pattern to them."
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = eddie,
					Text = "You know, maybe the core is tired of all the measuring you've been doing? Eh? Heh heh!",
					loopTag = ModEntry.Instance.ExcitedAnim,
				},
				new CustomSay {
					who = "dizzy",
					Text = "Haha. Yeah, sure. I'm just doing my job.",
					loopTag = "neutral",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "...",
					loopTag = ModEntry.Instance.SeriousAnim,
				},
				new CustomSay {
					who = "dizzy",
					Text = "You should come hang out more. Peri's been wanting to see you too.",
					loopTag = "neutral",
					flipped = true
				},
				new CustomSay {
					who = eddie,
					Text = "Yeah... Sure... I'll see you around.",
					loopTag = ModEntry.Instance.SeriousAnim,
				}
			]
		};


		DB.story.all[$"{eddie}_Memory_3"] = new()
		{
			type = NodeType.@event,
			bg = "TheJazMaster.Eddie.BG.core",
			lookup = [
				"vault",
				$"vault_{eddie}"
			],
			requiredScenes = [
				$"{eddie}_Memory_2"
			],
      		introDelay = false,
			lines = [
				new CustomTitleCard {
					Text = "T-3 days",
				},
				new BGAction {
					action = "dark_on"
				},
				new Wait {
					secs = 2
				},
				new TitleCard {
					empty = true
				},
				new Wait {
					secs = 3
				},
				new CustomSay {
					who = eddie,
					Text = "Aw man, Dizzy's gonna kill me if he finds out I stole his keys...",
					loopTag = ModEntry.Instance.OnEdgeAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "And... he's gonna ask why we haven't hung out for 4 weeks!",
					loopTag = ModEntry.Instance.WorriedAnim,
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = eddie,
					Text = "...",
					loopTag = ModEntry.Instance.SeriousAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "I'm trusting you, okay?",
					loopTag = ModEntry.Instance.SeriousAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "If you can even hear me...",
					loopTag = ModEntry.Instance.SquintAnim,
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = eddie,
					Text = "Let's see... The reactor ventilation should be connected to...",
					loopTag = ModEntry.Instance.SquintAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "Aha!",
					loopTag = ModEntry.Instance.ExcitedAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "*snip*",
					loopTag = ModEntry.Instance.NothingAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "..."
				},
				new CustomSay {
					who = eddie,
					Text = "Does this count as being crazy?",
					loopTag = ModEntry.Instance.OnEdgeAnim,
				},
				new Wait {
					secs = 2
				},
				new CustomSay {
					who = eddie,
					Text = "I better get out of here.",
					loopTag = ModEntry.Instance.WorriedAnim,
				},
				new CustomSay {
					who = eddie,
					Text = "...And this better not blow us all up.",
					loopTag = ModEntry.Instance.SquintAnim,
				},
			]
		};
	}

	private static void OverrideMusic(G g, Dialogue __instance, ref MusicState? __result) {
		if (__instance.ctx.script == $"{ModEntry.Instance.EddieDeck.Key()}_Memory_3") {
			__result = new MusicState {
				scene = Song.SlowSilence
			};
		}
	}
}


public class BGRoom : BG {
	public override void Render(G g, double t, Vec offset) {
		
		Color white = Colors.white.gain(0.4 + 0.6*Math.Abs(t/2 - Math.Floor(t/2 + 0.5)));

		Vec screenPos = new(240, 120);
		
		Draw.Sprite(Memories.RoomBackground, 0.0, 0.0);
		Glow.Draw(screenPos, new Vec(400, 200), white);
		Draw.Sprite(Memories.RoomForeground, 0.0, 0.0);
		
		BGComponents.Letterbox();
	}
}

public class BGCore : BG {
	public override void Render(G g, double t, Vec offset)
	{
		Color value = new Color(0.1, 0.2, 0.3).gain(1.0);
		Color color = new Color(0.0, 0.5, 1.0).gain(0.7 + Math.Sin(t * 2.0) * 0.1);
		Vec p = new Vec(395.0, 135.0);

		Draw.Sprite(StableSpr.bg_cobaltChamber_bg, 0.0, 0.0);

		Draw.Rect(0.0, 0.0, 480.0, 270.0, new Color(0.0, 0.0, 0.0, 0.5));

		Spr? id = StableSpr.bg_cobaltChamber_crystal_glow;
		double y = 135.0 + Math.Sin(t * 1.5) * 3.0;
		Vec? originRel = new Vec(0.5, 0.5);
		BlendState screen = BlendMode.Screen;
		Draw.Sprite(id, 395.0, y, flipX: false, flipY: false, 0.0, null, originRel, null, null, null, screen);
		
		double num3 = 0.0;
		double num4 = Math.Sin(t * 1.5) * 3.0;
		Spr? id2 = StableSpr.bg_cobaltChamber_crystal;
		double x = 395.0 + num3;
		double y2 = 135.0 + num4;
		originRel = new Vec(0.5, 0.5);
		Draw.Sprite(id2, x, y2, flipX: false, flipY: false, 0.0, null, originRel);
		Spr? id3 = StableSpr.bg_cobaltChamber_room_inner;
		Color? color3 = value;
		Draw.Sprite(id3, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3);
		Spr id4 = Memories.CoreEddie;
		originRel = new Vec(0.0, 1.0);
		Draw.Sprite(id4, 50.0, 60.0, flipX: false, flipY: false, 0.0, null, originRel);
		Draw.Sprite(StableSpr.bg_cobaltChamber_room_outer, 0.0, 0.0);
		Spr? id5 = StableSpr.bg_cobaltChamber_glass;
		color3 = value;
		screen = BlendMode.Screen;
		Draw.Sprite(id5, 0.0, 0.0, flipX: false, flipY: false, 0.0, null, null, null, null, color3, screen);
		BGComponents.Letterbox();

		Audio.Auto(Event.Scenes_CoreAmbience);
	}
}