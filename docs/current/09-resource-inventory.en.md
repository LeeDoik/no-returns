# Current resource inventory

[한국어](09-resource-inventory.ko.md)

> **Historical record · retired/superseded 2026-09-12.** The [PSX space-delivery design](01-overview.en.md) is current. Implementation, launch, art and values below are historical, not current status. Deleted files remain path records only.


Inspected 2026-09-09. Static code/file audit, not a runtime usage trace or rendering-quality review. No game code changes.

Scope: models, images and authoring sources in assets/ and art/, scenes/, scripts/ shaders and procedural resources below. Caches (.import/.godot), build outputs, artifacts/ test captures, tools, documents and job metadata are excluded from asset counts. Every scoped file is listed below.

34 files in runtime asset folder; 97 total scoped files.

## Runtime-folder models

| Resource | File | Meshes | Embedded images | Animations |
|---|---|---:|---:|---|
| Clinger | clinger.glb (`../../assets/art/release-01/clinger.glb`; retired file) | 21 | 0 | — |
| Conveyor module | conveyor_module.glb (`../../assets/art/release-01/conveyor_module.glb`; retired file) | 4 | 0 | — |
| Dispatch unit | dispatch.glb (`../../assets/art/release-01/dispatch.glb`; retired file) | 6 | 0 | — |
| Hopper | hopper.glb (`../../assets/art/release-01/hopper.glb`; retired file) | 22 | 0 | — |
| Packing cushion | packing_cushion.glb (`../../assets/art/release-01/packing_cushion.glb`; retired file) | 9 | 0 | — |
| Packrat | packrat.glb (`../../assets/art/release-01/packrat.glb`; retired file) | 1 | 4 | — |
| Pendant light | pendant.glb (`../../assets/art/release-01/pendant.glb`; retired file) | 4 | 0 | — |
| Return spring | return_spring.glb (`../../assets/art/release-01/return_spring.glb`; retired file) | 4 | 0 | — |
| Shelf | shelf.glb (`../../assets/art/release-01/shelf.glb`; retired file) | 3 | 0 | — |
| Sneezer | sneezer.glb (`../../assets/art/release-01/sneezer.glb`; retired file) | 20 | 0 | — |
| Standard carton | standard.glb (`../../assets/art/release-01/standard.glb`; retired file) | 9 | 0 | — |
| Workbench | workbench.glb (`../../assets/art/release-01/workbench.glb`; retired file) | 5 | 0 | — |
| Worker | worker.glb (`../../assets/art/release-01/worker.glb`; retired file) | 1 | 1 | air_fall, air_rise, carry_air, carry_back, carry_back_left, carry_back_right, carry_forward_left, carry_forward_right, carry_idle, carry_left, carry_right, carry_walk, hit_to_body_01, idle, land, run, throw, walk |

Usage evidence: art loader (`../../scripts/postal_art.gd`; retired file), worker (`../../scripts/worker.gd`; retired file), cargo (`../../scripts/cargo.gd`; retired file), rat (`../../scripts/packrat.gd`; retired file), reactive props (`../../scripts/reactive_prop.gd`; retired file). Facility meshes are embedded in the current map (`../../scenes/maps/shipping_shrine.tscn`; retired file), so an external-GLB-only search misses usage. Folder presence does not mean every file is loaded every frame.

## Materials, images and authoring sources

There are 21 external runtime-folder images: five each for cardboard, paint and floor (basecolor, normal, roughness, height, seamless), two worker images and four packrat images. Cardboard/paint basecolor and normal maps are dynamically assigned by the art loader; paint/floor basecolor and normal maps are also directly referenced in the map. Runtime references for the remaining auxiliary maps were not established; they are not declared safe to delete.

art/release-01: six worker/rat/depot/cardboard/paint/floor concepts, postal-kit/worker/packrat Blender sources and generated GLBs. art/tripo-01: worker T-poses, animation candidates, sources and previews. art/worker-motion-02: current worker-animation Blender source and GLB. art/sneezer: four early original/comic/PS1 concepts, model v0.1/v0.2, previews, textures and backup. The art/sneezer models are separate from the current release-01/sneezer.glb.

