This is a animation previewer tool built to the specs here: https://tinyurl.com/35fbwypc

The tool can be used here: 

Quick start:
- Open `SampleScene`
- Click `Play`
- Enjoy

Features include:
- Wide variety of animations to choose from
- Tabs for different categories of animation
- Search functionality for finding the animation you want
- Fluid tweening between all animations
- Model viewer allows you to rotate and see the animation from all directions
- Animation scrubber for fine movement within the animation
- Data-driven architecture designed for scaling
- UI is built to handle a range of aspect ratios
- WebGL support

Tech stack:
- Animancer is used for handling all animations programmatically
- R3 is used for a reactive veiwer state
- Mixamo provided all animations and models
- Skybox Series Free was used for the pretty skybox
- UGUI tools were used in the editor for setting up the UIs

Architectural notes:
- UI is controller by the `ViewerState` which is a reactive state all views subscribe to
- Animation data is found under the `_AnimLibrary` which has all references to clips and their associate metadata
- Most UI logic can be found in the `AnimViewerUI` which handles the populating of UI view elements as well as the scrubber logic
- `ViewerAnimator` handles all animation logic for the viewer model
- `CameraController` handles camera panning logic

TODO:
- Implement backend loaded animations utilizing a Baas
- Offer a variety of models to animate
- More animations
- I wanted to present this in WebGL but that seems to be broken on Unity 6
https://discussions.unity.com/t/webgl-builds-no-longer-function-after-upgrading-to-unity-6000-0-32f/1576293
