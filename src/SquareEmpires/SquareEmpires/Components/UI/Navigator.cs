using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;

namespace SquareEmpires.Components.UI {
    public class Navigator : Component, IUpdatable {
        private VirtualJoystick moveInput;
        private VirtualAxis zoomInput;
        private VirtualButton resetInput;

        public const int SCROLL_SPEED = 80;
        public const float ZOOM_SPEED = 0.2f;

        public override void initialize() {
            base.initialize();

            moveInput = new VirtualJoystick(true);
            moveInput.addKeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.Left, Keys.Right, Keys.Up,
                Keys.Down);
            zoomInput = new VirtualAxis();
            zoomInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.CancelOut, Keys.OemMinus,
                Keys.OemPlus));
            resetInput = new VirtualButton();
            resetInput.addKeyboardKey(Keys.D0);
        }

        public void update() {
            var scroll = moveInput.value * (SCROLL_SPEED / entity.scene.camera.rawZoom) * Time.deltaTime;
            entity.scene.camera.position += scroll;
            var zoom = zoomInput * ZOOM_SPEED * Time.deltaTime;
            entity.scene.camera.zoom = Mathf.clamp(entity.scene.camera.zoom + zoom, -1, 1);
            if (resetInput) {
                entity.scene.camera.position = Vector2.Zero;
                entity.scene.camera.zoom = 0;
            }
        }
    }
}