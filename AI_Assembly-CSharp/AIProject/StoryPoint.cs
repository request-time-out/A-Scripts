// Decompiled with JetBrains decompiler
// Type: AIProject.StoryPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject
{
  public class StoryPoint : Point
  {
    [SerializeField]
    private int _pointID;

    public int PointID
    {
      get
      {
        return this._pointID;
      }
    }

    public Vector3 Position
    {
      get
      {
        return ((Component) this).get_transform().get_position();
      }
    }

    public Vector3 EulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_eulerAngles();
      }
    }

    public Quaternion Rotation
    {
      get
      {
        return ((Component) this).get_transform().get_rotation();
      }
    }

    public Vector3 LocalPosition
    {
      get
      {
        return ((Component) this).get_transform().get_localPosition();
      }
    }

    public Vector3 LocalEulerAngles
    {
      get
      {
        return ((Component) this).get_transform().get_localEulerAngles();
      }
    }

    public Quaternion LocalRotation
    {
      get
      {
        return ((Component) this).get_transform().get_localRotation();
      }
    }

    public Vector3 Forward
    {
      get
      {
        return ((Component) this).get_transform().get_forward();
      }
    }

    public Vector3 Right
    {
      get
      {
        return ((Component) this).get_transform().get_right();
      }
    }

    public Vector3 Back
    {
      get
      {
        return Vector3.op_Multiply(((Component) this).get_transform().get_forward(), -1f);
      }
    }

    public Vector3 Left
    {
      get
      {
        return Vector3.op_Multiply(((Component) this).get_transform().get_right(), -1f);
      }
    }
  }
}
