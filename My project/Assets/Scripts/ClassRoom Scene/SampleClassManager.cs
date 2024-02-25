using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;


public class SampleClassManager : MonoBehaviour
{
    public GameObject classSamplePrefab, classSample;
    public Button enableSampleClass, disableSampleClass;
    
    public void EnableSampleClass()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            classSample = Instantiate(classSamplePrefab);
            classSample.GetComponent<NetworkObject>().Spawn();
        }

        enableSampleClass.gameObject.SetActive(false);
        disableSampleClass.gameObject.SetActive(true);
    }

    public void DisableSampleClass()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Destroy(classSample);
        }

        enableSampleClass.gameObject.SetActive(true);
        disableSampleClass.gameObject.SetActive(false);
    }

}
