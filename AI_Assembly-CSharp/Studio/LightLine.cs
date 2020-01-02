// Decompiled with JetBrains decompiler
// Type: Studio.LightLine
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Studio
{
  public static class LightLine
  {
    private static Material m_Material = (Material) null;
    private static Color m_Color = Color.get_white();
    private static float backfaceAlphaMultiplier = 0.2f;
    private static Color lineTransparency = new Color(1f, 1f, 1f, 0.75f);

    public static Shader shader { get; set; }

    public static Material material
    {
      get
      {
        if (Object.op_Equality((Object) LightLine.m_Material, (Object) null))
          LightLine.CreateMaterial();
        return LightLine.m_Material;
      }
    }

    public static Color color
    {
      get
      {
        return LightLine.m_Color;
      }
      set
      {
        LightLine.m_Color = value;
      }
    }

    public static void DrawLine(Light _light)
    {
      LightType type = _light.get_type();
      if (type != 2)
      {
        if (type != null)
          return;
        LightLine.m_Color = _light.get_color();
        LightLine.DrawSpotLight(((Component) _light).get_transform().get_rotation(), ((Component) _light).get_transform().get_position(), _light.get_spotAngle(), _light.get_range(), 1f, 1f);
      }
      else
      {
        LightLine.m_Color = _light.get_color();
        LightLine.DrawPointLight(Quaternion.get_identity(), ((Component) _light).get_transform().get_position(), _light.get_range());
      }
    }

    private static void DrawPointLight(Quaternion _rotation, Vector3 _position, float _radius)
    {
      Vector3[] vector3Array = new Vector3[3]
      {
        Quaternion.op_Multiply(_rotation, Vector3.get_right()),
        Quaternion.op_Multiply(_rotation, Vector3.get_up()),
        Quaternion.op_Multiply(_rotation, Vector3.get_forward())
      };
      if (Camera.get_current().get_orthographic())
      {
        Vector3 forward = ((Component) Camera.get_current()).get_transform().get_forward();
        LightLine.DrawWireDisc(_position, forward, _radius);
        for (int index = 0; index < 3; ++index)
        {
          Vector3 vector3 = Vector3.Cross(vector3Array[index], forward);
          Vector3 normalized = ((Vector3) ref vector3).get_normalized();
          LightLine.DrawTwoShadedWireDisc(_position, vector3Array[index], normalized, 180f, _radius);
        }
      }
      else
      {
        Vector3 _normal = Vector3.op_Subtraction(_position, ((Component) Camera.get_current()).get_transform().get_position());
        float sqrMagnitude = ((Vector3) ref _normal).get_sqrMagnitude();
        float num1 = _radius * _radius;
        float num2 = num1 * num1 / sqrMagnitude;
        float num3 = num2 / num1;
        if ((double) num3 < 1.0)
          LightLine.DrawWireDisc(Vector3.op_Subtraction(_position, Vector3.op_Division(Vector3.op_Multiply(num1, _normal), sqrMagnitude)), _normal, Mathf.Sqrt(num1 - num2));
        for (int index = 0; index < 3; ++index)
        {
          if ((double) num3 < 1.0)
          {
            float num4 = Vector3.Angle(_normal, vector3Array[index]);
            float num5 = Mathf.Tan((90f - Mathf.Min(num4, 180f - num4)) * ((float) Math.PI / 180f));
            float num6 = Mathf.Sqrt(num2 + num5 * num5 * num2) / _radius;
            if ((double) num6 < 1.0)
            {
              float num7 = Mathf.Asin(num6) * 57.29578f;
              Vector3 vector3 = Vector3.Cross(vector3Array[index], _normal);
              Vector3 normalized = ((Vector3) ref vector3).get_normalized();
              Vector3 _from = Quaternion.op_Multiply(Quaternion.AngleAxis(num7, vector3Array[index]), normalized);
              LightLine.DrawTwoShadedWireDisc(_position, vector3Array[index], _from, (float) ((90.0 - (double) num7) * 2.0), _radius);
            }
            else
              LightLine.DrawTwoShadedWireDisc(_position, vector3Array[index], _radius);
          }
          else
            LightLine.DrawTwoShadedWireDisc(_position, vector3Array[index], _radius);
        }
      }
    }

    private static void DrawSpotLight(
      Quaternion _rotation,
      Vector3 _position,
      float _angle,
      float _range,
      float _angleScale,
      float _rangeScale)
    {
      float num = _range * _rangeScale;
      float _radius = num * Mathf.Tan((float) (Math.PI / 180.0 * (double) _angle / 2.0)) * _angleScale;
      Vector3 _normal = Quaternion.op_Multiply(_rotation, Vector3.get_forward());
      Vector3 vector3_1 = Quaternion.op_Multiply(_rotation, Vector3.get_up());
      Vector3 vector3_2 = Quaternion.op_Multiply(_rotation, Vector3.get_right());
      LightLine.DrawLine(_position, Vector3.op_Addition(Vector3.op_Addition(_position, Vector3.op_Multiply(_normal, num)), Vector3.op_Multiply(vector3_1, _radius)));
      LightLine.DrawLine(_position, Vector3.op_Subtraction(Vector3.op_Addition(_position, Vector3.op_Multiply(_normal, num)), Vector3.op_Multiply(vector3_1, _radius)));
      LightLine.DrawLine(_position, Vector3.op_Addition(Vector3.op_Addition(_position, Vector3.op_Multiply(_normal, num)), Vector3.op_Multiply(vector3_2, _radius)));
      LightLine.DrawLine(_position, Vector3.op_Subtraction(Vector3.op_Addition(_position, Vector3.op_Multiply(_normal, num)), Vector3.op_Multiply(vector3_2, _radius)));
      LightLine.DrawWireDisc(Vector3.op_Addition(_position, Vector3.op_Multiply(num, _normal)), _normal, _radius);
    }

    public static void DrawWireDisc(Vector3 _center, Vector3 _normal, float _radius)
    {
      Vector3 _from = Vector3.Cross(_normal, Vector3.get_up());
      if ((double) ((Vector3) ref _from).get_sqrMagnitude() < 1.0 / 1000.0)
        _from = Vector3.Cross(_normal, Vector3.get_right());
      LightLine.DrawWireArc(_center, _normal, _from, 360f, _radius);
    }

    public static void DrawWireArc(
      Vector3 _center,
      Vector3 _normal,
      Vector3 _from,
      float _angle,
      float _radius)
    {
      Vector3[] _dest = new Vector3[60];
      LightLine.SetDiscSectionPoints(_dest, 60, _center, _normal, _from, _angle, _radius);
      LightLine.DrawPolyLine(_dest);
    }

    public static void DrawPolyLine(params Vector3[] _points)
    {
      if (!LightLine.BeginLineDrawing(Matrix4x4.get_identity()))
        return;
      for (int index = 1; index < _points.Length; ++index)
      {
        GL.Vertex(_points[index]);
        GL.Vertex(_points[index - 1]);
      }
      LightLine.EndLineDrawing();
    }

    public static void DrawLine(Vector3 p1, Vector3 p2)
    {
      if (!LightLine.BeginLineDrawing(Matrix4x4.get_identity()))
        return;
      GL.Vertex(p1);
      GL.Vertex(p2);
      LightLine.EndLineDrawing();
    }

    private static void DrawTwoShadedWireDisc(
      Vector3 _position,
      Vector3 _axis,
      Vector3 _from,
      float _degrees,
      float _radius)
    {
      LightLine.DrawWireArc(_position, _axis, _from, _degrees, _radius);
      Color color1 = LightLine.m_Color;
      Color color2 = color1;
      ref Color local = ref color1;
      local.a = (__Null) (local.a * (double) LightLine.backfaceAlphaMultiplier);
      LightLine.m_Color = color1;
      LightLine.DrawWireArc(_position, _axis, _from, _degrees - 360f, _radius);
      LightLine.m_Color = color2;
    }

    private static void DrawTwoShadedWireDisc(Vector3 position, Vector3 axis, float radius)
    {
      Color color1 = LightLine.m_Color;
      Color color2 = color1;
      ref Color local = ref color1;
      local.a = (__Null) (local.a * (double) LightLine.backfaceAlphaMultiplier);
      LightLine.m_Color = color1;
      LightLine.DrawWireDisc(position, axis, radius);
      LightLine.m_Color = color2;
    }

    private static void SetDiscSectionPoints(
      Vector3[] _dest,
      int _count,
      Vector3 _center,
      Vector3 _normal,
      Vector3 _from,
      float _angle,
      float _radius)
    {
      ((Vector3) ref _from).Normalize();
      Quaternion quaternion = Quaternion.AngleAxis(_angle / (float) (_count - 1), _normal);
      Vector3 vector3 = Vector3.op_Multiply(_from, _radius);
      for (int index = 0; index < _count; ++index)
      {
        _dest[index] = Vector3.op_Addition(_center, vector3);
        vector3 = Quaternion.op_Multiply(quaternion, vector3);
      }
    }

    private static bool BeginLineDrawing(Matrix4x4 matrix)
    {
      if (Event.get_current().get_type() != 7)
        return false;
      Color color = Color.op_Multiply(LightLine.m_Color, LightLine.lineTransparency);
      LightLine.material.SetPass(0);
      LightLine.material.SetColor("_Color", color);
      GL.PushMatrix();
      GL.MultMatrix(matrix);
      GL.Begin(1);
      return true;
    }

    private static void EndLineDrawing()
    {
      GL.End();
      GL.PopMatrix();
    }

    private static void CreateMaterial()
    {
      Shader shader = !Object.op_Equality((Object) LightLine.shader, (Object) null) ? LightLine.shader : Shader.Find("Custom/LightLine");
      if (Object.op_Equality((Object) shader, (Object) null))
        Debug.LogError((object) "シェーダーが見つかりません");
      else
        LightLine.m_Material = new Material(shader);
    }
  }
}
