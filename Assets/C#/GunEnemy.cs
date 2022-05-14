using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GunEnemy : MonoBehaviour
{
    [SerializeField]
    private Transform BulletSpawnPoint;
    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float ShootDelay = 0.1f;
    [SerializeField]
    private float Speed = 100;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private bool BouncingBullets;
    [SerializeField]
    private float BounceDistance = 10f;
    [SerializeField]
    private float _bulletHitRange;
    [SerializeField]
    private SOData _data;
    [SerializeField]
    private UIManager _ui;
    [SerializeField]
    private float _spread;

    private float LastShootTime;
    public void Shoot()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            TrailRenderer trail = Instantiate(BulletTrail, BulletSpawnPoint.position, Quaternion.identity);

            if (Physics.Raycast(BulletSpawnPoint.position,transform.forward + new Vector3(Random.Range(-_spread, _spread),0,0) , out RaycastHit hit, float.MaxValue, Mask))
            {
                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, BounceDistance, true));
            }
            else
            {
                StartCoroutine(SpawnTrail(trail, transform.forward * 10000, Vector3.zero, BounceDistance, false));
            }

            LastShootTime = Time.time;
        }
    }

    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, float BounceDistance, bool MadeImpact)
    {
        Vector3 startPosition = Trail.transform.position;
        Vector3 direction = (HitPoint - Trail.transform.position);

        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float startingDistance = distance;

        while (distance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (distance / startingDistance));
            distance -= Time.deltaTime * Speed;

            if (Physics.Raycast(Trail.transform.position, direction, out RaycastHit Rhit, _bulletHitRange, Mask))
            {
                if (Rhit.distance <= _bulletHitRange && Rhit.collider.tag == ("Wall") && Trail.gameObject.active == true)
                {
                    Trail.gameObject.SetActive(false);
                }
                if (Rhit.distance <= _bulletHitRange && (Rhit.collider.tag == ("Player1") || Rhit.collider.tag == ("Player2")) && Trail.gameObject.active == true)
                {
                    Rhit.collider.gameObject.SetActive(false);
                    Trail.gameObject.SetActive(false);
                    if (Rhit.collider.tag == ("Player2"))
                    {
                        _data._score1++;
                        OnHit();
                    }
                    if (Rhit.collider.tag == ("Player1"))
                    {
                        _data._score2++;
                        OnHit();
                    }
                }
            }

            yield return null;
        }

        Trail.transform.position = HitPoint;


        if (MadeImpact)
        {
            if (BouncingBullets && BounceDistance > 0)
            {
                Vector3 bounceDirection = Vector3.Reflect(direction, HitNormal);

                if (Physics.Raycast(HitPoint, bounceDirection, out RaycastHit hit, BounceDistance, Mask))
                {
                    yield return StartCoroutine(SpawnTrail(
                        Trail,
                        hit.point,
                        hit.normal,
                        BounceDistance - Vector3.Distance(hit.point, HitPoint),
                        true
                    ));
                }
                // Для других уровней
                /*else
                {
                    yield return StartCoroutine(SpawnTrail(
                        Trail,
                        bounceDirection * BounceDistance,
                        Vector3.zero,
                        0,
                        false
                    ));
                }*/
            }
        }

        Destroy(Trail.gameObject, Trail.time);
    }

    private void OnHit()
    {
        _data._round++;
        _ui.AddScore();
        SceneManager.LoadScene(0);
    }
}
