using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    [Header("Pool Settings")]
    public GameObject bulletPrefab;
    public int poolSize = 20;

    private Queue<GameObject> bulletPool = new Queue<GameObject>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bulletPool.Enqueue(bullet);
        }
    }
   
    public GameObject GetBullet(Vector3 position, Quaternion rotation)
    {
        GameObject bullet;

        if (bulletPool.Count > 0)
        {
            bullet = bulletPool.Dequeue();
        }
        else
        {
            bullet = Instantiate(bulletPrefab);
        }

        bullet.transform.position = position;
        bullet.transform.rotation = rotation;
        bullet.SetActive(true);

        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Enqueue(bullet);
    }
}
