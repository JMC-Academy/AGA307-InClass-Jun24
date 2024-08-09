using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    private CharacterController characterController;
    public int health = 100;
    public float speed = 10f;
    public float jumpHeight = 3f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("Audio")]
    public float stepRate = 0.5f;
    private float stepCooldown;
    private AudioSource audioSource;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Check if we are touching the ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //Get the input from the player
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Move the player
        Vector3 move = transform.right * x + transform.forward * z;
        characterController.Move(move * speed * Time.deltaTime);

        //Do the jump stuff
        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        //Audio stuff
        stepCooldown -= Time.deltaTime;
        if(stepCooldown < 0 && isGrounded && (move.x != 0 || move.z != 0))
        {
            stepCooldown = stepRate;
            _AUDIO.PlayPlayerFootsteps(audioSource);
        }
    }

    public void Hit(int _damage)
    {
        health -= _damage;
        _AUDIO.PlayHitSound(audioSource);
    }
}
