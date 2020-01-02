// Decompiled with JetBrains decompiler
// Type: UploaderSystem.NetPhpControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace UploaderSystem
{
  public class NetPhpControl : MonoBehaviour
  {
    public Canvas cvsChangeScene;

    public NetPhpControl()
    {
      base.\u002Ector();
    }

    protected NetworkInfo netInfo
    {
      get
      {
        return Singleton<NetworkInfo>.Instance;
      }
    }

    protected NetCacheControl cacheCtrl
    {
      get
      {
        return Singleton<NetworkInfo>.IsInstance() ? this.netInfo.cacheCtrl : (NetCacheControl) null;
      }
    }

    protected LogView logview
    {
      get
      {
        return this.netInfo.logview;
      }
    }

    protected Net_PopupMsg popupMsg
    {
      get
      {
        return this.netInfo.popupMsg;
      }
    }

    protected Net_PopupCheck popupCheck
    {
      get
      {
        return this.netInfo.popupCheck;
      }
    }

    public NetworkInfo.Profile profile
    {
      get
      {
        return this.netInfo.profile;
      }
    }

    public Dictionary<int, NetworkInfo.UserInfo> dictUserInfo
    {
      get
      {
        return this.netInfo.dictUserInfo;
      }
    }

    public List<NetworkInfo.CharaInfo> lstCharaInfo
    {
      get
      {
        return this.netInfo.lstCharaInfo;
      }
    }

    public List<NetworkInfo.HousingInfo> lstHousingInfo
    {
      get
      {
        return this.netInfo.lstHousingInfo;
      }
    }

    protected string EncodeFromBase64(string buf)
    {
      return Encoding.UTF8.GetString(Convert.FromBase64String(buf));
    }

    [DebuggerHidden]
    public IEnumerator GetBaseInfo(bool upload)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CGetBaseInfo\u003Ec__Iterator0()
      {
        upload = upload,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UpdateBaseInfo(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CUpdateBaseInfo\u003Ec__Iterator1()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator GetNewestVersion(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CGetNewestVersion\u003Ec__Iterator2()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UpdateUserInfo(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CUpdateUserInfo\u003Ec__Iterator3()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator UpdateHandleName(IObserver<string> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CUpdateHandleName\u003Ec__Iterator4()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator GetAllUsers(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CGetAllUsers\u003Ec__Iterator5()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator GetAllCharaInfo(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CGetAllCharaInfo\u003Ec__Iterator6()
      {
        observer = observer,
        \u0024this = this
      };
    }

    [DebuggerHidden]
    public IEnumerator GetAllSceneInfo(IObserver<bool> observer)
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new NetPhpControl.\u003CGetAllSceneInfo\u003Ec__Iterator7()
      {
        observer = observer,
        \u0024this = this
      };
    }

    private void Start()
    {
    }

    private void Update()
    {
    }
  }
}
