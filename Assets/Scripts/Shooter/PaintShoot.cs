using UnityEngine;

public class PaintShoot : MonoBehaviour
{
    [SerializeField] private int _selected;
    [SerializeField] private Color[] _colors = { new Color(0.8f, 0.2f, 0.0f), new Color(0.9f, 0.8f, 0.1f), new Color(0.3f, 0.6f, 0.1f), new Color(0.1f, 0.2f, 0.7f) };
    [SerializeField] private Vector2 _sizes = new Vector2(0.5f, 1);
    [SerializeField] private float _force = 100;
    [SerializeField] private float _maxSpread = 15;
    [SerializeField] private int _shotsCount = 16;
    [SerializeField] private Projectale _prefab;
    [SerializeField] private Inventory _inventory;
    [SerializeField] private ItemType[] _types;

    public int selected => _selected;
    public Color[] colors => _colors;

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
            _selected = 0;
        else if(Input.GetKeyDown(KeyCode.Alpha2)) 
            _selected = 1;
        else if( Input.GetKeyDown(KeyCode.Alpha3))
            _selected = 2;
        else if(Input.GetKeyDown(KeyCode.Alpha4))
            _selected = 3;

        ItemType type = _types[_selected];

        if (_inventory.GetCount(type) <= 0)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _inventory.Change(type, -1);

            for (int i = 0; i < _shotsCount; i++)
            {
                Vector2 offset = Random.insideUnitCircle;
                float distVal = offset.magnitude;
                offset *= _maxSpread;
                Vector3 direction = (Vector3)offset - new Vector3(0, 0, -10);
                float size = Mathf.Lerp(_sizes.x, _sizes.y, distVal);

                var proj = Instantiate(_prefab);
                proj.SetPallete(_colors);
                proj.Init(transform.position, transform.rotation * direction * _force, _selected, size);
            }
        }
    }
}
