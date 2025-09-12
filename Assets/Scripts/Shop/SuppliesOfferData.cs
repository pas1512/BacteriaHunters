using UnityEngine;

[CreateAssetMenu(menuName = "SuppliesOffer", fileName = "Offer")]
public class SuppliesOfferData : ScriptableObject
{
    [SerializeField] private ItemType _sell;
    public ItemType sell => _sell;

    [SerializeField] private int _sellCount;
    public int sellCount => _sellCount;

    [SerializeField] private ItemType _buy;
    public ItemType buy => _buy;

    [SerializeField] private int _buyCount;
    public int buyCount => _buyCount;
}