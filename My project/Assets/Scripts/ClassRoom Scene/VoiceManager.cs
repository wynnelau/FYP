using System.Collections;
using System.Collections.Generic;
using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

public class VoiceManager : MonoBehaviour
{
    public Button enableAudio, disableAudio;
    public void EnableAudio()
    {
        VivoxService.Instance.UnmuteInputDevice();
        disableAudio.gameObject.SetActive(true);
        enableAudio.gameObject.SetActive(false);
    }

    public void DisableAudio()
    {
        VivoxService.Instance.MuteInputDevice();
        enableAudio.gameObject.SetActive(true);
        disableAudio.gameObject.SetActive(false);
    }
}
