using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Multi_View_Data_Visualization_Tool
{
    class Apriori
    {
        public List<List<string>> transactions { get; private set; }
        public int MinimumSupportCount { get; private set; }        
        public Dictionary<string, int> FrequentItemSets { get; private set; }
        public Dictionary<string, double> ConfidenceItemSets { get; private set; }
        public Dictionary<string, int> FirstFrequent { get; private set; }

        
        public Apriori(List<List<string>> transactions)
        {
            if (transactions == null)
                throw new ArgumentException("Parameter cannot be null", "original");
            this.transactions = transactions;
            MinimumSupportCount = 1;            
            FrequentItemSets = new Dictionary<string, int>();
            ConfidenceItemSets = new Dictionary<string, double>();
            FirstFrequent = ExtractSupported(FirstCandidates(transactions));
        }

        public void SetUp(int minimumSupportCount)
        {
            FrequentItemSets = new Dictionary<string, int>();
            ConfidenceItemSets = new Dictionary<string, double>();
            MinimumSupportCount = minimumSupportCount;
            FirstFrequent = ExtractSupported(FirstCandidates(transactions));
        }

        public void FindFrequent()
        {            
            Dictionary<string, int> tempCandidates = new Dictionary<string, int>(FirstFrequent);

            for(int i =0; i<FirstFrequent.Count - 1; i++)
            {
                tempCandidates = ExtractSupported(NextCandidates(tempCandidates.Keys.ToList(), FirstFrequent.Keys.ToList()));
                foreach (var key in tempCandidates.Keys)
                {                    
                    FrequentItemSets.Add(key, tempCandidates[key]);
                }
            }
        }

        public void GetConfidence(int minimumConfidence)
        {
            ConfidenceItemSets = new Dictionary<string, double>();

            List<List<string>> result;
            List<string> set;

            foreach (var key in FrequentItemSets.Keys)
            {
                result = new List<List<string>>();
                set = key.Split(',').ToList();

                GetCombination(set, result);

                ConfidenceItemSets = ConfidenceItemSets.Concat(GetConfidenceList(result, minimumConfidence, key, FrequentItemSets[key])).GroupBy(d => d.Key).ToDictionary(d => d.Key, d => d.First().Value);
            }
        } 

        private Dictionary<string, int> FirstCandidates(List<List<string>> trans)
        {   
            Dictionary<string, int> firstCandidates = new Dictionary<string, int>();

            foreach (var transaction in trans)
            {
                foreach (var item in transaction)
                {
                    if (firstCandidates.Keys.Contains(item))
                    {
                        firstCandidates[item] += 1;
                        continue;
                    }
                    firstCandidates.Add(item, 1);
                }
            }
            return firstCandidates;
        }
        
        private Dictionary<string, int> NextCandidates(List<string> previousCandidates, List<string> firstCandidates)
        {            
            List<string> tempList = previousCandidates;
            Dictionary<string, int> nextCandidates = CreateCandidates(previousCandidates, firstCandidates);

            tempList = nextCandidates.Keys.ToList();

            foreach(var key in tempList)
            {
                List<string> pruneList = PruneElements(key);

                if (NeedToBePrune(pruneList, previousCandidates))
                {
                    nextCandidates.Remove(key);
                }
            }            
            CountCandidates(nextCandidates);
            return nextCandidates;
        }

        private Dictionary<string, int> CreateCandidates(List<string> previousCandidates, List<string> firstCandidates)
        {
            List<string> tempList = new List<string>(previousCandidates);
            Dictionary<string, int> nextCandidates = new Dictionary<string, int>();

            while (tempList.Count != 0)
            {
                for (int i = 1; i < firstCandidates.Count; i++)
                {
                    if (IsContainElement(tempList.ElementAt(0), firstCandidates.ElementAt(i)) ||
                        IsContainElement(tempList.ElementAt(0) + "," + firstCandidates.ElementAt(i), nextCandidates.Keys.ToList()))
                    {
                        continue;
                    }
                    nextCandidates.Add(tempList.ElementAt(0) + "," + firstCandidates.ElementAt(i), 0);
                }
                tempList.RemoveAt(0);
            }                        
            return nextCandidates;
        }


        private Dictionary<string, int> ExtractSupported(Dictionary<string, int> candidates)
        {
            List<string> notSupported = new List<string>();

            foreach (var item in candidates)
            {
                if (item.Value < MinimumSupportCount)
                {
                    notSupported.Add(item.Key);
                }
            }

            foreach (var key in notSupported)
            {
                candidates.Remove(key);
            }
            return candidates;
        }

        private bool IsContainElement(string firstElement, string secondElement)
        {
            if (firstElement.Split(',').Contains(secondElement))
            {
                return true;
            }
            return false;
        }

        private bool IsContainElement(string firstElement, List<string> secondElement)
        {   
            foreach (var item in secondElement)
            {
                if (!firstElement.Split(',').ToList().Except(item.Split(',').ToList()).Any())
                {
                    return true;
                }
            }
            return false;
        }       

        private void CountCandidates(Dictionary<string, int> candidates)
        {
            foreach (var key in candidates.Keys.ToList())
            {
                List<string> tempSplit = key.Split(',').ToList();

                foreach (List<string> transaction in transactions)
                {
                    if (tempSplit.Except(transaction).Any())
                    {
                        continue;
                    }
                    candidates[key] += 1;
                }
            }
        }

        private bool NeedToBePrune(List<string> firstElement, List<string> secondElement)
        {
            foreach (var item in firstElement)
            {
                int counter = 0;
                List<string> tempElements = item.Split(',').ToList();

                foreach (var element in secondElement)
                {
                    if (!tempElements.Except(element.Split(',').ToList()).Any())
                    {
                        counter += 1;
                    }
                }
                if (counter == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private List<string> PruneElements(string item)
        {
            List<string> elements = new List<string>();
            List<string> itemList = item.Split(',').ToList();

            for (int i = 0; i < itemList.Count; i++)
            {
                List<string> temp = new List<string>(itemList);
                temp.RemoveAt(i);

                StringBuilder tempString = new StringBuilder(temp[0]);

                for (int j = 1; j < temp.Count; j++)
                {
                    tempString.Append("," + temp[j]);
                }
                elements.Add(tempString.ToString());
            }
            return elements;
        }
        
        private void GetCombination(List<string> set, List<List<string>> result)
        {
            for (int i = 0; i < set.Count; i++)
            {
                string str = set.Where((s, index) => index != i).ToString();

                List<string> temp = new List<string>(set.Where((s, index) => index != i));

                if (temp.Count > 0 && !result.Where(l => l.Count == temp.Count).Any(l => l.SequenceEqual(temp)))
                {
                    result.Add(temp);

                    GetCombination(temp, result);
                }
            }
        }

        private Dictionary<string, double> GetConfidenceList(List<List<string>> candidates, int minimumConfidence, string orginalCandidates, int counter)
        {
            Dictionary<string, double> confidenceList = new Dictionary<string, double>();
            int denominator;
            double confidence;
            List<string> tempList = orginalCandidates.Split(',').ToList();            
            
            foreach (var list in candidates)
            {
                if (list.Count == 1)
                {
                    denominator = FirstFrequent[list[0]];
                }
                else
                {
                    denominator = FindDenominator(list);
                }
                confidence = (double)counter / (double)denominator * 100;

                if (confidence>= minimumConfidence)
                {
                    confidenceList.Add(GetConfidenceString(tempList, list), confidence);
                }
            }

            return confidenceList;
        }
        
        private int FindDenominator(List<string> checkedList)
        {            
            foreach (var item in FrequentItemSets)
            {
                if (!checkedList.Except(item.Key.Split(',').ToList()).Any())
                {
                    return item.Value;                    
                }
            }
            return 0;
        }

        private string GetConfidenceString(List<string> fullString, List<string> candidate)
        {
            StringBuilder temp = new StringBuilder(candidate[0]);            

            for(int i = 1; i<candidate.Count; i++)
            {
                temp.Append("," + candidate[i]);
            }

            temp.Append(" -> ");
            
            foreach(var item in fullString.Except(candidate))
            {
                temp.Append(item + ",");
            }

            return temp.ToString();
        }
    }
}
