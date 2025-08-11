using UnityEngine;
using UnityEngine.InputSystem;
using PrimeTween;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    private PlayerInput inputActions;
    CharacterController controller;
    public float jumpHeight = 5f;
    public Vector3 velocity;
    public float gravity = -9.81f;
    bool isGrounded;
    public float currentMovementSpeed = 2f;
    public float regularMovementSpeed = 2f;
    public float slideMovementSpeed = 2f;
    public float jumpMovementSpeed = 2f;
    public bool handleParenting;
    bool isJumping;
    Animator anim;
    bool wasGrounded;
    public Vector3 slideControllerCenter;
    public float slideHeight;
    public Vector3 regularCenter;
    public Vector2 moveInput;
    public float regularHeight;
    public float slideTime;
    bool isSliding;

    public Transform startPosition;
    public float transitionTime = 2f;
    public float startDelay;
    public bool gameStarted = false;
    public static UnityEvent startGame = new UnityEvent();
    public ParticleSystem slideParticle;
    public ParticleSystem speedLines;
    public float slideFOV;
    public float slideFOVTime;
    public Ease slideFOVEase;
    public float regularFOV;
    public float regularFOVTime;
    public Ease regularFOVEase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.gameManager.gameOver.AddListener(GameOver);
        regularFOV = Camera.main.fieldOfView;
        controller = GetComponent<CharacterController>();
        controller.enabled = false;
        anim = GetComponent<Animator>();
        currentSmoothTime = movementSmoothTime;
        Tween.Position(transform, startPosition.position, transitionTime, startDelay: startDelay).OnComplete(() => { 
            controller.enabled = true; 
            gameStarted = true;
            startGame?.Invoke();
            speedLines.Play();
        });
    }

    public void GameOver()
    {
        OnDisable();
        speedLines.Stop();
        anim.SetTrigger("GameOver");
    }


    private void Awake()
    {
        inputActions = new PlayerInput();
    }
 
    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        inputActions.Player.Jump.performed += ctx => Jump();
        inputActions.Player.Slide.performed += ctx => Slide();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled -= ctx => moveInput = Vector2.zero;
        inputActions.Player.Disable();
    }

    void Jump()
    {
        if (isGrounded && !isSliding)
        {
            Debug.Log("Jump");
            anim.SetTrigger("Jump");
            currentSmoothTime = jumpSmoothTime;
            currentMovementSpeed = jumpMovementSpeed;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            isJumping = true;
            SoundManager.instance.PlayJump();
        }
    }
    public float shakeStrength = 10f;
    public float shakeTime = 1f;
    public float shakeFrequency = 10f;
    void Slide()
    {
        if (isGrounded && !isSliding) { 
            Debug.Log("Slide");
            anim.SetTrigger("Slide");
            controller.height = slideHeight;
            controller.center = slideControllerCenter;
            isSliding = true;
            currentSmoothTime = slideSmoothTime;
            currentMovementSpeed = slideMovementSpeed;
            Tween.ShakeCamera(Camera.main, shakeStrength, shakeTime, shakeFrequency);
            Tween.Delay(slideTime, () => { StopSliding(); });
            slideParticle.Play();
            Tween.CameraFieldOfView(Camera.main, slideFOV, slideFOVTime, slideFOVEase);
            SoundManager.instance.PlaySlide();
        }
    }

    public float slideGracePeriod;

    void StopSliding()
    {
        isSliding = false;
        anim.SetTrigger("Run");
        Tween.Delay(slideGracePeriod, () => {
            if (isSliding) { return; }
            controller.height = regularHeight;
            controller.center = regularCenter;
        });

        Tween.CameraFieldOfView(Camera.main, regularFOV, regularFOVTime, regularFOVEase);
        SoundManager.instance.PlayFootStepSounds();

        currentSmoothTime = movementSmoothTime;
        currentMovementSpeed = regularMovementSpeed;
    }
    Vector2 currentMovement;
    Vector2 inputDir;
    Vector3 movementDir;
    public Vector2 currentMovementVelocity;
    public float currentSmoothTime;
    public float movementSmoothTime = 0.1f;
    public float slideSmoothTime = 0.6f;
    public float jumpSmoothTime = 0.4f;
    // Update is called once per frame
    void Update()
    {
        if (!gameStarted) { return; }
        moveInput.y = 0f;
        wasGrounded = isGrounded;
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            if (!wasGrounded && isGrounded)
            {
                isJumping = false;
                anim.SetTrigger("Land");
                currentSmoothTime = movementSmoothTime;
                currentMovementSpeed = regularMovementSpeed;
                SoundManager.instance.PlayLand();
                SoundManager.instance.PlayFootStepSounds();
            }
        }
        velocity.y += gravity * Time.deltaTime;

        inputDir = moveInput;

        currentMovement = Vector2.SmoothDamp(currentMovement, inputDir, ref currentMovementVelocity, currentSmoothTime);
        currentMovement = Vector2.ClampMagnitude(currentMovement, 1f);

        movementDir = currentMovement.x * transform.right + currentMovement.y * transform.forward;
        movementDir *= currentMovementSpeed;
        movementDir.y = velocity.y;
        controller.Move(movementDir * Time.deltaTime);
    }
}