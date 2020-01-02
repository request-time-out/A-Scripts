// Decompiled with JetBrains decompiler
// Type: Studio.MapComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using LuxWater;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Studio
{
  public class MapComponent : MonoBehaviour
  {
    [Header("オプション")]
    public MapComponent.OptionInfo[] optionInfos;
    [Header("海面関係")]
    public GameObject objSeaParent;
    public Renderer[] renderersSea;
    private Dictionary<Renderer, MapComponent.SeaInfo> dicSeaInfo;

    public MapComponent()
    {
      base.\u002Ector();
    }

    public bool CheckOption
    {
      get
      {
        return !((IList<MapComponent.OptionInfo>) this.optionInfos).IsNullOrEmpty<MapComponent.OptionInfo>();
      }
    }

    public void SetOptionVisible(bool _value)
    {
      if (((IList<MapComponent.OptionInfo>) this.optionInfos).IsNullOrEmpty<MapComponent.OptionInfo>())
        return;
      foreach (MapComponent.OptionInfo optionInfo in this.optionInfos)
        optionInfo.Visible = _value;
    }

    public void SetOptionVisible(int _idx, bool _value)
    {
      this.optionInfos.SafeProc<MapComponent.OptionInfo>(_idx, (Action<MapComponent.OptionInfo>) (_info => _info.Visible = _value));
    }

    public void SetSeaRenderer()
    {
      if (Object.op_Equality((Object) this.objSeaParent, (Object) null))
        return;
      this.renderersSea = (Renderer[]) this.objSeaParent.GetComponentsInChildren<Renderer>();
    }

    public void SetupSea()
    {
      if (((IList<Renderer>) this.renderersSea).IsNullOrEmpty<Renderer>())
        return;
      ObservableExtensions.Subscribe<Unit>(Observable.Take<Unit>((IObservable<M0>) ObservableTriggerExtensions.LateUpdateAsObservable((Component) this), 1), (Action<M0>) (_ =>
      {
        using (IEnumerator<Renderer> enumerator = ((IEnumerable<Renderer>) this.renderersSea).Where<Renderer>((Func<Renderer, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            Renderer current = enumerator.Current;
            Material material = current.get_material();
            material.DisableKeyword("USINGWATERVOLUME");
            current.set_material(material);
          }
        }
      }));
    }

    [Serializable]
    public class OptionInfo
    {
      public GameObject[] objectsOn;
      public GameObject[] objectsOff;

      public bool Visible
      {
        set
        {
          if (value)
          {
            this.SetVisible(this.objectsOff, false);
            this.SetVisible(this.objectsOn, true);
          }
          else
          {
            this.SetVisible(this.objectsOn, false);
            this.SetVisible(this.objectsOff, true);
          }
        }
      }

      private void SetVisible(GameObject[] _objects, bool _value)
      {
        using (IEnumerator<GameObject> enumerator = ((IEnumerable<GameObject>) _objects).Where<GameObject>((Func<GameObject, bool>) (v => Object.op_Inequality((Object) v, (Object) null))).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
            enumerator.Current.SetActiveIfDifferent(_value);
        }
      }
    }

    private class SeaInfo
    {
      public SeaInfo(Collider _collider, LuxWater_WaterVolume _waterVolume)
      {
        this.Collider = _collider;
        this.WaterVolume = _waterVolume;
      }

      public Collider Collider { get; private set; }

      public LuxWater_WaterVolume WaterVolume { get; private set; }

      public bool Enable
      {
        set
        {
          if (Object.op_Inequality((Object) this.Collider, (Object) null))
            this.Collider.set_enabled(value);
          if (!Object.op_Inequality((Object) this.WaterVolume, (Object) null))
            return;
          ((Behaviour) this.WaterVolume).set_enabled(value);
        }
      }
    }
  }
}
