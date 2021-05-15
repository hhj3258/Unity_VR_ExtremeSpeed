using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextMeshProUGUI txt_fps;
    public virtual void changeText(float speed)
    {
        float s = speed * 3.6f;
        text.text = Mathf.Clamp(Mathf.Round(s), 0f, 1000f) + "km/h";
    }

    private void Update()
    {
        int fps_counter = Mathf.RoundToInt(1f / Time.deltaTime);
        txt_fps.text = fps_counter +" FPS";
    }

}
