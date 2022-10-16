using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainScript {

    List<GameObject> ChainList = new List<GameObject>();
    private Vector3 ChainSize = new Vector3((float).25, (float).6, (float).1);
    bool Turn = false;

    // Start is called before the first frame update
    public ChainScript(Vector3 StartPos, Vector3 EndPos, GameObject parent, GameObject Chainlink)
    {
        float Distance = Vector3.Distance(StartPos, EndPos);
        float CurrDist = 0;

        //Start Of link
        GameObject StartChain = GameObject.Instantiate(Chainlink);
        StartChain.transform.position = StartPos;
        StartChain.transform.SetParent(parent.transform);
        StartChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        Turn = true;

        ChainList.Add(StartChain);

        Vector3 prevGameObjectPos = StartPos;

        while (CurrDist < Distance)
        {
            GameObject Link = GameObject.Instantiate(Chainlink);
            Link.transform.position = new Vector3((float)(ChainSize.y - .11) + prevGameObjectPos.x, prevGameObjectPos.y, prevGameObjectPos.z);
            Link.transform.SetParent(parent.transform);
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

        GameObject EndChain = GameObject.Instantiate(Chainlink);
        EndChain.transform.position = new Vector3((float)(ChainSize.y - .11) + prevGameObjectPos.x, prevGameObjectPos.y, prevGameObjectPos.z); ;
        EndChain.transform.SetParent(parent.transform);
        EndChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

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
