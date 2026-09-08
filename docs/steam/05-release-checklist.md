# NO RETURNS — Release readiness checklist

[한국어](05-release-checklist.ko.md)

**Status date:** September 8, 2026  
**Owners:** Creator = account holder/publishing decision maker; Development = the future implementation and production work.  
**How to use:** Check an item only when its evidence exists. A prepared plan is not completion of the underlying release requirement.

## 1. Prepared locally

- [x] Paired Korean/English concept design and confirmed camera/platform direction recorded.
- [x] Steam onboarding, timing, fee, and review requirements researched from official sources.
- [x] Store copy drafted with a Korean counterpart for review.
- [x] Asset sizes, screenshot assignments, and trailer storyboard prepared.
- [x] Multiplayer integration and build-validation plan prepared.
- [x] Creator/playtest drafts and feedback prompts prepared.
- [x] Godot 4.7.2 prototype 0.5 with four cargo types, A/B destinations, quotas, pings, overtime and local two-/four-process ENet verification; separate from Steam/other-PC validation.
- [x] Windows executable/ZIP, Korean/English play guides, third-party notices and preview-only SteamPipe generator prepared. See [build handoff](07-build-handoff.en.md).

## 2. Account and application — Creator

- [ ] Publishing entity and actual rights owner confirmed.
- [ ] Account access secured and the required Steam agreements accepted.
- [ ] Steam Direct fee paid; payment date recorded privately for release scheduling.
- [ ] Identity, banking, and tax onboarding accepted.
- [ ] Real App ID, depot IDs, and packages created and recorded.
- [ ] Public developer/publisher name and support contact selected.
- [ ] Account permissions for builds, metadata, pricing, and publication are available to the responsible people.

