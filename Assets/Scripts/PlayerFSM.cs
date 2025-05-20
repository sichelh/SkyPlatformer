using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Run,
    Jump,
    Dead
}

public class PlayerFSM : MonoBehaviour
{
    private PlayerState currentState = PlayerState.Idle;

    private PlayerController controller;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                if (controller.IsMoveInput()) currentState = PlayerState.Run;
                if (controller.IsJumpInput()) currentState = PlayerState.Jump;
                break;
            case PlayerState.Run:
                if (!controller.IsMoveInput()) currentState = PlayerState.Idle;
                if (controller.IsJumpInput()) currentState = PlayerState.Jump;
                break;
            case PlayerState.Jump:
                if (controller.IsGrounded()) currentState = PlayerState.Idle;
                break;
        }

        if (IsDead())
        {
            currentState = PlayerState.Dead;
        }
    }



    private bool IsDead()
    {
        return false;
    }


}
