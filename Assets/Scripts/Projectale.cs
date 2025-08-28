using UnityEngine;

public class Projectale : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Renderer _rndr;
    [SerializeField] private float _maxTime;

    private float _time;
    private float _size;
    private int _color;
    private Vector3 _lastPosition;
    private Color[] _palette;

    public void SetPallete(Color[] colors) => _palette = colors;

    public void Init(Vector3 origin, Vector3 force, int color, float size)
    {
        transform.position = origin;
        _lastPosition = origin;

        _size = size;
        transform.localScale = Vector3.one * size;

        _rb.AddForce(force);

        _color = color;
        _rndr.sharedMaterial = new Material(_rndr.sharedMaterial);
        _rndr.sharedMaterial.color = _palette[color];
    }

    private void FixedUpdate()
    {
        Vector3 direction = transform.position - _lastPosition;
        _lastPosition = transform.position;

        if (Physics.Raycast(_lastPosition, direction.normalized, out var hit, direction.magnitude + 0.1f))
        {
            GameWorld.AddPaint(hit.point, hit.normal, _color, _size);
            Destroy(gameObject);
        }

        _time += Time.fixedDeltaTime;

        if(_time >= _maxTime)
            Destroy(gameObject);
    }
}