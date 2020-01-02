// Decompiled with JetBrains decompiler
// Type: Housing.Command.ListDropCommand
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Battlehub.UIControls;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Housing.Command
{
  public class ListDropCommand : Housing.ICommand
  {
    private ListDropCommand.Info[] infos;
    private ObjectCtrl target;
    private ItemDropAction action;

    public ListDropCommand(ItemDropArgs _args)
    {
      this.target = (ObjectCtrl) _args.DropTarget;
      this.action = _args.Action;
      this.infos = ((IEnumerable<object>) _args.DragItems).Select<object, ListDropCommand.Info>((Func<object, ListDropCommand.Info>) (v => new ListDropCommand.Info((ObjectCtrl) v))).ToArray<ListDropCommand.Info>();
    }

    public void Do()
    {
      ObjectCtrl target = this.target;
      switch (this.action)
      {
        case ItemDropAction.SetLastChild:
          foreach (ListDropCommand.Info info in this.infos)
            info.ObjectCtrl.OnAttach(this.target, -1);
          break;
        case ItemDropAction.SetPrevSibling:
          foreach (ListDropCommand.Info info in this.infos)
          {
            ObjectCtrl objectCtrl = info.ObjectCtrl;
            if (objectCtrl.Parent != target.Parent)
              objectCtrl.OnAttach(target.Parent, -1);
            List<IObjectInfo> objectInfoList = this.ChildrenOf(target.Parent);
            int num1 = objectInfoList.IndexOf(target.ObjectInfo);
            int num2 = objectInfoList.IndexOf(objectCtrl.ObjectInfo);
            objectInfoList.Remove(objectCtrl.ObjectInfo);
            objectInfoList.Insert(num1 <= num2 ? num1 : num1 - 1, objectCtrl.ObjectInfo);
          }
          break;
        case ItemDropAction.SetNextSibling:
          for (int index = this.infos.Length - 1; index >= 0; --index)
          {
            ObjectCtrl objectCtrl = this.infos[index].ObjectCtrl;
            List<IObjectInfo> objectInfoList = this.ChildrenOf(target.Parent);
            int num1 = objectInfoList.IndexOf(target.ObjectInfo);
            if (objectCtrl.Parent != target.Parent)
            {
              objectCtrl.OnAttach(target.Parent, num1 + 1);
            }
            else
            {
              int num2 = objectInfoList.IndexOf(objectCtrl.ObjectInfo);
              objectInfoList.Remove(objectCtrl.ObjectInfo);
              objectInfoList.Insert(num1 + (num1 >= num2 ? 0 : 1), objectCtrl.ObjectInfo);
            }
          }
          break;
      }
    }

    public void Redo()
    {
      this.Do();
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.UpdateUI();
    }

    public void Undo()
    {
      switch (this.action)
      {
        case ItemDropAction.SetLastChild:
          foreach (ListDropCommand.Info info in this.infos)
            info.ObjectCtrl.OnAttach(info.Parent, info.Index);
          break;
        case ItemDropAction.SetPrevSibling:
          foreach (ListDropCommand.Info info in this.infos)
          {
            if (info.ObjectCtrl.Parent != info.Parent)
            {
              info.ObjectCtrl.OnAttach(info.Parent, info.Index);
            }
            else
            {
              List<IObjectInfo> objectInfoList = this.ChildrenOf(info.Parent);
              objectInfoList.Remove(info.ObjectCtrl.ObjectInfo);
              objectInfoList.Insert(info.Index, info.ObjectCtrl.ObjectInfo);
            }
          }
          break;
        case ItemDropAction.SetNextSibling:
          foreach (ListDropCommand.Info info in this.infos)
          {
            if (info.ObjectCtrl.Parent != info.Parent)
            {
              info.ObjectCtrl.OnAttach(info.Parent, info.Index);
            }
            else
            {
              List<IObjectInfo> objectInfoList = this.ChildrenOf(info.Parent);
              objectInfoList.Remove(info.ObjectCtrl.ObjectInfo);
              objectInfoList.Insert(info.Index, info.ObjectCtrl.ObjectInfo);
            }
          }
          break;
      }
      Singleton<CraftScene>.Instance.UICtrl.ListUICtrl.UpdateUI();
    }

    private List<IObjectInfo> ChildrenOf(ObjectCtrl parent)
    {
      if (parent is OCFolder ocFolder)
        return ocFolder.OIFolder.Child;
      return parent != null ? parent.CraftInfo.ObjectInfos : Singleton<Manager.Housing>.Instance.CraftInfo.ObjectInfos;
    }

    private class Info
    {
      public Info(ObjectCtrl _objectCtrl)
      {
        this.ObjectCtrl = _objectCtrl;
        this.Parent = this.ObjectCtrl.Parent;
        this.Index = this.ObjectCtrl.InfoIndex;
      }

      public ObjectCtrl ObjectCtrl { get; private set; }

      public ObjectCtrl Parent { get; private set; }

      public int Index { get; private set; }
    }
  }
}
