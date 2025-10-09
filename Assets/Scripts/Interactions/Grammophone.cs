using System.Collections;
using UnityEngine;

public class Grammophone : ShowMenuInteratiable
{
    [SerializeField] private AudioSource _effectSource;
    [SerializeField] private AudioSource _music;

    private bool _isPlaying;

    public float normalizedTime => _music.clip == null ? 0 : _music.time / _music.clip.length;

    public void Stop()
    {
        _isPlaying = false;
        _effectSource.Stop();
        _music.clip = null;
        _music.Stop();
    }

    public void Play(AudioClip clip)
    {
        _isPlaying = true;
        StartCoroutine(PlayProcess(clip));
    }

    private IEnumerator PlayProcess(AudioClip clip)
    {
        clip.LoadAudioData();
        _music.clip = clip;

        yield return new WaitUntil(() => _music.clip.loadState == AudioDataLoadState.Loaded || !_isPlaying); 

        if(!_isPlaying)
            yield break;

        _music.Play();
        _effectSource.Play();

        float time = 0;
        float targetTime = clip.length;

        while(time < targetTime)
        {
            time += Time.unscaledDeltaTime;

            if(!_isPlaying)
                break;
            else
                yield return null;
        }

        _effectSource.Stop();
    }
}
