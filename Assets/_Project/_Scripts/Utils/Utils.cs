using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public enum Direction
{
    front,
    behind,
    left,
    right
}


public static class Utils
{
    static Texture2D _whiteTexture;
    public static Texture2D WhiteTexture
    {
        get
        {
            if (_whiteTexture == null)
            {
                _whiteTexture = new Texture2D(1, 1);
                _whiteTexture.SetPixel(0, 0, Color.white);
                _whiteTexture.Apply();
            }
            return _whiteTexture;
        }
    }

    public static void DrawScreenRect(Rect rect, Color color)
    {
        GUI.color = color;
        GUI.DrawTexture(rect, WhiteTexture);
        GUI.color = Color.white;
    }

    public static void DrawScreenRectBorder(Rect rect, float thickness, Color color)
    {
        // Top
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, rect.width, thickness), color);
        // Left
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMin, thickness, rect.height), color);
        // Right
        Utils.DrawScreenRect(new Rect(rect.xMax - thickness, rect.yMin, thickness, rect.height), color);
        // Bottom
        Utils.DrawScreenRect(new Rect(rect.xMin, rect.yMax - thickness, rect.width, thickness), color);
    }

    public static Rect GetScreenRect(Vector3 screenPosition1, Vector3 screenPosition2)
    {
        // Move origin from bottom left to top left
        screenPosition1.y = Screen.height - screenPosition1.y;
        screenPosition2.y = Screen.height - screenPosition2.y;

        // Calculate corners
        var topLeft = Vector3.Min(screenPosition1, screenPosition2);
        var bottomRight = Vector3.Max(screenPosition1, screenPosition2);

        // Create rect
        return Rect.MinMaxRect(topLeft.x, topLeft.y, bottomRight.x, bottomRight.y);
    }

    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2)
    {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    public static string ColorToRichText(string str, Color color)
    {
        return "<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + str + "</color>";
    }

    public static string LinkToRichText(string str, string linkID)
    {
        return "<link=\"" + linkID + "\">" + str + "</link>";
    }

    public static string MonoFont(string str)
    {
        return "<font=\"Noto SDF\">" + str + "</font>";
    }

    public static string PlusSign(int _value)
    {
        if (_value < 0)
            return _value.ToString();
        else
            return "+" + _value.ToString();
    }

     
    public static bool IsWithinRange(Vector3 a, Vector3 b, float range)
    {
        return (range * range) > GetRelativeDistance(a, b);
    }
    public static float GetRelativeDistance(Vector3 a, Vector3 b)
    {
        float dist = (b - a).sqrMagnitude;
        return dist;
    }

    public static bool InLineOfSight(Vector3 start, Vector3 end, LayerMask blockingLayer)
    {
        return !Physics.Linecast(start + (Vector3.up), end + (Vector3.up), blockingLayer);
    }

    public static bool InLineOfSight(Vector3 start, Vector3 end, LayerMask blockingLayer, float verticalOffset = 1)
    {
        return !Physics.Linecast(start + (Vector3.up * verticalOffset), end + (Vector3.up * verticalOffset), blockingLayer);
    }
    public static Vector3 MousePosition()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return pos;
    }

    public static Vector3 RandomDirection3()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public static Vector3 RandomDirectionXZ()
    {
        return new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    }

    public static Direction TargetDirection(Transform self, Vector3 targetPosition)
    {
        Vector3 dirToTarget = targetPosition - self.position;
        float signedAngle = Vector3.SignedAngle(dirToTarget, self.forward, self.up);

        if(signedAngle >= -45f && signedAngle <= 45f)
        {
            return Direction.front;
        }
        else if (signedAngle > 45f && signedAngle <= 135f)
        {
            return Direction.left;
        }
        else if (signedAngle < -45f && signedAngle >= -135f)
        {
            return Direction.right;
        }
        else
        {
            return Direction.behind;
        }
    }

    public static bool TargetWithinDegrees(Transform self, Vector3 targetPosition, float degrees)
    {
        Vector3 selfPos = self.position;
        selfPos.y = 0;
        targetPosition.y = 0;
        Vector3 dirToTarget = targetPosition - self.position;
        float signedAngle = Vector3.SignedAngle(dirToTarget, self.forward, self.up);

        //if (signedAngle <= Mathf.Abs(degrees))
        if (signedAngle >= -degrees && signedAngle <= degrees)
        {
            return true;
        }

        return false;
    }

    public static Quaternion LookAtFlat(Vector3 self, Vector3 target)
    {
        target.y = 0;
        self.y = 0;

        return Quaternion.LookRotation(target - self);
    }

    public static Vector3 CameraRelativeDirection(Vector3 dir, Transform cameraTransform)
    {
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0f;
        camRight.y = 0f;

        return (camRight * dir.x + camForward * dir.z);
    }

    public static Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t, Vector3 up)
    {
        float parabolicT = t * 2 - 1;
        //start and end are not level, gets more complicated
        Vector3 travelDirection = end - start;
        Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
        //Vector3 right = Vector3.Cross(travelDirection, levelDirection);
        // Vector3 up = outDirection;
        Vector3 result = start + t * travelDirection;
        result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
        return result;
    }

    public static bool MouseScreenCheck()
    {
#if UNITY_EDITOR
        //if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Handles.GetMainGameViewSize().x - 1 || Input.mousePosition.y >= Handles.GetMainGameViewSize().y - 1)
        //{
        //    return false;
        //}
        if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1)
        {
            return false;
        }
#else
            if (Input.mousePosition.x == 0 || Input.mousePosition.y == 0 || Input.mousePosition.x >= Screen.width - 1 || Input.mousePosition.y >= Screen.height - 1) {
            return false;
            }
#endif
        else
        {
            return true;
        }

    }
}