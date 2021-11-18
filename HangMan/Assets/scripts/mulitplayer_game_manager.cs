using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mulitplayer_game_manager : MonoBehaviourPunCallbacks
{
    public TMP_InputField guess_input;

    public GameObject letter_holder;
    public GameObject letter_prefab;

    public string[] Words;

    public float letter_distance;

    string displayed_word;

    public static mulitplayer_game_manager game_manager;

    public string player_turn_id;
    public int player_turn_index;

    public GameObject win_screen;
    public GameObject loose_screen;
    public TextMeshProUGUI winning_name_text;

    public GameObject custom_word_input;

    public Toggle custom_word_toggle;

    private void Awake()
    {
        game_manager = this;
    }

    private void Start()
    {
        for (int i = 0; i < Words.Length; i++)
        {
            Words[i] = Words[i].ToUpper();
        }
    }

    public void call_change_turn()
    {
        photonView.RPC("change_turn", RpcTarget.All);
    }

    [PunRPC]
    void change_turn()
    {
        player_turn_id = PhotonNetwork.PlayerList[player_turn_index].UserId;
        player_turn_index += 1;
        if (player_turn_index >= PhotonNetwork.PlayerList.Length)
            player_turn_index = 0;
    }

    public void guess_letter(string guessed_letter)
    {
        if (your_turn())
        {
            call_change_turn();
            photonView.RPC("RPC_guess_letter", RpcTarget.All, guessed_letter);
        }
    }

    [PunRPC]
    void RPC_guess_letter(string guessed_letter)
    {
        for (int i = 0; i < displayed_word.Length; i++)
        {
            if (guessed_letter == displayed_word[i].ToString().ToUpper())
            {
                letter_holder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void custom_word_toggle_click()
    {
        custom_word_input.SetActive(!custom_word_toggle.isOn);
    }

    public void prepare_word()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (custom_word_toggle.isOn)
            {
                photonView.RPC("set_displayed_word", RpcTarget.All, Words[Random.Range(0, Words.Length)]);
            }
            else
            {
                photonView.RPC("set_displayed_word", RpcTarget.All, custom_word_input.GetComponent<TMP_InputField>().text);
            }
        }
    }

    public bool your_turn()
    {
        if (player_turn_id == PhotonNetwork.LocalPlayer.UserId)
            return true;

        return false;
    }

    public void guess_word()
    {
        if (guess_input.text.ToUpper() == displayed_word.ToUpper())
        {
            if (your_turn())
            {
                win_screen.SetActive(true);
                photonView.RPC("load_loose_screen", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName);
            }
        }

        call_change_turn();
    }

    [PunRPC]
    void load_loose_screen(string winning_player)
    {
        loose_screen.SetActive(true);
        winning_name_text.text = winning_player;
    }

    public void back_to_menu()
    {
        SceneManager.LoadScene("menu");
    }

    [PunRPC]
    void set_displayed_word(string word)
    {
        displayed_word = word;

        float x_pos = (float)displayed_word.Length / 2 * letter_distance * -1 + letter_distance / 2;

        foreach (char letter in displayed_word)
        {
            if (letter != ' ')
            {
                GameObject empty_letter = Instantiate(letter_prefab, letter_holder.transform);
                empty_letter.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(x_pos, 0);
                empty_letter.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = letter.ToString();
            }
            else
            {
                GameObject empty_letter = Instantiate(letter_prefab, letter_holder.transform);
                Destroy(empty_letter.transform.GetChild(0).gameObject);
                Destroy(empty_letter.transform.GetChild(1).gameObject);
            }

            x_pos += letter_distance;
        }

        if (displayed_word.Length > 7)
        {
            float start_size = displayed_word.Length * 80 + 75;
            float new_size = 1 - ((float)((displayed_word.Length - 8) * 80) / start_size);
            letter_holder.transform.localScale = new Vector3(new_size, new_size, 1);
        }
    }
}
