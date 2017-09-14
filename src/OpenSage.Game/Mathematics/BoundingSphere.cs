﻿using System;
using System.Numerics;

namespace OpenSage.Mathematics
{
    public struct BoundingSphere
    {
        public Vector3 Center;

        public float Radius;

        public BoundingSphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public static BoundingSphere CreateMerged(BoundingSphere original, BoundingSphere additional)
        {
            Vector3 ocenterToaCenter = Vector3.Subtract(additional.Center, original.Center);
            float distance = ocenterToaCenter.Length();
            if (distance <= original.Radius + additional.Radius)
            {
                if (distance <= original.Radius - additional.Radius)
                {
                    return original;
                }
                if (distance <= additional.Radius - original.Radius)
                {
                    return additional;
                }
            }
            float leftRadius = Math.Max(original.Radius - distance, additional.Radius);
            float Rightradius = Math.Max(original.Radius + distance, additional.Radius);
            ocenterToaCenter = ocenterToaCenter + (((leftRadius - Rightradius) / (2 * ocenterToaCenter.Length())) * ocenterToaCenter);

            return new BoundingSphere
            {
                Center = original.Center + ocenterToaCenter,
                Radius = (leftRadius + Rightradius) / 2
            };
        }

        public BoundingSphere Transform(Matrix4x4 matrix)
        {
            return new BoundingSphere
            {
                Center = Vector3.Transform(Center, matrix),
                Radius = Radius * (MathUtility.Sqrt(
                    Math.Max(
                        ((matrix.M11 * matrix.M11) + (matrix.M12 * matrix.M12)) + (matrix.M13 * matrix.M13), 
                        Math.Max(
                            ((matrix.M21 * matrix.M21) + (matrix.M22 * matrix.M22)) + (matrix.M23 * matrix.M23), 
                            ((matrix.M31 * matrix.M31) + (matrix.M32 * matrix.M32)) + (matrix.M33 * matrix.M33)))))
            };
        }
    }

    public static class MathUtility
    {
        public static readonly float Pi = (float) Math.PI;

        public static float Sqrt(float v) => (float) Math.Sqrt(v);
    }
}