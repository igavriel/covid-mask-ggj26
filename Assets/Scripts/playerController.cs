using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] float speed = 500f;
    [SerializeField] float dmgTime = 100f;
    [SerializeField] int maskTimer = 5;

    [SerializeField] FloatSO maskSO;
    [SerializeField] FloatSO health;
    [SerializeField] IntSO collectables;
    [SerializeField] canvasController canvasController;
    [SerializeField] MusicManager musicManager;

    Animator animatorController;

    Rigidbody2D rb2d;
    bool isMaskOn = false;
    bool isOnEnemy = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health.Value = 100;
        collectables.Value = 0;
        rb2d = GetComponent<Rigidbody2D>();
        animatorController = GetComponent<Animator>();

        // לוודא מצב התחלה הגיוני
        isMaskOn = false;
        maskSO.Value = 0f;
    }

    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (SystemInfo.operatingSystem.Contains("Mac")) vertical *= -1;

        rb2d.linearVelocity = new Vector2(
            horizontal * speed * Time.deltaTime,
            vertical * speed * Time.deltaTime);
        if (!isMaskOn) health.Value -= dmgTime * Time.deltaTime;
        if (maskSO.Value >= 0)
            maskSO.Value -= Time.deltaTime;

            if (maskSO.Value <= 0f)
                removeMask();

        handleAnim();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: Add collectable logic
        // if (collision.CompareTag("Collectable"))
        // {
        //     Destroy(collision.gameObject);
        //     collectables.Value++;
        // }

        if (collision.CompareTag("Mask"))
        {
            putMaskOn(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // חדש: Overlay
            if (canvasController != null)
                canvasController.StartDamageOverlay();
            isOnEnemy = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (canvasController != null)
                canvasController.StopDamageOverlay();
        }
    }

    private void putMaskOn(Collider2D collision)
    {
        isMaskOn = true;
        maskSO.Value = maskTimer;
        canvasController.putMaskOn();
        Destroy(collision.gameObject);
        Debug.Log("Mask put on");
        musicManager.ActivateSecondaryTrack(4.5f);
    }

    private void removeMask()
    {
        isMaskOn = false;
        canvasController.putMaskOff();
        Debug.Log("Mask removed");
    }

    void handleAnim()
    {
        if (Input.GetAxis("Horizontal") > 0) GetComponent<SpriteRenderer>().flipX = false;
        else if (Input.GetAxis("Horizontal") < 0) GetComponent<SpriteRenderer>().flipX = true;

        animatorController.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        animatorController.SetFloat("Vertical", Input.GetAxis("Vertical"));

        animatorController.SetBool("isWalking", Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") != 0);
    }
}
