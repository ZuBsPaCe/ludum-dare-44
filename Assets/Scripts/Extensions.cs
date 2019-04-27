using UnityEngine;

public static class Extensions
{
    //public static Vector2 xy(this Vector3 v)
    //{
    //    return new Vector2(v.x, v.y);
    //}

    public static Vector3 with_z(this Vector2 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 with_z(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }
}
