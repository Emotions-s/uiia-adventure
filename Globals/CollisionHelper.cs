using Microsoft.Xna.Framework;
using System;

namespace uiia_adventure.Globals;

public static class CollisionHelper
{
    public static bool RayVsRect(Vector2 rayOrigin, Vector2 rayDir, Rectangle target,
                                  out Vector2 contactPoint, out Vector2 contactNormal, out float tHitNear)
    {
        contactPoint = Vector2.Zero;
        contactNormal = Vector2.Zero;
        tHitNear = 0f;

        Vector2 invDir = new Vector2(
            rayDir.X != 0 ? 1.0f / rayDir.X : float.MaxValue,
            rayDir.Y != 0 ? 1.0f / rayDir.Y : float.MaxValue
        );

        // Calculate intersections with rectangle bounding axes
        Vector2 tNear = (new Vector2(target.Left, target.Top) - rayOrigin) * invDir;
        Vector2 tFar = (new Vector2(target.Right, target.Bottom) - rayOrigin) * invDir;

        if (float.IsNaN(tFar.X) || float.IsNaN(tFar.Y) || float.IsNaN(tNear.X) || float.IsNaN(tNear.Y))
            return false;

        // Sort distances
        if (tNear.X > tFar.X) Swap(ref tNear.X, ref tFar.X);
        if (tNear.Y > tFar.Y) Swap(ref tNear.Y, ref tFar.Y);

        // Early rejection
        if (tNear.X > tFar.Y || tNear.Y > tFar.X)
            return false;

        // Closest 'time' will be the first contact
        tHitNear = Math.Max(tNear.X, tNear.Y);

        // Furthest 'time' is contact on opposite side of target
        float tHitFar = Math.Min(tFar.X, tFar.Y);

        // Reject if ray direction is pointing away from object
        if (tHitFar < 0) return false;

        // Contact point of collision from parametric line equation
        contactPoint = rayOrigin + tHitNear * rayDir;

        if (tNear.X > tNear.Y)
            if (invDir.X < 0)
                contactNormal = new Vector2(1, 0);
            else
                contactNormal = new Vector2(-1, 0);
        else if (tNear.X < tNear.Y)
            if (invDir.Y < 0)
                contactNormal = new Vector2(0, 1);
            else
                contactNormal = new Vector2(0, -1);

        return true;
    }

    public static bool DynamicRectVsRect(Rectangle sourceRect, Vector2 velocity, float dt, Rectangle targetRect,
                                         out Vector2 contactPoint, out Vector2 contactNormal, out float contactTime)
    {
        contactPoint = Vector2.Zero;
        contactNormal = Vector2.Zero;
        contactTime = 0f;

        if (velocity == Vector2.Zero) return false;

        Rectangle expandedTarget = new Rectangle(
            targetRect.X - sourceRect.Width / 2,
            targetRect.Y - sourceRect.Height / 2,
            targetRect.Width + sourceRect.Width,
            targetRect.Height + sourceRect.Height
        );

        Vector2 rayOrigin = new(sourceRect.Center.X, sourceRect.Center.Y);
        Vector2 rayDir = velocity * dt;

        return RayVsRect(rayOrigin, rayDir, expandedTarget, out contactPoint, out contactNormal, out contactTime)
            && contactTime >= 0 && contactTime < 1.0f;
    }

    private static void Swap(ref float a, ref float b)
    {
        float temp = a;
        a = b;
        b = temp;
    }
}
