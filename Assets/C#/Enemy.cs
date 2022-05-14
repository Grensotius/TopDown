using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Transform _tPlayer;
    [SerializeField]
    private GunEnemy _gun;
    [SerializeField]
    private LayerMask Mask;
    [SerializeField]
    private Transform _barrel;
    private NavMeshAgent _agent;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    private void FixedUpdate()
    {
        _agent.destination = _tPlayer.position;
        transform.LookAt(_tPlayer.position);

        if (Physics.Raycast(_barrel.position, transform.forward, out RaycastHit hit, float.MaxValue, Mask)) 
        {
            if(hit.collider.tag == ("Player1"))
            {
                _gun.Shoot();
            }
        }
    }
}
