using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueueManager : MonoBehaviour
{
    private static MessageQueueManager _instance;

    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplaying = false;

    float waitTime = ToastNotificationTut.minimumMessageTime;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static void ShowMessage(string message)
    {
        if (_instance == null)
        {
            Debug.LogError("MessageQueueManager instance not found in the scene!");
            return;
        }

        _instance.messageQueue.Enqueue(message);

        if (!_instance.isDisplaying)
        {
            _instance.StartCoroutine(_instance.DisplayMessages());
        }
    }

    private IEnumerator DisplayMessages()
    {
        isDisplaying = true;

        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue();

            ToastNotificationTut.Show(message);

            yield return new WaitForSeconds(waitTime);
        }

        isDisplaying = false;
    }
}