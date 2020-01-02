// Decompiled with JetBrains decompiler
// Type: Studio.AnimeList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Studio
{
  public class AnimeList : MonoBehaviour
  {
    [SerializeField]
    private Transform transformRoot;
    [SerializeField]
    private GameObject objectPrefab;
    [SerializeField]
    private ScrollRect scrollRect;
    [SerializeField]
    private MPCharCtrl mpCharCtrl;
    private AnimeList.ListNodePool listNodePool;
    private bool isInit;
    private AnimeGroupList.SEX sex;
    private int group;
    private int category;
    private int select;
    private Dictionary<int, Studio.Anime.ListNode> dicNode;

    public AnimeList()
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
      }
    }

    public void InitList(AnimeGroupList.SEX _sex, int _group, int _category)
    {
      this.Init();
      this.listNodePool.Return();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.dicNode.Clear();
      foreach (KeyValuePair<int, Info.AnimeLoadInfo> keyValuePair in Singleton<Info>.Instance.dicAnimeLoadInfo[_group][_category])
      {
        // ISSUE: object of a compiler-generated type is created
        // ISSUE: method pointer
        Studio.Anime.ListNode listNode = this.listNodePool.Rent(keyValuePair.Value.name, new UnityAction((object) new AnimeList.\u003CInitList\u003Ec__AnonStorey0()
        {
          \u0024this = this,
          no = keyValuePair.Key
        }, __methodptr(\u003C\u003Em__0)));
        this.dicNode.Add(keyValuePair.Key, listNode);
      }
      this.sex = _sex;
      this.group = _group;
      this.category = _category;
      this.select = -1;
      this.active = true;
    }

    private void OnSelect(int _no)
    {
      this.mpCharCtrl.LoadAnime(this.sex, this.group, this.category, _no);
      int select = this.select;
      if (!Utility.SetStruct<int>(ref this.select, _no))
        return;
      Studio.Anime.ListNode listNode = (Studio.Anime.ListNode) null;
      if (this.dicNode.TryGetValue(select, out listNode) && Object.op_Inequality((Object) listNode, (Object) null))
        listNode.Select = false;
      listNode = (Studio.Anime.ListNode) null;
      if (!this.dicNode.TryGetValue(this.select, out listNode) || !Object.op_Inequality((Object) listNode, (Object) null))
        return;
      listNode.Select = true;
    }

    private void Init()
    {
      if (this.isInit)
        return;
      this.listNodePool = new AnimeList.ListNodePool(this.transformRoot, (Studio.Anime.ListNode) this.objectPrefab.GetComponent<Studio.Anime.ListNode>());
      this.dicNode = new Dictionary<int, Studio.Anime.ListNode>();
      this.isInit = true;
    }

    private class ListNodePool : ObjectPool<Studio.Anime.ListNode>
    {
      private readonly Transform parent;
      private readonly Studio.Anime.ListNode prefab;
      private List<Studio.Anime.ListNode> nodes;

      public ListNodePool(Transform _parent, Studio.Anime.ListNode _prefab)
      {
        this.\u002Ector();
        this.parent = _parent;
        this.prefab = _prefab;
        this.nodes = new List<Studio.Anime.ListNode>();
      }

      protected virtual Studio.Anime.ListNode CreateInstance()
      {
        return (Studio.Anime.ListNode) Object.Instantiate<Studio.Anime.ListNode>((M0) this.prefab, this.parent);
      }

      public Studio.Anime.ListNode Rent(string _text, UnityAction _action)
      {
        Studio.Anime.ListNode listNode = this.Rent();
        ((Component) listNode).get_transform().SetAsLastSibling();
        listNode.Select = false;
        this.nodes.Add(listNode);
        listNode.Text = _text;
        listNode.SetButtonAction(_action);
        return listNode;
      }

      public void Return()
      {
        foreach (Studio.Anime.ListNode node in this.nodes)
          this.Return(node);
        this.nodes.Clear();
      }
    }
  }
}
