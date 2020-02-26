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

	public int armaSelecionada;	                         // Variável que guarda qual arma está selecionada pelo jogador
	public GameObject tiro1Prefab;                       // Referência ao prefab do projétil que será disparado pelo Player com a arma 1 
	public GameObject tiro2Prefab;                       // Referência ao prefab do projétil que será disparado pelo Player com a arma 2 
	private AudioSource audioSrc;						 // Usado para guardar uma referência para o componente Autio Source do Player
	public AudioClip shootSound;                         // Clipe de Audio do som do disparo da arma
	
	public Image armaUIImagem;							// Referência para a imagem do HUD que representa a arma selecionada


	private float translationY;							 // Recebe o movimento vertical do Player
	private float translationX;							 // Recebe o movimento horizontal do Player


	void Start () {
		anim = GetComponent<Animator> ();
		rb2d = GetComponent<Rigidbody2D> ();
		audioSrc = GetComponent<AudioSource>();
		tocaChao = true;
		armaSelecionada = 0;
		armaUIImagem = GameObject.Find("ArmaUI").GetComponent<Image>();
		armaUIImagem.color = Color.clear;


		GameObject mensagemControleObject = GameObject.FindWithTag ("MensagemControle");
		if (mensagemControleObject != null) {
			MC = mensagemControleObject.GetComponent<MensagemControle> ();
		}
	}

	// Update is called once per frame
	void Update()
	{
		// Ao apertar Ctrl (Fire1), se estiver parado e tocando no chão, troca a arma
		if (Input.GetButtonDown("Fire1"))
		{
			TrocaArma();
		}

		// Ao apertar Alt (Fire2), testa se uma arma está equipada, e instancia o tiro
		if (Input.GetButtonDown("Fire2"))
		{
			if (armaSelecionada != 0)
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

				if (armaSelecionada == 1)
				{
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
				}
				else if (armaSelecionada == 2)
				{
					// Guarda uma referência ao prefab instanciado (projétil)
					GameObject instance = Instantiate(tiro2Prefab, position, Quaternion.identity) as GameObject;
					// Envia uma mensagem ao objeto instanciado (projétil) indicando a direção em que foi disparado
					if (viradoDireita)
					{
						instance.gameObject.SendMessage("FacingR");
					}
					else
					{
						instance.gameObject.SendMessage("FacingL");
					}
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

		if (translationX != 0 && tocaChao)
		{
			if (armaSelecionada == 0)
			{
				anim.SetTrigger("corre");        // Se arma selecionada for 0 chama animação corre (run)
			}
			else if (armaSelecionada == 1)
			{
				anim.SetTrigger("arma1_corre");     // Se arma selecionada for 1 chama animação correndo com a arma 1 (corre_arma1)
			}
			else if (armaSelecionada == 2)
			{
				anim.SetTrigger("arma2_corre");     // Se arma selecionada for 2 chama animação correndo com a arma 2 (corre_arma2)
			}

		}
		else
		{
			if (translationX == 0 && tocaChao)
			{
				if (armaSelecionada == 0)
				{
					anim.SetTrigger("parado");      // Se arma selecionada for 0 chama animação parado (stand)
				}
				else if (armaSelecionada == 1)
				{
					anim.SetTrigger("arma1_parado");    // Se arma selecionada for 1 chama Animação parado com a arma 1 (stand_shoot)
				}
				else if (armaSelecionada == 2)
				{
					anim.SetTrigger("arma2_parado");    // Se arma selecionada for 2 chama Animação parado com a arma 2 (stand_shoot2)
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

	void TrocaArma()
	{
		if (armaSelecionada <= 2)
		{
			armaSelecionada++;
		}
		if (armaSelecionada > 2)
		{
			armaSelecionada = 0;
		}

		// Altera o Sprite da Arma no HUD
		if (armaSelecionada == 1)
		{
			armaUIImagem.sprite = Resources.Load<Sprite>("Sprites/arma1");
			armaUIImagem.color = Color.white;
		}
		else if (armaSelecionada == 2)
		{
			armaUIImagem.sprite = Resources.Load<Sprite>("Sprites/arma2");
			armaUIImagem.color = Color.white;
		}
		if (armaSelecionada == 0)
		{
			armaUIImagem.sprite = Resources.Load<Sprite>("");
			armaUIImagem.color = Color.clear;
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
