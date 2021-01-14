using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace Project.Utility
{
    public static class Vector3Extension
    {
        public static List<Vector2> toVector2List(this Vector3[] v3)
        {
            return System.Array.ConvertAll(v3, getV3fromV2).ToList();
        }
        public static Vector2 getV3fromV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.y);
        }
    }
}
