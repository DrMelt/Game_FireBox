using Godot;
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;


namespace GameKernel
{

    public class CellState
    {
        public enum ECellType
        {
            None,
            Normal,
            Target,
            FireSource,
            WaterSource,
        }
        public ECellType cellType = ECellType.Normal;

        public Vector2I mapPos;
        bool canEnter = true;
        public bool CanEnter
        {
            get
            {
                return canEnter;
            }

            set => canEnter = value;
        }



        public CellState()
        {

        }
        public CellState(CellState boxState)
        {
            this.cellType = boxState.cellType;
            this.mapPos = boxState.mapPos;
            this.canEnter = boxState.canEnter;
        }
    }

    public class BoxState
    {
        public enum EBoxType
        {
            None,
            Normal,
        }
        public Vector2I mapPos;

        public EBoxType eBoxType = EBoxType.Normal;
        public bool canBePush = true;
        public bool canBeFire = true;
        public bool isFire = false;

        public BoxState()
        {
        }
        public BoxState(BoxState boxState)
        {
            this.mapPos = boxState.mapPos;
            this.eBoxType = boxState.eBoxType;
            this.canBePush = boxState.canBePush;
            this.canBeFire = boxState.canBeFire;
            this.isFire = boxState.isFire;
        }
    }

    public enum EPlayerAction
    {
        None,
        MoveLeft,
        MoveRight,
        MoveUp,
        MoveDown,
    }

    public class PlayerBoxState
    {
        Vector2I mapPos;
        public Vector2I MapPos { get => mapPos; }
        public PlayerBoxState()
        {
        }
        public PlayerBoxState(Vector2I pos)
        {
            mapPos = pos;
        }
        public PlayerBoxState(PlayerBoxState playerBoxState)
        {
            mapPos = playerBoxState.mapPos;
        }

        public void Step(EPlayerAction playerAction, FireBoxGameData fireBoxGameData)
        {
            Vector2I movement = Vector2I.Zero;
            switch (playerAction)
            {
                case EPlayerAction.MoveLeft:
                    movement = Vector2I.Left;
                    break;
                case EPlayerAction.MoveRight:
                    movement = Vector2I.Right;
                    break;
                case EPlayerAction.MoveUp:
                    movement = Vector2I.Up;
                    break;
                case EPlayerAction.MoveDown:
                    movement = Vector2I.Down;
                    break;
                default:
                    break;
            }

            List<BoxState> pushedBoxStateList = new List<BoxState>();
            Vector2I newPos = mapPos;
            bool isPushed;
            while (true)
            {
                newPos = newPos + movement;
                BoxState newBoxState = fireBoxGameData.GetBoxState(newPos);
                if (newBoxState != null)
                {
                    if (newBoxState.canBePush)
                    {
                        pushedBoxStateList.Add(newBoxState);
                    }
                    else
                    {
                        isPushed = false;
                        break;
                    }
                }
                else
                {
                    CellState cellState = fireBoxGameData.GetCellState(newPos);
                    if (cellState == null || cellState.CanEnter == false)
                    {
                        isPushed = false;
                        break;
                    }
                    else
                    {
                        isPushed = true;
                        break;
                    }
                }
            }

            if (isPushed)
            {
                foreach (BoxState boxState in pushedBoxStateList)
                {
                    boxState.mapPos += movement;
                }
                mapPos += movement;
            }


        }
    }

    public class FireBoxGameData
    {
        public class FireBoxGameState
        {
            public bool isLevelCleared = false;
            public FireBoxGameState() { }

            public FireBoxGameState(FireBoxGameState fireBoxGameState)
            {
                this.isLevelCleared = fireBoxGameState.isLevelCleared;
            }
        }


        public FireBoxGameState fireBoxGameState = new FireBoxGameState();
        public Vector2I MapSize { get; }

        public List<BoxState> boxStateList = new List<BoxState>();


        CellState[,] boxStateMap;
        public CellState[,] BoxStateMap { get => boxStateMap; }
        public void SetCellState(CellState cellState, Vector2I mapPos)
        {
            cellState.mapPos = mapPos;
            boxStateMap[mapPos.X, mapPos.Y] = cellState;
        }
        public void SetBoxState(BoxState box, Vector2I mapPos)
        {
            box.mapPos = mapPos;
            boxStateList.Add(box);
        }


        public bool IsPosInMap(Vector2I mapPos)
        {
            return mapPos.X >= 0 && mapPos.X < MapSize.X && mapPos.Y >= 0 && mapPos.Y < MapSize.Y;
        }

        public CellState GetCellState(Vector2I mapPos)
        {
            if (IsPosInMap(mapPos))
            {
                return boxStateMap[mapPos.X, mapPos.Y];
            }
            else
            {
                return null;
            }
        }

        public BoxState GetBoxState(Vector2I mapPos)
        {
            foreach (BoxState boxState in boxStateList)
            {
                if (boxState.mapPos == mapPos)
                {
                    return boxState;
                }
            }
            return null;
        }
        public PlayerBoxState playerBoxState = null;

