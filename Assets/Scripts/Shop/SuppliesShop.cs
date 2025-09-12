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

        for (int i = 0; i < datas.Length; i++)
        {
            SuppliesOffer offer = Instantiate(_prefab, _offersContainer);
            offer.Setup(this, datas[i]);
            offer.gameObject.SetActive(true);
        }
    }

    public void Apply(SuppliesOfferData data)
    {
        if (IsAviable(data))
        {
            _inventory.Change(data.buy, -data.buyCount);
            _inventory.Change(data.sell, data.sellCount);
        }
    }

    public bool IsAviable(SuppliesOfferData data)
    {
        try
        {
            return _inventory.GetCount(data.buy) >= data.buyCount;
        }
        catch
        {
            print($"inventory: {_inventory}, data: {data}");
            return false;
        }
    }
}