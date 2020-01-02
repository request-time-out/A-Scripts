// Decompiled with JetBrains decompiler
// Type: Studio.TreeNodeCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using GUITree;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Studio
{
  public class TreeNodeCtrl : MonoBehaviour, IPointerDownHandler, IEventSystemHandler
  {
    [SerializeField]
    protected GameObject m_ObjectNode;
    [SerializeField]
    protected GameObject m_ObjectRoot;
    [SerializeField]
    protected TreeRoot m_TreeRoot;
    protected List<TreeNodeObject> m_TreeNodeObject;
    protected HashSet<TreeNodeObject> hashSelectNode;
    public Action<TreeNodeObject, TreeNodeObject> onParentage;
    public Action<TreeNodeObject> onDelete;
    public Action<TreeNodeObject> onSelect;
    public Action onSelectMultiple;
    public Action<TreeNodeObject> onDeselect;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private ScrollRect scrollRect;

    public TreeNodeCtrl()
    {
      base.\u002Ector();
    }

    public TreeNodeObject selectNode
    {
      get
      {
        TreeNodeObject[] selectNodes = this.selectNodes;
        return selectNodes.Length != 0 ? selectNodes[0] : (TreeNodeObject) null;
      }
      set
      {
        this.SetSelectNode(value);
      }
    }

    public TreeNodeObject[] selectNodes
    {
      get
      {
        return this.hashSelectNode.ToArray<TreeNodeObject>();
      }
    }

    public ObjectCtrlInfo[] selectObjectCtrl
    {
      get
      {
        List<ObjectCtrlInfo> objectCtrlInfoList = new List<ObjectCtrlInfo>();
        foreach (TreeNodeObject key in this.hashSelectNode)
        {
          ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
          if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(key, out objectCtrlInfo))
            objectCtrlInfoList.Add(objectCtrlInfo);
        }
        return objectCtrlInfoList.ToArray();
      }
    }

    public TreeNodeObject AddNode(string _name, TreeNodeObject _parent = null)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.m_ObjectNode);
      gameObject.SetActive(true);
      gameObject.get_transform().SetParent(this.m_ObjectRoot.get_transform(), false);
      TreeNodeObject component = (TreeNodeObject) gameObject.GetComponent<TreeNodeObject>();
      component.textName = _name;
      if (Object.op_Implicit((Object) _parent))
        component.SetParent(_parent);
      else
        this.m_TreeNodeObject.Add(component);
      return component;
    }

    public bool AddNode(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null) || Object.op_Inequality((Object) _node.parent, (Object) null) || this.m_TreeNodeObject.Contains(_node))
        return false;
      this.m_TreeNodeObject.Add(_node);
      return true;
    }

    public void RemoveNode(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null) || Object.op_Equality((Object) _node.parent, (Object) null))
        return;
      this.m_TreeNodeObject.Remove(_node);
    }

    public bool CheckNode(TreeNodeObject _node)
    {
      return !Object.op_Equality((Object) _node, (Object) null) && this.m_TreeNodeObject.Contains(_node);
    }

    public void DeleteNode(TreeNodeObject _node)
    {
      if (!_node.enableDelete)
        return;
      _node.SetParent((TreeNodeObject) null);
      this.m_TreeNodeObject.Remove(_node);
      if (_node.onDelete != null)
        _node.onDelete();
      this.DeleteNodeLoop(_node);
      if (this.m_TreeNodeObject.Count != 0)
        return;
      this.scrollRect.set_verticalNormalizedPosition(1f);
    }

    public void DeleteAllNode()
    {
      int count = this.m_TreeNodeObject.Count;
      for (int index = 0; index < count; ++index)
        this.DeleteAllNodeLoop(this.m_TreeNodeObject[index]);
      this.m_TreeNodeObject.Clear();
      this.hashSelectNode.Clear();
      this.scrollRect.set_verticalNormalizedPosition(1f);
      this.scrollRect.set_horizontalNormalizedPosition(0.0f);
    }

    public TreeNodeObject GetNode(int _index)
    {
      int count = this.m_TreeNodeObject.Count;
      if (count == 0)
        return (TreeNodeObject) null;
      return _index >= 0 && _index < count ? this.m_TreeNodeObject[_index] : (TreeNodeObject) null;
    }

    public void SetParent(TreeNodeObject _node, TreeNodeObject _parent)
    {
      if (Object.op_Equality((Object) _node, (Object) null) || !_node.enableChangeParent || this.CheckNode(_node) && Object.op_Equality((Object) _parent, (Object) null) || !_node.SetParent(_parent))
        return;
      this.RefreshHierachy();
      ((MonoBehaviour) this.m_TreeRoot).Invoke("SetDirty", 0.0f);
      if (this.onParentage == null)
        return;
      this.onParentage(_parent, _node);
    }

    public void RefreshHierachy()
    {
      int count = this.m_TreeNodeObject.Count;
      for (int index = 0; index < count; ++index)
      {
        this.RefreshHierachyLoop(this.m_TreeNodeObject[index], 0, true);
        this.RefreshVisibleLoop(this.m_TreeNodeObject[index]);
      }
    }

    public void SetParent()
    {
      TreeNodeObject[] selectNodes = this.selectNodes;
      for (int index = 1; index < selectNodes.Length; ++index)
        this.SetParent(selectNodes[index], selectNodes[0]);
      this.SelectSingle((TreeNodeObject) null, true);
      Singleton<GuideObjectManager>.Instance.selectObject = (GuideObject) null;
    }

    public void RemoveNode()
    {
      foreach (TreeNodeObject selectNode in this.selectNodes)
        this.SetParent(selectNode, (TreeNodeObject) null);
      this.SelectSingle((TreeNodeObject) null, true);
    }

    public void DeleteNode()
    {
      foreach (TreeNodeObject selectNode in this.selectNodes)
        this.DeleteNode(selectNode);
      this.SelectSingle((TreeNodeObject) null, true);
    }

    public void CopyChangeAmount()
    {
      TreeNodeObject[] selectNodes = this.selectNodes;
      ObjectCtrlInfo objectCtrlInfo1 = (ObjectCtrlInfo) null;
      if (!Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(selectNodes[0], out objectCtrlInfo1))
        return;
      List<TreeNodeCommand.MoveCopyInfo> moveCopyInfoList = new List<TreeNodeCommand.MoveCopyInfo>();
      for (int index = 1; index < selectNodes.Length; ++index)
      {
        ObjectCtrlInfo objectCtrlInfo2 = (ObjectCtrlInfo) null;
        if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(selectNodes[index], out objectCtrlInfo2))
          moveCopyInfoList.Add(new TreeNodeCommand.MoveCopyInfo(objectCtrlInfo2.objectInfo.dicKey, objectCtrlInfo2.objectInfo.changeAmount, objectCtrlInfo1.objectInfo.changeAmount));
      }
      Singleton<UndoRedoManager>.Instance.Do((ICommand) new TreeNodeCommand.MoveCopyCommand(moveCopyInfoList.ToArray()));
      this.SelectSingle((TreeNodeObject) null, true);
    }

    public void SelectMultiple(TreeNodeObject _start, TreeNodeObject _end)
    {
      float y1 = (float) ((Transform) _start.rectNode).get_position().y;
      float y2 = (float) ((Transform) _end.rectNode).get_position().y;
      float _min = Mathf.Min(y1, y2);
      float _max = Mathf.Max(y1, y2);
      foreach (TreeNodeObject treeNodeObject in this.hashSelectNode)
        treeNodeObject.OnDeselect();
      this.hashSelectNode.Clear();
      this.AddSelectNode(_start, false);
      foreach (TreeNodeObject _source in this.m_TreeNodeObject)
        this.SelectMultipleLoop(_source, _min, _max);
      this.AddSelectNode(_end, true);
    }

    private void SelectMultipleLoop(TreeNodeObject _source, float _min, float _max)
    {
      if (Object.op_Equality((Object) _source, (Object) null))
        return;
      if (MathfEx.RangeEqualOff<float>(_min, (float) ((Transform) _source.rectNode).get_position().y, _max))
        this.AddSelectNode(_source, true);
      if (_source.treeState == TreeNodeObject.TreeState.Close)
        return;
      foreach (TreeNodeObject _source1 in _source.child)
        this.SelectMultipleLoop(_source1, _min, _max);
    }

    private void RefreshHierachyLoop(TreeNodeObject _source, int _indent, bool _visible)
    {
      ((Component) _source).get_transform().SetAsLastSibling();
      _source.treeNode.indent = _indent;
      if (((Component) _source).get_gameObject().get_activeSelf() != _visible)
        ((Component) _source).get_gameObject().SetActive(_visible);
      int childCount = _source.childCount;
      if (_visible)
        _visible = _source.treeState == TreeNodeObject.TreeState.Open;
      for (int index = 0; index < childCount; ++index)
        this.RefreshHierachyLoop(_source.child[index], _indent + 1, _visible);
    }

    private void RefreshVisibleLoop(TreeNodeObject _source)
    {
      if (!_source.visible)
      {
        _source.visible = _source.visible;
      }
      else
      {
        int childCount = _source.childCount;
        for (int index = 0; index < childCount; ++index)
          this.RefreshVisibleLoop(_source.child[index]);
      }
    }

    private void DeleteNodeLoop(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return;
      if (_node.onDelete != null)
        _node.onDelete();
      int childCount = _node.childCount;
      for (int index = 0; index < childCount; ++index)
        this.DeleteNodeLoop(_node.child[index]);
      Object.Destroy((Object) ((Component) _node).get_gameObject());
      if (this.onDelete == null)
        return;
      this.onDelete(_node);
    }

    private void DeleteAllNodeLoop(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return;
      int childCount = _node.childCount;
      for (int index = 0; index < childCount; ++index)
        this.DeleteAllNodeLoop(_node.child[index]);
      Object.DestroyImmediate((Object) ((Component) _node).get_gameObject());
    }

    private void SetSelectNode(TreeNodeObject _node)
    {
      bool flag = Input.GetKey((KeyCode) 306) | Input.GetKey((KeyCode) 305);
      TreeNodeCtrl.Calc calc = TreeNodeCtrl.Calc.None;
      if (Object.op_Implicit((Object) this.selectNode) && Input.GetKey((KeyCode) 112))
        calc = TreeNodeCtrl.Calc.Attach;
      else if (Input.GetKey((KeyCode) 111))
        calc = TreeNodeCtrl.Calc.Detach;
      else if (Input.GetKey((KeyCode) 120))
        calc = TreeNodeCtrl.Calc.Copy;
      switch (calc)
      {
        case TreeNodeCtrl.Calc.Attach:
          if (flag)
          {
            this.hashSelectNode.Add(_node);
            this.SetParent();
          }
          else
            this.SetParent(this.selectNode, _node);
          this.SelectSingle(_node, true);
          break;
        case TreeNodeCtrl.Calc.Detach:
          if (flag)
          {
            this.hashSelectNode.Add(_node);
            foreach (TreeNodeObject _node1 in this.hashSelectNode)
              this.SetParent(_node1, (TreeNodeObject) null);
          }
          else
            this.SetParent(_node, (TreeNodeObject) null);
          this.SelectSingle(_node, true);
          break;
        case TreeNodeCtrl.Calc.Copy:
          if (flag)
          {
            this.hashSelectNode.Add(_node);
            if (this.hashSelectNode.Count > 1)
              this.CopyChangeAmount();
          }
          this.SelectSingle(_node, true);
          break;
        default:
          if (flag)
          {
            this.AddSelectNode(_node, false);
            break;
          }
          this.SelectSingle(_node, true);
          break;
      }
    }

    public void SelectSingle(TreeNodeObject _node, bool _deselect = true)
    {
      ObjectCtrlInfo ctrlInfo = Studio.Studio.GetCtrlInfo(_node);
      bool flag = this.hashSelectNode.Count == 1 && ((this.hashSelectNode.Contains(_node) ? 1 : 0) & (ctrlInfo == null ? 1 : (ctrlInfo.guideObject.isActive ? 1 : 0))) != 0;
      foreach (TreeNodeObject treeNodeObject in this.hashSelectNode)
        treeNodeObject.OnDeselect();
      this.hashSelectNode.Clear();
      if (_deselect && flag)
        this.DeselectNode(_node);
      else
        this.AddSelectNode(_node, false);
    }

    private void AddSelectNode(TreeNodeObject _node, bool _multiple = false)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return;
      if (this.hashSelectNode.Add(_node))
      {
        if (this.onSelect != null && this.hashSelectNode.Count == 1)
          this.onSelect(_node);
        else if (this.onSelectMultiple != null && this.hashSelectNode.Count > 1)
          this.onSelectMultiple();
        _node.colorSelect = this.hashSelectNode.Count != 1 ? Utility.ConvertColor(94, 139, 100) : Utility.ConvertColor(91, 164, 82);
        ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
        if (_multiple)
          Singleton<GuideObjectManager>.Instance.AddSelectMultiple(!Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo) ? (GuideObject) null : objectCtrlInfo.guideObject);
        else
          Singleton<GuideObjectManager>.Instance.selectObject = !Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo) ? (GuideObject) null : objectCtrlInfo.guideObject;
      }
      else
        this.DeselectNode(_node);
    }

    private void DeselectNode(TreeNodeObject _node)
    {
      this.hashSelectNode.Remove(_node);
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      if (Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo))
        Singleton<GuideObjectManager>.Instance.deselectObject = objectCtrlInfo.guideObject;
      _node.OnDeselect();
      if (this.onDeselect == null)
        return;
      this.onDeselect(_node);
    }

    public bool CheckSelect(TreeNodeObject _node)
    {
      return this.hashSelectNode.Contains(_node);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      SortCanvas.select = this.canvas;
    }

    private void Start()
    {
      this.m_ObjectRoot.get_transform().set_localPosition(Vector3.get_zero());
    }

    private enum Calc
    {
      None,
      Attach,
      Detach,
      Delete,
      Copy,
    }
  }
}
