using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public InputAction moveAction, mouseAction, interactAction, pauseAction;
    public float movementSpeed, maxSpeed, cameraSpeed, interactRange;

    private AudioSource audioSource;
    private Animator animator;
    private Transform cameraPivot, playerCamera;
    private Vector2 moveInput, mouseInput;
    private Rigidbody rb;
    private PlayerState playerState;

    private enum PlayerState
    {
        Idle,
        Walking
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraPivot = transform.Find("CameraPivot");
        playerCamera = cameraPivot.transform.Find("PlayerCamera");
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
        mouseInput = mouseAction.ReadValue<Vector2>();

        if (moveInput.magnitude > 0)
        {
            playerState = PlayerState.Walking;
        }
        else
        {
            playerState = PlayerState.Idle;
        }

        if (interactAction.WasPressedThisFrame())
        {
            InteractAttempt();
        }

        if (pauseAction.WasPressedThisFrame())
        {
            PlayerUIManager.instance.PauseGame(gameObject);
        }

        animator.SetInteger("AnimationState", (int)playerState);
        transform.Rotate(Vector3.up, mouseInput.x * cameraSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(new Vector3(moveInput.x, 0, moveInput.y) * movementSpeed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        if (moveInput.magnitude == 0)
        {
            rb.velocity *= 0.85f;
        }
    }

    private void LateUpdate()
    {
        cameraPivot.Rotate(Vector3.up, (cameraPivot.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y) * 0.95f);
        playerCamera.Rotate(Vector3.right, -mouseInput.y * cameraSpeed * Time.deltaTime);
    }

    private void InteractAttempt()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, interactRange))
        {
            if (hit.collider.CompareTag("RaycastButton") && hit.collider.gameObject.GetComponentInParent<CameraManager>() != null)
            {
                hit.collider.gameObject.GetComponentInParent<CameraManager>().ApplyControls(hit.collider.gameObject);
            }
        }
    }

    public void StartWalk()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void EndWalk()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    public void OnEnable()
    {
        moveAction.Enable();
        mouseAction.Enable();
        interactAction.Enable();
        pauseAction.Enable();
    }

    public void OnDisable()
    {
        moveAction.Disable();
        mouseAction.Disable();
        interactAction.Disable();
        pauseAction.Disable();
    }
}