        public bool EqualState(FireBoxGameData fireBoxGameData)
        {
            throw new NotImplementedException();
        }
        public FireBoxGameData Backup()
        {
            FireBoxGameData backupFireBoxGameData = new FireBoxGameData(MapSize);
            backupFireBoxGameData.boxStateMap = new CellState[MapSize.X, MapSize.Y];
            for (int x = 0; x < MapSize.X; x++)
            {
                for (int y = 0; y < MapSize.Y; y++)
                {
                    if (boxStateMap[x, y] != null)
                    {
                        backupFireBoxGameData.boxStateMap[x, y] = new CellState(boxStateMap[x, y]);
                    }
                }
            }

            backupFireBoxGameData.boxStateList = new List<BoxState>();
            foreach (BoxState boxState in boxStateList)
            {
                backupFireBoxGameData.boxStateList.Add(new BoxState(boxState));
            }

            backupFireBoxGameData.fireBoxGameState = new FireBoxGameState(fireBoxGameState);
            backupFireBoxGameData.playerBoxState = new PlayerBoxState(playerBoxState);


            return backupFireBoxGameData;
        }


        public FireBoxGameData(Vector2I mapSize)
        {
            this.MapSize = mapSize;
            boxStateMap = new CellState[mapSize.X, mapSize.Y];
            boxStateList = new List<BoxState>();

            playerBoxState = new PlayerBoxState();
        }



        public List<CellState> GetAdjacentCells(Vector2I pos)
        {
            List<CellState> adjacent = new List<CellState>();

            Vector2I upPos = pos + Vector2I.Up;
            if (IsPosInMap(upPos))
            {
                adjacent.Add(GetCellState(upPos));
            }
            Vector2I downPos = pos + Vector2I.Down;
            if (IsPosInMap(downPos))
            {
                adjacent.Add(GetCellState(downPos));
            }
            Vector2I leftPos = pos + Vector2I.Left;
            if (IsPosInMap(leftPos))
            {
                adjacent.Add(GetCellState(leftPos));
            }
            Vector2I rightPos = pos + Vector2I.Right;
            if (IsPosInMap(rightPos))
            {
                adjacent.Add(GetCellState(rightPos));
            }
            adjacent.RemoveAll((box) => { return box == null; });
            return adjacent;
        }

        public List<BoxState> GetAdjacentBoxes(Vector2I pos)
        {
            List<Vector2I> tryMapPosList = new List<Vector2I>
            {
                pos + Vector2I.Up,
                pos + Vector2I.Down,
                pos + Vector2I.Left,
                pos + Vector2I.Right
            };

            List<BoxState> adjacent = new List<BoxState>();
            foreach (BoxState boxState in boxStateList)
            {
                foreach (Vector2I tryMapPos in tryMapPosList)
                {
                    if (boxState.mapPos == tryMapPos)
                    {
                        adjacent.Add(boxState);
                    }
                }
            }

            return adjacent;
        }
    }

    public class FireBoxGame
    {
        List<FireBoxGameData> historyList = new List<FireBoxGameData>();
        FireBoxGameData gameData = null;
        public FireBoxGameData GameData { get => gameData; }
        public Vector2I MapSize { get => gameData.MapSize; }


        public FireBoxGame(FireBoxGameData fireBoxGameData)
        {
            this.gameData = fireBoxGameData;
        }
        public FireBoxGame(Vector2I mapSize)
        {
            gameData = new FireBoxGameData(mapSize);
        }

        public void Undo()
        {
            if (historyList.Count > 0)
            {
                gameData = historyList.Last();
                historyList.RemoveAt(historyList.Count - 1);
            }
        }

        public void Restart()
        {
            if (historyList.Count > 0)
            {
                gameData = historyList[0];
            }
            historyList.Clear();
        }


        public void Step(EPlayerAction playerAction)
        {
            historyList.Add(gameData.Backup());

            PlayerMove(playerAction);

            MechanismOperation();

            FireBoxGameStateUpdate();
        }

        private void MechanismOperation()
        {
            // bool isChanged = true;

            // while (isChanged)
            {
                // isChanged = false;
                foreach (BoxState boxState in gameData.boxStateList)
                {
                    List<BoxState> adBoxes = gameData.GetAdjacentBoxes(boxState.mapPos);
                    foreach (BoxState box in adBoxes)
                    {
                        if (box.isFire)
                        {
                            if (boxState.canBeFire)
                            {
                                boxState.isFire = true;
                            }
                        }
                    }


                    List<CellState> adCells = gameData.GetAdjacentCells(boxState.mapPos);
                    foreach (CellState cellState in adCells)
                    {
                        if (cellState.cellType == CellState.ECellType.FireSource)
                        {
                            if (boxState.canBeFire)
                            {
                                boxState.isFire = true;
                            }
                        }
                    }

                }
            }
        }

        private void FireBoxGameStateUpdate()
        {
            bool hasUnclearedTarget = false;
            foreach (CellState boxState in gameData.BoxStateMap)
            {
                if (boxState != null)
                {
                    if (boxState.cellType == CellState.ECellType.Target)
                    {
                        bool hasFireBox = false;
                        foreach (BoxState box in gameData.boxStateList)
                        {
                            if (box.mapPos == boxState.mapPos)
                            {
                                if (box.isFire)
                                {
                                    hasFireBox = true;
                                    break;
                                }
                            }
                        }

                        hasUnclearedTarget = hasUnclearedTarget || !hasFireBox;
                    }
                }
            }

            gameData.fireBoxGameState.isLevelCleared = !hasUnclearedTarget;
        }

        private void PlayerMove(EPlayerAction playerAction)
        {
            gameData.playerBoxState.Step(playerAction, gameData);
        }

    }
}