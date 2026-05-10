using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/Dialogue")]
public class DialogueSO : ScriptableObject
{
    public Sprite defaultSprite;
    public List<DialogueLine> Lines;
}

[System.Serializable]
public class DialogueLine
{
    public Sprite expression;
    [TextArea(3, 10)]
    public string text;
}
