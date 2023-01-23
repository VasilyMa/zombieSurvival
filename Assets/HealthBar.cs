using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Camera camera;
    public Image HealthSlider;
    private void Start()
    {
        camera = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        HealthSlider.gameObject.transform.parent.LookAt(HealthSlider.transform.parent.position + camera.transform.forward);
    }
    public void ChangeHealth(float value)
    {
        HealthSlider.fillAmount = value;

        if (HealthSlider.fillAmount <= 0)
            HealthSlider.gameObject.SetActive(false);
    }
}
