using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class create_and_join_rooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField join_input;

    public void create_room()
    {
        PhotonNetwork.CreateRoom(RandomStringGenerator(5));
    }

    public void join_room()
    {
        PhotonNetwork.JoinRoom(join_input.text.ToUpper());
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        print(message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("multiplayer");
    }

    string RandomStringGenerator(int lenght)
    {
        string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        string generated_string = "";

        for(int i = 0; i < lenght; i++)
            generated_string += characters[Random.Range(0, characters.Length)];

        return generated_string;
    }
}
