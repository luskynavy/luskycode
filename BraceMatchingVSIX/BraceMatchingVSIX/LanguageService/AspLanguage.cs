using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.Package;
using Microsoft.VisualStudio.TextManager.Interop;
using System.Runtime.InteropServices;

namespace BraceMatchingVSIX
{
	public class AspLanguage : LanguageService
	{
		public AspLanguage()
		{

		}
		public override string Name => throw new NotImplementedException();

		public override string GetFormatFilterList()
		{
			throw new NotImplementedException();
		}

		public override LanguagePreferences GetLanguagePreferences()
		{
			throw new NotImplementedException();
		}

		public override IScanner GetScanner(IVsTextLines buffer)
		{
			throw new NotImplementedException();
		}

		public override AuthoringScope ParseSource(ParseRequest req)
		{
			throw new NotImplementedException();
		}

		/*public override TypeAndMemberDropdownBars CreateDropDownHelper(IVsTextView forView)
		{
			return new DropDownTocBars(this/*, forView*//*);
			//return base.CreateDropDownHelper(forView);
		}*/
	}
}
