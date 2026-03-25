using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VfxPool : MonoBehaviour
{
    public static VfxPool Instance { get; private set; }

    private Dictionary<GameObject, Queue<GameObject>> pools = new Dictionary<GameObject, Queue<GameObject>>();
    private bool shuttingDown = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy() { shuttingDown = true; }

    public GameObject PlayAt(Vector3 position, Quaternion rotation, GameObject prefab, float autoReturnDelay = -1f)
    {
        if (shuttingDown || prefab == null) return null;

        if (!pools.ContainsKey(prefab))
            pools[prefab] = new Queue<GameObject>();

        GameObject obj;
        if (pools[prefab].Count > 0)
        {
            obj = pools[prefab].Dequeue();
        }
        else
        {
            obj = Instantiate(prefab, transform);
        }

        obj.transform.SetPositionAndRotation(position, rotation);
        obj.SetActive(true);

        // Special handling for ParticleSystem
        ParticleSystem ps = obj.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Clear(true);
            ps.time = 0f;
            ps.Play();
            StartCoroutine(ReturnAfter(ps, prefab));
        }
        else if (autoReturnDelay > 0f)
        {
            // For non-particle prefabs (like lights), auto-return after delay
            StartCoroutine(ReturnAfterDelay(obj, prefab, autoReturnDelay));
        }

        return obj;
    }

    private IEnumerator ReturnAfter(ParticleSystem ps, GameObject prefab)
    {
        float timeout = 10f;
        float elapsed = 0f;
        while (!shuttingDown && ps != null && ps.IsAlive(true) && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        if (ps == null) yield break;
        ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ps.Clear(true);
        ps.gameObject.SetActive(false);
        pools[prefab].Enqueue(ps.gameObject);
    }

    private IEnumerator ReturnAfterDelay(GameObject obj, GameObject prefab, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (obj == null) yield break;
        obj.SetActive(false);
        pools[prefab].Enqueue(obj);
    }
}