## Procedural and embedded resources

- Map: floors, walls, stairs, ramps, partitions, facilities, lights, signs, dispatch areas, rat routes and interaction zones; nodes/materials are embedded.
- Reactive devices: cushion, spring, carton tower reusing three standard models, and procedural paper stacks, printed lines and stamps.
- Effects: sneeze warning/breath/direction, clinger/hopper cues, pings, device reactions and debris. Procedural; not captured by an image-file-only inventory.
- UI: menus, HUD, settings, contracts, results and bilingual strings, using interface/copy/campaign_copy/release_copy/settings_panel and Label3D/SystemFont.
- Audio: WAV data synthesized by sneeze_cues/feedback/reactive_prop. No external WAV/OGG/MP3 or TTF/OTF files in the scoped source paths. System fonts are used.
- Shader: crew_suit.gdshader. Model materials and procedural StandardMaterial3D resources also exist.

### Current map embedded counts

```json
{
  "nodes": {
    "Node3D": 431,
    "WorldEnvironment": 1,
    "DirectionalLight3D": 1,
    "OmniLight3D": 14,
    "MeshInstance3D": 1644,
    "Camera3D": 1,
    "StaticBody3D": 162,
    "CollisionShape3D": 171,
    "Label3D": 122,
    "Area3D": 9,
    "Marker3D": 40
  },
  "subresources": {
    "Environment": 1,
    "StandardMaterial3D": 1508,
    "ArrayMesh": 82,
    "BoxMesh": 1254,
    "Gradient": 1,
    "FastNoiseLite": 1,
    "NoiseTexture2D": 1,
    "BoxShape3D": 171,
    "SystemFont": 110,
    "CylinderMesh": 16,
    "TorusMesh": 5,
    "SphereMesh": 24
  },
  "art_nodes": {
    "Art_pendant": 14,
    "Art_shelf": 8,
    "Art_workbench": 7,
    "Art_dispatch": 2,
    "Art_conveyor_module": 8
  }
}
```

Counts describe declarations in scene text, not runtime object counts after spawning or removal.

## Complete per-file list

