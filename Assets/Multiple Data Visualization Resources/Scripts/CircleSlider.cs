using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CircleSlider : MonoBehaviour
{

    public bool b = true;
    public Image image;
    public float speed = 0.5f;
    public float fillamount;
    public float progressAmount;

    float time = 0f;

    public Text progress;


    void Update()
    {
        if (b && time <= fillamount)
        {
            time += Time.deltaTime * speed;
            image.fillAmount = time;
            if (progress)
            {
                progress.text = (progressAmount).ToString("F1");
            }

            if (time > 1)
            {

                time = 0;
            }
        }
    }


}
