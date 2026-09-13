# Baton shell restoration — 0.8.15

[한국어](baton-mesh-fix.ko.md)

The user reported surface holes or protruding fragments. Unity prefab rendering reproduced a long opening in the shell above the display. The previous preparation script deleted entire polygons when their centroids fell inside the screen coordinate range. Shell polygons extending beyond that boundary were also deleted, establishing the reproduced cause. Normal inspection also found faces requiring reorientation, but this does not establish the cause of every user-observed symptom.

## Changes

Removed the deletion of 357 faces from the [preparation script](../../art/psx-baton-02/prepare.py). Preserve the complete source shell, merge duplicate vertices within 0.00001m, then normalize face orientation and triangulation. A separate display face sits inside the existing frame without cutting the shell. Its Blender Y position is -0.048m. Export requires the source and final shell triangle counts to match.

The repaired model retains 9694 source shell triangles and a separate screen. Length remains 0.68m; the body texture and approved appearance remain. Updated the selected FBX, Blender file and Unity FBX. Attack rules, 6-second charging, electrical arcs and wall-proximity placement rules are unchanged. This document supersedes the screen-face deletion description from 0.8.13.

## Validation

- [x] Blender FBX round-trip: 2 meshes with UVs retained.
- [x] Unity MCP prefab rendering confirms the reproduced shell opening is closed.
- [x] Unity MCP Windows 0.8.15 build completed.
- [x] Charging/display checks across two real executables: 11 passed.
- [x] Ready and mid-charge executable captures confirm the restored shell and visible display.
- [ ] User-controlled confirmation that all reported symptoms are resolved.

[Model validation](../../art/psx-baton-02/selected/validation.json) · [Online display checks](../../artifacts/baton-feedback/latest.json). Captures/build logs are local in artifacts/baton-mesh and artifacts/baton-fix-build.json. These are scripted-input tests and rendered observations, not human impact-feel or 4-player validation.
