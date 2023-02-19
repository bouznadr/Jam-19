using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    GameObject player;
    public bool isClimbing = false;
    public bool canGoUp = false;
    public int ladderHeight = 1; // height in blocks
    [SerializeField] private Vector3 finalPos;

    public Sprite ladderSprite;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        finalPos = new Vector3(transform.position.x, transform.position.y + ladderHeight + .5f, transform.position.z);

        for(int i = 0; i < ladderHeight; i++)
        {
            GameObject newLadder = new GameObject("LadderPart");
            newLadder.AddComponent(typeof(SpriteRenderer));
            newLadder.GetComponent<SpriteRenderer>().sprite = ladderSprite;
            newLadder.transform.parent = this.transform;
            newLadder.transform.localPosition = new Vector3(0f, i, 0f);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if ((bool)Variables.Object(player).Get("canMove"))
            {
                Variables.Object(player).Set("canMove", false);
                player.GetComponent<CapsuleCollider>().enabled = false;
            }

            isClimbing = true;
            StartCoroutine(delayClimb());
        }
    }
    
    void OnTriggerStay(Collider col)
    {
        player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(this.transform.position.x, player.transform.position.y, player.transform.position.z), .1f);
    }

    private void Update()
    {
        if (isClimbing)
        {
            if (canGoUp)
            {
                player.transform.position = Vector3.Lerp(player.transform.position, new Vector3(this.transform.position.x, finalPos.y, player.transform.position.z), .0080f);
                if (player.transform.position.y - finalPos.y < .1f && player.transform.position.y - finalPos.y > -.1f)
                {
                    Debug.Log("Finished climbing");
                    Variables.Object(player).Set("canMove", true);
                    player.GetComponent<CapsuleCollider>().enabled = true;
                    isClimbing = false;
                    canGoUp = false;
                }
            }
        }
    }

    IEnumerator delayClimb()
    {
        yield return new WaitForSeconds(.5f);
        canGoUp = true;
    }

    private void OnDrawGizmos()
    {
        Vector3 endPos = new Vector3(transform.position.x, transform.position.y + ladderHeight - 1, transform.position.z);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, .1f);
        Gizmos.DrawWireSphere(endPos, .1f);
        Gizmos.DrawLine(this.transform.position, endPos);
    }
}
