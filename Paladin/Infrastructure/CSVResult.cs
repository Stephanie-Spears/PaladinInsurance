﻿using System.Collections;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Paladin.Infrastructure
{
	public class CSVResult : FileResult
	{
		private IEnumerable _data;

		public CSVResult(IEnumerable data, string fileName) : base("text/csv")
		{
			_data = data;
			FileDownloadName = fileName;
		}

		protected override void WriteFile(HttpResponseBase response)
		{
			var builder = new StringBuilder();
			var stringWriter = new StringWriter(builder);

			foreach (var item in _data)
			{
				var properties = item.GetType().GetProperties();
				foreach (var prop in properties)
				{
					stringWriter.Write(GetValue(item, prop.Name));
					stringWriter.Write(", ");
				}
				stringWriter.WriteLine();
			}

			response.Write(builder);
		}

		public static string GetValue(object item, string propName)
		{
			return item.GetType().GetProperty(propName).GetValue(item, null).ToString() ?? "";
		}
	}
}

/* Extension Point 1
 * Extension Point: Action Results
 * Benefit: Add efficiency to response handling, improve Action Method code
 * (Improving Application Responses with Custom Action Results)
 */