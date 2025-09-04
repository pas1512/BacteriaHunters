using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class ImagedLabel : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _text;
    [SerializeField] private float _verticalPadding = 5;
    [SerializeField] private float _horizontalPadding = 5;

    [SerializeField] private Image _image;
    [SerializeField] private RectTransform _imageRect;
    
    [SerializeField] private Text _textUI;
    [SerializeField] private RectTransform _textRect;

    private RectTransform _labelRect;

    public string text
    {
        get => _textUI.text;
        set { _textUI.text = value; _text = value; }
    }

    public Sprite sprite
    {
        get => _image.sprite;
        set { _image.sprite = value; _sprite = value; }
    }

    private void Awake()
    {
        _labelRect = GetComponent<RectTransform>();

        if(_image != null)
        {
            _image.preserveAspect = true;
            _imageRect = _image.rectTransform;
        }

        _imageRect.anchorMin = new Vector2(0, 0.5f);
        _imageRect.anchorMax = new Vector2(0, 0.5f);

        if (_textUI != null)
        {
            _textUI.alignment = TextAnchor.MiddleLeft;
            _textUI.resizeTextForBestFit = true;
            _textUI.resizeTextMaxSize = 300;
            _textRect = _textUI.rectTransform;
        }

        _textRect.anchorMin = new Vector2(0, 0.5f);
        _textRect.anchorMax = new Vector2(0, 0.5f);
    }

    private void OnValidate()
    {
        if(_imageRect == null || _labelRect == null || _textRect == null)
            Awake();

        UpdateImage();
        UpdateText();

        if (_image != null)
            _image.sprite = _sprite;

        if(_textUI != null)
            _textUI.text = _text;
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_imageRect == null || _labelRect == null || _textRect == null)
            Awake();

        UpdateImage();
        UpdateText();
    }

    private void UpdateImage()
    {
        float height = _labelRect.rect.height;
        float margin = height * (_verticalPadding / 100);
        float side = height - margin * 2;

        _imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, side);
        _imageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, side);
        _imageRect.anchoredPosition = new Vector2(side / 2f + _horizontalPadding, 0);
    }

    private void UpdateText()
    {
        float width = _labelRect.rect.width;
        float leftPadding = _horizontalPadding * 2 + _imageRect.rect.width;
        float restWidth = width - leftPadding - _horizontalPadding;

        _textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, restWidth);
        _textRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _imageRect.rect.height);
        _textRect.anchoredPosition = new Vector2(restWidth / 2 + leftPadding, 0);
    }
}