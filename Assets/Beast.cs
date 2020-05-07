using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.UI;

public class Beast : MonoBehaviour
{
    public float baseHitPoints = 1;
    public float newBaseHitPoints = 1;
    public float attack = 5F;
    public float speed = 0.4F;
    public bool isClose;
    public float visibilityRange = 1.7F;

    public float attackCoolOff = 2F;
    private GameObject player;
    private InfoMessage infoMessage;
    private float lastAttack = 0;
    private MeshRenderer meshRenderer;
    private Material originalMeshMaterial;
    public Material hitMeshMaterial;

    private Text hitNotification;
    // Start is called before the first frame update
    private void Start()
    {
        newBaseHitPoints = Random.Range(40 + baseHitPoints, 110 + baseHitPoints);
        transform.localScale = new Vector3(transform.localScale.x * (newBaseHitPoints / 100), transform.localScale.y * (newBaseHitPoints / 100), transform.localScale.z * (newBaseHitPoints / 100));
        player = GameObject.FindGameObjectWithTag("Player");
        infoMessage = GameObject.FindGameObjectWithTag("InfoCanvas") ? GameObject.FindGameObjectWithTag("InfoCanvas").GetComponent<InfoMessage>() : null;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        originalMeshMaterial = meshRenderer.material;

        if(GameObject.FindGameObjectWithTag("HIT") != null)
            hitNotification = GameObject.FindGameObjectWithTag("HIT").GetComponent<Text>();
    }

    private void Update()
    {
        if (newBaseHitPoints <= 0 && this.transform.eulerAngles.z < 90)
        {
            Died();
        }
        else if (newBaseHitPoints <= 0 && this.transform.eulerAngles.z >= 90 && Vector3.Distance(transform.position, player.transform.position) >= visibilityRange)
        {
            //HideMe();
            GameObject.Destroy(this.gameObject);
        }
        else if (baseHitPoints <= 0 && this.transform.eulerAngles.z >= 90 && Vector3.Distance(transform.position, player.transform.position) < visibilityRange)
        {
            //Show dead animal
            this.GetComponentsInChildren<MeshRenderer>().All(x => { x.enabled = true; return x; });
            this.GetComponentsInChildren<Collider>().All(x => { x.enabled = false; return x; });
        }
        else if (newBaseHitPoints > 0 && Vector3.Distance(transform.position, player.transform.position) < visibilityRange)
        {
            GoneHunting();
            Vector3 closestPoint = transform.GetComponent<Collider>().ClosestPointOnBounds(player.transform.position);
            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(closestPoint, player.transform.position) < 0.03f)
            {
                //ATTACK
                if (lastAttack + attackCoolOff < Time.time)
                {
                    lastAttack = Time.time;

                    player.GetComponent<Player>().Hit(attack);
                }
            }
        }
        else
        {
            HideMe();
        }
    }

    public void HideMe()
    {
        this.GetComponentsInChildren<MeshRenderer>().All(x => { x.enabled = false; return x; });
    }

    public void Hit(float attackValue)
    {
        if ((newBaseHitPoints - attackValue) <= 0)
        {
            newBaseHitPoints = 0;
        }
        else
        {
            newBaseHitPoints -= attackValue;
        }

        if (hitNotification != null)
            hitNotification.text = "X";

        StartCoroutine(SwitchMaterials());

    }

    IEnumerator SwitchMaterials()
    {
        meshRenderer.material = hitMeshMaterial;
        yield return new WaitForSeconds(0.1F);
        meshRenderer.material = originalMeshMaterial;

        yield return new WaitForSeconds(0.2F);
        if (hitNotification != null)
            hitNotification.text = "";
    }

    public void Died()
    {
        GetComponent<AudioSource>().enabled = false;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 90), 50 * Time.deltaTime);
    }

    private void GoneHunting()
    {
        this.GetComponentsInChildren<MeshRenderer>().All(x => { x.enabled = true; return x; });
        transform.LookAt(player.transform);
        //IF CLOSER THAN 2F THEN MOVE TOWARDS PLAYER
        if (Vector3.Distance(transform.position, transform.GetComponent<Collider>().ClosestPointOnBounds(player.transform.position)) < 2F)
        {
            if (isClose)
            {
                // MOVE AWAY
                float step = speed * Time.deltaTime; // calculate distance to move
                Vector3 newPosition = player.transform.position;
                newPosition.x -= Random.Range(0.4F, 1.5F);
                newPosition.z -= Random.Range(0.4F, 1.5F);
                Vector3 closestPoint = transform.GetComponent<Collider>().ClosestPointOnBounds(newPosition);

                if (Vector3.Distance(closestPoint, newPosition) < 0.2f)
                {
                    isClose = false;
                }
                transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
            }
            else
            {
                // MOVE TOWARDS
                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
                Vector3 closestPoint = transform.GetComponent<Collider>().ClosestPointOnBounds(player.transform.position);
                // Check if the position of the cube and sphere are approximately equal.
                if (Vector3.Distance(closestPoint, player.transform.position) < 0.025f)
                {
                    isClose = true;
                }
            }
        }
    }
}

