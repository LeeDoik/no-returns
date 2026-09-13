# Spring contact and impact-response revision

[한국어](09-reactive-impact.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](../current/01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Target: 0.9.4 / protocol 12. Status: implemented and checked; retest history and limits recorded below.

## Spring platform

The presentation moved/scaled only Top while springs/guide pins stayed still. The shipped GLB merges all four springs and guide pins into one metal mesh. Preserve plate thickness, lower it up to 0.10 m during the 0.18 s warning, then apply the existing 0.48 m launch-height curve. Anchor the support mesh's lower bound and align its upper bound with the actual plate underside. Test ready, compression, launch and recovery across the full sequence. Launch decisions and speeds are unchanged.

## Paper and carton piles

Impact strength derives from worker speed, cargo speed/mass and sneeze distance. Cargo strength is speed×sqrt(mass/3); sneezes have a distance-attenuated maximum of 8, with strength clamped to 0.1–20. This is an effect input scale, not solver-measured force in newtons. Direction and contact position are world-space. The first valid impact determines each activation.

Paper now also reacts to fast worker/cargo contact (at least 1.8 m/s). Initial velocity, spread and rotation use incoming direction/strength and reproducible per-event randomness. Preserve bundled flight followed by fanning, drag, floor/wall checks and timed restoration.

The three decorative cartons now fall/rotate as dynamic rigid bodies instead of interpolating toward predetermined poses. Speed attenuates with distance from the impact point; eccentric impacts contribute angular velocity. Debris collides with floors, walls and other decorative debris, but does not physically push workers/delivery cargo. Existing device effects on workers/cargo remain host-authoritative. This does not turn all decorative cartons into deliverable parcels.

## Networking and validation

Add impact strength/position to prop state, increasing each prop from 6 to 10 values. Bump protocol 11→12; participants must use the same 0.9.4 build. Guests replay the same impact inputs, but debris poses are not streamed frame by frame, so identical final positions are not guaranteed.

New tests cover rigid plate thickness, anchored support bottoms, support-top contact, strength-dependent paper speed, world direction under a rotated parent, carton rigid-body travel and reset. Existing network tests additionally check impact strength/position transmission. Record all three devices using the real renderer. Keep final results and limits in the work log and current validation document.

Compress the state array with DEFLATE for transmission; bound decompression to authored prop count×40 bytes. Retain the existing 2,048-byte campaign metadata regression check. The spring returns continuously to its resting height within about 0.63 seconds after launch.

Obtained passing results for 41 behavior checks, 10 two-peer scenarios and one four-peer scenario. The final throw-animation network check timed out once in the full run and passed on isolated retest; repeat-run stability needs further observation. Windows build/smoke checks and 180-frame packaged execution with the real renderer also passed. Evidence: `artifacts/reactive-fix-suite-final.log`, `artifacts/reactive-animation-network-retest.log`, `artifacts/reactive-four-final.log`, `artifacts/reactive-build.log`, `artifacts/reactive-packed-render.log`.
