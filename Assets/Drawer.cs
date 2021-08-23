using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using Shapes;
using Unity.Mathematics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Drawer : ImmediateModeShapeDrawer
{
    private Vector3 center = Vector3.right * 5f + Vector3.up * 1.25f;
    private const float radius = 1.2f;

    private static Vector3? DrawDestination = null;

    private Color[] lightColors;
    private Color[] darkColors;
    private void Start()
    {
        Camera.main.backgroundColor = ColorPalette.GreyLightest;
    }

    public override void DrawShapes( Camera cam )
    {
        if (lightColors is null)
        {
            lightColors = new[] {ColorPalette.GreenLight, ColorPalette.BlueLight, ColorPalette.RedLight, ColorPalette.PurpleLight};
            darkColors = new[] {ColorPalette.GreenDark, ColorPalette.BlueDark, ColorPalette.RedDark, ColorPalette.PurpleDark};
        }
        
        using (Draw.Command(cam))
        {
            Draw.Thickness = 0f;
            Draw.Radius = 0;
            Draw.Disc(center, Quaternion.identity, radius, DiscColors.Flat(lightColors[0]));
            
            Draw.Thickness = radius;
            Draw.Ring(center, Quaternion.identity, radius * 1.5f, DiscColors.Flat(lightColors[1]));
            Draw.Ring(center, Quaternion.identity, radius * 2.5f, DiscColors.Flat(lightColors[2]));
            Draw.Ring(center, Quaternion.identity, radius * 3.5f, DiscColors.Flat(lightColors[3]));
            
            Draw.Thickness = 0.03f;
            float length = radius * 4.2f;
            
            for (int i = 0; i < 12; i++)
            {
                Vector3 offset = Vector3.right * length * Mathf.Cos(i * 30f * Mathf.Deg2Rad) +
                                 Vector3.up * length * Mathf.Sin(i * 30f * Mathf.Deg2Rad);
                Draw.Line(center - offset, center + offset, ColorPalette.GreyDarker);
            }

            if (DrawDestination.HasValue)
            {
                float mag = DrawDestination.Value.magnitude;
                Color color = Color.red;
                int magInt = (int)mag;
                if (magInt < 4 * 10)
                {
                    color = darkColors[magInt];
                }
                
                Draw.Line(center, center + DrawDestination.Value * radius, color);

                float angle = Vector3.Angle(DrawDestination.Value, Vector3.right);
                //Player.Play(magInt, angle);
            }
        }
    }
    
    public static void DrawVector(float2? vector)
    {
        if (!vector.HasValue)
        {
            DrawDestination = null;
            return;
        }

        DrawDestination = new Vector3(vector.Value.x, vector.Value.y, 0f);
    }
}
