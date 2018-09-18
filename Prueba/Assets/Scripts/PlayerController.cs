using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public GameObject playerCamera;

    public Text numberOfPlayers;

    public AudioClip shootSound;
    private AudioSource shoot;

    static Animator anim;

	public float speed = 15.0f;
    public float jumpSpeed = 100.0f;
    public float gravity = 20.0f;

    private CharacterController controller;

    private Vector3 moveDirection = Vector3.zero;
    private Vector2 mouseDirection;
    private Camera mainCamera;

    public float mouseSensibility = 5.0f;

    [SyncVar]
    public int playersConnected;

    public void Start() 
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        AudioSource[] audios = GetComponents<AudioSource>();
        shoot = audios[0];
        mainCamera = Camera.main;

        if (isLocalPlayer) 
        {
            playerCamera.SetActive(true);
        }
        else
        {
            playerCamera.SetActive(false);
        }           
    }

    void Update()
    {

        if(isServer) {
           playersConnected = NetworkServer.connections.Count;
        }

        numberOfPlayers.text = "Players: " + playersConnected; 

        if (!isLocalPlayer)
        {
            return;
        }

        Vector2 mouseChange = new Vector2 (Input.GetAxisRaw("Mouse X"),Input.GetAxisRaw("Mouse Y"));
        mouseDirection += mouseChange * mouseSensibility;
        mainCamera.transform.localRotation = Quaternion.AngleAxis(-mouseDirection.y,Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mouseDirection.x,Vector3.up);


        if(controller.isGrounded)
        {
            float translation = Input.GetAxis("Vertical");
            moveDirection = new Vector3(Input.GetAxis("Horizontal"),0,translation);
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;
            if(Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            if(translation != 0)
            {
                anim.SetInteger("PlayerActions", 1);
            }

            else
            {
                anim.SetInteger("PlayerActions", 0);
            }
        }
        else 
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        
        controller.Move(moveDirection * Time.deltaTime);
    
    		
        if (Input.GetMouseButton(0))
        {
            shoot.PlayOneShot(shootSound,1F);
            CmdFire();
        }
    }

    // This [Command] code is called on the Client …
    // … but it is run on the Server!
    [Command]
    void CmdFire()
    {
        // Create the Bullet from the Bullet Prefab
        var bullet = (GameObject)Instantiate(
            bulletPrefab,
            bulletSpawn.position,
            bulletSpawn.rotation);

        // Add velocity to the bullet
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

        // Spawn the bullet on the Clients
        NetworkServer.Spawn(bullet);

        // Destroy the bullet after 2 seconds
        Destroy(bullet, 2.0f);
    }

    

    public override void OnStartLocalPlayer ()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}