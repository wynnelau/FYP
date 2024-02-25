using UnityEngine.UI;
using UnityEngine;


public class SampleClassManager : MonoBehaviour
{
    public GameObject classSample;
    public Button enableSampleClass, disableSampleClass;
    
    public void EnableSampleClass()
    {
        classSample.SetActive(true);
        enableSampleClass.gameObject.SetActive(false);
        disableSampleClass.gameObject.SetActive(true);
        SampleClassNetwork.Instance.ToggleSampleClassServerRpc(true);
    }

    public void DisableSampleClass()
    {
        classSample.SetActive(false);
        enableSampleClass.gameObject.SetActive(true);
        disableSampleClass.gameObject.SetActive(false);
        SampleClassNetwork.Instance.ToggleSampleClassServerRpc(false);
    }

}
