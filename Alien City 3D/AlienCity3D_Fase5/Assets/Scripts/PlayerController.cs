using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	public float MoveSpeed;
	public float RotationSpeed;
	CharacterController cc;
	private Animator anim;
	protected Vector3 gravidade = Vector3.zero;
	protected Vector3 move = Vector3.zero;
	private bool jump = false;

	public bool chaveAzul = false;
	public bool chaveVerde = false;
	public Image chaveAzulHUD, chaveVerdeHUD;

	private int health;
	public Slider sliderHealth;

	private int contaPedras;
	public Text textoHUDPedras;

	public int vidasExtras;
	public Image imgLife1, imgLife2, imgLife3;


	void Start()
	{
		sliderHealth.value = 1;
		health = 100;
		contaPedras = 0;

		vidasExtras = 3;
		imgLife1.enabled = true;
		imgLife1.enabled = true;
		imgLife1.enabled = true;

		chaveAzulHUD.enabled = false;
		chaveVerdeHUD.enabled = false;

		cc = GetComponent<CharacterController> ();
		anim = GetComponent<Animator>();
		anim.SetTrigger("Parado");
	}

	void Update()
	{
		textoHUDPedras.text = ("Pedras:" + contaPedras).ToString();


		Vector3 move = Input.GetAxis ("Vertical") * transform.TransformDirection (Vector3.forward) * MoveSpeed;
		transform.Rotate (new Vector3 (0, Input.GetAxis ("Horizontal") * RotationSpeed * Time.deltaTime, 0));
		
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
			if(Input.GetKeyDown("space"))
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

	private void OnTriggerEnter(Collider other)
	{	
		if (other.gameObject.name == "ChaveAzul")
		{
			chaveAzul = true;
			chaveAzulHUD.enabled = true;
			Destroy(other.gameObject);
		}
		if (other.gameObject.name == "ChaveVerde")
		{
			chaveVerde = true;
			chaveVerdeHUD.enabled = true;
			Destroy(other.gameObject);
		}
		if (other.gameObject.name == "DoorActivator")
		{
			if ((chaveAzul) && (chaveVerde))
			{
				other.SendMessage("Activate");
			}
		}
	}

	public void DoDamage(int damage)
	{
		health = health - damage;
		Debug.Log("Health: " + health + "  Life:" + vidasExtras);

		sliderHealth.value = health / 100.0f;

		if (health <= 0)
		{
			vidasExtras--;
			Respawn();

//			string cenaAtual = SceneManager.GetActiveScene().name;
//			SceneManager.LoadScene(cenaAtual);
		}
	}

	public void AdicionaPedra()
	{
		contaPedras++;
		Debug.Log("Pedras: " + contaPedras);
	}

	public void Respawn()
	{

		if (vidasExtras == 2)
			imgLife3.enabled = false;
		if (vidasExtras == 1)
			imgLife2.enabled = false;
		if (vidasExtras == 0)
			imgLife1.enabled = false;

		health = 100;
		sliderHealth.value = health / 100.0f;

		this.gameObject.transform.position = new Vector3(35.16f, 7.8f, -43.1f);
	}
}
