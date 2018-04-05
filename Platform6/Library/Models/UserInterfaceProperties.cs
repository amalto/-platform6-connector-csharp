using System.Collections.Generic;

namespace Library.Models {
	public class UserInterfaceProperties {
		/** Visibility of the entry menu. */
		public bool visible;
		/** Icon's name of the entry menu. */
		public string iconName;
		/** Position of the entry in the menu. */
		public int weight;
		/** Multi-language label for the entry menu (language: 'en-US', 'fr-FR'). */
		public Dictionary< string, string > label;
	}
}