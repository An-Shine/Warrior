using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCtrlByEvent : MonoBehaviour
{
    InputAction moveAction;
    InputAction attackAction;

    Animator anim;
    Vector3 moveDir;
    void Start()
    {
        anim = GetComponent<Animator>();
        //Move 액션 생성 및 타입 설정
        moveAction = new InputAction("Move", InputActionType.Value);
        //Move 액션의 복합바인딩 정보 정의
        moveAction.AddCompositeBinding("2DVector")
                                        .With("Up", "<Keyboard>/w")
                                        .With("Down", "<Keyboard>/s")
                                        .With("Left", "<Keyboard>/a")
                                        .With("Right", "<Keyboard>/d");
        //Move 액션의 performed, cancled  이벤트 연결
        moveAction.performed += ctx =>
        {
            Vector2 dir = ctx.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            anim.SetFloat("Movement", dir.magnitude);   //Warrior_Run 애니메이션 실행
        };
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };
        moveAction.Enable();        //move action 활성화

        //Attack 액션 생성
        attackAction = new InputAction("Attack", InputActionType.Button, "<Keyboard>/space");
        //Attack 액션의 performed 이벤트 연결
        attackAction.performed += ctx =>
        {
            anim.SetTrigger("Attack");
        };
        // Attack 액션 활성화
        attackAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(moveDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(moveDir);      //진행방향으로 회전
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);   //회전 후 전진방향으로 이동
        }
    }
}
