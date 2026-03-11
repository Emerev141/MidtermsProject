using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

[RequireComponent(typeof(CircleCollider2D))]
public class ParryController : MonoBehaviour
{
    [Header("Parry Settings")]
    [SerializeField] private float parryDuration = 0.3f; // adjustable in Inspector
    [SerializeField] private CircleCollider2D parryCollider;

    private bool isParrying = false;

    public static event System.Action OnParrySuccess;

    void Awake()
    {
        if (parryCollider == null)
            parryCollider = GetComponent<CircleCollider2D>();

        parryCollider.isTrigger = true;
        parryCollider.enabled = false;
    }

    // This will be called by PlayerInput when the Parry action is triggered
    public void OnParry()
    {
        if (!isParrying)
            StartCoroutine(DoParry());
    }


    private IEnumerator DoParry()
    {
        isParrying = true;
        parryCollider.enabled = true;

        yield return new WaitForSeconds(parryDuration);

        parryCollider.enabled = false;
        isParrying = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyBullet enemyBullet = other.GetComponent<EnemyBullet>();
        if (enemyBullet != null)
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            enemyBullet.Deflect(randomDir);

            OnParrySuccess?.Invoke(); // notify reload system
            return;
        }

        Bullet playerBullet = other.GetComponent<Bullet>();
        if (playerBullet != null)
        {
            Vector2 randomDir = UnityEngine.Random.insideUnitCircle.normalized;
            playerBullet.Deflect(randomDir);

            OnParrySuccess?.Invoke(); // also counts as parry success
        }
    }

    // Draw gizmo for parry radius in Scene view
    void OnDrawGizmosSelected()
    {
        if (parryCollider != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, parryCollider.radius);
        }
    }
}
