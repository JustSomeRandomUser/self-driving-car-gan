using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingGround : MonoBehaviour
{
    [System.Serializable]
    public enum Connection { Top, Bottom, Left, Right }

    [System.Serializable]
    public class Chunk
    {
        [SerializeField]
        private GameObject model;
        public GameObject Model {
            get { return model; }
            set { model = value; }
        }

        [SerializeField]
        private Connection connector;
        public Connection Connector {
            get { return connector; }
            set { connector = value; }
        }

        public Quaternion rotation { get; set; }
        public Vector3 position { get; set; }

        public void Reset()
        {
            model.transform.position = position;
            model.transform.rotation = rotation;
            model.SetActive(false);
        }
    }

    [SerializeField]
    private Chunk[] chunks;
    public Chunk[] Chunks {
        get { return chunks; }
        set { chunks = value; }
    }
}
