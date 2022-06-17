using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class TouchContoller : MonoBehaviour
{
    Vector2 startPos, endPos,currentPos;
    Touch touch;
    [SerializeField]Transform lookPos;
    bool isMoving = false;
   public  float timer;
    [SerializeField] Transform a, b, c, d, e, pota;
    [SerializeField] float f, u;
    // Start is called before the first frame update
    [SerializeField] Rigidbody childRb;
    [SerializeField] CinemachineVirtualCamera cam;
    bool indistance;
    [SerializeField] GameObject vfx;
    void Start()
    {
        a.transform.position = (b.position + transform.position) * 0.5f + Vector3.up * 2;
        c.position = transform.position + lookPos.forward * f + lookPos.up * u;
        //   d.position = transform.position + lookPos.forward * f * 2;
        d.position = transform.position + new Vector3(transform.position.x, 0, lookPos.forward.z * f * 2);
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        //  transform.rotation = lookPos.transform.rotation;
        Move();
        indistance = Vector3.Distance(transform.position, pota.position) <= 10f;
        if (!firla)
        {
            childRb.AddForce(Vector3.down * 50);
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -20, 1.51f), transform.position.z);
        }
            
            
    }
    public bool firla = false;
    private void Move()
    {
        if (Input.touchCount > 0)
        {

            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
                timer = 0f;
            }
            else if (touch.phase == TouchPhase.Stationary)
            {
              //  timer = 0f;
                float timer2 = 0;
                timer2 += Time.deltaTime;
                if (timer2 > 2f)
                {
                    startPos = touch.position;
                    
                }
                transform.Translate((lookPos.right * currentPos.normalized.x + lookPos.forward * currentPos.normalized.y) * Time.deltaTime * 5);
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                currentPos = touch.position - startPos;
                
            
                transform.Translate((lookPos.right * currentPos.normalized.x + lookPos.forward * currentPos.normalized.y) * Time.deltaTime * 5);

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                if (currentPos.magnitude > 100 && timer < 0.5f)
                {
                
                    d.position = transform.position + new Vector3(0, 1.5f-transform.position.y, lookPos.forward.z * f * 2);
                    
                    firla = true;
                 
                    if (indistance)
                    {
                      
                        cam.Priority = 11;
                        transform.DOJump(b.position, 5, 1, 1);
                    }
                    else
                    {
                   
                       
                        transform.DOJump(d.position, 3, 1, 1);
                    }

                }
            }
        }
    }
   
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
            firla = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pota"))
        {
            Win();
        }
    }
    void Win()
    {
        
        vfx.SetActive(true);
        Invoke(nameof(VfxKapat), 2f);
    }
    void VfxKapat()
    {
        vfx.SetActive(false);
        cam.Priority = 9;
    }

}
