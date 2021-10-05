using UnityEngine;
public static class Vector3Extensions
{
    public static Vector3 ReplaceWith(this Vector3 original, float? x = null, float? y = null, float? z = null) // float? means it could be a float or it could be null
    {
        return new Vector3(x ?? original.x, y ?? original.y, z ?? original.z); // The ?? operator means if the first parameter is null then do the second
    }


}
