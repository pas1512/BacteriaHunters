using UnityEngine;

[CreateAssetMenu(menuName = "Interaction")]
public class InteractionData : ScriptableObject
{
    [SerializeField] private Sprite _sprite;
    public Sprite sprite => _sprite;

    [SerializeField] private bool _continued;
    public bool continued => _continued;

    [SerializeField] private bool _cancleable;
    public bool cancleable => _cancleable;

    [SerializeField] private string _actionName;
    public string actionName => _actionName;

    [SerializeField] private bool _allowTrigger;
    public bool allowTrigger => _allowTrigger;
}