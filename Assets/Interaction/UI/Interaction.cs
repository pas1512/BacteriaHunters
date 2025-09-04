using UnityEngine;
using UnityEngine.UI;

public class Interaction: MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _text;
    [SerializeField] private InteractionSelectionPointer _pointer;
    [SerializeField] private Color _active = Color.white;
    [SerializeField] private Color _inactive = Color.white * 0.7f;

    public void Init(string name, Sprite image = null)
    {
        _text.text = name;

        if(_image != null)
            _image.sprite = image;
    }

    public void SetActive(bool active)
    {
        if (active)
        {
            _text.color = _active;
            _image.color = _active;
        }
        else
        {
            _text.color = _active * _inactive;
            _image.color = _inactive;
        }
    }

    public void Select()
    {
        _pointer.gameObject.SetActive(true);
    }

    public void Unselect()
    {
        _pointer.gameObject.SetActive(false);
    }
}