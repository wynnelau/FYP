using UnityEngine;

public class LogOutBed : MonoBehaviour
{
    public GameObject logoutUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        logoutUI.SetActive(true);
    }
}
