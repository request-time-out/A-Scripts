// Decompiled with JetBrains decompiler
// Type: Studio.MapList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Studio
{
  public class MapList : MonoBehaviour
  {
    [SerializeField]
    private GameObject objectNode;
    [SerializeField]
    private Transform transformRoot;
    private int select;
    private Dictionary<int, ListNode> dicNode;
    private bool isInit;

    public MapList()
    {
      base.\u002Ector();
    }

    public void UpdateInfo()
    {
      if (!this.isInit)
        return;
      foreach (KeyValuePair<int, ListNode> keyValuePair in this.dicNode)
        keyValuePair.Value.select = false;
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(Singleton<Studio.Studio>.Instance.sceneInfo.map, out listNode))
      {
        listNode.select = true;
        this.select = Singleton<Studio.Studio>.Instance.sceneInfo.map;
      }
      else
      {
        if (!this.dicNode.TryGetValue(-1, out listNode))
          return;
        listNode.select = true;
        this.select = -1;
      }
    }

    public void OnClick(int _no)
    {
      Singleton<Studio.Studio>.Instance.AddMap(_no, Studio.Studio.optionSystem.autoHide, false, false);
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(this.select, out listNode))
        listNode.select = false;
      if (this.dicNode.TryGetValue(_no, out listNode))
        listNode.select = true;
      this.select = _no;
    }

    public void Init()
    {
      this.AddNode(-1, "なし");
      foreach (KeyValuePair<int, Info.MapLoadInfo> keyValuePair in Singleton<Info>.Instance.dicMapLoadInfo)
        this.AddNode(keyValuePair.Key, keyValuePair.Value.name);
      ListNode listNode = (ListNode) null;
      if (this.dicNode.TryGetValue(Singleton<Studio.Studio>.Instance.sceneInfo.map, out listNode))
        listNode.select = true;
      this.isInit = true;
    }

    private void AddNode(int _key, string _name)
    {
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      MapList.\u003CAddNode\u003Ec__AnonStorey0 nodeCAnonStorey0 = new MapList.\u003CAddNode\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.\u0024this = this;
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.objectNode);
      gameObject.get_transform().SetParent(this.transformRoot, false);
      if (!gameObject.get_activeSelf())
        gameObject.SetActive(true);
      ListNode component = (ListNode) gameObject.GetComponent<ListNode>();
      // ISSUE: reference to a compiler-generated field
      nodeCAnonStorey0.key = _key;
      // ISSUE: method pointer
      component.AddActionToButton(new UnityAction((object) nodeCAnonStorey0, __methodptr(\u003C\u003Em__0)));
      component.text = _name;
      // ISSUE: reference to a compiler-generated field
      this.dicNode.Add(nodeCAnonStorey0.key, component);
    }
  }
}
