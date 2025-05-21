using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagableObstacle : MonoBehaviour
{
    private PlayerData playerData;

    private Coroutine damageCoroutine;

    [SerializeField] private float damage = 10f;

    // 데미지 주는 오브젝트와 충돌하면 지속 데미지
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
            playerData = other.GetComponent<PlayerData>();
            
            // 코루틴 중복 실행 방지
            if (damageCoroutine == null)
            {
                // 코루틴 변수로 저장
                damageCoroutine = StartCoroutine(DamageRoutine());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 충돌체 밖으로 나갔을 때 코루틴 중지
        if (other.gameObject.layer == 7)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
        }
    }

    IEnumerator DamageRoutine()
    {
        while (playerData.CurHp > 0)
        {
            playerData.TakeDamage(damage);
            yield return new WaitForSeconds(3);
        }
    }

}
