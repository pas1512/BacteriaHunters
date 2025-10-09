using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GrammophoneGUI : MonoBehaviour
{
    private Inventory _invetory;
    private Grammophone _grammophone;

    [Header("Discs GUI")]
    [SerializeField] private DiscGUI _guiPrefab;
    [SerializeField] private RectTransform _container;

    [Header("Effects GUI")]
    [SerializeField] private RectTransform _rotor;
    [SerializeField] private RectTransform _progress;
    [SerializeField] private Sprite _emptyImage;
    [SerializeField] private Image _discImage;
    [SerializeField] private float _maxOffset;
    [SerializeField] private float _minOffset;

    private List<DiscGUI> _currentGUIs;
    private DiscGUI _currentPlayed;
    private DiscType[] _discs;
    private RectTransform _progressParent;

    public void Start()
    {
        _invetory = FindAnyObjectByType<Inventory>();
        _grammophone = FindAnyObjectByType<Grammophone>();
        _currentGUIs = new List<DiscGUI>();
        _progressParent = _progress.parent as RectTransform;
    }

    private void OnEnable()
    {
        StartCoroutine(DiscsSearching());
    }

    private void Update()
    {
        if (_currentPlayed != null && _grammophone.normalizedTime > 0)
        {
            _rotor.Rotate(0, 0, -Time.unscaledDeltaTime * 180);

            float width = _progressParent.rect.size.x;
            float max = -_maxOffset * width;
            float min = -_minOffset * width;

            Vector3 position = _progress.localPosition;
            position.x = Mathf.Lerp(max, min, _grammophone.normalizedTime);
            _progress.localPosition = position;
        }

        if (_discs == null || _currentGUIs == null)
            return;
            
        if(_currentPlayed != null && _invetory.GetCount(_currentPlayed.type) == 0)
            StopPlaing();

        if (_discs.Length != _currentGUIs.Count)
            Setup(_discs);
    }

    private IEnumerator DiscsSearching()
    {
        while(gameObject.activeSelf && enabled)
        {
            if(_invetory != null)
            {
                var allDiscs = _invetory.GetAll<DiscType>();
                _discs = allDiscs.Select(d => d.Item1).ToArray();
            }

            yield return null;
        }
    }

    public void StopPlaing()
    {
        if (_currentPlayed == null)
            return;

        _discImage.sprite = _emptyImage;

        _currentPlayed.SetState(false);
        _currentPlayed = null;

        _grammophone.Stop();
    }

    public void StartPlay(DiscGUI gui)
    {
        DiscGUI prev = _currentPlayed;
        StopPlaing();
        if (gui == prev)  return;

        DiscType type = gui.type;

        _discImage.sprite = type.sprite;

        _currentPlayed = gui;
        _currentPlayed.SetState(true);

        _grammophone.Play(type.clip);
    }

    public void Setup(DiscType[] allDiscs)
    {
        for (int i = _currentGUIs.Count - 1; i >= 0; i--)
            Destroy(_currentGUIs[i].gameObject);

        _currentGUIs.Clear();

        for (int i = 0; i < allDiscs.Length; i++)
        {
            var gui = Instantiate(_guiPrefab, _container);
            gui.Setup(this, allDiscs[i]);
            gui.gameObject.SetActive(true);
            _currentGUIs.Add(gui);
        }
    }
}
