using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InfoMessage : MonoBehaviour
{
    public string CurrentMessage;
    [SerializeField]
    private List<Message> messages = new List<Message>();

    public Text taskMessage;
    bool isFinished;
    private void Start()
    {
        StartCoroutine(ShowMessages());
    }

    public void SetIsFinished(bool value)
    {
        isFinished = value;
    }

    public void DisplayMessage(string message, float duration = 1F, bool priority = false)
    {
        if (!isFinished)
        {
            Color color = Color.white;
            if (!messages.Any(x => x.MessageString == message))
            {
                if (message.Contains(":"))
                {
                    color = Color.magenta;

                    if (priority)
                    {
                        StartCoroutine(ProrityMessage(taskMessage, message, duration, color));
                    }
                    else
                    {
                        messages.Add(new Message { MessageString = message, Duration = duration, Color = color, TextComponent = taskMessage });
                    }

                }
                else
                {
                    color = Color.white;
                    if (priority)
                    {
                        StartCoroutine(ProrityMessage(GetComponent<Text>(), message, duration, color));
                    }
                    else if (!messages.Any(x => x.MessageString == message))
                    {
                        messages.Add(new Message { MessageString = message, Duration = duration, Color = color, TextComponent = GetComponent<Text>() });
                    }

                }
            }
        }
    }

    private bool prioMessage;

    private IEnumerator ProrityMessage(Text text, string message, float duration, Color color)
    {
        prioMessage = true;
        text.text = message;
        text.color = color;
        CurrentMessage = message;

        yield return new WaitForSeconds(duration);

        CurrentMessage = "";
        text.text = "";
        prioMessage = false;
    }

    private IEnumerator ShowMessages()
    {
        for (; ; )
        {
            foreach (Message message in messages.ToList())
            {
                while (prioMessage)
                {
                    yield return new WaitForSeconds(0.5F);
                }
                message.TextComponent.text = message.MessageString;
                message.TextComponent.color = message.Color;
                CurrentMessage = message.MessageString;
                yield return new WaitForSeconds(message.Duration);
                CurrentMessage = "";
                message.TextComponent.text = "";
                messages.Remove(message);
            }
            yield return new WaitForSeconds(0.5F);
        }

    }

}
[System.Serializable]
internal class Message
{
    public string MessageString { get; set; }
    public float Duration { get; set; }
    public Color Color { get; set; }
    public Text TextComponent { get; set; }
}
