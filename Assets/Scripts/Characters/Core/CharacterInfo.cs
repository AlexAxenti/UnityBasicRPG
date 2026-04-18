using UnityEngine;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] private string characterName = "Character";
    [SerializeField] private FactionType factionType = FactionType.Neutral;

    public string CharacterName => characterName;
    public FactionType FactionType => factionType;
}

public enum FactionType
    {
        Player,
        Enemy,
        Friendly,
        Neutral
    }