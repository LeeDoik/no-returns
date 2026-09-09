# Animation transition polish

[한국어](08-animation-polish.ko.md)

## Scope and implementation

0.9.3 refines animation only. Movement speeds, gravity, mass, collision shapes, grabbing/throwing rules and the production map remain unchanged. Reuse the current 18 clips.

Replace immediate switching at 22.5-degree carry-direction boundaries with a 31-degree exit threshold. Reuse side steps while turning in place with cargo to reduce stationary feet under a rotating body. Smooth gait playback rates exponentially. Use 0.24-second stopping transitions, 0.18-second general transitions, 0.09-second throw/landing/hit transitions and 0.12-second airborne transitions. Do not treat a single zero vertical-speed sample at a jump apex as landing. Add subtle breathing and torso lean when empty-handed, without affecting carrying arm poses.

## Verification plan

Regression-test directional boundaries, turning footwork and the jump apex; run existing behavior/network checks and continuous rendering. Human-perceived naturalness and slope foot planting remain separate judgments. In-place turns reuse a side-step clip rather than a dedicated turning clip and do not guarantee perfectly planted feet.

## Verification results

Passed 39 behavior checks, 10 two-peer scenarios and one four-peer scenario. Verified the Windows 0.9.3 package and its actual renderer launch. Protocol 11 is unchanged. `artifacts/motion-polish.mp4` records the same 16-second scenario at 60 fps; carrying, turning and movement samples were reviewed. The transition regression first failed on the old apex detection and then passed with the fix. Full results are in `artifacts/animation-polish-suite.log`.
