using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{
    public Transform targetBone;
    public Animator animator;
    public Vector3 rot;
    public LayerMask aimLayer;

    public float speed;
    public float rotateSpeed;

    public Camera cam;

    public float animationDuration;
    public float animationHitTime;
    public float finishingDistance;
    public float finishingMoveSpeed;
    public Enemy enemy;
    public GameObject rifleAttach;
    public GameObject swordAttach;
    public GameObject tipText;

    private bool finishing = false;

 
    private void Update()
    {
        if (finishing)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        if (direction.magnitude >= 1f)
            direction.Normalize();

            transform.Translate(-direction * speed * Time.deltaTime, Space.World);

        float VelocityX = direction.magnitude;      
        animator.SetFloat("VelocityX", VelocityX, 0.1f, Time.deltaTime);
        RotateTowardMovement(-direction);

        if (Vector3.Distance(transform.position, enemy.finishingPosition.position) < finishingDistance && !enemy.isDead)
        {
            tipText.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(MoveToFinishingPosition(enemy.finishingPosition.position));                
            }
        }
        else
        {
            tipText.SetActive(false);
        }

    }
    
    private void LateUpdate()
    {
        if (!finishing)
            RotateFromMouseVector();
    }

    private IEnumerator MoveToFinishingPosition(Vector3 position)
    {
        finishing = true;
        var dif = Vector3.Distance(transform.position, position);
        while (transform.position != position)
        {        
            transform.position = Vector3.MoveTowards(transform.position, position, finishingMoveSpeed * Time.deltaTime);
            transform.rotation = enemy.transform.rotation;
            targetBone.rotation = enemy.transform.rotation;
            yield return null;
        }    
        StartCoroutine(Finishing());
    }

    private IEnumerator Finishing()
    {
        animator.SetTrigger("Finishing");
        rifleAttach.SetActive(false);
        swordAttach.SetActive(true);
        StartCoroutine(enemy.DeathSequence(animationHitTime)); 

        yield return new WaitForSeconds(animationDuration);
        rifleAttach.SetActive(true);
        swordAttach.SetActive(false);
        finishing = false;

    }

    public void SwordHit()
    {
        Debug.Log("hit");
    }


    private void RotateTowardMovement(Vector3 movement)
    {
        if (movement.magnitude == 0) 
            return;

        var rotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }
 
    private void RotateFromMouseVector()
    {
        var mousePos = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, aimLayer))
        {
            Vector3 target = hitInfo.point;
            target.y = transform.position.y;

            Vector3 lookDir = target - transform.position;
            float angle = Mathf.Atan2(lookDir.z, lookDir.x) * Mathf.Rad2Deg - 90f;
            targetBone.localEulerAngles = new Vector3(transform.localEulerAngles.y + angle, 0, 0); 
        }
    }
}
