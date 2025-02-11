using System.Collections;
using Tactile.TactileMatch3Challenge.Model;
using UnityEngine;

namespace Tactile.TactileMatch3Challenge.ViewComponents {

	public class BoardRenderer : MonoBehaviour {
		
		[SerializeField] private PieceTypeDatabase pieceTypeDatabase;
		[SerializeField] private VisualPiece visualPiecePrefab;
		
		private Board board;
		private VisualPiece[,] visualPieces;
		
		public void Initialize(Board board) {
			this.board = board;
			visualPieces = new VisualPiece[board.Width, board.Height];
			CenterCamera();
			CreateVisualPiecesFromBoardState();
		}

		private void CenterCamera() {
			Camera.main.transform.position = new Vector3((board.Width-1)*0.5f,-(board.Height-1)*0.5f);
		}

		private void CreateVisualPiecesFromBoardState() {
			DestroyVisualPieces();

			foreach (var pieceInfo in board.IteratePieces()) {
				
				var visualPiece = CreateVisualPiece(pieceInfo.piece);
				StartCoroutine(AnimatePiece(visualPiece, pieceInfo.pos.x, pieceInfo.pos.y));

			}
		}

		 private IEnumerator AnimatePiece(VisualPiece visualPiece, int targetX, int targetY) {
            Vector3 startPos = LogicPosToVisualPos(targetX, targetY - board.Height);
            Vector3 endPos = LogicPosToVisualPos(targetX, targetY);

            float duration = 0.5f;
            float elapsedTime = 0f;

            visualPiece.transform.localPosition = startPos;

            while (elapsedTime < duration) {
                visualPiece.transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; 
            }

            visualPiece.transform.localPosition = endPos;
        }
		
		public Vector3 LogicPosToVisualPos(float x,float y) { 
			return new Vector3(x, -y, -y);
		}

		private BoardPos ScreenPosToLogicPos(float x, float y) { 
			
			var worldPos = Camera.main.ScreenToWorldPoint(new Vector3(x,y,-Camera.main.transform.position.z));
			var boardSpace = transform.InverseTransformPoint(worldPos);

			return new BoardPos() {
				x = Mathf.RoundToInt(boardSpace.x),
				y = -Mathf.RoundToInt(boardSpace.y)
			};

		}

		private VisualPiece CreateVisualPiece(Piece piece) {
			
			var pieceObject = Instantiate(visualPiecePrefab, transform, true);
			var sprite = pieceTypeDatabase.GetSpriteForPieceType(piece.type);
			pieceObject.SetSprite(sprite);
			return pieceObject;
			
		}

		private void DestroyVisualPieces() {
			foreach (var visualPiece in GetComponentsInChildren<VisualPiece>()) {
				Object.Destroy(visualPiece.gameObject);
			}
		}

		private void Update() {
			
			if (Input.GetMouseButtonDown(0)) {

				var pos = ScreenPosToLogicPos(Input.mousePosition.x, Input.mousePosition.y);

				if (board.IsWithinBounds(pos.x, pos.y)) {
					board.Resolve(pos.x, pos.y);
					CreateVisualPiecesFromBoardState();
				}

			}
		}
		
	}

}
