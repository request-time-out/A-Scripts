// Decompiled with JetBrains decompiler
// Type: GameInterface
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class GameInterface
{
  public static GameInterface Instance = new GameInterface();
  public static string EnvironmentRootName = "Environment";
  public static Dictionary<string, string> ObjectNames = new Dictionary<string, string>()
  {
    {
      "Scene_Objects",
      "Environment/Models"
    },
    {
      "Scene_Objects_(Lv1)",
      "Environment/Models/level_1"
    },
    {
      "Scene_Objects_(Lv2)",
      "Environment/Models/level_2"
    },
    {
      "Scene_Objects_(Lv3)",
      "Environment/Models/level_3"
    },
    {
      "Scene_Effects",
      "Environment/SceneEffect"
    },
    {
      "Scene_Terrain",
      "Environment/Terrain"
    },
    {
      "UI",
      "Main/UIMgr"
    },
    {
      "Players",
      "Main/_Players"
    },
    {
      "Effects",
      "Main/AnimationEffects"
    }
  };
  public static Dictionary<string, double> VisiblePercentages = new Dictionary<string, double>()
  {
    {
      "Objects",
      100.0
    },
    {
      "Effects",
      100.0
    }
  };
  public Dictionary<string, GameObject> KeyNodes = new Dictionary<string, GameObject>();
  public List<Renderer> DisabledRenderers = new List<Renderer>();
  public List<ParticleSystemRenderer> DisabledParticleSystems = new List<ParticleSystemRenderer>();

  public bool Init()
  {
    foreach (KeyValuePair<string, string> objectName in GameInterface.ObjectNames)
    {
      GameObject gameObject = GameObject.Find(objectName.Value);
      if (Object.op_Inequality((Object) gameObject, (Object) null))
      {
        Log.Info((object) "KeyNode {0} added as {1}", (object) objectName.Value, (object) objectName.Key);
        this.KeyNodes[objectName.Key] = gameObject;
      }
    }
    return true;
  }

  public void ToggleSwitch(string name, bool on)
  {
    if (!this.KeyNodes.ContainsKey(name))
      return;
    Log.Info((object) "{0} toggled as {1}", (object) name, (object) on);
    this.KeyNodes[name].SetActive(on);
  }

  public void ChangePercentage(string name, double val)
  {
    if (!GameInterface.VisiblePercentages.ContainsKey(name))
      return;
    GameInterface.VisiblePercentages[name] = val;
    Log.Info((object) "{0} slided to {1:0.00}", (object) name, (object) val);
    this.FilterVisibleObjects((float) GameInterface.VisiblePercentages["Objects"], (float) GameInterface.VisiblePercentages["Effects"]);
  }

  private void DoFilter<T>(List<T> visible, List<T> disabled, float percentage) where T : Renderer
  {
    int num = (int) ((double) (visible.Count + disabled.Count) * (double) percentage);
    if (visible.Count > num)
    {
      for (int index = 0; index < visible.Count - num; ++index)
      {
        T obj = visible[index];
        obj.set_enabled(false);
        disabled.Add(obj);
        GameUtil.Log("{0} is hidden.", (object) ((Object) obj.get_gameObject()).get_name());
      }
    }
    else
    {
      if (num <= visible.Count)
        return;
      for (int index = 0; index < num - visible.Count; ++index)
      {
        T obj = disabled[index];
        obj.set_enabled(true);
        disabled.Remove(obj);
        GameUtil.Log("{0} is shown.", (object) ((Object) obj.get_gameObject()).get_name());
      }
    }
  }

  private bool IsEnvironmentObject(GameObject go)
  {
    return ((Object) ((Component) go.get_transform().get_root()).get_gameObject()).get_name() == GameInterface.EnvironmentRootName;
  }

  public void FilterVisibleObjects(float percentage, float psysPercent)
  {
    List<Renderer> visible1 = new List<Renderer>();
    List<ParticleSystemRenderer> visible2 = new List<ParticleSystemRenderer>();
    foreach (Renderer renderer in Object.FindObjectsOfType(typeof (Renderer)) as Renderer[])
    {
      if (renderer.get_isVisible() && renderer.get_enabled() && this.IsEnvironmentObject(((Component) renderer).get_gameObject()))
      {
        if (renderer is ParticleSystemRenderer)
          visible2.Add((ParticleSystemRenderer) renderer);
        else
          visible1.Add(renderer);
      }
    }
    this.DoFilter<Renderer>(visible1, this.DisabledRenderers, GameUtil.Clamp(percentage, 0.0f, 1f));
    this.DoFilter<ParticleSystemRenderer>(visible2, this.DisabledParticleSystems, GameUtil.Clamp(psysPercent, 0.0f, 1f));
  }
}
