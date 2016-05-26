using Mapdo;

namespace Mapdo.iOS
{
	public class CustomAnnotation : MapAnnotation<ExtendedPin> 
	{
		public override string Title { get { return Model.Name; } }
		public override string Subtitle { get { return Model.Details; } }

        public CustomAnnotation(ExtendedPin model) : base(model) { }
	}
}
