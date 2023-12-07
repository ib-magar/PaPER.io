using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    public LayerMask layerMask;
    [SerializeField] bool isPathing;

    [SerializeField] float readPathInterval;
    private float readpathintervalCounter;
    private Vector3 lastPos;

    [SerializeField] private List<Vector3> vertices = new List<Vector3>();
    public WaypointMeshGenerator _meshGenerator;
    private void Start()
    {
        isPathing = false;
        readpathintervalCounter = readPathInterval;
        vertices.Clear();
    }

    private MeshCollider meshCollider;
    private Mesh mesh;
    public pathObj pathObjPrefab;
    [SerializeField] private List<pathObj> pathObjs = new List<pathObj>();

    void InitializeCollider()
    {
        pathObjs.Clear();
       pathObj p =  Instantiate(pathObjPrefab, transform.position-transform.forward*1f, transform.rotation);
        p.Initialize(GetComponent<CharacterController>());
        pathObjs.Add(p);
    }

    public void AddVertexToCollider(Vector3 vertex)
    {
        pathObj p = Instantiate(pathObjPrefab, transform.position - transform.forward * 1f, transform.rotation);
        p.Initialize(GetComponent<CharacterController>());
        pathObjs.Add(p);

    }


    private void Update()
    {
        if (isPathing)
        {
            readpathintervalCounter -= Time.deltaTime;
            if (readpathintervalCounter <= 0f && Vector3.Distance(lastPos,transform.position)>.5f)
            {
                readpathintervalCounter = readPathInterval;
                //Send position to the script
                //AddVertexPoint(transform.position);
                vertices.Add(transform.position);
                AddVertexToCollider(transform.position);
                lastPos= transform.position;    
            }
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _meshGenerator.GenerateMeshFromWayPoints(vertices);
            isPathing = false;
            vertices.Clear();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        int objLayerMask = 1 << other.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0 && isPathing)
        {
            isPathing = false;
            readpathintervalCounter = 1f;
            vertices.Add(transform.position+transform.forward*1f);
            vertices.Add(transform.position+transform.forward*1f);
            _meshGenerator.GenerateMeshFromWayPoints(vertices);
            vertices.Clear();
             foreach(pathObj g in pathObjs) Destroy(g.gameObject);
            pathObjs.Clear();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        int objLayerMask = 1 << other.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0)
        {
            InitializeCollider();

            isPathing = true;
            vertices.Add(transform.position-transform.forward*1f);
            vertices.Add(transform.position-transform.forward*1f);
            lastPos = transform.position;
        }
    }

    /*public CustomPolygonCollider AreaPrefab;
    private CustomPolygonCollider currentAreaPrefab;
    void InitializePolygonCollider(Vector2 pos)
    {
        currentAreaPrefab = Instantiate(AreaPrefab, transform.position, Quaternion.identity);
        currentAreaPrefab.InitializeCollider(pos);
    }
    void AddVertexPoint(Vector2 pos)
    {
        //send position
        currentAreaPrefab.AddVertex(pos);
    }

    [Header("Sprite Creation")]
    public SpriteCreator _spriteCreator;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int objLayerMask = 1 << collision.gameObject.layer;
        if ((layerMask.value & objLayerMask) != 0 && isPathing)
        {
            isPathing = false;
            readpathintervalCounter = readPathInterval;
            if (currentAreaPrefab != null)
            {
                //_spriteCreator.CreateSpriteFromColliderData(currentAreaPrefab.GetComponent<PolygonCollider2D>());
                _spriteCreator.CreateSprite(currentAreaPrefab.GetComponent<PolygonCollider2D>());
                //provide the vector2 data to the polygon Collider script And for the sprite out of it

            }
        }
    }*/

}
