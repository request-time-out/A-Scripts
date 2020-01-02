// Decompiled with JetBrains decompiler
// Type: UI_TreeView
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_TreeView : MonoBehaviour
{
  public bool rootFlag;
  public bool ExpandFirst;
  public float topMargin;
  public float bottomMargin;
  public ScrollRect scrollRect;
  public RectTransform rtScroll;
  public bool unused;
  private Toggle minmax;
  private UI_TreeView tvRoot;
  private List<UI_TreeView> lstChild;

  public UI_TreeView()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    if (!this.rootFlag)
      return;
    this.CreateTree(this);
    if (this.ExpandFirst)
      this.ExpandAll();
    else
      this.CollapseAll();
  }

  private void CreateTree(UI_TreeView tvroot)
  {
    if (Object.op_Equality((Object) null, (Object) this.minmax))
      this.minmax = (Toggle) ((Component) this).get_gameObject().GetComponent<Toggle>();
    if (Object.op_Implicit((Object) this.minmax))
    {
      // ISSUE: method pointer
      ((UnityEvent<bool>) this.minmax.onValueChanged).AddListener(new UnityAction<bool>((object) this, __methodptr(MinMaxChange)));
    }
    this.tvRoot = tvroot;
    IEnumerator enumerator = ((Component) this).get_gameObject().get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        UI_TreeView component = (UI_TreeView) ((Component) enumerator.Current).get_gameObject().GetComponent<UI_TreeView>();
        if (!Object.op_Equality((Object) null, (Object) component))
        {
          this.lstChild.Add(component);
          component.CreateTree(tvroot);
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void UpdateView(ref float totalPosY, float parentPosY)
  {
    float parentPosY1 = totalPosY;
    if (this.rootFlag)
    {
      totalPosY = -this.topMargin;
    }
    else
    {
      RectTransform component = (RectTransform) ((Component) this).get_gameObject().GetComponent<RectTransform>();
      if (Object.op_Implicit((Object) component))
      {
        component.set_anchoredPosition(new Vector2((float) component.get_anchoredPosition().x, totalPosY - parentPosY));
        if (((Component) this).get_gameObject().get_activeSelf() && !this.unused)
          totalPosY -= (float) component.get_sizeDelta().y;
      }
    }
    IEnumerator enumerator = ((Component) this).get_gameObject().get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        Transform current = (Transform) enumerator.Current;
        UI_TreeView component = (UI_TreeView) ((Component) current).get_gameObject().GetComponent<UI_TreeView>();
        if (!Object.op_Equality((Object) null, (Object) component))
        {
          if (!((Component) this).get_gameObject().get_activeSelf() || component.unused)
            ((Component) current).get_gameObject().SetActive(false);
          else if (Object.op_Implicit((Object) this.minmax))
            ((Component) current).get_gameObject().SetActive(this.minmax.get_isOn());
          component.UpdateView(ref totalPosY, parentPosY1);
        }
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
    if (!this.rootFlag || !Object.op_Implicit((Object) this.rtScroll))
      return;
    this.rtScroll.set_sizeDelta(new Vector2((float) this.rtScroll.get_sizeDelta().x, -totalPosY + this.bottomMargin));
    if (!Object.op_Implicit((Object) this.scrollRect))
      return;
    ((Behaviour) this.scrollRect).set_enabled(false);
    ((Behaviour) this.scrollRect).set_enabled(true);
  }

  public void ExpandAll()
  {
    if (!this.rootFlag)
      return;
    this.ChangeExpandOrCollapseLoop(true);
    float totalPosY = 0.0f;
    this.UpdateView(ref totalPosY, 0.0f);
  }

  public void CollapseAll()
  {
    if (!this.rootFlag)
      return;
    this.ChangeExpandOrCollapseLoop(false);
    float totalPosY = 0.0f;
    this.UpdateView(ref totalPosY, 0.0f);
  }

  private void ChangeExpandOrCollapseLoop(bool expand)
  {
    if (Object.op_Implicit((Object) this.minmax))
      this.minmax.set_isOn(expand);
    IEnumerator enumerator = ((Component) this).get_gameObject().get_transform().GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        UI_TreeView component = (UI_TreeView) ((Component) enumerator.Current).get_gameObject().GetComponent<UI_TreeView>();
        if (!Object.op_Equality((Object) null, (Object) component))
          component.ChangeExpandOrCollapseLoop(expand);
      }
    }
    finally
    {
      if (enumerator is IDisposable disposable)
        disposable.Dispose();
    }
  }

  public void SetUnused(bool flag)
  {
    this.unused = flag;
    ((Component) this).get_gameObject().SetActive(!this.unused);
  }

  private void Update()
  {
  }

  private void MinMaxChange(bool flag)
  {
    float totalPosY = 0.0f;
    if (!Object.op_Implicit((Object) this.tvRoot))
      return;
    this.tvRoot.UpdateView(ref totalPosY, 0.0f);
  }

  public void UpdateView()
  {
    float totalPosY = 0.0f;
    if (!Object.op_Implicit((Object) this.tvRoot) || !((Component) this.tvRoot).get_gameObject().get_activeSelf())
      return;
    this.tvRoot.UpdateView(ref totalPosY, 0.0f);
  }

  public UI_TreeView GetRoot()
  {
    return this.tvRoot;
  }
}
