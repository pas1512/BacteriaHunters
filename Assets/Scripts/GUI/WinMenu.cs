using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private GameObject _menu;
    [SerializeField] private PasueMenu _pause;
    [SerializeField] private Image _rewardImage;
    [SerializeField] private Text _rewardNumber;
    [SerializeField] private Inventory _inventory;

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
            Reward reward = LevelIniter.reward;

            _rewardNumber.text = reward.count.ToString();
            _rewardImage.sprite = reward.itemType.sprite;
            _inventory.Change(reward.itemType, reward.count);
            Inventory.SaveStat();

            _pause.Hide();
            _menu.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ToLobby()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.LoadScene(0);
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