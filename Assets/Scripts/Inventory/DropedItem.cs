using UnityEngine;

public class DropedItem : MonoBehaviour
{
    [SerializeField] private ItemType _type;
    public ItemType type => _type;

    [SerializeField] private int _number;
    public int number => _number;

    [SerializeField] private MeshRenderer[] _renderers;
    [SerializeField] private float _size = 1;
    [SerializeField] private float _speed = 4f;

    private void Start()
    {
        Init(_type, _number);
    }

    public void Init(ItemType type, int number)
    {
        _type = type;
        _number = number;

        Material material = new Material(_renderers[0].sharedMaterial);
        material.mainTexture = _type.texture;

        for (int i = 0; i < _renderers.Length; i++)
            _renderers[i].sharedMaterial = material;

        transform.localScale = new Vector3(_size, _size, transform.localScale.z);
    }

    public void SetupPosition(Vector3 point, Vector3 normal, float distance)
    {
        float dist = _size + distance;
        transform.position = point + (normal * dist);
    }

    private void Update()
    {
        transform.Rotate(0, _speed * Time.deltaTime, 0);
        var colliders = Physics.OverlapSphere(transform.position, _size);

        if (colliders == null || colliders.Length <= 0)
            return;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].TryGetComponent(out Inventory inventory))
            {
                inventory.Add(this);
                Destroy(gameObject);
                break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _size);
    }
}