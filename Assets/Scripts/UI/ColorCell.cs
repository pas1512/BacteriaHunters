using UnityEngine;
using UnityEngine.UI;

public class ColorCell : MonoBehaviour
{
    [SerializeField] private Image _bckg;
    [SerializeField] private Color _normal;
    [SerializeField] private Color _active;

    public void Select() => _bckg.color = _active;
    public void Unselect() => _bckg.color = _normal;
}