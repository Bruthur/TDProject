using UnityEngine;
using System.Collections;

[RequireComponent( typeof( CharacterController ) ) ]

public class sFPSPlayer : MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float jumpSpeed = 15.0f;
	float verticalVelocity = 0f;
	public float gravityMultiplier = 1.5f;
	
	public float mouseSensitivity = 5.0f;
	float verticalRotation = 0f;
	public float upDownRange = 60.0f;

	CharacterController characterController;

	private sPlayerSettings playerSettings;
	public GameObject FPSCamera;

	void Awake(){
		playerSettings = GameObject.Find ("PlayerSettings").GetComponent<sPlayerSettings>();
		playerSettings.SetFPSCamera(FPSCamera);
		playerSettings.SetFPSPlayer(gameObject);

		if(!playerSettings.GetPlayerIsRTS()){
			FPSCamera.SetActive(true);
		}
	}

	void Start(){
		characterController = GetComponent<CharacterController>();
	}

	void Update()
	{
		if (!playerSettings.GetPlayerIsRTS())
		{
			InputMovement();
		}
		else
		{
			SyncedMovement();
		}
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
		transform.rotation = newRotation;
	}
	
	void InputMovement()
	{
		// Rotation
		float rotLeftRight = Input.GetAxis("Mouse X") * mouseSensitivity;
		transform.Rotate( 0, rotLeftRight, 0);
		
		verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
		verticalRotation = Mathf.Clamp( verticalRotation, -upDownRange, upDownRange );
		Camera.main.transform.localRotation = Quaternion.Euler( verticalRotation, 0, 0 );
		
		
		// Movement
		float forwardSpeed = Input.GetAxis("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis("Horizontal") * movementSpeed;
		
		if( characterController.isGrounded ) {
			verticalVelocity = 0;
		}

		if( characterController.isGrounded && Input.GetButtonDown("Jump") ) {
			verticalVelocity = jumpSpeed;
		}
		
		verticalVelocity += Physics.gravity.y *gravityMultiplier * Time.deltaTime;
		
		Vector3 speed = new Vector3( sideSpeed, verticalVelocity, forwardSpeed );
		
		speed = transform.rotation * speed;
		
		characterController.Move( speed * Time.deltaTime );
	}
	
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	private Quaternion syncRotation = Quaternion.identity;
	private Quaternion newRotation =  Quaternion.identity;
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		Quaternion syncRotation = Quaternion.identity;
		if (stream.isWriting)
		{
			syncPosition = transform.position;
			syncVelocity = characterController.velocity;
			syncRotation = transform.rotation;

			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRotation);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);
			stream.Serialize(ref syncRotation);
			
			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = transform.position;
			newRotation = syncRotation;
		}
	}
	}