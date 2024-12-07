using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    private int currentRoom = 0;
    private bool inventoryChecked = false;
    private bool enemyDefeated = false;

    public GameObject inventoryUI;
    public GameObject enemy;
    
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
                ToastNotification.Show("Greetings, traveler! I am King Arthurth III, ruler of these lands. I've heard rumors that you seek fortune. Well, lucky for you, I have a proposition.", "info");
                ToastNotification.Show("Press 'E' to take stock of your belongings.", "info");
                break;

            case 1: 
                ToastNotification.Show("Here we are—the Trap Room. These rooms are common in the Tower, so prepare yourself each time you step through a doorway.", "info");
                ToastNotification.Show("Now, let’s talk combat. See that strange clock near your HP bar and the enemy’s HP bar? That’s a turn timer.", "info");
                ToastNotification.Show("Each card in your deck has a number in the top-left corner. That’s how much time it adds to your clock. Cards show effects like attack power, defense, range, or movement points.", "info");
                ToastNotification.Show("Defeat the enemy to proceed.", "info");
                if (enemy != null) enemy.SetActive(true); // Activate enemy
                break;

            case 2: 
                ToastNotification.Show("This is it—the entrance to the Tower. Within, you'll find rooms with portals. These portals allow you to ascend—or perhaps descend.", "info");
                ToastNotification.Show("What lies beyond each portal? Danger, treasure, or worse. Steel your resolve, traveler, for only the brave and cunning will thrive here.", "info");
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
        ToastNotification.Show("Congratulations! You’ve completed the tutorial. Now, go forth and conquer the Tower!", "info");
        SceneManager.LoadScene("Z01berg_1");
    }

    private void HandleSpawnRoom()
    {
        if (Input.GetKeyDown(KeyCode.E) && !inventoryChecked)
        {
            inventoryChecked = true;
            inventoryUI.SetActive(true); 
            ToastNotification.Show("Hmm... You're quite poor, aren't you? No matter. Take a look around your inventory.", "info");
        }
        else if (inventoryChecked && Input.GetKeyDown(KeyCode.Escape))
        {
            inventoryUI.SetActive(false); 
            ToastNotification.Show("Finished poking around? Good. Now, follow me down this corridor.", "info");
            ProceedToNextRoom();
        }
    }

    private void HandleTrapRoom()
    {
        if (enemy == null && !enemyDefeated) 
        {
            enemyDefeated = true;
            ToastNotification.Show("Well done! You’re already shaping up to be a fine tower diver. Let’s continue through the corridor.", "info");
            ProceedToNextRoom();
        }
    }

    private void HandleTowerEntrance()
    {
        if (Input.GetKeyDown(KeyCode.W)) 
        {
            ToastNotification.Show("This is the entrance to the Tower. Prepare yourself!", "info");
            CompleteTutorial();
        }
    }
    
}
