using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] float speed = 500;
    [SerializeField] float dmgTime = 100;
    [SerializeField] int maskTimer = 5;
    [SerializeField] FloatSO maskSO;
    Rigidbody2D rb2d;
    bool isMaskOn = false;

    [SerializeField] FloatSO health; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health.Value = 100;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        if (!isMaskOn) health.Value -= dmgTime * Time.deltaTime;
        if (maskSO.Value >= 0) maskSO.Value -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mask"))
        {
            Destroy(collision.gameObject);
            isMaskOn = true;
            maskSO.Value = maskTimer;
            Invoke("removeMask", maskTimer);

        }
    }

    void removeMask()
    {
        isMaskOn = false;
    }
}
