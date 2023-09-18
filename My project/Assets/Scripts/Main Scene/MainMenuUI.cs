using UnityEngine;
using UnityEngine.UI;

/*
 *Location: Main Scene, attached to "StudentControls"
 *Purpose: manage main menu UI
 */
public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuOthers, resourceReservation;
    public Text identity;
    public GameObject addslots;
    public PlayerControls player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
     * Purpose: To close the main menu UI, attached to "closeMainMenuButton"
     * Outcomes: deactivate main menu
     */
    public void closeMainMenu()
    {
        if (mainMenuStudent.activeSelf == true)
        {
            mainMenuStudent.SetActive(false);
        }
        else if (mainMenuOthers.activeSelf == true)
        {
            mainMenuOthers.SetActive(false);
        }
        player.enabled = true;
    }

    public void openResourceReservation()
    {
        if (resourceReservation.activeSelf == false)
        {
            resourceReservation.SetActive(true);
            mainMenuOthers.SetActive(false);
        }
        if (identity.text == "Staff")
        {
            addslots.SetActive(true);
        } else
        {
            addslots.SetActive(false);
        }
    }
}
