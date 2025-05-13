using System.Collections.Generic;

namespace Project_9.Parsers;

public class WingConfig
{
	public string Type { get; set; }
	public string Name { get; set; }
	public double RootChord { get; set; }
	public double Span { get; set; }
	public double IncidenceAngle { get; set; }
	public string RootAirfoilPath { get; set; }
	public string TipAirfoilPath { get; set; }
	public double Sweep { get; set; }
	public double TipExclusion { get; set; }
	public double TaperRatio { get; set; }
	public int RibCount { get; set; }
	public List<SparConfig> Spars { get; set; }
}