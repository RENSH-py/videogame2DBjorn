using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Bandit : MonoBehaviour {

    [SerializeField] float      m_speed = 4.0f;
    [SerializeField] float      m_jumpForce = 7.5f;

    public Text textLife;
    public int life = 11;//16

    public Text textItems;
    public int items;
    
    private Animator            m_animator;
    private Rigidbody2D         m_body2d;
    private Sensor_Bandit       m_groundSensor;
    private bool                m_grounded = false;
    private bool                m_combatIdle = false;
    private bool                m_isDead = false;

    
    //Carga de datos antes de inicializar juego
    public void Awake()
    {
        LoadData();
    }
    

    void Start () {
        
        //Remplazo de datos guardados en nivel 0
        Scene scene = SceneManager.GetActiveScene();
       Debug.Log(items);
        if (scene.name == "Nivel0"){
            Debug.Log("me estoy ejecutando acá en el nivel 0");
            if(items > 0 || life > 0){
                Debug.Log("me estoy ejecutando acá");
                items = 0;
                PlayerPrefs.SetInt("itemsPrefs", items);
                life = 10;
                PlayerPrefs.SetInt("lifePrefs", life);
                SaveData();
            }

            //DeleteKey();
        }
        //UpdateUI();
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }
	
	// Update is called once per frame
	void Update () {
        //Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State()) {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }

        //Check if character just started falling
        if(m_grounded && !m_groundSensor.State()) {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeed", m_body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e")) {
            if(!m_isDead)
                m_animator.SetTrigger("Death");
            else
                m_animator.SetTrigger("Recover");

            m_isDead = !m_isDead;
        }
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            m_animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0)) {
            m_animator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            m_combatIdle = !m_combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && m_grounded) {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
            m_animator.SetInteger("AnimState", 2);

        //Combat Idle
        else if (m_combatIdle)
            m_animator.SetInteger("AnimState", 1);

        //Idle
        else
            m_animator.SetInteger("AnimState", 0);
    }
    private void OnCollisionEnter2D(Collision2D obj){
        if(obj.collider.CompareTag("EnemyNormal")){
            Destroy(obj.collider.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D obj){
        if(obj.CompareTag("Trampa")){
            m_animator.SetTrigger("Death");
            SceneManager.LoadScene("GameOver");
        }
        if(obj.CompareTag("EnemyNormal")){
            Debug.Log("se esta ejecutando acá");
            Debug.Log(life);
            life --; //life = life -1 ;
            textLife.text = life.ToString();
            m_animator.SetTrigger("Hurt");
            if(life == 0){
                m_animator.SetTrigger("Death");
                SceneManager.LoadScene("GameOver");
            }
        }
        if(obj.CompareTag("EnemyOp")){
            m_animator.SetTrigger("Death");
            SceneManager.LoadScene("Ganaste");
        }
        if(obj.CompareTag("Traslador")){
            SceneManager.LoadScene("Nivel1");
        }
        if(obj.CompareTag("TrasladorDos")){
            SceneManager.LoadScene("Nivel2");
        }
        if(obj.CompareTag("Fuego")){
            m_animator.SetTrigger("Death");
            SceneManager.LoadScene("GameOver");
        }


    } 
    private void OnTriggerStay2D(Collider2D obj){
        if(obj.CompareTag("PowerUp")){
            m_speed = 8.0f;
            m_jumpForce = 12.0f;
            items ++;
            textItems.text = items.ToString();
            life ++; // 1 + de vida
            textLife.text = life.ToString();
            Destroy(obj.gameObject);
        }
        if(obj.CompareTag("PowerOff")){
            m_speed = 4.0f;
            m_jumpForce = 7.5f;
        }
    }
    //testing
    //Guardado de datos
    /*
    public void UpdateUI()
    {
    textItems.text = AC.GlobalVariables.GetVariable("items").IntegerValue.ToString();
    textLife.text = AC.GlobalVariables.GetVariable("life").IntegerValue.ToString();
    }  */
    public void OnDestroy()
    {

        SaveData();
        
        
    }

    private void SaveData()
    {
        //se utiliza a clase PlayerPrefs para guardado
        PlayerPrefs.SetInt("itemsPrefs", items);
        PlayerPrefs.SetInt("lifePrefs", life);
    }
    private void LoadData()
    {
        items = PlayerPrefs.GetInt("itemsPrefs");
        life = PlayerPrefs.GetInt("lifePrefs");
    }
    
    public void DeleteKey(/*string KeyName*/)
    {
        PlayerPrefs.DeleteAll();
        //PlayerPrefs.DeleteKey(KeyName);
    }
    

}
