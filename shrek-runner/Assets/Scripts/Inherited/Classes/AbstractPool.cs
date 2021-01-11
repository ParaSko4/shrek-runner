using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Inherited.Classes
{
    public class AbstractPool : MonoBehaviour
    {
        [SerializeField]
        protected GameObject poolObject;

        [SerializeField]
        protected int MaxGameObjects = 100;
        [SerializeField]
        protected float LifeTime = 10;

        protected Queue<GameObject> freeObjects;
        protected List<GameObject> busyObjects;

        public AbstractPool()
        {
            freeObjects = new Queue<GameObject>();
            busyObjects = new List<GameObject>();
        }

        protected virtual void SetObjectWaitSettings(GameObject obj)
        {
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;
        }

        protected virtual GameObject CreateObject()
        {
            GameObject newPoolObject = Instantiate(poolObject);
            SetObjectWaitSettings(newPoolObject);

            return newPoolObject;
        }

        public virtual GameObject GetObject()
        {
            GameObject takenObject;

            if (freeObjects.Count == 0)
            {
                MaxGameObjects++;
                takenObject = CreateObject();
            }
            else
            {
                takenObject = freeObjects.Dequeue();
            }

            takenObject.SetActive(true);
            busyObjects.Add(takenObject);
            StartCoroutine(ObjectLifeTime(takenObject));

            return takenObject;
        }

        public virtual void ReturnObject(GameObject obj)
        {
            if (busyObjects.Contains(obj))
            {
                busyObjects.Remove(obj);
                freeObjects.Enqueue(obj);
            }
        }

        protected virtual IEnumerator ObjectLifeTime(GameObject obj)
        {
            yield return new WaitForSeconds(LifeTime);

            SetObjectWaitSettings(obj);
            ReturnObject(obj);
        }
    }
}