using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImprovedSearching
{
    public class ImprovedSearching
    {
        public GameObject[] FindAllChildrenOf(GameObject target)
        {
            GameObject[] allChildren = target.GetComponentsInChildren<GameObject>();
            List<GameObject> resultList = new List<GameObject>();

            foreach (var child in allChildren)
            {
                if (child == target) { continue; }
                resultList.Add(child);
            }

            return resultList.ToArray();
        }
        //public void DestroyAllChildrenOf(Transform target)
        //{

        //}
        //public void DisableAllChildrenOf(Transform target)
        //{

        //}


        //public GameObject FindParentWithTag(Transform target, string tag) { }
        //public GameObject[] FindParentsWithTag(Transform target, string tag) { }
        //public GameObject FindChildWithTag(Transform target, string tag) { }
        //public GameObject[] FindChildrenWithTag(Transform target, string tag) { }

    }

}
