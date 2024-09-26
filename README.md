# Reproduction of a possible bug with loading Collider Shape

This simple project contains a scene, Entity and a script. Script loads Model and Colider Shape too add new Entities to the Scene when Left mouse button clicked and cursor is inside the Groud.

### A possible bug description
After loading Collider Shapes's field Shape is null if there is no Entity in the Scene with this Collider Shape. It leads to "[PhysicsComponent]: Error: Entity { Entity.Name} has a PhysicsComponent without any collider shape." when you try to load and add ColliderComponent to a new Enity programmatically.


### Steps to reproduce a possible bug:
1. Delete M Entity from the scene.
2. Mark ColliderHull Asset with "Include in build as root asset".

