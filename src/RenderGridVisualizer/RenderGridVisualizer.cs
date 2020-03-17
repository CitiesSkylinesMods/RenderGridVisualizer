using ColossalFramework.Math;
using UnityEngine;

namespace RenderGridVisualizer
{
    public class RenderGridVisualizer : MonoBehaviour
    {
        private RenderGroup[] _renderGroups;
        private FastList<RenderGroup> _renderedGroups;

        private readonly Color32 _accentColor = new Color32(255, 255, 255, 200);
        private readonly Color32 _backgroundColor = new Color32(255, 0, 0, 200);
        private readonly int _renderGridLength = 45;
        private Texture2D _targetTexture;

        private bool _isOn;
        private OverlayGridTool _tool;

        private void Start()
        {
            _renderGroups = RenderManager.instance.m_groups;
            _renderedGroups = RenderManager.instance.m_renderedGroups;

            _targetTexture = new Texture2D(_renderGridLength, _renderGridLength, TextureFormat.ARGB32, true, true);
            _targetTexture.filterMode = FilterMode.Point;

            GameObject controllerGameObject = ToolsModifierControl.toolController.gameObject;
            _tool = controllerGameObject.GetComponent<OverlayGridTool>() ?? controllerGameObject.AddComponent<OverlayGridTool>();
            _tool.SetRenderTexture(_targetTexture);
        }

        private void OnGUI()
        {
            //bug... tool like TM:PE cannot be changed when this is enabled.
            _isOn = GUI.Toggle(new Rect(600, 15, 100, 20), _isOn, "Toggle Grid");

            if (!(Event.current.type == EventType.Repaint))
            {
                return;
            }

            var renderedGroupIndex = 0;
            var renderedGroup = _renderedGroups[renderedGroupIndex];
            for (int x = 0; x < _renderGridLength; x++)
            {
                for (int y = 0; y < _renderGridLength; y++)
                {
                    var renderGroupIndex = x * _renderGridLength + y;
                    var renderGroup = _renderGroups[renderGroupIndex];
                    if (renderedGroupIndex < _renderedGroups.m_size && renderGroup == renderedGroup)
                    {
                        _targetTexture.SetPixel(x, y, _accentColor);

                        renderedGroupIndex++;
                        renderedGroup = _renderedGroups[renderedGroupIndex];
                    }
                    else
                    {
                        _targetTexture.SetPixel(x, y, _backgroundColor);
                    }
                }
            }

            //TODO convert to 1D again, and use SetPixels instead, should be faster.

            _targetTexture.Apply();
            GUI.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(_renderGridLength * 8, _renderGridLength * 8)), _targetTexture);

            //TODO try render directly
            //var quad = new Quad3(
            //    new Vector3(0f, 0f, 0f),
            //    new Vector3(1000f, 0f, 0f),
            //    new Vector3(1000f, 0f, 1000f),
            //    new Vector3(0f, 0f, 1000f)
            //);
            //RenderManager.instance.OverlayEffect.DrawQuad(RenderManager.instance.CurrentCameraInfo, _targetTexture, Color.white, quad, -10000f, 10000f, false, true);

            if (_isOn && ToolsModifierControl.toolController.CurrentTool != _tool)
            {
                ToolsModifierControl.toolController.CurrentTool = _tool;
                ToolsModifierControl.SetTool<OverlayGridTool>();
            }
            else if (!_isOn && ToolsModifierControl.toolController.CurrentTool == _tool)
            {
                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                ToolsModifierControl.SetTool<DefaultTool>();
            }
        }
    }
}
