using System.Collections.Generic;

namespace PDD.Models
{
    internal class SearchBlock
    {
        public List<Rule> RulesFound { get; set; }
        public List<Sign> SignsFound { get; set; }
        public List<Marking> MarkingsFound { get; set; }
        public List<Malfunction> MalfunctionsFound { get; set; }
        public List<TrafficLight> TrafficLightsFound { get; set; }
        public List<Pointsman> PointsmanFound { get; set; }
        public List<Penalty> PenaltiesFound { get; set; }
    }
}
