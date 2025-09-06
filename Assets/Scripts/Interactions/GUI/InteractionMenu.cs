using UnityEngine;

public class InteractionMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    public bool active => _menu.activeSelf;

    private void Start() => Hide();

    public void Show()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _menu.SetActive(true);
    }

    public void Hide()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _menu.SetActive(false);
    }
}