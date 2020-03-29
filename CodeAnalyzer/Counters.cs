using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeAnalyzer
{
    class Counters
    {
        private Dictionary<string, List<Counter>> docTotals = new Dictionary<string, List<Counter>>();
        private List<Counter> totals = new List<Counter>();

        internal void AddTypeReferences(string docName, string name, int count)
        {
            GetOrCreateDocCounter(docName, name).TypeRefs += count;
            GetOrCreateTotalsCounter(name).TypeRefs += count;
        }

        internal void AddFieldReferences(string docName, string name, int count)
        {
            GetOrCreateDocCounter(docName, name).FieldRefs += count;
            GetOrCreateTotalsCounter(name).FieldRefs += count;
        }

        private Counter GetOrCreateDocCounter(string docName, string name)
        {
            if (!docTotals.TryGetValue(docName, out List<Counter> docCounters))
            {
                docCounters = new List<Counter>();
                docTotals.Add(docName, docCounters);
            }
            var docCounter = docCounters.FirstOrDefault(o => o.Name == name);
            if (docCounter == null)
            {
                docCounter = new Counter() { Name = name };
                docCounters.Add(docCounter);
            }
            return docCounter;
        }

        private Counter GetOrCreateTotalsCounter(string name)
        {
            var counter = totals.FirstOrDefault(o => o.Name == name);
            if (counter == null)
            {
                counter = new Counter() { Name = name };
                totals.Add(counter);
            }
            return counter;
        }

        internal IEnumerable<Counter> GetCounters(string docName)
        {
            docTotals.TryGetValue(docName, out List<Counter> docCounters);
            return docCounters ?? new List<Counter>();
        }

        internal IEnumerable<Counter> GetTotals()
        {
            return totals;
        }
    }

    class Counter
    {
        public string Name { get; set; }
        public int TypeRefs { get; set; }
        public int FieldRefs { get; set; }
    }
}
