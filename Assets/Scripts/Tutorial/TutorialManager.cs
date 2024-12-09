using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private int currentRoom = 0;
    private bool inventoryChecked = false;
    private bool enemyDefeated = false;
    
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
                ToastNotificationTut.Show("Greetings, traveler! I am King Arthurth III, ruler of these lands. I've heard rumors that you seek fortune. Well, lucky for you, I have a proposition.");
                ToastNotificationTut.Show("Press 'E' to take stock of your belongings.");
                break;

            case 1: 
                ToastNotificationTut.Show("Here we are—the Trap Room. These rooms are common in the Tower, so prepare yourself each time you step through a doorway.");
                ToastNotificationTut.Show("Now, let’s talk combat. See that strange clock near your HP bar and the enemy’s HP bar? That’s a turn timer.");
                ToastNotificationTut.Show("Each card in your deck has a number in the top-left corner. That’s how much time it adds to your clock. Cards show effects like attack power, defense, range, or movement points.");
                ToastNotificationTut.Show("LEFT-MOUSE button to select card RIGHT-MOUSE button to unselect card, and LEFT-MOUSE button on grid to use a card");
                ToastNotificationTut.Show("After you played your card click button END TURN to the right or just hit ENTER");
                ToastNotificationTut.Show("Defeat the enemy to proceed.");
                //if (enemy != null) enemy.SetActive(true); // Activate enemy
                break;

            case 2: 
                ToastNotificationTut.Show("This is it—the entrance to the Tower. Within, you'll find rooms with portals. These portals allow you to ascend—or perhaps descend.");
                ToastNotificationTut.Show("What lies beyond each portal? Danger, treasure, or worse. Steel your resolve, traveler, for only the brave and cunning will thrive here.");
                break;
        }
    }

    private void ProceedToNextRoom()
    {
        currentRoom++;
        EnterRoom(currentRoom);
    }

    private void CompleteTutorial()
    {
        ToastNotificationTut.Show("Congratulations! You’ve completed the tutorial. Now, go forth and conquer the Tower!");
        SceneManager.LoadScene("Z01berg_1");
    }

    private void HandleSpawnRoom()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inventoryChecked)
        {
            inventoryChecked = true;
            ToastNotificationTut.Show("Hmm... You're quite poor, aren't you? No matter. Take a look around your inventory.");
        }
        else if (inventoryChecked && Input.GetKeyDown(KeyCode.E))
        {
            ToastNotificationTut.Show("Finished poking around? Good. Now, follow me down this corridor. (use LEFT-MOUSE BUTTON)");
            ProceedToNextRoom();
        }
    }

    private void HandleTrapRoom()
    {
        if (enemyDefeated) 
        {
            enemyDefeated = true;
            ToastNotificationTut.Show("Well done! You’re already shaping up to be a fine tower diver. Let’s continue through the corridor. (use LEFT-MOUSE BUTTON)");
            ToastNotificationTut.Show("Remember after clearing the room you get new item to inventory. (press E)");
            ProceedToNextRoom();
        }
    }

    private void HandleTowerEntrance()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            ToastNotificationTut.Show("This is the entrance to the Tower. Prepare yourself! (enter inside of portal)");
            CompleteTutorial();
        }
    }
    
}
