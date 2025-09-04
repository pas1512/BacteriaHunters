using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField] private KeyCode _key = KeyCode.L;
    [SerializeField] private KeyCode _realoadKey = KeyCode.G;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemType _reloadType;
    [SerializeField] private Light _light;
    [SerializeField] private float _max = 1;
    [SerializeField] private float _current = 1;
    [SerializeField] private float _speed = 0.001f;

    public float value => _light.intensity / _max;
    private bool _enabled;

    private void Start()
    {
        _light.intensity = _max;
        _current = _max;
        _enabled = _light.enabled;
    }

    void Update()
    {
        if(Input.GetKeyDown(_key))
            _enabled = !_enabled;

        if (Input.GetKeyDown(_realoadKey) &&
            _inventory.GetCount(_reloadType) > 0)
        {
            _inventory.Change(_reloadType, -1);
            _current = _max;
        }

        if (_enabled)
        {
            _current -= _speed * Time.deltaTime;
            _light.intensity = _current;
        }
        else
        {
            _light.intensity = 0;
        }
    }
}