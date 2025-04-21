using GameKernel;
using Godot;
using MathNet.Numerics.Distributions;
using System;

[Tool]
public partial class GameShowCell : Node3D
{
	[ExportGroup("GameShow")]
	[Export]
	Label3D endLabel = null;
	[Export]
	Label3D targetLabel = null;
	[Export]
	Label3D fireLabel = null;
	[Export]
	Node3D fireSource = null;



	[Export]
	Label3D canEnterLabel = null;

	[ExportGroup("Edit")]
	[Export]
	CellState.ECellType boxType = CellState.ECellType.Normal;
	[Export]
	bool canEnter = true;



	public Vector2I mapPos;


	public Vector2I Position2I { get { return new Vector2I((int)Position.X, (int)Position.Z); } set { Position = new Vector3(value.X, 0, value.Y); } }




	public override void _Ready()
	{

	}

	public override void _Process(double delta)
	{
		if (Engine.IsEditorHint())
		{
			UpdateStateShow();
		}
	}

	public CellState GetState()
	{
		CellState newBoxState = new CellState()
		{
			mapPos = mapPos,
			cellType = boxType,
			CanEnter = canEnter,
		};
		return newBoxState;
	}



	internal void UpdateState(FireBoxGame fireBoxGame)
	{
		CellState boxState = fireBoxGame.GameData.GetCellState(mapPos);
		if (boxState == null)
		{
			return;
		}

		boxType = boxState.cellType;
		canEnter = boxState.CanEnter;

		UpdateStateShow();
	}

	void UpdateStateShow()
	{
		fireSource.Visible = boxType == CellState.ECellType.FireSource;

		fireLabel.Visible = false;
		canEnterLabel.Visible = !canEnter;
		targetLabel.Visible = boxType == CellState.ECellType.Target;

		if (canEnter == false && boxType == CellState.ECellType.Normal)
		{
			Visible = Engine.IsEditorHint();
		}
		else
		{
			Visible = true;
		}
	}
}
