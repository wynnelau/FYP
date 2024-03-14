using Unity.Services.Vivox;
using UnityEngine;
using UnityEngine.UI;

/*
 * Location: ClassRoom Scene/ ClassRoomSceneControls
 * Purpose: Manage the audio enable and disable of the user
 */

public class VoiceManager : MonoBehaviour
{
    public Button enableAudio, disableAudio;

    /*
     * Purpose: Enable the user's audio
     * Input: Click on "enableVoiceBtn" button
     * Output: Enable user's audio, set "disableVoiceBtn" as active and "enableVoiceBtn" is inactive
     */
    public void EnableAudio()
    {
        VivoxService.Instance.UnmuteInputDevice();
        disableAudio.gameObject.SetActive(true);
        enableAudio.gameObject.SetActive(false);
    }

    /*
     * Purpose: Disable the user's audio
     * Input: Click on "disableVoiceBtn" button
     * Output: Enable user's audio, set "enableVoiceBtn" as active and "disableVoiceBtn" is inactive
     */
    public void DisableAudio()
    {
        VivoxService.Instance.MuteInputDevice();
        enableAudio.gameObject.SetActive(true);
        disableAudio.gameObject.SetActive(false);
    }
}
