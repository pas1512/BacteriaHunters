using UnityEngine;

[CreateAssetMenu(menuName = "SuppliesOffer", fileName = "Offer")]
public class SuppliesOfferData : ScriptableObject
{
    [SerializeField] private int _id;
    public int id => _id;

    [SerializeField] private ItemType _item;
    public ItemType item => _item;

    [SerializeField] private ItemType _priceItem;
    public ItemType priceItem => _priceItem;

    [SerializeField] private int _buyCount;
    public int buyCount => _buyCount;

    [SerializeField] private int _buyPrice;
    public int buyPrice => _buyPrice;

    [SerializeField] private bool _sellable;
    public bool sellable => _sellable;

    [SerializeField] private int _sellCount;
    public int sellCount => _sellCount;

    [SerializeField] private int _sellPrice;
    public int sellPrice => _sellPrice;
}