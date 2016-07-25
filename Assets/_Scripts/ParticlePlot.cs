using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class ParticlePlot : MonoBehaviour
{
    private const float _radius = .4f;
    private float _curSize;

    private object[] _points;

    private Quaternion _baseRotation;
    private Vector3 _inV;
    private Quaternion[] _rotations;

    [SerializeField] private TextAsset _tpzpath;


    private DataPlot _dataPlot;

    private Transform _camT;

    float ParticleSize = .3f;

    [SerializeField] private Slider _sizeSlider;
    [SerializeField] private InputField _xPos, _yPos, _zPos;
    [SerializeField] private Text _infoText;

    public float RotationSpeed = .05f;

    //these are the public variables for choosing mesh and material for
    //the mesh System
    public Mesh mesh;
    public Material material;
    public GameObject meshParent;



    public void Start()
    {
        CreatePoints(@"C:\Users\pgb\Desktop\VR_ISV3\Assets\Data\vecs-depth8.tpz");
        //
        //  _dataPlot = LoadJson.Instance.LoadFromFile();
        //  _points = new object[_dataPlot.points.Length];
        //
        // _camT = Camera.main.transform;
        // _baseRotation = Quaternion.identity;
        // _rotations = new[] {Quaternion.identity, Quaternion.identity};
        //
        // _inV = Vector3.zero;
        //
        // _sizeSlider.value = ParticleSize;
        //
        // //Create Points
        // for (int index = 0; index < _dataPlot.points.Length; index++)
        // {
        //     var p = _dataPlot.points[index];
        //
        //     var quat = new Quaternion(p.v[0], p.v[1], p.v[2], p.v[3]);
        //
        //     var normal = new Vector4(quat.x, quat.y, quat.z, quat.w);
        //     normal.Normalize();
        //
        //     //this creates gameobjects and meshes instead of a particle system. the meshes need to be combined!
        //     var meshParticle = new GameObject();
        //     meshParticle.name = index.ToString();
        //     meshParticle.transform.parent = meshParticle.transform;
        //     meshParticle.transform.position = normal.StereographicProjection();
        //     meshParticle.AddComponent<MeshFilter>().mesh = mesh;
        //     meshParticle.AddComponent<MeshRenderer>().material = material;
        //     //meshParticle.transform.localScale = new Vector3(p.Size, p.Size, p.Size);
        //     meshParticle.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
        //
        // }
        //
        // _xPos.text = "0";
        // _yPos.text = "0";
        // _zPos.text = "0";
    }

//this receives a path from SelectFile onclick GUI and passes it to LoadFromFile in LoadJson
    public void CreatePoints(string filePath)
    {
        _infoText.text = "Hit Me 1";
        _dataPlot = LoadJson.Instance.LoadFromFile(@"C:\Users\pgb\Desktop\VR_ISV3\Assets\Data\vecs-depth8.tpz");
        Debug.Log("This will print to console. The error will be thrown afterwards.");
        _points = new object[_dataPlot.points.Length];
        Debug.Log("This will not print to console.");

        for (int index = 0; index < _dataPlot.points.Length; index++)
        {
            var p = _dataPlot.points[index];

            var quat = new Quaternion(p.v[0], p.v[1], p.v[2], p.v[3]);

            var normal = new Vector4(quat.x, quat.y, quat.z, quat.w);
            normal.Normalize();

            var meshParticle = new GameObject();
            meshParticle.name = index.ToString();
            meshParticle.transform.parent = meshParticle.transform;
            meshParticle.transform.position = normal.StereographicProjection();
            meshParticle.AddComponent<MeshFilter>().mesh = mesh;
            meshParticle.AddComponent<MeshRenderer>().material = material;
            //meshParticle.transform.localScale = new Vector3(p.Size, p.Size, p.Size);
            meshParticle.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
        }

    }

    public void Update()
    {
//         // ParticleSize = _sizeSlider.value;
//         //
//         // int xpos = (int)_camT.position.x;
//         // int ypos = (int)_camT.position.y;
//         // int zpos = (int)_camT.position.z;
//         // int.TryParse(_xPos.text, out xpos);
//         // int.TryParse(_yPos.text, out ypos);
//         // int.TryParse(_zPos.text, out zpos);
//         // _camT.position = new Vector3(xpos, ypos, zpos);
//         //
//         // if (MouseControls.rotate)
//         // {
//         //     var inX = Input.GetAxis("Horizontal");
//         //     var inY = Input.GetAxis("Vertical");
//         //
//         //     _inV = _camT.right * inX + _camT.forward * inY;
//         //
//         //     var invn = _inV.normalized;
//         //
//         //     var inq = new Quaternion(invn.x, invn.y, invn.z, 0);
//         //
//         //     _baseRotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Inverse(inq), Time.deltaTime * _inV.magnitude * .25f);
//         //
//         //     _rotations[0] = _baseRotation * _rotations[0];
//         //     _rotations[1] = _rotations[1] * _baseRotation;
//
//             //this controsl the movement of S3 about the camera and resets the particle
//             //information before being passed to SetParticles at the end of Update
//             // for (int index = 0; index < _dataPlot.points.Length; index++)
//             // {
//             //     var p = _dataPlot.points[index];
//             //
//             //     var rot = _rotations[0] * p.v * _rotations[1];
//             //
//             //     _points[index].position = rot.StereographicProjection();
//             //     _points[index].size = p.Size * ParticleSize;
//             //     _points[index].color = p.Color;
//             // }
//         }
//         else
//         {
//             var mouseP = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
//                 Input.mousePosition.y, Camera.main.nearClipPlane));
//
//             var mouseVec = mouseP - _camT.position;
//             mouseVec.Normalize();
//
//             float minDistance = 10000f;
//             int curPoint = -1;
//             for (int index = 0; index < _points.Length; index++)
//             {
//                 var p = _points[index];
//
//                 var _pVec = new Vector3(p.v[0], p.v[1], p.v[2]);
//                 var newVec = _pVec - _camT.position;
//
//                 var dot = Vector3.Dot(mouseVec, newVec);
//                 var magSquared = newVec.sqrMagnitude;
//
//                 //needs to be resolved
//                 var r = _radius; //* p.size;
//
//                 var radSquared = r * r;
//
//                 if (dot * dot >= ((magSquared * magSquared) / (magSquared + radSquared)) && dot > 0)
//                 {
//                     if (curPoint == -1)
//                     {
//                         minDistance = Vector3.Distance(_camT.position, _pVec);
//                         curPoint = index;
//                     }
//                     else
//                     {
//                         float newDistance = Vector3.Distance(_camT.position, _pVec);
//                         if (newDistance < minDistance)
//                         {
//                             minDistance = newDistance;
//                             curPoint = index;
//                         }
//                     }
//                 }
//             }
//
//             if (curPoint != -1)
//             {
//               //turns highlighted point white... needs to be resolved
//               //  _points[curPoint].color = Color.white;
//               //  _infoText.text = Info(curPoint);
//             }
//         }
//     }
//
// //     private string Info(int index)
// //     {
// //         StringBuilder sb = new StringBuilder();
// //         var particle = _dataPlot.points[index];
// //
// //         for (int i = 0; i < _dataPlot.attributes.Length; ++i)
// //         {
// //             var prop = _dataPlot.points.a[i];
// //
// //             sb.Append(prop.Name + ": ");
// //
// //             if(prop.Type < 3)
// //                 sb.Append(particle.Props[i].ToString() + "\n");
// //             else
// //             {
// //                 sb.Append(_dataPlot.Enums[prop.Type - 3].values[TakeOnlyNumbers(particle.Props[i].ToString())] + "\n");
// //             }
// //         }
// //         return sb.ToString();
// //     }
// //
// //     private int TakeOnlyNumbers(string s)
// //     {
// //         StringBuilder sb= new StringBuilder();
// //
// //         foreach (char c in s.ToCharArray())
// //         {
// //             if ((int) c >= 48 && (int) c <= 57)
// //                 sb.Append(c);
// //         }
// //
// //         return int.Parse(sb.ToString());
    }
}
