// Decompiled with JetBrains decompiler
// Type: Studio.ItemCategoryList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class ItemCategoryList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private ItemList itemList;
    private Studio.Item.ListNodePool listNodePool;
    private Dictionary<int, Studio.Anime.ListNode> dicNode;
    private bool isInit;
    private int group;
    private int select;

    public ItemCategoryList()
    {
      base.\u002Ector();
    }

    public bool active
    {
      get
      {
        return ((Component) this).get_gameObject().get_activeSelf();
      }
      set
      {
        if (((Component) this).get_gameObject().get_activeSelf() == value)
          return;
        ((Component) this).get_gameObject().SetActive(value);
        if (((Component) this).get_gameObject().get_activeSelf())
          return;
        this.itemList.active = false;
      }
    }

    public void InitList(int _group)
    {
      this.Init();
      this.listNodePool.Return();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode.Clear();
      foreach (KeyValuePair<int, Info.CategoryInfo> keyValuePair in (IEnumerable<KeyValuePair<int, Info.CategoryInfo>>) Singleton<Info>.Instance.dicItemGroupCategory[_group].dicCategory.OrderBy<KeyValuePair<int, Info.CategoryInfo>, int>((Func<KeyValuePair<int, Info.CategoryInfo>, int>) (v => v.Value.sort)))
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: method pointer
        Studio.Anime.ListNode listNode = this.listNodePool.Rent(keyValuePair.Value.name, new UnityAction((object) new ItemCategoryList.\u003CInitList\u003Ec__AnonStorey0()
        {
          \u0024this = this,
          no = keyValuePair.Key
        }, __methodptr(\u003C\u003Em__0)), false);
        this.dicNode.Add(keyValuePair.Key, listNode);
      }
      this.select = -1;
      this.group = _group;
      this.active = true;
      this.itemList.active = false;
    }

    private void OnSelect(int _no)
    {
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      this.itemList.InitList(this.group, _no);
      Studio.Anime.ListNode listNode1 = (Studio.Anime.ListNode) null;
      if (this.dicNode.TryGetValue(select, out listNode1) && Object.op_Inequality((Object) listNode1, (Object) null))
        listNode1.Select = false;
      Studio.Anime.ListNode listNode2 = (Studio.Anime.ListNode) null;
      if (!this.dicNode.TryGetValue(this.select, out listNode2) || !Object.op_Inequality((Object) listNode2, (Object) null))
        return;
      listNode2.Select = true;
    }

    private void Init()
    {
      if (this.isInit)
        return;
      this.listNodePool = new Studio.Item.ListNodePool(this.transformRoot, (Studio.Anime.ListNode) this.objectPrefab.GetComponent<Studio.Anime.ListNode>());
      this.dicNode = new Dictionary<int, Studio.Anime.ListNode>();
      this.isInit = true;
    }
  }
}
