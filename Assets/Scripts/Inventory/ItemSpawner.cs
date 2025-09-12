using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private int _attempts;
    [SerializeField] private DropedItem _prefab;
    [SerializeField] private ItemType[] _types;
    private UnevenSelector<ItemType> _selector;
    private static ItemSpawner _instance;

    private void Start()
    {
        _selector = new UnevenSelector<ItemType>(_types);
        _instance = this;
    }

    public static void TrySpawnStat(Vector3 position, Vector3 normal)
    {
        _instance.TrySpawn(position, normal);
    }

    public void TrySpawn(Vector3 position, Vector3 normal)
    {
        if (Random.Range(0, _attempts) == 0)
        {
            var created = Instantiate(_prefab);
            created.SetupPosition(position, normal, 0.25f);

            ItemType type = _selector.GetRandom();
            int number = Random.Range(type.minSpawnNumber, type.maxSpawnNumber + 1);
            created.Init(type, number);
        }
    }
}