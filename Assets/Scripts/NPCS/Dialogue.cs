using UnityEngine;

[CreateAssetMenu(fileName = "NPC_Dialouge", menuName = "Scriptable Objects/NPC_Dialouge")]
public class Dialogue : ScriptableObject
{
    [TextArea]
    public string[] dialogue;
}
