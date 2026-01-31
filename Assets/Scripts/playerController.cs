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
    [SerializeField] MusicManager musicManager;
    [SerializeField] SoundManager soundManager;


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
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (SystemInfo.operatingSystem.Contains("Mac")) vertical *= -1;

        rb2d.linearVelocity = new Vector2(
            horizontal * speed * Time.deltaTime,
            vertical * speed * Time.deltaTime);
        if (!isMaskOn) health.Value -= dmgTime * Time.deltaTime;
        if (maskSO.Value >= 0)
            maskSO.Value -= Time.deltaTime;
        else
            removeMask();
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

    private void putMaskOn(Collider2D collision)
    {
        isMaskOn = true;
        maskSO.Value = maskTimer;
        canvasController.putMaskOn();
        Destroy(collision.gameObject);
        Debug.Log("Mask put on");
        soundManager.Play(SoundId.MaskOn);
        //musicManager.ActivateSecondaryTrack(4.5f);
    }

    private void removeMask()
    {
        isMaskOn = false;
        soundManager.Play(SoundId.MaskOff);
        canvasController.putMaskOff();
        Debug.Log("Mask removed");
    }
}
