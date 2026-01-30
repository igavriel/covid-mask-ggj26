using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] float speed = 500;
    [SerializeField] float dmgTime = 100;
    [SerializeField] int maskTimer = 5;
    [SerializeField] FloatSO maskSO;
    [SerializeField] FloatSO health;
    [SerializeField] IntSO collectables;
    [SerializeField] canvasController canvasController;

    Rigidbody2D rb2d;
    bool isMaskOn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health.Value = 100;
        collectables.Value = 0;
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2d.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, Input.GetAxis("Vertical") * speed * Time.deltaTime);
        if (!isMaskOn) health.Value -= dmgTime * Time.deltaTime;
        if (maskSO.Value >= 0)
            maskSO.Value -= Time.deltaTime;
        else
            removeMask();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if (collision.CompareTag("Collectable"))
        // {
        //     Destroy(collision.gameObject);
        //     collectables.Value++;
        // }

        if (collision.CompareTag("Mask"))
        {
            Destroy(collision.gameObject);
            isMaskOn = true;
            maskSO.Value = maskTimer;

        }

        if (collision.CompareTag("Enemy"))
        {
            canvasController.dmgStart();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            canvasController.dmgStop();
        }
    }

    void removeMask()
    {
        isMaskOn = false;
    }
}
