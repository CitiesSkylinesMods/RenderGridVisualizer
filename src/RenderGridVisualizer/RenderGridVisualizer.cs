using UnityEngine;

namespace RenderGridVisualizer
{
    public class RenderGridVisualizer : MonoBehaviour
    {
        private Texture2D _textureWhite;
        private Texture2D _textureRed;

        private readonly int _renderGridLength = 45;
        private readonly float _uiItemLength = 10f;
        private readonly float _uiItemGapLength = 1f;

        private RenderGroup[] _renderGroups;
        private FastList<RenderGroup> _renderedGroups;

        private void Start()
        {
            _textureWhite = new Texture2D(1, 1);
            _textureWhite.SetPixel(0, 0, Color.white);
            _textureWhite.Apply();

            _textureRed = new Texture2D(1, 1);
            _textureRed.SetPixel(0, 0, Color.red);
            _textureRed.Apply();

            _renderGroups = RenderManager.instance.m_groups;
            _renderedGroups = RenderManager.instance.m_renderedGroups;
        }

        private void OnGUI()
        {
            if (!Event.current.type.Equals(EventType.Repaint))
            {
                return;
            }

            var renderedGroupIndex = 0;
            var renderedGroup = _renderedGroups[renderedGroupIndex];
            for (int x = 0; x < _renderGridLength; x++)
            {
                for (int y = 0; y < _renderGridLength; y++)
                {
                    var position = GetPosition(x, y, _uiItemLength, _uiItemGapLength);
                    var renderGroupIndex = x * _renderGridLength + y;
                    var renderGroup = _renderGroups[renderGroupIndex];
                    if (renderedGroupIndex < _renderedGroups.m_size && renderGroup == renderedGroup)
                    {
                        GUI.DrawTexture(new Rect(position, new Vector2(_uiItemLength, _uiItemLength)), _textureWhite);
                        renderedGroupIndex++;
                        renderedGroup = _renderedGroups[renderedGroupIndex];
                    }
                    else
                    {
                        GUI.DrawTexture(new Rect(position, new Vector2(_uiItemLength, _uiItemLength)), _textureRed);
                    }
                }
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
