// Decompiled with JetBrains decompiler
// Type: AIProject.MapSourceTag
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using IllusionUtility.GetUtility;
using Manager;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  public class MapSourceTag : MonoBehaviour
  {
    [SerializeField]
    private Transform _startPoint;

    public MapSourceTag()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      if (Object.op_Inequality((Object) this._startPoint, (Object) null))
        Singleton<Manager.Map>.Instance.PlayerStartPoint = this._startPoint;
      Singleton<Manager.Map>.Instance.WaterRefrections = (AQUAS_Reflection[]) ((Component) this).GetComponentsInChildren<AQUAS_Reflection>(true);
      MeshCollider[] componentsInChildren = (MeshCollider[]) ((Component) this).GetComponentsInChildren<MeshCollider>(true);
      if (componentsInChildren.IsNullOrEmpty<MeshCollider>())
        return;
      foreach (MeshCollider meshCollider in componentsInChildren)
      {
        if (((Collider) meshCollider).get_enabled())
          ((Collider) meshCollider).set_enabled(false);
      }
    }

    private void Start()
    {
      ObservableExtensions.Subscribe<Unit>((IObservable<M0>) Singleton<Resources>.Instance.LoadMapResourceStream, (Action<M0>) (_ =>
      {
        this.LoadMapGroups();
        this.LoadAreaOpenObjects();
        this.LoadTimeRelationObjects();
      }));
    }

    private void LoadMapGroups()
    {
      Manager.Map instance = Singleton<Manager.Map>.Instance;
      instance.MapGroupObjList.Clear();
      Dictionary<int, string> dictionary;
      if (!Singleton<Resources>.Instance.Map.MapGroupNameList.TryGetValue(instance.MapID, out dictionary))
        return;
      foreach (KeyValuePair<int, string> keyValuePair in dictionary)
      {
        GameObject loop = ((Component) this).get_transform().FindLoop(keyValuePair.Value);
        if (!Object.op_Equality((Object) loop, (Object) null))
          instance.MapGroupObjList[keyValuePair.Key] = loop;
      }
    }

    private void LoadAreaOpenObjects()
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Dictionary<int, Dictionary<bool, ForcedHideObject[]>> areaOpenObjectTable = Singleton<Manager.Map>.Instance.AreaOpenObjectTable;
      Dictionary<int, Dictionary<bool, string[]>> stateObjectNameTable = Singleton<Resources>.Instance.Map.AreaOpenStateObjectNameTable;
      areaOpenObjectTable.Clear();
      if (stateObjectNameTable.IsNullOrEmpty<int, Dictionary<bool, string[]>>())
        return;
      foreach (KeyValuePair<int, Dictionary<bool, string[]>> keyValuePair1 in stateObjectNameTable)
      {
        if (!keyValuePair1.Value.IsNullOrEmpty<bool, string[]>())
        {
          int key1 = keyValuePair1.Key;
          foreach (KeyValuePair<bool, string[]> keyValuePair2 in keyValuePair1.Value)
          {
            if (!keyValuePair2.Value.IsNullOrEmpty<string>())
            {
              bool key2 = keyValuePair2.Key;
              List<GameObject> gameObjectList = ListPool<GameObject>.Get();
              foreach (string str in keyValuePair2.Value)
              {
                if (!str.IsNullOrEmpty())
                {
                  GameObject loop = ((Component) this).get_transform().FindLoop(str);
                  if (!Object.op_Equality((Object) loop, (Object) null))
                    gameObjectList.Add(loop);
                }
              }
              if (!gameObjectList.IsNullOrEmpty<GameObject>())
              {
                List<ForcedHideObject> toRelease = ListPool<ForcedHideObject>.Get();
                for (int index = 0; index < gameObjectList.Count; ++index)
                {
                  ForcedHideObject orAddComponent = gameObjectList[index].GetOrAddComponent<ForcedHideObject>();
                  if (!Object.op_Equality((Object) orAddComponent, (Object) null) && !toRelease.Contains(orAddComponent))
                  {
                    orAddComponent.Init();
                    toRelease.Add(orAddComponent);
                  }
                }
                ForcedHideObject[] forcedHideObjectArray = new ForcedHideObject[toRelease.Count];
                for (int index = 0; index < forcedHideObjectArray.Length; ++index)
                  forcedHideObjectArray[index] = toRelease[index];
                Dictionary<bool, ForcedHideObject[]> dictionary;
                if (!areaOpenObjectTable.TryGetValue(key1, out dictionary) || dictionary == null)
                  areaOpenObjectTable[key1] = dictionary = new Dictionary<bool, ForcedHideObject[]>();
                dictionary[key2] = forcedHideObjectArray;
                ListPool<ForcedHideObject>.Release(toRelease);
              }
              ListPool<GameObject>.Release(gameObjectList);
            }
          }
        }
      }
      Singleton<Manager.Map>.Instance.RefreshAreaOpenObject();
    }

    private void LoadTimeRelationObjects()
    {
      if (!Singleton<Manager.Map>.IsInstance() || !Singleton<Resources>.IsInstance())
        return;
      Resources instance1 = Singleton<Resources>.Instance;
      Manager.Map instance2 = Singleton<Manager.Map>.Instance;
      string str1 = "_EmissionColor";
      Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>> relationObjectTable = instance2.TimeRelationObjectTable;
      Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>> objectStateTable = instance1.Map.TimeRelationObjectStateTable;
      relationObjectTable.Clear();
      if (objectStateTable.IsNullOrEmpty<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>>())
        return;
      using (Dictionary<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>>.Enumerator enumerator1 = objectStateTable.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<int, Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>> current1 = enumerator1.Current;
          if (!current1.Value.IsNullOrEmpty<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>())
          {
            int key1 = current1.Key;
            using (Dictionary<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
            {
              while (enumerator2.MoveNext())
              {
                KeyValuePair<int, Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>> current2 = enumerator2.Current;
                if (!current2.Value.IsNullOrEmpty<bool, Dictionary<int, List<ValueTuple<string, float>>>>())
                {
                  int key2 = current2.Key;
                  using (Dictionary<bool, Dictionary<int, List<ValueTuple<string, float>>>>.Enumerator enumerator3 = current2.Value.GetEnumerator())
                  {
                    while (enumerator3.MoveNext())
                    {
                      KeyValuePair<bool, Dictionary<int, List<ValueTuple<string, float>>>> current3 = enumerator3.Current;
                      if (!current3.Value.IsNullOrEmpty<int, List<ValueTuple<string, float>>>())
                      {
                        bool key3 = current3.Key;
                        using (Dictionary<int, List<ValueTuple<string, float>>>.Enumerator enumerator4 = current3.Value.GetEnumerator())
                        {
                          while (enumerator4.MoveNext())
                          {
                            KeyValuePair<int, List<ValueTuple<string, float>>> current4 = enumerator4.Current;
                            if (!current4.Value.IsNullOrEmpty<ValueTuple<string, float>>())
                            {
                              int key4 = current4.Key;
                              ValueTuple<GameObject, Material, float, Color>[] valueTupleArray = (ValueTuple<GameObject, Material, float, Color>[]) null;
                              switch (key2)
                              {
                                case 0:
                                  List<GameObject> gameObjectList = ListPool<GameObject>.Get();
                                  using (List<ValueTuple<string, float>>.Enumerator enumerator5 = current4.Value.GetEnumerator())
                                  {
                                    while (enumerator5.MoveNext())
                                    {
                                      string str2 = (string) enumerator5.Current.Item1;
                                      if (!str2.IsNullOrEmpty())
                                      {
                                        GameObject loop = ((Component) this).get_transform().FindLoop(str2);
                                        if (!Object.op_Equality((Object) loop, (Object) null))
                                          gameObjectList.Add(loop);
                                      }
                                    }
                                  }
                                  if (!gameObjectList.IsNullOrEmpty<GameObject>())
                                  {
                                    valueTupleArray = new ValueTuple<GameObject, Material, float, Color>[gameObjectList.Count];
                                    for (int index = 0; index < valueTupleArray.Length; ++index)
                                      valueTupleArray[index] = new ValueTuple<GameObject, Material, float, Color>(gameObjectList[index], (Material) null, 0.0f, Color.get_white());
                                  }
                                  ListPool<GameObject>.Release(gameObjectList);
                                  break;
                                case 1:
                                  List<ValueTuple<GameObject, Material, float, Color>> valueTupleList = ListPool<ValueTuple<GameObject, Material, float, Color>>.Get();
                                  using (List<ValueTuple<string, float>>.Enumerator enumerator5 = current4.Value.GetEnumerator())
                                  {
                                    while (enumerator5.MoveNext())
                                    {
                                      ValueTuple<string, float> current5 = enumerator5.Current;
                                      string str2 = (string) current5.Item1;
                                      if (!str2.IsNullOrEmpty())
                                      {
                                        GameObject loop = ((Component) this).get_transform().FindLoop(str2);
                                        if (!Object.op_Equality((Object) loop, (Object) null))
                                        {
                                          Renderer componentInChildren = (Renderer) loop.GetComponentInChildren<Renderer>(true);
                                          if (!Object.op_Equality((Object) componentInChildren?.get_material()?.get_shader(), (Object) null))
                                          {
                                            Color color = Color.get_white();
                                            if (componentInChildren.get_material().HasProperty(str1))
                                            {
                                              color = componentInChildren.get_material().GetColor(str1);
                                              if (1.0 < color.r)
                                                color.r = (__Null) (double) Mathf.Repeat((float) color.r, 1f);
                                              if (1.0 < color.g)
                                                color.g = (__Null) (double) Mathf.Repeat((float) color.g, 1f);
                                              if (1.0 < color.b)
                                                color.b = (__Null) (double) Mathf.Repeat((float) color.b, 1f);
                                              if (1.0 < color.a)
                                                color.a = (__Null) (double) Mathf.Repeat((float) color.a, 1f);
                                            }
                                            valueTupleList.Add(new ValueTuple<GameObject, Material, float, Color>(loop, componentInChildren.get_material(), (float) current5.Item2, color));
                                          }
                                        }
                                      }
                                    }
                                  }
                                  if (!valueTupleList.IsNullOrEmpty<ValueTuple<GameObject, Material, float, Color>>())
                                  {
                                    valueTupleArray = new ValueTuple<GameObject, Material, float, Color>[valueTupleList.Count];
                                    for (int index = 0; index < valueTupleArray.Length; ++index)
                                      valueTupleArray[index] = new ValueTuple<GameObject, Material, float, Color>((GameObject) valueTupleList[index].Item1, (Material) valueTupleList[index].Item2, (float) valueTupleList[index].Item3, (Color) valueTupleList[index].Item4);
                                  }
                                  ListPool<ValueTuple<GameObject, Material, float, Color>>.Release(valueTupleList);
                                  break;
                              }
                              if (valueTupleArray == null)
                                valueTupleArray = new ValueTuple<GameObject, Material, float, Color>[0];
                              Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>> dictionary1;
                              if (!relationObjectTable.TryGetValue(key1, out dictionary1))
                                relationObjectTable[key1] = dictionary1 = new Dictionary<int, Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>>();
                              Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>> dictionary2;
                              if (!dictionary1.TryGetValue(key2, out dictionary2))
                                dictionary1[key2] = dictionary2 = new Dictionary<bool, Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>>();
                              Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]> dictionary3;
                              if (!dictionary2.TryGetValue(key3, out dictionary3))
                                dictionary2[key3] = dictionary3 = new Dictionary<int, ValueTuple<GameObject, Material, float, Color>[]>();
                              dictionary3[key4] = valueTupleArray;
                            }
                          }
                        }
                      }
                    }
                  }
                }
              }
            }
          }
        }
      }
      instance2.RefreshActiveTimeRelationObjects();
    }
  }
}
