using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;

namespace Assets.Script
{
    internal class WeightRandomClass<T>
    {
        public Dictionary<T, float> DictionaryValuesWithWeight { get; private set; }

        public WeightRandomClass()
        {
            DictionaryValuesWithWeight= new Dictionary<T, float>();
        }

        public WeightRandomClass(T[] values, float totalWeights) : this()
        {
            foreach (var value in values)
                DictionaryValuesWithWeight.Add(value, totalWeights / values.Length);
        }

        public WeightRandomClass(T[] values, float[] weitghts) : this()
        {
            for(int index = 0; index < values.Length; index++)
            {
                if (index < weitghts.Length)
                    DictionaryValuesWithWeight.Add(values[index], weitghts[index]);
                else
                    DictionaryValuesWithWeight.Add(values[index], 0);
            }
        }

        public void ChangeValueWeight(T value, float weight) 
        {
            if (DictionaryValuesWithWeight.ContainsKey(value) == false)
                DictionaryValuesWithWeight.Add(value, weight);
            else
                DictionaryValuesWithWeight[value] += weight;

            if (DictionaryValuesWithWeight[value] < 0) DictionaryValuesWithWeight[value] = 0;
        }

        public void SetValue(T value, float weight)
        {
            if (DictionaryValuesWithWeight.ContainsKey(value) == false)
                DictionaryValuesWithWeight.Add(value, weight);
            else
                DictionaryValuesWithWeight[value] = weight;

            if (DictionaryValuesWithWeight[value] < 0) DictionaryValuesWithWeight[value] = 0;
        }

        public void RemoveValue(T value)
        {
            if (DictionaryValuesWithWeight.ContainsKey(value))
                DictionaryValuesWithWeight.Remove(value);
        }

        public T GetRandomValue()
        {
            float totalWeith = 0;

            foreach(var keyValuePair in DictionaryValuesWithWeight)
            {
                totalWeith += keyValuePair.Value;
            }

            float rnd = UnityEngine.Random.Range(0f, totalWeith - 0.000001f);

            foreach (var keyValuePair in DictionaryValuesWithWeight)
            {
                rnd -= keyValuePair.Value;

                if (rnd < 0) return keyValuePair.Key;
            }

            //
            //if some shit happens
            //
            foreach (var keyValuePair in DictionaryValuesWithWeight)
            {
                if (keyValuePair.Value > 0) return keyValuePair.Key;
            }

            throw new Exception("FAILED CONFIGURATION");
        }

    }
}
