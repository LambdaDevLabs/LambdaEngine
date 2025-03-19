# LambdaEngine

LambdaEngine is a modular 2D game engine built on SDL3. It provides a
highly flexible architecture where core systems, such as the
`PhysicsSystem` and `RenderSystem`, can be completely replaced with
custom implementations.

Additionally, the `SceneModule` offers a high-level abstraction layer
with a `Scene/GameObject/Component` model, similar to Unity.

## Features
- **Modular Design** – All systems are easily replaceable with custom 
implementations
- **Scene Management** – The API of the `SceneModule` is intentionally
minimalistic, to allow for entirely custom approaches to Scene Management.
- **Performance-Oriented** – All default system implementations aim for
efficient memory usage and low-level optimizations.

## Compatibility & Requirements
- The LambdaEngine is written in **C#** and **.net8**.
- Currently, only **64bit Windows** is explicitly supported.

## License
The LambdaEngine is published under the MIT license.
