using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEsperSample
{
    class TotalEvent
    {
        public string Month { get; set; }
        public int Day { get; set; }
        public string Time { get; set; }
        public string Ids { get; set; }
        public string Cheetah { get; set; }
        public string Numbers { get; set; }
        public string Info { get; set; }
        public string HttpInspect { get; set; }
        public string Message { get; set; }
        public string Periority { get; set; }
        public string TCP { get; set; }
        public string SourceIP { get; set; }
        public string Way { get; set; }
        public string DestinationIP { get; set; }
        public string Classification { get; set; }
        public string Characters { get; set; }

        public override string ToString()
        {
            return string.Format("Month: {0}, Day: {1}, Time: {2}, ids: {3}, cheetah: {4}, numbers: {5}, info: {6}, http_inspect: {7} message: {8},periority: {9},TCP: {10}, SourceIP: {11}, way: {12}, DestinationIP: {13}, classification: {14}, characters: {15}", Month, Day, Time, Ids, Cheetah, Numbers, Info, HttpInspect, Message, Periority, TCP, SourceIP, Way, DestinationIP, Classification, Characters);
        }
    }
}
