# 14. SIDE EFFECTS — cooperative game concept

[한국어](14-side-effects.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


> Archived record · Discontinued 2026-09-12. Not a current launch guide or implementation specification. Links to removed files point to the archive containing their originals. [Unity 메인 / Mainline](18-unity-mainline.en.md).

[Lead development agent handoff](16-side-effects-handoff.en.md) — recommended implementation order and first-prototype acceptance criteria.

2026-09-11 · Revision 2 · New proposal / awaiting direction review / not implemented

[Manyfast](https://manyfast.io/editor/427d2942-d820-4899-abbf-664df084d5ed?map=prdMap) · [Previous candidate comparison](13-manyfast-brief.en.md)

## Game definition

Working title: SIDE EFFECTS / 부작용 원정대. A 2–4-player 3D cooperative action roguelite about apprentices from an incompetent magic guild reclaiming a castle with defective magical tools. Proposed format: 20–30-minute runs, PC first, individual third-person cameras. The team has powerful abilities it has not learned to handle, rather than simply being weak. All counts, durations and numeric rules are testing proposals.

## Art concept — a broken fairy-tale stage

Keywords: awkward heroes, repaired magic and exaggerated mishaps. Use large simple forms reminiscent of paper theater, with three-dimensional puppet characters made of cloth, wood and matte metal. A worn castle contains leaning towers, stitched banners, cracked porcelain knights and overgrown mushrooms. Aim for adventurous comedy that is cute without looking exclusively preschool-oriented.
Characters have short broad torsos, large gloves and loose capes so hand actions and falls read clearly. Distinguish teammates using color together with hat silhouettes and patterns, never color alone. Convey emotion through head, arm and body reactions rather than facial detail.
Use ink navy, faded teal and ivory for environments, with saturated player accents. Telegraph danger through amber warnings and floor patterns; distinguish healing through mint rings and upward symbols. Combine color, shape and sound, and keep effects from obscuring characters or footing.
The representative scene is a ruined moonlit banquet hall. An apprentice with a cauldron staff heals a friend, producing a giant bandage-shaped shockwave that launches the friend. Beside them, another apprentice swings a hammer and knocks a porcelain knight the other way. Broken chairs, a turning magic windmill and the friend's awkward pose make the cause readable. Start production review with this scene and 1 character. No concept image or model has yet been produced.

## Core Fun — turn mishaps into team techniques

The primary pleasure is recovering from a mishap with teammates, then deliberately exploiting that drawback next time. Anticipation of random rewards supports this.
1. Cooperation and accountability: the healer who knocks a friend away runs to rescue them. Players participate in both cause and resolution.
2. Discovery: repurpose initially dangerous pushing, pulling and recoil against enemies and terrain to invent team techniques.
3. Mastery: read telegraphs and coordinate position, direction and timing for better results with the same tool. Do not deliberately destabilize basic movement.
The emotional sequence is anticipation → tension → mishap → laughter or alarm → rescue → shared success → desire for another combination. Do not assume every accident is funny; provide recovery opportunities and understandable causes.

## Core tools and combat

All players share movement, jump, dodge, basic attack, tool use and interaction. Equipped tools create temporary roles rather than fixed classes. Basic attacks deal normal damage without friendly damage. Tool displacement also affects allies to create mishaps, but brief protection prevents repeated friendly side effects; tune its duration in testing.
Tool A, knockback healing staff: heals allies in a short forward cone and pushes them away from its user. Show direction and range before casting. It supports rescue, spacing and gap crossing.
Tool B, magnetic shield: while blocking, attracts metallic enemies and props in front. It groups enemies but can pile them onto a teammate's position. Identify metal through form and glints.
Tool C, recoil hammer: strongly pushes enemies and recoils its user backward. Back against a wall, attack stably; with clear space behind, use recoil to escape.
Design each tool as a useful ability paired with a drawback. Instead of mass-producing damage variants, give tools distinct combat and traversal or rescue applications.

## Randomness — preserve choices after the draw

Show tool abilities and drawbacks before departure. Never unexpectedly change an already presented tool rule inside a room. Between rooms, players may replace tools from offered candidates or keep their current loadout. Keeping a familiar configuration should remain viable.
Start with authored combat and traversal rooms connected with varied enemy placement, selected devices and rewards. Propose 1 dominant environmental modifier per room. For example, crosswinds affect hammer recoil and healing knockback without randomizing attack direction itself.
Restrict combinations through tag-based allowlists. Filter inaccessible mandatory targets, falls with no rescue route, and magnetic-tool offers where no metallic target exists. Mandatory routes remain traversable with basic actions rather than requiring one specific tool. Distinguish random draws from physical outcomes, and communicate causes and telegraphs.

## Core Loop — four time scales

Moment loop: read the situation and telegraphs → choose position and target → use a tool → experience the drawback → dodge, combine or rescue → gain brief safety. Repetition develops control mastery and coordination.
Room loop: identify the objective and environmental modifier → resolve combat or a cooperative device → inspect rewards → replace, retain or distribute → choose the next room. Vary required actions while keeping control rules consistent.
Run loop: practice tools and prepare at the guild → undertake the expedition → bank rewards and choose to continue or return at an intermediate safe room → defeat the castle guardian, return early or wipe → settle results and prepare again. Defeating the guardian and returning is the full objective; early return is partial success.
Long-term loop: experiment → discover combinations → unlock tool choices and cosmetics → retry with a new setup. Emphasize horizontal options and customization over permanent stat disparities. One execution mistake should not erase already banked unlock progress.

## Failure, rescue and rewards

When health is depleted, players crawl slowly and send help pings. Teammates pull them to safety and revive them. Where possible, falls lead to lower rescue ledges; for unrecoverable falls, propose returning the player downed to the last safe position to limit spectating.
A full-team down ends the run. Retain rewards banked at the safe room and lose subsequent unbanked rewards. A wipe before banking may yield no run reward, but existing unlocks remain. Exact payouts and safe-room spacing are open.
Avoid turning revival into unlimited healing: do not revive at full health, and tune repeated downs through healing resources and enemy pressure. Test risk borne by the rescuing team rather than timed life depletion or lengthy spectating.

## UI and scenes readable on stream

Keep a reticle and contextual action prompts at the center. At the bottom show health, dodge state, and current tool benefit/drawback icons. Show teammate names, unique patterns and downed status on the left, and one current objective at the top. Differentiate enemy telegraphs from allied tool areas by shape. Reticle and short feedback should immediately reflect changed targets or unavailable actions.
The practice area teaches movement, safe tool use and a short drawback-enabled shortcut through actions rather than mandatory long text. Keyboard/mouse bindings remain proposed; rebinding and controller scope belong to the next design stage.
Favor rescues, shared successes and the farthest launch in the results screen over kill-count competition. The last item is a decorative event-based statistic, not automatic video editing in the initial scope. Play must work with external voice chat; built-in proximity voice and viewer voting are optional expansions.

## Representative run sequence

At the guild, divide a mint healing staff, yellow magnetic shield and red recoil hammer. In the first banquet hall, the shield groups knights and the hammer pushes them together, teaching combined effects.
Crosswinds sweep the next tower. A healer knocks a friend beside the railing onto a lower ledge. Rescue teaches the push direction. At a broken bridge later, the same staff launches a teammate to a switch across the gap. A basic detour exists, so this tool is not a mandatory key.
At the safe room, bank rewards and discuss whether to swap for a stronger but narrower push tool. Against the guardian, the hammer user deliberately recoils to bait a charge while a teammate attracts its metallic armor to create an opening. The same tools progress from early mistakes to solutions and a final team technique.

## Initial validation scope and decisions

Propose a first experiment with 2–4 players, 1 practice space, 1 combat room, 1 traversal room, 3 tools, 2 enemy types, 1 environmental modifier, and downing, rescue and restart. Bosses, long-term progression and many additional maps follow only after core fun is demonstrated. Test controls and recovery with placeholder models without calling the final art complete.
Try both safe use and deliberate drawbacks for every tool, and record whether players change plans after their first mistake. Compare combat and cooperation without random modifiers. Observe whether one expert does everything, protection makes tools feel unresponsive, or rescue becomes repetitive work.
If players cannot explain failures, drawbacks frustrate more than amuse, or combination use does not emerge, revise controls, telegraphs and tool effects before expanding art and content. Fun, networking quality, camera and accessibility remain unvalidated. Next review the representative art direction and core tools, then settle detailed rules.

## SE-PROT-01 — 2026-09-11

The first-prototype scope was approved and implemented as a separate Unity scene. This does not mean the complete game vision below is implemented. [SE-PROT-01](17-side-effects-implementation.en.md).
