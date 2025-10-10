using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName="Item")]
public class ItemType : ScriptableObject, IUnevenSelectable
{
    private const string TYPES_PATH = "Scriptables/ItemTypes";

    [SerializeField] private int _index;
    public int index => _index;

    [SerializeField] private Sprite _sprite;
    public Sprite sprite => _sprite;
    public Texture texture => _sprite.texture;

    [SerializeField] private string _name;
    public string typeName => _name;

    [SerializeField] private int _minSpawnNumber = 1;
    public int minSpawnNumber => _minSpawnNumber;

    [SerializeField] private int _maxSpawnNumber = 1;
    public int maxSpawnNumber => _maxSpawnNumber;

    [SerializeField] private int _spawnChance;
    public int spawnChance => _spawnChance;

    public float unevenSelectionWeight => _spawnChance;

    private void Reset()
    {
        ItemType[] allTypes = GetAll();
        _index = allTypes.Max(t => t._index) + 1;
    }

    private void OnValidate()
    {
        _name = name;
    }

    public static ItemType[] GetAll() => Resources.LoadAll<ItemType>(TYPES_PATH);
}