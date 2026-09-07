# NO RETURNS — Steam release roadmap

[한국어](01-release-roadmap.ko.md)

**Prepared:** September 7, 2026  
**Account status:** Not registered, as reported by the creator.  
**Target:** Windows PC / Steam / 3D third-person / online cooperation.  
**Planning status:** Release preparation; no launch date announced.

## 1. Account setup: the first owner action

Open [Steam Direct](https://partner.steamgames.com/steamdirect) and sign in with the Steam account intended to administer the product.

Prepare the publishing entity's legal identity, matching bank-account information, and the information requested by the tax interview. The actual rights owner must accept the agreements and complete identity verification. Steam permits individual onboarding; the legal name and bank holder must match. Enter sensitive details directly in Steamworks and its verification flow. This repository should contain completion status, not identity documents or financial identifiers. [Official onboarding](https://partner.steamgames.com/doc/gettingstarted/onboarding)

| Owner input | Where it belongs | What to report back for project planning |
| --- | --- | --- |
| Individual or company publishing identity | Official onboarding | Chosen entity type and whether verification is complete |
| Legal name, address, identity verification | Official onboarding/verification | Completion status only |
| Bank details | Official payout setup | Completion status only |
| Tax interview | Official tax flow | Accepted or awaiting verification |
| Product registration payment | Official checkout | Payment date and receipt retained privately |
| Public developer/publisher name | Store metadata | Exact public spelling |
| Support email | Creator-controlled mailbox and store settings | Public support address |
| App ID and related depot/package IDs | Steamworks application dashboard | The actual non-secret IDs after creation |

The public studio name and contracting legal name serve different purposes. Do not invent a company to fill a form.

## 2. Costs and timing verified in official documentation

| Item | Current rule or planning implication |
| --- | --- |
| Steam Direct registration | USD 100 or local equivalent per product; applicable consumption taxes may be added |
| Recovery of the registration fee | Not a refund; Steam can recoup it in a payout after the product reaches USD 1,000 in Adjusted Gross Revenue |
| Payment method | Steam Wallet funds cannot pay this fee |
| Initial release waiting period | Plan for 30 days after the product fee is paid |
| Coming Soon visibility | The public Coming Soon page must be up for at least two weeks |
| Store and build review | Typically 3–5 business days each; budget at least 7 business days for each submission and potential fixes |

Sources: [Steam Direct fee](https://partner.steamgames.com/doc/gettingstarted/appfee), [onboarding](https://partner.steamgames.com/doc/gettingstarted/onboarding), and [review process](https://partner.steamgames.com/doc/store/review_process).

These waiting periods can overlap with development and one another. They are release prerequisites, not estimates of how long making the game will take. The actual earliest release is after all applicable waits, store approval, build approval, pricing readiness, and our QA gate have cleared. No calendar date can be calculated until payment and public-page dates exist.

## 3. Milestone sequence

| Gate | Work | Exit evidence |
| --- | --- | --- |
| A — Product definition | Review package interactions and first shift; agree effort cap | A reviewed design for the smallest experiment |
| B — Registration | Owner completes onboarding, payment, verification, and application setup | Accessible app dashboard and real App ID |
| C — Online experiment | Two players on separate networks; movement, pickup, shared package effects, delivery | A recorded successful session and resolved ownership errors |
| D — Replay test | Four players; complete shifts; test repeated play with new groups | Players coordinate intentionally and want another shift |
| E — Coming Soon | Final naming, actual screenshots, capsules, copy, survey, metadata; submit and publish after approval | Public page URL and first-publication timestamp |
| F — External playtest | Prepare controlled access and gather usability/network feedback | Playtest report, fixes, and a tested connection flow |
| G — Release candidate | Finish committed content, settings, English text, notices, assets, pricing, and uploaded build | Candidate build ID, QA results, and truthful store claims |
| H — Steam review | Submit store presence before build review; submit a nearly final build | Both reviews approved and applicable waits cleared |
| I — Launch | Final smoke test, release action, public availability verification, support readiness | Store purchase availability and successful customer-like installation |

Gates A and B can progress in parallel. Registration is useful before the connection test and starts an external waiting period, but payment is the owner's action. Coming Soon materials require a representative build; conceptual mockups do not replace real screenshots.

The initial commercial proposal is a one-time purchase. Price, discount, paid-release content volume, and launch date remain creator decisions informed by playtests. Avoid an ongoing-service commitment while proving a small game.

## 4. Proposed effort control

Use a **40 focused developer-hour checkpoint** for the first online experiment. This is a proposed spending-of-time limit, not a delivery estimate or a promised finished game.

| Investigation/work | Proposed hours |
| --- | ---: |
| Steam/Godot compatibility and two-machine connection experiment | 12 |
| Third-person movement, carry, throw, and basic camera | 8 |
| Sneezer, Clinger, and bounded interactions | 10 |
| Depot, quota, timer, recovery, and restart | 4 |
| External playtest, observed fixes, and checkpoint report | 6 |
| **Total** | **40** |

Re-estimate after the connection experiment if implementation experience or tool compatibility makes this unrealistic. At the checkpoint, choose among continuing, simplifying the design, or stopping the experiment. Do not turn the checkpoint into a launch promise.

Initial paid production scope should stay near one depot, a few authored layout arrangements, three special packages plus standard cargo, and replayable shifts. This is a ceiling to evaluate, not a public content promise. More content follows evidence that the existing rules support replay.

## 5. Cash plan

- **Known platform outlay:** the registration fee and any tax shown at checkout.
- **Networking direction:** player-hosted Steam sessions, to avoid operating our own always-on game servers. Verify integration and service conditions before budgeting ongoing costs as zero.
- **Art/audio/font purchases:** deferred; maintain a license record before using assets commercially.
- **Localization:** English source text first. Additional languages need actual translation and QA effort.
- **Marketing:** begin with gameplay clips, a store page, and small creator playtests. No paid advertising budget is assumed.
- **Other variable costs:** test hardware/access, support arrangements, professional services, and any services added later. No all-in cost has been quoted.

## 6. Visibility and testing plan

Use a small closed group first. Steam Playtest is a later option for controlled external testing through an associated child app; it has its own setup and review. A public demo is a separate product decision. Neither is inherently required for selling the base game. [Steam Playtest](https://partner.steamgames.com/doc/features/playtest)

After a public page exists, capture a few short gameplay sequences that show a readable setup, a mistake, and a recovery. Invite creators who already play cooperative party games; prepare invitations and support information before contacting anyone. Track store visits and wishlists to compare messaging, without promising sales from those numbers.

Evaluate events such as Steam Next Fest only when a representative demo is feasible. Check the current event's eligibility and deadlines then; no event slot or date is assumed in this roadmap.

## 7. Publishing controls

Steam approval does not automatically launch the game. An authorized Steamworks user must deliberately release it. Preserve a final preflight record of the intended build, price, supported platforms, and public store contents. [Release process](https://partner.steamgames.com/doc/store/releasing)

Prepared locally now: the design, roadmap, copy, production briefs, technical validation plan, and release checklists. Account verification, fee payment, artwork, playable content, store submissions, and release execution remain actual work to complete.
