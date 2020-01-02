// Decompiled with JetBrains decompiler
// Type: Studio.Item.ListNodePool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UniRx.Toolkit;
using UnityEngine;
using UnityEngine.Events;

namespace Studio.Item
{
  public class ListNodePool : ObjectPool<Studio.Anime.ListNode>
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

    public Studio.Anime.ListNode Rent(string _text, UnityAction _action, bool _textSlide = true)
    {
      Studio.Anime.ListNode listNode = this.Rent();
      ((Component) listNode).get_transform().SetAsLastSibling();
      listNode.Select = false;
      this.nodes.Add(listNode);
      listNode.UseSlide = _textSlide;
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
