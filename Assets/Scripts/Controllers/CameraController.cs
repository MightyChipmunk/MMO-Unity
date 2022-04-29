using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode _mode = Define.CameraMode.Quarterview;

    [SerializeField]
    Vector3 _delta = new Vector3(0.0f, 6.0f, -5.0f); // 플레이어와 카메라 상의 거리

    [SerializeField]
    GameObject _player = null;

    public void SetPlayer (GameObject player) { _player = player; } 

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (_player.IsValid() == false)
            return;

        if (_mode == Define.CameraMode.Quarterview)
        {
            RaycastHit hit;

            if (Physics.Raycast(_player.transform.position, _delta, out hit, _delta.magnitude, 1 << (int)Define.Layer.Block))
            {
                float dist = (hit.point - _player.transform.position).magnitude * 0.8f;
                transform.position = _player.transform.position + _delta.normalized * dist;
            }
            else
            {
                transform.position = _player.transform.position + _delta;
                transform.LookAt(_player.transform);
            }
        }
    }

    public void SetQuarterView(Vector3 delta)
    {
        _mode = Define.CameraMode.Quarterview;
        _delta = delta;
    }
}
