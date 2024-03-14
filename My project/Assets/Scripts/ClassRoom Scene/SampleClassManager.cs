using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the instantiation and destruction of the classSample in both host and client side
 * Tutorial used: https://www.youtube.com/watch?v=HWPKlpeZUjM
 */

public class SampleClassManager : MonoBehaviour
{
    public GameObject classSamplePrefab;
    private GameObject classSample;
    public Button enableSampleClass, disableSampleClass;

    /*
     * Purpose: Spawn the classSample on all clients, including host
     * Input: Click on "Enable Sample" button in EnableClass UI
     * Output: Spawn the classSample
     */
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

    /*
     * Purpose: Destroy the classSample on all clients, including host
     * Input: Click on "Disable Sample" button in EnableClass UI
     * Output: Destroy the classSample
     */
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
