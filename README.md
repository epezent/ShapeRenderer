# ShapeRenderer

![ShapeRenderer](https://github.com/epezent/ShapeRenderer/blob/master/screenshot.png)

## Developers

- [Evan Pezent](http://evanpezent.com) (epezent@rice.edu)

## About

ShapeRenderer is a simple yet powerful tool for creating vector graphics directly in the Unity editor. With ShapeRenderer you can create and stylize any shapes you want on the fly without ever having to use third-party software. Because it creates dynamic meshes, your art will scale to any resolution without pixelation and your build sizes can be kept to a minimum. ShapeRenderer has an easy-to-use (and not overly bloated) interface similar to other built-in Renderer components. Simply attach a ShapeRenderer component to any GameObject and use the numerous appearance options to create your art. The ShapeRenderer component can also be controlled through scripting to achieve dynamic 2D animations, and supports 2D colliders for physics as well.

This project was born out of my frustration with having to constantly reopen Photoshop/Illustrator every time I needed to make changes to my art. It has also proven particularly useful for programmatically animating geometric shapes. While similar packages are available on the Asset Store for a fee, very few free alternatives exist. Hopefully ShapeRenderer can fill this void and help you make games quickly like it has helped me. Enjoy!

## Main Features

### Shape Geometry

- create **concave/convex** shapes with an unlimited number of **anchor points**
- apply **rounded corners** with adjustable radii and smoothness
- rotate, scale and mirror shapes without having to use the Transform component
- several built-in scripts for rapidly creating **primitives** such as Rectangles, Polygons, Triangles, Circles, Diamonds, and more
- multiple **Collider** options:
    - **To Collider** mode updates PolygonCollider2D points to match shape anchors or vertices
    - **From Collider** mode updates shape anchors to match PolygonCollider2D points
    - use the built-in PolygonCollider2D point editing tool to rapidly create shape geometry in From Collider mode
- fast **C/C++ DLL** plugin handles heavy math and triangulation to keep framerates reasonably high during animation

### Shape Appearance

- **Fill:**
    - 1-Color **solid**
    - 2-Color **linear gradient** with adjustable angle and percent
    - 2-Color **radial gradient** with adjustable center and percent
- **Stroke:**
    - variable **width**
    - solid or multi-color gradient stroke colors
- apply separate **texture overlays** to Fill and Stroke
- toggle fill/stroke on/off independently
- supports **2D sorting layers** and works perfectly with Sprites, LineRenderers, etc.
- comes with **custom materials and shaders** optimized to use MPBs and Per Renderer Data for fast material batching
- optionally use your own materials for both fill and stroke

## Usage

**WARNING**: This package depends on a custom DLL plugin. The compiled [ShapeRenderer.dll](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer/Assets/Plugins) file provided in this repository was built for **win64** platforms. If you are developing on a different platform you will need to compile [ShapeRendererDLL C++ code](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRendererDLL) yourself and move the resulting DLL to the [Plugins folder](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer/Assets/Plugins).

- clone or download this repository to you computer
- use one of the scenes in the [ShapeRenderer project folder](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer) as a starting point, or drag all of the files in [Assets](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer/Assets) to your own project
- there are two options for creating a new shape
    1. attach the ShapeRenderer script to a GameObject
    2. attach one of the Shape primitive classes (e.g. CircleShape, SquareShape, etc.) to a GameObject
- use the options available in the Inspector to make your shape look the way you want it to
- alternatively, use the public properties of ShapeRenderer or the Shape class to change the shape through scripting
- one last thing -- you can derive from the Shape class to make new and interesting shapes (and please consider contributing them if you do!)

## Known Issues/Quirks

- Stroke width may or may not change in real-time while in Editor mode. It will, however, update correctly in Play mode. This seems to be an issue with Unity's built-in LineRenderer component. I'm still investigating.
- Copying a GameObject with a ShapeRenderer may create an issue where meshes are shared. This will be fixed in the next update.

## Coming Eventually (Maybe)

In order of most likely to least likely, these are some things I may eventually add to ShapeRenderer (feel free to make contributions toward any!):

- texture blend modes and opacity adjustment
- linear gradient offset/shifting
- functions for checking if points lie inside of shapes and detecting intersecting shapes
- GUI support
- multi-color linear gradient fill (mesh vertex-color based)
- options for inside and outside strokes
- ability to create composite shapes from multiple shapes using Unity's parent/child structure
- holes in shapes
