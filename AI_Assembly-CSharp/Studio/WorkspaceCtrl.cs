// Decompiled with JetBrains decompiler
// Type: Studio.WorkspaceCtrl
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
  public class WorkspaceCtrl : MonoBehaviour
  {
    [SerializeField]
    private Button buttonClose;
    [SerializeField]
    private Button buttonRemove;
    [SerializeField]
    private Button buttonParent;
    [SerializeField]
    private TreeNodeCtrl treeNodeCtrl;
    [SerializeField]
    private Button buttonDelete;
    [SerializeField]
    private Button buttonCopy;
    [SerializeField]
    private Button buttonDuplicate;
    [SerializeField]
    private Button buttonFolder;
    [SerializeField]
    private Button buttonCamera;
    [SerializeField]
    private Button buttonRoute;
    [SerializeField]
    private StudioScene studioScene;

    public WorkspaceCtrl()
    {
      base.\u002Ector();
    }

    private Button[] buttons { get; set; }

    private void OnClickClose()
    {
      this.studioScene.OnClickDraw(1);
    }

    private void OnClickRemove()
    {
      this.treeNodeCtrl.RemoveNode();
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    private void OnClickParent()
    {
      this.treeNodeCtrl.SetParent();
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    public void OnClickDelete()
    {
      this.treeNodeCtrl.DeleteNode();
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
      Singleton<UndoRedoManager>.Instance.Clear();
    }

    private void OnClickCopy()
    {
      this.treeNodeCtrl.CopyChangeAmount();
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    private void OnClickDuplicate()
    {
      Singleton<Studio.Studio>.Instance.Duplicate();
    }

    private void OnClickFolder()
    {
      Singleton<Studio.Studio>.Instance.AddFolder();
    }

    private void OnClickCamera()
    {
      Singleton<Studio.Studio>.Instance.AddCamera();
    }

    private void OnClickRoute()
    {
      Singleton<Studio.Studio>.Instance.AddRoute();
    }

    public void UpdateUI()
    {
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    public void OnParentage(TreeNodeObject _parent, TreeNodeObject _child)
    {
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    public void OnDeleteNode(TreeNodeObject _node)
    {
      for (int index = 0; index < this.buttons.Length; ++index)
        ((Selectable) this.buttons[index]).set_interactable(false);
    }

    public void OnSelectSingle(TreeNodeObject _node)
    {
      ((Selectable) this.buttonParent).set_interactable(false);
      ((Selectable) this.buttonRemove).set_interactable(_node.isParent);
      ((Selectable) this.buttonDelete).set_interactable(_node.enableDelete);
      ((Selectable) this.buttonCopy).set_interactable(false);
      ((Selectable) this.buttonDuplicate).set_interactable(_node.enableCopy);
    }

    public void OnSelectMultiple()
    {
      TreeNodeObject[] selectNodes = this.treeNodeCtrl.selectNodes;
      if (((IList<TreeNodeObject>) selectNodes).IsNullOrEmpty<TreeNodeObject>())
        return;
      ((Selectable) this.buttonParent).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableChangeParent)));
      ((Selectable) this.buttonRemove).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.isParent)));
      ((Selectable) this.buttonDelete).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableDelete)));
      ((Selectable) this.buttonCopy).set_interactable(selectNodes[0].enableCopy && ((IEnumerable<TreeNodeObject>) selectNodes).Count<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableCopy)) > 1);
      ((Selectable) this.buttonDuplicate).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableCopy)));
    }

    public void OnDeselectSingle(TreeNodeObject _node)
    {
      TreeNodeObject[] selectNodes = this.treeNodeCtrl.selectNodes;
      if (((IList<TreeNodeObject>) selectNodes).IsNullOrEmpty<TreeNodeObject>())
      {
        for (int index = 0; index < this.buttons.Length; ++index)
          ((Selectable) this.buttons[index]).set_interactable(false);
      }
      else
      {
        ((Selectable) this.buttonParent).set_interactable(selectNodes.Length > 1 && ((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableChangeParent)));
        ((Selectable) this.buttonRemove).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.isParent)));
        ((Selectable) this.buttonDelete).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableDelete)));
        ((Selectable) this.buttonCopy).set_interactable(selectNodes.Length > 1 && (selectNodes[0].enableCopy && ((IEnumerable<TreeNodeObject>) selectNodes).Count<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableCopy)) > 1));
        ((Selectable) this.buttonDuplicate).set_interactable(((IEnumerable<TreeNodeObject>) selectNodes).Any<TreeNodeObject>((Func<TreeNodeObject, bool>) (v => v.enableCopy)));
      }
    }

    private void Awake()
    {
      // ISSUE: method pointer
      ((UnityEvent) this.buttonClose.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickClose)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonRemove.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRemove)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonParent.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickParent)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonDelete.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDelete)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonCopy.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickCopy)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonDuplicate.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickDuplicate)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonFolder.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickFolder)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonCamera.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickCamera)));
      // ISSUE: method pointer
      ((UnityEvent) this.buttonRoute.get_onClick()).AddListener(new UnityAction((object) this, __methodptr(OnClickRoute)));
      this.treeNodeCtrl.onParentage += new Action<TreeNodeObject, TreeNodeObject>(this.OnParentage);
      this.treeNodeCtrl.onDelete += new Action<TreeNodeObject>(this.OnDeleteNode);
      this.treeNodeCtrl.onSelect += new Action<TreeNodeObject>(this.OnSelectSingle);
      this.treeNodeCtrl.onSelectMultiple += new Action(this.OnSelectMultiple);
      this.treeNodeCtrl.onDeselect += new Action<TreeNodeObject>(this.OnDeselectSingle);
      this.buttons = new Button[5]
      {
        this.buttonRemove,
        this.buttonParent,
        this.buttonDelete,
        this.buttonCopy,
        this.buttonDuplicate
      };
    }
  }
}
