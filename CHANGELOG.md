# Changelog

All notable changes to this project will be documented in this file.

## [1.1.0] - 2026-07-18

### Added
- **`--skew-text-color`** custom USS property to style SkewButton text color declaratively.
- Demo scene, UXML, and USS assets in `docs/demo/`.

### Changed
- Organized runtime files into `Gap/` and `Skew/` subfolders.
- SkewButton text now renders via a child Label overlay with one-shot sync for reliable rendering on top of procedural backgrounds.
- Minimum Unity version set to `6000.0` (Unity 6+) to reflect `[UxmlElement]` usage.

## [1.0.0] - 2026-07-17

### Added
- **Gap** custom VisualElement to dynamically simulate Flexbox gap spacing based on the `--gap` USS custom style property.
- **Skew** custom VisualElement to draw procedurally skewed backgrounds with customizable skew angles, fill colors, and border strokes.
- **SkewButton** custom Button to draw procedurally skewed button backgrounds with pixel-perfect slanted hit-testing bounds.
