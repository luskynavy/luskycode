﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FindCompressableJpegWinforms
{
	public static class ProgressBarExtension
	{
		/// <summary>
		/// Sets the progress bar value, without using 'Windows Aero' animation.
		/// This is to work around a known WinForms issue where the progress bar
		/// is slow to update.
		/// </summary>
		public static void SetProgressNoAnimation(this ProgressBar pb, int value)
		{
			//Only change progress if different than actual value (progress animation take a lot of time)
			if (pb.Value != value)
			{
				// To get around the progressive animation, we need to move the
				// progress bar backwards.
				if (value == pb.Maximum)
				{
					// Special case as value can't be set greater than Maximum.
					pb.Maximum = value + 1;     // Temporarily Increase Maximum
					pb.Value = value + 1;       // Move past
					pb.Maximum = value;         // Reset maximum
				}
				else
				{
					pb.Value = value + 1;       // Move past
				}
				pb.Value = value;               // Move to correct value
			}
		}
	}
}