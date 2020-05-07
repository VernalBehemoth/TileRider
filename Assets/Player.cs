using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public bool GodModeEditor;
    public static bool GodMode;
    public Animator animator;
    public GameManagerScript gameManager;
    public float attackValue = 10;
    private readonly float attackCoolOff = 0.25F;
    public bool isDead;
    public float hitPoints;
    private float range = 0.3F;
    //private float delayAttackUntil = 1F;
    private InfoMessage infoMessage;
    private Vector3 higherPosition;
    private PostProcessVolume activeVolume;
    private Vignette vignette;
    private Color originalColor = new Color();
    private float originalIntensity;
    private float cooldownCounter = 1F;
    private bool coolingdown = false;
    public AudioClip hitSound;

    public static bool isHitting;

    public static bool isRunning = false;
    private void Start()
    {
        activeVolume = GameObject.FindGameObjectWithTag("PostProcessVolume") ? GameObject.FindGameObjectWithTag("PostProcessVolume").GetComponent<PostProcessVolume>() : null;
        if (activeVolume != null)
        {
            activeVolume.GetComponent<PostProcessVolume>();
            activeVolume.profile.TryGetSettings(out vignette);
        }
        originalColor = vignette.color.value;
        originalIntensity = vignette.intensity.value;
        //higherPosition = (transform.forward * -0.3F) + transform.position;
        // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // sphere.transform.position = higherPosition;
        // sphere.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);
        if (GameObject.FindGameObjectWithTag("InfoCanvas") != null)
        {
            infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas") ? GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>() : null;
        }
        GodMode = false;
        GodModeEditor = false;
    }

    public void Setup(float currentHealth)
    {
        SetHealth(currentHealth);
    }

    public void SetHealth(float currentHealth)
    {
        hitPoints = currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isHitting)
        {
            if (other.transform.tag == "Beast")
            {
                if (other.transform.GetComponent<Beast>() != null)
                {
                    if (!coolingdown)
                    {
                        if (GodMode)
                        {
                            other.transform.GetComponent<Beast>().Hit(1000);
                        }
                        else
                        {
                            other.transform.GetComponent<Beast>().Hit(attackValue);
                        }

                        GetComponent<AudioSource>().PlayOneShot(hitSound);
                        coolingdown = true;
                    }
                    else { Debug.Log("is cooling"); }
                }
            }
        }
    }

    private IEnumerator ResetVignette()
    {
        yield return new WaitForSeconds(0.5F);

        vignette.color.value = originalColor;
        vignette.intensity.value = originalIntensity;

    }

    public void Hit(float points)
    {
        if (hitPoints - points > 0)
        {
            if (GodMode)
            {
                if (infoMessage != null)
                {
                    infoMessage.DisplayMessage("GOD MODE", 2F);
                }
            }
            else
            {
                hitPoints -= points;
                if (infoMessage != null)
                {
                    infoMessage.DisplayMessage("YOU ARE HIT! RUN YOU FOOLS", 2F);
                }

                if (gameManager != null)
                {
                    gameManager.TakeDamage(points);
                }

                if (activeVolume != null)
                {
                    if (vignette != null)
                    {
                        vignette.color.value = new Color(50, 0, 0, 255);
                        vignette.intensity.value = 0.02F;
                        StartCoroutine(ResetVignette());
                    }
                }
            }
        }
        else if (!isDead)
        {
            isDead = true;
            if (infoMessage != null)
            {
                infoMessage.DisplayMessage("YOU ARE DEAD", 100, true);
            }

            Debug.Log("DEAD");
            if (activeVolume != null)
            {
                activeVolume.profile.TryGetSettings(out vignette);
                if (vignette != null)
                {
                    vignette.intensity.value = 100F;
                }
            }
            // Game Over, load gameoverscene
            SceneManager.LoadScene("GameOverScene");
        }
    }

    private void Update()
    {
        #if UNITY_EDITOR
            GodMode = GodModeEditor;
        #endif
        if (coolingdown)
        {
            cooldownCounter -= Time.deltaTime;

            if (cooldownCounter <= 0)
            {
                coolingdown = false;
                cooldownCounter = attackCoolOff;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("IsAttacking2", true);
            animator.SetBool("IsAttacking", true);

            if (Input.GetKey(KeyCode.W))
            {
                animator.SetBool("IsAttacking2", true);
                animator.SetBool("IsAttacking", false);
                range = 0.3F;

            }
            if (Input.GetKey(KeyCode.S))
            {
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsAttacking2", false);
                range = 0.5F;
            }

            // if (!GetComponent<AudioSource>().isPlaying)
            // {
            //     GetComponent<AudioSource>().enabled = true;
            //     GetComponent<AudioSource>().clip = audioSources[0];
            //     GetComponent<AudioSource>().loop = false;
            //     GetComponent<AudioSource>().Play();
            // }
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            animator.SetBool("IsWalking", true);

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("IsAttacking2", true);
                animator.SetBool("IsAttacking", false);
                range = 0.3F;

            }
            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("IsAttacking", true);
                animator.SetBool("IsAttacking2", false);
                range = 0.5F;
            }
        }


        if (Input.GetKeyUp(KeyCode.Space))
        {
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsAttacking2", false);
            range = 0.3F;
            //GetComponent<AudioSource>().loop = false;
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            if (Input.GetKey(KeyCode.Space))
            {
                animator.SetBool("IsAttacking2", true);
            }

            animator.SetBool("IsWalking", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {

            isRunning = true;
        }
        else { isRunning = false; }
        //if ((animator.GetBool("IsAttacking") || animator.GetBool("IsAttacking2")) && Input.GetKey(KeyCode.Space))
        //{

        //    // // Bit shift the index of the layer (8) to get a bit mask
        //    // int layerMask = LayerMask.GetMask("Beast");

        //    // higherPosition = (transform.forward * -0.3F) + transform.position;


        //    // // Does the ray intersect any objects excluding the player layer
        //    // //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit, 0.3F, layerMask))
        //    // if (Physics.SphereCast(higherPosition, 0.5F, transform.TransformDirection(Vector3.forward), out RaycastHit hit, range, layerMask))
        //    // {
        //    //     Debug.DrawRay(higherPosition, transform.TransformDirection(Vector3.forward) * range, Color.green);
        //    //     if (lastAttack + attackCoolOff < Time.time)
        //    //     {
        //    //         lastAttack = Time.time;
        //    //         hit.transform.gameObject.GetComponent<Beast>().Hit(attackValue);
        //    //     }
        //    //     else
        //    //     {

        //    //     }
        //    // }
        //    // else
        //    // {

        //    //     Debug.DrawRay(higherPosition, transform.TransformDirection(Vector3.forward) * 1, Color.red);
        //    //     // GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //    //     // sphere.transform.position = higherPosition;
        //    //     // sphere.transform.localScale = new Vector3(0.75F, 0.75F, 0.75F);
        //    //     //Debug.Log("Did not Hit");
        //    // }
        //}

    }

    public void ToggleGodMode()
    {
        GodMode = !GodMode;
        GodModeEditor = GodMode;
    }
}

