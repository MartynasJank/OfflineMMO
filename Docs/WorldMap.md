# World Map

The world is divided into a 2x2 grid of regions. Each region is stored in its own scene and loaded additively at runtime.

| Coordinates | Scene |
|-------------|-------|
| (0,0)       | Region_A1 |
| (0,1)       | Region_A2 |
| (1,0)       | Region_B1 |
| (1,1)       | Region_B2 |

`WorldMap` is a lightweight bootstrap scene that contains the `SceneStreamer` component. The streamer loads and unloads region scenes based on the player's position.

To author new regions, create additional scenes in `Assets/Scenes/World` and add them to the streamer list. Use the **World Builder** window under `Tools/World Builder` to create terrain, place environment prefabs, and bake navigation meshes for each region.
