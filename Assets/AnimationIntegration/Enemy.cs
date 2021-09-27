using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform finishingPosition;
    public Animator animator;
    public Rigidbody[] parts;
    public float respawnTime;
    public bool isDead = false;

    private void Start() 
    {
        SetKinematic(true);    
    }
    
    private void SetKinematic(bool isKinematic)
    {
        animator.enabled = isKinematic;
        foreach(Rigidbody part in parts)
        {
            part.isKinematic = isKinematic;
        }
    }

    public IEnumerator DeathSequence(float hitTime)
    {
        isDead = false;
        yield return new WaitForSeconds(hitTime);
        SetKinematic(false);
        StartCoroutine(Respawn());
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnTime);
        SetKinematic(true);
        Vector3 position = new Vector3(Random.Range(-8f,8f), 0, Random.Range(-8f,8f)); 
        transform.position = position;
        isDead = false;
    }

}
