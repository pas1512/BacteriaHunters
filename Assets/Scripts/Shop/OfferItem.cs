using UnityEngine;
using UnityEngine.UI;

public class OfferItem : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private Text _number;

    private void Reset()
    {
        if(_image == null)
            _image = GetComponentInChildren<Image>();

        if(_number == null)
            _number = GetComponentInChildren<Text>();
    }

    public void Setup(ItemType type, int number)
    {
        _image.sprite = type.sprite;
        _number.text = number.ToString();
    }
}