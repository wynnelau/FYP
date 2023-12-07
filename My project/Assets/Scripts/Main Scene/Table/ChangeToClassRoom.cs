using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeToClassRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeToClassRoom()
    {
        SceneManager.LoadScene("ClassRoom Scene");
    }
}
