using UnityEngine;

public class BacterialCounter : MonoBehaviour
{
    [SerializeField] private ImagedLabel _text;
    void Update() => _text.text = GameWorld.bacterials.ToString();
}