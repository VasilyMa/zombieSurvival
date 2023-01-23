using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePopup : MonoBehaviour
{
    public Camera camera;
    [SerializeField] private Text AmountDamage;
    private void Start()
    {
        camera = FindObjectOfType<Camera>();
    }
    private void Update()
    {
        AmountDamage.gameObject.transform.parent.LookAt(AmountDamage.transform.parent.position + camera.transform.forward);
    }
    public void SetDamageAmount(string value)
    {
        AmountDamage.text = value;
        StartCoroutine(FadeText(1.5f, true, AmountDamage));
    }
    IEnumerator FadeText(float fadeTime, bool fadeIn, Text text)
    {
        float step = 0.35f;
        float elapsedTime = 0.0f;
        Color color = text.color;
        while (elapsedTime < fadeTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (fadeIn)
            {
                color.a = Mathf.Clamp01(elapsedTime / fadeTime);
            }
            else
            {
                color.a = 1f - Mathf.Clamp01(elapsedTime / fadeTime);
            }
            gameObject.transform.Translate(Vector3.up * step * Time.deltaTime);
            text.color = color;
        }
        gameObject.SetActive(false);
    }
}
