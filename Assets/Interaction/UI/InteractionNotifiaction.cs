using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class InteractionNotifiaction : MonoBehaviour
{
    [SerializeField] private float _time = 0.5f;
    [SerializeField] private AnimationCurve _curve;
    private static InteractionNotifiaction _instance;
    private Text _text;
    private Color _color;

    private void Awake()
    {
        gameObject.SetActive(false);
        _text = GetComponent<Text>();
        _color = _text.color;
        _instance = this;
    }

    public static void Show(string text)
    {
        _instance._text.text = text;
        _instance.gameObject.SetActive(true);
        _instance.StartCoroutine(_instance.DisableProcess());
    }

    public static void Show(string text, Color color)
    {
        _instance._text.color = color;
        Show(text);
    }

    private IEnumerator DisableProcess()
    {
        Color color = _text.color;

        for (float i = 0; (_time - i) >= 0; i += Time.deltaTime)
        {
            float val = i / _time;
            color.a = _curve.Evaluate(val);
            _text.color = color;
            yield return null;
        }

        gameObject.SetActive(false);
        _text.color = _color;
    }
}