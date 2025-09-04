using UnityEngine;

public class HealthCounter : MonoBehaviour
{
    [SerializeField] private LineBar _lineBar;
    [SerializeField] private Health _health;
    public void Update() => _lineBar.SetValue(_health.value);
}
