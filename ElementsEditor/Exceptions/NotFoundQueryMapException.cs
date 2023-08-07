using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElementsEditor
{

	[Serializable]
	public class NotFoundQueryMapException : Exception
	{
		public NotFoundQueryMapException() { }
		public NotFoundQueryMapException(string message) : base(message) { }
		public NotFoundQueryMapException(string message, Exception inner) : base(message, inner) { }
		protected NotFoundQueryMapException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
