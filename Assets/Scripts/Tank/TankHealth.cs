 using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    //坦克初始血量
    public float m_StartingHealth = 100f;          
    public Slider m_Slider;                        
    public Image m_FillImage;                      
    public Color m_FullHealthColor = Color.green;  
    public Color m_ZeroHealthColor = Color.red;    
    //爆炸预制体
    public GameObject m_ExplosionPrefab;


    private AudioSource m_ExplosionAudio;
    //坦克爆炸特效
    private ParticleSystem m_ExplosionParticles;
    //坦克当前血量
    private float m_CurrentHealth;
    //判断是否死亡，true是死亡 false生存
    private bool m_Dead;


    private void Awake()
    {
        //从获取爆炸预制体中获取爆炸特效
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();
        //从获取爆炸预制体中获取爆炸声音组件
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();
        //隐藏爆炸特效
        m_ExplosionParticles.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        //将当前血量设置成初始血量
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI();
    }


    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        //减少血量
        m_CurrentHealth -= amount;

        SetHealthUI();

        if (m_CurrentHealth<=0 && !m_Dead)
        {
           OnDeath();
        }

    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.

        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth / m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        gameObject.SetActive(false);
        //播放死亡特效
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();

        //播放死亡声音
        m_ExplosionAudio.Play();
    }
}