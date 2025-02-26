using UnityEngine;

namespace Kunnymann.Navigation
{
    /// <summary>
    /// Vector3 utility
    /// </summary>
    public class Vector3Utility
    {
        /// <summary>
        /// Vector3 YZ projection
        /// </summary>
        /// <param name="vector">Vector3</param>
        /// <returns>YZ Vector3</returns>
        public static Vector3 YZProject(Vector3 vector)
        {
            return new Vector3(0, vector.y, vector.z);
        }

        /// <summary>
        /// Vector3 XZ projection
        /// </summary>
        /// <param name="vector">Vector3</param>
        /// <returns>XZ Vector3</returns>
        public static Vector3 XZProject(Vector3 vector)
        {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// Vector3 XY projection
        /// </summary>
        /// <param name="vector">Vector3</param>
        /// <returns>XY Vector3</returns>
        public static Vector3 XYProject(Vector3 vector)
        {
            return new Vector3(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Return distance between two vectors
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Distance</returns>
        public static float GetYZDistance(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Distance(YZProject(vector1), YZProject(vector2));
        }

        /// <summary>
        /// Return distance between two XZ projected vectors
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Distance</returns>
        public static float GetXZDistance(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Distance(XZProject(vector1), XZProject(vector2));
        }

        /// <summary>
        /// Return distance between two XY projected vectors
        /// </summary>
        /// <param name="vector1">Vector 1</param>
        /// <param name="vector2">Vector 2</param>
        /// <returns>Distance</returns>
        public static float GetXYDistance(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Distance(XYProject(vector1), XYProject(vector2));
        }
    }
}