using UnityEngine;

public class FlashlightCounter : MonoBehaviour
{
    [SerializeField] private LineBar _lineBar;
    [SerializeField] private Flashlight _light;
    private void Update() => _lineBar.SetValue(_light.value);
}
