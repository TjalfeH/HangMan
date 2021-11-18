using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Solo_game_manager : MonoBehaviour
{
    public GameObject win_obj;

    public TMP_InputField guess_input;

    public GameObject letter_holder;
    public GameObject letter_prefab;

    public string[] Words;

    public float letter_distance;

    string displayed_word;

    private void Start()
    {
        for (int i = 0; i < Words.Length; i++)
        {
            Words[i] = Words[i].ToUpper();
        }

        prepare_random_word();
    }

    public void guess_letter(string guessed_letter)
    {
        for (int i = 0; i < displayed_word.Length; i++)
        {
            if(guessed_letter == displayed_word[i].ToString().ToUpper())
            {
                letter_holder.transform.GetChild(i).GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void prepare_random_word()
    {
        displayed_word = Words[Random.Range(0, Words.Length)];

        float x_pos = (float)displayed_word.Length / 2 * letter_distance * -1 + letter_distance/2;

        foreach(char letter in displayed_word)
        {
            if(letter != ' ')
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

    public void guess_word()
    {
        if(guess_input.text.ToUpper() == displayed_word.ToUpper())
        {
            win_obj.SetActive(true);
        }
    }

    public void reset_scene()
    {
        SceneManager.LoadScene("single_player");
    }
}
