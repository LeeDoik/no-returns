# NO RETURNS — Steam asset and trailer brief

[한국어](03-assets-and-trailer.ko.md)

**Status:** Production specifications only; artwork and footage have not been produced.  
**Checked against official documentation:** September 7, 2026. Recheck the current Steamworks templates at export time.

## 1. Required store images

| Asset | Delivery dimensions | Proposed filename |
| --- | --- | --- |
| Header capsule | 920 × 430 | store_header.png |
| Small capsule | 462 × 174 | store_small.png |
| Main capsule | 1232 × 706 | store_main.png |
| Vertical capsule | 748 × 896 | store_vertical.png |
| Gameplay screenshots | At least 5; 1920 × 1080 or larger, 16:9 | screenshot_01.png through screenshot_05.png |

These are the current enlarged capsule sizes. Use game artwork and the title in standard capsules; keep ratings, discounts, review quotes, and promotional sentences out of the base artwork. Screenshots must show the game itself. [Asset overview](https://partner.steamgames.com/doc/store/assets), [screenshot specifications](https://partner.steamgames.com/doc/store/assets/standard), [capsule rules](https://partner.steamgames.com/doc/store/assets/rules)

## 2. Library and client images

| Asset | Delivery dimensions / format | Proposed filename |
| --- | --- | --- |
| Library capsule | 600 × 900 | library_capsule.png |
| Library header | 920 × 430 | library_header.png |
| Library hero | 3840 × 1240, PNG; artwork without text | library_hero.png |
| Library logo | Transparent PNG; width 1280 and/or height 720, preserving the logo's aspect ratio | library_logo.png |
| Shortcut icon | 256 × 256, ICO or PNG | shortcut_icon.png |
| App icon | 184 × 184, JPG | app_icon.jpg |

Compose the hero and its separate logo using Steam's current template and preview tools. Verify cropping and title legibility in compact library views. [Library assets](https://partner.steamgames.com/doc/store/assets/libraryassets), [client icons](https://partner.steamgames.com/doc/store/assets/community)

For a Steam announcement, plan an 800 × 450 cover image. A 1920 × 622 event header and a 1438 × 810 store background are optional. These are additional marketing assets, not extra gameplay requirements. [Asset overview](https://partner.steamgames.com/doc/store/assets)

## 3. Key-art direction for later production

Show a coworker trying to hold a sneezing package while another worker reaches for a delivery about to escape. Make the packages expressive and the shared job understandable. The image should communicate a cooperative workplace accident at thumbnail size.

Build a composition that can be rearranged into horizontal and vertical crops. Keep characters, packages, logo, and background on separate source layers. The final logo must remain legible on the small capsule.

The tagline belongs in supporting copy or a trailer end card, not in the standard capsule. Art style, character designs, colors, and asset sourcing will be selected later with the creator.

## 4. Five screenshot assignments

| Shot | What it must communicate | Capture requirement |
| --- | --- | --- |
| 01 — The job | Multiple workers, distinct dispatch bays, and shared progress | Actual third-person gameplay; readable UI |
| 02 — The sneeze | Windup direction and teammates preparing for it | A real package event with visible setup |
| 03 — The clinger | Two deliveries joined in a useful or inconvenient way | The implemented attachment behavior |
| 04 — The rescue | A coordinated throw or catch around the divider | A real multiplayer interaction |
| 05 — The busy shift | Different cargo behaviors sharing the depot | A playable, representative workload |

If a scene depends on an unfinished mechanic, postpone that capture. Do not replace a missing gameplay screenshot with a generated image, mockup, or pre-rendered scene. Capture with the intended launch UI and no debug overlays.

## 5. Gameplay trailer storyboard

**Production target:** 35–45 seconds. This duration is our proposal, not a Steam requirement.

| Time | Footage | Text / audio intent |
| --- | --- | --- |
| 0–5 sec | A clear sneeze windup followed by a recoverable delivery accident | Open with understandable gameplay and natural sound |
| 5–10 sec | A simple pickup, throw, and successful dispatch | "The packages are alive." |
| 10–20 sec | Show Sneezer and Clinger first as trouble, then as tools | Let repeated cause and effect explain the hook |
| 20–30 sec | Coordinated handoff, rescue, and a busy four-player shift | "Ship them anyway." |
| 30–37 sec | A satisfying quota completion or an accidental success | Short payoff with readable team progress |
| 37–42 sec | Title and real Steam destination | "Wishlist on Steam" only after a live wishlist page exists |

Keep footage close to the player's camera. Show supported player counts only after they work. Use music and sounds with rights covering commercial game marketing and creator video use.

Our recommended export is 1920 × 1080, MP4, H.264 video and AAC audio, at a supported 30 or 60 fps. Use a high bitrate and inspect the processed Steam version. The official guidance lists 5,000+ Kbps; a higher-quality master is appropriate. Finish processing before launch because a converting trailer can block release. [Trailer guidance](https://partner.steamgames.com/doc/store/trailer)

## 6. Acceptance before upload

- All required exports exist at their specified size.
- The title is readable at thumbnail size and matches the store metadata.
- Store and library crops have been inspected in Steam's previews.
- The hero contains no text; its logo is a separate transparent image.
- Screenshots and trailer show actual, representative gameplay.
- Music, fonts, artwork, and footage permissions are recorded in the rights register.
- English copy is proofread; localized assets are tracked separately when produced.
- No screenshot or trailer implies a feature missing from the review build.
