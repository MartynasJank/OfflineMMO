using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChatUI : MonoBehaviour
{
    public Text chatText;

    void Start()
    {
        if (chatText != null)
            chatText.text = string.Empty;
        StartCoroutine(NPCChatter());
    }

    public void AddMessage(string sender, string message)
    {
        if (chatText == null) return;
        chatText.text += $"\n[{sender}] {message}";
    }

    IEnumerator NPCChatter()
    {
        yield return new WaitForSeconds(2f);
        AddMessage("NPC", "Welcome to the world!");
        yield return new WaitForSeconds(5f);
        AddMessage("NPC", "Let me know if you need anything.");
    }
}
