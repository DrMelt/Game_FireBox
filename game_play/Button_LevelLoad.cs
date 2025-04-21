using Godot;
using System;

public partial class Button_LevelLoad : Button
{
	public delegate void LevelLoadPressedDelegate(Button_LevelLoad button_LevelLoad);
	public event LevelLoadPressedDelegate OnLevelLoadPressedEvent;


	[Export]
	ELevelLoad eLevel = ELevelLoad.None;
	public ELevelLoad ELevel => eLevel;

	public override void _Ready()
	{
		Pressed += () => { OnLevelLoadPressedEvent.Invoke(this); };
	}

}
