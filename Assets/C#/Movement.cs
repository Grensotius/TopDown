using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 8f;
    [SerializeField]
    private  Camera _cam;
    [SerializeField]
    private GameObject _player;

    private float hitDist = 0.0f;
    void Update()
    {
        // Направление в сторону мыши
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray cameraRay = _cam.ScreenPointToRay(Input.mousePosition);
        if (playerPlane.Raycast(cameraRay, out hitDist))
        {
            Vector3 targetPoint = cameraRay.GetPoint(hitDist);
            _player.transform.LookAt(targetPoint);
            Debug.DrawLine(_cam.transform.position, targetPoint);
        }
        // Передвижение
        if (Input.GetKey(KeyCode.S))
            transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.Translate(Vector3.left * _moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.Translate(Vector3.right * _moveSpeed * Time.deltaTime);
    }
}
