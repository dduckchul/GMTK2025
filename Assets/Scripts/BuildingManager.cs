using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    // 마지막으로 내가 있던 빌딩 정보 저장
    public char currentBuilding;
    public GameObject buildingPrefab;
    public GameObject[] buildings;
    public int buildingOffset = 20;
    public float moveMultiple = 0.5f;
    
    private int currentInd;
    private int prevInd;
    private int prevprevInd;
    private int nextInd;
    private int nextnextInd;

    private Vector2 moveDir = Vector2.zero;
    
    private void Awake()
    {
        InstantiateBuilding();
        InitBuildingPosition();
    }

    private void Start()
    {
        ShowBuildings();
    }

    private void FixedUpdate()
    {
        if (moveDir != Vector2.zero)
        {
            Vector3 toPlayerMove = new Vector3(moveDir.x, 0, moveDir.y) * (-1 * moveMultiple);
            MoveBuildings(toPlayerMove);
        }
    }

    private void InstantiateBuilding()
    {
        buildings = new GameObject[26];
        
        for (int i = 0; i < buildings.Length; i++)
        {
            buildings[i] = Instantiate(buildingPrefab, Vector3.zero, Quaternion.identity);
            BuildingInfo buildingInfo = buildings[i].GetComponent<BuildingInfo>();
            buildingInfo.buildingName = (char)(97 + i);
            buildingInfo.SetBuildingManager(this);
            buildings[i].SetActive(false);
            buildings[i].transform.parent = gameObject.transform;
        }        
    }

    // 빌딩 포지션 초기화하기
    private void InitBuildingPosition()
    {
        if (currentBuilding == '0')
        {
            currentBuilding = 'a';
        }

        currentInd = currentBuilding - 97;
        CalculatePosition();
    }

    // 빌딩 포지션 계산하기
    private void CalculatePosition()
    {
        prevInd = currentInd - 1 >= 0 ? currentInd - 1 : 25;
        prevprevInd = prevInd - 1 >= 0 ? prevInd - 1 : 25;
        
        nextInd = currentInd + 1 > 25 ? 0 : currentInd + 1;
        nextnextInd = nextInd + 1 > 25 ? 0 : nextInd + 1;
        
        Debug.Log("currentIdx : " + currentInd + "prevInd : " + prevInd + "nextInd : " + nextInd);
    }

    private void ShowBuildings()
    {
        ShowBuilding(currentInd, Vector3.zero);
        ShowBuilding(prevInd, Vector3.left * buildingOffset);
        ShowBuilding(prevprevInd, Vector3.left * (buildingOffset * 2));
        ShowBuilding(nextInd, Vector3.right * buildingOffset);
        ShowBuilding(nextnextInd, Vector3.right * (buildingOffset * 2));
    }

    private void ShowBuilding(int index, Vector3 pos)
    {
        buildings[index].transform.position = pos;
        buildings[index].SetActive(true);
    }

    private void MoveBuildings(Vector3 pos)
    {
        MoveBuilding(currentInd, pos);
        MoveBuilding(prevInd, pos);
        MoveBuilding(prevprevInd, pos);
        MoveBuilding(nextInd, pos);
        MoveBuilding(nextnextInd, pos);
    }
    
    private void MoveBuilding(int index, Vector3 pos)
    {
        buildings[index].transform.position += pos;
    }

    private void SwapBuilding(int removeIdx, int showIdx, Vector3 position)
    {
        buildings[removeIdx].SetActive(false);
        buildings[showIdx].SetActive(true);
        buildings[showIdx].transform.position = position;
    }
    
    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 dir = ctx.ReadValue<Vector2>();
        moveDir = dir;
    }

    public void TriggerBuilding(BuildingInfo building, Collider2D other)
    {
        // 지우면 안되는데 지워질경우 에러 발생
        int toRemove = 100;
        int targetIndex = building.buildingName - 97;
        bool isNext = false;
        
        if (targetIndex == nextInd)
        {
            isNext = true;
            toRemove = prevprevInd;
        }
        else if (targetIndex == prevInd)
        {
            isNext = false;
            toRemove = nextnextInd;            
        }
            
        if (targetIndex != currentInd)
        {
            currentBuilding = building.buildingName;
            currentInd = targetIndex;
            CalculatePosition();
        
            if (isNext)
            {
                Vector3 intantiatePos = buildings[nextInd].transform.position + Vector3.right * buildingOffset;
                SwapBuilding(toRemove, nextnextInd, intantiatePos);
            }
            else
            {
                Vector3 intantiatePos = buildings[prevInd].transform.position + Vector3.left *  buildingOffset;
                SwapBuilding(toRemove, prevprevInd, intantiatePos);                
            }
        }
    }
}
