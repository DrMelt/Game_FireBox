using Godot;
using System;
using GameKernel;
using System.Collections.Generic;

public partial class GameShow : Node3D
{
	[Export]
	GameLogic gameLogicRef = null;
	[Export]
	PackedScene gameShowCellPacked = null;
	[Export]
	PackedScene gameShowPlayerBoxPacked = null;
	[Export]
	PackedScene gameShowBoxPacked = null;


	GameShowCell[,] gameShowCells = null;
	List<GameShowBox> gameShowBoxes = null;

	GameShowPlayerBox gameShowPlayerBox = null;

	public void Init()
	{
		FireBoxGameData fireBoxGameData = gameLogicRef.FireBoxGameRef.GameData;

		if (gameShowCells != null)
		{
			foreach (GameShowCell cell in gameShowCells)
			{
				if (cell != null)
				{
					cell.QueueFree();
				}
			}
			gameShowCells = null;
		}
		if (gameShowPlayerBox != null)
		{
			gameShowPlayerBox.QueueFree();
			gameShowPlayerBox = null;
		}
		if (gameShowBoxes != null)
		{
			foreach (GameShowBox gameShowBox in gameShowBoxes)
			{
				gameShowBox.QueueFree();
			}
			gameShowBoxes = null;
		}

		gameShowCells = new GameShowCell[fireBoxGameData.MapSize.X, fireBoxGameData.MapSize.Y];
		for (int x = 0; x < fireBoxGameData.MapSize.X; x++)
		{
			for (int y = 0; y < fireBoxGameData.MapSize.Y; y++)
			{
				CellState boxState = fireBoxGameData.GetCellState(new Vector2I(x, y));
				if (boxState == null)
				{
					continue;
				}


				GameShowCell cell = gameShowCellPacked.Instantiate<GameShowCell>();
				cell.mapPos = new Vector2I(x, y);
				cell.Position2I = new Vector2I(x, y);

				gameShowCells[x, y] = cell;
				AddChild(cell);
			}
		}

		FlashBoxes();



		gameShowPlayerBox = gameShowPlayerBoxPacked.Instantiate<GameShowPlayerBox>();
		gameShowPlayerBox.mapPos = fireBoxGameData.playerBoxState.MapPos;
		gameShowPlayerBox.Position2I = fireBoxGameData.playerBoxState.MapPos;
		AddChild(gameShowPlayerBox);

		UpdateState();
	}

	void FlashBoxes()
	{
		FireBoxGameData fireBoxGameData = gameLogicRef.FireBoxGameRef.GameData;
		if (gameShowBoxes != null)
		{
			foreach (GameShowBox gameShowBox in gameShowBoxes)
			{
				gameShowBox.QueueFree();
			}
		}


		gameShowBoxes = new List<GameShowBox>();
		foreach (BoxState box in fireBoxGameData.boxStateList)
		{
			GameShowBox gameShowBox = gameShowBoxPacked.Instantiate<GameShowBox>();
			gameShowBox.mapPos = box.mapPos;
			gameShowBox.Position2I = box.mapPos;

			gameShowBox.UpdateState(box);

			AddChild(gameShowBox);
			gameShowBoxes.Add(gameShowBox);
		}
	}

	public override void _Ready()
	{
		gameLogicRef.OnLoadedLevelEvent += Init;
		gameLogicRef.OnGameSteppedEvent += UpdateState;
	}


	private void UpdateState()
	{
		if (gameShowCells != null)
		{
			foreach (GameShowCell cell in gameShowCells)
			{
				if (cell != null)
				{
					cell.UpdateState(gameLogicRef.FireBoxGameRef);
				}
			}
		}

		FlashBoxes();

		if (gameShowPlayerBox != null)
		{
			gameShowPlayerBox.UpdateState(gameLogicRef.FireBoxGameRef);
		}
	}

}
