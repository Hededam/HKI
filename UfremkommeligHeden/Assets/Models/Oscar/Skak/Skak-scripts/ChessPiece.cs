using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    public enum PieceType { Pawn, Rook, Knight, Bishop, Queen, King }
    public PieceType pieceType;
    public int currentSquareIndex;

    public void MoveToSquare(int squareIndex)
    {
        // Flyt brikken til det nye felt
        currentSquareIndex = squareIndex;
        // Her kan du opdatere brikkens position i spilverdenen
    }
}