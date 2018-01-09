using UnityEngine;
using FairyGUI;
using System.Collections;
using System;

/// <summary>
/// A game bag demo, demonstrated how to customize loader to load icons not in the UI package.
/// </summary>
public class BagMain : MonoBehaviour
{
    GComponent _mainView;
    BagWindow _bagWindow;

    void Awake()
    {
        //Register custom loader class
        UIObjectFactory.SetLoaderExtension(typeof(MyGLoader));
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        Stage.inst.onKeyDown.Add(OnKeyDown);
        GRoot.inst.SetContentScaleFactor(1136, 640);
        _mainView = this.GetComponent<UIPanel>().ui;

        _bagWindow = new BagWindow();
        _mainView.GetChild("bagBtn").onClick.Add(() => { _bagWindow.Show(); });

        //Debug.Log("开始！！");
        //StartCoroutine(Init());
        //Debug.Log("完成！！");

        IEnumerator e = YieldSomeStuff();
        /* 
            第一次循环 返回hello，第二次循环 打印了 foo！且返回 world 然后才跳出循环
            如果没有第一次 e.MoveNext() 就不会返回hello，没有第二次 不会执行 打印foo!和返回world
        */
        while (e.MoveNext())
        {
            Debug.Log(e.Current + DateTime.Now.Millisecond.ToString());
        }
        Debug.Log("123");

    }

    IEnumerator YieldSomeStuff()
    {
        yield return "hello";

        Debug.Log("foo!" + DateTime.Now.Millisecond.ToString());
        yield return "world";
    }


    IEnumerator Init()
    {
        Debug.Log("init---");
        yield return StartCoroutine(init1());
        Debug.Log("init1 finish" + DateTime.Now.ToString("hh:mm:ss"));
        yield return StartCoroutine(init2());
        Debug.Log("init2 finish" + DateTime.Now.ToString("hh:mm:ss"));
        yield return StartCoroutine(init3());
        Debug.Log("init3 finish" + DateTime.Now.ToString("hh:mm:ss"));
    }

    IEnumerator init1()
    {
        // 模拟初始化
        Debug.Log("init1---" + DateTime.Now.ToString("hh:mm:ss"));
        yield return new WaitForSeconds(2);//
        Debug.Log("init1---成功 " + DateTime.Now.ToString("hh:mm:ss"));
    }
    IEnumerator init2()
    {
        // do somthing..
        Debug.Log("init2---" + DateTime.Now.ToString("hh:mm:ss"));
        yield return null;//new WaitForSeconds(2)
        Debug.Log("init2---成功" + DateTime.Now.ToString("hh:mm:ss"));
    }
    IEnumerator init3()
    {
        // do somthing..
        Debug.Log("init3---" + DateTime.Now.ToString("hh:mm:ss"));
        yield return null;//new WaitForSeconds(2)
        Debug.Log("init3---成功" + DateTime.Now.ToString("hh:mm:ss"));
    }



    void OnClick()
    {
        _bagWindow.Show();
    }

    void OnKeyDown(EventContext context)
    {
        if (context.inputEvent.keyCode == KeyCode.Escape)
        {
            Application.Quit();
        }
    }
}