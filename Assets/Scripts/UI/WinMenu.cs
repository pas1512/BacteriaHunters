using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private PasueMenu _pause;

    private void Start()
    {
        _menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(GameWorld.bacterials <= 0 && !_menu.activeSelf)
        {
            Inventory.SaveStat();
            _pause.Hide();
            _menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Exit() => Application.Quit();

    public void Repeat()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.buildIndex);
    }
}