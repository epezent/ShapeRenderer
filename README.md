# ShapeRenderer

## Developers

- [Evan Pezent](http://evanpezent.com) (epezent@rice.edu)

## About

ShapeRenderer is a simple yet powerful tool for creating vector graphics directly in the Unity editor. With ShapeRender you can create and stylize any shapes you want on the fly without ever having to use third-party software. Because it creates dynamic meshes, your art will scale to any resolution without pixelation and your build sizes can be kept to a minimum. ShapeRender has an easy-to-use (and not overly bloated) interface similar to other built-in Renderer components. Simply attached the ShapeRender component to any GameObject and use the numerous appearance options to create your art. The ShapeRenderer component can also be controlled through scripting to achieve dynamic 2D animations, and supports 2D colliders for physics as well.

This project was born out of my frustration with having to constantly reopen PhotoShop/Illustrator every time I needed to make changes to my art. While similar packages are available on the Asset Store for a fee, very few free alternatives exist. Hopefully ShapeRender can fill this void and help you make games quickly like it has helped me. Enjoy!

## Features

- Create Concave/Convex shapes with an unlimited number of Anchor points
- Apply Rounded Corners with adjustable radii to any Anchor point, independently
- Fill:
    - 1-Color Solid
    - 2-Color Linear Gradient with adjustable angle and percent
    - 2-Color Radial Gradient with adjustable center and percent
- Stroke:
    - variable Width
    - multi-color Gradients
- Apply separate Texture overlays to Fill and Stroke
- Toggle Fill/Stroke on/off independently
- Supports 2D Sorting Layers and works perfectly with Sprites, LineRenderers, etc.
- Collider options:
    - To Collider - updates PolygonCollider2D points to match shape vertices
    - From Collider - updates shape vertices to match PolygonCollider2D points
    - Use the built-in PolygonCollider2D point editing tool to create shape geometry in From Collider mode
- Several built-in scripts for rapidly creating primitives such as Rectangles, Polygons, Triangles, Circles, Diamonds, and more
- Fast C/C++ DLL plugin handles heavy math and triangulation to keep framerates reasonably high during animation
- Custom Shaders optimized to use MPBs and Per Renderer data

## Usage

## How It Works

## Known Issues/Quirks

## Comming Soon

- Multicolor Linear Gradient Fill
- Rotation / Scaling
- stroke solid color
- Textures

