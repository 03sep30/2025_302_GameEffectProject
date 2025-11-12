using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private float detectionRdadius = 10.0f;
    [SerializeField] private LayerMask enemyLayer;

    public GameObject GetClosestEnemy()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRdadius, enemyLayer);

        if(enemiesInRange.Length > 0)
        {
            GameObject bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPostion = transform.position;
            
            foreach(Collider enemyCollider in enemiesInRange)
            {
                if (enemyCollider.gameObject == this)
                    continue;

                Vector3 directionToTarget = enemyCollider.transform.position = currentPostion;
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                if (dSqrToTarget <  closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = enemyCollider.gameObject;
                }
            }
            return bestTarget;
        }
        else
        {
            return null;
        }
    }

    public List<GameObject> GetEneiesInRange()
    {
        List<GameObject> enemiesList = new List<GameObject>();
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, detectionRdadius, enemyLayer);
        
        foreach(Collider enemyCollider in enemiesInRange)
        {
            if(enemyCollider.gameObject != this.gameObject)
            {
                enemiesList.Add(enemyCollider.gameObject);
            }
        }
        return enemiesList;
    }
}
