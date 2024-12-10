using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private static int currentRoom = 0;
    private static bool inventoryChecked = false;
    private static bool enemyDefeated = false;
    
    private void Start()
    {
        EnterRoom(currentRoom);
    }

    private void Update()
    {
        switch (currentRoom)
        {
            case 0:
                HandleSpawnRoom();
                break;

            case 1: 
                HandleTrapRoom();
                break;

            case 2: 
                HandleTowerEntrance();
                break;
        }
    }

    private void EnterRoom(int roomIndex)
    {
        switch (roomIndex)
        {
            case 0: 
                MessageQueueManager.ShowMessage("Greetings, traveler! I am King Arthurth III, ruler of these lands. I've heard rumors that you seek fortune. Well, lucky for you, I have a proposition.");
                MessageQueueManager.ShowMessage("Press 'E' to take stock of your belongings.");
                break;

            case 1: 
                MessageQueueManager.ShowMessage("Here we are—the Trap Room. These rooms are common in the Tower, so prepare yourself each time you step through a doorway.");
                MessageQueueManager.ShowMessage("Now, let’s talk combat. See that strange clock near your HP bar and the enemy’s HP bar? That’s a turn timer.");
                MessageQueueManager.ShowMessage("Each card in your deck has a number in the top-left corner. That’s how much time it adds to your clock. Cards show effects like attack power, defense, range, or movement points.");
                MessageQueueManager.ShowMessage("LEFT-MOUSE button to select card RIGHT-MOUSE button to unselect card, and LEFT-MOUSE button on grid to use a card");
                MessageQueueManager.ShowMessage("After you played your card click button END TURN to the right or just hit ENTER");
                MessageQueueManager.ShowMessage("Defeat the enemy to proceed.");
                break;

            case 2: 
                MessageQueueManager.ShowMessage("This is it—the entrance to the Tower. Within, you'll find rooms with portals. These portals allow you to ascend—or perhaps descend.");
                MessageQueueManager.ShowMessage("What lies beyond each portal? Danger, treasure, or worse. Steel your resolve, traveler, for only the brave and cunning will thrive here.");
                break;
        }
    }

    public void ProceedToNextRoom()
    {
        currentRoom++;
        EnterRoom(currentRoom);
    }

    private void CompleteTutorial()
    {
        MessageQueueManager.ShowMessage("Congratulations! You’ve completed the tutorial. Now, go forth and conquer the Tower!");
    }

    private void HandleSpawnRoom()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inventoryChecked)
        {
            inventoryChecked = true;
            MessageQueueManager.ShowMessage("Hmm... You're quite poor, aren't you? No matter. Take a look around your inventory.");
        }
        else if (inventoryChecked && Input.GetKeyDown(KeyCode.E))
        {
            inventoryChecked = false;
            MessageQueueManager.ShowMessage("Finished poking around? Good. Now, follow me down this corridor. (use LEFT-MOUSE BUTTON)");
            ProceedToNextRoom();
        }
    }

    private void HandleTrapRoom()
    {
        if (enemyDefeated) 
        {
            enemyDefeated = true;
            MessageQueueManager.ShowMessage("Well done! You’re already shaping up to be a fine tower diver. Let’s continue through the corridor. (use LEFT-MOUSE BUTTON)");
            MessageQueueManager.ShowMessage("Remember after clearing the room you get new item to inventory. (press E)");
            ProceedToNextRoom();
        }
    }

    private void HandleTowerEntrance()
    {
            MessageQueueManager.ShowMessage("This is the entrance to the Tower. Prepare yourself! (enter inside of portal)");
            CompleteTutorial();
    }
    
}
