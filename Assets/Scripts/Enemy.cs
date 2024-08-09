using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using TMPro.EditorUtilities;

public class Enemy : GameBehaviour
{
    public EnemyType myType;

    public float moveDistance = 1000f;
    public float stopDistance = 0.3f;
    public float attackRange = 3f;
    public float detectDistance = 10f;
    public float chaseDistance = 6f;
    public float detectTime = 5f;
    private float currentDetectTime;

    public PatrolType myPatrol;     //What patrol type we are
    private Transform moveToPos;    //Needed for all patrol
    private Transform startPos;     //Needed for Loop patrol movement
    private Transform endPos;       //Needed for Loop patrol movement
    private bool reverse;           //Needed for Loop patrol movement
    private int patrolPoint;        //Needed for Linear patrol movement

    private float mySpeed = 5f;
    private int myHealth = 100;

    private Animator anim;
    private NavMeshAgent agent;
    private AudioSource audioSource;

    private void ChangeSpeed(float _speed) => agent.speed = _speed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (myPatrol == PatrolType.Die)
            return;

        //Get the distance between us and the player
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);
        if(distToPlayer < detectDistance && myPatrol != PatrolType.Attack) 
        {
            if(myPatrol != PatrolType.Chase)
                myPatrol = PatrolType.Detect;
        }

        //Set the animators speed parameter to mySpeed
        anim.SetFloat("Speed", agent.speed);

        switch(myPatrol)
        {
            case PatrolType.Patrol:
                //Get the distance between us and the destination
                float distToDestination = Vector3.Distance(transform.position, endPos.position);
                //If the distance is close enough, run the AI setup again
                if (distToDestination < 1)
                    SetupAI();
                break;

            case PatrolType.Detect:
                //Set the destination to ourselves, essentially stopping us
                agent.SetDestination(transform.position);
                ChangeSpeed(0);
                //Decrement our detect timer
                currentDetectTime -= Time.deltaTime;
                if(distToPlayer <= detectDistance)
                {
                    myPatrol = PatrolType.Chase;
                    currentDetectTime = detectTime;
                }
                if(currentDetectTime <= 0)
                {
                    SetupAI();
                }
                break;

            case PatrolType.Chase:
                //Set the destination to that of the player
                agent.SetDestination(_PLAYER.transform.position);
                //Increase the speed of the agent
                ChangeSpeed(mySpeed * 1.5f);
                //If the player gets outside of the detect distance, then return to detect state
                if (distToPlayer > detectDistance)
                    myPatrol = PatrolType.Detect;
                //If we are close to the player, then attack
                if (distToPlayer < attackRange)
                    StartCoroutine(Attack());
                break;
        }
    }

    public void Setup(Transform _startPos)
    {
        switch (myType)
        {
            case EnemyType.OneHanded:
                mySpeed = 2f;
                myHealth = 100;
                myPatrol = PatrolType.Patrol;
                break;
            case EnemyType.TwoHanded:
                mySpeed = 1f;
                myHealth = 200;
                myPatrol = PatrolType.Patrol;
                break;
            case EnemyType.Archer:
                mySpeed = 3f;
                myHealth = 75;
                myPatrol = PatrolType.Patrol;
                break;
        }
        SetupAI();
    }

    private void SetupAI()
    {
        myPatrol = PatrolType.Patrol;
        currentDetectTime = detectTime;
        //startPos = _startPos;
        endPos = _EM.GetRandomSpawnPoint();
        moveToPos = endPos;

        ChangeSpeed(mySpeed);
        agent.SetDestination(endPos.position);
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
        myPatrol = PatrolType.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack");
        _AUDIO.PlayAttackSound(audioSource);
        yield return new WaitForSeconds(1);
        myPatrol = PatrolType.Chase;
    }

    public void Hit(int _damage)
    {
        myHealth -= _damage;
        if (myHealth <= 0)
            Die();
        else
        {
            PlayAnimation("Hit");
            _AUDIO.PlayHitSound(audioSource);
            GameEvents.ReportOnEnemyHit(gameObject);
        }
    }

    private void Die()
    {
        myPatrol = PatrolType.Die;
        ChangeSpeed(0);
        StopAllCoroutines();
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die");
        _AUDIO.PlayDieSound(audioSource);
        GameEvents.ReportOnEnemyDie(gameObject);
    }

    private void PlayAnimation(string _animationName)
    {
        int rnd = Random.Range(1, 4);
        anim.SetTrigger(_animationName + rnd);
    }

    public void PlayFootstep()
    {
        _AUDIO.PlayEnemyFootsteps(audioSource);
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
