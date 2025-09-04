using UnityEngine;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private PaintShoot _shoots;
    [SerializeField] private ColorCell[] _cells;

    private void Update()
    {
        for (int i = 0; i < _cells.Length; i++)
        {
            if(_shoots.selected == i)
                _cells[i].Select();
            else
                _cells[i].Unselect();
        }
    }
}