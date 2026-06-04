using UnityEngine;
public class BrushFollowUI : MonoBehaviour { void Update() { Vector3 mouse = Input.mousePosition; mouse.z = 0; transform.position = mouse; } }