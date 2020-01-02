// Decompiled with JetBrains decompiler
// Type: LightMapDataObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using OutputLogControl;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "LightMapData", menuName = "LightMapData", order = 1)]
public class LightMapDataObject : ScriptableObject
{
  public LightProbes lightProbes;
  public LightmapsMode lightmapsMode;
  public Cubemap cubemap;
  public Texture2D[] light;
  public Texture2D[] dir;
  public Scene.FogData fog;

  public LightMapDataObject()
  {
    base.\u002Ector();
  }

  public static LightMapDataObject operator +(
    LightMapDataObject a,
    LightMapDataObject b)
  {
    List<Texture2D> list1 = ((IEnumerable<Texture2D>) a.light).ToList<Texture2D>();
    List<Texture2D> list2 = ((IEnumerable<Texture2D>) a.dir).ToList<Texture2D>();
    list1.AddRange((IEnumerable<Texture2D>) b.light);
    list2.AddRange((IEnumerable<Texture2D>) b.dir);
    return new LightMapDataObject()
    {
      lightProbes = a.lightProbes,
      cubemap = a.cubemap,
      lightmapsMode = a.lightmapsMode,
      light = list1.ToArray(),
      dir = list2.ToArray()
    };
  }

  public void Change()
  {
    LightmapData[] lightmapDataArray = new LightmapData[this.light.Length];
    for (int index = 0; index < lightmapDataArray.Length; ++index)
    {
      LightmapData lightmapData = new LightmapData();
      lightmapData.set_lightmapDir(this.dir[index]);
      lightmapData.set_lightmapColor(this.light[index]);
      lightmapDataArray[index] = lightmapData;
    }
    LightmapSettings.set_lightmaps(lightmapDataArray);
    LightmapSettings.set_lightProbes(this.lightProbes);
    LightmapSettings.set_lightmapsMode(this.lightmapsMode);
    if (Object.op_Inequality((Object) this.cubemap, (Object) null))
    {
      RenderSettings.set_customReflection(this.cubemap);
      RenderSettings.set_defaultReflectionMode((DefaultReflectionMode) 1);
    }
    else
      RenderSettings.set_defaultReflectionMode((DefaultReflectionMode) 0);
    if (this.fog != null)
      this.fog.Change();
    OutputLog.Log("LightMapChange", true, "Log");
  }
}
