using UnityEngine;
using UnityEngine.UI;

public class LineBar : MonoBehaviour
{
    [SerializeField] private Color _fullColor = Color.white;
    [SerializeField] private Color _emptyColor = Color.white;
    public Color fullColor => _fullColor;
    public Color emptyColor => _emptyColor;

    [SerializeField] private Color _backgroundColor = Color.gray;
    public Color backgroundColor => _backgroundColor;

    [Range(0, 1)][SerializeField] private float _value;
    public float value => _value;

    [SerializeField] private Image _fill;
    [SerializeField] private Image _background;

    private float _maxWidth;

    private void OnValidate()
    {
        _maxWidth = (transform as RectTransform).rect.width;
        SetBackgroundColor(backgroundColor);
        SetFillColors(fullColor, emptyColor);
        SetValue(value);
    }

    private void Start() => OnValidate();

    private void Reset()
    {
        int childsCound = transform.childCount;

        if (childsCound >= 1)
        {
            var bg = transform.Find("Background");
            _background = bg?.GetComponent<Image>();


            if (_background == null)
                _background = transform.GetChild(0).GetComponent<Image>();
        }

        if (childsCound >= 2)
        {
            var fl = transform.Find("Fill");
            _fill = fl?.GetComponent<Image>();


            if (_fill == null)
                _fill = transform.GetChild(1).GetComponent<Image>();
        }

        OnValidate();
    }

    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        _value = value;
        float right = _maxWidth * (1 - _value);
        _fill.rectTransform.offsetMax = new Vector2(-right, _fill.rectTransform.offsetMax.y);
        _fill.color = Color.Lerp(_emptyColor, _fullColor, _value);
    }

    public void SetFillColors(Color full, Color empty)
    {
        _emptyColor = empty;
        _fullColor = full;
        _fill.color = Color.Lerp(empty, full, _value);
    }

    public void SetBackgroundColor(Color color)
    {
        _backgroundColor = color;
        _background.color = color;
    }
}