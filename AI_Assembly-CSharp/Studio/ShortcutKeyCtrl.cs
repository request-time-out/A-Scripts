// Decompiled with JetBrains decompiler
// Type: Studio.ShortcutKeyCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Manager;
using System.Collections.Generic;
using UnityEngine;

namespace Studio
{
  public class ShortcutKeyCtrl : MonoBehaviour
  {
    [SerializeField]
    private StudioScene studioScene;
    [SerializeField]
    private SystemButtonCtrl systemButtonCtrl;
    [SerializeField]
    private WorkspaceCtrl workspaceCtrl;
    [SerializeField]
    private CameraControl cameraControl;
    [SerializeField]
    private TreeNodeCtrl treeNodeCtrl;
    [SerializeField]
    private GameScreenShot gameScreenShot;
    [SerializeField]
    private Sprite[] sprites;
    private readonly KeyCode[] cameraKey;

    public ShortcutKeyCtrl()
    {
      // ISSUE: unable to decompile the method.
    }

    private void Notification(int _id)
    {
      NotificationScene.spriteMessage = this.sprites[_id];
      NotificationScene.waitTime = 2f;
      NotificationScene.width = 416f;
      NotificationScene.height = 160f;
      Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
      {
        levelName = "StudioNotification",
        isAdd = true
      }, false);
    }

    private void Update()
    {
      if (!Singleton<Studio.Studio>.IsInstance() || Singleton<Studio.Studio>.Instance.isInputNow || (!Singleton<Scene>.IsInstance() || Singleton<Scene>.Instance.AddSceneName != string.Empty))
        return;
      bool flag1 = Input.GetKey((KeyCode) 306) | Input.GetKey((KeyCode) 305);
      if ((Input.GetKey((KeyCode) 304) | Input.GetKey((KeyCode) 303)) & Input.GetKeyDown((KeyCode) 122))
      {
        if (!Singleton<UndoRedoManager>.Instance.CanRedo)
          return;
        Singleton<UndoRedoManager>.Instance.Redo();
      }
      else if (Input.GetKeyDown((KeyCode) 122))
      {
        if (!Singleton<UndoRedoManager>.Instance.CanUndo)
          return;
        Singleton<UndoRedoManager>.Instance.Undo();
      }
      else if (Input.GetKeyDown((KeyCode) 102))
      {
        TreeNodeObject selectNode = this.treeNodeCtrl.selectNode;
        if (Object.op_Equality((Object) selectNode, (Object) null))
          return;
        ObjectCtrlInfo objectCtrlInfo = (ObjectCtrlInfo) null;
        if (!Singleton<Studio.Studio>.Instance.dicInfo.TryGetValue(selectNode, out objectCtrlInfo))
          return;
        this.cameraControl.targetPos = ((Component) objectCtrlInfo.guideObject).get_transform().get_position();
      }
      else if (Input.GetKeyDown((KeyCode) 99))
      {
        GuideObject[] selectObjects = Singleton<GuideObjectManager>.Instance.selectObjects;
        if (((IList<GuideObject>) selectObjects).IsNullOrEmpty<GuideObject>())
          return;
        List<GuideCommand.EqualsInfo> self = new List<GuideCommand.EqualsInfo>();
        foreach (GuideObject guideObject in selectObjects)
        {
          if (guideObject.enablePos)
            self.Add(guideObject.SetWorldPos(this.cameraControl.targetPos));
        }
        if (self.IsNullOrEmpty<GuideCommand.EqualsInfo>())
          return;
        Singleton<UndoRedoManager>.Instance.Push((ICommand) new GuideCommand.MoveEqualsCommand(self.ToArray()));
      }
      else if (flag1 && Input.GetKeyDown((KeyCode) 115))
        this.systemButtonCtrl.OnClickSave();
      else if (flag1 && Input.GetKeyDown((KeyCode) 100))
        Singleton<Studio.Studio>.Instance.Duplicate();
      else if (Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue))
        this.workspaceCtrl.OnClickDelete();
      else if (Input.GetKeyDown((KeyCode) 119))
        Singleton<GuideObjectManager>.Instance.mode = 0;
      else if (Input.GetKeyDown((KeyCode) 101))
        Singleton<GuideObjectManager>.Instance.mode = 1;
      else if (Input.GetKeyDown((KeyCode) 114))
        Singleton<GuideObjectManager>.Instance.mode = 2;
      else if (Input.GetKeyDown((KeyCode) 113))
        this.studioScene.OnClickAxis();
      else if (Input.GetKeyDown((KeyCode) 106))
        this.studioScene.OnClickAxisTrans();
      else if (Input.GetKeyDown((KeyCode) 107))
        this.studioScene.OnClickAxisCenter();
      else if (Input.GetKeyDown((KeyCode) 292))
        this.gameScreenShot.Capture(string.Empty);
      else if (Input.GetKeyDown((KeyCode) 283))
      {
        if (Singleton<GameCursor>.IsInstance())
          Singleton<GameCursor>.Instance.SetCursorLock(false);
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "StudioShortcutMenu",
          isAdd = true
        }, false);
      }
      else if (Input.GetKeyDown((KeyCode) 27))
      {
        if (Singleton<GameCursor>.IsInstance())
          Singleton<GameCursor>.Instance.SetCursorLock(false);
        Singleton<Scene>.Instance.GameEnd(true);
      }
      else
      {
        bool flag2 = false;
        for (int _no = 0; _no < 10; ++_no)
        {
          if (Input.GetKeyDown((KeyCode) (int) this.cameraKey[_no]))
          {
            this.studioScene.OnClickLoadCamera(_no);
            flag2 = true;
            break;
          }
        }
        if (flag2 || !Input.GetKeyDown((KeyCode) 104))
          return;
        Singleton<Studio.Studio>.Instance.cameraSelector.NextCamera();
      }
    }
  }
}
