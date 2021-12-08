using UnityEngine;

public enum TurnDirection
{
    Left,
    Right,
    Forward
}

public static class TurnMethods
{
    public static Vector3 TurnVector(this Vector3 vector, TurnDirection turn)
    {
        if (turn == TurnDirection.Forward) return vector;

        Vector3 resultVector =
            vector.GetPerpendicular();

        bool isReversed = ((turn == TurnDirection.Right) && (vector.x != 0));
        isReversed |= ((turn == TurnDirection.Left) && (vector.z != 0));

        resultVector *= (isReversed) ? -1 : 1;

        return resultVector;
    }

    public static int Angle(this TurnDirection turn)
    {
        switch (turn)
        {
            case TurnDirection.Left:
                return -90;
            case TurnDirection.Right:
                return 90;
            default:
                return 0;
        }
    }

    public static Vector3 GetPerpendicular(this Vector3 vector)
    {
        return new Vector3(vector.z, vector.y, vector.x);
    }

    public static Vector3 Direction(this float angle)
    {
        if ((angle >= 0 && angle < 45) ||
            (angle >= 315 && angle < 360))
        {
            return Vector3.right;
        }

        if (angle >= 45 && angle < 135)
        {
            return Vector3.back;
        }

        if (angle >= 135 && angle < 225)
        {
            return Vector3.left;
        }

        if (angle >= 225 && angle < 315)
        {
            return Vector3.forward;
        }


        return Vector3.zero;
    }

    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector3 ToSign(this Vector3 vector)
    {
        Vector3 signVector = new Vector3();

        signVector.x = (vector.x == 0) ? 0 : Mathf.Sign(vector.x);
        signVector.y = (vector.y == 0) ? 0 : Mathf.Sign(vector.y);
        signVector.z = (vector.z == 0) ? 0 : Mathf.Sign(vector.z);

        return signVector;
    }

    public static Vector3 Scale(this Vector3 vector, float value)
    {
        return new Vector3(vector.x * value, vector.y * value, vector.z * value);
    }
}