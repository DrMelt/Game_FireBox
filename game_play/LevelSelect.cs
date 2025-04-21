using Godot;
using System;

public partial class LevelSelect : Control
{
	[Export]
	GameLogic gameLogicRef = null;
	public override void _Ready()
	{
		Visible = false;
        Godot.Collections.Array<Node> children = GetChildren();
		foreach (Node child in children)
		{
			if (child is Button_LevelLoad)
			{
				Button_LevelLoad button_LevelLoad = (Button_LevelLoad)child;
				button_LevelLoad.OnLevelLoadPressedEvent += OnLevelSelectButtonPressed;
			}
		}


	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Setting"))
		{
			Visible = !Visible;
		}
	}

	public void OnLevelSelectButtonPressed(Button_LevelLoad button_LevelLoad)
	{
		gameLogicRef.LoadLevel(button_LevelLoad.ELevel);
		Visible = false;
	}
}
