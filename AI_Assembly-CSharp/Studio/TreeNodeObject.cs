// Decompiled with JetBrains decompiler
// Type: Studio.TreeNodeObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using GUITree;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class TreeNodeObject : MonoBehaviour
  {
    [SerializeField]
    protected TreeNode m_TreeNode;
    [SerializeField]
    protected Sprite[] m_SpriteState;
    [SerializeField]
    protected Button m_ButtonState;
    [SerializeField]
    protected Button m_ButtonSelect;
    [SerializeField]
    protected Image m_ImageSelect;
    [SerializeField]
    protected RectTransform m_TransSelect;
    [SerializeField]
    protected Text m_TextName;
    [SerializeField]
    protected Canvas canvas;
    protected TreeNodeObject.TreeState m_TreeState;
    protected Image m_ImageState;
    [SerializeField]
    protected TreeNodeCtrl m_TreeNodeCtrl;
    protected bool m_Visible;
    [SerializeField]
    protected Button m_ButtonVisible;
    protected Image m_ImageVisible;
    [SerializeField]
    protected Sprite[] m_SpriteVisible;
    public TreeNodeObject.OnVisibleFunc onVisible;
    [SerializeField]
    private float textPosX;
    [Space(10f)]
    [SerializeField]
    private RectTransform _rectNode;
    protected List<TreeNodeObject> m_child;
    private float _addPosX;
    public Action onDelete;
    public TreeNodeObject.CheckFunc checkChild;
    public TreeNodeObject.CheckFunc checkParent;

    public TreeNodeObject()
    {
      base.\u002Ector();
    }

    public TreeNode treeNode
    {
      get
      {
        return this.m_TreeNode;
      }
    }

    public Button buttonState
    {
      get
      {
        return this.m_ButtonState;
      }
    }

    public Button buttonSelect
    {
      get
      {
        return this.m_ButtonSelect;
      }
    }

    public Image imageSelect
    {
      get
      {
        return this.m_ImageSelect;
      }
    }

    public Color colorSelect
    {
      set
      {
        ((Graphic) this.imageSelect).set_color(value);
      }
    }

    public string textName
    {
      get
      {
        return this.m_TextName.get_text();
      }
      set
      {
        this.m_TextName.set_text(value);
      }
    }

    public TreeNodeObject.TreeState treeState
    {
      get
      {
        return this.m_TreeState;
      }
      set
      {
        if (!Utility.SetStruct<TreeNodeObject.TreeState>(ref this.m_TreeState, value))
          return;
        this.SetTreeState(this.m_TreeState);
      }
    }

    public Image imageState
    {
      get
      {
        if (Object.op_Equality((Object) this.m_ImageState, (Object) null))
          this.m_ImageState = (Image) ((Component) this.m_ButtonState).GetComponent<Image>();
        return this.m_ImageState;
      }
    }

    public bool visible
    {
      get
      {
        return this.m_Visible;
      }
      set
      {
        this.SetVisible(value);
      }
    }

    public Button buttonVisible
    {
      get
      {
        return this.m_ButtonVisible;
      }
    }

    public Image imageVisible
    {
      get
      {
        if (Object.op_Equality((Object) this.m_ImageVisible, (Object) null))
          this.m_ImageVisible = ((Selectable) this.m_ButtonVisible).get_image();
        return this.m_ImageVisible;
      }
    }

    public float imageVisibleWidth
    {
      get
      {
        return (float) ((Graphic) this.imageVisible).get_rectTransform().get_sizeDelta().x;
      }
    }

    public bool enableVisible
    {
      set
      {
        ((Component) this.m_ButtonVisible).get_gameObject().SetActive(value);
        this.RecalcSelectButtonPos();
      }
    }

    public RectTransform rectNode
    {
      get
      {
        return this._rectNode;
      }
    }

    public TreeNodeObject parent { get; set; }

    public bool isParent
    {
      get
      {
        return Object.op_Inequality((Object) this.parent, (Object) null) && this.enableChangeParent;
      }
    }

    public int childCount
    {
      get
      {
        return this.m_child.Count;
      }
    }

    public List<TreeNodeObject> child
    {
      get
      {
        return this.m_child;
      }
    }

    public bool enableChangeParent { get; set; }

    public bool enableDelete { get; set; }

    public bool enableAddChild { get; set; }

    public bool enableCopy { get; set; }

    public Color baseColor { get; set; }

    public float addPosX
    {
      set
      {
        this._addPosX = value;
        this.RecalcSelectButtonPos();
      }
    }

    public TreeNodeObject childRoot { get; set; }

    public void OnClickState()
    {
      SortCanvas.select = this.canvas;
      this.SetTreeState(this.m_TreeState != TreeNodeObject.TreeState.Open ? TreeNodeObject.TreeState.Open : TreeNodeObject.TreeState.Close);
    }

    public void OnClickSelect()
    {
      this.Select(true);
    }

    public void OnClickVisible()
    {
      SortCanvas.select = this.canvas;
      this.SetVisible(!this.m_Visible);
    }

    public void OnDeselect()
    {
      this.colorSelect = this.baseColor;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      if (!Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(this, out objectCtrlInfo))
        return;
      objectCtrlInfo.guideObject.SetActive(false, true);
    }

    public bool SetParent(TreeNodeObject _parent)
    {
      if (!this.enableChangeParent || !this.CheckChildLoop(this, _parent))
        return false;
      if (Object.op_Inequality((Object) _parent, (Object) null) && Object.op_Inequality((Object) _parent.childRoot, (Object) null))
        _parent = _parent.childRoot;
      if (this.CheckParentLoop(_parent, this) || Object.op_Implicit((Object) _parent) && (_parent.child.Contains(this) || !_parent.enableAddChild))
        return false;
      bool flag = true;
      if (this.checkParent != null)
        flag &= this.checkParent(_parent);
      if (!flag)
        return false;
      if (Object.op_Inequality((Object) this.parent, (Object) null))
        this.parent.RemoveChild(this, false);
      if (Object.op_Implicit((Object) _parent))
      {
        _parent.AddChild(this);
      }
      else
      {
        this.parent = (TreeNodeObject) null;
        this.m_TreeNodeCtrl.AddNode(this);
      }
      return true;
    }

    public bool AddChild(TreeNodeObject _child)
    {
      if (!this.enableAddChild || Object.op_Equality((Object) _child, (Object) null) || (Object.op_Equality((Object) _child, (Object) this) || this.m_child.Contains(_child)))
        return false;
      this.m_child.Add(_child);
      _child.parent = this;
      this.m_TreeNodeCtrl.RemoveNode(_child);
      this.SetStateVisible(true);
      this.SetTreeState(this.m_TreeState);
      this.SetVisibleChild(_child, this.m_Visible);
      return true;
    }

    public void RemoveChild(TreeNodeObject _child, bool _removeOnly = false)
    {
      ((Component) _child).get_transform().SetAsLastSibling();
      this.m_child.Remove(_child);
      if (_removeOnly)
        return;
      _child.parent = (TreeNodeObject) null;
      this.m_TreeNodeCtrl.AddNode(_child);
      this.SetStateVisible(this.childCount != 0);
    }

    public void SetTreeState(TreeNodeObject.TreeState _state)
    {
      this.m_TreeState = _state;
      this.imageState.set_sprite(this.m_SpriteState[(int) _state]);
      bool _visible = _state == TreeNodeObject.TreeState.Open;
      foreach (TreeNodeObject _source in this.m_child)
        this.SetVisibleLoop(_source, _visible);
    }

    public void SetVisible(bool _visible)
    {
      this.m_Visible = _visible;
      if (this.onVisible != null)
        this.onVisible(_visible);
      this.imageVisible.set_sprite(this.m_SpriteVisible[!_visible ? 0 : 1]);
      foreach (TreeNodeObject _source in this.m_child)
        this.SetVisibleChild(_source, _visible);
    }

    public void ResetVisible()
    {
      if (this.onVisible != null)
        this.onVisible(this.m_Visible);
      this.imageVisible.set_sprite(this.m_SpriteVisible[!this.m_Visible ? 0 : 1]);
      ((Selectable) this.buttonVisible).set_interactable(true);
      foreach (TreeNodeObject _source in this.child)
        this.SetVisibleChild(_source, this.m_Visible);
    }

    public void Select(bool _button = false)
    {
      SortCanvas.select = this.canvas;
      TreeNodeObject selectNode = this.m_TreeNodeCtrl.selectNode;
      if (_button && Object.op_Inequality((Object) selectNode, (Object) null) && Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303))
        this.m_TreeNodeCtrl.SelectMultiple(selectNode, this);
      else
        this.m_TreeNodeCtrl.selectNode = this;
    }

    protected void SetStateVisible(bool _visible)
    {
      if (!Object.op_Implicit((Object) this.m_ButtonState))
        return;
      ((Component) this.m_ButtonState).get_gameObject().SetActive(_visible);
    }

    protected void SetVisibleLoop(TreeNodeObject _source, bool _visible)
    {
      if (((Component) _source).get_gameObject().get_activeSelf() != _visible)
        ((Component) _source).get_gameObject().SetActive(_visible);
      if (_visible && _source.treeState == TreeNodeObject.TreeState.Close)
        _visible = false;
      foreach (TreeNodeObject _source1 in _source.child)
        this.SetVisibleLoop(_source1, _visible);
    }

    protected bool CheckParentLoop(TreeNodeObject _source, TreeNodeObject _target)
    {
      if (Object.op_Equality((Object) _source, (Object) null))
        return false;
      return Object.op_Equality((Object) _source, (Object) _target) || this.CheckParentLoop(_source.parent, _target);
    }

    protected void SetVisibleChild(TreeNodeObject _source, bool _visible)
    {
      bool flag = (!_visible || _source.visible) && _visible;
      if (_source.onVisible != null)
        _source.onVisible(flag);
      ((Selectable) _source.buttonVisible).set_interactable(_visible);
      foreach (TreeNodeObject _source1 in _source.child)
        this.SetVisibleChild(_source1, flag);
    }

    protected void RecalcSelectButtonPos()
    {
      Vector2 anchoredPosition = this.m_TransSelect.get_anchoredPosition();
      anchoredPosition.x = (__Null) ((double) this._addPosX + (!((Component) this.m_ButtonVisible).get_gameObject().get_activeSelf() ? (double) this.textPosX * 0.5 : (double) this.textPosX));
      this.m_TransSelect.set_anchoredPosition(anchoredPosition);
    }

    protected bool CheckChildLoop(TreeNodeObject _source, TreeNodeObject _parent)
    {
      if (Object.op_Equality((Object) _source, (Object) null) || Object.op_Equality((Object) _parent, (Object) null))
        return true;
      bool flag = true;
      if (_source.checkChild != null)
        flag &= _source.checkChild(_parent);
      if (!flag)
        return false;
      foreach (TreeNodeObject _source1 in _source.child)
      {
        if (!this.CheckChildLoop(_source1, _parent))
          return false;
      }
      return true;
    }

    private void Awake()
    {
      this.enableChangeParent = true;
      this.enableDelete = true;
      this.enableAddChild = true;
      this.enableCopy = true;
      this.baseColor = Utility.ConvertColor(100, 99, 94);
      this.colorSelect = this.baseColor;
    }

    private void Start()
    {
      this.SetStateVisible(this.childCount != 0);
    }

    public enum TreeState
    {
      Open,
      Close,
    }

    public delegate void OnVisibleFunc(bool _b);

    public delegate bool CheckFunc(TreeNodeObject _parent);
  }
}
