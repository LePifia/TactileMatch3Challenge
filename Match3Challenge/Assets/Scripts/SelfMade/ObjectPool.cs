using System.Collections.Generic;
using Tactile.TactileMatch3Challenge.ViewComponents;
using UnityEngine;

namespace Tactile.TactileMatch3Challenge.Model {
public class ObjectPool : MonoBehaviour {
    
    [SerializeField] private VisualPiece visualPiecePrefab;
    private Queue<VisualPiece> pool = new Queue<VisualPiece>();

    public static ObjectPool Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public VisualPiece GetPiece() {
        if (pool.Count > 0) {
            VisualPiece piece = pool.Dequeue();
            piece.gameObject.SetActive(true);
            return piece;
        }
        return Instantiate(visualPiecePrefab);
    }

    public void ReturnPiece(VisualPiece piece) {
        piece.gameObject.SetActive(false);
        pool.Enqueue(piece);
    }
}
}
