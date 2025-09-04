using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueMenu : MonoBehaviour
{
    public const string LEVEL = "LevelNumber";

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

    public static int GetLevelNumber() => PlayerPrefs.GetInt(LEVEL, 0);
    public static void SetLevelNumber(int number) => PlayerPrefs.SetInt(LEVEL, number);
    public void Exit() => Application.Quit();

    public void Reload()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        var level = SceneManager.GetActiveScene();
        SceneManager.LoadScene(level.buildIndex);
    }

    public void NextLevel()
    {
        int level = PlayerPrefs.GetInt(LEVEL);
        SetLevelNumber(level + 1);
        Reload();
    }

    public void Hide()
    {
        _menu.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}