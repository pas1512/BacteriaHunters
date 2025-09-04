using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private KeyCode _treatKey;
    [SerializeField] private ItemType _treatType;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private Flashlight _light;
    [SerializeField] private float _loseSpeed;
    [SerializeField] private AnimationCurve _loseCurve;
    [SerializeField] private float _max = 100;
    [SerializeField] private float _current = 100;
    public float value => _current / _max;

    private void Update()
    {
        if (Input.GetKeyDown(_treatKey) &&
            _inventory.GetCount(_treatType) > 0 )
        {
            _inventory.Change(_treatType, -1);
            _current = _max;
        }

        _current -= _loseCurve.Evaluate(_light.value) * _loseSpeed * Time.deltaTime;
    }
}
