using UnityEngine;

[CreateAssetMenu(menuName = "Plate")]
public class DiscType : ItemType
{
    [SerializeField] private AudioClip _clip;
    public AudioClip clip => _clip;
}