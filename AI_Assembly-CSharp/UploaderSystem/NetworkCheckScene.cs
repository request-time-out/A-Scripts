// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetworkCheckScene
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using Manager;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace UploaderSystem
{
  public class NetworkCheckScene : MonoBehaviour
  {
    public Text txtInfomation;
    public GameObject objClick;
    public GameObject objMsg;
    private CoroutineAssist caCheck;
    private int nextType;
    private bool changeScene;
    private float startTime;
    private bool maintenance;
    private bool isActiveUploader;

    public NetworkCheckScene()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      if (File.Exists(UserData.Path + "maintenance.dat"))
        this.maintenance = true;
      this.caCheck = new CoroutineAssist((MonoBehaviour) this, new Func<IEnumerator>(this.CheckNetworkStatus));
      this.caCheck.Start(true, 10f);
      this.startTime = Time.get_realtimeSinceStartup();
    }

    [DebuggerHidden]
    public IEnumerator CheckNetworkStatus()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetworkCheckScene.\u003CCheckNetworkStatus\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void Update()
    {
      if (this.nextType == -1)
      {
        int num = Mathf.FloorToInt(Time.get_realtimeSinceStartup() - this.startTime);
        string str = "サーバーをチェックしています";
        for (int index = 0; index < num; ++index)
          str += "．";
        if (Object.op_Implicit((Object) this.txtInfomation))
          this.txtInfomation.set_text(str);
      }
      if (this.caCheck != null && this.caCheck.status == CoroutineAssist.Status.Run && this.caCheck.TimeOutCheck())
      {
        this.caCheck.End();
        if (Object.op_Implicit((Object) this.txtInfomation))
          this.txtInfomation.set_text("サーバーへのアクセスに失敗しました。");
        this.caCheck.EndStatus();
        if (Object.op_Implicit((Object) this.objClick))
          this.objClick.SetActiveIfDifferent(true);
        if (Object.op_Implicit((Object) this.objMsg))
          this.objMsg.SetActiveIfDifferent(false);
        this.nextType = 2;
      }
      if (this.changeScene)
        return;
      if (this.nextType == 0)
      {
        if (!Input.get_anyKeyDown())
          return;
        this.nextType = 1;
      }
      else if (this.nextType == 1)
      {
        bool flag1 = false;
        bool flag2 = true;
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          isAdd = flag1,
          levelName = Singleton<GameSystem>.Instance.networkSceneName,
          isFade = flag2
        }, false);
        this.changeScene = true;
      }
      else
      {
        if (this.nextType != 2 || !Input.get_anyKeyDown())
          return;
        Singleton<Scene>.Instance.LoadReserve(new Scene.Data()
        {
          levelName = "Title",
          isFade = true
        }, false);
        this.changeScene = true;
      }
    }
  }
}
