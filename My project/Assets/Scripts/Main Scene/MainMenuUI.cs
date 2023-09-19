using UnityEngine;
using UnityEngine.UI;

/*
 *Location: Main Scene, attached to "StudentControls"
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
            mainMenuStudent.SetActive(false);
        }
        else if (mainMenuStaff.activeSelf == true)
        {
            mainMenuStaff.SetActive(false);
        }
        else if (mainMenuProf.activeSelf == true) 
        { 
            mainMenuProf.SetActive(false); 
        }
        player.enabled = true;
    }

    public void openResourceReservation()
    {
        if (mainMenuStaff.activeSelf == true)
        {
            Debug.Log("openResourceReservation Staff");
            resourceReservationStaff.SetActive(true);
            mainMenuStaff.SetActive(false);
        } 
        else if (mainMenuProf.activeSelf == true)
        {
            Debug.Log("openResourceReservation Prof");
            resourceReservationProf.SetActive(true);
            mainMenuProf.SetActive(false);
        }
    }
}
