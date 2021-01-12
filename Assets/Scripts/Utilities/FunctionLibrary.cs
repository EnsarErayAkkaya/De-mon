using UnityEngine;

namespace Project.Utility
{
    public class FunctionLibrary
    {
        /// <summary>
        /// returns world position of any screen point at Z axis.
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
            float distance;
            xy.Raycast(ray, out distance);
            return ray.GetPoint(distance);
        }
    }
}