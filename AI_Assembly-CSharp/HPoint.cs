// Decompiled with JetBrains decompiler
// Type: HPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEx;

[Serializable]
public class HPoint : MonoBehaviour
{
  private static List<HScene.AnimationListInfo>[] animationLists;
  [Tooltip("登録ID")]
  public int id;
  private Dictionary<int, ValueTuple<int, int>> nPlace;
  [Tooltip("あたり")]
  public Transform markerPos;
  public Transform pivot;
  private Collider col;
  [Space]
  [Header("終了時の復帰ポイント")]
  public Transform endPlayerPos;
  public Transform[] endFemalePos;
  private ParticleSystem[] effect;
  public List<int> OpenID;
  [Header("デバッグ表示 モーション除外リスト")]
  public HPoint.NotMotionInfo[] notMotion;
  private HPoint.HpointData data;

  public HPoint()
  {
    base.\u002Ector();
  }

  public Dictionary<int, ValueTuple<int, int>> _nPlace
  {
    get
    {
      return this.nPlace;
    }
    set
    {
      this.nPlace = value;
    }
  }

  public void Init()
  {
    if (HPoint.animationLists == null)
      HPoint.animationLists = Singleton<Resources>.Instance.HSceneTable.lstAnimInfo;
    if (Object.op_Inequality((Object) this.markerPos, (Object) null))
    {
      this.col = (Collider) ((Component) this.markerPos).GetComponent<Collider>();
      if (Object.op_Inequality((Object) this.col, (Object) null))
        this.col.set_enabled(false);
    }
    this.effect = (ParticleSystem[]) ((Component) this).get_gameObject().GetComponentsInChildren<ParticleSystem>(true);
    if (this.effect != null)
    {
      foreach (ParticleSystem particleSystem in this.effect)
      {
        particleSystem.Stop();
        ((Component) particleSystem).get_gameObject().SetActive(false);
      }
    }
    if (!Singleton<Resources>.Instance.HSceneTable.loadHPointDatas.TryGetValue(this.id, out this.data))
      return;
    using (Dictionary<int, ValueTuple<int, int>>.Enumerator enumerator = this.data.place.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<int, ValueTuple<int, int>> current = enumerator.Current;
        this.nPlace[current.Key] = new ValueTuple<int, int>((int) current.Value.Item1, (int) current.Value.Item2);
      }
    }
    for (int index = 0; index < 6; ++index)
    {
      foreach (HScene.AnimationListInfo animationListInfo in HPoint.animationLists[index])
      {
        if (this.data.notMotion[index].motionID.Contains(animationListInfo.id))
        {
          this.notMotion[index].motionID.Add(animationListInfo.id);
          this.notMotion[index].motionNames.Add(animationListInfo.nameAnimation);
        }
      }
    }
  }

  public Collider GetCollider()
  {
    return Object.op_Inequality((Object) this.col, (Object) null) ? this.col : (Collider) null;
  }

  public bool EffectActive()
  {
    bool flag = true;
    if (this.effect == null)
      return false;
    for (int index = 0; index < this.effect.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.effect[index], (Object) null))
        flag = flag & ((Component) this.effect[index]).get_gameObject().get_activeSelf() & this.effect[index].get_isPlaying();
    }
    return flag;
  }

  public void SetEffectActive(bool set)
  {
    if (this.effect == null)
      return;
    for (int index = 0; index < this.effect.Length; ++index)
    {
      if (!Object.op_Equality((Object) this.effect[index], (Object) null))
      {
        if (set)
          this.effect[index].Play();
        else
          this.effect[index].Stop();
        ((Component) this.effect[index]).get_gameObject().SetActive(set);
      }
    }
  }

  [Serializable]
  public struct NotMotionInfo
  {
    [HideInInspector]
    public List<int> motionID;
    public List<string> motionNames;
  }

  [Serializable]
  public class HpointData
  {
    public Dictionary<int, ValueTuple<int, int>> place = new Dictionary<int, ValueTuple<int, int>>();
    public HPoint.NotMotionInfo[] notMotion = new HPoint.NotMotionInfo[6];
  }
}
