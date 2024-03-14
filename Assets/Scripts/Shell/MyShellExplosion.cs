using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviorScript : MonoBehaviour
{
    //�趨����ʱ��
    public float MaxLifeTime = 2f;
    //�趨��Ч�ļ�
    public ParticleSystem ExplosionParticles;
    //�趨�����ļ�
    public AudioSource ExplosionAudio;
    public LayerMask TankMask;
    public float ExplosionForce = 1000f;
    public float MaxDamage = 100f;
    public float ExplosionRadius = 5f;
    // Start is called before the first frame update
    void Start()
    {
        //�趨�����ڵ����ʱ��
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
            //��ȡ�������
            Rigidbody target = targetCollider[i].GetComponent<Rigidbody>();

            if (target == null)
                continue;

            target.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);

            TankHealth targetTankHealth = targetCollider[i].GetComponent<TankHealth>();

            float damage = CalculateDamage(target.gameObject.transform.position);

            targetTankHealth.TakeDamage(damage);
            
        }


        //������Ч�Ӽ�
        ExplosionParticles.transform.parent = null;
        //������Ч
        ExplosionParticles.Play();
        //��������
        ExplosionAudio.Play();
        //�����ڵ��ļ�
        Destroy(gameObject);
        //������Ч�ļ�
        Destroy(ExplosionParticles.gameObject,ExplosionParticles.main.duration);
    }
    private float CalculateDamage(Vector3 targetPosition) 
    {
        //����̹���뱬ը����
        Vector3 v3 = targetPosition - transform.position;
        float distance = v3.magnitude;
        float damage = (ExplosionRadius - distance) / ExplosionRadius * MaxDamage ;
        damage = Mathf.Max(0,damage);
        return damage;
    }
}
