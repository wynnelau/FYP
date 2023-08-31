using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabControls : MonoBehaviour
{
    [SerializeField] GameObject loginRegisterScene;
    public TMP_Text email, password, loginRegisterError;
    private string encryptedPassword;
    
}
