# Steam private testing registration

[한국어](steam-testing.ko.md)

2026-09-13 · Registration preparation. Observed the login dialog at the Steamworks signup entry point. Partner approval, an owned NO RETURNS AppID, payment, build distribution and test keys remain unverified. Opening the login dialog does not complete registration.

## Objective and first step

Grant friends initial access, then repeatedly update test builds of the same app. First sign in to [Steamworks](https://partner.steamgames.com/newpartner) with the account that will manage the game. Check existing partner and app ownership before duplicate registration or purchase.

New onboarding requires legal identity, banking and tax information, identity verification and agreements. Individuals can onboard; the registered name must match the bank account holder. The account owner enters personal information and reviews agreements on the official site. Do not store banking details, tax identifiers, passwords or authentication codes in documents or Git. [Onboarding](https://partner.steamgames.com/doc/gettingstarted/onboarding)

The app fee is USD 100 or the local equivalent; check applicable taxes and the actual charge at checkout. No payment was made in this task. [Fee guidance](https://partner.steamgames.com/doc/gettingstarted/appfee)

## Progress checklist

- [x] Review the preceding session and official onboarding/testing guidance.
- [x] Open the signup login dialog.
- [ ] Sign in and inspect existing partner/app status.
- [ ] Confirm completion of required onboarding, agreements, payment, identity, banking and tax steps.
- [ ] Obtain and record the owned NO RETURNS AppID.
- [ ] Configure the Windows depot and launch option; verify app/depot inclusion in testing account packages.
- [ ] Upload a test build and apply it to a restricted test branch.
- [ ] Verify installation and launch through Steam on a developer account.
- [ ] Request Release State Override keys for small external testing; verify approval and access.
- [ ] Verify installation, launch and a new build update on another account/PC.
- [ ] Implement Steam invitations/joining and verify across separate accounts/networks.

The initial external-test candidate uses the main AppID with prerelease test keys. Key issuance is not guaranteed before approval. A branch password or lobby invitation alone does not grant app access. Consider a separate Playtest app later for larger recruitment. [Official testing guidance](https://partner.steamgames.com/doc/store/testing)

## Acceptance and separate development scope

Distribution checks cover Steam installation, launch, updates and access. Cooperative checks cover invitation, joining, delivery, ship-return saving and resuming after everyone exits. Registration alone does not implement cooperative networking or operator servers. Follow the [backlog](04-backlog.en.md) for the current server recommendation and open decisions. No gameplay code changes or actual Steam distribution/invitation tests yet.
