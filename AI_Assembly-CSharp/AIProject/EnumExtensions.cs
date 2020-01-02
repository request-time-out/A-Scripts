// Decompiled with JetBrains decompiler
// Type: AIProject.EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.Definitions;

namespace AIProject
{
  public static class EnumExtensions
  {
    public static bool Contains(this TimeZone source, TimeZone zone)
    {
      return source == (source | zone) && zone != (TimeZone) 0;
    }

    public static bool Contains(this Temperature source, Temperature temperature)
    {
      return source == (source | temperature) && temperature != (Temperature) 0;
    }

    public static bool Contains(this Rarelity source, Rarelity shape)
    {
      return source == (source | shape) && shape != Rarelity.None;
    }

    public static bool Contains(this Desire.Type source, Desire.Type desire)
    {
      return source == (source | desire) && desire != Desire.Type.None;
    }

    public static bool Contains(this EventType source, EventType type)
    {
      return source == (source | type) && type != (EventType) 0;
    }

    public static bool Contains(this ObjectLayer source, ObjectLayer layer)
    {
      return source == (source | layer) && layer != (ObjectLayer) 0;
    }

    public static bool Contains(this PoseType source, PoseType poseType)
    {
      return source == (source | poseType) && poseType != (PoseType) 0;
    }
  }
}
