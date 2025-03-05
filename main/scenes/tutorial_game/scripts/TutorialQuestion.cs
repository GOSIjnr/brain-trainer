using Godot;
using Godot.Collections;

namespace GOSIjnr;

[GlobalClass]
public partial class TutorialQuestion : Question
{
	[Export] public Data.Subjects QuestionType { get; private set; }
	[Export] public Array<TutorialOption> QuestionOptions { get; private set; } = [];
}
