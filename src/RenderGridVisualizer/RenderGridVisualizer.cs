using UnityEngine;

namespace RenderGridVisualizer
{
    public class RenderGridVisualizer : MonoBehaviour
    {
        private readonly Color32 _accentColor = new Color32(255, 255, 255, 200);
        private readonly Color32 _backgroundColor = new Color32(255, 0, 0, 200);
        private readonly int _renderGridLength = 45;

        private RenderGroup[] _renderGroups;
        private FastList<RenderGroup> _renderedGroups;

        private int _renderGridMaxIndex;
        private Texture2D _targetTexture;
        private Color32[] _targetTextureBuffer;
        private OverlayGridTool _tool;
        
        private bool _isOn;

        private void Start()
        {
            _renderGroups = RenderManager.instance.m_groups;
            _renderedGroups = RenderManager.instance.m_renderedGroups;

            _renderGridMaxIndex = _renderGridLength * _renderGridLength;
            _targetTexture = new Texture2D(_renderGridLength, _renderGridLength, TextureFormat.ARGB32, true, true);
            _targetTexture.filterMode = FilterMode.Point;
            _targetTextureBuffer = new Color32[_renderGridLength * _renderGridLength];

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
            for (int i = 0; i < _renderGridMaxIndex; i++)
            {
                var renderGroup = _renderGroups[i];
                if (renderGroup == renderedGroup)
                {
                    _targetTextureBuffer[i] = _accentColor;

                    renderedGroupIndex++;
                    renderedGroup = _renderedGroups[renderedGroupIndex];
                }
                else
                {
                    _targetTextureBuffer[i] = _backgroundColor;
                }
            }

            _targetTexture.SetPixels32(_targetTextureBuffer);
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
