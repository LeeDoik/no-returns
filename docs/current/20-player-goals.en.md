# NO RETURNS — Exploring Stronger Player Goals

[한국어](20-player-goals.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


2026-09-12 · Revision 3 · Rising parcel value and reinvestment selected / detailed economy proposed / not implemented

## Latest decision — Progress toward more valuable parcels

The user selected How to Fish as an economic reference: start with inexpensive parcels and progressively handle parcels with higher delivery payouts. Increasing cargo value is the main progression axis; volume and efficiency tools such as trolleys support it. The income-target discussion below provides background, not approval of mandatory quotas, payments or deadlines.

Reference: [Mobalytics How to Fish money guide](https://mobalytics.gg/gamebase/guides/how-to-fish-how-to-make-money), updated 2026-08-27, accessed 2026-09-12. It describes better lures enabling more valuable catches, reinvesting earnings in lures, and inventory expansion improving hauling efficiency. This is public-guide research, not hands-on analysis or reproduction of the entire reference economy.

Adaptation proposal: deliver inexpensive parcels → save for the next handling equipment → purchase it → receive higher-paying parcel contracts → solve new handling problems → earn more → pursue the next equipment. Prefer presenting the next equipment purchase cost as the immediate monetary goal. Whether to impose a separate mandatory shift quota remains undecided.

Unlock comparison: buying company tiers is clear but can feel like a numerical toll. Opening regions motivates exploration but expands map-production scope. Handling equipment connects spending to new delivery actions, so it is the recommended starting point. Equipment would both unlock a contract family and be used during hauling; avoid charging separately for a permit and mandatory equipment. The exact unlock mechanism remains proposed.

Progression example: small ordinary parcels → heavy cargo handled with a reinforced trolley → impact-sensitive cargo handled with a cushioned cradle. Names, order and equipment are examples, not approval to produce new cargo. Expensive cargo should change loading, orientation, handoffs and recovery decisions rather than merely lengthening walks. Retain light physical reactions on starter parcels so core fun appears before the first purchase.

Illustrative values: initial payout 30, next equipment cost 120, unlocked payout 90. With starting balance 0, no additional costs or bonuses and every delivery successful, 4 deliveries fund the purchase. These explain the relationship and do not replace the existing equipment-price proposals with finalized balance. If higher-paying deliveries take much longer, progression can lose its benefit; measure actual income per unit of time including delivery and recovery.

Randomness proposal: vary destinations, behaviors and optional valuable orders within currently handleable tiers. Basic orders must fund the next step without requiring a rare order to appear. Retain inexpensive orders as a safe financial recovery option. Test whether repeating low-tier orders remains the most efficient late-game strategy.

After initial handling validation, the minimum growth experiment connects inexpensive and valuable contract families with one unlocking tool. Observe understanding of the next purchase goal, perceived changes in cargo and income afterward, and changed cooperative decisions. Total tiers, final goal, equipment loss, failure penalties and deadlines remain undecided. No remote Manyfast changes, economy implementation or human play validation were performed.

## Area expansion versus repeated-map deliveries — 2026-09-12 / unadopted proposal

The user requested a comparison between gradual area expansion and different deliveries on the same map. Recommend varying deliveries within one town and opening adjacent areas at major progression milestones. If choosing only one, area expansion better communicates the larger goal of reaching higher-paying cargo; this is a design judgment, not a playtest finding or confirmed user decision.

- Area expansion: a visible next destination can motivate saving and provide recognizable stages for a stream. Each area can change handling problems, but adds map-production effort, navigation burden and longer return trips.
- Different deliveries on the same map: supports shortcut mastery and comparison of equipment benefits with a smaller production scope. Changing only parcel prices risks repetitive labor. Cargo behavior, acceptance conditions and route choices must actually change.
- Recommended combination: experience new tools and valuable deliveries in a familiar area before expanding into an adjacent one. Do not require area unlocks for every purchase. New areas offer higher payouts and different handling decisions while earlier orders remain available.

Example: small deliveries around the plaza and alley → handling equipment and higher-paying deliveries in the same area → an adjacent service area with new route problems. Area names, counts and unlock conditions remain open. Initially avoid charging both area admission and mandatory equipment costs. Use looped shortcuts so expansion does not continually increase trips back to the existing truck. This is not simultaneous production of multiple maps or unlimited expansion.

First validate inexpensive versus valuable deliveries within a small existing area. Then observe whether adding an adjacent area changes next-goal comprehension, perceived growth, repetition fatigue, and travel/recovery time. This task adds comparison documentation only; no Manyfast edits, code changes or game tests.

## Current decision — Income targets and reinvestment

The user chose meeting a monetary target and spending earnings to earn more. The main goals are revenue achievement and growing the company's earning capability. The revision 1 recommendation of regional signature deliveries below is historical comparison, not the adopted direction.

Recommended loop: inspect income target → choose contracts/loadout → earn through deliveries → meet target and choose extra earnings or return → reinvest in equipment → attempt more profitable contracts. Signature-delivery stamps are not the primary progression requirement.

Economy proposals:

- Separate this shift's cumulative income, the company's spendable balance and the next investment cost. Buying equipment does not revoke a completed income target. Old savings do not automatically satisfy a new shift's target.
- Trolleys increase cargo per trip, straps reduce recovery/reloading time, and ramps expand route options. Higher earnings result from more efficient completion or solving more demanding contracts with equipment. Do not build progression solely from income multipliers.
- Investments expand earning opportunities rather than guaranteeing income. Trolleys remain awkward in narrow passages and straps do not prevent sneeze blasts, preserving cargo/route-dependent choices.
- Propose revealing higher targets together with more profitable contracts at the next tier and letting the team choose when to attempt it. Do not automatically raise targets after purchases and erase the benefit of growth. Retain current-tier repeats and basic contracts achievable with free equipment.
- Missing-target penalties, deadlines and failure progression remain undecided. Start from existing preservation of confirmed delivery earnings; do not infer bankruptcy, permanent debt or forced resets from this choice.

Validate actual earnings and handling/recovery time before and after investment, and whether the first and subsequent purchases offer distinct choices. Check dominant equipment, saving instead of buying, and remaining contract/loadout choices after all unlocks. Tune targets, prices and tier count after measuring play time and completion rates. This task records direction; economy implementation, play validation and Manyfast revision have not been performed.

## PRD improvement review — 2026-09-12

Read the current Manyfast PRD in the browser. It displays 85% completion and empty user-role and device attributes; the percentage calculation was not verified. Its revision 3 loop still emphasizes contract completion and needs the income/reinvestment decision above. The chat reports 6 requirements and 11 features with approval guidance, but individual features and approval status were not inspected. This review records proposals without remote edits or code changes.

Priority decisions to develop:

- Success/end conditions: income accounting period, target rewards, returning below target, deadlines and failure handling. First reconcile the existing 3-contract ending with the new monetary target.
- Economy: distinguish shift income, balance and target; specify payment, purchase and save timing. Decide whether meeting a target consumes money. Available contract rewards must permit reaching the target.
- Growth: explain which delivery methods and earning opportunities the first and subsequent purchases open, contracts achievable without equipment, and long-term goals after buying every tool. Review prices, rewards and expected purchase timing together.
- Cooperation: decide who confirms shared spending, contract selection and return, what guests retain, and how departures work. Distinguish host simulation authority from team decision rights.
- Randomness: connect pre-departure information, on-site discoveries, incident warnings, recovery and rewards. Check changed team choices rather than incident counts.
- Experience/validation: write a representative scenario from departure through settlement, purchase and the next shift. Observe goal comprehension, time to first purchase, earnings/recovery time before and after investment, and reasons to replay. Separate automated checks from fun validation.

Suggested Manyfast placement: update product goals and user scenarios with the growth loop; group economy, ending and cooperation rules under the solution. Put observation methods in metrics and unresolved decisions in risks. Propose Player as the role, distinguishing host/participant rights, and PC as the device. Check feature consistency after PRD decisions. Retain the first handling validation, then add a small income/purchase/next-shift growth experiment. Deadlines and penalties remain undecided.

## Historical revision 1 comparison

Source: [current concept](19-no-returns-manyfast.en.md). This is a discussion document and does not change current contract or return rules.

## Diagnosis

The concept provides destinations and equipment purchases, but a major achievement for this shift and an ultimate destination remain weak. This is a design assessment of the document, not player research. Connect a visible endpoint, tangible progress toward it and a specific reason to retry rather than only emphasizing goal markers.

## Comparison

| Direction | Goal players can express | Benefit | Risk |
|---|---|---|---|
| Company quota | Meet this shift's income target | Clear numbers and success conditions | Repeating cheap contracts and optimization labor; fines conflict with retained achievement |
| Regional signature delivery | Complete preparations and deliver this area's final major order | A memorable beginning, challenge and conclusion for streams | Content cost and repetitive solved routes |
| Company equipment growth | Save for the trolley and truck setup we want | A reason for another shift and meaningful choices | Goals expire after purchases; buying may substitute for core fun |

Recommend signature deliveries as the main goal and equipment configuration as support. Consider quotas as optional challenges without fines or permanent debt. Do not expand into city management or resident reputation.

## Recommended structure

Immediate: know where and how to place the parcel currently being handled.

This shift: reveal the signature order and completion condition before departure. Preparatory deliveries contribute actual prerequisites for releasing that order. Show progress on the order board and through facility changes. Players may return after preparation while keeping earned rewards. The signature delivery grants a regional completion record and full success.

Long term: collect regional signature-delivery stamps to reach a final delivery. Keep stamps separate from currency so equipment purchases cannot reverse progress. Decide region count and final-order scope after validating the first signature delivery. Determine whether preparations must repeat every time from observed repetition fatigue.

Equipment changes how goals are achieved. Do not require repeated contracts to purchase mandatory equipment. Provide both solo safe routes and cooperative shortcuts.

## Example — The Festival's Last Delivery

Show the hilltop festival venue and its large living gift parcel from the start. The order board says, ‘Deliver the final gift to the festival.’ Earlier deliveries complete dispatch preparation and prepare the receiving facility, enabling final transport. Each preparation order pays its own delivery reward.

The gift has a readable destination and is movable; its size creates a choice between a broad detour and a narrow shortcut. Solo players use basic mechanisms and the safe route, while teammates speed up route clearing, handoffs and recovery. Falls preserve the parcel and preparation progress for recovery and retry. Arrival changes venue lighting and the order board and awards the regional stamp. No combat boss or extensive cutscenes are necessary.

This illustrates a signature delivery; it does not approve a festival theme or new parcel production. Handling difficulty and arrival presentation still require validation.

## Difference from the current concept

This changes the optional extra order into a clear signature-delivery climax. Adoption would distinguish returning after basic deliveries as partial success from returning after the signature delivery as regional completion. Retain existing earned rewards. Since this changes the current statement that declining extras still constitutes normal completion, do not overwrite the active Manyfast PRD before agreement.

## Minimum validation and next decision

Compare ordinary-contract and signature-delivery endings in the same small map. Observe whether players can state today's objective after departure, identify remaining preparation during play and describe their next goal after settlement. Check whether the finale is interesting or merely a heavier final chore. No measurements, code changes or fun validation exist.

Next decision: choose the main motivation among income quotas, memorable regional signature deliveries and company equipment growth.

## Progression visualization — 2026-09-12

Concept image (`../../artifacts/concepts/no-returns-progression-v1.png`; retired file). At the user's request, generated a town overview and delivery scenes showing inexpensive deliveries → equipment investment and valuable deliveries → adjacent-area expansion. Inspected the main stage labels and visual sequence. Architecture, character proportions, decorative copy and conveyor forms are visual proposals, not approved production art or an actual game screenshot. Detailed area-unlock conditions and economy values remain open. No Manyfast upload, code changes or game tests.


## Comedy direction correction — 2026-09-12

The user explicitly wants a more absurd comedic game. The previous wholesome town image did not adequately express the desired tone. The revised concept (`../../artifacts/concepts/no-returns-slapstick-v2.png`; retired file) depicts deadpan workers, a shabby delivery setting, oversized sneezing cargo and physical mishaps. Retain the selected higher-paying cargo and reinvestment direction. Propose equipment making earlier work easier while choosing higher payouts introduces new incidents to anticipate and recover from. Do not impose constant loss of control or erase growth through random penalties. The giant walking cargo, scanner and realistic setting are visual examples, not production approvals. Inspected main labels and the incident scene; actual play enjoyment, physics and art performance remain unverified. No remote edits or game code changes.

