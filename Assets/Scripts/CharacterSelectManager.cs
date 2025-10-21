
using UnityEngine;

public class CharacterSelectManager : MonoBehaviour
{
    public void ChooseKael()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Kael");
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZoneOfTrial");
    }

    public void ChooseLyra()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Lyra");
        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene("ZoneOfTrial");
    }
}
