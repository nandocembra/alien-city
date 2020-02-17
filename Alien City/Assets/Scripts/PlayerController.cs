using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

	// Use this for initialization
	private Animator anim;
	private Rigidbody2D rb2d;

	public Transform posPe;
	[HideInInspector] public bool tocaChao = false;


	public float Velocidade;
	public float ForcaPulo = 1000f;
	[HideInInspector] public bool viradoDireita = true;

	public Image vida;
	private MensagemControle MC;

	public bool armaEquipada = false;					 // Variável booleana, verdadeiro se uma arma está equipada, falso se nenhuma arma está equipada
	public GameObject tiro1Prefab;                       // Referência ao prefab do projétil que será disparado pelo Player com a arma 1 
	private AudioSource audioSrc;                        // Usado para guardar uma referência para o componente Autio Source do Player
	public AudioClip shootSound;                         // Clipe de Audio do som do disparo da arma

	private float translationY;							// Recebe o movimento vertical do Player
	private float translationX;							// Recebe o movimento horizontal do Player


	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		audioSrc = GetComponent<AudioSource>();
		tocaChao = true;

		GameObject mensagemControleObject = GameObject.FindWithTag ("MensagemControle");
		if (mensagemControleObject != null) {
			MC = mensagemControleObject.GetComponent<MensagemControle> ();
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Ao apertar Ctrl (Fire1), se estiver parado e tocando no chão, equipa arma
		if (Input.GetButtonDown("Fire1"))
		{
			if (armaEquipada == false)
			{
				armaEquipada = true;
			}
			else
			{
				armaEquipada = false;
			}
		}

		// Ao apertar Alt (Fire2), testa se uma arma está equipada, e instancia o tiro
		if (Input.GetButtonDown("Fire2"))
		{
			if (armaEquipada == true)
			{

				// Guarda a posição do Player, posiciona na variável position e prepara para instanciar o projétil em frente a arma do Player
				Vector3 position = new Vector3();
				if (viradoDireita)
				{
					position = new Vector3(transform.position.x + ((transform.localScale.x / 2f) + 0.6f), transform.position.y - (transform.localScale.y - 1.1f));
				}
				else
				{
					position = new Vector3(transform.position.x + ((transform.localScale.x / 2f) - 0.6f), transform.position.y - (transform.localScale.y - 1.1f));
				}

				// Guarda uma referência ao prefab instanciado (projétil)
				GameObject instance = Instantiate(tiro1Prefab, position, Quaternion.identity) as GameObject;

				// Envia uma mensagem ao objeto instanciado (projétil) indicando a direção em que foi disparado
				if (viradoDireita)
				{
					instance.gameObject.SendMessage("FacingR");
				}
				else
				{
					instance.gameObject.SendMessage("FacingL");
				}

				// Executa o som do tiro
				audioSrc.clip = shootSound;
				audioSrc.Play();



				//Implementar Pulo Aqui!



			}
		}

	}


	void FixedUpdate()
	{
//	Alterei a declaração dessas variáveis, passando para variáveis globais da Classe para corrigir um bug nas animações do Player
//		float translationY = 0;
//		float translationX = Input.GetAxis ("Horizontal") * Velocidade;

		translationY = 0;
		translationX = Input.GetAxis("Horizontal") * Velocidade;
		transform.Translate (translationX, translationY, 0);
		transform.Rotate (0, 0, 0);

		if (translationX != 0 && tocaChao) {
			if (armaEquipada == false)
			{
				anim.SetTrigger("corre");	     // Se arma não está equipada chama animação corre (run)
			} else
			{
				anim.SetTrigger("arma1_corre");		// Se arma está equipada chama Animação correndo com a arma (corre_arma1)
			}
		
		} else {
			if (translationX == 0 && tocaChao) {
				if (armaEquipada == false)
				{
					anim.SetTrigger("parado");      // Se arma não está equipada chama animação parado (stand)
				} else
				{
					anim.SetTrigger("arma1_parado");    // Se arma está equipada chama Animação parado com a arma (stand_shoot)
				}
			}
		


		}


		//Programar o pulo Aqui! 


		if (translationX > 0 && !viradoDireita) {
			Flip ();
		} else if (translationX < 0 && viradoDireita) {
			Flip();
		}


	}
	

	void Flip()
	{
		viradoDireita = !viradoDireita;
		Vector3 escala = transform.localScale;
		escala.x *= -1;
		transform.localScale = escala;
	}

	public void SubtraiVida()
	{
		vida.fillAmount-=0.1f;
		if (vida.fillAmount <= 0) {
			MC.GameOver();
			Destroy(gameObject);
		}
	}
	
}
