// Decompiled with JetBrains decompiler
// Type: AIProject.TimeOpenLinkedObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AIProject
{
  public class TimeOpenLinkedObject : MonoBehaviour
  {
    private static Dictionary<int, List<TimeOpenLinkedObject>> _table = new Dictionary<int, List<TimeOpenLinkedObject>>();
    public static ReadOnlyDictionary<int, List<TimeOpenLinkedObject>> Table = (ReadOnlyDictionary<int, List<TimeOpenLinkedObject>>) null;
    [SerializeField]
    [DisableInPlayMode]
    private int _timeOpenID;
    [SerializeField]
    [DisableInPlayMode]
    private bool _enableFlag;

    public TimeOpenLinkedObject()
    {
      base.\u002Ector();
    }

    public int TimeOpenID
    {
      get
      {
        return this._timeOpenID;
      }
    }

    public bool EnableFlag
    {
      get
      {
        return this._enableFlag;
      }
    }

    private void Awake()
    {
      if (TimeOpenLinkedObject.Table == null)
        TimeOpenLinkedObject.Table = new ReadOnlyDictionary<int, List<TimeOpenLinkedObject>>((IDictionary<int, List<TimeOpenLinkedObject>>) TimeOpenLinkedObject._table);
      List<TimeOpenLinkedObject> openLinkedObjectList;
      if (!TimeOpenLinkedObject._table.TryGetValue(this._timeOpenID, out openLinkedObjectList) || openLinkedObjectList == null)
        TimeOpenLinkedObject._table[this._timeOpenID] = openLinkedObjectList = new List<TimeOpenLinkedObject>();
      if (openLinkedObjectList.Contains(this))
        return;
      openLinkedObjectList.Add(this);
    }

    private void OnDestroy()
    {
      List<TimeOpenLinkedObject> source;
      if (!TimeOpenLinkedObject._table.TryGetValue(this._timeOpenID, out source) || source.IsNullOrEmpty<TimeOpenLinkedObject>() || !source.Contains(this))
        return;
      source.Remove(this);
    }

    public void SetActive(bool active)
    {
      if (((Component) this).get_gameObject().get_activeSelf() == active)
        return;
      ((Component) this).get_gameObject().SetActive(active);
    }
  }
}
