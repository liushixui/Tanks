using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviorScript : MonoBehaviour
{
    //设定销毁时间
    public float MaxLifeTime = 2f;
    //设定特效文件
    public ParticleSystem ExplosionParticles;
    //设定声音文件
    public AudioSource ExplosionAudio;
    public LayerMask TankMask;
    public float ExplosionForce = 1000f;
    public float MaxDamage = 100f;
    public float ExplosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //设定销毁炮弹最大时间
        Destroy(gameObject,MaxLifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Collider[] targetCollider = Physics.OverlapSphere(transform.position,ExplosionRadius,TankMask);
        for (int i = 0; i < targetCollider.Length; i++)
        {
            //获取刚体组件
            Rigidbody target = targetCollider[i].GetComponent<Rigidbody>();

            if (target == null)
                continue;

            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            TankHealth targetTankHealth = targetCollider[i].GetComponent<TankHealth>();

            float damage = CalculateDamage(target.gameObject.transform.position);

            targetTankHealth.TakeDamage(damage);
            
        }


        //剥离特效子集
        ExplosionParticles.transform.parent = null;
        //播放特效
        ExplosionParticles.Play();
        //播放声音
        ExplosionAudio.Play();
        //销毁炮弹文件
        Destroy(gameObject);
        //销毁特效文件
        Destroy(ExplosionParticles.gameObject,ExplosionParticles.main.duration);
    }
    private float CalculateDamage(Vector3 targetPosition) 
    {
        //计算坦克与爆炸距离
        Vector3 v3 = targetPosition - transform.position;
        float distance = v3.magnitude;
        float damage = (ExplosionRadius - distance) / ExplosionRadius * MaxDamage ;
        damage = Mathf.Max(0,damage);
        return damage;
    }
}
