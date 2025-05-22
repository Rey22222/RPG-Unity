using UnityEngine;
using UnityEngine.InputSystem;

public class State
{
    public Character character;
    public StateMachine stateMachine;

    protected Vector3 gravityVelocity;
    protected Vector3 velocity;
    protected Vector2 input;

    public InputAction moveAction;
    public InputAction jumpAction;
    public InputAction sprintAction;
    public InputAction drawWeaponAction;
    public InputAction attackAction;
    public InputAction sheathWeaponAction;
    public InputAction yellAction;

    public State(Character _character, StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
        moveAction = character.playerInput.actions["Move"];
        jumpAction = character.playerInput.actions["Jump"];
        sprintAction = character.playerInput.actions["Sprint"];
        drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        attackAction = character.playerInput.actions["Attack"];
        sheathWeaponAction = character.playerInput.actions["SheathWeapon"];
        yellAction = character.playerInput.actions["Yell"];

    }

    public virtual void Enter()
    {
        Debug.Log("enter state: " + this.ToString());
    }

    public virtual void HandleInput()
    {
    }

    public virtual void LogicUpdate()
    {
    }

    public virtual void PhysicsUpdate()
    {
    }

    public virtual void Exit()
    {
    }
}