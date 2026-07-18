# Playsmart UI Toolkit Extensions

A performance-safe, lightweight library extending Unity UI Toolkit with custom procedurally-drawn components and simulated layout helpers.

This library is designed for Unity projects using UI Toolkit, providing a clean separation between code and visual design by using USS custom style properties (CSS variables).

## Features

### 1. Gap
Simulates flexbox gap spacing natively in USS using margins. It resolves the `Unknown property 'gap'` compiler warning while avoiding layout cycle loops.
* **USS Property:** `--gap` (takes a raw float value)
* **Flex Direction Aware:** Automatically detects and adjusts spacing horizontally or vertically depending on the parent's `flex-direction`.
* **Predictable Margin Clear:** Automatically sets the margin of the last child in the list to `0` to keep alignment crisp.

### 2. Skew
Procedurally-drawn VisualElements that render slanted parallelogram backgrounds with customizable border strokes, skew size, and skew angle.
* **Skew:** A container element.
* **SkewButton:** A clickable element that inherits from Unity's standard `Button` and supports custom skew styles and pixel-perfect slanted hit-testing.
* **USS Properties:**
  * `--skew`: The offset size of the slant (default: `10f`).
  * `--skew-angle`: The angle of the slant in degrees (takes precedence over skew size if set).
  * `--skew-fill-color`: Background color.
  * `--skew-stroke-color`: Border color.
  * `--skew-stroke-width`: Border width.

---

## Installation

### Add via Git URL (UPM)
To add this package to your Unity project:
1. Open the Unity Package Manager (`Window > Package Manager`).
2. Click the `+` button in the top-left corner.
3. Select **Add package from git URL...**.
4. Enter the repository URL:
   `https://github.com/playsmart/uitoolkit-extensions.git`

---

## Quick Start

### Using `Gap` in UXML and USS

**UXML:**
```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:ps="Playsmart.UIToolkit">
    <ps:Gap class="my-container">
        <ui:VisualElement class="item" />
        <ui:VisualElement class="item" />
        <ui:VisualElement class="item" />
    </ps:Gap>
</ui:UXML>
```

**USS:**
```css
.my-container {
    flex-direction: row;
    --gap: 20; /* 20px gap spacing between items */
}
```

### Using `Skew` and `SkewButton` in UXML and USS

**UXML:**
```xml
<ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:ps="Playsmart.UIToolkit">
    <!-- Container -->
    <ps:Skew class="slanted-card">
        <ui:Label text="Procedural Parallelogram" />
    </ps:Skew>

    <!-- Button -->
    <ps:SkewButton class="slanted-card" text="PLAY GAME" />
</ui:UXML>
```

**USS:**
```css
.slanted-card {
    width: 200px;
    height: 50px;
    --skew-angle: 15;
    --skew-fill-color: rgba(20, 20, 30, 0.8);
    --skew-stroke-color: cyan;
    --skew-stroke-width: 1.5;
    --skew-text-color: #ffffff;
}
```

---

If this library helps your workflow, please consider leaving a ⭐️ to help other Unity developers find it!

---

## License
Licensed under the [MIT License](LICENSE).
