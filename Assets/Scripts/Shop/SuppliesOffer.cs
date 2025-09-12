using UnityEngine;
using UnityEngine.UI;

public class SuppliesOffer : MonoBehaviour
{
    [SerializeField] private SuppliesOfferData _data;
    [SerializeField] private ItemView _itemView;
    [SerializeField] private Image _buyImage;
    [SerializeField] private Text _buyCount;
    [SerializeField] private Image _sellImage;
    [SerializeField] private Text _sellCount;
    [SerializeField] private Button _buyButton;

    private SuppliesShop _shop;

    public void Setup(SuppliesShop shop, SuppliesOfferData data)
    {
        _shop = shop;
        _data = data;
        _itemView.Setup(data.sell);
        _buyImage.sprite = data.buy.sprite;
        _buyCount.text = data.buyCount.ToString();
        _sellImage.sprite = data.sell.sprite;
        _sellCount.text = data.sellCount.ToString();
        _buyButton.onClick.AddListener(Apply);
    }

    private void Update()
    {
        _buyButton.interactable = _shop.IsAviable(_data);
    }

    public void Apply()
    {
        _shop.Apply(_data);
    }
}