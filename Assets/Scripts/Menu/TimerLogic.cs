using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerLogic : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TMP_Text text;

    void Update()
    {
        int time = (int)player.GetTimer();
        if (time % 60 < 10)
            text.text = (time / 60) + " :0" + (time % 60);
        else
            text.text = (time / 60) + " :" + (time % 60);
    }
}
