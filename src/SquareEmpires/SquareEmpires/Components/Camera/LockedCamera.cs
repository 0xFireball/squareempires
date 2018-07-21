using System;
using Microsoft.Xna.Framework;
using Nez;

namespace SquareEmpires.Components.Camera {
    public class LockedCamera : Component, IUpdatable {
        public Entity target { get; private set; }
        private readonly Nez.Camera camera;
        private readonly LockMode lockMode;

        private Vector2 _precisePosition;
        private Vector2 _lastPosition;

        [Flags]
        public enum LockMode {
            Position,
            Rotation
        }

        public LockedCamera(Entity target, Nez.Camera camera, LockMode lockMode) {
            this.target = target;
            this.camera = camera;
            this.lockMode = lockMode;
        }

        public override void onAddedToEntity() {
            base.onAddedToEntity();

            entity.updateOrder = int.MaxValue;
        }

        public void setTarget(Entity target) {
            this.target = target;
        }

        public void update() {
            if (target != null) {
                updateFollow();
            }
        }

        private void updateFollow() {
            // handle teleportation
            if (_lastPosition != camera.position) {
                _precisePosition = camera.position;
            }

            // lock position
            if (lockMode.HasFlag(LockMode.Position)) {
                _precisePosition = target.position;
            }

            // lock rotation
            if (lockMode.HasFlag(LockMode.Rotation)) {
                camera.transform.localRotation = -target.transform.localRotation;
            }

            camera.position = _precisePosition;

            _lastPosition = camera.position;
        }
    }
}