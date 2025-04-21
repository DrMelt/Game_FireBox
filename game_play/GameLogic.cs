using Godot;
using System;
using GameKernel;
using System.Collections.Generic;


public partial class GameLogic : Node
{
	public delegate void OnLoadedLevelDelegate();
	public event OnLoadedLevelDelegate OnLoadedLevelEvent = null;
	public delegate void OnGameSteppedDelegate();
	public event OnGameSteppedDelegate OnGameSteppedEvent = null;

	FireBoxGame fireBoxGame = null;

	public FireBoxGame FireBoxGameRef { get => fireBoxGame; }

	Dictionary<ELevelLoad, PackedScene> levelSceneDict = new Dictionary<ELevelLoad, PackedScene>();

	ELevelLoad currentLevel = ELevelLoad.None;
	public ELevelLoad CurrentLevel => currentLevel;

	[Export]
	PackedScene levelScene = null;


	public override void _Ready()
	{
        Godot.Collections.Array<Node> children = GetChildren();
		foreach (Node child in children)
		{
			if (child is LevelLoadSetting)
			{
				LevelLoadSetting levelLoadSetting = (LevelLoadSetting)child;
				levelSceneDict.Add(levelLoadSetting.ELevel, levelLoadSetting.LevelScene);
			}
		}
	}

	public void LoadLevel(ELevelLoad levelLoad)
	{
		PackedScene loadLevelScene = levelSceneDict[levelLoad];
		LevelEditTree level = loadLevelScene.Instantiate<LevelEditTree>();
		FireBoxGameData fireBoxGameData = level.GetFireBoxGameData();
		level.QueueFree();
		fireBoxGame = new FireBoxGame(fireBoxGameData);
		currentLevel = levelLoad;

		if (OnLoadedLevelEvent != null)
		{ OnLoadedLevelEvent.Invoke(); }
		if (OnGameSteppedEvent != null)
		{
			OnGameSteppedEvent.Invoke();
		}
	}


	public override void _Process(double delta)
	{
		bool isAction = false;
		if (Input.IsActionJustPressed("MoveLeft"))
		{
			fireBoxGame.Step(EPlayerAction.MoveLeft);
			isAction = true;
		}
		else if (Input.IsActionJustPressed("MoveRight"))
		{
			fireBoxGame.Step(EPlayerAction.MoveRight);
			isAction = true;
		}
		else if (Input.IsActionJustPressed("MoveUp"))
		{
			fireBoxGame.Step(EPlayerAction.MoveUp);
			isAction = true;
		}
		else if (Input.IsActionJustPressed("MoveDown"))
		{
			fireBoxGame.Step(EPlayerAction.MoveDown);
			isAction = true;
		}

		else if (Input.IsActionJustPressed("ReStart"))
		{
			fireBoxGame.Restart();
			isAction = true;
		}
		else if (Input.IsActionJustPressed("Undo"))
		{
			fireBoxGame.Undo();
			isAction = true;
		}

		if (OnGameSteppedEvent != null && isAction)
		{
			OnGameSteppedEvent.Invoke();
		}
	}
}