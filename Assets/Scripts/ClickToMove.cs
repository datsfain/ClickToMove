using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ClickToMove : MonoBehaviour
{
    private Queue<MoveCommand> moveCommands = new Queue<MoveCommand>();
    private MoveCommand currentMoveCommand;

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float moveSpeed => speedSlider.value;

    private void Start() => StartCoroutine(_ManageCommandQueue());

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 clickPosition = Input.mousePosition;
            Vector2 destination = mainCamera.ScreenToWorldPoint(clickPosition);
            MoveCommand moveCommand = new MoveCommand(transform, destination);
            moveCommands.Enqueue(moveCommand);
        }
    }
    private void Update()
    {
        ProcessInput();

        currentMoveCommand?.Tick(Time.deltaTime, moveSpeed);
    }

    IEnumerator _ManageCommandQueue()
    {
        WaitUntil waitUntilQueueNotEmpty = new WaitUntil(() => moveCommands.Count > 0);

        yield return waitUntilQueueNotEmpty;

        while (true)
        {
            if (moveCommands.Count == 0) 
                yield return waitUntilQueueNotEmpty;

            currentMoveCommand = moveCommands.Dequeue();
            currentMoveCommand.Begin();
            yield return new WaitUntil(() => currentMoveCommand.isCompleted);
        }
    }

}
