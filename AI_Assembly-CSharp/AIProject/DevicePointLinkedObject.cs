// Decompiled with JetBrains decompiler
// Type: AIProject.DevicePointLinkedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIProject.SaveData;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace AIProject
{
  public class DevicePointLinkedObject : MonoBehaviour
  {
    private bool _enable;
    [SerializeField]
    private int _devicePointID;
    [SerializeField]
    private List<DevicePointLinkedObject.OneLight> _lightList;
    [SerializeField]
    private List<GameObject> _enableObjectList;
    [SerializeField]
    private List<GameObject> _disableObjectList;

    public DevicePointLinkedObject()
    {
      base.\u002Ector();
    }

    public bool IsEmpty
    {
      get
      {
        return this._lightList.IsNullOrEmpty<DevicePointLinkedObject.OneLight>() && this._enableObjectList.IsNullOrEmpty<GameObject>() && this._disableObjectList.IsNullOrEmpty<GameObject>();
      }
    }

    private void Awake()
    {
      if (this.IsEmpty)
      {
        Object.Destroy((Object) this);
      }
      else
      {
        this._enable = this.IsEnable();
        this.SetupList();
        this.UpdateObject(this._enable);
        ObservableExtensions.Subscribe<long>(Observable.Where<long>(Observable.TakeUntilDestroy<long>((IObservable<M0>) Observable.EveryUpdate(), (Component) this), (Func<M0, bool>) (_ => ((Behaviour) this).get_isActiveAndEnabled())), (Action<M0>) (_ => this.OnUpdate()));
      }
    }

    private void SetupList()
    {
      for (int index = 0; index < this._lightList.Count; ++index)
      {
        DevicePointLinkedObject.OneLight light = this._lightList[index];
        if (light == null || Object.op_Equality((Object) light.rend, (Object) null))
        {
          this._lightList.RemoveAt(index);
          --index;
        }
        else
        {
          Material material = light.rend.get_material();
          light.Mat = material;
          if (Object.op_Equality((Object) material, (Object) null))
          {
            this._lightList.RemoveAt(index);
            --index;
          }
          else if (!light.Mat.HasProperty(light.emissionParamName))
          {
            this._lightList.RemoveAt(index);
            --index;
          }
          else
            light.Mat.EnableKeyword(light.emissionKeyName);
        }
      }
      this.UpdateColor(this._enable);
    }

    private void OnUpdate()
    {
      bool flag = this.IsEnable();
      if (this._enable == flag)
        return;
      this.UpdateElement(this._enable = flag);
    }

    private bool IsEnable()
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, AgentData> agentTable = Singleton<Game>.Instance.WorldData?.AgentTable;
      AgentData agentData;
      return agentTable != null && (agentTable.TryGetValue(this._devicePointID, out agentData) && agentData != null) && agentData.OpenState;
    }

    private void UpdateElement(bool enable)
    {
      this.UpdateColor(enable);
      this.UpdateObject(enable);
    }

    private void UpdateColor(bool enable)
    {
      foreach (DevicePointLinkedObject.OneLight light in this._lightList)
      {
        if (light != null && !Object.op_Equality((Object) light.Mat, (Object) null) && light.Mat.IsKeywordEnabled(light.emissionKeyName))
          light.Mat.SetColor(light.emissionParamName, !enable ? light.disableColor : light.enableColor);
      }
    }

    private void UpdateObject(bool enable)
    {
      List<GameObject> source1 = !enable ? this._enableObjectList : this._disableObjectList;
      List<GameObject> source2 = !enable ? this._disableObjectList : this._enableObjectList;
      if (!source1.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = source1.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (Object.op_Inequality((Object) current, (Object) null) && current.get_activeSelf())
              current.SetActive(false);
          }
        }
      }
      if (source2.IsNullOrEmpty<GameObject>())
        return;
      using (List<GameObject>.Enumerator enumerator = source2.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null) && !current.get_activeSelf())
            current.SetActive(true);
        }
      }
    }

    [Serializable]
    public class OneLight
    {
      public Color enableColor = Color.get_white();
      public Color disableColor = Color.get_white();
      public string emissionKeyName = string.Empty;
      public string emissionParamName = string.Empty;
      public Renderer rend;

      public Material Mat { get; set; }
    }
  }
}
