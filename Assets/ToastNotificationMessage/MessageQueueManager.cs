using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageQueueManager : MonoBehaviour
{
    private static MessageQueueManager _instance;

    private Queue<string> messageQueue = new Queue<string>();
    private bool isDisplaying = false;

    float waitTime = ToastNotificationTut.minimumMessageTime;
    private bool isPausedMessage = ToastNotificationTut.isStoped;

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

    private void Update()
    {
        checkForPause();
    }

    private void checkForPause()
    {
        if (isPausedMessage && isDisplaying)
        {
            StopCoroutine(DisplayMessages());
            isDisplaying = false; 
        }
        else if (!isPausedMessage && !isDisplaying && messageQueue.Count > 0)
        {
            StartCoroutine(DisplayMessages());
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

        if (!_instance.isDisplaying && !_instance.isPausedMessage)
        {
            _instance.StartCoroutine(_instance.DisplayMessages());
        }
    }


    private IEnumerator DisplayMessages()
    {
        if (isDisplaying) yield break; 

        isDisplaying = true;

        while (messageQueue.Count > 0)
        {
            string message = messageQueue.Dequeue(); 

            ToastNotificationTut.Show(message); 

            waitTime = ToastNotificationTut.minimumMessageTime; 

            while (waitTime > 0)
            {
                if (!isPausedMessage)
                {
                    waitTime -= Time.deltaTime;
                }
                yield return null;
            }
        }

        isDisplaying = false;
    }

}