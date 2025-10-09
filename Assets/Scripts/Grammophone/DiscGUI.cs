using UnityEngine;
using UnityEngine.UI;

public class DiscGUI : MonoBehaviour
{
    [SerializeField] private DiscType _disc;
    [SerializeField] private Button _button;
    [SerializeField] private Image _state;
    [SerializeField] private Image _image;
    [SerializeField] private Text _name;
    [SerializeField] private bool _played;
    private GrammophoneGUI _owner;

    public DiscType type => _disc;

    private void OnValidate() => UpdateView();

    public void StartPlay() => _owner.StartPlay(this);

    public void SetState(bool played)
    {
        _played = played;
        UpdateView();
    }

    public void UpdateView()
    {
        if (_disc != null)
        {
            _state.enabled = _played;
            _image.enabled = true;
            _image.sprite = _disc.sprite;
            _name.text = _disc.name;
        }
        else
        {
            _state.enabled = false;
            _image.enabled = false;
            _name.text = "";
        }
    }

    public void Setup(GrammophoneGUI owner, DiscType disc)
    {
        _owner = owner;
        _disc = disc;
        UpdateView();
        _button.onClick.AddListener(StartPlay);
    }
}