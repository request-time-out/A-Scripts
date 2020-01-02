// Decompiled with JetBrains decompiler
// Type: AIProject.OnceSearchActionPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace AIProject
{
  public class OnceSearchActionPoint : SearchActionPoint
  {
    [SerializeField]
    private int _mapItemID = -1;
    private List<GameObject> _mapItems;

    public int MapItemID
    {
      get
      {
        return this._mapItemID;
      }
    }

    public bool HaveMapItems
    {
      get
      {
        return !this._mapItems.IsNullOrEmpty<GameObject>();
      }
    }

    private void Awake()
    {
      this._mapItems = MapItemData.Get(this._mapItemID);
    }

    protected override void Start()
    {
      base.Start();
      if (this.IsAvailable())
        return;
      ((Component) this).get_gameObject().SetActive(false);
    }

    protected override void OnEnable()
    {
      base.OnEnable();
      if (this._mapItems.IsNullOrEmpty<GameObject>())
        return;
      using (List<GameObject>.Enumerator enumerator = this._mapItems.GetEnumerator())
      {
        while (enumerator.MoveNext())
        {
          GameObject current = enumerator.Current;
          if (Object.op_Inequality((Object) current, (Object) null) && !current.get_activeSelf())
            current.SetActive(true);
        }
      }
    }

    protected override void OnDisable()
    {
      if (!this._mapItems.IsNullOrEmpty<GameObject>())
      {
        using (List<GameObject>.Enumerator enumerator = this._mapItems.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            GameObject current = enumerator.Current;
            if (Object.op_Inequality((Object) current, (Object) null) && current.get_activeSelf())
              current.SetActive(false);
          }
        }
      }
      base.OnDisable();
    }

    public override bool Entered(
      Vector3 basePosition,
      float distance,
      float radiusA,
      float radiusB,
      float angle,
      Vector3 forward)
    {
      return this.IsAvailable() && base.Entered(basePosition, distance, radiusA, radiusB, angle, forward);
    }

    public void SetAvailable(bool active)
    {
      if (!Singleton<Game>.IsInstance())
        return;
      Dictionary<int, bool> actionPointStateTable = Singleton<Game>.Instance.Environment?.OnceActionPointStateTable;
      if (actionPointStateTable == null)
        return;
      if (Object.op_Inequality((Object) ((Component) this).get_gameObject(), (Object) null) && ((Component) this).get_gameObject().get_activeSelf() != active)
        ((Component) this).get_gameObject().SetActive(active);
      actionPointStateTable[this.RegisterID] = !active;
    }

    public bool IsAvailable()
    {
      if (!Singleton<Game>.IsInstance())
        return false;
      Dictionary<int, bool> actionPointStateTable = Singleton<Game>.Instance.Environment?.OnceActionPointStateTable;
      if (actionPointStateTable == null)
        return false;
      bool flag;
      if (!actionPointStateTable.TryGetValue(this.RegisterID, out flag))
        actionPointStateTable[this.RegisterID] = flag = false;
      return !flag;
    }
  }
}
