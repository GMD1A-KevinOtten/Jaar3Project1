using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Message : MonoBehaviour {

    private struct MessageElement
    {
        public string message;
        public float duration;
    }

    public Text messageText;

    private Animator anim;
    private float timer;
    private bool isBusy;
    private List<MessageElement> messageQueue = new List<MessageElement>();

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    /// <summary>
    /// Shows a string message on the UI
    /// </summary>
    /// <param name="message"></param>
    /// <param name="showDuration"></param>
    public void ShowMessage(string message, float showDuration)
    {
        if (!isBusy)
        {
            PutMessageOnUI(message, showDuration);
        }
        else
        {
            messageQueue.Add(new MessageElement
            {
                message = message,
                duration = showDuration
            });
        }
    }

    private void PutMessageOnUI(string message, float showDuration)
    {
        messageText.text = message;
        timer = showDuration;

        anim.SetBool("ShowMessage", true);
        isBusy = true;
        StartCoroutine(MessageTimer());
    }
    private void CheckQueue()
    {
        if (messageQueue.Count > 0)
        {
            PutMessageOnUI(messageQueue[0].message, messageQueue[0].duration);
            messageQueue.RemoveAt(0);
        }
    }
    private IEnumerator MessageTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        anim.SetBool("ShowMessage", false);
        yield return new WaitForSeconds(1.5f);
        isBusy = false;
        CheckQueue();
    }
}
