using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;


public class SampleClassManager : MonoBehaviour
{
    public GameObject classSamplePrefab;
    public Button enableSampleClass, disableSampleClass;
    
    public void EnableSampleClass()
    {
        NetworkManager.Instantiate(classSamplePrefab);
        enableSampleClass.gameObject.SetActive(false);
        disableSampleClass.gameObject.SetActive(true);
        SampleClassNetwork.Instance.ToggleSampleClassServerRpc(true);
    }

    public void DisableSampleClass()
    {
        NetworkManager.Destroy(classSamplePrefab);
        enableSampleClass.gameObject.SetActive(true);
        disableSampleClass.gameObject.SetActive(false);
        SampleClassNetwork.Instance.ToggleSampleClassServerRpc(false);
    }

}
