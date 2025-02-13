using System.Collections.Generic;
using UnityEngine;
using Tactile.TactileMatch3Challenge.Model;
using Tactile.TactileMatch3Challenge.ViewComponents;

namespace Tactile.TactileMatch3Challenge {
	
	public class Boot : MonoBehaviour {
		
		[SerializeField] private BoardRenderer boardRenderer;

		[Space]
		[Header ("Board size")]
		[Space]
		[SerializeField] private int boardWidth = 6;
		[SerializeField] private int boardHeight = 6;

		[Space]
		[Header ("Board Information")]
		[Space]
		[SerializeField] private List<RowData> boardRows = new List<RowData>();

		[Space]
		[Header ("Randomizer")]
		[Space]

		[SerializeField] private bool useRandomBoard = false;

		[SerializeField] private int minPieceType = 0;
		[SerializeField] private int maxPieceType = 4;

		

		private void OnValidate() {
			while (boardRows.Count < boardHeight) {
				boardRows.Add(new RowData(boardWidth));
			}
			while (boardRows.Count > boardHeight) {
				boardRows.RemoveAt(boardRows.Count - 1);
			}
			foreach (var row in boardRows) {
				row.EnsureSize(boardWidth);
			}
			if (useRandomBoard) {
				GenerateRandomBoard();
			}
		}

		void Start() {
			int[,] boardDefinition = ConvertToMatrix();
			var pieceSpawner = new PieceSpawner();
			var board = Board.Create(boardDefinition, pieceSpawner);
			boardRenderer.Initialize(board);
		}

		private int[,] ConvertToMatrix() {
			int[,] matrix = new int[boardWidth, boardHeight];

			for (int y = 0; y < boardHeight; y++) {
				for (int x = 0; x < boardWidth; x++) {
					matrix[x, y] = boardRows[y].row[x];
				}
			}

			return matrix;
		}


		private void GenerateRandomBoard() {
			for (int y = 0; y < boardHeight; y++) {
				for (int x = 0; x < boardWidth; x++) {
					boardRows[y].row[x] = Random.Range(minPieceType, maxPieceType + 1);
				}
			}
		}
	}

	[System.Serializable]
	public class RowData {
		[SerializeField] public List<int> row;
		public RowData(int size) {
			row = new List<int>(new int[size]);
		}
		public void EnsureSize(int size) {
			while (row.Count < size) {
				row.Add(0); 
			}
			while (row.Count > size) {
				row.RemoveAt(row.Count - 1);
			}
		}
	}
}
