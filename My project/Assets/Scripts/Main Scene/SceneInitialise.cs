using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/*
 * Location: Main Scene/ MainSceneControls
 * Purpose: Deactivate the UIs when user enters "MainScene" scene
 */

public class SceneInitialise : MonoBehaviour
{
    public Camera mainCamera;
    public GameObject buildingFirstPerson, middlewall;
    public GameObject bed, wardrobe, table, door, player, mainSceneCanvas;
    public GameObject logoutUI, userProfileStudentUI, userProfileOthersUI, mainMenuStudentUI, mainMenuStaffUI, mainMenuProfUI;
    public GameObject resourceReservationProfUI, resourceReservationStaffUI, dateDetailsProfUI, dateDetailsStaffUI;
    public GameObject timeDetailsProf, timeDetailsStaff, joinMeetingUI;
    public GameObject manageSlotsUI, meetingScheduleUI, meetingDetailsUI, newMeetingUI;
    public Text meetingDetailsText;

    /*
     * Purpose: Deactivates all the UIs when the user enters "MainScene" scene
     *          Start is called before the first frame update
     * Input: NA
     * Output: deactivates all the UIs
     */
    void Start()
    {
        logoutUI.SetActive(false);
        userProfileStudentUI.SetActive(false);
        userProfileOthersUI.SetActive(false);
        mainMenuStudentUI.SetActive(false);
        mainMenuStaffUI.SetActive(false);
        mainMenuProfUI.SetActive(false);
        resourceReservationStaffUI.SetActive(false);
        resourceReservationProfUI.SetActive(false);
        dateDetailsProfUI.SetActive(false);
        dateDetailsStaffUI.SetActive(false);
        manageSlotsUI.SetActive(false);
        timeDetailsProf.SetActive(false);
        timeDetailsStaff.SetActive(false);
        joinMeetingUI.SetActive(false);
        meetingScheduleUI.SetActive(false);
        meetingDetailsUI.SetActive(false);
        newMeetingUI.SetActive(false);
    }

    /*
     * Purpose: If a scene is loaded, add it to SceneManager.sceneLoaded
     * Input: A scene is loaded
     * Output: add it to SceneManager.sceneLoaded and call OnSceneLoaded()
     */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /*
     * Purpose: If a scene is unloaded, remove it from SceneManager.sceneLoaded
     * Input: A scene is unloaded
     * Output: remove it from SceneManager.sceneLoaded and call OnSceneLoaded()
     */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
     * Purpose: Set the respective gameObjects as active or inactive according to the scene loaded
     * Input: Scene loaded is passed in
     * Output: Set the respective gameObjects as active or inactive according to the scene loaded
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Scene")
        {
            if (mainSceneCanvas != null) mainSceneCanvas.SetActive(true);
            if (player != null) player.SetActive(true);
            if (bed != null) bed.SetActive(true);
            if (wardrobe != null) wardrobe.SetActive(true);
            if (table != null) table.SetActive(true);
            if (door != null) door.SetActive(true);
            if (mainCamera != null) mainCamera.enabled = true;
            if (buildingFirstPerson != null) buildingFirstPerson.SetActive(false);
            if (middlewall != null) middlewall.SetActive(true);
        }
        else if (scene.name != "Main Scene")
        {
            if (player != null)
            {
                PlayerControls playerControls = FindObjectOfType<PlayerControls>();
                playerControls.enabled = true;
                player.transform.position = new Vector3(-10, 7, -20);
                player.SetActive(false);
            }
            if (bed != null) bed.SetActive(false);
            if (wardrobe != null) wardrobe.SetActive(false);
            if (table != null) table.SetActive(false);
            if (door != null) door.SetActive(false);
            if (meetingDetailsUI != null)
            {
                meetingDetailsText.text = "";
                DynamicButtonCreator buttonCreator = FindObjectOfType<DynamicButtonCreator>();
                buttonCreator.DeleteAllButtons();
                meetingDetailsUI.SetActive(false);
            }
            if (joinMeetingUI != null) joinMeetingUI.SetActive(false);
            if (mainSceneCanvas != null) mainSceneCanvas.SetActive(false);
            if (mainCamera != null) mainCamera.enabled = false;
            if (buildingFirstPerson != null) buildingFirstPerson.SetActive(false);
            if (middlewall != null) middlewall.SetActive(false);
            
        }
    }

}
