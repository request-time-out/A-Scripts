// Decompiled with JetBrains decompiler
// Type: Studio.ManipulatePanelCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Studio
{
  public class ManipulatePanelCtrl : MonoBehaviour
  {
    [SerializeField]
    private ManipulatePanelCtrl.CharaPanelInfo charaPanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.ItemPanelInfo itemPanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.LightPanelInfo lightPanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.FolderPanelInfo folderPanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.RoutePanelInfo routePanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.CameraPanelInfo cameraPanelInfo;
    [SerializeField]
    private ManipulatePanelCtrl.RoutePointPanelInfo routePointPanelInfo;
    private int[] kinds;

    public ManipulatePanelCtrl()
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
        ((Component) this).get_gameObject().SetActive(value);
        if (((Component) this).get_gameObject().get_activeSelf())
          this.SetActive();
        else
          this.Deactivate();
      }
    }

    private ManipulatePanelCtrl.RootInfo[] rootPanel { get; set; }

    public ObjectCtrlInfo objectCtrlInfo
    {
      set
      {
        int[] numArray;
        if (value != null)
          numArray = value.kinds;
        else
          numArray = new int[1]{ -1 };
        this.kinds = numArray;
        this.charaPanelInfo.mpCharCtrl.ociChar = value as OCIChar;
        this.itemPanelInfo.mpItemCtrl.ociItem = value as OCIItem;
        this.lightPanelInfo.mpLightCtrl.ociLight = value as OCILight;
        this.folderPanelInfo.mpFolderCtrl.ociFolder = value as OCIFolder;
        this.routePanelInfo.mpRouteCtrl.ociRoute = value as OCIRoute;
        this.cameraPanelInfo.mpCameraCtrl.ociCamera = value as OCICamera;
        this.routePointPanelInfo.mpRoutePointCtrl.ociRoutePoint = value as OCIRoutePoint;
      }
    }

    public void OnSelect(TreeNodeObject _node)
    {
      Singleton<Studio.Studio>.Instance.colorPalette.visible = false;
      ObjectCtrlInfo loop = this.TryGetLoop(_node);
      int[] numArray;
      if (loop != null)
        numArray = loop.kinds;
      else
        numArray = new int[1]{ -1 };
      this.kinds = numArray;
      for (int index = 0; index < this.kinds.Length; ++index)
      {
        switch (this.kinds[index])
        {
          case 0:
            this.charaPanelInfo.mpCharCtrl.ociChar = loop[index] as OCIChar;
            break;
          case 1:
            this.itemPanelInfo.mpItemCtrl.ociItem = loop[index] as OCIItem;
            break;
          case 2:
            this.lightPanelInfo.mpLightCtrl.ociLight = loop[index] as OCILight;
            break;
          case 3:
            this.folderPanelInfo.mpFolderCtrl.ociFolder = loop[index] as OCIFolder;
            break;
          case 4:
            this.routePanelInfo.mpRouteCtrl.ociRoute = loop[index] as OCIRoute;
            break;
          case 5:
            this.cameraPanelInfo.mpCameraCtrl.ociCamera = loop[index] as OCICamera;
            break;
          case 6:
            this.routePointPanelInfo.mpRoutePointCtrl.ociRoutePoint = loop[index] as OCIRoutePoint;
            break;
        }
      }
      if (!this.active)
        return;
      this.SetActive();
    }

    public void OnDeselect(TreeNodeObject _node)
    {
      ObjectCtrlInfo loop = this.TryGetLoop(_node);
      switch (loop != null ? loop.kind : -1)
      {
        case 0:
          this.charaPanelInfo.mpCharCtrl.Deselect(loop as OCIChar);
          break;
        case 1:
          this.itemPanelInfo.mpItemCtrl.Deselect(loop as OCIItem);
          break;
        case 2:
          this.lightPanelInfo.mpLightCtrl.Deselect(loop as OCILight);
          break;
        case 3:
          this.folderPanelInfo.mpFolderCtrl.Deselect(loop as OCIFolder);
          break;
        case 4:
          this.routePanelInfo.mpRouteCtrl.Deselect(loop as OCIRoute);
          break;
        case 5:
          this.cameraPanelInfo.mpCameraCtrl.Deselect(loop as OCICamera);
          break;
        case 6:
          this.routePointPanelInfo.mpRoutePointCtrl.Deselect(loop as OCIRoutePoint);
          break;
      }
    }

    public void UpdateInfo(int _kind)
    {
      if (_kind != 1)
      {
        if (_kind != 5)
          return;
        this.cameraPanelInfo.mpCameraCtrl.UpdateInfo();
      }
      else
        this.itemPanelInfo.mpItemCtrl.UpdateInfo();
    }

    private void SetActive()
    {
      for (int index = 0; index < this.rootPanel.Length; ++index)
        this.rootPanel[index].active = ((IEnumerable<int>) this.kinds).Contains<int>(index);
    }

    private void Deactivate()
    {
      for (int index = 0; index < this.rootPanel.Length; ++index)
        this.rootPanel[index].active = false;
    }

    public void OnDelete(TreeNodeObject _node)
    {
      this.kinds = new int[1]{ -1 };
      this.SetActive();
    }

    private ObjectCtrlInfo TryGetLoop(TreeNodeObject _node)
    {
      if (Object.op_Equality((Object) _node, (Object) null))
        return (ObjectCtrlInfo) null;
      ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
      return Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(_node, out objectCtrlInfo) ? objectCtrlInfo : this.TryGetLoop(_node.parent);
    }

    private void Awake()
    {
      this.rootPanel = new ManipulatePanelCtrl.RootInfo[7]
      {
        (ManipulatePanelCtrl.RootInfo) this.charaPanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.itemPanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.lightPanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.folderPanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.routePanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.cameraPanelInfo,
        (ManipulatePanelCtrl.RootInfo) this.routePointPanelInfo
      };
      this.kinds = new int[1]{ -1 };
      this.SetActive();
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.onSelect += new Action<TreeNodeObject>(this.OnSelect);
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.onDelete += new Action<TreeNodeObject>(this.OnDelete);
      Singleton<Studio.Studio>.Instance.treeNodeCtrl.onDeselect += new Action<TreeNodeObject>(this.OnDeselect);
    }

    [Serializable]
    private class RootInfo
    {
      public GameObject root;

      public virtual bool active
      {
        set
        {
          if (this.root.get_activeSelf() == value)
            return;
          this.root.SetActive(value);
        }
      }
    }

    [Serializable]
    private class CharaPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPCharCtrl m_MPCharCtrl;

      public MPCharCtrl mpCharCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPCharCtrl, (Object) null))
            this.m_MPCharCtrl = (MPCharCtrl) this.root.GetComponent<MPCharCtrl>();
          return this.m_MPCharCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpCharCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class ItemPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPItemCtrl m_MPItemCtrl;

      public MPItemCtrl mpItemCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPItemCtrl, (Object) null))
            this.m_MPItemCtrl = (MPItemCtrl) this.root.GetComponent<MPItemCtrl>();
          return this.m_MPItemCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpItemCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class LightPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPLightCtrl m_MPLightCtrl;

      public MPLightCtrl mpLightCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPLightCtrl, (Object) null))
            this.m_MPLightCtrl = (MPLightCtrl) this.root.GetComponent<MPLightCtrl>();
          return this.m_MPLightCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpLightCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class FolderPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPFolderCtrl m_MPFolderCtrl;

      public MPFolderCtrl mpFolderCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPFolderCtrl, (Object) null))
            this.m_MPFolderCtrl = (MPFolderCtrl) this.root.GetComponent<MPFolderCtrl>();
          return this.m_MPFolderCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpFolderCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class RoutePanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPRouteCtrl m_MPRouteCtrl;

      public MPRouteCtrl mpRouteCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPRouteCtrl, (Object) null))
            this.m_MPRouteCtrl = (MPRouteCtrl) this.root.GetComponent<MPRouteCtrl>();
          return this.m_MPRouteCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpRouteCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class CameraPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPCameraCtrl m_MPCameraCtrl;

      public MPCameraCtrl mpCameraCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPCameraCtrl, (Object) null))
            this.m_MPCameraCtrl = (MPCameraCtrl) this.root.GetComponent<MPCameraCtrl>();
          return this.m_MPCameraCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpCameraCtrl.active = value;
        }
      }
    }

    [Serializable]
    private class RoutePointPanelInfo : ManipulatePanelCtrl.RootInfo
    {
      private MPRoutePointCtrl m_MPRoutePointCtrl;

      public MPRoutePointCtrl mpRoutePointCtrl
      {
        get
        {
          if (Object.op_Equality((Object) this.m_MPRoutePointCtrl, (Object) null))
            this.m_MPRoutePointCtrl = (MPRoutePointCtrl) this.root.GetComponent<MPRoutePointCtrl>();
          return this.m_MPRoutePointCtrl;
        }
      }

      public override bool active
      {
        set
        {
          this.mpRoutePointCtrl.active = value;
        }
      }
    }
  }
}
