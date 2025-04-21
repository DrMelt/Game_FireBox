using GameKernel;
using Godot;
using System;
using System.Collections.Generic;

public partial class LevelEditTree : Node3D
{
	public FireBoxGameData GetFireBoxGameData()
	{
		List<GameShowCell> cellShowList = new List<GameShowCell>();
		List<GameShowBox> boxShowList = new List<GameShowBox>();
		LevelPlayerBox levelPlayerBox = null;

		Godot.Collections.Array<Node> children = GetChildren();
		foreach (Node child in children)
		{
			if (child is GameShowCell)
			{
				GameShowCell levelEditBox = (GameShowCell)child;
				cellShowList.Add(levelEditBox);
			}
			if (child is LevelPlayerBox)
			{
				levelPlayerBox = (LevelPlayerBox)child;
			}
			if (child is GameShowBox)
			{
				GameShowBox gameShowBox = (GameShowBox)child;
				boxShowList.Add(gameShowBox);
			}
		}

		Vector2I maxCorner = cellShowList[0].Position2I;
		Vector2I minCorner = maxCorner;
		foreach (GameShowCell levelEditBox in cellShowList)
		{
            Vector2I pos = levelEditBox.Position2I;
			if (pos.X > maxCorner.X)
			{
				maxCorner.X = pos.X;
			}
			if (pos.Y > maxCorner.Y)
			{
				maxCorner.Y = pos.Y;
			}
			if (pos.X < minCorner.X)
			{
				minCorner.X = pos.X;
			}
			if (pos.Y < minCorner.Y)
			{
				minCorner.Y = pos.Y;
			}
		}
		Vector2I mapSize = maxCorner - minCorner + new Vector2I(1, 1);


		FireBoxGameData fireBoxGameData = new FireBoxGameData(mapSize);
		foreach (GameShowCell cell in cellShowList)
		{
            Vector2I mapPos = cell.Position2I - minCorner;
			fireBoxGameData.SetCellState(cell.GetState(), mapPos);
		}
		foreach (GameShowBox box in boxShowList)
		{
            Vector2I mapPos = box.Position2I - minCorner;
			fireBoxGameData.SetBoxState(box.GetState(), mapPos);
		}

		fireBoxGameData.playerBoxState = levelPlayerBox.GetBoxPlayerState(minCorner);
		return fireBoxGameData;
	}

}
