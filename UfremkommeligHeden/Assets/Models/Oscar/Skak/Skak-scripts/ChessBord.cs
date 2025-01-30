using UnityEngine;

public class ChessBoard : MonoBehaviour
{
    public GameObject[] squares; // 64 felter (a1 til h8)
    public GameObject[] pieces; // 32 brikker (0-15: hvide, 16-31: sorte)

    private void Start()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        // Placer hvide brikker
        PlacePiece(pieces[0], squares[0]);   // Hvidt tårn på a1
        PlacePiece(pieces[1], squares[1]);   // Hvid springer på b1
        PlacePiece(pieces[2], squares[2]);   // Hvid løber på c1
        PlacePiece(pieces[3], squares[3]);   // Hvid dronning på d1
        PlacePiece(pieces[4], squares[4]);   // Hvid konge på e1
        PlacePiece(pieces[5], squares[5]);   // Hvid løber på f1
        PlacePiece(pieces[6], squares[6]);   // Hvid springer på g1
        PlacePiece(pieces[7], squares[7]);   // Hvidt tårn på h1

        // Hvide bønder
        for (int i = 8; i < 16; i++)
        {
            PlacePiece(pieces[i], squares[i + 8]); // Bønder på a2 til h2
        }

        // Placer sorte brikker
        PlacePiece(pieces[16], squares[56]);  // Sort tårn på a8
        PlacePiece(pieces[17], squares[57]);  // Sort springer på b8
        PlacePiece(pieces[18], squares[58]);  // Sort løber på c8
        PlacePiece(pieces[19], squares[59]);  // Sort dronning på d8
        PlacePiece(pieces[20], squares[60]);  // Sort konge på e8
        PlacePiece(pieces[21], squares[61]);  // Sort løber på f8
        PlacePiece(pieces[22], squares[62]);  // Sort springer på g8
        PlacePiece(pieces[23], squares[63]);  // Sort tårn på h8

        // Sorte bønder
        for (int i = 24; i < 32; i++)
        {
            PlacePiece(pieces[i], squares[i - 8]); // Bønder på a7 til h7
        }
    }

    public void PlacePiece(GameObject piece, GameObject square)
    {
        if (piece == null || square == null)
        {
            Debug.LogWarning("Piece or square is null. Check your references.");
            return;
        }

        // Flyt brikken til feltet
        piece.transform.position = square.transform.position;

        // Opdater brikkens nuværende felt
        ChessPiece chessPiece = piece.GetComponent<ChessPiece>();
        if (chessPiece != null)
        {
            chessPiece.currentSquareIndex = System.Array.IndexOf(squares, square);
        }
        else
        {
            Debug.LogWarning("ChessPiece component missing on piece: " + piece.name);
        }
    }
}