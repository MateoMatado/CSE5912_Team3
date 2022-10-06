using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDebre : MonoBehaviour
{

    [SerializeField] private Vector2 area = new Vector2();
    // Start is called before the first frame update
    void Awake()
    {
        this.transform.Rotate(new Vector3(0, Random.Range(-180, 180), 0), Space.Self);

        this.transform.position = new Vector3(this.transform.position.x + Random.Range(-area.x, area.x),
            this.transform.position.y, this.transform.position.z + Random.Range(-area.y, area.y));
    }

}
