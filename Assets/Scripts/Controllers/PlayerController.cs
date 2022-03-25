using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float _speed = 10.0f;

    Vector3 _destPos;


    void Start()
    {
        Managers.Input.MouseAction -= OnMouseClicked;
        Managers.Input.MouseAction += OnMouseClicked;
        // Action을 두번 구독하는 경우가 생기지 않도록 먼저 구독을 끊고 다시 구독을 한다.

        Managers.UI.ShowSceneUI<UI_Inven>();
    }

    public enum PlayerState
    {
        Die,
        Moving,
        Idle,
    }

    PlayerState _state = PlayerState.Idle;

    void UpdateMoving()
    {
        Vector3 dir = _destPos - transform.position;
        if (dir.magnitude < 0.0001f)
        {
            _state = PlayerState.Idle;
        } // 목적지까지의 거리가 매우 작다면(도착했다면) 이동중이라는 상태를 false로 만든다.
        else
        {
            float moveDist = Mathf.Clamp(_speed * Time.deltaTime, 0, dir.magnitude);
            // 목적지 좌표에 도착하고도 계속 rotation을 변경하며 목적지에 가려는 상황을 없애기 위해 
            // 목적지 까지의 거리보다 속도가 높아지면 속도를 0으로 만든다.
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        }

        // 애니메이션 처리
        Animator anim = GetComponent<Animator>();
        // 현재 게임 상태에 대한 정보를 넘겨준다.
        anim.SetFloat("speed", _speed);
    }

    void UpdateIdle()
    {
        // 애니메이션
        Animator anim = GetComponent<Animator>();
        // 현재 게임 상태에 대한 정보를 넘겨준다.
        anim.SetFloat("speed", 0);
    }

    void UpdateDie()
    {

    }

    void Update()
    {
        switch (_state)
        {
            case PlayerState.Idle:
                UpdateIdle();
                break;
            case PlayerState.Moving:
                UpdateMoving();
                break;
            case PlayerState.Die:
                UpdateDie();
                break;
        }
    }

    void OnMouseClicked(Define.MouseEvent evt)
    {
        if (_state == PlayerState.Die)
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100.0f, LayerMask.GetMask("Wall")))
        {
            _destPos = hit.point;
            _state = PlayerState.Moving;
        }
    }
}
