using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class remove_letter : MonoBehaviourPun
{
    public void destroy_object()
    {
        if(mulitplayer_game_manager.game_manager.your_turn())
        {
            photonView.RPC("destroy_object_RPC", RpcTarget.All);
        }
    }

    [PunRPC]
    void destroy_object_RPC()
    {
        Destroy(gameObject);
    }
}
