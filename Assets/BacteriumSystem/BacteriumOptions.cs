using UnityEngine;

[CreateAssetMenu(menuName = "Bacterium Options")]
public class BacteriumOptions : ScriptableObject
{
    [SerializeField] private Bacterium _prefab;
    public Bacterium prefab => _prefab;

    [SerializeField] private Sprite _sprite;
    public Sprite sprite => _sprite;

    [SerializeField] private int _typeId;
    public int typeId => _typeId;

    [SerializeField] private float _size = 1;
    public float size => _size;

    [SerializeField] private float _lookArea = 5;
    public float lookArea => _lookArea;

    [SerializeField] private float _speed = 0.2f;
    public float speed => _speed;

    [SerializeField] private TargetsSet _targets = TargetsSet.clear;
    public TargetsSet targets => _targets;

    [SerializeField] private TargetsSet _enemies = TargetsSet.clear;
    public TargetsSet enemies => _enemies;

    [SerializeField] private Vector3 _boidsWheights;
    public Vector3 boidsWheights => _boidsWheights;

    [SerializeField] private Vector2 _pointsWheights;
    public Vector2 pointsWheights => _pointsWheights;

    [SerializeField] private Vector2 _navigationWheights;
    public Vector2 navigationWheights => _navigationWheights;
}