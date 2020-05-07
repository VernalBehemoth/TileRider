using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMaker : MonoBehaviour
{
    [SerializeField]
    public GameObject environment = null;
    private List<GameObject> tiles = new List<GameObject>();

    public bool BridgeIsFixed;

    [SerializeField]
    public AnimationChoice animationChoice = AnimationChoice.Shrinking;

    public GameObject[] cameras;

    public void SetAnimation()
    {
        if (animationChoice == AnimationChoice.Falling)
            animationChoice = AnimationChoice.Shrinking;
        else
            animationChoice = AnimationChoice.Falling;
    }
    public void SetCamera()
    {
        foreach (GameObject go in cameras)
        {
            go.SetActive(!go.activeSelf);
        }
    }
    private void Start()
    {
        foreach (GameObject eachChild in GameObject.FindGameObjectsWithTag("TileCollider"))
        {
            if (eachChild.tag == "TileCollider")
            {
                if (eachChild.name != "PlayStartTiles")
                {
                    foreach (Transform eachChild2 in eachChild.transform)
                    {
                        if (eachChild2.tag != "TileCollider")
                        {
                            eachChild2.gameObject.SetActive(false);
                        }
                    }
                }
                tiles.Add(eachChild);
            }
        }


        StartCoroutine(FallingTiles());
    }

    private IEnumerator FallingTilesAnimation(Transform o)
    {

        int loopCount = 0;
        for (; ; )
        {
            if (o != null)
            {
                if (animationChoice == AnimationChoice.Falling)
                {
                    o.Translate(Vector3.down * Random.Range(2, 10) * Time.deltaTime * 0.1F, Space.World);
                    o.Rotate(Random.Range(1, 10), Random.Range(1, 10), Random.Range(1, 10), Space.World);
                }
                else if (animationChoice == AnimationChoice.Shrinking)
                {
                    if (o.localScale.x > 0 && o.localScale.y > 0 && o.localScale.z > 0)
                        o.localScale -= new Vector3(0.2F, 0.2F, 0.2F);
                    else
                        o.Translate(Vector3.down * Random.Range(2, 10) * Time.deltaTime * 1F, Space.World);
                }
            }

            yield return new WaitForSeconds(0.01F);

            loopCount++;
            if (loopCount >= 10) { break; }
        }

        if(o != null)
        GameObject.Destroy(o.gameObject);
    }


    private IEnumerator FallingTiles()
    {
        List<GameObject> previousTransformsCollidedWith = new List<GameObject>();

        for (; ; )
        {
            List<GameObject> transformsCollidedWith = new List<GameObject>();
            transformsCollidedWith.Clear();
            // Cast a sphere wrapping character controller 10 meters forward
            // to see if it is about to hit anything.
            int layerMask = LayerMask.GetMask("Tile");

            Vector3 playerPos = transform.position;
            Vector3 playerDirection = transform.forward;
            Quaternion playerRotation = transform.rotation;
            float spawnDistance = 1;

            Vector3 spawnPos = playerPos + playerDirection * spawnDistance;

            foreach (RaycastHit hit in Physics.SphereCastAll(spawnPos, 1.2F, -transform.up, 1F, layerMask))
            {
                transformsCollidedWith.Add(hit.transform.gameObject);

                foreach (Transform eachChild in hit.transform)
                {
                    //Dont show bridge if its not been fixed
                    if (eachChild.tag == "Bridge" && !BridgeIsFixed)
                    {
                        eachChild.transform.gameObject.SetActive(false);
                    }
                    //Show the children objects of the tile
                    else if (eachChild.tag != "TileCollider")
                    {
                        eachChild.transform.gameObject.SetActive(true);
                    }
                }
            }

            //Hide other tiles
            foreach (GameObject g in previousTransformsCollidedWith)
            {
                if (!transformsCollidedWith.Contains(g))
                {
                    foreach (Transform eachChild in g.transform)
                    {
                        Transform o = GameObject.Instantiate(eachChild, eachChild.position, eachChild.rotation);
                        StartCoroutine(FallingTilesAnimation(o));

                        eachChild.gameObject.SetActive(false);
                    }
                }
            }
            previousTransformsCollidedWith = transformsCollidedWith;
            yield return new WaitForSeconds(0.3F);
        }

    }

    private void FixedUpdate()
    {

        // if (Physics.SphereCast(transform.position, 1.2F, -transform.forward, out hit, 1.2F, layerMask))
        // {
        //     foreach (Transform eachChild in hit.transform)
        //     {
        //         if (eachChild.tag != "TileCollider")
        //         {
        //             eachChild.transform.gameObject.SetActive(false);
        //             Debug.Log(eachChild.transform.gameObject.name);
        //         }
        //     }
        // }
    }
    // private void OnTriggerEnter(Collider other)
    // {
    //     transformsCollidedWith.Clear();
    //     Check for a Tile.
    //     LayerMask mask = LayerMask.GetMask("Tile");
    //     Collider[] colliders = Physics.OverlapSphere(transform.position, 1.2F, mask);
    //     foreach (Collider c in colliders)
    //     {
    //         if (c.transform.tag == "TileCollider")
    //         {
    //             transformsCollidedWith.Add(c.transform);
    //             foreach (Transform eachChild in c.transform)
    //             {
    //                 if (eachChild.tag != "TileCollider")
    //                 {
    //                     eachChild.transform.gameObject.SetActive(true);
    //                     Debug.Log(eachChild.transform.gameObject.name);
    //                 }
    //             }
    //         }
    //     }

    // }
    // private void FixedUpdate()
    // {

    // }
    private void OnTriggerExit(Collider other)
    {
        // if (other.tag == "TileCollider")
        // {
        //     foreach (Transform eachChild in other.transform)
        //     {
        //         if (eachChild.gameObject.activeSelf && eachChild.gameObject.name.Contains("Tile"))
        //         {
        //             Transform o = GameObject.Instantiate(eachChild, eachChild.position, eachChild.rotation);
        //             StartCoroutine(FallingTilesAnimation(o));
        //             GameObject.Destroy(o.gameObject, 1F);
        //         }
        //         eachChild.gameObject.SetActive(false);
        //     }
        // }
    }
}
public enum AnimationChoice { Falling, Fading, Shrinking, None }