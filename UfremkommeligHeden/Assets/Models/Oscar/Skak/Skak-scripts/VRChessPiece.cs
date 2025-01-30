using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class VRChessPiece : MonoBehaviour
{
    public ChessGame chessGame;
    public ChessPiece chessPiece;
    private XRGrabInteractable grabInteractable;

    private void Start()
    {
       // grabInteractable = GetComponent<XRGrabInteractable>();
      //  grabInteractable.onSelectExited.AddListener(OnPieceDropped);
    }

    private void OnPieceDropped(XRBaseInteractor interactor)
    {
        // Find det felt, brikken blev sluppet over
        // Dette kan gøres ved at caste en raycast nedad fra brikkens position
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            GameObject square = hit.collider.gameObject;
            int squareIndex = System.Array.IndexOf(chessGame.chessBoard.squares, square);
            if (squareIndex != -1)
            {
                chessGame.MovePiece(chessPiece, squareIndex);
            }
        }
    }
}