// Decompiled with JetBrains decompiler
// Type: AIProject.InfiniteTurnScroll
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AIProject
{
  public class InfiniteTurnScroll : UIBehaviour
  {
    [SerializeField]
    private RectTransform _optionNode;
    [SerializeField]
    [Range(0.0f, 30f)]
    private int _instantiateCount;
    [SerializeField]
    private InfiniteTurnScroll.DirectionType _direction;
    [SerializeField]
    private OptionPositionChangeEvent _onUpdate;
    private LinkedList<RectTransform> _options;
    protected float _diffPreFramePosition;
    protected int _currentItemNo;
    private RectTransform _rectTransform;
    private float _optionSize;

    public InfiniteTurnScroll()
    {
      base.\u002Ector();
    }

    public InfiniteTurnScroll.DirectionType Direction
    {
      get
      {
        return this._direction;
      }
      set
      {
        this._direction = value;
      }
    }

    public OptionPositionChangeEvent OnUpdate
    {
      get
      {
        return this._onUpdate;
      }
      set
      {
        this._onUpdate = value;
      }
    }

    public LinkedList<RectTransform> Options
    {
      get
      {
        return this._options;
      }
      set
      {
        this._options = new LinkedList<RectTransform>((IEnumerable<RectTransform>) value);
      }
    }

    public RectTransform RectTransform
    {
      get
      {
        return this._rectTransform ?? (this._rectTransform = (RectTransform) ((Component) this).GetComponent<RectTransform>());
      }
    }

    private float AnchoredPosition
    {
      get
      {
        switch (this._direction)
        {
          case InfiniteTurnScroll.DirectionType.Vertical:
            return (float) -this.RectTransform.get_anchoredPosition().y;
          case InfiniteTurnScroll.DirectionType.Horizontal:
            return (float) this.RectTransform.get_anchoredPosition().x;
          default:
            return this._direction == InfiniteTurnScroll.DirectionType.Vertical ? (float) -this._rectTransform.get_anchoredPosition().y : (float) this._rectTransform.get_anchoredPosition().x;
        }
      }
    }

    public float OptionSize
    {
      get
      {
        if (Object.op_Inequality((Object) this._optionNode, (Object) null) && (double) this._optionSize == -1.0)
          this._optionSize = this._direction != InfiniteTurnScroll.DirectionType.Vertical ? (float) this._optionNode.get_sizeDelta().x : (float) this._optionNode.get_sizeDelta().y;
        return this._optionSize;
      }
    }

    protected virtual void Start()
    {
      IInfiniteScrollControl[] components = (IInfiniteScrollControl[]) ((Component) this).GetComponents<IInfiniteScrollControl>();
      ScrollRect componentInParent = (ScrollRect) ((Component) this).GetComponentInParent<ScrollRect>();
      componentInParent.set_horizontal(this._direction == InfiniteTurnScroll.DirectionType.Horizontal);
      componentInParent.set_vertical(this._direction == InfiniteTurnScroll.DirectionType.Vertical);
      componentInParent.set_content(this._rectTransform);
      ((Component) this._optionNode).get_gameObject().SetActive(false);
      for (int count = 0; count < this._instantiateCount; ++count)
      {
        RectTransform transform = ((GameObject) Object.Instantiate<GameObject>((M0) ((Component) this._optionNode).get_gameObject())).get_transform() as RectTransform;
        ((Transform) transform).SetParent(((Component) this).get_transform(), false);
        ((Object) transform).set_name(string.Format("Item {0}", (object) count.ToString("000")));
        transform.set_anchoredPosition(this._direction != InfiniteTurnScroll.DirectionType.Vertical ? new Vector2(this.OptionSize * (float) count, 0.0f) : new Vector2(0.0f, -this.OptionSize * (float) count));
        this._options.AddLast(transform);
        ((Component) transform).get_gameObject().SetActive(true);
        foreach (IInfiniteScrollControl infiniteScrollControl in components)
          infiniteScrollControl.OnUpdateOption(count, ((Component) transform).get_gameObject());
      }
      foreach (IInfiniteScrollControl infiniteScrollControl in components)
        infiniteScrollControl.OnPostSetupOption();
    }

    public enum DirectionType
    {
      Vertical,
      Horizontal,
    }
  }
}
