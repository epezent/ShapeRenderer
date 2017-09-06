# ShapeRenderer

## Developers

- [Evan Pezent](http://evanpezent.com) (epezent@rice.edu)

## About

ShapeRenderer is a simple yet powerful tool for creating vector graphics directly in the Unity editor. With ShapeRender you can create and stylize any shapes you want on the fly without ever having to use third-party software. Because it creates dynamic meshes, your art will scale to any resolution without pixelation and your build sizes can be kept to a minimum. ShapeRender has an easy-to-use (and not overly bloated) interface similar to other built-in Renderer components. Simply attach a ShapeRender component to any GameObject and use the numerous appearance options to create your art. The ShapeRenderer component can also be controlled through scripting to achieve dynamic 2D animations, and supports 2D colliders for physics as well.

This project was born out of my frustration with having to constantly reopen Photoshop/Illustrator every time I needed to make changes to my art. It has also proven particularly useful for programmatically animating geometric shapes. While similar packages are available on the Asset Store for a fee, very few free alternatives exist. Hopefully ShapeRender can fill this void and help you make games quickly like it has helped me. Enjoy!

## Main Features

### Shape Geometry

- create **concave/convex** shapes with an unlimited number of **anchor points**
- apply **rounded corners** with adjustable radii and smoothness
- several built-in scripts for rapidly creating **primitives** such as Rectangles, Polygons, Triangles, Circles, Diamonds, and more
- fast **C/C++ DLL** plugin handles heavy math and triangulation to keep framerates reasonably high during animation
- multiple **Collider** options:
    - **To Collider** mode updates PolygonCollider2D points to match shape anchors or vertices
    - **From Collider** mode updates shape anchors to match PolygonCollider2D points
    - use the built-in PolygonCollider2D point editing tool to rapidly create shape geometry in From Collider mode
- rotate and mirror shapes

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

- clone or download this repo to you computer
- use one of the scenes in the [ShapeRenderer project folder](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer) as a starting point, or drag all of the files in [Assets](https://github.com/epezent/ShapeRenderer/tree/master/ShapeRenderer/Assets) to your own project
- there are two options for creating a new shape
    1. attach the ShapeRenderer script to a GameObject
    2. attach one of the Shape primitive classes (e.g. CircleShape, SquareShape, etc.) to a GameObject
- use the options available in the inspector to make your shape look the way you want it to
- alternatively, use the public properties of ShapeRenderer or the shape class to change the shape through scripting

## Known Issues/Quirks

- Stroke width may or may not change in real-time while in Editor mode. It will, however, update correctly in Play mode. This seems to be an issue with Unity's built-in LineRenderer component.

## Coming Eventually (Maybe)

- linear gradient offset/shifting
- texture blend modes and opacity adjustment
- shape scaling
- options for inside and outside strokes
- functions for checking if points lie inside of shapes and detecting intersecting shapes
- create composite shapes from multiple shapes using a parent/child structure
- holes
- multi-color linear gradient fill (mesh based)

