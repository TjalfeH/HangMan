using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class button_handler : MonoBehaviour
{
    public TMP_InputField name_input;

    public void single_player_play()
    {
        SceneManager.LoadScene("Single_player");
    }

    public void multiplayer_play()
    {
        PhotonNetwork.LocalPlayer.NickName = name_input.text;
        SceneManager.LoadScene("Loading");
    }
}
