using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorType { FreeOpen, NeedBlueKey, NeedRedKey }
    public DoorType doorType = DoorType.FreeOpen;
    public float openAngle = 90f;
    public float closeAngle = 0f;
    public float openSpeed = 2f;
    public float interactDistance = 3f;

    private bool isOpen = false;
    private Quaternion targetRotation;

    private Transform player;
    private KeyInventory keyInventory;

    void Start()
    {
        targetRotation = Quaternion.Euler(0, closeAngle, 0);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        keyInventory = player.GetComponent<KeyInventory>();
    }

    void Update()
    {
        // หมุนประตูไปยังเป้าหมาย
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * openSpeed);

        // เช็คระยะกับผู้เล่น
        if (Vector3.Distance(transform.position, player.position) <= interactDistance && Input.GetKeyDown(KeyCode.E))
        {
            switch (doorType)
            {
                case DoorType.FreeOpen:
                    ToggleDoor();
                    break;
                case DoorType.NeedBlueKey:
                    if (keyInventory.hasBlueKey) ToggleDoor();
                    else Debug.Log("ต้องการ Blue Key!");
                    break;
                case DoorType.NeedRedKey:
                    if (keyInventory.hasRedKey) ToggleDoor();
                    else Debug.Log("ต้องการ Red Key!");
                    break;
            }
        }
    }

    void ToggleDoor()
    {
        isOpen = !isOpen;
        targetRotation = Quaternion.Euler(0, isOpen ? openAngle : closeAngle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // กันศัตรูทะลุประตู
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }
}
