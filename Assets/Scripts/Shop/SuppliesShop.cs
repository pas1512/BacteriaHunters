using System.Linq;
using UnityEngine;

public class SuppliesShop : MonoBehaviour
{
    public const string DATAS_PATH = "Scriptables/Offers";
    [SerializeField] private Inventory _inventory;
    [SerializeField] private SuppliesOffer _prefab;
    [SerializeField] private RectTransform _offersContainer;

    private void Awake()
    {
        SuppliesOfferData[] datas = Resources.LoadAll<SuppliesOfferData>(DATAS_PATH);
        datas = datas.OrderBy(d => d.id).ToArray();

        for (int i = 0; i < datas.Length; i++)
        {
            SuppliesOffer offer = Instantiate(_prefab, _offersContainer);
            offer.Setup(this, datas[i]);
            offer.gameObject.SetActive(true);
        }
    }

    public void Buy(SuppliesOfferData data)
    {
        if (BuyAviable(data))
        {
            _inventory.Change(data.priceItem, -data.buyPrice);
            _inventory.Change(data.item, data.buyCount);
        }
    }

    public void Sell(SuppliesOfferData data)
    {
        if (SellAviable(data))
        {
            _inventory.Change(data.item, -data.sellCount);
            _inventory.Change(data.priceItem, data.sellPrice);
        }
    }

    public bool BuyAviable(SuppliesOfferData data)
    {
        return _inventory.GetCount(data.priceItem) >= data.buyCount;
    }

    public bool SellAviable(SuppliesOfferData data)
    {
        return data.sellable && _inventory.GetCount(data.item) >= data.sellCount;
    }
}