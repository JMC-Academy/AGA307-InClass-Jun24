using UnityEngine;
using System.Collections;

public class Enemy : GameBehaviour
{
    public EnemyType myType;

    public float moveDistance = 1000f;
    public float stopDistance = 0.3f;
    public float attackRange = 3;

    private PatrolType myPatrol;    //What patrol type we are
    private Transform moveToPos;    //Needed for all patrol
    private Transform startPos;     //Needed for Loop patrol movement
    private Transform endPos;       //Needed for Loop patrol movement
    private bool reverse;           //Needed for Loop patrol movement
    private int patrolPoint;        //Needed for Linear patrol movement

    private float mySpeed = 5f;
    private int myHealth = 100;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Setup(Transform _startPos)
    {
        switch (myType)
        {
            case EnemyType.OneHanded:
                mySpeed = 5f;
                myHealth = 100;
                myPatrol = PatrolType.Random;
                break;
            case EnemyType.TwoHanded:
                mySpeed = 3f;
                myHealth = 200;
                myPatrol = PatrolType.Loop;
                break;
            case EnemyType.Archer:
                mySpeed = 7f;
                myHealth = 75;
                myPatrol = PatrolType.Linear;
                break;
        }

        startPos = _startPos;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = endPos;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        switch(myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.GetSpawnPoint(patrolPoint);
                patrolPoint = patrolPoint != _EM.GetSpawnPointsCount() - 1 ? patrolPoint + 1 : 0;
                break;
            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint();
                break;
            case PatrolType.Loop:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;
        }

        transform.LookAt(moveToPos);
        while(Vector3.Distance(transform.position, moveToPos.position) > stopDistance)
        {
            if(Vector3.Distance(transform.position, _PLAYER.transform.position) < attackRange)
            {
                StopAllCoroutines();
                StartCoroutine(Attack());
                break;
            }
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, Time.deltaTime * mySpeed);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Move());
    }

    private IEnumerator Attack()
    {
        PlayAnimation("Attack");
        yield return new WaitForSeconds(1);
        StartCoroutine(Move());
    }

    public void Hit(int _damage)
    {
        myHealth -= _damage;
        if (myHealth <= 0)
            Die();
        else
        {
            PlayAnimation("Hit");
            GameEvents.ReportOnEnemyHit(gameObject);
        }
    }

    private void Die()
    {
        StopAllCoroutines();
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die");
        GameEvents.ReportOnEnemyDie(gameObject);
    }

    private void PlayAnimation(string _animationName)
    {
        int rnd = Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

    /*
    private IEnumerator Move()
    {
        for(int i = 0;i<moveDistance;i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * mySpeed);
            yield return null;
        }
        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1, 3));
        StartCoroutine(Move());
    }
    */
}
