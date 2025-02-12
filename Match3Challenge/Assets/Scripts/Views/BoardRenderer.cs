using System.Collections;
using System.Collections.Generic;
using Tactile.TactileMatch3Challenge.Model;
using UnityEngine;

namespace Tactile.TactileMatch3Challenge.ViewComponents {

	public class BoardRenderer : MonoBehaviour {
		
		[SerializeField] private PieceTypeDatabase pieceTypeDatabase;
		[SerializeField] private VisualPiece visualPiecePrefab;

		private List<Piece> createdPieces;
		private List<Piece> movedPieces;
		
		private Board board;
		
		public void Initialize(Board board) {
			this.board = board;
			this.createdPieces = new List<Piece>();
			this.movedPieces = new List<Piece>();

			board.SetBoardRenderer(this);

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
				visualPiece.transform.localPosition = LogicPosToVisualPos(pieceInfo.pos.x, pieceInfo.pos.y);

				if (createdPieces.Contains(pieceInfo.piece)) {
                    AnimatePiece(visualPiece);
                }

				if (movedPieces.Contains(pieceInfo.piece)) {
					AnimateMovedPiece(visualPiece);
				}
			}
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

		private void AnimatePiece(VisualPiece visualPiece) {
			Vector3 startPosition = new Vector3(visualPiece.transform.localPosition.x, visualPiece.transform.localPosition.y +5, 0);
            Vector3 targetPosition = visualPiece.transform.localPosition;

			float animationTime = 0.75f; 
			StartCoroutine(AnimateCoroutine(visualPiece, startPosition, targetPosition, animationTime));
		}

		private void AnimateMovedPiece(VisualPiece visualPiece) {
			Vector3 startPosition = new Vector3(visualPiece.transform.localPosition.x, visualPiece.transform.localPosition.y +1, 0);
            Vector3 targetPosition = visualPiece.transform.localPosition;

			float animationTime = 0.75f; 
			StartCoroutine(AnimateCoroutine(visualPiece, startPosition, targetPosition, animationTime));
		}

		private IEnumerator AnimateCoroutine(VisualPiece visualPiece, Vector3 startPosition, Vector3 targetPosition, float animationTime) {
			visualPiece.transform.localPosition = targetPosition;
			float elapsedTime = 0f;

			while (elapsedTime < animationTime) {
				elapsedTime += Time.deltaTime;
				float lerpFactor = elapsedTime / animationTime;

				visualPiece.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, lerpFactor);
				

				yield return null;
			}

			if (elapsedTime > animationTime){
				RefreshCreatedPieces();
				RefreshMovedPieces();
			}
			
		}

		public void AddCreatedPiece(Piece piece) {
			createdPieces.Add(piece);
		}

		public void RefreshCreatedPieces() {
            createdPieces.Clear();
        }

		public void AddMovedPiece(Piece piece) {
			movedPieces.Add(piece);
		}

		public void RefreshMovedPieces() {
            movedPieces.Clear();
        }
		
	}

}
