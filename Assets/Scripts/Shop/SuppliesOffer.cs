using UnityEngine;
using UnityEngine.UI;

public class SuppliesOffer : MonoBehaviour
{
    [SerializeField] private SuppliesOfferData _data;
    [SerializeField] private ItemView _itemView;
    [SerializeField] private OfferItem _buyItem;
    [SerializeField] private OfferItem _buyPrice;
    [SerializeField] private OfferItem _sellItem;
    [SerializeField] private OfferItem _sellPrice;
    [SerializeField] private Button _buyButton;
    [SerializeField] private Button _sellButton;

    private SuppliesShop _shop;

    public void Setup(SuppliesShop shop, SuppliesOfferData data)
    {
        _shop = shop;
        _data = data;
        _itemView.Setup(data.item);
        _buyButton.onClick.AddListener(Buy);
        _sellButton.onClick.AddListener(Sell);
        _buyButton.interactable = _shop.BuyAviable(_data);
        _sellButton.interactable = _shop.SellAviable(_data);

        _buyItem.Setup(data.item, data.buyCount);
        _buyPrice.Setup(data.priceItem, data.buyPrice);
        _sellItem.Setup(data.item, data.sellCount);
        _sellPrice.Setup(data.priceItem, data.sellPrice);
    }

    private void Update()
    {
        _buyButton.interactable = _shop.BuyAviable(_data);
        _sellButton.interactable = _shop.SellAviable(_data);
    }

    public void Buy() => _shop.Buy(_data);
    public void Sell() => _shop.Sell(_data);
}