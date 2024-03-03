using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public TMP_Text waveText;

    public async void AnnounceWave(int wave)
    {
        waveText.text = $"Wave {wave + 1} started!";
        await new WaitForSeconds(2f);
        waveText.text = "";
    }
}
