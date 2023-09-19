using UnityEngine;
using UnityEngine.UI;

/*
 *Location: Main Scene, attached to "Controls"
 *Purpose: manage main menu UI
 */
public class MainMenuUI : MonoBehaviour
{
    public GameObject mainMenuStudent, mainMenuStaff, mainMenuProf, resourceReservationStaff, resourceReservationProf;
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
            Debug.Log("MainMenuUI closeMainMenu Student");
            mainMenuStudent.SetActive(false);
        }
        else if (mainMenuStaff.activeSelf == true)
        {
            Debug.Log("MainMenuUI closeMainMenu Staff");
            mainMenuStaff.SetActive(false);
        }
        else if (mainMenuProf.activeSelf == true) 
        {
            Debug.Log("MainMenuUI closeMainMenu Prof");
            mainMenuProf.SetActive(false); 
        }
        player.enabled = true;
    }

    /*
     * Purpose: To open the resourceReservation UI, attached to "resourceReservation" in "mainMenuUI"
     * Outcomes: activates resourceReservation and deactivates mainmenu
     */
    public void openResourceReservation()
    {
        if (mainMenuStaff.activeSelf == true)
        {
            Debug.Log("MainMenuUI openResourceReservation Staff");
            resourceReservationStaff.SetActive(true);
            mainMenuStaff.SetActive(false);
        } 
        else if (mainMenuProf.activeSelf == true)
        {
            Debug.Log("MainMenuUI openResourceReservation Prof");
            resourceReservationProf.SetActive(true);
            mainMenuProf.SetActive(false);
        }
    }
}
