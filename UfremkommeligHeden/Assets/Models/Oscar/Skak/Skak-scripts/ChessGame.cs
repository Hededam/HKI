using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class ChessGame : MonoBehaviour
{
    public ChessBoard chessBoard;
    public Button restartButton; // Reference til TMP Button

    private List<ChessPiece> pieces;

    private void Start()
    {
        // Tilføj en listener til knappen
        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        InitializePieces();
    }

    private void InitializePieces()
    {
        pieces = new List<ChessPiece>();
        foreach (GameObject pieceObject in chessBoard.pieces)
        {
            ChessPiece piece = pieceObject.GetComponent<ChessPiece>();
            if (piece != null)
            {
                pieces.Add(piece);
            }
        }
    }

    public void RestartGame()
    {
        Debug.Log("Spillet genstartes...");

        // Nulstil brikkernes positioner
        chessBoard.InitializeBoard();

        // Eventuelt: Nulstil spilvariabler, point, osv.
    }

    public bool IsMoveValid(ChessPiece piece, int targetSquareIndex)
    {
        // Implementer logik for at tjekke, om et træk er gyldigt
        return true; // Dette er bare en placeholder
    }

    public void MovePiece(ChessPiece piece, int targetSquareIndex)
    {
        if (IsMoveValid(piece, targetSquareIndex))
        {
            piece.MoveToSquare(targetSquareIndex);
            chessBoard.PlacePiece(piece.gameObject, chessBoard.squares[targetSquareIndex]);
        }
        else
        {
            Debug.Log("Ugyldigt træk");
        }
    }
}