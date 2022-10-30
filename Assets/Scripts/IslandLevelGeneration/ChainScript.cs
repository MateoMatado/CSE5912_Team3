using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class ChainScript
{

    List<GameObject> ChainList = new List<GameObject>();
    private Vector3 ChainSize = new Vector3((float).25 * 150, (float).6 * 150, (float).1 * 150);
    bool Turn = false;

    // Start is called before the first frame update
    public ChainScript(Vector3 StartPos, Vector3 EndPos, GameObject parent, GameObject Chainlink, GameObject FloatingRock)
    {
        float Distance = Vector3.Distance(StartPos, EndPos);

        Vector3 PosDiff = new Vector3(0 - StartPos.x, 0 - StartPos.y, 0 - StartPos.z);
        StartPos = new Vector3(0, 0, 0);
        EndPos = new Vector3(EndPos.x + PosDiff.x, EndPos.y + PosDiff.y, EndPos.z + PosDiff.z);

        float CurrDist = 0;


        //Start Of link
        GameObject StartChain = GameObject.Instantiate(Chainlink);
        StartChain.transform.position = StartPos;
        StartChain.transform.SetParent(parent.transform);
        StartChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        GameObject FloatingRockStart = GameObject.Instantiate(FloatingRock);
        FloatingRockStart.transform.position = StartPos;
        FloatingRockStart.transform.SetParent(parent.transform);
        FloatingRockStart.transform.localScale = new Vector3(25, 25, 40);
        FloatingRockStart.transform.Rotate(new Vector3(90, 0, 0), Space.Self);

        Turn = true;

        ChainList.Add(StartChain);

        Vector3 prevGameObjectPos = StartPos;
        if(StartPos.x < EndPos.x || StartPos.y < EndPos.y || StartPos.z < EndPos.z) { Distance += ChainSize.y * 3; }

        while (CurrDist < Distance + ChainSize.y)
        {
            GameObject Link = GameObject.Instantiate(Chainlink);
            Link.transform.position = new Vector3(prevGameObjectPos.x, prevGameObjectPos.y, (float)(ChainSize.y - ChainSize.z) + prevGameObjectPos.z);
            StartChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            Link.transform.SetParent(parent.transform);
            prevGameObjectPos = Link.transform.position;

            if (Turn)
            {
                Link.transform.Rotate(new Vector3(0, 90, 0), Space.Self);
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
        EndChain.transform.position = new Vector3(prevGameObjectPos.x, prevGameObjectPos.y, (float)(ChainSize.y - ChainSize.z) + prevGameObjectPos.z);
        EndChain.transform.SetParent(parent.transform);
        EndChain.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;


        GameObject FloatingRockEnd = GameObject.Instantiate(FloatingRock);
        FloatingRockEnd.transform.position = new Vector3(prevGameObjectPos.x, prevGameObjectPos.y, (float)(ChainSize.y*1.3) + prevGameObjectPos.z);
        FloatingRockEnd.transform.SetParent(parent.transform);
        FloatingRockEnd.transform.localScale = new Vector3(25, 25, 40);
        FloatingRockEnd.transform.Rotate(new Vector3(90,0,0), Space.Self);


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
        Vector3 look = new Vector3(EndPos.x, EndPos.y, EndPos.z);
        parent.transform.LookAt(look);
        parent.transform.position = -PosDiff;
    }
}
