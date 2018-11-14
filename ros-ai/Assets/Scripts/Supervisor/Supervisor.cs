using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Htw.SelfDriving.GAN;
using Htw.SelfDriving.GAN.Gym;

namespace Htw.SelfDriving
{
    public class Supervisor : MonoBehaviour
    {
        public GameObject prefab; //Get Set
        public Transform target;
        public Transform spawn;
        public int inputs;
        public int outputs;
        public int hiddenLayer;
        public int neurons;
        public int population;
        public float surivingRate;
        public float mutationRate;
        public float perpetuationRate;

        private NeuralNetwork network { get; set; }
        private Species species { get; set; }
        private Zoo zoo { get; set; }

        private List<Vector3> lastAgentPosition;
        private List<Vector3> lastAgentTimePosition;
        private List<float> lastAgentTime;
        private List<float> currentAgentDistance;
        private bool setup = true;

        public void Start()
        {
            //IN MODEL
            this.network = new NeuralNetwork(inputs, outputs, hiddenLayer, neurons);
            this.species = Species.Random(network.Weights.Count, population);
            this.zoo = new Zoo(prefab, species, network);
        }

        public void Update()
        {
            if(setup)
            {
                setup = false;
                Setup();
                return;
            }

            if(IsFinished())
                Next();
            else
                Supervise();
        }

        public void Setup()
        {
            Debug.Log("Setup for Generation: " + species.Generation);

            lastAgentPosition = new List<Vector3>();
            lastAgentTimePosition = new List<Vector3>();
            lastAgentTime = new List<float>();
            currentAgentDistance = new List<float>();

            foreach(Agent agent in zoo.Agents)
            {
                agent.Reset();

                Rigidbody r = agent.GetComponent<Rigidbody>();
                r.position = spawn.position;
                r.rotation = spawn.rotation;
                lastAgentPosition.Add(r.position);
                lastAgentTimePosition.Add(r.position);
                lastAgentTime.Add(Time.time);
                currentAgentDistance.Add(0f);
            }
        }

        public void Supervise()
        {
            for(int i = 0; i < zoo.Agents.Count; ++i)
            {
                Agent agent = zoo.Agents[i];

                if(!agent.Alive)
                    continue;

                Rigidbody r = agent.GetComponent<Rigidbody>();

                if(Time.time > (lastAgentTime[i] + 5f))
                {
                    if(Vector3.Distance(lastAgentTimePosition[i], r.position) < 1f)
                    {
                        agent.Alive = false;
                        continue;
                    }

                    lastAgentTime[i] = Time.time;
                    lastAgentTimePosition[i] = r.position;
                }

                if((r.position - target.position).sqrMagnitude < 0.1f)
                {
                    agent.Alive = false;
                    agent.Reward(1000f);
                    continue;
                }

                currentAgentDistance[i] += Vector3.Distance(lastAgentPosition[i], r.position);
                lastAgentPosition[i] = r.position;
            }
        }

        public void Next()
        {
            for(int i = 0; i < zoo.Agents.Count; ++i)
            {
                Agent agent = zoo.Agents[i];
                Rigidbody r = agent.GetComponent<Rigidbody>();

                agent.Reward(currentAgentDistance[i]);
                agent.Punish(Vector3.Distance(target.position, r.position));
            }

            zoo.Next();

            Setup();
        }

        public bool IsFinished()
        {
            foreach(Agent agent in zoo.Agents)
                if(agent.Alive)
                    return false;

            return true;
        }

    }
}
