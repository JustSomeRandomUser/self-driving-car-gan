using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Htw.SelfDriving.GAN
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; set; }
        public int Inputs => Layers[0].Neurons[0].Weights.Count - 1;
        public int Outputs => Layers[Layers.Count - 1].Neurons.Count;
        public int Hidden => Layers.Count - 1;
        public int Neurons => Layers[0].Neurons.Count;
        public List<float> Weights
        {
            get
            {
                List<float> weights = new List<float>();

                foreach(Layer layer in this.Layers)
                    foreach(float weight in layer.Weights)
                        weights.Add(weight);

                return weights;
            }

            set
            {
                int index = 0;

                foreach(Layer layer in this.Layers)
                {
                    layer.Weights = value.GetRange(index, layer.Weights.Count);
                    index += layer.Weights.Count;
                }
            }
        }

        public NeuralNetwork()
        {
            this.Layers = null;
        }

        public NeuralNetwork(int inputs, int outputs, int hidden, int neurons) : this()
        {
            this.Layers = new List<Layer>();

            // Input Layer to Hidden Layer
            this.Layers.Add(new Layer(neurons, inputs));

            // Hidden Layer (fully connected)
            for(int i = 0; i < (hidden - 1); ++i)
                this.Layers.Add(new Layer(neurons, neurons));

            // Output Layer
            this.Layers.Add(new Layer(outputs, neurons));
        }

        public List<float> Evaluate(List<float> input)
        {
            List<float> output = input;

            foreach(Layer layer in Layers)
                output = layer.Evaluate(output);

            return output;
        }

        public NeuralNetwork Copy()
        {
            return new NeuralNetwork(this.Inputs, this.Outputs, this.Hidden, this.Neurons);
        }
    }
}
