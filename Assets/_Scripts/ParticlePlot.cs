using System;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

public class ParticlePlot : MonoBehaviour
{
    private const float _radius = .4f;
    private float _curSize;

    private object[] _points;

    private Quaternion _baseRotation;
    private Vector3 _inV;
    private Quaternion[] _rotations;

    public string defaultTPZ; //this value is assigned in the Unity Editor window under the
                              // ParticlePlot property of Particles gameObject


    private DataPlot _dataPlot;

    private Transform _camT;

    float ParticleSize = .3f;

    [SerializeField] private Slider _sizeSlider;
    [SerializeField] private InputField _xPos, _yPos, _zPos;
    [SerializeField] public Text _infoText;

    public float RotationSpeed = .05f;

    //these are the public variables for choosing mesh and material for
    //the mesh System
    public Mesh sphereMesh;
    public Material sphereMaterial;
    public GameObject[] respawn;
    public GameObject meshParent;


    public void Start()
    {
      if(File.Exists(defaultTPZ))
      {
        CreateSpheres(defaultTPZ);
      }

        _xPos.text = "0";
        _yPos.text = "0";
        _zPos.text = "0";
    }

//this receives a path from SelectFile onclick GUI and passes it to LoadFromFile in LoadJson
    public void CreateSpheres(string filePath)
    {
        _infoText.text = "Loading...";
        _dataPlot = LoadJson.Instance.LoadFromFile(filePath);

        int lengthIndex = Array.IndexOf(_dataPlot.attributes,"length");

        int i=0;
        foreach (var p in _dataPlot.points)
        {
            double L = Convert.ToDouble(p.a[lengthIndex]);
            float R = (float)(Math.Pow(L,-1.2) * _sizeSlider.value);
            var normal = new Vector4(p.v[0],p.v[1],p.v[2],p.v[3]);
            normal.Normalize();

            var meshParticle = new GameObject();
            meshParticle.name = String.Format("Sphere {0}",i);

            meshParticle.transform.SetParent(meshParent.transform);
            meshParticle.transform.position = normal.StereographicProjection();
            meshParticle.AddComponent<MeshFilter>().mesh = sphereMesh;
            meshParticle.AddComponent<MeshRenderer>().material = sphereMaterial;
            meshParticle.transform.localScale = new Vector3(R,R,R);
            meshParticle.GetComponent<Renderer>().material.SetColor("_TintColor", Color.white);
            i++;
        }
        _infoText.text = "";
        combineMeshes(meshParent);
    }

    public void combineMeshes(GameObject obj)
  {
    //Zero transformation is needed because of localToWorldMatrix transform
    Vector3 position = obj.transform.position;
    obj.transform.position = Vector3.zero;

    //whatever man
    MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
    CombineInstance[] combine = new CombineInstance[meshFilters.Length];
    int i = 0;
    while (i < meshFilters.Length) {
        combine[i].mesh = meshFilters[i].sharedMesh;
        combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        meshFilters[i].gameObject.SetActive(false);
        i++;
    }
    obj.transform.GetComponent<MeshFilter>().mesh = new Mesh();
    obj.transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true, true);
    obj.transform.gameObject.SetActive(true);

    //Reset position
    obj.transform.position = position;

    //Adds collider to mesh
    obj.AddComponent<MeshCollider> ();
  }

    public void Update()
    {
           // TODO: Make new update code

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
