using UnityEngine;

/*
 *Location: Main Scene, attached to "Table"
 *Purpose: Trigger the resourceReservation when user comes into contact with "Table"
 */
public class CalendarTable : MonoBehaviour
{
    public GameObject resourceReservation;
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
     * Purpose: Trigger the resourceReservationUI when user comes into contact with "Table"
     * Outcomes: resourceReservation is activated
     */
    private void OnTriggerEnter(Collider other)
    {
        resourceReservation.SetActive(true);
        player.enabled = false;
    }
}
