using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float playerSpeed = 5.0f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 1f;
    public float gravityMultiplier = 2;
    public float rotationSpeed = 5f;

    [Header("Animation Smoothing")]
    [Range(0, 1)]
    public float speedDampTime = 0.1f;
    [Range(0, 1)]
    public float velocityDampTime = 0.1f;
    [Range(0, 1)]
    public float rotationDampTime = 0.2f;
    [Range(0, 1)]
    public float airControl = 0.5f;

    public StateMachine movementSM;
    public StandingState standing;
    public JumpingState jumping;
    public SprintState sprinting;
    public CombatState combatting;
    public AttackState attacking;
    public YellState yelling;

    [HideInInspector]
    public float gravityValue = -9.81f;
    [HideInInspector]
    public float normalColliderHeight;
    [HideInInspector]
    public CharacterController controller;
    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public Transform cameraTransform;
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Vector3 playerVelocity;

    [Header("Game Settings")]
    public string mainMenuScene = "MainMenu";

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        movementSM = new StateMachine();
        standing = new StandingState(this, movementSM);
        jumping = new JumpingState(this, movementSM);
        sprinting = new SprintState(this, movementSM);
        combatting = new CombatState(this, movementSM);
        attacking = new AttackState(this, movementSM);
        yelling = new YellState(this, movementSM);

        movementSM.Initialize(standing);

        normalColliderHeight = controller.height;
        gravityValue *= gravityMultiplier;
    }

    private void Update()
    {
        movementSM.currentState.HandleInput();
        movementSM.currentState.LogicUpdate();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ExitToMenu();
        }
    }

    private void FixedUpdate()
    {
        movementSM.currentState.PhysicsUpdate();
    }

    private float originalSpeed;
    private Coroutine slowRoutine;
    private Coroutine burnRoutine;
    public HealthSystem healthSystem;

    public void ModifySpeed(float multiplier, float duration)
    {
        if (slowRoutine != null) StopCoroutine(slowRoutine);
        slowRoutine = StartCoroutine(SlowdownRoutine(multiplier, duration));
    }

    private IEnumerator SlowdownRoutine(float multiplier, float duration)
    {
        float speed = originalSpeed;
        float slowedSpeed = speed * multiplier;

        playerSpeed = slowedSpeed;

        yield return new WaitForSeconds(duration);

        playerSpeed = speed;
    }

    public void ApplyBurn(float duration, float damagePerSecond)
    {
        if (burnRoutine != null) StopCoroutine(burnRoutine);
        burnRoutine = StartCoroutine(BurnRoutine(duration, damagePerSecond));
    }

    private IEnumerator BurnRoutine(float duration, float damagePerSecond)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (healthSystem != null)
            {
                healthSystem.TakeDamage(damagePerSecond);
            }

            yield return new WaitForSeconds(1f);
            elapsed += 1f;
        }
    }

    public void RunAwayForSeconds(float duration)
    {
        StartCoroutine(RunAwayRoutine(duration));
    }

    private IEnumerator RunAwayRoutine(float duration)
    {
        Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;

        sprinting.SetRunDirection(randomDirection);
        movementSM.ChangeState(sprinting);

        float elapsed = 0f;
        while (elapsed < duration)
        {
            controller.Move(randomDirection * sprintSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        movementSM.ChangeState(standing);
    }

    private void ExitToMenu()
    {
        SceneManager.LoadScene(mainMenuScene);
    }
}
