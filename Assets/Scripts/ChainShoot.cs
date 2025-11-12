using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainShoot : MonoBehaviour
{
    [SerializeField] float refreshRate = 0.01f;
    [SerializeField][Range(1, 10)] int maximumEnemiesInChain = 3;
    [SerializeField] float delayBetweenEachChain = 0.3f;
    [SerializeField] Transform playerFirePoint;
    [SerializeField] EnemyDetector playerEnemyDetector;
    [SerializeField] GameObject lineRendererPrefab;

    bool shooting;
    bool shot;
    float counter = 1;
    GameObject currentClosestEnemy;
    List<GameObject> spawnedLineRenderers = new List<GameObject>();
    List<GameObject> enemiesInChain = new List<GameObject>();
    List<GameObject> activeEffect = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if(playerEnemyDetector.GetEneiesInRange().Count > 0)
            {
                if(shooting)
                {
                    StartShooting();
                }
            }
            else
            {
                StopShooting();
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            StopShooting();
        }
    }

    void StopShooting()
    {
        shooting = false;
        shot = false;
        counter = 1;

        for (int i = 0; i <  spawnedLineRenderers.Count; i++)
        {
            Destroy(spawnedLineRenderers[i]);
        }

        spawnedLineRenderers.Clear();
        enemiesInChain.Clear();

        for (int i = 0;i < activeEffect.Count;i++)
        {
            Destroy(activeEffect[i]);
        }
        activeEffect.Clear();
    }

    void StartShooting()
    {
        shooting = false;
        shot = false;
        counter = 1;

        for (int i = 0; i < spawnedLineRenderers.Count; i++)
        {
            Destroy(spawnedLineRenderers[i]);
        }

        spawnedLineRenderers.Clear();
        enemiesInChain.Clear();

        for (int i = 0; i < activeEffect.Count; i++)
        {
            Destroy(activeEffect[i]);
        }
        activeEffect.Clear();
    }

    IEnumerator UpdateLineRenderer(GameObject lineR, Transform starPos, Transform endPos, bool getClosestEnemyToPlayer = false)
    {
        if (shooting && shot && lineR != null)
        {
            lineR.GetComponent<LineRendererController>().SetPosition(starPos, endPos);
            
            yield return new WaitForSeconds(refreshRate);

            if (getClosestEnemyToPlayer)
            {
                StartCoroutine(UpdateLineRenderer(lineR, starPos, playerEnemyDetector.GetClosestEnemy().transform,true));

                if(currentClosestEnemy != playerEnemyDetector.GetClosestEnemy())
                {
                    StopShooting();
                    //StartShootiong();
                }
            }
            else
            {
                StartCoroutine(UpdateLineRenderer(lineR, starPos, endPos));
            }
        }
    }

    void NewLineRenderer(Transform starPos, Transform endPos, bool getClosestEnemyToPlayer = false)
    {
        GameObject lineR = Instantiate(lineRendererPrefab);
        spawnedLineRenderers.Add(lineR);
        StartCoroutine(UpdateLineRenderer(lineR, starPos, endPos, getClosestEnemyToPlayer));
    }

    IEnumerator ChainReaction(GameObject closestEnemy)
    {
        yield return new WaitForSeconds(delayBetweenEachChain);

        if (counter == maximumEnemiesInChain)
        {
            yield return null;
        }
        else
        {
            if(shooting)
            {
                counter++;
                enemiesInChain.Add(closestEnemy);

                if(!enemiesInChain.Contains(closestEnemy.GetComponent<EnemyDetector>().GetClosestEnemy()))
                {
                    NewLineRenderer(closestEnemy.transform, closestEnemy.GetComponent<EnemyDetector>().GetClosestEnemy().transform);
                    StartCoroutine(ChainReaction(closestEnemy.GetComponent<EnemyDetector>().GetClosestEnemy()));
                }
            }
        }
    }
}
