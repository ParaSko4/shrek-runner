using Assets.Scripts.Inherited.Classes;
using System.Collections.Generic;
using UnityEngine;

public class DestructionWallPSPool : AbstractPool
{
    private Dictionary<GameObject, ParticleSystem> objectPS;

    private void Awake()
    {
        objectPS = new Dictionary<GameObject, ParticleSystem>();

        for (int i = 0; i < MaxGameObjects; i++)
        {
            var newObj = CreateObject();
            freeObjects.Enqueue(newObj);
            objectPS.Add(newObj, newObj.GetComponent<ParticleSystem>());
        }
    }

    public ParticleSystem GetPS(GameObject keyObj)
    {
        return objectPS[keyObj];
    }
}