Evidence: Steamworks status and real application identifiers. Retain sensitive onboarding documents outside the repository. Follow the [onboarding requirements](https://partner.steamgames.com/doc/gettingstarted/onboarding).

## 3. Product, rights, and content — Creator + Development

- [ ] Final product name reviewed for conflicting game branding and the intended markets; NO RETURNS remains a working title until then.
- [ ] All shipped code, art, fonts, sounds, music, and marketing materials have adequate commercial-use rights recorded.
- [ ] Godot and relevant third-party notices are included in the actual distributed game and accessible to players.
- [ ] The Steam Content Survey accurately reflects the final content, including any relevant violence, mature material, and AI-assisted player-consumed content.
- [ ] Regional rating/availability checks are resolved for the intended markets; do not assume a global release target creates permission to sell everywhere.
- [ ] Any applicable additional local publishing or rating requirements are identified and resolved before enabling those territories.
- [ ] Actual handling of Steam identifiers, diagnostics, crash reports, support submissions, and any analytics is documented; required player notices match that behavior.
- [ ] Additional EULA/privacy text is prepared if needed for the final data flow and publishing setup; no unrelated template is treated as finalized legal text.

Godot distribution requires its license notice and applicable third-party notices. Include the notices corresponding to the engine and dependencies actually shipped. [Godot license compliance](https://docs.godotengine.org/en/stable/about/complying_with_licenses.html)

Steam's survey covers general content, mature content, and generative AI. Its AI section focuses on content shipped and consumed by players, including art, sound, narrative, and localization. Development-tool efficiency alone is not the same question. The final answers must follow what actually ships. [Content Survey](https://partner.steamgames.com/doc/gettingstarted/contentsurvey)

For example, Steam documents a rating requirement for Germany and a rating workflow for Indonesia. Complete the survey and verify the actual app's regional status; the survey is not a blanket guarantee of every territory's availability. [Germany](https://partner.steamgames.com/doc/gettingstarted/contentsurvey/germany), [Indonesia](https://partner.steamgames.com/doc/gettingstarted/contentsurvey/indonesia)

### Rights and provenance register

Create one record for every external asset or dependency as it is selected. Each record needs its source, version, author/provider, license, commercial rights, attribution requirement, purchase proof if any, intended use, and distribution status.

| Current item | Provenance / state | Required action before shipping |
| --- | --- | --- |
| Korean/English design, copy and prototype UI | AI-assisted; some text is used in the local prototype, before public distribution | Review retained UI/narrative/localization text and answer the content survey accurately |
| Godot Engine | 4.7.2 standard Windows x64 portable runtime prepared; official checksum verified | Include version-matched engine and third-party notices in the distribution |
| Steam integration | Candidate only; exact implementation not selected | Verify license, redistribution terms, versions, and runtime files |
| Visual art / fonts / audio | 0.9.0 integrates Higgsfield GPT Image 2 surfaces, Image to 3D worker/rat assets, and Blender-authored parcels/facilities. Generation sources and IDs are recorded in `art/release-01`. Synthesized effects and device system fonts remain | AI-generated art is now player-consumed content. Use the [art ledger](../art/02-release.en.md) to review retained assets, provider terms and commercial usage rights and reflect them in the content survey. Do not bundle system font files |

## 4. Store and Coming Soon — Creator + Development

- [ ] Final copy matches the implemented features and supported player count.
- [ ] Required store, library, and client images exist and have been checked in Steam previews.
- [ ] At least five real gameplay screenshots are uploaded.
- [ ] A representative gameplay trailer is prepared as our marketing goal; any uploaded trailer has finished processing.
- [ ] Supported platforms, languages, accessibility options, input devices, and feature flags match test evidence.
- [ ] Minimum and recommended specifications are based on measured exports.
- [ ] Public support information works.
- [ ] Pricing proposal and regional prices are reviewed by the creator; any launch discount is deliberate.
- [ ] Store presence is submitted for review and feedback is resolved.
- [ ] Approved Coming Soon page is published and its first-publication time recorded.

Pricing is chosen by the publisher and submitted through Steam's tools for review. Use the current regional-pricing tools and check their output before submission. The project has no final price yet. [Pricing](https://partner.steamgames.com/doc/store/pricing)

## 5. Playable build — Development

- [ ] A release export installs and launches through Steam on clean supported Windows systems.
- [ ] Native integration DLLs and game data are present; development App ID overrides and private material are absent.
- [ ] Friends on separate networks can join, finish a shift, return to the lobby, and replay.
- [ ] Two-, three-, and four-player sessions have been tested.
- [ ] Simultaneous pickups, attachments, catches, and dispatches do not duplicate or strand objects.
- [ ] Disconnects, host departure, version mismatch, full parties, and Steam startup failures have useful recovery paths.
- [ ] Camera collision, aiming, sensitivity, reduced shake, readable text, and audio levels work.
- [ ] The game remains understandable with important sounds muted and without relying on color alone.
- [ ] Local settings persist and the game handles missing/corrupted settings gracefully.
- [ ] A sustained session and repeated-shift test do not reveal accumulating state or performance failures.
- [ ] Store claims have been checked against this exact candidate build.
- [ ] SteamPipe upload, depot entitlement, executable launch settings, and branch assignment are correct.
- [ ] The candidate has a recorded build ID, source revision, test results, and a known rollback build.

The detailed test conditions are in the [online and build plan](04-online-and-build-plan.md). Local rules and two-/four-process checks for [prototype 0.4](../prototype/04-four-players.en.md) pass, but the Steam release-candidate conditions above remain unmet.

## 6. Review and launch — Creator + Development

- [ ] Store presence was submitted before the build-review submission.
- [ ] The nearly final build is on the required review branch and Valve can exercise the online feature using supplied reviewer instructions/access.
- [ ] Store and build reviews are both approved; requested fixes have been retested.
- [ ] The app-fee waiting period and Coming Soon visibility period have cleared.
- [ ] Pricing and intended regional availability are ready.
- [ ] Trailer conversion and store publishing tasks are complete.
- [ ] Support and issue-reporting paths are ready, and the creator has time to respond after launch.
- [ ] A final Steam-installed smoke test passes on the intended release candidate.
- [ ] The intended build, price, store content, and release moment are confirmed by the publishing owner.
- [ ] An authorized operator uses Steam's release controls and verifies the public result.
- [ ] A customer-like account can obtain the correct package, install, launch, and enter an online session.

Store/build review and the release action are separate steps. Allow review lead time, and retain the actual approval evidence. [Review process](https://partner.steamgames.com/doc/store/review_process), [release process](https://partner.steamgames.com/doc/store/releasing)

## 7. Launch-day operations

Before release, retain the last working build and write a short known-issues list. After release, check installation reports, startup failures, invitations, and connection problems first. Reproduce each critical report against the released build and record a fix or workaround.

Test a hotfix on a private branch before promoting it. If a new build breaks startup or core multiplayer, use the documented previous working build while preparing a fix. Publish concise patch notes describing actual changes. No automatic deployment or message-sending system has been set up by this preparation.

## 8. Items currently awaiting real inputs or production

| Item | Current state | Next evidence needed |
| --- | --- | --- |
| Steamworks account and App ID | Not registered | Creator completes official onboarding |
| Publishing name and support contact | Not provided | Creator selects real public details |
| Development availability and effort cap | Not confirmed | Creator supplies available hours; agree prototype scope |
| Godot/Steam version pairing | Not tested | Maintainer release check and two-machine test |
| Gameplay and commercial scope | Proposed | External playtest results |
| Final art, screenshots, trailer | Deferred / not produced | Representative build and later art production |
| Ratings, rights, data notices | Cannot finalize from a concept | Final shipped content and data inventory |
| Price and launch date | Not chosen | Scope, measured readiness, and owner decision |
| Steam approval and publication | Not submitted | App access, actual artifacts, review, and launch execution |


## 0.7 development build addendum

- [x] Implemented three-contract campaign, shared upgrades, relay and Packrat; passed local behavior/multiprocess checks.
- [x] Prepared complete paired 33-section design and 0.7 play guides.
- [x] Verified Windows export, 22 packed gameplay scripts, isolated-folder launch and rendering.
- [ ] Steam invites/relay and separate-PC/external-network validation.
- [ ] Independent newcomer/friend-group observation and target-hardware performance.

Evidence and limitations are in the [0.7 implementation ledger](../superpowers/plans/2026-09-07-release-polish.en.md). This addendum does not change the unfinished account, rights, store and review gates above.

## 0.7.9 Release usability audit

- [x] Implemented pre-play settings, rebinding/current-binding prompts, volume/sensitivity/FOV/display controls.
- [x] Solo time/physics pause, online input blocking and focus-loss/return handling.
- [x] Leave/quit confirmation, bilingual layout/keyboard focus and preservation of settings during contract/overtime transitions.
- [x] Admission timeout, completed-record write failure feedback/atomic replacement/retry and damaged numeric preference fallback.
- [ ] Steam invitations/relay and online validation with Steam-installed builds on separate PCs.
- [ ] Target-spec performance/long sessions and observation of external newcomers and real friend groups.
- [ ] Final art/audio/localization/rights/store assets and Valve review.

Release assessment remains **incomplete**. This playtest fixes technical defects; automated checks cannot certify commercial quality. [Changes and evidence](../superpowers/plans/2026-09-08-release-usability.en.md). Official review criteria were rechecked on 2026-09-08. Store claims must still match shipping features, and store/build review remain required. [Steam review process](https://partner.steamgames.com/doc/store/review_process).