| Path | Type | Bytes |
|---|---|---:|
| assets/art/release-01/clinger.glb (`../../assets/art/release-01/clinger.glb`; retired file) | glb | 638152 |
| assets/art/release-01/conveyor_module.glb (`../../assets/art/release-01/conveyor_module.glb`; retired file) | glb | 317088 |
| assets/art/release-01/dispatch.glb (`../../assets/art/release-01/dispatch.glb`; retired file) | glb | 275120 |
| assets/art/release-01/hopper.glb (`../../assets/art/release-01/hopper.glb`; retired file) | glb | 571452 |
| assets/art/release-01/packing_cushion.glb (`../../assets/art/release-01/packing_cushion.glb`; retired file) | glb | 139460 |
| assets/art/release-01/packrat.glb (`../../assets/art/release-01/packrat.glb`; retired file) | glb | 8279884 |
| assets/art/release-01/packrat_Image_0.jpg (`../../assets/art/release-01/packrat_Image_0.jpg`; retired file) | jpg | 3293919 |
| assets/art/release-01/packrat_Image_1.jpg (`../../assets/art/release-01/packrat_Image_1.jpg`; retired file) | jpg | 1835691 |
| assets/art/release-01/packrat_Image_2.jpg (`../../assets/art/release-01/packrat_Image_2.jpg`; retired file) | jpg | 2191727 |
| assets/art/release-01/packrat_Image_3.jpg (`../../assets/art/release-01/packrat_Image_3.jpg`; retired file) | jpg | 85913 |
| assets/art/release-01/pendant.glb (`../../assets/art/release-01/pendant.glb`; retired file) | glb | 46556 |
| assets/art/release-01/return_spring.glb (`../../assets/art/release-01/return_spring.glb`; retired file) | glb | 306080 |
| assets/art/release-01/shelf.glb (`../../assets/art/release-01/shelf.glb`; retired file) | glb | 306356 |
| assets/art/release-01/sneezer.glb (`../../assets/art/release-01/sneezer.glb`; retired file) | glb | 532228 |
| assets/art/release-01/standard.glb (`../../assets/art/release-01/standard.glb`; retired file) | glb | 224888 |
| assets/art/release-01/textures/cardboard_basecolor.png (`../../assets/art/release-01/textures/cardboard_basecolor.png`; retired file) | png | 15970585 |
| assets/art/release-01/textures/cardboard_height.png (`../../assets/art/release-01/textures/cardboard_height.png`; retired file) | png | 5296492 |
| assets/art/release-01/textures/cardboard_normal.png (`../../assets/art/release-01/textures/cardboard_normal.png`; retired file) | png | 16736327 |
| assets/art/release-01/textures/cardboard_roughness.png (`../../assets/art/release-01/textures/cardboard_roughness.png`; retired file) | png | 7036822 |
| assets/art/release-01/textures/cardboard_seamless.png (`../../assets/art/release-01/textures/cardboard_seamless.png`; retired file) | png | 15970585 |
| assets/art/release-01/textures/floor_basecolor.png (`../../assets/art/release-01/textures/floor_basecolor.png`; retired file) | png | 16267315 |
| assets/art/release-01/textures/floor_height.png (`../../assets/art/release-01/textures/floor_height.png`; retired file) | png | 5186926 |
| assets/art/release-01/textures/floor_normal.png (`../../assets/art/release-01/textures/floor_normal.png`; retired file) | png | 16354810 |
| assets/art/release-01/textures/floor_roughness.png (`../../assets/art/release-01/textures/floor_roughness.png`; retired file) | png | 7181005 |
| assets/art/release-01/textures/floor_seamless.png (`../../assets/art/release-01/textures/floor_seamless.png`; retired file) | png | 16267315 |
| assets/art/release-01/textures/paint_basecolor.png (`../../assets/art/release-01/textures/paint_basecolor.png`; retired file) | png | 10412420 |
| assets/art/release-01/textures/paint_height.png (`../../assets/art/release-01/textures/paint_height.png`; retired file) | png | 4871511 |
| assets/art/release-01/textures/paint_normal.png (`../../assets/art/release-01/textures/paint_normal.png`; retired file) | png | 15530362 |
| assets/art/release-01/textures/paint_roughness.png (`../../assets/art/release-01/textures/paint_roughness.png`; retired file) | png | 5341554 |
| assets/art/release-01/textures/paint_seamless.png (`../../assets/art/release-01/textures/paint_seamless.png`; retired file) | png | 10412420 |
| assets/art/release-01/workbench.glb (`../../assets/art/release-01/workbench.glb`; retired file) | glb | 229788 |
| assets/art/release-01/worker.glb (`../../assets/art/release-01/worker.glb`; retired file) | glb | 9591636 |
| assets/art/release-01/worker_texture_0.png (`../../assets/art/release-01/worker_texture_0.png`; retired file) | png | 7638560 |
| assets/art/release-01/worker_worker-tripo-input_glb_basecolor.png (`../../assets/art/release-01/worker_worker-tripo-input_glb_basecolor.png`; retired file) | png | 7638560 |
| art/release-01/concepts/cardboard.png (`../../art/release-01/concepts/cardboard.png`; retired file) | png | 18931940 |
| art/release-01/concepts/depot.png (`../../art/release-01/concepts/depot.png`; retired file) | png | 12956554 |
| art/release-01/concepts/floor.png (`../../art/release-01/concepts/floor.png`; retired file) | png | 19069742 |
| art/release-01/concepts/packrat.png (`../../art/release-01/concepts/packrat.png`; retired file) | png | 6075376 |
| art/release-01/concepts/paint.png (`../../art/release-01/concepts/paint.png`; retired file) | png | 11184418 |
| art/release-01/concepts/worker.png (`../../art/release-01/concepts/worker.png`; retired file) | png | 5580957 |
| art/release-01/source/packrat-generated.glb (`../../art/release-01/source/packrat-generated.glb`; retired file) | glb | 8045144 |
| art/release-01/source/packrat.blend (`../../art/release-01/source/packrat.blend`; retired file) | blend | 8145510 |
| art/release-01/source/postal-kit.blend (`../../art/release-01/source/postal-kit.blend`; retired file) | blend | 710738 |
| art/release-01/source/worker-generated.glb (`../../art/release-01/source/worker-generated.glb`; retired file) | glb | 8983180 |
| art/release-01/source/worker.blend (`../../art/release-01/source/worker.blend`; retired file) | blend | 9183863 |
| art/sneezer/concepts/sneezer-states-ps1-v01.png (`../../art/sneezer/concepts/sneezer-states-ps1-v01.png`; retired file) | png | 1526885 |
| art/sneezer/concepts/sneezer-states-ps1-v02.png (`../../art/sneezer/concepts/sneezer-states-ps1-v02.png`; retired file) | png | 1396600 |
| art/sneezer/concepts/sneezer-states-v01.png (`../../art/sneezer/concepts/sneezer-states-v01.png`; retired file) | png | 1611205 |
| art/sneezer/concepts/sneezer-states-v02.png (`../../art/sneezer/concepts/sneezer-states-v02.png`; retired file) | png | 1617172 |
| art/sneezer/model-v01/previews/glb-reimport-sneeze.png (`../../art/sneezer/model-v01/previews/glb-reimport-sneeze.png`; retired file) | png | 875915 |
| art/sneezer/model-v01/previews/idle.png (`../../art/sneezer/model-v01/previews/idle.png`; retired file) | png | 870559 |
| art/sneezer/model-v01/previews/sneeze.png (`../../art/sneezer/model-v01/previews/sneeze.png`; retired file) | png | 875720 |
| art/sneezer/model-v01/previews/warning.png (`../../art/sneezer/model-v01/previews/warning.png`; retired file) | png | 869628 |
| art/sneezer/model-v01/sneezer-idle.glb (`../../art/sneezer/model-v01/sneezer-idle.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-idle_cardboard.png (`../../art/sneezer/model-v01/sneezer-idle_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer-sneeze.glb (`../../art/sneezer/model-v01/sneezer-sneeze.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-sneeze_cardboard.png (`../../art/sneezer/model-v01/sneezer-sneeze_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer-warning.glb (`../../art/sneezer/model-v01/sneezer-warning.glb`; retired file) | glb | 942512 |
| art/sneezer/model-v01/sneezer-warning_cardboard.png (`../../art/sneezer/model-v01/sneezer-warning_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/sneezer.blend (`../../art/sneezer/model-v01/sneezer.blend`; retired file) | blend | 1015798 |
| art/sneezer/model-v01/sneezer.glb (`../../art/sneezer/model-v01/sneezer.glb`; retired file) | glb | 950504 |
| art/sneezer/model-v01/sneezer_cardboard.png (`../../art/sneezer/model-v01/sneezer_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v01/textures/cardboard.png (`../../art/sneezer/model-v01/textures/cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v02/previews/glb-reimport-sneeze.png (`../../art/sneezer/model-v02/previews/glb-reimport-sneeze.png`; retired file) | png | 885737 |
| art/sneezer/model-v02/previews/idle.png (`../../art/sneezer/model-v02/previews/idle.png`; retired file) | png | 878331 |
| art/sneezer/model-v02/previews/sneeze.png (`../../art/sneezer/model-v02/previews/sneeze.png`; retired file) | png | 885535 |
| art/sneezer/model-v02/previews/warning.png (`../../art/sneezer/model-v02/previews/warning.png`; retired file) | png | 879853 |
| art/sneezer/model-v02/sneezer.blend (`../../art/sneezer/model-v02/sneezer.blend`; retired file) | blend | 1030197 |
| art/sneezer/model-v02/sneezer.blend1 (`../../art/sneezer/model-v02/sneezer.blend1`; retired file) | blend1 | 1030173 |
| art/sneezer/model-v02/sneezer.glb (`../../art/sneezer/model-v02/sneezer.glb`; retired file) | glb | 1027052 |
| art/sneezer/model-v02/sneezer_cardboard.png (`../../art/sneezer/model-v02/sneezer_cardboard.png`; retired file) | png | 912480 |
| art/sneezer/model-v02/textures/cardboard.png (`../../art/sneezer/model-v02/textures/cardboard.png`; retired file) | png | 912480 |
| art/tripo-01/preview-air_rise.png (`../../art/tripo-01/preview-air_rise.png`; retired file) | png | 613307 |
| art/tripo-01/preview-carry_idle.png (`../../art/tripo-01/preview-carry_idle.png`; retired file) | png | 599032 |
| art/tripo-01/preview-carry_walk.png (`../../art/tripo-01/preview-carry_walk.png`; retired file) | png | 604310 |
| art/tripo-01/preview-idle.png (`../../art/tripo-01/preview-idle.png`; retired file) | png | 605111 |
| art/tripo-01/preview-jump.png (`../../art/tripo-01/preview-jump.png`; retired file) | png | 602158 |
| art/tripo-01/preview-lift_heavy.png (`../../art/tripo-01/preview-lift_heavy.png`; retired file) | png | 595494 |
| art/tripo-01/preview-run.png (`../../art/tripo-01/preview-run.png`; retired file) | png | 598775 |
| art/tripo-01/preview-throw.png (`../../art/tripo-01/preview-throw.png`; retired file) | png | 597642 |
| art/tripo-01/preview-walk.png (`../../art/tripo-01/preview-walk.png`; retired file) | png | 603134 |
| art/tripo-01/worker-animation.blend (`../../art/tripo-01/worker-animation.blend`; retired file) | blend | 11267608 |
| art/tripo-01/worker-candidate.glb (`../../art/tripo-01/worker-candidate.glb`; retired file) | glb | 9685620 |
| art/tripo-01/worker-tpose-angle.png (`../../art/tripo-01/worker-tpose-angle.png`; retired file) | png | 931538 |
| art/tripo-01/worker-tpose-front.png (`../../art/tripo-01/worker-tpose-front.png`; retired file) | png | 937326 |
| art/tripo-01/worker-tripo-input.blend (`../../art/tripo-01/worker-tripo-input.blend`; retired file) | blend | 8766776 |
| art/tripo-01/worker-tripo-input.glb (`../../art/tripo-01/worker-tripo-input.glb`; retired file) | glb | 8469988 |
| art/tripo-01/worker-tripo-seven.glb (`../../art/tripo-01/worker-tripo-seven.glb`; retired file) | glb | 9405548 |
| art/worker-motion-02/worker-motion.blend (`../../art/worker-motion-02/worker-motion.blend`; retired file) | blend | 12395974 |
| art/worker-motion-02/worker-motion.blend1 (`../../art/worker-motion-02/worker-motion.blend1`; retired file) | blend1 | 12345109 |
| art/worker-motion-02/worker.glb (`../../art/worker-motion-02/worker.glb`; retired file) | glb | 9591636 |
| scenes/main.tscn (`../../scenes/main.tscn`; retired file) | tscn | 321 |
| scenes/maps/shipping_shrine.tscn (`../../scenes/maps/shipping_shrine.tscn`; retired file) | tscn | 2889605 |
| scenes/physics_lab.tscn (`../../scenes/physics_lab.tscn`; retired file) | tscn | 171 |
| scenes/pieces/reactive_prop.tscn (`../../scenes/pieces/reactive_prop.tscn`; retired file) | tscn | 176 |
| scenes/pieces/solid_block.tscn (`../../scenes/pieces/solid_block.tscn`; retired file) | tscn | 942 |
| scripts/crew_suit.gdshader (`../../scripts/crew_suit.gdshader`; retired file) | gdshader | 846 |

Machine-readable inventory: [JSON](09-resource-inventory.json). Current source only; parity with installed builds, external rights, final quality and performance are unverified. Existing ART-01/PERF-01 remain open.
