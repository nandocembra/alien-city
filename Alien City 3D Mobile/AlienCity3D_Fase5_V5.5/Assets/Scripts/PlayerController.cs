using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float MoveSpeed;
	public float RotationSpeed;
	CharacterController cc;
	private Animator anim;
	protected Vector3 gravidade = Vector3.zero;
	protected Vector3 move = Vector3.zero;
	private bool jump = false;

	private int health;
	public Slider healthBar;

	void Start()
	{
		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator>();
		anim.SetTrigger("Parado");

		health = 100;
		healthBar.value = health / 100.0f;
		
	}

	void Update()
	{
		//Vector3 move = Input.GetAxis ("Vertical") * transform.TransformDirection (Vector3.forward) * MoveSpeed;
		//transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * RotationSpeed * Time.deltaTime, 0));

		Vector3 move = CrossPlatformInputManager.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward) * MoveSpeed;
		transform.Rotate (new Vector3 (0, CrossPlatformInputManager.GetAxis ("Horizontal") * RotationSpeed * Time.deltaTime, 0));

		if (!cc.isGrounded) {
			gravidade += Physics.gravity * Time.deltaTime;
		} 
		else 
		{
			gravidade = Vector3.zero;
			if(jump)
			{
				gravidade.y = 6f;
				jump = false;
			}
		}
		move += gravidade;
		cc.Move (move* Time.deltaTime);
		Anima ();
	}
	 
	void Anima()
	{
		if (!Input.anyKey)
		{
			anim.SetTrigger("Parado");
		} 
		else 
		{
			//if(Input.GetKeyDown("space"))
			if (CrossPlatformInputManager.GetButtonDown("Jump"))
			{
				anim.SetTrigger("Pula");
				jump = true;
			}
			else
			{
				anim.SetTrigger("Corre");
			}
		}
	}

	public void DoDamage(int damage)
	{
		health = health - damage;
		Debug.Log("Damage: " + damage + "  Health:" + health);
		healthBar.value = health / 100.0f;
		Handheld.Vibrate();

		if (health <= 0)
		{
			Debug.Log(" ");
			Respawn();

			//			string cenaAtual = SceneManager.GetActiveScene().name;
			//			SceneManager.LoadScene(cenaAtual);
		}
	}

	public void Respawn()
	{
		health = 100;
		healthBar.value = health / 100.0f;

		this.gameObject.transform.position = new Vector3(0f, 0f, -43.1f);
	}

}
