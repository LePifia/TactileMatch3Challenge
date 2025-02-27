using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Tactile.TactileMatch3Challenge.Model;

namespace Tactile.TactileMatch3Challenge.Tests.UnitTests {
    
    public class BoardTests {
        
        [Test]
        public void Width_GivenBoardDefinition_ShouldReturnLengthOfXAxis() {
            // Arrange
            int[,] state = {
                {0, 0, 0}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act
            var width = board.Width;
            
            // Assert
            Assert.That(width, Is.EqualTo(3));
        }
        
        [Test]
        public void Height_GivenBoardDefinition_ShouldReturnLengthOfYAxis() {
            // Arrange
            int[,] state = {
                {0, 0, 0},
                {0, 0, 0}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act
            var height = board.Height;
            
            // Assert
            Assert.That(height, Is.EqualTo(2));
        }

        [TestCase( 0, 0, ExpectedResult = 1, TestName = "Coordinate: (0,0) = 1" )]
        [TestCase( 1, 1, ExpectedResult = 2, TestName = "Coordinate: (1,1) = 2" )]
        [TestCase( 2, 1, ExpectedResult = 3, TestName = "Coordinate: (2,1) = 3" )]
        [TestCase( 1, 0, ExpectedResult = 4, TestName = "Coordinate: (1,0) = 4" )]
        public int GetAt_GivenCoordinate_ShouldReturnExpectedValue(int x, int y) {
            // Arrange
            int[,] state = {
                {1, 4, 0},
                {0, 2, 3}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act & Assert
            return board.GetAt(x, y).type;
        }

        [Test]
        public void GetAt_GivenCoordinatesOutsideBounds_ShouldThrowException() {
            // Arrange
            int[,] state = {
                {1, 0, 0},
                {0, 0, 0}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act - Assert
            Assert.Throws<IndexOutOfRangeException>(() => board.GetAt(x: 42, y: 0));
        }
        
        [TestCase( 1, 1, ExpectedResult = new[] { 2, 2, 2, 2 } )]
        [TestCase( 0, 0, ExpectedResult = new[] { 2, 2 } )]
        [TestCase( 3, 1, ExpectedResult = new[] { 2, 4, 4 } )]
        public int[] GetNeighbors_GivenCoordinate_ShouldReturnNeighboringValues(int x, int y) {
            // Arrange
            int[,] state = {
                {1, 2, 1, 4},
                {2, 3, 2, 4},
                {1, 2, 1, 4}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act & Assert
            return GetTypesFromPieces(board.GetNeighbors(x, y));
        }

        [Test]
        public void GetConnected_GivenAllNeighborsIsPieceType_ShouldReturnNeighborCells() {
            // Arrange
            int[,] state = {
                {1, 3, 1},
                {2, 3, 2},
                {1, 3, 1}
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act
            var connectedNeighbors = board.GetConnected(x: 1, y: 1);
            
            // Assert
            Assert.That(GetTypesFromPieces(connectedNeighbors), Is.EqualTo(new[] { 3, 3, 3 }));
        }
        
        [Test]
        public void GetConnected_GivenAsymmetricCluster_ShouldReturnConnectionOfSameType() {
            // Arrange
            int[,] state = {
                {1, 1, 0, 0, 0, 0},
                {0, 1, 1, 0, 0, 0},
                {0, 1, 1, 1, 0, 0},
                {0, 1, 0, 0, 0, 0},
                {0, 0, 0, 0, 1, 1},
                {0, 0, 0, 0, 1, 1},
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act
            var connectedNeighbors = board.GetConnected(x: 1, y: 1);
            
            // Assert
            Assert.That(connectedNeighbors.Count, Is.EqualTo(8));
        }
        
        [Test]
        public void GetConnected_GivenLoopedCluster_ShouldReturnConnectionOfSameType() {
            // Arrange
            int[,] state = {
                {0, 0, 0, 0, 0, 0},
                {0, 1, 1, 1, 0, 0},
                {0, 1, 0, 1, 0, 0},
                {0, 1, 0, 1, 0, 0},
                {0, 1, 1, 1, 0, 0},
                {0, 0, 0, 0, 1, 1},
            };
            var board = Board.Create(state, new PieceSpawner());
            
            // Act
            var cluster = board.GetConnected(x: 1, y: 1);
            
            // Assert
            Assert.That(cluster.Count, Is.EqualTo(10));
        }
        
        [Test]
        public void Board_GivenDefinition_StateArrayHasSameOrientation() {
            // Arrange
            int[,] state = {
                {0, 1, 1, 1, 1 },
                {0, 2, 3, 4, 5 },
            };
            
            //Act            
            var board = Board.Create(state, new PieceSpawner());
            
            //Assert
            Assert.That(board.GetAt(0, 0).type,Is.EqualTo(0));
            Assert.That(board.GetAt(4, 1).type,Is.EqualTo(5));
        }
        
        [Test]
		public void MoveAndCreatePiecesUntilFull_GivenSelectedPieceInAsymmetricalCluster_ShouldRemoveAllNeighborsAndMovePiecesToAvailableSlots() {
			// Arrange
			int[,] state = {
				{0, 4, 4, 4, 4},
				{0, 2, 3, 1, 4},
				{0, 4, 4, 4, 0},
				{0, 0, 4, 0, 0}
			};
			var randomSpawner = new PieceSpawnerFake(42);
			var board = Board.Create(state, randomSpawner);
			
			// Act
			board.FindAndRemoveConnectedAt(1, 2, TypeOfConnections.normal);
			board.MoveAndCreatePiecesUntilFull();

			// Assert
			int[,] expected = {
				{0, 42, 42, 42, 4},
				{0,  4, 42,  4, 4},
				{0,  2,  4,  1, 0},
				{0,  0,  3,  0, 0}
			};

			var result = board.GetBoardStateAsArrayWithTypes();
			Assert.That(result, Is.EqualTo(expected));
			
		}

        private int[] GetTypesFromPieces(Piece[] pieces) {
            return pieces.Select(p => p.type).ToArray();
        }
        
        private int[] GetTypesFromPieces(List<Piece> pieces) {
            return pieces.Select(p => p.type).ToArray();
        }
        
    }
    
}