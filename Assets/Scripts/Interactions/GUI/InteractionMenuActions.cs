using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionMenuActions : MonoBehaviour
{
    public const string LEVEL = "LevelNumber";
    public static int GetLevelNumber() => PlayerPrefs.GetInt(LEVEL, 0);
    public static void SetLevelNumber(int number) => PlayerPrefs.SetInt(LEVEL, number);

    public void LoadLevel(int id)
    {
        Inventory.SaveStat();
        SetLevelNumber(id);
        SceneManager.LoadScene(1);
    }

    public void Quit() => Application.Quit();
}