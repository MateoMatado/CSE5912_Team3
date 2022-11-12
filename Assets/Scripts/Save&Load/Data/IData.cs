using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    void NewData(ref GameData data);

    void LoadData(GameData data);

    void SaveData(ref GameData data);
}
