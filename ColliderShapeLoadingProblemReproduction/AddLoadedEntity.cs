#nullable enable

using Stride.Core.Mathematics;
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

        public override void Start()
        {
            _simulation = this.GetSimulation();
            if (_simulation == null)
                return;

            _model = Content.Load<Model>("MaterialModel");
            _colliderShape = Content.Load<PhysicsColliderShape>("ColliderHull");

            var entity = new Entity(null, new Vector3(0f))
            {
                new ModelComponent(_model),
                new StaticColliderComponent()
                {
                    ColliderShape = _colliderShape.Shape
                }
            };

            Entity.Scene.Entities.Add(entity);
        }

        public override void Update()
        {
        }
    }
}
