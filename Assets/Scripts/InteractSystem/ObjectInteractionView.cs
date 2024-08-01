using UnityEngine;

[CreateAssetMenu(fileName = "VisualDataConfig", menuName = "Game/Visual Data Config")]
public class ObjectInteractionView : ScriptableObject
{
    [SerializeField] private Color[] _outlineColors = new Color[2];
    [SerializeField, Range(0, 5)] private float _outlineSizeOnEnter;
    [SerializeField, Range(0, 5)] private float _outlineSizeOnSelect;
    public Color[] OutlineColors => _outlineColors;
    public float OutlineSizeOnEnter => _outlineSizeOnEnter;
    public float OutlineSizeOnSelect => _outlineSizeOnSelect;

    public enum OutlineType
    {
        MouseStay,
        Selected
    }
}
