# NO RETURNS — Online and Steam build plan

[한국어](04-online-and-build-plan.ko.md)

**Status:** Godot 4.7.2 expanded prototype 0.5 includes four cargo types, crew quotas, A/B destinations, pings and overtime. Local two-/four-process ENet tests and a Windows export are verified. Steam transport, real App ID and Steam installation remain unconfigured. See the [current play guide](../prototype/05-expanded.en.md) and [build handoff](07-build-handoff.en.md).  
**Updated:** September 7, 2026

## 1. Proposed connection design

Use a private Steam lobby for invitations and membership. Use modern Steam P2P networking for game messages. One player hosts the simulation and final decisions; the other players connect to that host.

Steam's lobby service and networking transport are separate capabilities. Valve recommends lobby/P2P arrangements for small parties, and its newer networking APIs can relay traffic through Valve's network. This is a suitable direction for our small cooperative game, subject to implementation tests. [Steam multiplayer](https://partner.steamgames.com/doc/features/multiplayer), [Steam networking](https://partner.steamgames.com/doc/features/multiplayer/networking)

The design avoids an always-running game server operated by us. It does not imply that every optional service or integration is free, or that long-distance latency disappears. Measure the selected path before making operating-cost or connectivity promises.

## 2. Godot integration decision

Candidate: GodotSteam with a compatible Steam-backed MultiplayerPeer implementation, using a modern SteamNetworkingSockets-based route. Choose one supported integration path and pin the full version combination before development expands.

The former [GodotSteam GitHub repository](https://github.com/GodotSteam/GodotSteam) currently redirects maintainers' work to [Codeberg](https://codeberg.org/godotsteam/godotsteam). Its moved status was verified; the current release compatibility matrix was not verified in this preparation. Read the maintainer's current release instructions before choosing versions. Another implementation reference is [Expresso Bits' Steam Multiplayer Peer](https://github.com/expressobits/steam-multiplayer-peer); its compatibility with our eventual engine version is also untested.

Record the selected Godot version, export-template version, integration source/version, Steamworks SDK requirements, Windows architecture, dependency licenses, and a two-machine test result. Avoid combining tutorials from different integration generations without checking the actual API.

Selling a game on Steam does not inherently require Steamworks API integration. Our proposal uses it to deliver the selected friend-join and networking experience. [Steamworks API overview](https://partner.steamgames.com/doc/sdk/api)

## 3. Player flow

1. Launch through Steam and initialize the integration.
2. Host creates a private lobby; invitees join through Steam.
3. Verify compatible game/protocol versions and available slots.
4. Players ready up; the host starts the shift.
5. Lock new joins for the active shift in the first version. Joining a departed player's spot happens back in the lobby.
6. Finish together, return to the lobby, and replay without recreating the party.

If a guest disconnects, release their held cargo into a recoverable state and allow the remaining team to end or restart the shift. If fewer than two remain, return to the lobby with an explanation. If the host exits, return guests to the main menu with a clear message. Host migration and mid-shift reconnect are deferred; these limits must be clear in the interface and playtest instructions.

## 4. Shared-state responsibilities

| System | Host responsibility | Client presentation |
| --- | --- | --- |
| Player movement | Resolve authoritative interaction positions and bounded knockback | Responsive local movement with reconciliation |
| Carrying | Decide one holder per package and validate pickup distance | Stable carry pose and clear pickup success/failure |
| Throws/catches | Confirm release and catch outcomes | Aim preview and smooth package movement |
| Adhesion | Choose one valid attachment and its expiry | Visible link, timer cues, and clean detachment |
| Sneezes/jumps | Schedule consistent windups and apply effects | Visible anticipation and locally played effects |
| Shipments | Spawn controlled queues, process correct dispatch once, recover lost cargo | Shared labels, progress, and recovery feedback |
| Shift results | Own timer, quota, and success state | Display the confirmed team result |

Prefer stable gameplay states and bounded impulses for important interactions. Avoid requiring deterministic rigid-body simulation on every machine. Limit active packages, attachments, and effect frequency. The visual reaction may be exaggerated while the gameplay state stays simple.

Validate messages against lobby membership, game phase, object existence, distance, and ownership. Reject stale or duplicate delivery actions. These checks preserve the game rules when packets arrive late or players act simultaneously.

## 5. Connection experiment

Before building the full depot, demonstrate two distinct Steam accounts on two PCs and separate internet connections creating a party, moving, competing to pick up one object, throwing it, disconnecting, and restoring a usable lobby. Later repeat with four players.

Testing multiple windows on one computer is useful during development but does not validate account ownership, invitations, home-network traversal, or real multiplayer latency.

Use the actual application and appropriate Steam test access once available. Any sample App ID used for a development-only experiment must be isolated from release configuration. Do not publish or distribute the game under an unrelated App ID.

## 6. Multiplayer QA matrix

All cases below are **untested**. These are proposed test conditions, not supported-latency claims.

| Test | Expected outcome |
| --- | --- |
| Two players / separate networks | Invite, join, play, finish, and replay successfully |
| Four players / separate networks | All players see matching shipment and attachment outcomes |
| Simultaneous pickup | Exactly one player receives ownership |
| Pickup during sneeze or detachment | No duplicate object, permanent tether, or lost input state |
| Simultaneous delivery attempts | Quota increments once |
| Guest leaves while holding cargo | Cargo becomes recoverable; session flow remains usable |
| Host leaves or crashes | Guests receive a clear disconnect message and can start again |
| Incompatible versions / full lobby | Join is rejected with a useful reason |
| Steam unavailable at startup | Clear error and retry path; no frozen loading screen |
| Simulated RTT 50/150/250 ms and packet loss 0/1/2% | Record delayed actions, cue readability, ownership corrections, and playability; revise design for poor results |
| A real inter-region session | Record participant regions, observed latency, and actual usability |
| Repeat shifts and lobby reuse | No growing cargo count, leaked attachments, or broken ready state |

Prioritize a clear windup and forgiving catch window over frame-perfect interactions. Specify the final tolerances after measuring the networked prototype.

## 7. Windows export and SteamPipe preparation

Once a project and real App ID exist:

1. Produce a Windows release export with a supported Godot/export-template pair and matching native dependencies.
2. Assemble a clean distribution directory containing the executable, required game data, Steam integration runtime files, and third-party notices.
3. Exclude editor caches, source credentials, private logs, development executables, and development-only App ID overrides.
4. Configure the real application, Windows depot, packages, and executable launch option in Steamworks.
5. Prepare SteamPipe build/depot configuration using those real IDs and actual export paths.
6. Upload with a permitted Steamworks account, record the build ID, and assign it to a private test branch.
7. Install through Steam using a tester entitled to the app/depot; launch and complete an online shift on a clean machine.
8. Place the nearly final candidate on the branch required by Steam's review checklist. Re-test that exact installed candidate before submitting it.

Valve documents SteamPipe upload and branch testing in [Uploading to Steam](https://partner.steamgames.com/doc/sdk/uploading) and [Builds](https://partner.steamgames.com/doc/store/application/builds).

For a Windows build using the Steam API, the required Steam runtime DLL must be in the expected runtime location. A development `steam_appid.txt` override must not be shipped with the client build; Steam supplies the application identity when launching it. [Steamworks API overview](https://partner.steamgames.com/doc/sdk/api)

## 8. Release evidence to retain

For every candidate, record the source revision, engine and integration versions, export profile, build timestamp, Steam build/depot IDs, branch, testers, Windows versions, hardware, network conditions, known issues, and previous working build ID.

Measure minimum requirements on the build we intend to sell. Do not infer support from an editor run. Achievements, Cloud, controller support, Steam Deck claims, and additional operating systems remain optional features that need their own implementation and evidence.

## 9. Small-scope default

Start with private parties and external voice chat. Use local settings and no separate game account. No persistent online progression, public matchmaking backend, live AI service, paid database, or dedicated-server fleet is assumed. Revisit only if the playtests identify a concrete need.
