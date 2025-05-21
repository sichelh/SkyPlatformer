using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicatorUI : MonoBehaviour
{
    private Image image;
    [SerializeField] float flashSpeed;

    private PlayerData playerData;
    private Coroutine coroutine;

    private void Awake()
    {
        playerData = FindObjectOfType<PlayerData>();
        image = this.GetComponent<Image>();
        playerData.OnTakeDamage += Flash; // 데미지 받으면 Indicator 켜기
    }

    private void OnDestroy()
    {
        playerData.OnTakeDamage -= Flash;
    }

    private void Start()
    {
        playerData.OnTakeDamage += Flash;
        image.enabled = false;
    }

    void Flash()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        image.enabled = true;
        coroutine = StartCoroutine(FadeAway());
    }

    // Indicator 켜지면 flashSpeed로 서서히 사라지도록
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while (a > 0)
        {
            a-=(startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
    }
}
