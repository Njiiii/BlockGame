using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour {


    public GameObject[] blocks;


	void Start ()
    {
        SpawnNextBlock();
	}
	

    public void SpawnNextBlock()
    {
        int i = Random.Range(0, blocks.Length);

        Instantiate(blocks[i], transform.position, Quaternion.identity);
    }
}
