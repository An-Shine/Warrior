#pragma warning disable IDE0051


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

public class PlayerCtrl : MonoBehaviour
{
    Animator anim;
    new Transform transform;
    Vector3 moveDir;
    PlayerInput playerInput;
    InputActionMap mainActionMap;
    InputAction moveAction;
    InputAction attackAction;
    void Start()
    {
        anim = GetComponent<Animator>();
        transform = GetComponent<Transform>();
        playerInput = GetComponent<PlayerInput>();
        mainActionMap = playerInput.actions.FindActionMap("PlayerActions");
        moveAction = mainActionMap.FindAction("Move");
        attackAction = mainActionMap.FindAction("Attack");

        moveAction.performed += ContextMenu =>
        {
            Vector2 dir = ContextMenu.ReadValue<Vector2>();
            moveDir = new Vector3(dir.x, 0, dir.y);
            anim.SetFloat("Movement", dir.magnitude);
        };
        moveAction.canceled += ctx =>
        {
            moveDir = Vector3.zero;
            anim.SetFloat("Movement", 0.0f);
        };
        attackAction.performed += ctx =>
        {
            Debug.Log("Attack by c# event");
            anim.SetTrigger("Attack");
        };
    }

    void Update()
    {
        if (moveDir != Vector3.zero)
        {
            //진행방향으로 회전
            transform.rotation = Quaternion.LookRotation(moveDir);
            //회전한 후 전진 방향으로 이동
            transform.Translate(Vector3.forward * Time.deltaTime * 4.0f);
        }
    }
    #region SEND_MESSAGE
    void OnMove(InputValue value)
    {
        Vector2 dir = value.Get<Vector2>();
        //2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);  // y값을 y가 아닌 z에 넣음

        //Run animation
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = {dir.x}, {dir.y}");
    }

    void OnAttack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Attack");
    }
    #endregion

    #region UNITY_EVENTS
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        //2차원 좌표를 3차원 좌표로 변환
        moveDir = new Vector3(dir.x, 0, dir.y);  // y값을 y가 아닌 z에 넣음

        //Run animation
        anim.SetFloat("Movement", dir.magnitude);
        Debug.Log($"Move = {dir.x}, {dir.y}");
    }

    public void OnAtack(InputAction.CallbackContext ctx)
    {
        Debug.Log($"ctx.phase = {ctx.phase}");
        if(ctx.performed)
        {
            Debug.Log("Attack");
            anim.SetTrigger("Attack");
        }
    }

#endregion
}



