using UnityEngine;
using UnityEngine.UI;

public class ItemView : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    [SerializeField] private Image _item;
    [SerializeField] private Text _count;
    private Inventory _invetory;

    public void Setup(ItemType type)
    {
        _type = type;
    }

    private void Start()
    {
        _invetory = FindAnyObjectByType<Inventory>();
        _item.sprite = _type.sprite;
    }

    private void Update()
    {
        _count.text = _invetory.GetCount(_type).ToString();
    }
}