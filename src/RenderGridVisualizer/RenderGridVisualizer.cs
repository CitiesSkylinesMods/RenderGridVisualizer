using UnityEngine;

namespace RenderGridVisualizer
{
    public class RenderGridVisualizer : MonoBehaviour
    {
        private Texture2D _textureWhite;
        private Texture2D _textureRed;

        private GUIStyle _guiStyleWhite;
        private GUIStyle _guiStyleRed;

        private readonly int _renderGridLength = 45;
        private readonly float _uiItemLength = 10f;
        private readonly float _uiItemGapLength = 1f;

        private FastList<RenderGroup> _renderedGroups;
        private bool _isOn;
        private OverlayGridTool _tool;

        private void Start()
        {
            _textureWhite = new Texture2D(1, 1);
            _textureWhite.SetPixel(0, 0, Color.white);
            _textureWhite.Apply();

            _textureRed = new Texture2D(1, 1);
            _textureRed.SetPixel(0, 0, Color.red);
            _textureRed.Apply();

            _guiStyleWhite = new GUIStyle();
            _guiStyleWhite.normal.background = _textureWhite;

            _guiStyleRed = new GUIStyle();
            _guiStyleRed.normal.background = _textureRed;

            _renderedGroups = RenderManager.instance.m_renderedGroups;
        }

        private void OnGUI()
        {
            for (var i = 0; i < _renderGridLength; i++)
            {
                for (var j = 0; j < _renderGridLength; j++)
                {
                    var found = false;

                    for (int k = 0; k < _renderedGroups.m_size; k++)
                    {
                        var renderGroup = _renderedGroups[k];
                        if (renderGroup.m_x == i && renderGroup.m_z == j)
                        {
                            found = true;
                            break;
                        }
                    }

                    var position = GetPosition(i, j, _uiItemLength, _uiItemGapLength);
                    if (found)
                    {
                        GUI.Box(new Rect(position.x, position.y, _uiItemLength, _uiItemLength), GUIContent.none, _guiStyleWhite);
                    }
                    else
                    {
                        GUI.Box(new Rect(position.x, position.y, _uiItemLength, _uiItemLength), GUIContent.none, _guiStyleRed);
                    }
                }
            }

            //bug... tool like TM:PE cannot be changed when this is enabled.
            _isOn = GUI.Toggle(new Rect(600, 15, 100, 20), _isOn, "Toggle Grid");

            if (_isOn && ToolsModifierControl.toolController.CurrentTool != _tool) {
                ToolsModifierControl.toolController.CurrentTool = _tool;
                ToolsModifierControl.SetTool<OverlayGridTool>();
            } else if (!_isOn && ToolsModifierControl.toolController.CurrentTool == _tool) {
                ToolsModifierControl.toolController.CurrentTool = ToolsModifierControl.GetTool<DefaultTool>();
                ToolsModifierControl.SetTool<DefaultTool>();
            }
        }

        private Vector2 GetPosition(int i, int j, float itemLength, float gapLength)
        {
            return new Vector2
            (
                gapLength + i * (itemLength + gapLength),
                gapLength + j * (itemLength + gapLength)
            );
        }
    }
}
