using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour

{
    [SerializeField]
    GameObject[] possibleSpawns;

    private GameObject chosenSpawn;
    // Start is called before the first frame update
    void Start()
    {
        chosenSpawn = ChooseSpawn();
        FindObjectOfType<ScoreHandler>().AddToMaxScore(chosenSpawn.GetComponent<Destructible>().destroyValue);
        Instantiate(chosenSpawn, transform.position, new Quaternion(0, 0, 0, 0));
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    GameObject ChooseSpawn()
    {
        return possibleSpawns[Random.Range(0, possibleSpawns.Length - 1)];
    }
}
