using System;
using System.Collections.Generic;
using FairyGUI;
using UnityEngine;
using DG.Tweening;

public class BagWindow : Window
{
	GList _list;

	public BagWindow()
	{
	}

	protected override void OnInit()
	{
		this.contentPane = UIPackage.CreateObject("Bag", "BagWin").asCom;
		this.Center();
		this.modal = true;

		_list = this.contentPane.GetChild("list").asList;
		_list.onClickItem.Add(__clickItem);
		_list.itemRenderer = RenderListItem;
        // 设置30的时候生成了GButton对象
        _list.numItems = 30;

        //GButton._iconObject 是一个继承了GLoader的资源加载对象，这里修改icon 等于 修改url 开始资源加载
        GButton button = (GButton)_list.GetFromPool(null);// 在做UI的时候有默认的item,所以这里直接传入null
        Debug.Log("开始设置了i9 图标");
        button.icon = "i" + 9;
        Debug.Log("设置了i9 图标");
        button.title = "" + 101;
        _list.AddChild(button);
        
    }

	void RenderListItem(int index, GObject obj)
	{
		GButton button = (GButton)obj;
		button.icon = "i" + UnityEngine.Random.Range(0, 10);
		button.title = "" + UnityEngine.Random.Range(0, 100);
	}

	override protected void DoShowAnimation()
	{
		this.SetScale(0.1f, 0.1f);
		this.SetPivot(0.5f, 0.5f);
		this.TweenScale(new Vector2(1, 1), 0.3f).SetEase(Ease.OutQuad).OnComplete(this.OnShown);
	}

	override protected void DoHideAnimation()
	{
		this.TweenScale(new Vector2(0.1f, 0.1f), 0.3f).SetEase(Ease.OutQuad).OnComplete(this.HideImmediately);
	}

    void __clickItem(EventContext context)
    {
        GButton item = (GButton)context.data;
        this.contentPane.GetChild("n11").asLoader.url = item.icon;
        this.contentPane.GetChild("n13").text = item.icon;
        Debug.Log("-------点击物品:" + item.icon + ",数量:" + item.title);
    }
}
