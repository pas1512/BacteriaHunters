using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private PasueMenu _pause;
    private Health _health;

    private void Start()
    {
        _health = FindAnyObjectByType<Health>();
        _menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(_health.value <= 0 && !_menu.activeSelf)
        {
            _pause.Hide();
            _menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

    public void Repeat()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.buildIndex);
    }
}