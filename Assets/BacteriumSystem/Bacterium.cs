using UnityEngine;

public class Bacterium : MonoBehaviour
{
    [SerializeField] private BacteriumOptions _options;
    public BacteriumOptions data => _options;

    [SerializeField] private MeshRenderer _renderer;

    public int typeId => _options.typeId;
    public float size => _options.size;
    public float lookArea => _options.lookArea;
    public float speed => _options.speed;
    public TargetsSet targets => _options.targets;
    public TargetsSet enemies => _options.enemies;
    public Vector3 boidsWheights => _options.boidsWheights;
    public Vector2 pointsWheights => _options.pointsWheights;
    public Vector2 navigationWheights => _options.navigationWheights;

    private Vector3 _velocity;
    public Vector3 velcity => _velocity;

    private void Start()
    {
        _renderer.sharedMaterial = new Material(_renderer.sharedMaterial);
        _renderer.sharedMaterial.mainTexture = _options.sprite.texture;
        _velocity = Random.insideUnitSphere * speed;
    }

    public void SetData(BacteriumOptions options)
    {
        _options = options;
        _renderer.sharedMaterial.mainTexture = _options.sprite.texture;
    }

    public void ReturnData(BacteriumData data)
    {
        _velocity = data.velocity;
    }
}