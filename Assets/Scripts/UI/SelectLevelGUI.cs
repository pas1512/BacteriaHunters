using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevelGUI : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    public bool active => _menu.activeSelf;

    public const string LEVEL = "LevelNumber";
    public static int GetLevelNumber() => PlayerPrefs.GetInt(LEVEL, 0);
    public static void SetLevelNumber(int number) => PlayerPrefs.SetInt(LEVEL, number);

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

    public void LoadLevel(int id)
    {
        Inventory.SaveStat();
        SetLevelNumber(id);
        SceneManager.LoadScene(1);
    }
}