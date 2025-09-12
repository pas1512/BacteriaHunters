using UnityEngine;

[CreateAssetMenu(menuName = "LevelParameters", fileName = "Level")]
public class LevelParameters : ScriptableObject
{
    [SerializeField] private MinMaxValue _bacteriaCount = new MinMaxValue(100, 500);
    [SerializeField] private BacteriumData[] _bacteriaTypes;
    [SerializeField] private Reward[] _rewards;
    [SerializeField] private RoomParameters _room;
    [SerializeField] private Material _floor;
    [SerializeField] private Material _walls;
    [SerializeField] private Material _ceil;

    public int GetBacteriaCount() => _bacteriaCount.GetRandomInt();
    public BacteriumData[] bacterium => _bacteriaTypes;

    public RoomParameters room => _room;
    public Material[] GetMaterials() => new Material[] { _floor, _walls, _ceil };

    public Reward GetReavard()
    {
        UnevenSelector<Reward> selector = new UnevenSelector<Reward>(_rewards);
        Reward selected = selector.GetRandom();
        return selected.GetNewReward();
    }
}