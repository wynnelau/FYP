using UnityEngine.UI;
using UnityEngine;

public class SampleClassManager : MonoBehaviour
{
    public GameObject sampleClass;
    public Button enableSampleClass, disableSampleClass;
    
    public void EnableSampleClass()
    {
        sampleClass.SetActive(true);
        enableSampleClass.gameObject.SetActive(false);
        disableSampleClass.gameObject.SetActive(true);
    }

    public void DisableSampleClass()
    {
        sampleClass.SetActive(false);
        enableSampleClass.gameObject.SetActive(true);
        disableSampleClass.gameObject.SetActive(false);
    }
}
