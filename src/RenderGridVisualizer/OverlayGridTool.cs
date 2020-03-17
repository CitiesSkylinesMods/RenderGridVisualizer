using System;
using ColossalFramework.Math;
using UnityEngine;

namespace RenderGridVisualizer {
    public class OverlayGridTool : DefaultTool {
        private static float GRID_CELL_SIZE = VehicleManager.VEHICLEGRID_CELL_SIZE;
        private static float GRID_COLOR_ALPHA = 0.8f;

        private static Vector3 _cursorPos = Vector3.zero;
        private static int _tilesWidth = 1;
        private static bool _onOff;

        private static Rect _buttonPosition = new Rect(new Vector2(500, 10), new Vector2(80, 20));
        private static Rect _sliderLabelPosition = new Rect(730, 35, 50, 20);
        private static Rect _sliderPosition = new Rect(510, 40, 200, 20);

        private Texture2D _renderTexture = null;

        public void SetRenderTexture(Texture2D renderTexture)
        {
            _renderTexture = renderTexture;
        }

        protected override void OnToolGUI(Event e) {
            base.OnToolGUI(e);

            _tilesWidth = (int) Math.Floor(GUI.HorizontalSlider(_sliderPosition, _tilesWidth, 1, 60));
            GUI.Label(_sliderLabelPosition, $"{_tilesWidth}x{_tilesWidth}");

            if (GUI.Button(_buttonPosition, (_onOff ? "Disable" : "Enable"))) {
                _onOff = !_onOff;
            }

            RaycastInput input = new RaycastInput(m_mouseRay, m_mouseRayLength);
            if (RayCast(input, out RaycastOutput output)) {
                _cursorPos = output.m_hitPos;
            }
        }

        public override void RenderOverlay(RenderManager.CameraInfo cameraInfo) {
            base.RenderOverlay(cameraInfo);
            if (_onOff) {
                RenderGrid(cameraInfo);
            }
        }

        private void RenderGrid(RenderManager.CameraInfo cameraInfo) {
            int halfTileNumber = (int) Math.Ceiling(_tilesWidth / 2.0);
            int gridX = (int) Mathf.Clamp(_cursorPos.x / GRID_CELL_SIZE, -270, 270) - halfTileNumber;
            int gridZ = (int) Mathf.Clamp(_cursorPos.z / GRID_CELL_SIZE, -270, 270) - halfTileNumber;

            int x = (int) Math.Ceiling(gridX * GRID_CELL_SIZE);
            int z = (int) Math.Ceiling(gridZ * GRID_CELL_SIZE);

            for (int i = 0; i < _tilesWidth; i++) {
                for (int j = 0; j < _tilesWidth; j++) {
                    RenderQuad(cameraInfo, new Vector3(x + (GRID_CELL_SIZE * i), 0, z + (GRID_CELL_SIZE * j)), i, j);
                }
            }
        }

        private void RenderQuad(RenderManager.CameraInfo cameraInfo, Vector3 pos, int i, int y) {
            var quad = new Quad3(
                pos,
                new Vector3(pos.x + GRID_CELL_SIZE, pos.y, pos.z),
                new Vector3(pos.x + GRID_CELL_SIZE, pos.y, pos.z + GRID_CELL_SIZE),
                new Vector3(pos.x, pos.y, pos.z + GRID_CELL_SIZE)
            );
            Color color = (i % 2 == 0 ^ y % 2 != 0 ? Color.yellow : Color.white);
            color.a = GRID_COLOR_ALPHA;
            if (_renderTexture == null)
            {
                RenderManager.instance.OverlayEffect.DrawQuad(cameraInfo, color, quad, -10000f, 10025f, false, true);
            }
            else
            {
                RenderManager.instance.OverlayEffect.DrawQuad(cameraInfo, _renderTexture, Color.white, quad, -10000f, 10025f, false, true);
            }
        }
    }
}