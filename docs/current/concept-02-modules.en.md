# CONCEPT / 02 — modular asset breakdown

[한국어](concept-02-modules.ko.md)

2026-09-25

Visual breakdown of the supplied three-panel image: exterior delivery, ship cargo interior and terminal interaction. Source filename: `codex-clipboard-6c65fca9-da5a-4569-a696-d33b049855bf.png`. Visible elements are distinguished from proposed assembly additions. No new models, existing-asset quality audit or exact dimension approval occurred. There are 34 production entries; sets may contain multiple meshes and do not represent file counts or finished assets.

| ID | Category | Production item | Basis |
|---|---|---|---|
| A01 | Exterior | Basic exterior wall panel | Visible |
| A02 | Exterior | Vertical structural column | Visible |
| A03 | Exterior | Header beam / horizontal band | Visible |
| A04 | Exterior | Large entrance frame | Visible |
| A05 | Exterior | Sealed door leaf | Visible |
| A06 | Exterior | Exterior stair flight | Visible |
| A07 | Exterior | Stair landing | Visible |
| A08 | Exterior | Sloped handrail and posts | Visible |
| B01 | Interior | Interior wall panel | Visible |
| B02 | Interior | Sloped wall-to-ceiling panel | Visible |
| B03 | Interior | Repeating rib frame | Visible |
| B04 | Interior | Ceiling panel | Visible |
| B05 | Interior | Metal floor panel | Visible |
| B06 | Interior | Floor rail / channel strip | Visible |
| B07 | Interior | Wall-mounted restraint strap panel | Visible |
| B08 | Interior | Hanging handle / loop | Visible |
| C01 | Utilities | Straight pipe | Visible |
| C02 | Utilities | 90-degree pipe elbow | Visible |
| C03 | Utilities | Pipe support / clamp | Visible |
| C04 | Utilities | Overhead pipe support frame | Visible |
| C05 | Utilities | Vent / external equipment housing | Visible |
| C06 | Utilities | Linear work light | Visible |
| C07 | Utilities | Red warning light | Visible |
| C08 | Utilities | Small indicator lamp | Visible |
| D01 | Devices and props | Large route display frame and console | Visible |
| D02 | Devices and props | Wall-mounted small terminal | Visible |
| D03 | Devices and props | Sealed delivery parcel | Visible |
| D04 | Devices and props | Storage hard case | Visible |
| E01 | Assembly additions | Inner / outer corner set | Proposed assembly addition |
| E02 | Assembly additions | Half-width wall / filler panel | Proposed assembly addition |
| E03 | Assembly additions | Wall end / top cap set | Proposed assembly addition |
| E04 | Assembly additions | Straight / corner / end railing set | Proposed assembly addition |
| E05 | Assembly additions | Pipe tee / end cap set | Proposed assembly addition |
| E06 | Assembly additions | Ceiling / floor edge trim | Proposed assembly addition |

## Assets separate from repeatable architecture

- Employee body: one base with orange, teal and other team-color variants. First-person arms are not shown but are a separate requirement of the current game camera.
- Giant outer creature: its distant silhouette is visible; do not infer and approve detailed anatomy from it. Prioritize the already approved brown A design.
- Ship: separate angular shell, rear entrance frame, landing legs, moving door/ramp and equipment assemblies. The pictured ramp is an observation, not approval to override current interior decisions, resume exterior production or reintroduce a ramp.
- Distant scenery: factory masses, chimneys/towers and small warning lamps. Separate simplified backdrop models from accessible buildings.
- Planet and sky: consider background materials/sky; a gigantic 3D model is not required.

## Materials, surfaces and presentation rather than extra meshes

Prepare shared worn painted metal, dark reinforcement, orange/red bands, metal floor and exterior ground. Logos, barcodes, numbers, grime and chipped paint are material/decal variations. Separate UI screens and emissive surfaces into material regions. Purple fog, warm work lights, green displays and red alarms are Unity lighting/effects work. The pictured outdoor ground needs its own surface mesh/material treatment rather than being treated as the same interior tile.

## Suggested first production group

Use A01–A08, B01–B06, C01–C03, C06–C07, D02–D03 and necessary E connectors to assemble an entrance, workroom and delivery device. Existing models are reuse candidates; inspect defects before remaking them. Separate door leaves, screens, emissive faces and hanging loops for movement/material changes. Floor rails are initially visual; the image does not authorize conveyors or automatic transport.

Do not derive fixed dimensions from the picture. Use the current 3.2m-wide, 3.3m-high openings and 4m ceiling underside as an initial compatibility reference, then verify employee/cargo passage. Preserve interior-first production and image approval before 3D generation.

[Current spatial specification](cinder-density.en.md) · [Production guide](03-guides.en.md)
