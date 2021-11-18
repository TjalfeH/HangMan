using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class room_text : MonoBehaviourPunCallbacks
{
    public GameObject game_canvas;
    public GameObject this_canvas;

    public GameObject master_buttons;

    public GameObject player_text;

    public GameObject name_holder;

    private void Awake()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(master_buttons);
        GetComponent<TextMeshProUGUI>().text = PhotonNetwork.CurrentRoom.Name;
    }

    private void Start()
    {
        photonView.RPC("reset_players", RpcTarget.All);
    }

    public void start_game()
    {
        mulitplayer_game_manager.game_manager.prepare_word();
        mulitplayer_game_manager.game_manager.call_change_turn();
        photonView.RPC("play_game", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        photonView.RPC("reset_players", RpcTarget.All);
    }

    [PunRPC]
    void play_game()
    {
        game_canvas.SetActive(true);
        this_canvas.SetActive(false);
    }

    [PunRPC]
    void reset_players()
    {
        for (int i = 0; i < name_holder.transform.childCount; i++)
            Destroy(name_holder.transform.GetChild(i).gameObject);

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject new_player = Instantiate(player_text, name_holder.transform);
            new_player.GetComponent<TextMeshProUGUI>().text = player.NickName;
        }
    }
}
