using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainScriptTest : MonoBehaviour
{
    [SerializeField] private Vector3 StartPos;
    [SerializeField] private Vector3 EndPos;
    [SerializeField] private GameObject Chain;
    [SerializeField] private GameObject ChainLink;
    [SerializeField] private Vector3 ChainSize;

    List<GameObject> ChainList = new List<GameObject>();

    bool Turn = false;

    // Start is called before the first frame update
    void Start()
    {
        float Distance = Vector3.Distance(StartPos, EndPos);
        float CurrDist = 0;

        //Start Of link
        GameObject StartChain = GameObject.Instantiate(Chain);
        StartChain.transform.position = StartPos;
        StartChain.transform.SetParent(this.transform);
        StartChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        Turn = true;

        ChainList.Add(StartChain);

        Vector3 prevGameObjectPos = StartPos;

        while (CurrDist <= Distance)
        {
            GameObject Link = GameObject.Instantiate(ChainLink);
            Link.transform.position = new Vector3((float)(ChainSize.y - .11) + prevGameObjectPos.x, prevGameObjectPos.y, prevGameObjectPos.z);
            StartChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX;
            Link.transform.SetParent(gameObject.transform);
            prevGameObjectPos = Link.transform.position;

            if (Turn)
            {
                Link.transform.Rotate(new Vector3(0,90,0),Space.Self);
                Turn = false;
            }
            else
            {
                Turn = true;
            }

            ChainList.Add(Link);
            CurrDist += ChainSize.y;
        }

        GameObject EndChain = GameObject.Instantiate(Chain);
        EndChain.transform.position = new Vector3((float)(ChainSize.y - .11) + prevGameObjectPos.x, prevGameObjectPos.y, prevGameObjectPos.z); ;
        EndChain.transform.SetParent(this.transform);
        EndChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

        if (Turn)
        {
            EndChain.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
            Turn = false;
        }
        else
        {
            Turn = true;
        }

        ChainList.Add(EndChain);

    }

}
