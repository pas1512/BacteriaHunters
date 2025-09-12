using UnityEngine;

public class Grammophone : Interactable
{
    [SerializeField] private AudioClip[] _clips;
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioSource _music;

    private bool _isPlaying;

    public override string actionName => _isPlaying ? "Вимкнути" : "Увімкнути";

    public override bool CanInteract() => true;

    protected override void BreakAction(Interactor interactor) { }

    protected override void InteractAction(Interactor interactor)
    {
        _isPlaying = !_isPlaying;

        if(_isPlaying)
        {
            int randomId = Random.Range(0, _clips.Length);
            _music.clip = _clips[randomId];
            _effectSource.Play();
            _music.Play();
        }
        else
        {
            _effectSource.Stop();
            _music.Stop();
        }

        Break(interactor);
    }
}
