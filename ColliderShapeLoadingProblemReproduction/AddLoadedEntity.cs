#nullable enable

using System;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Stride.Rendering;

namespace ColliderShapeLoadingProblemReproduction
{
    public class AddLoadedEntity : SyncScript
    {
        private Simulation? _simulation;
        private Model? _model;
        private PhysicsColliderShape? _colliderShape;

        public CameraComponent Camera;

        public override void Start()
        {
            _simulation = this.GetSimulation();
        }

        public override void Update()
        {
            if (_simulation != null && Input.HasMouse && Input.IsMouseButtonPressed(MouseButton.Left))
            {
                HitResult hitResult = CalculateMouseCursorPosition(_simulation);

                if (hitResult.Succeeded && hitResult.Collider.CollisionGroup == CollisionFilterGroups.StaticFilter)
                {
                    _model ??= Content.Load<Model>("MaterialModel");
                    _colliderShape ??= Content.Load<PhysicsColliderShape>("ColliderHull");

                    var position = new Vector3(
                        (float) Math.Round((double) hitResult.Point.X),
                        (float) Math.Round((double) hitResult.Point.Y),
                        (float) Math.Round((double) hitResult.Point.Z)
                    );

                    var entity = new Entity(null, position)
                    {
                        new ModelComponent(_model),
                        new StaticColliderComponent()
                        {
                            ColliderShape = _colliderShape.Shape
                        }
                    };

                    Entity.Scene.Entities.Add(entity);
                }
            }
        }

        private HitResult CalculateMouseCursorPosition(Simulation simulation)
        {
            Matrix invViewProj = Matrix.Invert(Camera.ViewProjectionMatrix);

            // Reconstruct the projection-space position in the (-1, +1) range.
            //    Don't forget that Y is down in screen coordinates, but up in projection space
            Vector3 sPos;
            sPos.X = Input.Mouse.Position.X * 2f - 1f;
            sPos.Y = 1f - Input.Mouse.Position.Y * 2f;

            // Compute the near (start) point for the raycast
            // It's assumed to have the same projection space (x,y) coordinates and z = 0 (lying on the near plane)
            // We need to unproject it to world space
            sPos.Z = 0f;
            var vectorNear = Vector3.Transform(sPos, invViewProj);
            vectorNear /= vectorNear.W;

            // Compute the far (end) point for the raycast
            // It's assumed to have the same projection space (x,y) coordinates and z = 1 (lying on the far plane)
            // We need to unproject it to world space
            sPos.Z = 1f;
            var vectorFar = Vector3.Transform(sPos, invViewProj);
            vectorFar /= vectorFar.W;

            // Raycast from the point on the near plane to the point on the far plane and get the collision result
            var hitResult = simulation.Raycast(vectorNear.XYZ(), vectorFar.XYZ());
            return hitResult;
        }
    }
}
