using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class SpriteMover : MonoBehaviour
{
    [SerializeField] private SpriteSlicer spriteSlicer;
    [SerializeField] private InputManager inputManager;

    public event Action MovementCompletedEvent;

    private bool isMovementAllowed = true;

    private void OnEnable()
    {
        inputManager.ClickEvent += HandleClick;
    }

    private void OnDisable()
    {
        inputManager.ClickEvent -= HandleClick;
    }

    private void HandleClick(Vector2 context)
    {
        if (isMovementAllowed)
            HandleSpriteMovement(context, spriteSlicer.pieces, spriteSlicer.gridSize);
    }

    private void HandleSpriteMovement(Vector2 mousePosition, List<Transform> pieces, int gridSize)
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePosition), Vector2.zero);
        if (hit)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == hit.transform)
                {
                    if (SwapIfValid(i, -spriteSlicer.gridSize, spriteSlicer.gridSize)) 
                    {
                        MovementCompletedEvent?.Invoke();
                        break; 
                    }
                    if (SwapIfValid(i, +gridSize, gridSize))
                    {
                        MovementCompletedEvent?.Invoke();
                        break;
                    }
                    if (SwapIfValid(i, -1, 0))
                    {
                        MovementCompletedEvent?.Invoke();
                        break;
                    }
                    if (SwapIfValid(i, +1, gridSize - 1))
                    {
                        MovementCompletedEvent?.Invoke();
                        break;
                    }
                }
            }
        }
    }
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % spriteSlicer.gridSize) != colCheck) && ((i + offset) == spriteSlicer.emptyLocation))
        {
            // Swap them in game state.
            (spriteSlicer.pieces[i], spriteSlicer.pieces[i + offset]) = (spriteSlicer.pieces[i + offset], spriteSlicer.pieces[i]);
            // Swap their transforms.
            (spriteSlicer.pieces[i].localPosition, spriteSlicer.pieces[i + offset].localPosition) = ((spriteSlicer.pieces[i + offset].localPosition, spriteSlicer.pieces[i].localPosition));
            // Update empty location.
            spriteSlicer.emptyLocation = i;
            return true;
        }
        return false;
    }

    public bool CheckCompletion()
    {
        for (int i = 0; i < spriteSlicer.pieces.Count; i++)
        {
            if (spriteSlicer.pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    //public IEnumerator WaitShuffle(float duration)
    //{
    //    yield return new WaitForSeconds(duration);
    //    Shuffle();
    //    ToggleMovementAllowance(true);
    //}

    public void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (spriteSlicer.gridSize * spriteSlicer.gridSize * spriteSlicer.gridSize))
        {
            // Pick a random location.
            int rnd = UnityEngine.Random.Range(0, spriteSlicer.gridSize * spriteSlicer.gridSize);
            // Only thing we forbid is undoing the last move.
            if (rnd == last) { continue; }
            last = spriteSlicer.emptyLocation;
            // Try surrounding spaces looking for valid move.
            if (SwapIfValid(rnd, -spriteSlicer.gridSize, spriteSlicer.gridSize))
            {
                count++;
            }
            else if (SwapIfValid(rnd, spriteSlicer.gridSize, spriteSlicer.gridSize))
            {
                count++;
            }
            else if (SwapIfValid(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfValid(rnd, +1, spriteSlicer.gridSize - 1))
            {
                count++;
            }
        }
    }

    public void ToggleMovementAllowance(bool active)
    {
        isMovementAllowed = active;
    }
}
