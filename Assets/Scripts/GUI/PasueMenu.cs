using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;

    private void Start() => Hide();

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            bool active = !_menu.activeSelf;
            _menu.SetActive(active);
            Time.timeScale = active ? 0 : 1;
            Cursor.lockState = active ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = active;
        }
    }

    public void Exit() => Application.Quit();

    public void ToLobby()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(0);
    }

    public void Reload()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.buildIndex);
    }

    public void Hide()
    {
        _menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}