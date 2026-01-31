using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] float speed = 500f;
    [SerializeField] float dmgTime = 100f;
    [SerializeField] int maskTimer = 5;

    [SerializeField] FloatSO maskSO;
    [SerializeField] FloatSO health;

    [SerializeField] canvasController canvasController;

    Rigidbody2D rb2d;
    bool isMaskOn = false;

    void Start()
    {
        health.Value = 100f;
        rb2d = GetComponent<Rigidbody2D>();

        // לוודא מצב התחלה הגיוני
        isMaskOn = false;
        maskSO.Value = 0f;
    }

    void Update()
    {
        rb2d.linearVelocity = new Vector2(
            Input.GetAxis("Horizontal") * speed * Time.deltaTime,
            Input.GetAxis("Vertical") * speed * Time.deltaTime
        );

        // נזק רק אם אין מסכה
        if (!isMaskOn)
            health.Value -= dmgTime * Time.deltaTime;

        // טיימר מסכה יורד רק כשהמסכה פעילה
        if (isMaskOn)
        {
            maskSO.Value -= Time.deltaTime;

            if (maskSO.Value <= 0f)
                removeMask();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mask"))
        {
            Destroy(collision.gameObject);
            isMaskOn = true;
            maskSO.Value = maskTimer;
        }

        if (collision.CompareTag("Enemy"))
        {
            // חדש: Overlay
            if (canvasController != null)
                canvasController.StartDamageOverlay();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // חדש: Overlay
            if (canvasController != null)
                canvasController.StopDamageOverlay();
        }
    }

    void removeMask()
    {
        isMaskOn = false;
        maskSO.Value = 0f;
    }
}
