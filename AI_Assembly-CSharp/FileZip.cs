// Decompiled with JetBrains decompiler
// Type: FileZip
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Ionic.Zip;
using System;
using System.Collections;
using System.Diagnostics;

public class FileZip
{
  private ZipAssist zipAssist = new ZipAssist();

  public void ZipSaveProgress(object sender, SaveProgressEventArgs e)
  {
    if (((ZipProgressEventArgs) e).get_EventType() != 9)
      ;
  }

  [DebuggerHidden]
  public IEnumerator FileZipCor(
    IObserver<byte[]> observer,
    byte[] srcBytes,
    string entryName)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new FileZip.\u003CFileZipCor\u003Ec__Iterator0()
    {
      srcBytes = srcBytes,
      entryName = entryName,
      observer = observer,
      \u0024this = this
    };
  }

  [DebuggerHidden]
  public IEnumerator FileUnzipCor(IObserver<byte[]> observer, byte[] srcBytes)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new FileZip.\u003CFileUnzipCor\u003Ec__Iterator1()
    {
      srcBytes = srcBytes,
      observer = observer,
      \u0024this = this
    };
  }
}
